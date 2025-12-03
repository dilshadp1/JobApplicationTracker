import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {
  JobApplication,
  JobApplicationUpdate,
} from '../../../features/models/job-application';

@Injectable({
  providedIn: 'root',
})
export class JobApplicationService {
  private apiUrl = 'https://localhost:7126/api/jobapplications';
  constructor(private http: HttpClient) {}

  getApplications(status?: string): Observable<JobApplication[]> {
    let params = {};
    if (status && status !== 'All') {
      params = { status: status };
    }
    return this.http.get<JobApplication[]>(this.apiUrl, { params });
  }

  getJob(id: number): Observable<JobApplication> {
    return this.http.get<JobApplication>(`${this.apiUrl}/${id}`);
  }

  addJob(formData: JobApplicationUpdate): Observable<number> {
    return this.http.post<number>(this.apiUrl, formData);
  }

  deleteJob(id: number) {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  updateJob(id: number, formData: JobApplicationUpdate) {
    return this.http.put(`${this.apiUrl}/${id}`, formData);
  }
}
