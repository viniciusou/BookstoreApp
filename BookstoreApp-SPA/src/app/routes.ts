import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { BookListComponent } from './books/book-list/book-list.component';
import { RegisterBookComponent } from './register-book/register-book.component';
import { AuthGuard } from './_guards/auth.guard';
import { BookDetailComponent } from './books/book-detail/book-detail.component';
import { BookDetailResolver } from './_resolvers/book-detail.resolver';
import { BookListResolver } from './_resolvers/book-list.resolver';

export const appRoutes: Routes = [
  { path: '', component: HomeComponent },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      { path: 'books', component: BookListComponent, resolve: {books: BookListResolver} },
      { path: 'books/:id', component: BookDetailComponent, resolve: {book: BookDetailResolver} },
      { path: 'register-book', component: RegisterBookComponent }
    ]
  },
  { path: '**', redirectTo: '', pathMatch: 'full' }
];
