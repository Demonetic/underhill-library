import { Injectable } from '@angular/core';
import { HttpClient} from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../environments/environment';
import { BookRequest, BookResponse } from '../models/book.models';

@Injectable({
  providedIn: 'root',
})
export class BookService {
  private readonly apiUrl = `${environment.apiUrl}/books`;

  constructor(private readonly http: HttpClient) {

  }

  getAll(): Observable<BookResponse[]> {
    return this.http.get<BookResponse[]>(this.apiUrl);
  }

  getById(id: number): Observable<BookResponse> {
    return this.http.get<BookResponse>(`${this.apiUrl}/${id}`);
  }

  create(request: BookRequest): Observable<BookResponse> {
    return this.http.post<BookResponse>(this.apiUrl, request);
  }

  update(id: number, request: BookRequest): Observable<BookResponse> {
    return this.http.put<BookResponse>(`${this.apiUrl}/${id}`, request);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
