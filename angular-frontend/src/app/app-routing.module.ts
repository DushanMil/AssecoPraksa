import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TransactionsListComponent } from './transactions-list/transactions-list.component';
import { CategorizeTransactionComponent } from './categorize-transaction/categorize-transaction.component';

const routes: Routes = [
  {path: "", component: TransactionsListComponent},
  {path: "categorize", component: CategorizeTransactionComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
