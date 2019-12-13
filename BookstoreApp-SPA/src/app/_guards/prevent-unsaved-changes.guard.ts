import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { BookEditComponent } from '../books/book-edit/book-edit.component';
import { RegisterBookComponent } from '../register-book/register-book.component';

@Injectable()
export class PreventUnsavedChanges implements CanDeactivate<BookEditComponent | RegisterBookComponent> {
    canDeactivate(component: BookEditComponent | RegisterBookComponent) {
        if (component.form.dirty) {
            return confirm('Are you sure you want to continue? Any unsaved changes will be lost');
        }
        return true;
    }
}
