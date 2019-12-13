import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Book } from 'src/app/_models/book';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { NgForm } from '@angular/forms';
import { BookService } from 'src/app/_services/book.service';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-book-edit',
  templateUrl: './book-edit.component.html',
  styleUrls: ['./book-edit.component.css']
})
export class BookEditComponent implements OnInit {
  @ViewChild('form', {static: true}) form: NgForm;
  book: Book;
  @HostListener('window:beforeunload', ['$event'])
  unloadNotification($event: any) {
    if (this.form.dirty) {
      $event.returnValue = true;
    }
  }

  constructor(
    private route: ActivatedRoute,
    private alertify: AlertifyService,
    private bookService: BookService,
    private authService: AuthService,
    private router: Router) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.book = data['book'];
    });
  }

  updateBook() {
    this.bookService.updateBook(this.book).subscribe(next => {
      this.alertify.success('Book updated succesfully');
      this.form.reset(this.book);
    }, error => {
      this.alertify.error(error);
    });
  }

  updateMainPhoto(photoUrl) {
    this.book.photoUrl = photoUrl;
  }

  deleteBook(id) {
    this.alertify.confirm('Are you sure you want to delete this book?', () => {
      this.bookService.deleteBook(id).subscribe(() => {
        this.alertify.success('Book has been deleted');
      }, error => {
        this.alertify.error('error');
      }, () => {
        this.router.navigate(['/books']);
      });
    });
  }

}
