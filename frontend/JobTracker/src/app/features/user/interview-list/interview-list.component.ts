import { Component, effect, OnInit, signal } from '@angular/core';
import { DatePipe, CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { InterviewService } from '../../../core/services/interview/interview.service';
import { Interview } from '../../models/interview';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-interview-list',
  imports: [DatePipe, RouterLink, CommonModule,FormsModule],
  templateUrl: './interview-list.component.html',
  styleUrl: './interview-list.component.scss',
})
export class InterviewListComponent {
  interviews = signal<Interview[]>([]);
  filterStatus = signal<string>('All');

  constructor(private interviewService: InterviewService) {
    effect(() => {
      this.loadInterviews();
    });
  }

  loadInterviews() {
    this.interviewService.getInterviews(this.filterStatus()).subscribe((data) => {
      this.interviews.set(data);
    });
  }

  onDelete(id: number) {
    if(confirm('Are you sure you want to cancel this interview?')) {
      this.interviewService.deleteInterview(id).subscribe(() => {
        this.loadInterviews();
      });
    }
  }
}
