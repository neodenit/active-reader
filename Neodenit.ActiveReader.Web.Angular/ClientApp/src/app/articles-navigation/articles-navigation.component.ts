import { Component, Inject, Input, Output, EventEmitter } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { NavigationTarget } from "../models/NavigationTarget";
import { INavigation } from "../models/INavigation";

@Component({
  selector: "articles-navigation",
  templateUrl: "./articles-navigation.component.html"
})
export class ArticlesNavigationComponent {

  @Input() articleId: number;

  @Output() navigate: EventEmitter<number> = new EventEmitter();

  navigationTarget = NavigationTarget;

  constructor(private http: HttpClient, @Inject("BASE_URL") private baseUrl: string) { }

  navigateTo(target: NavigationTarget) {
    let navigation: INavigation = {
      ArticleId: this.articleId,
      Target: target
    };

    this.http.post<number>(`${this.baseUrl}articles/navigate`, navigation).subscribe(
      position => this.navigate.emit(position),
      error => console.error(error));
  }
}
