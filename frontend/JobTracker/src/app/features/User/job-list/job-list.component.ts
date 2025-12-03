import { Component, effect, signal } from '@angular/core';
import { JobApplication } from '../../models/job-application';
import { JobApplicationService } from '../../../core/services/job-application/job-application.service';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-job-list',
  imports: [DatePipe, CurrencyPipe, RouterLink,FormsModule],
  templateUrl: './job-list.component.html',
  styleUrl: './job-list.component.scss',
})
export class JobListComponent {
  applications = signal<JobApplication[]>([]);
  filterStatus = signal<string>('All');

  constructor(private jobApplicationService: JobApplicationService) {
    effect(() => {
      this.loadJobs();
    });
  }

  loadJobs() {
    this.jobApplicationService
      .getApplications(this.filterStatus())
      .subscribe((data) => {
        this.applications.set(data);
      });
  }

  onDelete(id: number) {
    if (confirm('Are you sure you want to delete this application?')) {
      this.jobApplicationService.deleteJob(id).subscribe(() => {
        this.loadJobs();
      });
    }
  }
}
