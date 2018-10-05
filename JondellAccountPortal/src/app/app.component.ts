import { Component } from '@angular/core';
import { AuthGuard } from 'src/app/auth-guard.service'
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  constructor(private authGuard: AuthGuard,private router: Router,) { }

  title = 'Jondell Account Portal';
  username = localStorage.getItem("username");

  isLoggedIn() {
    return this.authGuard.canActivateWithoutNavigation();
  }

  logOut() {
    localStorage.removeItem("jwt");
    this.router.navigate(["/login"]);
  }


}


