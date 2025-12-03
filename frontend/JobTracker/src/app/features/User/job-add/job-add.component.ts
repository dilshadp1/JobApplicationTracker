import { Component, OnInit } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { JobApplicationService } from '../../../core/services/job-application/job-application.service';
import { JobApplicationUpdate } from '../../models/job-application';

@Component({
  selector: 'app-job-add',
  imports: [ReactiveFormsModule],
  templateUrl: './job-add.component.html',
  styleUrl: './job-add.component.scss',
})
export class JobAddComponent implements OnInit {
  public maxDate: string = new Date().toISOString().split('T')[0];
  public isEditMode = false;
  public jobId: number | null = null;

  public hasInterviews = false;
  public hasOffer = false;

  jobForm = new FormGroup({
    company: new FormControl('', {
      nonNullable: true,
      validators: [Validators.required],
    }),
    position: new FormControl('', {
      nonNullable: true,
      validators: [Validators.required],
    }),
    jobUrl: new FormControl(''),
    salaryExpectation: new FormControl<number | null>(null),
    notes: new FormControl(''),
    appliedDate: new FormControl(this.maxDate, { nonNullable: true }),
    status: new FormControl<number>(0, { nonNullable: true }),
  });

  constructor(
    private jobApplicationService: JobApplicationService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    const idString = this.route.snapshot.paramMap.get('id');
    if (idString) {
      this.isEditMode = true;
      this.jobId = +idString;
      this.loadJobData(this.jobId);
    }
  }

  private mapStatusToEnum(statusString: string): number {
    const statusMap: { [key: string]: number } = {
      Applied: 0,
      Interviewing: 1,
      OfferReceived: 2,
      Hired: 3,
      Declined: 4,
      Rejected: 5,
    };
    return statusMap[statusString] ?? 0;
  }

  loadJobData(id: number) {
    this.jobApplicationService.getJob(id).subscribe((job) => {
      this.hasInterviews = (job.interviewCount || 0) > 0;
      this.hasOffer = job.offerStatus === 'Received';

      const isClosed =
        job.currentStatus === 'Rejected' || job.currentStatus === 'Declined';
      const formattedDate = new Date(job.appliedDate)
        .toISOString()
        .split('T')[0];
      const statusEnum = this.mapStatusToEnum(job.currentStatus);

      this.jobForm.patchValue({
        company: job.company,
        position: job.position,
        jobUrl: job.jobUrl || '',
        salaryExpectation: job.salaryExpectation,
        notes: job.notes || '',
        status: statusEnum,
        appliedDate: formattedDate,
      });

      if (isClosed) {
        this.jobForm.controls.status.disable();
      }
    });
  }

  public onSubmit() {
    if (this.jobForm.invalid) return;

    const formValue = this.jobForm.getRawValue();

    const requestData: JobApplicationUpdate = {
      company: formValue.company,
      position: formValue.position,
      jobUrl: formValue.jobUrl || null,
      salaryExpectation: formValue.salaryExpectation,
      notes: formValue.notes || null,
      appliedDate: formValue.appliedDate,
      status: formValue.status,
    };

    if (this.isEditMode && this.jobId) {
      this.jobApplicationService.updateJob(this.jobId, requestData).subscribe({
        next: () => {
          this.handleRedirection(requestData.status, this.jobId!, true);
        },
        error: (err) => alert(err.error?.Message || 'Failed to update job'),
      });
    } else {
      this.jobApplicationService.addJob(requestData).subscribe({
        next: (newJobId) => {
          this.handleRedirection(requestData.status, newJobId, false);
        },
        error: (err) => alert(err.error?.Message || 'Failed to add job'),
      });
    }
  }

  private handleRedirection(status: number, jobId: number, isUpdate: boolean) {
    const msgPrefix = isUpdate ? 'Job updated!' : 'Job added!';

    // Logic: Only ask if status is Interviewing AND no interviews exist yet
    if (status === 1 && !this.hasInterviews) {
      if (confirm(`${msgPrefix} Do you want to schedule the interview now?`)) {
        this.router.navigate(['/user/interviews/add'], {
          queryParams: { jobId: jobId },
        });
        return;
      }
    }

    // Logic: Only ask if status is Offer Received AND no offer exists yet
    if (status === 2 && !this.hasOffer) {
      if (
        confirm(`${msgPrefix} Do you want to record the offer details now?`)
      ) {
        this.router.navigate(['/user/offers/add'], {
          queryParams: { jobId: jobId },
        });
        return;
      }
    }

    this.router.navigate(['/user/jobs']);
  }
}
