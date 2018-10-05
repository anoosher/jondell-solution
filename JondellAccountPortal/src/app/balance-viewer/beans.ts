export interface LatestMonthlyBalances {
    month: string;
    accountBalances: Array<MonthlyBalanceViewModels>; 
}

export interface MonthlyBalanceViewModels {
    displayAccountName: string;
    displayAccountBalance: string;
    order: number;
  }