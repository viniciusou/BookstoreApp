import { Component, OnInit } from '@angular/core';
import { Book } from '../../_models/book';
import { BookService } from '../../_services/book.service';
import { AlertifyService } from '../../_services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { Pagination, PaginatedResult } from 'src/app/_models/pagination';

@Component({
  selector: 'app-book-list',
  templateUrl: './book-list.component.html',
  styleUrls: ['./book-list.component.css']
})
export class BookListComponent implements OnInit {
  books: Book[];
  pagination: Pagination;
  bookParams: any = {};
  constructor(
    private bookService: BookService,
    private alertify: AlertifyService,
    private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.books = data['books'].result;
      this.pagination = data['books'].pagination;
    });

    this.bookParams.orderBy = 'title';
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadBooks();
  }

  loadBooks() {
    this.bookService.getBooks(this.pagination.currentPage, this.pagination.itemsPerPage, this.bookParams)
      .subscribe(
        (res: PaginatedResult<Book[]>) => {
          this.books = res.result;
          this.pagination = res.pagination;
    }, error => {
      this.alertify.error(error);
    });
  }
}
