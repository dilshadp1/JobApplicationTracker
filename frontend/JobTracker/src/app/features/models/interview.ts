export interface Interview {
  id: number;
  applicationId: number;
  companyName: string;
  jobPosition: string;
  interviewDate: string;
  roundName: string;
  mode: string;
  status: string;
  locationUrl?: string;
  feedback?: string;
}

export interface InterviewUpdate {
  id?: number;
  userId: number;
  applicationId: number;
  interviewDate: string;
  roundName: string;
  mode: number;
  status: number;
  locationUrl: string | null;
  feedback: string | null;
}
