import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Offer, OfferUpdate } from '../../../features/models/offer';

@Injectable({ providedIn: 'root' })
export class OfferService {
  private apiUrl = 'https://localhost:7126/api/offers';

  constructor(private http: HttpClient) {}

  getOffers(): Observable<Offer[]> {
    return this.http.get<Offer[]>(this.apiUrl);
  }

  getOffer(id: number): Observable<Offer> {
    return this.http.get<Offer>(`${this.apiUrl}/${id}`);
  }

  addOffer(data: OfferUpdate) {
    return this.http.post(this.apiUrl, data);
  }

  updateOffer(id: number, data: OfferUpdate) {
    return this.http.put(`${this.apiUrl}/${id}`, data);
  }

  deleteOffer(id: number) {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
