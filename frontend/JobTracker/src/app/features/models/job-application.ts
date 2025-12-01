export interface JobApplication {
  id: number;
  company: string;
  position: string;
  appliedDate: Date;
  currentStatus: number;
  jobUrl?: string;
  salaryExpectation?: number;
  notes?: string;
  interviewCount?: number;
  offerStatus?: string;
}
