import { Component, EventEmitter, OnInit, Output } from "@angular/core";
import { NgForm } from "@angular/forms";
import { IArticle } from "../models/IArticle";
import { IDefaultSettings } from "../models/IDefaultSettings";
import { ArrayHelperService } from "../shared/services/array-helper.service";
import { HttpClientService } from "../shared/services/http-client.service";

@Component({
  selector: "articles-create",
  templateUrl: "./articles-create.component.html"
})
export class ArticlesCreateComponent implements OnInit {
  newArticleTitle: string;
  newArticleText: string;

  prefixLength: string;
  maxChoices: string;

  prefixLengthOptions: string[];
  maxChoicesOptions = ["3", "4", "5"];

  @Output()
  close: EventEmitter<IArticle> = new EventEmitter();

  constructor(private http: HttpClientService, private arrayHelper: ArrayHelperService) { }

  ngOnInit() {
    this.http.get<IDefaultSettings>("articles/defaultsettings",
      data => {
        this.prefixLength = data.prefixLength.toString();
        this.prefixLengthOptions = this.arrayHelper.range(data.prefixLengthMinOption, data.prefixLengthMaxOption).map(x => x.toString());
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
