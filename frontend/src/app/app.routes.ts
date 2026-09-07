import { Routes } from '@angular/router';
import { Login } from './pages/login/login';
import { Register } from './pages/register/register';
import { Books } from './pages/books/books';
import { Quotes } from './pages/quotes/quotes';
import { authGuard } from './guards/auth-guard';
import { BookForm } from './pages/book-form/book-form';

export const routes: Routes = [
  { path: 'login', component: Login },
  { path: 'register', component: Register },
  { path: 'books',
    component: Books,
    canActivate: [authGuard]
  },
  {
    path: 'books/new',
    component: BookForm,
    canActivate: [authGuard]
  },
  {
    path: 'books/:id/edit',
    component: BookForm,
    canActivate: [authGuard]
  },
  { path: 'quotes',
    component: Quotes,
    canActivate: [authGuard]
  },
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: '**', redirectTo: 'login' }
];
