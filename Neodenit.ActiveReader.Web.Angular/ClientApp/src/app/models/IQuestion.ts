export interface IQuestion {
  answerPosition: number;
  progress: number;
  startingText: string;
  newText: string;
  choices: string[];
  correctAnswer: string;
}
