import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { JobApplication } from '../../models/job-application';
import { JobApplicationService } from '../../../core/services/job-application/job-application.service';

@Component({
  selector: 'app-job-list',
  imports: [RouterLink],
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
