Personal Finance Management
===========================
Personal Finance Management is a set of activities performed by the customer on its financial data for the purpose of getting clear picture of the incomes/expenses trends, as well as enabling customer to manage monthly budget and personal financial goals.

Personal Finance Management starts with **categorization** of transactions into income, expense and savings & investment categories and subcategories. Majority of transactions should be automatically categorized, allowing the customer to manage categorization manually and using user categorization rules. Each transaction has a category and optionally a subcategory assigned. Categorization scheme used within the solution is described below. 

Note that besides spending categories, categories enum includes values for uncatogorized and split transactions.

![image](https://user-images.githubusercontent.com/11427016/140492547-adef28d7-b88f-42f8-8087-c0a80f6e62e0.png)

PFM Backend
====================================
PFM microservice written in .Net core

Basic Features
--------------

### BE-B1 Import transactions from csv file  (1 points)

- Enable import of bank transactions based on the format of bank_transactions.csv file.
- Expose POST /transactions/import API endpoint for this purpose.
- Create relational DB schema to support import of transactions with `id` as primary key.
- Validate input according to OAS3 spec.
- Persist transaction into database.

### BE-B2 List transactions with filters and pagination (0.5 points)

- Enable paginated listing of transactions based on supplied filter conditions.
- Expose GET /transactions API endpoints for this purpose.
- Implement period filter (start-date and end-date).
- Implement transaction kinds filter as a list of acceptable transaction kinds.

### BE-B3 Import categories from csv file (1 points)

- Enable import of spending categories based on the format of categories.csv file.
- Expose POST /categories/import API endpoint for this purpose.
- Create relational DB schema to support import of categories with `code` as primary key and foreign key from transactions to categories on `catcode` matching `code` field.
- Validate input according to OAS3 spec.
- Persist categories into database.

**Note that:**
- when `code` already exists its name should be updated
- `parent-code` already exists it should be updated

### BE-B4 Categorize single transaction (0.5 points)

- Enable categorization of a single transaction.
- Expose POST /transactions/{id}/categorize endpoint for this purpose.
- Validate that both category and transaction exists in database.
- Persist newly set category in database.

### BE-B5 Analytical view of spending by categories and subcategories (1 points)

- Enable analytical views of spendings by categories and subcategories.
- Expose GET /spending-analytics endpoint for this purpose.
- Implement optional category filter.
- Implement optional period filter (start-date and end-date).
- Implement optional direction filter (debits or credits)

### BE-B6 Split transaction (1 points)

- Enable split of transaction into multiple spending categories or subcategories.
- Expose POST /transactions/{id}/split endpoint for this purpose.
- If transaction is already split, deleta existing splits and replace them with new ones.
- Validate that the transaction and categories exist.
- Create relational DB schema that can persist splits for a transaction.
- Extend transaction list endpoint to return splits for each transaction.
- Persist splits into database with `amount` and `catcode`.


Advanced features
-----------------
### BE-A1 Write API tests in Postman (2 points)

- Write test cases that cover basic requirements with API tests written in Postman tool.
- In test assertions validate at least status code and schema of response payload.
- Create test report in HTML using newman html-reporter.

### BE-A2 Automatically assign categories based on predefined rules (2 points)

- Enable automatic assignment of categories and subcategories based on predefined rules.
- Expose POST /transactions/auto-categorize endpoint for this purpose.
- Define rules to correctly categorize at least 50% of transactions. Hint: some transactions occur more frequently than others.
- If transaction already has a category assigned do not reasign it to automaticaly determined category.
- Each rule has a code of categery and SQL compliant predicate expression (filter condition) that defines which transactions should fall into the category.
- Make rules configurable (outside of your code) in a config file.

**Examples of rules:**

```yaml
rule-1:
  title: When beneficiary name contains "chevron" or "shell" categorize transaction as 4 (Auto & Transport / Gas & Fuel)
  catcode: 4
  predicate: beneficiary-name LIKE '%chevron%' OR beneficiary-name LIKE '%shell%'
rule-2:
  title: When mcc is 5811 (Caterers) categorize transaction as 39 (Food & Dining / Restaurants)
  catcode: 39
  predicate: mcc = 5811
```

### BE-A3 Create a basic web UI for transaction list and categorization of single transaction (2 points)

- Make a functional web UI in technology of your choosing (plain JS+HTML, ASP.NET, Angular, React, etc).
- Design is not important as long as it works.
- See requirements FE-B1 and FE-B2 for details.
