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

  articleId: number;

  articleState = ArticleState;

  constructor(private http: HttpClientService) { }

  ngOnInit() {
    this.getArticles();
  }

  startAdding() {
    this.articleId = null;
    this.isAdding = true;
  }

  finishAdding(article: IArticle) {
    this.isAdding = false;
    this.articleId = null;

    if (article) {
      if (article.id) {
        this.articles = this.articles.filter(a => a.id !== article.id);
      }

      this.articles.push(article);
    }
  }

  updateList() {
    this.getArticles();
  }

  edit(article: IArticle) {
    this.articleId = article.id;
    this.isAdding = true;
  }

  restart(article: IArticle) {
    article.state = ArticleState.Processing;

    this.http.post(`articles/${article.id}/restart`,
      null,
      () => this.getArticles());
  }

  remove(article: IArticle) {
    this.http.delete(`articles/${article.id}`,
      () => this.articles = this.articles.filter(x => x.id !== article.id));
  }

  private getArticles() {
    this.http.get<IArticle[]>("articles", data => this.articles = data);
  }
}
