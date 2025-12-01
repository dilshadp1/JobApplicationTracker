import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { JobApplication } from '../../../features/models/job-application';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class JobApplicationService {

  private apiUrl='https://localhost:5001/api/jobapplications'
  constructor(private http: HttpClient) { }

  getApplications() : Observable<JobApplication[]>{
   return this.http.get<JobApplication[]>(this.apiUrl)
  }
}
