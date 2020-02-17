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
  scoreStyle: string;
  position: number;
  startText: string;
  choices: string[];
  answer: string;

  constructor(private http: HttpClientService, private route: ActivatedRoute) {
    this.position = 1;
    this.score = 0;
  }

  ngOnInit(): void {
    let idParam = this.route.snapshot.paramMap.get("id");
    let id = parseInt(idParam);

    this.http.get<IArticle>(`articles/${id}`, data => this.article = data);

    this.getQuestion(id, this.position);
  }

  check(answer: string) {
    if (answer === this.answer) {
      this.score++;
      this.scoreStyle = "right";

      let nextPosition = this.position + 1;
      this.getQuestion(this.article.id, nextPosition);
    } else {
      this.score--;
      this.scoreStyle = "wrong";
      this.choices = this.choices.filter(x => x !== answer);
    }
  }

  getQuestion(articleId: number, nextPosition: number) {
    this.http.get<IQuestion>(`questions/article/${articleId}/position/${nextPosition}`,
      data => {
        this.position = data.answerPosition;
        this.startText = data.startingWords;
        this.choices = data.variants;
        this.answer = data.answer;

        this.http.post<IQuestion>(`articles/${articleId}/position/${data.answerPosition}`, null, null);
      });
    }

  navigateToPosition(position: number) {
    this.score = 0;
    this.scoreStyle = "";

    this.getQuestion(this.article.id, position);
  }
}
