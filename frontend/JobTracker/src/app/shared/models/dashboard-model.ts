import { UpcomingInterview } from "./upcoming-interview";

export interface DashboardStats {
  totalApplications: number;
  interviewing: number;
  offers: number;
  rejected: number;
  hired: number;
  declined: number;

    upcomingInterviews: UpcomingInterview[];

}