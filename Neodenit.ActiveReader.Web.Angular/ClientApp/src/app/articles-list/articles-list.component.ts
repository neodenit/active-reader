import { Component, OnInit } from "@angular/core";
import { IArticle } from "../models/IArticle";
import { HttpClientService } from "../shared/services/http-client.service";
import { ArticleState } from "../models/ArticleState";

@Component({
  selector: "articles-list",
  templateUrl: "./articles-list.component.html"
})
export class ArticlesListComponent implements OnInit {
  articles: IArticle[];
  isAdding: boolean;

  articleState = ArticleState;

  constructor(private http: HttpClientService) { }

  ngOnInit() {
    this.getArticles();
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

  updateList() {
    this.getArticles();
  }

  remove(article: IArticle) {
    this.http.delete(`articles/${article.id}`,
      () => this.articles = this.articles.filter(x => x.id !== article.id));
  }

  private getArticles() {
    this.http.get<IArticle[]>("articles", data => this.articles = data);
  }
}
