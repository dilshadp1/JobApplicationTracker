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

export interface JobApplicationUpdate {
  id?: number;
  company: string;
  position: string;
  status: number;
  appliedDate: string;
  jobUrl: string | null;
  salaryExpectation: number | null;
  notes: string | null;
}
