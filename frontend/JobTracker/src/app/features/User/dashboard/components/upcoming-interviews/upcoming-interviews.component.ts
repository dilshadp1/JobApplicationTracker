import { Component, Input } from '@angular/core';
import { UpcomingInterview } from '../../../../../shared/models/upcoming-interview';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-upcoming-interviews',
  imports: [CommonModule,RouterLink],
  templateUrl: './upcoming-interviews.component.html',
  styleUrl: './upcoming-interviews.component.scss'
})
export class UpcomingInterviewsComponent {
  @Input() interviews: UpcomingInterview[] = [];
}
