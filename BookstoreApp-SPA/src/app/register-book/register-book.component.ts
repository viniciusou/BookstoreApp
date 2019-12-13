import { Component, OnInit, HostListener } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap';
import { Router } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { Book } from '../_models/book';
import { BookService } from '../_services/book.service';

@Component({
  selector: 'app-register-book',
  templateUrl: './register-book.component.html',
  styleUrls: ['./register-book.component.css']
})
export class RegisterBookComponent implements OnInit {
  book: Book;
  form: FormGroup;
  bsConfig: Partial<BsDatepickerConfig>;
  @HostListener('window:beforeunload', ['$event'])
  unloadNotification($event: any) {
    if (this.form.dirty) {
      $event.returnValue = true;
    }
  }

  constructor(
    private bookService: BookService,
    private fb: FormBuilder,
    private router: Router,
    private alertify: AlertifyService
  ) {}

  ngOnInit() {
    this.bsConfig = {
      containerClass: 'theme-blue'
    };
    this.creteform();
  }

  creteform() {
    this.form = this.fb.group({
      title: ['', Validators.required],
      author: ['', Validators.required],
      publisher: ['', Validators.required],
      dateReleased: [null, Validators.required],
      description: ['', Validators.required]
    });
  }

  registerBook() {
    if (this.form.valid) {
      this.book = Object.assign({}, this.form.value);
      this.bookService.addBook(this.book).subscribe(() => {
        this.alertify.success('Book registered successfully');
        this.form.reset(this.book);
        this.router.navigate(['/books']);
      }, error => {
        this.alertify.error('It was not possible to register the book');
      });
    }
  }

  cancel() {
    this.form.reset(this.book);
    this.router.navigate(['/books']);
  }
}
