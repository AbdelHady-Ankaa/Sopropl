import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Graph } from 'src/app/_models/graph.model';

@Component({
  selector: 'app-graph',
  templateUrl: './graph.component.html',
  styleUrls: ['./graph.component.scss']
})
export class GraphComponent implements OnInit {

  graph: Graph;

  constructor(private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(res => {
      this.graph = res.graph;
    });
  }
}
