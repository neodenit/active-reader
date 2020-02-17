import { Component, OnInit } from "@angular/core";
import { IArticle } from "../models/IArticle";
import { HttpClientService } from "../shared/services/http-client.service";

@Component({
  selector: "articles-list",
  templateUrl: "./articles-list.component.html"
})
export class ArticlesListComponent implements OnInit {
  articles: IArticle[];
  isAdding: boolean;

  constructor(private http: HttpClientService) { }

  ngOnInit() {
    this.http.get<IArticle[]>("articles", data => this.articles = data);
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
    this.http.delete(`articles/${article.id}`,
      () => this.articles = this.articles.filter(x => x.id !== article.id));
  }
}
