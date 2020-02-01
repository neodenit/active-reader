import { Component, Inject, OnInit } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Article } from "../articles-list/articles-list.component";
import { ActivatedRoute } from "@angular/router";

@Component({
  selector: "articles-read",
  templateUrl: "./articles-read.component.html"
})
export class ArticlesReadComponent implements OnInit {
  article: Article;
  score: number;
  scoreStyle: string;
  position: number;
  startText: string;
  choices: string[];
  answer: string;

  constructor(private http: HttpClient, @Inject("BASE_URL") private baseUrl: string, private route: ActivatedRoute) {
    this.position = 1;
    this.score = 0;
  }

  ngOnInit(): void {
    let idParam = this.route.snapshot.paramMap.get("id");
    let id = parseInt(idParam);

    this.http.get<Article>(`${this.baseUrl}articles/${id}`).subscribe(
      data => this.article = data,
      error => console.error(error));

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
    this.http.get<Question>(`${this.baseUrl}questions/article/${articleId}/position/${nextPosition}`).subscribe(
      data => {
        this.position = data.answerPosition;
        this.startText = data.startingWords;
        this.choices = data.variants;
        this.answer = data.answer;

        this.http.post<Question>(`${this.baseUrl}articles/${articleId}/position/${data.answerPosition}`, null).subscribe();
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
