import { Component, Inject, Input, Output, EventEmitter } from "@angular/core";
import { HttpClient } from "@angular/common/http";

@Component({
  selector: "articles-navigation",
  templateUrl: "./articles-navigation.component.html"
})
export class ArticlesNavigationComponent {

  @Input() articleId: number;

  @Output() navigate: EventEmitter<number> = new EventEmitter();

  constructor(private http: HttpClient, @Inject("BASE_URL") private baseUrl: string) { }

  navigateTo() {
    this.http.post<number>(`${this.baseUrl}articles/navigate`, this.articleId).subscribe(
      position => this.navigate.emit(position),
      error => console.error(error));
  }
}
