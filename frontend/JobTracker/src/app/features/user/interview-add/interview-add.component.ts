import { Component, OnInit } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { InterviewService } from '../../../core/services/interview/interview.service';
import { JobApplicationService } from '../../../core/services/job-application/job-application.service';
import { JobApplication } from '../../models/job-application';
import {
  InterviewMode,
  InterviewStatus,
  InterviewUpdate,
} from '../../models/interview';

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

  minDate: string = '';

  originalStatus: InterviewStatus | null = null;

  eMode = InterviewMode;
  eStatus = InterviewStatus;

  interviewForm = new FormGroup({
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
  });

  constructor(
    private interviewService: InterviewService,
    private jobService: JobApplicationService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.jobService.getApplications().subscribe((jobs) => {
      this.userJobs = jobs.filter(
        (j) =>
          j.currentStatus === 'Applied' || j.currentStatus === 'Interviewing'
      );
    });

    this.interviewForm.controls.applicationId.valueChanges.subscribe(
      (selectedJobId) => {
        if (selectedJobId) {
          const selectedJob = this.userJobs.find((j) => j.id === selectedJobId);
          if (selectedJob) {
            this.minDate = new Date(selectedJob.appliedDate)
              .toISOString()
              .split('T')[0];
          }
        }
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

  loadInterview(id: number) {
    this.interviewService.getInterview(id).subscribe((intv) => {
      let formattedDate = '';
      if (intv.interviewDate) {
        const dateObj = new Date(intv.interviewDate);
        dateObj.setMinutes(dateObj.getMinutes() - dateObj.getTimezoneOffset());
        formattedDate = dateObj.toISOString().slice(0, 16);
      }

      this.originalStatus = intv.status;

      this.interviewForm.patchValue({
        applicationId: intv.applicationId,
        roundName: intv.roundName,
        interviewDate: formattedDate,
        mode: intv.mode,
        status: intv.status,
        locationUrl: intv.locationUrl,
        feedback: intv.feedback,
      });

      this.jobService.getJob(intv.applicationId).subscribe((job) => {
        this.minDate = new Date(job.appliedDate).toISOString().split('T')[0];
      });
    });
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

    if (this.isEditMode && this.interviewId) {
      this.interviewService
        .updateInterview(this.interviewId, reqData)
        .subscribe({
          next: () => this.router.navigate(['/user/interviews']),
          error: (err) => {
            alert(err.error?.Message || 'Failed to update interview.');
          },
        });
    } else {
      this.interviewService.addInterview(reqData).subscribe({
        next: () => this.router.navigate(['/user/interviews']),
        error: (err) => {
          alert(err.error?.Message || 'Failed to add interview.');
        },
      });
    }
  }
}
