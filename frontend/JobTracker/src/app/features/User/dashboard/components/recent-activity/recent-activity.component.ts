import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-recent-activity',
  imports: [CommonModule],
  templateUrl: './recent-activity.component.html',
  styleUrl: './recent-activity.component.scss'
})
export class RecentActivityComponent {
  @Input() activities: any[] = []; 

  getStatusColor(status: string): string {
    switch (status?.toLowerCase()) {
      case 'interviewing': return 'primary';
      case 'offer': return 'purple'; 
      case 'hired': return 'success';
      case 'rejected': return 'danger';
      case 'declined': return 'orange';
      default: return 'secondary';
    }
  }
}
