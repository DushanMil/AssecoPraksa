import { TransactionWithSplits } from "./transactionWithSplits"

export class TransactionPagedList{
    "page-size": number = 0
    "page": number = 0
    "total-count": number = 0
    "sort-by": string = ""
    "sort-order": string = ""
    "items": Array<TransactionWithSplits> = new Array()
  }
  