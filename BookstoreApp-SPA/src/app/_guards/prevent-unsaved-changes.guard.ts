import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { BookEditComponent } from '../books/book-edit/book-edit.component';

@Injectable()
export class PreventUnsavedChanges implements CanDeactivate<BookEditComponent> {
    canDeactivate(component: BookEditComponent) {
        if (component.editForm.dirty) {
            return confirm('Are you sure you want to continue? Any unsaved changes will be lost');
        }
        return true;
    }
}