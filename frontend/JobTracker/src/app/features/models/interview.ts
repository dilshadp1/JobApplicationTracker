export enum InterviewMode {
  Online = 'Online',
  Offline = 'Offline',
}

export enum InterviewStatus {
  Scheduled = 'Scheduled',
  Completed = 'Completed',
  Cancelled = 'Cancelled',
  NoShow = 'NoShow',
}

export interface Interview {
  id: number;
  applicationId: number;
  companyName: string;
  jobPosition: string;
  interviewDate: string;
  roundName: string;
  mode: InterviewMode;
  status: InterviewStatus;
  locationUrl?: string;
  feedback?: string;
}

export interface InterviewUpdate {
  id?: number;
  applicationId: number;
  interviewDate: string;
  roundName: string;
  mode: InterviewMode;
  status: InterviewStatus;
  locationUrl: string | null;
  feedback: string | null;
}
