import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { Book } from '../_models/book';
import { BookService } from '../_services/book.service';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class BookListResolver implements Resolve<Book[]> {
  pageNumber = 1;
  pageSize = 5;

  constructor(
    private bookService: BookService,
    private router: Router,
    private alertify: AlertifyService
  ) {}

  resolve(route: ActivatedRouteSnapshot): Observable<Book[]> {
    return this.bookService.getBooks(this.pageNumber, this.pageSize).pipe(
        catchError(error => {
            this.alertify.error('Problem retrieving data');
            this.router.navigate(['/home']);
            return of(null);
        })
    );
  }
}
