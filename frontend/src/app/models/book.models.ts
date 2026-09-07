export interface BookRequest {
  title: string;
  author: string;
  genre: string | null;
  publicationDate: string;
  description: string | null;
}

export interface BookResponse extends BookRequest {
  id: number;
  createdAt: string;
}
