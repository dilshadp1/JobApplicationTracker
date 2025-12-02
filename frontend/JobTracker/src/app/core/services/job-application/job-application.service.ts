import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {
  JobApplication,
  JobApplicationUpdate,
} from '../../../features/models/job-application';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class JobApplicationService {
  private apiUrl = 'https://localhost:7126/api/jobapplications';
  constructor(private http: HttpClient) {}

  getApplications(): Observable<JobApplication[]> {
    return this.http.get<JobApplication[]>(this.apiUrl);
  }

  getJob(id: number): Observable<JobApplication> {
    return this.http.get<JobApplication>(`${this.apiUrl}/${id}`);
  }

  addJob(formData: JobApplicationUpdate) {
    return this.http.post(this.apiUrl, formData);
  }

  deleteJob(id: number) {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  updateJob(id: number, formData: JobApplicationUpdate) {
    return this.http.put(`${this.apiUrl}/${id}`, formData);
  }
}
