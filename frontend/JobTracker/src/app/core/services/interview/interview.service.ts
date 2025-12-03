import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Interview, InterviewUpdate } from '../../../features/models/interview';

@Injectable({ providedIn: 'root' })
export class InterviewService {
  private apiUrl = 'https://localhost:7126/api/interviews';

  constructor(private http: HttpClient) {}

  getInterviews(status?: string): Observable<Interview[]> {
    let params = {};
    if (status && status !== 'All') {
      params = { status: status };
    }
    return this.http.get<Interview[]>(this.apiUrl, { params });
  }

  getInterview(id: number): Observable<Interview> {
    return this.http.get<Interview>(`${this.apiUrl}/${id}`);
  }

  addInterview(formData: InterviewUpdate) {
    return this.http.post(this.apiUrl, formData);
  }

  updateInterview(id: number, formData: InterviewUpdate) {
    return this.http.put(`${this.apiUrl}/${id}`, formData);
  }

  deleteInterview(id: number) {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
