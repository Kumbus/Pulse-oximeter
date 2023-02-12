import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { RegistrationResponseDto } from '../Interfaces/registrationResponseDto';
import { UserForRegisterDto } from '../Interfaces/userForRegisterDto';
import { UserForLoginDto } from '../Interfaces/userForLoginDto';
import { LoginResponseDto } from '../Interfaces/loginResponseDto';
import { HttpClient } from '@angular/common/http';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private authChangeSub = new Subject<boolean>()
  public authChanged = this.authChangeSub.asObservable();

  apiUrl = "https://localhost:7212/api/Users/"

  constructor(private http: HttpClient, private jwtHelper: JwtHelperService) { }

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




}
