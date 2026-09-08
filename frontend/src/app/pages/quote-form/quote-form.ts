import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';

import { QuoteService } from '../../services/quote-service';
import { QuoteRequest } from '../../models/quote.models';

@Component({
  selector: 'app-quote-form',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './quote-form.html',
  styleUrl: './quote-form.css',
})
export class QuoteForm implements OnInit {
  errorMessage = '';
  isSubmitting = false;
  isEditing = false;
  isLoading = false;
  private quoteId: number | null = null;

  quoteForm = new FormGroup({
    text: new FormControl('', {
      nonNullable: true,
      validators: [
        Validators.required
      ]
    }),
    author: new FormControl('', {
      nonNullable: true,
      validators: [
        Validators.maxLength(100)
      ]
    })
  });

  constructor(
    private readonly quoteService: QuoteService,
    private readonly router: Router,
    private readonly route: ActivatedRoute
  ) {

  }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');

    if (id !== null) {
      this.quoteId = Number(id);
      this.isEditing = true;
      this.loadQuote();
    }
  }

  private loadQuote(): void {
    if (this.quoteId === null) {
      return;
    }

    this.isLoading = true;

    this.quoteService.getById(this.quoteId).subscribe({
      next: (quote) => {
        this.quoteForm.patchValue({
          text: quote.text,
          author: quote.author ?? ''
        });

        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Citatet kunde inte hämtas.';
        this.isLoading = false;
      }
    });
  }

  onSubmit(): void {
    if (this.quoteForm.invalid) {
      this.quoteForm.markAllAsTouched();
      return;
    }

    const values = this.quoteForm.getRawValue();

    const request: QuoteRequest = {
      text: values.text.trim(),
      author: values.author.trim() === '' ? null : values.author.trim()
    };

    this.errorMessage = '';
    this.isSubmitting = true;

    const saveRequest = this.isEditing && this.quoteId !== null
      ? this.quoteService.update(this.quoteId, request)
      : this.quoteService.create(request);

    saveRequest.subscribe({
      next: () => {
        void this.router.navigate(['/quotes']);
      },
      error: () => {
        this.errorMessage = this.isEditing
          ? 'Citatet kunde inte uppdateras. Försök igen.'
          : 'Citatet kunde inte sparas. Försök igen.'

        this.isSubmitting = false;
      }
    });
  }
}
