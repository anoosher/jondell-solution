import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { HttpHeaders } from '@angular/common/http';
import { Credentials } from 'src/app/login/beans';


@Injectable()
export class AppService {

  baseUrl: string = "http://jondellaccountserviceapi.azurewebsites.net/api";

  constructor(private http: HttpClient, ) { }

  login(credentials: Credentials): Observable<any> {

    return this.http.post(this.baseUrl + "/auth/login", credentials, {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    });


  }

  getLatestAccountBalances(): Observable<any> {
    let token = localStorage.getItem("jwt");
    return this.http.get(this.baseUrl + "/monthlyBalance/getLatestBalances", {
      headers: new HttpHeaders({
        "Content-Type": "application/json",
        "Authorization": "Bearer " + token
      })
    });
  }

  getAccountBalancesHistory(): Observable<any> {
    let token = localStorage.getItem("jwt");
    return this.http.get(this.baseUrl + "/monthlyBalance", {
      headers: new HttpHeaders({
        "Content-Type": "application/json",
        "Authorization": "Bearer " + token
      })
    });


  }
}