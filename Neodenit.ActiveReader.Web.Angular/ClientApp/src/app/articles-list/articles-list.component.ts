import { Component, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";

@Component({
  selector: "articles-list",
  templateUrl: "./articles-list.component.html"
})
export class ArticlesListComponent {
  articles: Article[];
  isAdding: boolean;

  constructor(private http: HttpClient, @Inject("BASE_URL") private baseUrl: string) {
    this.http.get<Article[]>(`${this.baseUrl}articles`).subscribe(
      data => this.articles = data,
      error => console.error(error));
  }

  startAdding() {
    this.isAdding = true;
  }

  finishAdding(article: Article) {
    this.isAdding = false;

    if (article) {
      this.articles.push(article);
    }
  }

  remove(article: Article) {
    this.http.delete<Article>(`${this.baseUrl}articles/${article.id}`).subscribe(
        data => this.articles = this.articles.filter(x => x.id !== data.id),
        error => console.error(error));
  }
}

export interface Article {
  id: number;
  title: string;
  text: string;
}
