import { Component, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";

@Component({
  selector: "articles-list",
  templateUrl: "./articles-list.component.html"
})
export class ArticlesListComponent {
  articles: Article[];
  isAdding: boolean;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<Article[]>(baseUrl + "articles").subscribe(
      data => this.articles = data,
      error => console.error(error));
  }

  public startAdding() {
    this.isAdding = true;
  }

  public finishAdding(article: Article) {
    this.isAdding = false;

    if (article) {
      this.articles.push(article);
    }
  }
}

export interface Article {
  id: number;
  title: string;
  text: string;
}
