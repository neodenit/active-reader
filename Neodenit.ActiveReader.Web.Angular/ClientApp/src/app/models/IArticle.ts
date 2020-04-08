import { ArticleState } from "./ArticleState";

export interface IArticle {
  id?: number;
  title: string;
  text: string;
  prefixLength: number;
  answerLength: number;
  maxChoices: number;
  ignoreCase: boolean;
  ignorePunctuation: boolean;
  state: ArticleState;
}
