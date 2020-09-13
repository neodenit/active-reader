export interface IQuestion {
  answerPosition: number;
  lastPosition: number;
  progress: number;
  startingText: string;
  newText: string;
  choices: string[];
  correctAnswer: string;
}
