export interface IArticle {
  id?: number;
  title: string;
  text: string;
  prefixLength: number;
  maxChoices: number;
  ignoreCase: boolean;
}
