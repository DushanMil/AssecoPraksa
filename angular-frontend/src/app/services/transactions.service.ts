import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TransactionPagedList } from '../models/transactionPagedList';
import { CategoryList } from '../models/categoryList';

@Injectable({
  providedIn: 'root'
})
export class TransactionsService {

  constructor(private http: HttpClient) { }

  formatDate(isoDateString: string): string {
    const date = new Date(isoDateString);
    const month = ('0' + (date.getMonth() + 1)).slice(-2); // Months are zero-based
    const day = ('0' + date.getDate()).slice(-2);
    const year = date.getFullYear();
    return `${month}/${day}/${year}`;
  }

  getTransactions(kinds: string, 
    startDate: string, 
    endDate: string, 
    page: number, 
    pageSize: number, 
    sortBy: string, 
    sortOrder: string){
      
    let params = new HttpParams();
    
    if (kinds != "") {
      params = params.set('transaction-kind', kinds);
    }
    if (startDate != "") {
      startDate = this.formatDate(startDate);
      params = params.set('start-date', startDate);
    }
    if (endDate != "") {
      endDate = this.formatDate(endDate);
      params = params.set('end-date', endDate);
    }
    if (page != 1) {
      params = params.set('page', page);
    }
    if (pageSize != 10) {
      params = params.set('page-size', pageSize);
    }
    if (sortBy != "") {
      params = params.set('sort-by', sortBy);
    }
    if (sortOrder != "") {
      params = params.set('sort-order', sortOrder);
    }

    // console.log(params.set('page', 10))
    console.log(params)
    
    return this.http.get<TransactionPagedList>("http://localhost:5078/transactions", { params })
  }

  getAllCategories(parameter: string) {
    let params = new HttpParams().set('parent-id', parameter);

    return this.http.get<CategoryList>("http://localhost:5078/categories", { params })
  }

  setCategoryForTransaction(transactionId: string, catcode: string) {
    const data={
      catcode: catcode
    }
    return this.http.post<any>("http://localhost:5078/transaction/" + transactionId + "/categorize", data, { observe: 'response' })
  }


}