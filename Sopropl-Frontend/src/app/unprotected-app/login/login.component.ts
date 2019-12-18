import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/_services/auth.service';
import { ActivatedRoute } from '@angular/router';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  model: any = {};
  constructor(
    private auth: AuthService,
    private route: ActivatedRoute,
    private titleService: Title
  ) {}

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.titleService.setTitle(data.title);
    });
  }

  login() {
    this.auth.login(this.model);
  }
}
