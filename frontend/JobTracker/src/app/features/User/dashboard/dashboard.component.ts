import { Component, inject, OnInit } from '@angular/core';
import { DashboardStats } from '../../../shared/models/dashboard-model';
import { Router } from '@angular/router';
import { DashboardService } from '../../../core/services/dashboard/dashboard-service.service';
import { UpcomingInterviewsComponent } from './components/upcoming-interviews/upcoming-interviews.component';

@Component({
  selector: 'app-dashboard',
  imports: [UpcomingInterviewsComponent],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit {
  stats: DashboardStats | null = null;
  
  private dashboardService = inject(DashboardService);
  private router = inject(Router);

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
