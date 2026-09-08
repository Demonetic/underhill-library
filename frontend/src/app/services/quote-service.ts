import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../environments/environment';
import { QuoteRequest, QuoteResponse } from '../models/quote.models';

@Injectable({
  providedIn: 'root',
})
export class QuoteService {
  private readonly apiUrl = `${environment.apiUrl}/quotes`;

  constructor(private readonly http: HttpClient) {

  }

  getAll(): Observable<QuoteResponse[]> {
    return this.http.get<QuoteResponse[]>(this.apiUrl);
  }

  getById(id: number): Observable<QuoteResponse> {
    return this.http.get<QuoteResponse>(`${this.apiUrl}/${id}`);
  }

  create(request: QuoteRequest): Observable<QuoteResponse> {
    return this.http.post<QuoteResponse>(this.apiUrl, request);
  }

  update(id: number, request: QuoteRequest): Observable<QuoteResponse> {
    return this.http.put<QuoteResponse>(`${this.apiUrl}/${id}`, request);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
