import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { DashboardService } from '../../../core/services/dashboard/dashboard-service.service';
import { UpcomingInterviewsComponent } from './components/upcoming-interviews/upcoming-interviews.component';
import { CommonModule } from '@angular/common';
import { RecentActivityComponent } from './components/recent-activity/recent-activity.component';
import { DashboardStats } from '../../models/models/dashboard-model';

@Component({
  selector: 'app-dashboard',
  imports: [UpcomingInterviewsComponent,CommonModule,RecentActivityComponent],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit {
  private dashboardService = inject(DashboardService);

  stats = signal<DashboardStats | null>(null);
  isLoading = signal(true);

  // 2. Computed Signal for Cards Configuration
  statCards = computed(() => {
    const data = this.stats();
    if (!data) return [];

    return [
      { label: 'Total', value: data.totalApplications, icon: 'bi-briefcase-fill', colorClass: 'card-yellow', textClass: 'text-warning' },
      { label: 'Interviews', value: data.interviewing, icon: 'bi-calendar-check-fill', colorClass: 'card-blue', textClass: 'text-primary' },
      { label: 'Offers', value: data.offers, icon: 'bi-gift-fill', colorClass: 'card-purple', textClass: 'text-purple' },
      { label: 'Hired', value: data.hired, icon: 'bi-check-circle-fill', colorClass: 'card-green', textClass: 'text-success' },
      { label: 'Rejected', value: data.rejected, icon: 'bi-x-circle-fill', colorClass: 'card-red', textClass: 'text-danger' },
      { label: 'Declined', value: data.declined, icon: 'bi-dash-circle-fill', colorClass: 'card-orange', textClass: 'text-orange' },
    ];
  });

  ngOnInit() {
    this.loadStats();
  }

  loadStats() {
    this.dashboardService.getStats().subscribe({
      next: (data) => {
        this.stats.set(data);
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error('Error loading stats:', err);
        this.isLoading.set(false);
      }
    });
  }
}
