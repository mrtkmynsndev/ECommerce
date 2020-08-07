import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AccountService } from '../../services/account.service';
import { Router } from '@angular/router';
import { timer, of, Observable } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;
  errors: string[];

  constructor(
    private fb: FormBuilder,
    private accountService: AccountService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.initRegisterForm();
  }

  initRegisterForm(): void {
    this.registerForm = this.fb.group({
      displayName: [null, [Validators.required]],
      userName: [null, [Validators.required]],
      email: [null,
        [Validators.required, Validators.email, Validators.pattern('^\\w+[\\w-\\.]*\\@\\w+((-\\w+)|(\\w*))\.[a-z]{2,3}$')],
        [this.validateEmailNotTaken()]
      ],
      password: [null, [Validators.required,
      Validators.pattern('(?=^.{6,255}$)((?=.*\\d)(?=.*[A-Z])(?=.*[a-z])|(?=.*\\d)' +
        '(?=.*[^A-Za-z0-9])(?=.*[a-z])|(?=.*[^A-Za-z0-9])(?=.*[A-Z])(?=.*[a-z])|(?=.*\\d)(?=.*[A-Z])(?=.*[^A-Za-z0-9]))^.*')]],
    });
  }

  onSubmit(): void {
    this.accountService.register(this.registerForm.value).subscribe(result => {
      this.router.navigateByUrl('/shop');
    },
      error => {
        this.errors = error.errors;
      });
  }

  validateEmailNotTaken(): any {
    return (control: { value: string; }) => {
      return timer(0).pipe(
        switchMap(() => {
          if (!control.value) {
            return of(null);
          }
          return this.accountService.checkEmailExist(control.value).pipe(
            map(res => {
              return res ? { exists: true } : null;
            })
          );
        })
      );
    };
  }
}
