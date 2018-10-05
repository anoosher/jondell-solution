import { Component, OnInit } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { AppService } from 'src/app/app.service';
import { AuthGuard } from 'src/app/auth-guard.service';

@Component({
  selector: 'app-content',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
})
export class DashboardComponent implements OnInit {

  constructor(private jwtHelper: JwtHelperService, private appService: AppService,private authGuard: AuthGuard) { }

  role: String;
  responseList = [];

  ngOnInit() {

    this.role = localStorage.getItem("role");

    if(this.role=='Admin')
    {
      var lineData:Array<any> = [
        {data: [], label: 'R&D'},
        {data: [], label: 'Canteen'},
        {data: [], label: 'CEO’s car'},
        {data: [], label: 'Marketing'},
        {data: [], label: 'Parking fines'}];
  
       var lineLabels:Array<any>= []; 
  
      this.appService.getAccountBalancesHistory().subscribe(response => {
  
        this.responseList = (<any>response);
  
        for (let entry of this.responseList) {
          lineLabels.push(entry.displayMonth);
  
          for (let insideEntry of entry.monthlyBalanceViewModels) {
  
              if(insideEntry.displayAccountName=='R&D')
              {
                lineData[0].data.push(+insideEntry.displayAccountBalance);
              }
              else if (insideEntry.displayAccountName=='Canteen')
              {
                lineData[1].data.push(+insideEntry.displayAccountBalance);
              }
              else if (insideEntry.displayAccountName=='CEO’s car')
              {
                lineData[2].data.push(+insideEntry.displayAccountBalance);
              }
              else if (insideEntry.displayAccountName=='Marketing')
              {
                lineData[3].data.push(+insideEntry.displayAccountBalance);
              }
              else if (insideEntry.displayAccountName=='Parking fines')
              {
                lineData[4].data.push(+insideEntry.displayAccountBalance);
              }
          }
  
        }

        localStorage.setItem("lineChardData", JSON.stringify(lineData));
        localStorage.setItem("lineChardLabels", JSON.stringify(lineLabels));
      }, err => {
  
      });
  
    }

  }



}
