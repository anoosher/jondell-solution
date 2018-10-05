import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { HttpHeaders } from '@angular/common/http';
import { HttpClient } from '@angular/common/http';
import { AppService } from 'src/app/app.service';
import { Credentials, LoginResponse } from 'src/app/login/beans';
import { Observable } from 'rxjs/internal/Observable';
import { Input } from '@angular/core';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {


  username: string;
  password: string;
  loginResp : LoginResponse={token: "", invalidLogin:false};
  

  constructor( private router: Router, private appService : AppService) { }

  
 
  ngOnInit() {
    
  }

  login ()
  {
    let creds : Credentials = { Password:this.password,UserName:this.username};
    this.loginResp = {token: "", invalidLogin:false};

     this.appService.login(creds).subscribe(response => {

      let token = (<any>response).token;
      let displayName = (<any>response).displayName;
      let role = (<any>response).role;
      this.loginResp.token = token;
      this.loginResp.invalidLogin = false;

      localStorage.setItem("jwt", this.loginResp.token); 
      localStorage.setItem("username", displayName); 
      localStorage.setItem("role", role);  
      
      this.router.navigate(["/dashboard"]);

      
    }, err => {
      this.loginResp.invalidLogin = true;
    });

   
  }

  
}
