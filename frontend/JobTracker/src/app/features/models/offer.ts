export interface Offer {
  id: number;
  applicationId: number;
  companyName: string;
  jobPosition: string;
  salary: number;
  offerDate: string;
  deadline: string;
  benefits?: string;
}

export interface OfferUpdate {
  id?: number;
  applicationId: number;
  salary: number;
  offerDate: string;
  deadline: string;
  benefits: string | null;
}
