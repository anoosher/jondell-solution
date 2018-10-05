import { Component, OnInit } from '@angular/core';
import { LatestMonthlyBalances } from 'src/app/balance-viewer/beans';
import { AppService } from 'src/app/app.service';

@Component({
  selector: 'app-balance-viewer',
  templateUrl: './balance-viewer.component.html',
  styleUrls: ['./balance-viewer.component.css']
})
export class BalanceViewerComponent implements OnInit {

  latestMonthlyBalance: LatestMonthlyBalances= {month:"",accountBalances:null};
  displayedColumns: string[] = ['account', 'balance'];

  constructor(private appService: AppService) { }

  ngOnInit() {

    this.appService.getLatestAccountBalances().subscribe(response => {
      this.latestMonthlyBalance.month= (<any>response).displayMonth;
      this.latestMonthlyBalance.accountBalances= (<any>response).monthlyBalanceViewModels;
      console.log(this.latestMonthlyBalance);
    }, err => {

    });
  }

 
}



