import { Component, EventEmitter, OnInit, Output } from "@angular/core";
import { NgForm } from "@angular/forms";
import { IArticle } from "../models/IArticle";
import { IDefaultSettings } from "../models/IDefaultSettings";
import { ArrayHelperService } from "../shared/services/array-helper.service";
import { HttpClientService } from "../shared/services/http-client.service";
import { ArticleState } from "../models/ArticleState";

@Component({
  selector: "articles-create",
  templateUrl: "./articles-create.component.html"
})
export class ArticlesCreateComponent implements OnInit {
  newArticleTitle: string;
  newArticleText: string;

  prefixLength: string;
  maxChoices: string;
  ignoreCase = false;
  ignorePunctuation = false;

  prefixLengthOptions: string[];
  maxChoicesOptions: string[];

  showAdvancedOptions: boolean;

  @Output() close: EventEmitter<IArticle> = new EventEmitter();
  @Output() update: EventEmitter<void> = new EventEmitter();

  constructor(private http: HttpClientService, private arrayHelper: ArrayHelperService) { }

  ngOnInit() {
    this.http.get<IDefaultSettings>("articles/defaultsettings",
      data => {
        this.prefixLength = data.prefixLength.toString();
        this.prefixLengthOptions = this.arrayHelper.range(data.prefixLengthMinOption, data.prefixLengthMaxOption).map(x => x.toString());
        this.maxChoicesOptions = this.arrayHelper.range(data.maxChoicesMinOption, data.maxChoicesMaxOption).map(x => x.toString());
        this.maxChoices = data.maxChoices.toString();
      });
  }

  add(form: NgForm) {
    if (form.valid) {
      let article: IArticle = {
        title: this.newArticleTitle,
        text: this.newArticleText,
        prefixLength: parseInt(this.prefixLength),
        maxChoices: parseInt(this.maxChoices),
        ignoreCase: this.ignoreCase,
        ignorePunctuation: this.ignorePunctuation,
        state: ArticleState.Processing
      };

      this.http.post<IArticle>("articles", article, () => this.update.emit());

      this.reset(form);
      this.close.emit(article);
    } else {
      form.control.markAllAsTouched();
    }
  }

  cancel(form: NgForm) {
    this.reset(form);
    this.close.emit();
  }

  toggleAdvancedOptions() {
    this.showAdvancedOptions = !this.showAdvancedOptions;
  }

  private reset(form: NgForm) {
    this.newArticleTitle = "";
    this.newArticleText = "";

    this.showAdvancedOptions = false;

    form.control.markAsPristine();
    form.control.markAsUntouched();
  }
}
