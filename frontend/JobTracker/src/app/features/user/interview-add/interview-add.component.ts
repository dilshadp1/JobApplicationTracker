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
import { JobApplicationService } from '../../../core/services/job-application/job-application.service'; // Need this!
import { JobApplication } from '../../models/job-application';
import { InterviewUpdate } from '../../models/interview';

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

  interviewForm = new FormGroup({
    applicationId: new FormControl<number | null>(null, Validators.required),
    roundName: new FormControl('', Validators.required),
    interviewDate: new FormControl('', Validators.required),
    mode: new FormControl<number>(0),
    status: new FormControl<number>(0),
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
      this.userJobs = jobs;
    });

    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.interviewId = +id;
      this.loadInterview(this.interviewId);
    }
  }

  loadInterview(id: number) {
    this.interviewService.getInterview(id).subscribe((intv) => {
      const dateString = intv.interviewDate;
      let formattedDate = '';
      if (dateString) {
        const dateObj = new Date(dateString);
        dateObj.setMinutes(dateObj.getMinutes() - dateObj.getTimezoneOffset());
        formattedDate = dateObj.toISOString().slice(0, 16);
      }

      const modeMap: any = { Online: 0, Offline: 1, online: 0, offline: 1 };
      const statusMap: any = {
        Scheduled: 0,
        Completed: 1,
        Cancelled: 2,
        NoShow: 3,
        scheduled: 0,
        completed: 1,
        cancelled: 2,
        noshow: 3,
      };

      const modeValue =
        typeof intv.mode === 'number' ? intv.mode : modeMap[intv.mode] ?? 0;
      const statusValue =
        typeof intv.status === 'number'
          ? intv.status
          : statusMap[intv.status] ?? 0;

      this.interviewForm.patchValue({
        applicationId: intv.applicationId,
        roundName: intv.roundName,
        interviewDate: formattedDate,
        mode: modeMap[intv.mode] ?? 0,
        status: statusMap[intv.status] ?? 0,
        locationUrl: intv.locationUrl,
        feedback: intv.feedback,
      });
    });
  }

  onSubmit() {
    if (this.interviewForm.invalid) return;

    const rawData = this.interviewForm.value;

    const reqData: InterviewUpdate = {
      userId: 0,
      applicationId: rawData.applicationId!,
      interviewDate: rawData.interviewDate!,
      roundName: rawData.roundName!,
      mode: rawData.mode!,
      status: rawData.status!,
      locationUrl: rawData.locationUrl || null,
      feedback: rawData.feedback || null,
    };

    if (this.isEditMode && this.interviewId) {
      this.interviewService
        .updateInterview(this.interviewId, reqData)
        .subscribe(() => this.router.navigate(['/user/interviews']));
    } else {
      this.interviewService
        .addInterview(reqData)
        .subscribe(() => this.router.navigate(['/user/interviews']));
    }
  }
}
