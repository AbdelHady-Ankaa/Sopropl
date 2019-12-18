import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.scss']
})
export class AccountComponent implements OnInit {
  constructor(private route: ActivatedRoute, private titleService: Title) {}

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.titleService.setTitle(data.title);
    });
  }
}
