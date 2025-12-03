import { RecentActivity } from "./recent-activity";
import { UpcomingInterview } from "./upcoming-interview";

export interface DashboardStats {
  totalApplications: number;
  applied:number;
  interviewing: number;
  offers: number;
  rejected: number;
  hired: number;
  declined: number;

  upcomingInterviews: UpcomingInterview[];
  recentActivities: RecentActivity[];

}