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
  score: number;
  scoreStyle: string;
  position: number;
  startText: string;
  choices: string[];
  answer: string;

  constructor(private http: HttpClient, @Inject("BASE_URL") private baseUrl: string, private route: ActivatedRoute) {    
    let idParam = this.route.snapshot.paramMap.get("id");
    let id = parseInt(idParam);
    http.get<Article>(`${baseUrl}articles/${id}`).subscribe(
      data => this.article = data,
      error => console.error(error));

    this.position = 0;
    this.score = 0;
    this.getQuestion(id, this.position);
  }

  check(answer: string) {
    if (answer === this.answer) {
      this.score++;
      this.scoreStyle = "right";
      this.getQuestion(this.article.id, this.position);
    } else {
      this.score--;
      this.scoreStyle = "wrong";
      this.choices = this.choices.filter(x => x !== answer);
    }
  }

  getQuestion(articleId: number, position: number) {
    this.http.get<Question>(`${this.baseUrl}questions/${articleId}/${position}`).subscribe(
      data => {
        this.position = data.answerPosition;
        this.startText = data.startingWords;
        this.choices = data.variants;
        this.answer = data.answer;
      },
      error => console.error(error));
  }
}

interface Question {
  answerPosition: number;
  startingWords: string;
  variants: string[];
  answer: string;
}
