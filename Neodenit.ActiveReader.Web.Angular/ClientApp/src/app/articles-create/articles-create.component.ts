import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges, ViewChild } from "@angular/core";
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
export class ArticlesCreateComponent implements OnInit, OnChanges {
  @ViewChild("form", { static: false }) form: NgForm;

  pageTitle: string;

  readonly createPageTitle = "Add New Article";
  readonly updatePageTitle = "Edit Article";

  newArticleTitle: string;
  newArticleText: string;

  prefixLength: string;
  answerLength = "1";
  maxChoices: string;
  ignoreCase: boolean;
  ignorePunctuation: boolean;

  readonly defaultArticleTitle = "";
  readonly defaultArticleText = "";
  defaultPrefixLength: string;
  defaultMaxChoices: string;
  readonly defaultIgnoreCaseState = false;
  readonly defaultIgnorePunctuationState = false;

  prefixLengthOptions: string[];
  maxChoicesOptions: string[];

  showAdvancedOptions: boolean;

  @Input() articleId: number;

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

  ngOnChanges(changes: SimpleChanges) {
    if (changes.articleId.currentValue) {
      this.pageTitle = this.updatePageTitle;

      this.http.get<IArticle>(`articles/${this.articleId}`,
        article => {
          this.newArticleText = article.text;
          this.newArticleTitle = article.title;

          this.prefixLength = article.prefixLength.toString();
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

  createArticle() {
    let article: IArticle = {
      title: this.newArticleTitle,
      text: this.newArticleText,
      prefixLength: parseInt(this.prefixLength),
      answerLength: parseInt(this.answerLength),
      maxChoices: parseInt(this.maxChoices),
      ignoreCase: this.ignoreCase,
      ignorePunctuation: this.ignorePunctuation,
      state: ArticleState.Processing
    };

    this.http.post<IArticle>("articles", article, () => this.update.emit(), () => this.update.emit());

    this.reset();
    this.close.emit(article);
  }

  updateArticle() {
    let article: IArticle = {
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

    this.http.put<IArticle>("articles", article, () => this.update.emit());

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
