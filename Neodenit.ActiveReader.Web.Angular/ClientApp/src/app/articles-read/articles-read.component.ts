import { Component, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Article } from "../articles-list/articles-list.component";
import { ActivatedRoute } from "@angular/router";

@Component({
  selector: "articles-read",
  templateUrl: "./articles-read.component.html"
})
export class ArticlesReadComponent {
  article: Article;

  constructor(private http: HttpClient, @Inject("BASE_URL") private baseUrl: string, private route: ActivatedRoute) {    
    let id = this.route.snapshot.paramMap.get("id");
    http.get<Article>(baseUrl + "articles" + "/" + id).subscribe(
      data => this.article = data,
      error => console.error(error));   
  }
}
