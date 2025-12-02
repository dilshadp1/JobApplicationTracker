import { Component, OnInit } from '@angular/core';
import { DatePipe, CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { InterviewService } from '../../../core/services/interview/interview.service';
import { Interview } from '../../models/interview';

@Component({
  selector: 'app-interview-list',
  imports: [DatePipe, RouterLink, CommonModule],
  templateUrl: './interview-list.component.html',
  styleUrl: './interview-list.component.scss',
})
export class InterviewListComponent implements OnInit {
  interviews: Interview[] = [];

  constructor(private interviewService: InterviewService) {}

  ngOnInit(): void {
    this.loadInterviews();
  }

  loadInterviews() {
    this.interviewService.getInterviews().subscribe((data) => {
      this.interviews = data;
    });
  }

  onDelete(id: number) {
    // if(confirm('Are you sure you want to cancel this interview?')) {
    this.interviewService.deleteInterview(id).subscribe(() => {
      this.loadInterviews();
    });
    // }
  }
}
