import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ProjectService } from 'src/app/_services/project.service';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Project } from 'src/app/_models/project.model';
import { AlertifyService } from 'src/app/_services/Alertify.service';

@Component({
  selector: 'app-new-proj',
  templateUrl: './new-proj.component.html',
  styleUrls: ['./new-proj.component.scss']
})
export class NewProjComponent implements OnInit {

  constructor(private route: ActivatedRoute,
              private projService: ProjectService,
              private formBuilder: FormBuilder,
              private alertifyService: AlertifyService) { }
  createForm: FormGroup;
  // inviteMemebersF
  ngOnInit() {
    console.log(this.route.snapshot.parent.params.orgName);
    this.createForm = this.formBuilder.group({
      name: ['', Validators.required]
    });
    // this.inviteMemebersForm = this.formBuilder.group({
    //   userName: ['', Validators.required]
    // });
  }

  create() {
    if (this.createForm.valid) {
      const model: Project = Object.assign(
        {},
        this.createForm.value
      );
      this.projService.create(this.route.snapshot.parent.params.orgName, model).subscribe(res => {
        this.alertifyService.success(res.message);
      }, err => {
        this.alertifyService.error(err);
      });
    }

  }
}
