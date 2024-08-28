import { SingleCategorySplit } from "./singleCategorySplit"

export class TransactionWithSplits{
    "id": string = ""
    "beneficiary-name": string = ""
    "date": string = ""
    "direction": string = ""
    "amount": number = 0
    "description": string = ""
    "currency": string = ""
    "mcc": string = ""
    "transaction-kind": string = ""
    "catcode": string = ""
    "splits": Array<SingleCategorySplit> = new Array()
  }
  