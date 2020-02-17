import { Component, EventEmitter, Input, Output } from "@angular/core";
import { INavigation } from "../models/INavigation";
import { NavigationTarget } from "../models/NavigationTarget";
import { HttpClientService } from "../shared/services/http-client.service";

@Component({
  selector: "articles-navigation",
  templateUrl: "./articles-navigation.component.html"
})
export class ArticlesNavigationComponent {

  @Input() articleId: number;

  @Output() navigate: EventEmitter<number> = new EventEmitter();

  navigationTarget = NavigationTarget;

  constructor(private http: HttpClientService) { }

  navigateTo(target: NavigationTarget) {
    let navigation: INavigation = {
      ArticleId: this.articleId,
      Target: target
    };

    this.http.post<number>("articles/navigate",
      navigation,
      position => this.navigate.emit(position));
  }
}
