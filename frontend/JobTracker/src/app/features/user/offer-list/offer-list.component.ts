import { Component, OnInit } from '@angular/core';
import { Offer } from '../../models/offer';
import { OfferService } from '../../../core/services/offer/offer.service';
import { CurrencyPipe, DatePipe, CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-offer-list',
  imports: [CommonModule, DatePipe, CurrencyPipe, RouterLink],
  templateUrl: './offer-list.component.html',
  styleUrl: './offer-list.component.scss',
})
export class OfferListComponent implements OnInit {
  offers: Offer[] = [];
  now = new Date().toISOString();
  constructor(private offerService: OfferService) {}

  ngOnInit() {
    this.loadOffers();
  }

  loadOffers() {
    this.offerService.getOffers().subscribe((data) => (this.offers = data));
  }

  onDelete(id: number) {
    if (confirm('Are you sure you want to delete this offer?')) {
      this.offerService.deleteOffer(id).subscribe(() => this.loadOffers());
    }
  }
}
