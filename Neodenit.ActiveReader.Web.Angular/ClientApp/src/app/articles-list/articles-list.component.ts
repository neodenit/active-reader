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

  refreshInterval = 1000;

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
      let articles = this.articles.slice();

      if (article.id) {
        articles = articles.filter(a => a.id !== article.id);
      }

      articles.push(article);

      this.articles = articles;

      this.sortArticles();
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
    this.http.get<IArticle[]>("articles", data => {
      this.articles = data;

      this.sortArticles();

      if (this.articles.some(x => x.state === ArticleState.Processing)) {
        setTimeout(() => this.getArticles(), this.refreshInterval);
      }
    });
  }

  private sortArticles() {
    this.articles = this.articles.sort((a, b) => a.title.localeCompare(b.title));
  }
}
