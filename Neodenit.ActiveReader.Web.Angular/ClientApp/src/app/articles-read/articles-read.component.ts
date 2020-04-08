import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { IArticle } from "../models/IArticle";
import { IQuestion } from "../models/IQuestion";
import { HttpClientService } from "../shared/services/http-client.service";

@Component({
  selector: "articles-read",
  templateUrl: "./articles-read.component.html"
})
export class ArticlesReadComponent implements OnInit {
  article: IArticle;
  score: number;
  position: number;
  startingText: string;
  newText: string;
  choices: string[];
  correctAnswer: string;

  constructor(private http: HttpClientService, private route: ActivatedRoute) {
    this.position = 1;
    this.score = null;
  }

  ngOnInit(): void {
    let idParam = this.route.snapshot.paramMap.get("id");
    let id = parseInt(idParam);

    this.http.get<IArticle>(`articles/${id}`, data => this.article = data);

    this.getQuestion(id, this.position);
  }

  check(answer: string) {
    if (answer === this.correctAnswer) {
      this.score++;

      let nextPosition = this.position + this.article.answerLength;
      this.getQuestion(this.article.id, nextPosition);
    } else {
      this.score--;

      this.choices = this.choices.filter(x => x !== answer);
    }
  }

  getQuestion(articleId: number, nextPosition: number) {
    this.http.get<IQuestion>(`questions/article/${articleId}/position/${nextPosition}`,
      data => {
        this.position = data.answerPosition;
        this.startingText = data.startingText;
        this.newText = data.newText;
        this.choices = data.choices;
        this.correctAnswer = data.correctAnswer;
      });

      this.http.post<IQuestion>(`articles/${articleId}/position/${nextPosition}`, null, null);
    }

  navigateToPosition(position: number) {
    this.score = null;

    this.getQuestion(this.article.id, position);
  }
}
