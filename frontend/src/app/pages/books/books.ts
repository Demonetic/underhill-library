import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';

import { BookService } from '../../services/book-service';
import { BookResponse } from '../../models/book.models';

@Component({
  selector: 'app-books',
  imports: [RouterLink],
  templateUrl: './books.html',
  styleUrl: './books.css',
})
export class Books implements OnInit {
  books: BookResponse[] = [];
  isLoading = true;
  errorMessage = '';

  constructor(private readonly bookServie: BookService) {

  }

  ngOnInit(): void {
    this.loadBooks();
  }

  loadBooks(): void {
    this.errorMessage = '';
    this.isLoading = true;

    this.bookServie.getAll().subscribe({
      next: (books) => {
        this.books = books;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Böckerna kunde inte hämtas.';
        this.isLoading = false;
      }
    });
  }

  deleteBook(id: number): void {
    const confirmed = window.confirm('Är du säker på att du vill ta bort boken?');

    if (!confirmed) {
      return;
    }

    this.bookServie.delete(id).subscribe({
      next: () => {
        this.books = this.books.filter(book => book.id !== id);
      },
      error: () => {
        this.errorMessage = 'Boken kunde inte tas bort.';
      }
    });
  }
}
