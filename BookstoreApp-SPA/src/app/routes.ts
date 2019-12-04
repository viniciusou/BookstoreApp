import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { BookListComponent } from './book-list/book-list.component';
import { RegisterBookComponent } from './register-book/register-book.component';
import { AuthGuard } from './_guards/auth.guard';

export const appRoutes: Routes = [
  { path: '', component: HomeComponent },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      { path: 'books', component: BookListComponent },
      { path: 'register-book', component: RegisterBookComponent }
    ]
  },
  { path: '**', redirectTo: '', pathMatch: 'full' }
];
