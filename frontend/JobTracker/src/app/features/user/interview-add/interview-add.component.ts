import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  ValidationErrors,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { InterviewService } from '../../../core/services/interview/interview.service';
import { JobApplicationService } from '../../../core/services/job-application/job-application.service';
import { OfferService } from '../../../core/services/offer/offer.service'; // Import OfferService
import { JobApplication } from '../../models/job-application';
import { Offer } from '../../models/offer'; // Import Offer Model
import {
  InterviewMode,
  InterviewStatus,
  InterviewUpdate,
} from '../../models/interview';
import { forkJoin } from 'rxjs'; // Import forkJoin

@Component({
  selector: 'app-interview-add',
  imports: [ReactiveFormsModule, RouterLink, CommonModule],
  templateUrl: './interview-add.component.html',
  styleUrl: './interview-add.component.scss',
})
export class InterviewAddComponent implements OnInit {
  isEditMode = false;
  interviewId: number | null = null;

  userJobs: JobApplication[] = [];
  allOffers: Offer[] = []; // Store offers to check constraints

  // Constraints
  selectedJobAppliedDate: string | null = null;
  minDate: string = ''; // Constraint: Applied Date
  maxDate: string = ''; // Constraint: Offer Date (if exists)

  originalStatus: InterviewStatus | null = null;
  eMode = InterviewMode;
  eStatus = InterviewStatus;

  interviewForm = new FormGroup(
    {
      applicationId: new FormControl<number | null>(null, Validators.required),
      roundName: new FormControl('', Validators.required),
      interviewDate: new FormControl('', Validators.required),
      mode: new FormControl<InterviewMode>(InterviewMode.Online, {
        nonNullable: true,
      }),
      status: new FormControl<InterviewStatus>(InterviewStatus.Scheduled, {
        nonNullable: true,
      }),
      locationUrl: new FormControl(''),
      feedback: new FormControl(''),
    },
    {
      validators: [
        this.dateConstraintValidator.bind(this),
        this.statusLogicValidator,
      ],
    }
  );

  constructor(
    private interviewService: InterviewService,
    private jobService: JobApplicationService,
    private offerService: OfferService, // Inject OfferService
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    // 1. Load Jobs AND Offers together
    forkJoin({
      jobs: this.jobService.getApplications(),
      offers: this.offerService.getOffers(),
    }).subscribe(({ jobs, offers }) => {
      this.allOffers = offers; // Save offers for lookup

      // Filter active jobs for the dropdown
      this.userJobs = jobs.filter(
        (j) =>
          j.currentStatus === 'Applied' ||
          j.currentStatus === 'Interviewing' ||
          j.id === this.interviewForm.value.applicationId
      );

      // 2. Initialize constraints based on current form value (if reloading/editing)
      this.updateConstraints(this.interviewForm.controls.applicationId.value);
    });

    // 3. Watch for changes in Job Selection
    this.interviewForm.controls.applicationId.valueChanges.subscribe(
      (selectedJobId) => {
        this.updateConstraints(selectedJobId);
      }
    );

    const id = this.route.snapshot.paramMap.get('id');
    const queryJobId = this.route.snapshot.queryParamMap.get('jobId');

    if (id) {
      this.isEditMode = true;
      this.interviewId = +id;
      this.loadInterview(this.interviewId);
    } else if (queryJobId) {
      this.interviewForm.patchValue({ applicationId: +queryJobId });
    }
  }

  updateConstraints(jobId: number | null) {
    // Reset defaults
    this.minDate = '';
    this.maxDate = '';
    this.selectedJobAppliedDate = null;

    if (jobId) {
      // Set Min Date (Applied Date)
      const selectedJob = this.userJobs.find((j) => j.id === jobId);
      if (selectedJob) {
        this.selectedJobAppliedDate = selectedJob.appliedDate
          .toString()
          .split('T')[0];
        this.minDate = this.selectedJobAppliedDate;
      }

      // Set Max Date (Offer Date) if an offer exists for this job
      const linkedOffer = this.allOffers.find((o) => o.applicationId === jobId);
      if (linkedOffer) {
        this.maxDate = linkedOffer.offerDate.toString().split('T')[0];
      }

      this.interviewForm.updateValueAndValidity(); // Trigger validation
    }
  }

  loadInterview(id: number) {
    this.interviewService.getInterview(id).subscribe((intv) => {
      let formattedDate = '';
      if (intv.interviewDate) {
        formattedDate = intv.interviewDate.toString().split('T')[0];
      }

      this.originalStatus = intv.status;

      // Handle case where the job might be closed but we are editing its interview
      this.jobService.getJob(intv.applicationId).subscribe((job) => {
        if (!this.userJobs.find((j) => j.id === job.id)) {
          this.userJobs.push(job);
        }

        // Re-run constraints now that we definitely have the job and offers loaded
        this.updateConstraints(job.id);

        this.interviewForm.patchValue({
          applicationId: intv.applicationId,
          roundName: intv.roundName,
          interviewDate: formattedDate,
          mode: intv.mode,
          status: intv.status,
          locationUrl: intv.locationUrl,
          feedback: intv.feedback,
        });
      });
    });
  }

  /**
   * Validation Logic
   */
  dateConstraintValidator(group: AbstractControl): ValidationErrors | null {
    const interviewDate = group.get('interviewDate')?.value;

    // Check 1: Cannot be before Applied Date
    if (interviewDate && this.selectedJobAppliedDate) {
      if (interviewDate < this.selectedJobAppliedDate) {
        return { appliedDateConflict: true };
      }
    }

    // Check 2: Cannot be after Offer Date
    if (interviewDate && this.maxDate) {
      if (interviewDate > this.maxDate) {
        return { offerDateConflict: true };
      }
    }

    return null;
  }

  statusLogicValidator(group: AbstractControl): ValidationErrors | null {
    const interviewDateStr = group.get('interviewDate')?.value;
    const status = group.get('status')?.value;

    if (interviewDateStr && status === InterviewStatus.Completed) {
      const today = new Date().toISOString().split('T')[0];
      if (interviewDateStr > today) {
        return { futureDateCompleted: true };
      }
    }
    return null;
  }

  get isTerminalState(): boolean {
    return (
      this.originalStatus === InterviewStatus.Completed ||
      this.originalStatus === InterviewStatus.Cancelled ||
      this.originalStatus === InterviewStatus.NoShow
    );
  }

  onSubmit() {
    if (this.interviewForm.invalid) return;
    const rawData = this.interviewForm.getRawValue();
    const reqData: InterviewUpdate = {
      applicationId: rawData.applicationId!,
      interviewDate: rawData.interviewDate!,
      roundName: rawData.roundName!.trim(),
      mode: rawData.mode,
      status: rawData.status,
      locationUrl: rawData.locationUrl || null,
      feedback: rawData.feedback || null,
    };

    const action$ =
      this.isEditMode && this.interviewId
        ? this.interviewService.updateInterview(this.interviewId, reqData)
        : this.interviewService.addInterview(reqData);

    action$.subscribe({
      next: () => this.router.navigate(['/user/interviews']),
      error: (err) => {
        let errorData =
          err.error?.error ||
          err.error?.Error ||
          err.error?.message ||
          err.error?.Message ||
          'An unexpected error occurred.';

        if (Array.isArray(errorData)) {
          errorData = errorData.join('\n');
        }
        else if (typeof errorData === 'object') {
          errorData = JSON.stringify(errorData);
        }
        alert(errorData);
      },
    });
  }
}
