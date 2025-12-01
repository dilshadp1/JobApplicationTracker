export interface JobApplication {
  id: number;
  company: string;
  position: string;
  appliedDate: Date;
  currentStatus: string;
  jobUrl?: string;
  salaryExpectation?: number;
  notes?: string;
  interviewCount?: number;
  offerStatus?: string;
}
