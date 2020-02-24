import { Injectable } from "@angular/core";

@Injectable({
  providedIn: "root"
})
export class ArrayHelperService {
  range(minElement: number, maxElement: number): number[] {
    return [...Array(maxElement - minElement + 1).keys()].map(x => x + minElement);
  }
}
