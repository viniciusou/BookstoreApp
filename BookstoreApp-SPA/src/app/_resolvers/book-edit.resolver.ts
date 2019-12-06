import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { Book } from '../_models/book';
import { BookService } from '../_services/book.service';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthService } from '../_services/auth.service';

@Injectable()
export class BookEditResolver implements Resolve<Book> {
  constructor(
    private bookService: BookService,
    private router: Router,
    private alertify: AlertifyService
  ) {}

  resolve(route: ActivatedRouteSnapshot): Observable<Book> {
    return this.bookService.getBook(route.params['id']).pipe(
        catchError(error => {
            this.alertify.error('Problem retrieving your data');
            this.router.navigate(['/books']);
            return of(null);
        })
    );
  }
}
