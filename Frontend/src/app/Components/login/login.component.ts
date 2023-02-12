import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { LoginResponseDto } from 'src/app/Interfaces/loginResponseDto';
import { UserForLoginDto } from 'src/app/Interfaces/userForLoginDto';
import { UserService } from 'src/app/Services/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  loginForm: FormGroup;
  errorMessage: string = '';
  showError: boolean = false;

  constructor(private _userService: UserService, private router: Router, private route: ActivatedRoute) {
    this.loginForm = new FormGroup({
      username: new FormControl("", [Validators.required]),
      password: new FormControl("", [Validators.required])
    })
  }


  validateControl = (controlName: string) => {
    return this.loginForm.get(controlName)!.invalid && this.loginForm.get(controlName)!.touched
  }

  hasError = (controlName: string, errorName: string) => {
    return this.loginForm.get(controlName)!.hasError(errorName)
  }

  loginUser = (loginFormValue: any) => {
    this.showError = false;
    const login = {... loginFormValue };

    const userForAuth: UserForLoginDto = {
      userName: login.username,
      password: login.password
    }

    this._userService.login(userForAuth)
    .subscribe({
      next: (res:LoginResponseDto) => {
       localStorage.setItem("token", res.token);
       this._userService.sendAuthStateChangeNotification(res.isAuthSuccessful);
       this.router.navigate(["/results"]);
    },
    error: (err: HttpErrorResponse) => {
      this.errorMessage = err.message;
      this.showError = true;
    }})
  }
}
