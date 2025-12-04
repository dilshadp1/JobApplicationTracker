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
import { OfferService } from '../../../core/services/offer/offer.service';
import { JobApplicationService } from '../../../core/services/job-application/job-application.service';
import { OfferUpdate } from '../../models/offer';

@Component({
  selector: 'app-offer-add',
  imports: [ReactiveFormsModule, RouterLink, CommonModule],
  templateUrl: './offer-add.component.html',
  styleUrl: './offer-add.component.scss',
})
export class OfferAddComponent implements OnInit {
  isEditMode = false;
  offerId: number | null = null;
  selectedJobDisplay: string = 'Loading...';

  today: string = new Date().toISOString().split('T')[0];
  minDate: string = '';

  offerForm = new FormGroup(
    {
      applicationId: new FormControl<number | null>(null, Validators.required),
      salary: new FormControl<number | null>(null, [
        Validators.required,
        Validators.min(1),
      ]),
      offerDate: new FormControl('', Validators.required),
      deadline: new FormControl('', Validators.required),
      benefits: new FormControl(''),
    },
    {
      validators: [
        this.dateRangeValidator,
        this.appliedDateValidator.bind(this),
      ],
    }
  );

  constructor(
    private offerService: OfferService,
    private jobService: JobApplicationService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    const queryJobId = this.route.snapshot.queryParamMap.get('jobId');

    if (id) {
      this.isEditMode = true;
      this.offerId = +id;
      this.loadOffer(this.offerId);
    } else if (queryJobId) {
      this.offerForm.patchValue({ applicationId: +queryJobId });
      this.fetchJobDetails(+queryJobId);
    } else {
      alert('Please select a job from your Job List to add an offer.');
      this.router.navigate(['/user/jobs']);
    }
  }

  fetchJobDetails(jobId: number) {
    this.jobService.getJob(jobId).subscribe({
      next: (job) => {
        this.selectedJobDisplay = `${job.company} - ${job.position}`;
        // Fix: Direct Split
        this.minDate = job.appliedDate.toString().split('T')[0];
        this.offerForm.updateValueAndValidity();
      },
      error: () => {
        this.selectedJobDisplay = 'Job not found';
      },
    });
  }

  loadOffer(id: number) {
    this.offerService.getOffer(id).subscribe((offer) => {
      const safeOfferDate = offer.offerDate.toString().split('T')[0];
      const safeDeadline = offer.deadline.toString().split('T')[0];

      this.offerForm.patchValue({
        applicationId: offer.applicationId,
        salary: offer.salary,
        offerDate: safeOfferDate,
        deadline: safeDeadline,
        benefits: offer.benefits,
      });

      this.fetchJobDetails(offer.applicationId);
    });
  }

  dateRangeValidator(control: AbstractControl): ValidationErrors | null {
    const start = control.get('offerDate')?.value;
    const end = control.get('deadline')?.value;
    if (start && end && end < start) {
      return { invalidDateRange: true };
    }
    return null;
  }

  appliedDateValidator(control: AbstractControl): ValidationErrors | null {
    const offerDate = control.get('offerDate')?.value;
    if (offerDate && this.minDate && offerDate < this.minDate) {
      return { offerBeforeApplied: true };
    }
    return null;
  }

  onSubmit() {
    if (this.offerForm.invalid) return;

    const val = this.offerForm.getRawValue();
    const reqData: OfferUpdate = {
      applicationId: val.applicationId!,
      salary: val.salary!,
      offerDate: val.offerDate!,
      deadline: val.deadline!,
      benefits: val.benefits || null,
    };

    const action$ =
      this.isEditMode && this.offerId
        ? this.offerService.updateOffer(this.offerId, reqData)
        : this.offerService.addOffer(reqData);

    action$.subscribe({
      next: () => this.router.navigate(['/user/offers']),
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

  onDelete() {
    if (this.offerId && confirm('Are you sure?')) {
      this.offerService.deleteOffer(this.offerId).subscribe(() => {
        this.router.navigate(['/user/offers']);
      });
    }
  }
}
