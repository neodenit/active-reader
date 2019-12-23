import { Component, Inject, EventEmitter, Output } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Article } from "../articles-list/articles-list.component";

@Component({
  selector: "articles-create",
  templateUrl: "./articles-create.component.html"
})
export class ArticlesCreateComponent {
  public newArticleTitle: string;
  public newArticleText: string;

  @Output()
  close: EventEmitter<Article> = new EventEmitter()

  constructor(private http: HttpClient, @Inject("BASE_URL") private baseUrl: string) { }

  public add() {
    var data = {
      title: this.newArticleTitle,
      text: this.newArticleText
    };

    this.http.post<Article>(this.baseUrl + "articles", data).subscribe(
      article => this.close.emit(article),
      error => console.error(error));
  }

  public cancel() {
    this.close.emit(null);
  }
}
