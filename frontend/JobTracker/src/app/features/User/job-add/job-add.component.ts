import { Component, OnInit } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { JobApplicationService } from '../../../core/services/job-application/job-application.service';
import { InterviewService } from '../../../core/services/interview/interview.service';
import { OfferService } from '../../../core/services/offer/offer.service';
import { JobApplicationUpdate } from '../../models/job-application';
import { CommonModule } from '@angular/common';
import { forkJoin, map } from 'rxjs';

@Component({
  selector: 'app-job-add',
  imports: [ReactiveFormsModule, RouterLink, CommonModule],
  templateUrl: './job-add.component.html',
  styleUrl: './job-add.component.scss',
})
export class JobAddComponent implements OnInit {
  public maxDate: string = new Date().toISOString().split('T')[0];

  public restrictionDate: string | null = null;
  public restrictionReason: string = '';

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
    appliedDate: new FormControl(this.maxDate, {
      nonNullable: true,
      validators: [Validators.required],
    }),
    status: new FormControl<number>(0, { nonNullable: true }),
  });

  constructor(
    private jobApplicationService: JobApplicationService,
    private interviewService: InterviewService,
    private offerService: OfferService,
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
      const formattedDate = job.appliedDate.toString().split('T')[0];
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

      this.calculateDateConstraints(id);
    });
  }

  calculateDateConstraints(jobId: number) {
    forkJoin({
      interviews: this.interviewService.getInterviews(),
      offers: this.offerService.getOffers(),
    })
      .pipe(
        map((data) => {
          const jobInterviews = data.interviews.filter(
            (i) => i.applicationId === jobId
          );
          const jobOffers = data.offers.filter(
            (o) => o.applicationId === jobId
          );

          let earliestIntDate = '';
          if (jobInterviews.length > 0) {
            jobInterviews.sort((a, b) =>
              a.interviewDate > b.interviewDate ? 1 : -1
            );
            earliestIntDate = jobInterviews[0].interviewDate
              .toString()
              .split('T')[0];
          }

          let earliestOfferDate = '';
          if (jobOffers.length > 0) {
            earliestOfferDate = jobOffers[0].offerDate.toString().split('T')[0];
          }

          return { earliestIntDate, earliestOfferDate };
        })
      )
      .subscribe(({ earliestIntDate, earliestOfferDate }) => {
        const today = new Date().toISOString().split('T')[0];

        let limit = today;
        let reason = 'Cannot be in the future.';

        if (earliestIntDate && earliestIntDate < limit) {
          limit = earliestIntDate;
          reason = `Cannot be after the first interview (${earliestIntDate}).`;
        }

        if (earliestOfferDate && earliestOfferDate < limit) {
          limit = earliestOfferDate;
          reason = `Cannot be after the offer date (${earliestOfferDate}).`;
        }

        this.maxDate = limit;
        this.restrictionReason = reason;

        this.jobForm.controls.appliedDate.updateValueAndValidity();
      });
  }

  public onSubmit() {
    if (this.jobForm.invalid) return;

    const formValue = this.jobForm.getRawValue();

    if (formValue.appliedDate > this.maxDate) {
      alert(`Invalid Date: ${this.restrictionReason}`);
      return;
    }

    const requestData: JobApplicationUpdate = {
      company: formValue.company,
      position: formValue.position,
      jobUrl: formValue.jobUrl || null,
      salaryExpectation: formValue.salaryExpectation,
      notes: formValue.notes || null,
      appliedDate: formValue.appliedDate,
      status: formValue.status,
    };

    const action$ =
      this.isEditMode && this.jobId
        ? this.jobApplicationService.updateJob(this.jobId, requestData)
        : this.jobApplicationService.addJob(requestData);

    action$.subscribe({
      next: (res) => {
        const targetId =
          this.isEditMode && this.jobId
            ? this.jobId
            : typeof res === 'number'
            ? res
            : 0;
        this.handleRedirection(requestData.status, targetId, this.isEditMode);
      },
      error: (err) => {
        let errorData =
          err.error?.error ||
          err.error?.Error ||
          err.error?.message ||
          err.error?.Message ||
          'An unexpected error occurred.';

        if (Array.isArray(errorData)) {
          errorData = errorData.join('\n');
        } else if (typeof errorData === 'object') {
          errorData = JSON.stringify(errorData);
        }
        alert(errorData);
      },
    });
  }

  private handleRedirection(status: number, jobId: number, isUpdate: boolean) {
    const msgPrefix = isUpdate ? 'Job updated!' : 'Job added!';

    if (status === 1 && !this.hasInterviews) {
      if (confirm(`${msgPrefix} Do you want to schedule the interview now?`)) {
        this.router.navigate(['/user/interviews/add'], {
          queryParams: { jobId: jobId },
        });
        return;
      }
    }
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
