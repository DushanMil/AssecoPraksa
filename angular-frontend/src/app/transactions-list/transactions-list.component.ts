import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { TransactionsService } from '../services/transactions.service';
import { TransactionPagedList } from '../models/transactionPagedList';
import { TransactionWithSplits } from '../models/transactionWithSplits';

@Component({
  selector: 'app-transactions-list',
  templateUrl: './transactions-list.component.html',
  styleUrls: ['./transactions-list.component.css']
})
export class TransactionsListComponent {

  constructor(private transactionService: TransactionsService, private router: Router) { }

  transactionPagedList: TransactionPagedList = new TransactionPagedList()

  kinds: string = ""
  startDate: string = ""
  endDate: string = ""
  page: number = 1
  pageSize: number = 10;
  sortBy: string = ""
  sortOrder: string = ""

  ngOnInit(): void {
    this.transactionService.getTransactions("", "", "", 1, 10, "", "asc").subscribe((resp: TransactionPagedList) => {
      this.transactionPagedList = resp;
    })
  }

  categorize(t: TransactionWithSplits) {
    localStorage.setItem("transaction", JSON.stringify(t))
    this.router.navigate(['categorize']);
  }

  filterTransactions() {
    this.transactionService.getTransactions(this.kinds, this.startDate, this.endDate, this.page, this.pageSize, this.sortBy, this.sortOrder).subscribe((resp: TransactionPagedList) => {
      console.log(this.transactionPagedList)
      this.transactionPagedList = resp;
    })
  }

}
