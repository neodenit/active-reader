import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthorizeService } from '../../api-authorization/authorize.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {
  isAuthenticated: Observable<boolean>;

  constructor(private authorizeService: AuthorizeService) { }

  ngOnInit() {
    this.isAuthenticated = this.authorizeService.isAuthenticated();
  }
}
