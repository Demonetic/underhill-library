import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';

import { QuoteService } from '../../services/quote-service';
import { QuoteResponse } from '../../models/quote.models';

@Component({
  selector: 'app-quotes',
  imports: [RouterLink],
  templateUrl: './quotes.html',
  styleUrl: './quotes.css',
})
export class Quotes implements OnInit {
  quotes: QuoteResponse[] = [];
  isLoading = true;
  errorMessage = '';

  constructor(private readonly quoteService: QuoteService) {

  }

  ngOnInit(): void {
    this.loadQuotes();
  }

  loadQuotes(): void {
    this.errorMessage = '';
    this.isLoading = true;

    this.quoteService.getAll().subscribe({
      next: (quotes) => {
        this.quotes = quotes;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Citaten kunde inte hämtas.';
        this.isLoading = false;
      }
    });
  }

  deleteQuote(id: number): void {
    const confirmed = window.confirm('Är du säker på att du vill ta bort citatet?');

    if (!confirmed) {
      return;
    }

    this.quoteService.delete(id).subscribe({
      next: () => {
        this.quotes = this.quotes.filter(quote => quote.id !== id);
      },
      error: () => {
        this.errorMessage = 'Citatet kunde inte tas bort.';
      }
    });
  }
}
