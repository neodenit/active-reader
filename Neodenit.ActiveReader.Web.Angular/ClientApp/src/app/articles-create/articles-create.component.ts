import { Component, Inject, EventEmitter, Output, OnInit } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { IArticle } from "../models/IArticle";

@Component({
  selector: "articles-create",
  templateUrl: "./articles-create.component.html"
})
export class ArticlesCreateComponent implements OnInit {
  newArticleTitle: string;
  newArticleText: string;
  prefixLength: string;
  prefixLengthOptions = ["1", "2", "3"];

  @Output()
  close: EventEmitter<IArticle> = new EventEmitter()

  constructor(private http: HttpClient, @Inject("BASE_URL") private baseUrl: string) { }

  ngOnInit(): void {
    this.http.get<number>(`${this.baseUrl}articles/defaultprefixlength`).subscribe(
      data => this.prefixLength = data.toString(),
      error => console.error(error));  }

  add() {
    if (this.newArticleTitle && this.newArticleText) {
      let data = {
        title: this.newArticleTitle,
        text: this.newArticleText,
        prefixLength: parseInt(this.prefixLength)
      };

      this.http.post<IArticle>(`${this.baseUrl}articles`, data).subscribe(
        article => this.close.emit(article),
        error => console.error(error));
    }
  }

  cancel() {
    this.close.emit(null);
  }
}
