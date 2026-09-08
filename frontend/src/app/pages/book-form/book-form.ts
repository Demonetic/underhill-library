import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';

import { BookService } from '../../services/book-service';
import { BookRequest } from '../../models/book.models';

@Component({
  selector: 'app-book-form',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './book-form.html',
  styleUrl: './book-form.css',
})
export class BookForm implements OnInit {
  errorMessage = '';
  isSubmitting = false;
  isEditing = false;
  isLoading = false;
  private bookId: number | null = null;

  bookForm = new FormGroup({
    title: new FormControl('', {
      nonNullable: true,
      validators: [
        Validators.required,
        Validators.maxLength(255)
      ]
    }),
    author: new FormControl('', {
      nonNullable: true,
      validators: [
        Validators.required,
        Validators.maxLength(150)
      ]
    }),
    genre: new FormControl('', {
      nonNullable: true,
      validators: [
        Validators.maxLength(100)
      ]
    }),
    publicationDate: new FormControl('', {
      nonNullable: true,
      validators: [
        Validators.required,
      ]
    }),
    description: new FormControl('', {
      nonNullable: true
    })
  });

  constructor(
    private readonly bookService: BookService,
    private readonly router: Router,
    private readonly route: ActivatedRoute
  ) {

  }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');

    if (id !== null) {
      this.bookId = Number(id);
      this.isEditing = true;
      this.loadBook();
    }
  }

  private loadBook(): void {
    if (this.bookId === null) {
      return;
    }

    this.isLoading = true;

    this.bookService.getById(this.bookId).subscribe({
      next: (book) => {
        this.bookForm.patchValue({
          title: book.title,
          author: book.author,
          genre: book.genre ?? '',
          publicationDate: book.publicationDate,
          description: book.description ?? ''
        });

        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Boken kunde inte hämtas.';
        this.isLoading = false;
      }
    });
  }

  onSubmit(): void {
    if (this.bookForm.invalid) {
      this.bookForm.markAllAsTouched();
      return;
    }

    const values = this.bookForm.getRawValue();

    const request: BookRequest = {
      title: values.title,
      author: values.author,
      genre: values.genre.trim() === '' ? null : values.genre.trim(),
      publicationDate: values.publicationDate,
      description: values.description.trim() === '' ? null : values.description.trim()
    };

    this.errorMessage = '';
    this.isSubmitting = true;

    const saveRequest = this.isEditing && this.bookId !== null
      ? this.bookService.update(this.bookId, request)
      : this.bookService.create(request);

    saveRequest.subscribe({
      next: () => {
        void this.router.navigate(['/books']);
      },
      error: () => {
        this.errorMessage = this.isEditing
          ? 'Boken kunde inte uppdateras. Försök igen.'
          : 'Boken kunde inte sparas. Försök igen.';

        this.isSubmitting = false;
      }
    });
  }
}
