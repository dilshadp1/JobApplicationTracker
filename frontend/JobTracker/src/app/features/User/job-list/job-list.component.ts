import { Component, OnInit } from '@angular/core';
import { JobApplication } from '../../models/job-application';
import { JobApplicationService } from '../../../core/services/job-application/job-application.service';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-job-list',
  imports: [DatePipe, CurrencyPipe, RouterLink],
  templateUrl: './job-list.component.html',
  styleUrl: './job-list.component.scss',
})
export class JobListComponent implements OnInit {
  constructor(private jobApplicationService: JobApplicationService) {}
  public applications: JobApplication[] = [];

  loadJobs() {
    this.jobApplicationService.getApplications().subscribe((data) => {
      this.applications = data;
    });
  }
  ngOnInit(): void {
    this.loadJobs();
  }

  public onDelete(id: number) {
    this.jobApplicationService.deleteJob(id).subscribe(() => {
      this.loadJobs();
    });
  }
}
