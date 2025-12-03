import { Component, effect, OnInit, signal } from '@angular/core';
import { Offer } from '../../models/offer';
import { OfferService } from '../../../core/services/offer/offer.service';
import { CurrencyPipe, DatePipe, CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-offer-list',
  imports: [CommonModule, DatePipe, CurrencyPipe, RouterLink,FormsModule],
  templateUrl: './offer-list.component.html',
  styleUrl: './offer-list.component.scss',
})
export class OfferListComponent {
  offers = signal<Offer[]>([]);
  now = new Date().toISOString();

  filterType = signal<string>('All');
  sortType = signal<string>('Default');

  constructor(private offerService: OfferService) {
    effect(() => {
      this.loadOffers();
    });
  }

  loadOffers() {
    this.offerService
      .getOffers(this.filterType(), this.sortType())
      .subscribe((data) => {
        this.offers.set(data);
      });
  }
}
