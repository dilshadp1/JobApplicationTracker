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
import { JobApplication } from '../../models/job-application';
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

  dateRangeValidator(control: AbstractControl): ValidationErrors | null {
    const start = control.get('offerDate')?.value;
    const end = control.get('deadline')?.value;

    if (start && end && new Date(end) < new Date(start)) {
      return { invalidDateRange: true }; // Error found
    }
    return null; // No error
  }

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
    { validators: this.dateRangeValidator }
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
      this.jobService.getJob(+queryJobId).subscribe({
        next: (job) => {
          this.selectedJobDisplay = `${job.company} - ${job.position}`;
        },
        error: () => {
          alert('Invalid Job ID');
          this.router.navigate(['/user/jobs']);
        },
      });
    } else {
      alert('Please select a job from your Job List to add an offer.');
      this.router.navigate(['/user/jobs']);
    }
  }

  loadOffer(id: number) {
    this.offerService.getOffer(id).subscribe((offer) => {
      this.selectedJobDisplay = `${offer.companyName} - ${offer.jobPosition}`;
      this.offerForm.patchValue({
        applicationId: offer.applicationId,
        salary: offer.salary,
        offerDate: new Date(offer.offerDate).toISOString().split('T')[0],
        deadline: new Date(offer.deadline).toISOString().split('T')[0],
        benefits: offer.benefits,
      });
      // this.offerForm.controls.applicationId.disable();
    });
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

    if (this.isEditMode && this.offerId) {
      this.offerService.updateOffer(this.offerId, reqData).subscribe(() => {
        this.router.navigate(['/user/offers']);
      });
    } else {
      this.offerService.addOffer(reqData).subscribe({
        next: () => this.router.navigate(['/user/offers']),
        error: (err) => alert(err.error?.Message || 'Failed to add offer'),
      });
    }
  }
}
