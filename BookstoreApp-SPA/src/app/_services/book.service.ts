import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Book } from '../_models/book';


@Injectable({
  providedIn: 'root'
})
export class BookService {
  baseUrl = environment.apiUrl;

constructor(private http: HttpClient) { }

  getBooks(): Observable<Book[]> {
    return this.http.get<Book[]>(this.baseUrl + 'books');
  }

  getBook(id): Observable<Book> {
    return this.http.get<Book>(this.baseUrl + 'books/' + id);
  }

  updateBook(book: Book) {
    return this.http.put(this.baseUrl + 'books/' + book.id, book);
  }

  setMainPhoto(bookId: number, id: number) {
    return this.http.post(this.baseUrl + 'books/' + bookId + '/photos/' + id + '/setMain', {});
  }

  deletePhoto(bookId: number, id: number) {
    return this.http.delete(this.baseUrl + 'books/' + bookId + '/photos/' + id);
  }
}

