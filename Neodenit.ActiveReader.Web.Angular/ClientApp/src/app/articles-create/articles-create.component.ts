import { Component, EventEmitter, OnInit, Output } from "@angular/core";
import { NgForm } from "@angular/forms";
import { IArticle } from "../models/IArticle";
import { HttpClientService } from "../shared/services/http-client.service";

@Component({
  selector: "articles-create",
  templateUrl: "./articles-create.component.html"
})
export class ArticlesCreateComponent implements OnInit {
  newArticleTitle: string;
  newArticleText: string;
  prefixLength: string;
  prefixLengthOptions = ["1", "2", "3"];

  @Output()
  close: EventEmitter<IArticle> = new EventEmitter();

  constructor(private http: HttpClientService) { }

  ngOnInit() {
    this.http.get<number>("articles/defaultprefixlength",
      data => this.prefixLength = data.toString());
  }

  add(form: NgForm) {
    if (form.valid) {
      let article: IArticle = {
        title: this.newArticleTitle,
        text: this.newArticleText,
        prefixLength: parseInt(this.prefixLength)
      };

      this.http.post<IArticle>("articles", article, newArticle => this.close.emit(newArticle));
    } else {
      form.control.markAllAsTouched();
    }
  }

  cancel() {
    this.close.emit(null);
  }
}
