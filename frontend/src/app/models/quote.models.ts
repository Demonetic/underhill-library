export interface QuoteRequest {
  text: string;
  author: string | null;
}

export interface QuoteResponse extends QuoteRequest {
  id: number;
  createdAt: string;
}
