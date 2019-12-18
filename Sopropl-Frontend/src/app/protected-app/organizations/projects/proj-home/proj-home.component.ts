import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-proj-home',
  templateUrl: './proj-home.component.html',
  styleUrls: ['./proj-home.component.scss']
})
export class ProjHomeComponent implements OnInit {

  constructor(private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(res => {
      console.log(res);
    });
  }

}
