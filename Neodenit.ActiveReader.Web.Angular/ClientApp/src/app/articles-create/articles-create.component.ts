import { Component, Inject, EventEmitter, Output } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { IArticle } from "../models/IArticle";

@Component({
  selector: "articles-create",
  templateUrl: "./articles-create.component.html"
})
export class ArticlesCreateComponent {
  newArticleTitle: string;
  newArticleText: string;

  @Output()
  close: EventEmitter<IArticle> = new EventEmitter()

  constructor(private http: HttpClient, @Inject("BASE_URL") private baseUrl: string) { }

  add() {
    if (this.newArticleTitle && this.newArticleText) {
      let data = {
        title: this.newArticleTitle,
        text: this.newArticleText
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
