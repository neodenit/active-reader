import { Component, Input, OnChanges, SimpleChanges } from "@angular/core";

@Component({
  selector: "articles-score",
  templateUrl: "./articles-score.component.html"
})
export class ArticlesScoreComponent implements OnChanges {
  @Input() score: number;

  scoreStyle: string;

  ngOnChanges(changes: SimpleChanges): void {
    this.scoreStyle = changes.score.currentValue === null
      ? ""
      : changes.score.currentValue > changes.score.previousValue
        ? "right"
        : "wrong";
  }
}
