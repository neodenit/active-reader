import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges, ViewChild } from "@angular/core";
import { NgForm } from "@angular/forms";
import { ArticleState } from "../models/ArticleState";
import { IArticle } from "../models/IArticle";
import { IDefaultSettings } from "../models/IDefaultSettings";
import { ArrayHelperService } from "../shared/services/array-helper.service";
import { HttpClientService } from "../shared/services/http-client.service";
import { IImportArticle } from "../models/IImportArticle";

@Component({
  selector: "articles-create",
  templateUrl: "./articles-create.component.html"
})
export class ArticlesCreateComponent implements OnInit, OnChanges {
  @ViewChild("form", { static: false }) form: NgForm;

  pageTitle: string;

  readonly createPageTitle = "Add New Article";
  readonly updatePageTitle = "Edit Article";

  newArticleTitle: string;
  newArticleText: string;
  newArticleUrl: string;

  prefixLength: string;
  answerLength: string;
  maxChoices: string;
  ignoreCase: boolean;
  ignorePunctuation: boolean;

  readonly defaultArticleUrl = "";
  readonly defaultArticleTitle = "";
  readonly defaultArticleText = "";
  defaultPrefixLength: string;
  defaultAnswerLength: string;
  defaultMaxChoices: string;
  defaultIgnoreCaseState: boolean;
  defaultIgnorePunctuationState: boolean;

  prefixLengthOptions: string[];
  answerLengthOptions: string[];
  maxChoicesOptions: string[];

  showAdvancedOptions: boolean;

  @Input() articleId: number;

  @Output() close: EventEmitter<IArticle> = new EventEmitter();

  constructor(private http: HttpClientService, private arrayHelper: ArrayHelperService) { }

  ngOnInit() {
    this.http.get<IDefaultSettings>("articles/defaultsettings",
      data => {
        this.prefixLengthOptions = this.arrayHelper.range(data.prefixLengthMinOption, data.prefixLengthMaxOption).map(x => x.toString());
        this.answerLengthOptions = this.arrayHelper.range(data.answerLengthMinOption, data.answerLengthMaxOption).map(x => x.toString());
        this.maxChoicesOptions = this.arrayHelper.range(data.maxChoicesMinOption, data.maxChoicesMaxOption).map(x => x.toString());

        this.defaultPrefixLength = data.prefixLength.toString();
        this.defaultAnswerLength = data.answerLength.toString();
        this.defaultMaxChoices = data.maxChoices.toString();
        this.defaultIgnoreCaseState = data.ignoreCaseState;
        this.defaultIgnorePunctuationState = data.ignorePunctuationState;

        this.reset();
      });
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.articleId.currentValue) {
      this.pageTitle = this.updatePageTitle;

      this.http.get<IArticle>(`articles/${this.articleId}`,
        article => {
          this.newArticleText = article.text;
          this.newArticleTitle = article.title;

          this.prefixLength = article.prefixLength.toString();
          this.answerLength = article.answerLength.toString();
          this.maxChoices = article.maxChoices.toString();
          this.ignoreCase = article.ignoreCase;
          this.ignorePunctuation = article.ignorePunctuation;
        });
    } else {
      this.pageTitle = this.createPageTitle;
    }
  }

  save() {
    if (this.form.valid) {
      if (this.articleId) {
        this.updateArticle();
      } else {
        this.createArticle();
      }
    } else {
      this.form.control.markAllAsTouched();
    }
  }

  import() {
    if (this.newArticleUrl) {
      const encoddedUrl = encodeURIComponent(this.newArticleUrl);

      this.http.get<IImportArticle>(`articles/import?url=${encoddedUrl}`, article => {
        this.newArticleTitle = article.title;
        this.newArticleText = article.text;
      });
    }
  }

  createArticle() {
    const article: IArticle = {
      title: this.newArticleTitle,
      text: this.newArticleText,
      prefixLength: parseInt(this.prefixLength),
      answerLength: parseInt(this.answerLength),
      maxChoices: parseInt(this.maxChoices),
      ignoreCase: this.ignoreCase,
      ignorePunctuation: this.ignorePunctuation,
      state: ArticleState.Processing
    };

    this.http.post<IArticle>("articles", article);

    this.reset();
    this.close.emit(article);
  }

  updateArticle() {
    const article: IArticle = {
      id: this.articleId,
      title: this.newArticleTitle,
      text: this.newArticleText,
      prefixLength: parseInt(this.prefixLength),
      answerLength: parseInt(this.answerLength),
      maxChoices: parseInt(this.maxChoices),
      ignoreCase: this.ignoreCase,
      ignorePunctuation: this.ignorePunctuation,
      state: ArticleState.Processing
    };

    this.http.put<IArticle>("articles", article);

    this.reset();
    this.close.emit(article);
  }

  cancel() {
    this.reset();
    this.close.emit();
  }

  toggleAdvancedOptions() {
    this.showAdvancedOptions = !this.showAdvancedOptions;
  }

  private reset() {
    this.newArticleUrl = this.defaultArticleUrl;

    this.newArticleTitle = this.defaultArticleTitle;
    this.newArticleText = this.defaultArticleText;

    this.prefixLength = this.defaultPrefixLength;
    this.answerLength = this.defaultAnswerLength;
    this.maxChoices = this.defaultMaxChoices;

    this.ignoreCase = this.defaultIgnoreCaseState;
    this.ignorePunctuation = this.defaultIgnorePunctuationState;

    this.showAdvancedOptions = false;

    this.form.control.markAsPristine();
    this.form.control.markAsUntouched();
  }
}
