import { Injectable, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";

@Injectable({
  providedIn: "root"
})
export class HttpClientService {
  constructor(private http: HttpClient, @Inject("BASE_URL") private baseUrl: string) { }

  get<T>(localUrl: string, callback: (data: T) => void) {
    this.http.get<T>(`${this.baseUrl}${localUrl}`).subscribe(
      data => callback && callback(data),
      error => this.handleError(error));
  }

  post<T>(localUrl: string, body: any, callback: (data: T) => void, failCallback: (data: any) => void = null) {
    this.http.post<T>(`${this.baseUrl}${localUrl}`, body).subscribe(
      data => callback && callback(data),
      error => failCallback ? failCallback(error) : this.handleError(error));
  }

  put<T>(localUrl: string, body: any, callback: (data: void) => void) {
    this.http.put<T>(`${this.baseUrl}${localUrl}`, body).subscribe(
      () => callback && callback(),
      error => this.handleError(error));
  }

  delete(localUrl: string, callback: (data: void) => void) {
    this.http.delete(`${this.baseUrl}${localUrl}`).subscribe(
      () => callback && callback(),
      error => this.handleError(error));
  }

  private handleError(error) {
    console.error(error);
  }
}
