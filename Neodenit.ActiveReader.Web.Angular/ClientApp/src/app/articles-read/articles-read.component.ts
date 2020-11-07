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
  progress: number;
  startingText: string;
  newText: string;
  choices: string[];
  correctAnswer: string;
  incorrectAnswer: string;
  showCorrectAnswer: boolean;
  showIncorrectAnswer: boolean;

  incorrectAnswerTimeout = 3000;
  correctAnswerTimeout = 1000;

  correctAnswerTime: Date;

  constructor(private http: HttpClientService, private route: ActivatedRoute) {
    this.position = 1;
    this.score = null;
  }

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get("id");
    const id = parseInt(idParam);

    this.http.get<IArticle>(`articles/${id}`, data => this.article = data);

    this.getQuestion(id, this.position);
  }

  check(answer: string) {
    if (!this.showCorrectAnswer && !this.showIncorrectAnswer) {
      if (answer === this.correctAnswer) {
        this.correctAnswerTime = new Date();

        this.score++;

        this.showCorrectAnswer = true;

        const nextPosition = this.position + this.article.answerLength;
        this.getQuestion(this.article.id, nextPosition);
      } else {
        this.score--;

        this.incorrectAnswer = answer;
        this.showIncorrectAnswer = true;

        setTimeout(() => {
          this.showIncorrectAnswer = false;
          this.choices = this.choices.filter(x => x !== answer);
        }, this.incorrectAnswerTimeout);
      }
    }
  }

  getQuestion(articleId: number, nextPosition: number) {
    this.http.get<IQuestion>(`questions/article/${articleId}/position/${nextPosition}`,
      data => {
        const currentTime = new Date().getTime();
        const previousTime = this.correctAnswerTime ? this.correctAnswerTime.getTime() : 0
        const timeDiff = currentTime - previousTime;
        const delay = Math.max(0, this.correctAnswerTimeout - timeDiff);

        setTimeout(() => {
          this.showCorrectAnswer = false;
          this.showIncorrectAnswer = false;

          this.position = data.answerPosition;
          this.progress = data.progress;
          this.startingText = data.startingText;
          this.newText = data.newText;
          this.choices = data.choices;
          this.correctAnswer = data.correctAnswer;

          setTimeout(() => window.scrollTo({ top: document.body.scrollHeight, behavior: "smooth" }));
        }, delay);

        this.http.post<IQuestion>(`articles/${articleId}/position/${data.lastPosition}`);
      });
  }

  navigateToPosition(position: number) {
    this.score = null;

    this.getQuestion(this.article.id, position);
  }
}
