import { Component, OnInit } from '@angular/core';
import { Book } from '../../_models/book';
import { BookService } from '../../_services/book.service';
import { AlertifyService } from '../../_services/alertify.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-book-list',
  templateUrl: './book-list.component.html',
  styleUrls: ['./book-list.component.css']
})
export class BookListComponent implements OnInit {
  books: Book[];

  constructor(
    private bookService: BookService, 
    private alertify: AlertifyService,
    private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.books = data['books'];
    });
  }

  // loadBooks() {
  //   this.bookService.getBooks().subscribe((books: Book[]) => {
  //     this.books = books;
  //   }, error => {
  //     this.alertify.error(error);
  //   });
  // }

}
