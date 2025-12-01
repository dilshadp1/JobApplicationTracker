import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { JobApplication } from '../../features/models/job-application';

@Injectable({
  providedIn: 'root'
})
export class JobApplicationService {

  private apiUrl='https://localhost:7126/api/jobapplications'
  constructor(private http: HttpClient) { }

  getApplications() : Observable<JobApplication[]>{
   return this.http.get<JobApplication[]>(this.apiUrl)
  }
}
