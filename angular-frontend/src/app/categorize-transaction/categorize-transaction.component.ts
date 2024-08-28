import { Component } from '@angular/core';
import { TransactionsService } from '../services/transactions.service';
import { Router } from '@angular/router';
import { TransactionWithSplits } from '../models/transactionWithSplits';
import { Category } from '../models/category';
import { CategoryList } from '../models/categoryList';
import { HttpResponse } from '@angular/common/http';

@Component({
  selector: 'app-categorize-transaction',
  templateUrl: './categorize-transaction.component.html',
  styleUrls: ['./categorize-transaction.component.css']
})
export class CategorizeTransactionComponent {

  constructor(private transactionService: TransactionsService, private router: Router) { }

  t: TransactionWithSplits = new TransactionWithSplits()
  topLevelCategrories: CategoryList = new CategoryList()

  selectedCategory: string = ""
  selectedSubCategory: string = ""

  lowLevelCategories: CategoryList = new CategoryList()


  ngOnInit(): void {
    let temp = localStorage.getItem('transaction');
    if (temp != null) {
      this.t = JSON.parse(temp);
    }

    this.transactionService.getAllCategories("").subscribe((resp : CategoryList) => {
      this.topLevelCategrories = resp;
    })

  }

  selectedTopLevel() {
    // alert(this.selectedCategory)
    this.selectedSubCategory = ""
    this.transactionService.getAllCategories(this.selectedCategory).subscribe((resp : CategoryList) => {
      this.lowLevelCategories = resp;
    })
  }

  selectedLowLevel() {
    this.selectedCategory = ""
    // console.log(this.selectedCategory)
  }

  applyCategory() {
    // ako je selectovao low level - top level je ""
    // ako je selectovao top level - low level je ""
    alert("category:" + this.selectedCategory + "; subcategory: " + this.selectedSubCategory)

    if (this.selectedCategory != "" ) {
      // send category
      this.transactionService.setCategoryForTransaction(this.t.id, this.selectedCategory).subscribe((resp: HttpResponse<any>) => {
        if (resp.status == 200) {
          this.t.catcode = this.selectedCategory;
        }
      })
    }
    else if (this.selectedSubCategory != "") {
      // send subcategory
      this.transactionService.setCategoryForTransaction(this.t.id, this.selectedSubCategory).subscribe((resp: HttpResponse<any>) => {
        if (resp.status == 200) {
          this.t.catcode = this.selectedSubCategory;
        }
      })
    }
    else {
      // remaining uncategorized
    }
  }

  returnToStart() {
    localStorage.clear();
    this.router.navigate([''])
  }

}
