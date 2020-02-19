import { Component, EventEmitter, OnInit, Output } from "@angular/core";
import { NgForm } from "@angular/forms";
import { IArticle } from "../models/IArticle";
import { HttpClientService } from "../shared/services/http-client.service";
import { IDefaultSettings } from "../models/IDefaultSettings";

@Component({
  selector: "articles-create",
  templateUrl: "./articles-create.component.html"
})
export class ArticlesCreateComponent implements OnInit {
  newArticleTitle: string;
  newArticleText: string;

  prefixLength: string;
  maxChoices: string;

  prefixLengthOptions = ["1", "2", "3"];
  maxChoicesOptions = ["3", "4", "5"];

  @Output()
  close: EventEmitter<IArticle> = new EventEmitter();

  constructor(private http: HttpClientService) { }

  ngOnInit() {
    this.http.get<IDefaultSettings>("articles/defaultsettings",
      data => {
        this.prefixLength = data.prefixLength.toString();
        this.maxChoices = data.maxChoices.toString();
      });
  }

  add(form: NgForm) {
    if (form.valid) {
      let article: IArticle = {
        title: this.newArticleTitle,
        text: this.newArticleText,
        prefixLength: parseInt(this.prefixLength),
        maxChoices: parseInt(this.maxChoices)
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
