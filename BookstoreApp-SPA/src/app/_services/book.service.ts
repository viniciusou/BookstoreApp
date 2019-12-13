import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Book } from '../_models/book';
import { PaginatedResult } from '../_models/pagination';
import { map } from 'rxjs/operators';


@Injectable({
  providedIn: 'root'
})
export class BookService {
  baseUrl = environment.apiUrl;

constructor(private http: HttpClient) { }

  addBook(book: Book) {
    return this.http.post(this.baseUrl + 'books', book);
  }

  deleteBook(id: number) {
    return this.http.delete(this.baseUrl + 'books/' + id);
  }

  getBooks(page?, itemsPerPage?, bookParams?): Observable<PaginatedResult<Book[]>> {
    const paginatedResult: PaginatedResult<Book[]> = new PaginatedResult<Book[]>();

    let params = new HttpParams();

    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }

    if (bookParams != null) {
      params = params.append('orderBy', bookParams.orderBy);
    }

    return this.http.get<Book[]>(this.baseUrl + 'books', { observe: 'response', params })
      .pipe(
        map(response => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
          }
          return paginatedResult;
        })
      );
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

