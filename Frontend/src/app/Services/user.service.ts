import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { RegistrationResponseDto } from '../Interfaces/registrationResponseDto';
import { UserForRegisterDto } from '../Interfaces/userForRegisterDto';
import { UserForLoginDto } from '../Interfaces/userForLoginDto';
import { LoginResponseDto } from '../Interfaces/loginResponseDto';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { JwtHelperService } from '@auth0/angular-jwt';
import { GoogleLoginProvider, SocialAuthService, SocialUser } from '@abacritt/angularx-social-login';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private authChangeSub = new Subject<boolean>();
  private extAuthChangeSub = new Subject<SocialUser>();
  public authChanged = this.authChangeSub.asObservable();
  public extAuthChanged = this.extAuthChangeSub.asObservable();


  apiUrl = "https://localhost:7212/api/Users/"

  constructor(private http: HttpClient, private jwtHelper: JwtHelperService, private externalAuthService: SocialAuthService, private router: Router) {}

  public register = (user: UserForRegisterDto) => {
    return this.http.post<RegistrationResponseDto> (`${this.apiUrl}register`, user);
  }

  public login = (user: UserForLoginDto) => {
    return this.http.post<LoginResponseDto> (`${this.apiUrl}login`, user);
  }

  public sendAuthStateChangeNotification = (isAuthenticated: boolean) => {
    this.authChangeSub.next(isAuthenticated);
  }

  public logout = () => {
    localStorage.removeItem("token");
    this.sendAuthStateChangeNotification(false);
  }

  public isUserAuthenticated = (): boolean => {
    const token = localStorage.getItem("token");

    return token!=null && !this.jwtHelper.isTokenExpired(token);
  }

  public signInWithGoogle = ()=> {

    this.externalAuthService.authState.subscribe((user) => {
      this.extAuthChangeSub.next(user);

      const externalAuth = {
        provider: user.provider,
        idToken: user.idToken
      }

      console.log(externalAuth)


     this.externalLogin(externalAuth)
     .subscribe({
       next: (res) => {
            console.log(res)
            localStorage.setItem("token", res.token);
            this.sendAuthStateChangeNotification(res.isAuthSuccessful);
            this.router.navigate(['/results'])
     },
       error: (err: HttpErrorResponse) => {
          console.log(err)
       }
     });
    })
  }



  public externalLogin = (body: object) => {
    console.log(body)
    return this.http.post<LoginResponseDto>(`${this.apiUrl}googleLogin`, body);
  }




}
