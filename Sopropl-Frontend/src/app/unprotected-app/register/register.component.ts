import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AuthService } from 'src/app/_services/auth.service';
import { AlertifyService } from 'src/app/_services/Alertify.service';
import { ActivatedRoute } from '@angular/router';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;

  constructor(
    private auth: AuthService,
    private formBuilder: FormBuilder,
    private alertify: AlertifyService,
    private route: ActivatedRoute,
    private titleService: Title
  ) {}

  createRegisterForm() {
    this.registerForm = this.formBuilder.group(
      {
        userName: [
          '',
          [
            Validators.required,
            Validators.minLength(1),
            Validators.maxLength(40),
            Validators.pattern('^(?=.*[a-zA-Z])(?=.*[0-9])[a-zA-Z0-9]+$')
          ]
        ],
        email: ['', [Validators.required, Validators.email]],
        password: [
          '',
          [
            Validators.required,
            Validators.minLength(8),
            Validators.maxLength(50),
            Validators.pattern(
              '^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])[a-zA-Z0-9]+$'
            )
          ]
        ],
        confirmPassword: ['', [Validators.required]]
      },
      {
        validator: (form: FormGroup) => {
          return form.get('password').value ===
            form.get('confirmPassword').value
            ? null
            : { mismatch: true };
        }
      }
    );
  }
  register() {
    if (this.registerForm.valid) {
      const user: {
        userName: string;
        password: string;
        email: string;
      } = Object.assign({}, this.registerForm.value);

      this.auth.reigster(user).subscribe(
        () => {
          this.alertify.success('registered successfully');
        },
        error => {
          this.alertify.error(error);
        },
        () => {
          this.auth.login(user);
        }
      );
    }
  }
  ngOnInit() {
    this.route.data.subscribe(data => {
      this.titleService.setTitle(data.title);
    });
    this.createRegisterForm();
  }
}
