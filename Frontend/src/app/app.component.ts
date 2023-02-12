import { Component } from '@angular/core';
import { UserService } from './Services/user.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'Frontend';

  constructor(private _userService: UserService){}

  ngOnInit(): void {
    if(this._userService.isUserAuthenticated())
      this._userService.sendAuthStateChangeNotification(true);
  }
}
