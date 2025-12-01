import { Component, inject, OnInit } from '@angular/core';
import { DashboardStats } from '../../../shared/models/dashboard-model';
import { Router } from '@angular/router';
import { DashboardService } from '../../../core/services/dashboard/dashboard-service.service';
import { UpcomingInterviewsComponent } from './components/upcoming-interviews/upcoming-interviews.component';
import { CommonModule } from '@angular/common';
import { RecentActivityComponent } from './components/recent-activity/recent-activity.component';

@Component({
  selector: 'app-dashboard',
  imports: [UpcomingInterviewsComponent,CommonModule,RecentActivityComponent],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit {
  stats: DashboardStats | null = null;
  

  getStatusColor(status: string): string {
    switch (status) {
      case 'Applied': return 'primary';      // Blue
      case 'Interviewing': return 'warning'; // Yellow
      case 'OfferReceived': return 'purple'; // Purple (Custom)
      case 'Hired': return 'success';        // Green
      case 'Rejected': return 'danger';      // Red
      case 'Declined': return 'orange';      // Orange (Custom)
      default: return 'secondary';
    }
  }

  private dashboardService = inject(DashboardService);

  ngOnInit() {
    this.loadStats();
  }

  loadStats() {
    this.dashboardService.getStats().subscribe({
      next: (data) => {
        this.stats = data;
        if (this.stats) {
            this.stats.hired = this.stats.hired || 0;
            this.stats.declined = this.stats.declined || 0;
        }
      },
      error: (err) => console.error('Error loading stats:', err)
    });
  }

}
