import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { UpcomingInterview } from '../../../../models/upcoming-interview';

@Component({
  selector: 'app-upcoming-interviews',
  imports: [CommonModule],
  templateUrl: './upcoming-interviews.component.html',
  styleUrl: './upcoming-interviews.component.scss',
})
export class UpcomingInterviewsComponent {
  @Input() interviews: UpcomingInterview[] = [];
}
