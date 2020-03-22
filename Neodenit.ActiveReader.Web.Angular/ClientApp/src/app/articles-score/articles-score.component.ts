import { Component, Input, OnChanges, SimpleChanges } from "@angular/core";

@Component({
  selector: "articles-score",
  templateUrl: "./articles-score.component.html"
})
export class ArticlesScoreComponent implements OnChanges {
  @Input() score: number;

  scoreStyle: string;

  readonly resetClass = "";
  readonly rightAnswerClass = "right";
  readonly wrongAnswerClass = "wrong";

  ngOnChanges(changes: SimpleChanges) {
    this.scoreStyle = changes.score.currentValue === null
      ? this.resetClass
      : changes.score.currentValue > changes.score.previousValue
        ? this.rightAnswerClass
        : this.wrongAnswerClass;
  }
}
