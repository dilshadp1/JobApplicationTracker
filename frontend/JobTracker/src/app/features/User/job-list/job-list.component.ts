import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { JobApplication } from '../../models/job-application';
import { JobApplicationService } from '../../../core/services/job-application/job-application.service';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-job-list',
  imports: [DatePipe],
  templateUrl: './job-list.component.html',
  styleUrl: './job-list.component.scss',
})
export class JobListComponent implements OnInit {
  constructor(private jobApplicationService: JobApplicationService) {}
  public applications: JobApplication[] = [];

  ngOnInit(): void {
    this.jobApplicationService.getApplications().subscribe((data) => {
      this.applications = data;
    });
  }
}
