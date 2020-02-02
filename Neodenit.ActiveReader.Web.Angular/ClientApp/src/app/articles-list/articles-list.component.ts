import { Component, Inject, OnInit } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { IArticle } from "../models/IArticle";

@Component({
  selector: "articles-list",
  templateUrl: "./articles-list.component.html"
})
export class ArticlesListComponent implements OnInit {
  articles: IArticle[];
  isAdding: boolean;

  constructor(private http: HttpClient, @Inject("BASE_URL") private baseUrl: string) { }

  ngOnInit() {
    this.http.get<IArticle[]>(`${this.baseUrl}articles`).subscribe(
      data => this.articles = data,
      error => console.error(error));
  }

  startAdding() {
    this.isAdding = true;
  }

  finishAdding(article: IArticle) {
    this.isAdding = false;

    if (article) {
      this.articles.push(article);
    }
  }

  remove(article: IArticle) {
    this.http.delete<IArticle>(`${this.baseUrl}articles/${article.id}`).subscribe(
      () => this.articles = this.articles.filter(x => x.id !== article.id),
      error => console.error(error));
  }
}
