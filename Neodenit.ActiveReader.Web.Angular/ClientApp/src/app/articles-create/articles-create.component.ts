import { Component, EventEmitter, OnInit, Output, ViewChild } from "@angular/core";
import { NgForm } from "@angular/forms";
import { ArticleState } from "../models/ArticleState";
import { IArticle } from "../models/IArticle";
import { IDefaultSettings } from "../models/IDefaultSettings";
import { ArrayHelperService } from "../shared/services/array-helper.service";
import { HttpClientService } from "../shared/services/http-client.service";

@Component({
  selector: "articles-create",
  templateUrl: "./articles-create.component.html"
})
export class ArticlesCreateComponent implements OnInit {
  @ViewChild("form", { static: false }) form: NgForm;

  newArticleTitle: string;
  newArticleText: string;

  prefixLength: string;
  maxChoices: string;
  ignoreCase: boolean;
  ignorePunctuation: boolean;

  defaultArticleTitle = "";
  defaultArticleText = "";
  defaultPrefixLength: string;
  defaultMaxChoices: string;
  defaultIgnoreCaseState = false;
  defaultIgnorePunctuationState = false;

  prefixLengthOptions: string[];
  maxChoicesOptions: string[];

  showAdvancedOptions: boolean;

  @Output() close: EventEmitter<IArticle> = new EventEmitter();
  @Output() update: EventEmitter<void> = new EventEmitter();

  constructor(private http: HttpClientService, private arrayHelper: ArrayHelperService) { }

  ngOnInit() {
    this.http.get<IDefaultSettings>("articles/defaultsettings",
      data => {
        this.prefixLengthOptions = this.arrayHelper.range(data.prefixLengthMinOption, data.prefixLengthMaxOption).map(x => x.toString());
        this.maxChoicesOptions = this.arrayHelper.range(data.maxChoicesMinOption, data.maxChoicesMaxOption).map(x => x.toString());

        this.defaultPrefixLength = data.prefixLength.toString();
        this.defaultMaxChoices = data.maxChoices.toString();

        this.reset();
      });
  }

  add() {
    if (this.form.valid) {
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

      this.reset();
      this.close.emit(article);
    } else {
      this.form.control.markAllAsTouched();
    }
  }

  cancel() {
    this.reset();
    this.close.emit();
  }

  toggleAdvancedOptions() {
    this.showAdvancedOptions = !this.showAdvancedOptions;
  }

  private reset() {
    this.newArticleTitle = this.defaultArticleTitle;
    this.newArticleText = this.defaultArticleText;

    this.prefixLength = this.defaultPrefixLength;
    this.maxChoices = this.defaultMaxChoices;

    this.ignoreCase = this.defaultIgnoreCaseState;
    this.ignorePunctuation = this.defaultIgnorePunctuationState;

    this.showAdvancedOptions = false;

    this.form.control.markAsPristine();
    this.form.control.markAsUntouched();
  }
}
