﻿using Newtonsoft.Json;
using AssecoPraksa.Models;
using AssecoPraksa.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace AssecoPraksa.Controllers
{
    // using cors, this is an ApiController - it accepts http requests
    // routing requests starting with transaction
    [EnableCors("MyCORSPolicy")]
    [ApiController]
    [Route("transaction")]
    public class TransactionController : Controller
    {
        ITransactionService _transactionService;
        private readonly ILogger<TransactionsController> _logger;

        public TransactionController(ILogger<TransactionsController> logger, ITransactionService productService)
        {
            _logger = logger;
            _transactionService = productService;
        }

        // transaction/{transactionId}/split
        [HttpPost("{transactionId}/split")]
        public async Task<IActionResult> SplitTransactionAsync([FromRoute] string transactionId, [FromBody] SplitTransactionCommand command)
        {
            var problem = new ValidationProblem();

            int number;
            if (transactionId == null || string.IsNullOrEmpty(transactionId) || !int.TryParse(transactionId, out number))
            {
                // validation problem
                problem.Errors.Add(new ValidationProblem.ProblemDetails("transactionId", "required", "Transaction id invalid id"));
                return BadRequest(problem);
            }

            if (command == null)
            {
                // validation problem
                problem.Errors.Add(new ValidationProblem.ProblemDetails("transaction-categorize-command", "required", "Missing category code"));
            }

            if (problem.Errors.Count > 0)
            {
                return BadRequest(problem);
            }


            var value = await _transactionService.SplitTransactionAsync(number, command);
            if (value == 0)
            {
                return Ok("OK - Transaction splitted");
            }
            else if (value == 1)
            {
                // business problem - category code is not a valid code
                return StatusCode(440, new BusinessProblem("provided-category-does-not-exists", "Category code doesn't exist", "Some category code is invalid"));

            }
            else if (value == 2)
            {
                // business problem - transactionId is not a valid id
                return StatusCode(440, new BusinessProblem("transaction-does-not-exists", "Transaction id doesn't exist", "Transaction with id = " + transactionId + " doesn't exist"));
            }
            else if (value == 3)
            {
                // business problem - sum of transaction splits amounts doesn't equal to the transaction amount
                return StatusCode(440, new BusinessProblem("split-amount-over-transaction-amount", "Wrong splits amounts", "Sum of transaction splits amounts doesn't equal to the amount of the transaction"));
            }
            else
            {
                // unknown error
                return BadRequest("unknown problem");
            }

        }

        // transaction/{transactionId}/categorize
        [HttpPost("{transactionId}/categorize")]
        public async Task<IActionResult> CategorizeTransactionAsync([FromRoute] string transactionId, [FromBody] TransactionCategorizeCommand command)
        {
            var problem = new ValidationProblem();

            int number = 0;
            if (transactionId == null || string.IsNullOrEmpty(transactionId) || !int.TryParse(transactionId, out number))
            {
                // validation problem
                problem.Errors.Add(new ValidationProblem.ProblemDetails("transaction-id", "required", "Transaction id invalid id"));
                
            }

            if (command == null || string.IsNullOrEmpty(command.Catcode))
            {
                // validation problem
                problem.Errors.Add(new ValidationProblem.ProblemDetails("transaction-categorize-command", "required", "Missing category code"));
            }

            if (problem.Errors.Count > 0)
            {
                return BadRequest(problem);
            }

            
            // this is okay because only one business problem can be returned
            var value = await _transactionService.CategorizeTransactionAsync(number, command);
            if (value == 0)
            {
                return Ok("OK - Transaction categorized");
            }
            else if (value == 1)
            {
                // business problem - category code is not a valid code
                return StatusCode(440, new BusinessProblem("provided-category-does-not-exists", "Category code doesn't exist", "Code " + command.Catcode + " doesn't exists"));

            }
            else if (value == 2)
            {
                // business problem - transactionId is not a valid id
                return StatusCode(440, new BusinessProblem("transaction-does-not-exists", "Transaction id doesn't exist", "Transaction with id = " + transactionId + " doesn't exist"));
            }
            else
            {
                // unknown error
                return BadRequest("unknown problem");
            }

        }

        // transaction/auto-categorize
        [HttpPost("auto-categorize")]
        public async Task<IActionResult> AutoCategorizeAsync()
        {
            // ucitavanje fajla koji sadrzi pravila
            // bar 50% transakcija se kategorizuje
            // fajl je JSON koji sadrzi:
            //  title: - ne sluzi nicemu
            //  catcode: - koji catcode se dodeljuje
            //  predicate: - pod kojim uslovom
            string jsonString = System.IO.File.ReadAllText("config.json");

            var commands = JsonConvert.DeserializeObject<List<AutoCategorizeCommand>>(jsonString);

            if (commands == null)
            {
                return BadRequest("Json file is empty or has no rules");
            }

            // go over transactions which don't have a catcode
            // if a rule is met set a category code
            var transactions = await _transactionService.getAllTransactionsWithouthCatcodeAsync();

            foreach (var transaction in transactions)
            {
                foreach (var command in commands)
                {
                    // go over commands and execute them

                    // command predicate is in the format: column operation literal
                    string[] predicateWords = command.Predicate.Split(' ');

                    // switch by colums
                    switch (predicateWords[0])
                    {
                        case "id":
                            break;
                        case "beneficiary-name":
                            break;
                        case "date":
                            break;
                        case "direction":
                            break;
                        case "amount":
                            break;
                        case "description":
                            break;
                        case "currency":
                            break;
                        case "mcc":
                            if (string.IsNullOrEmpty(transaction.Mcc))
                            {
                                break;
                            }

                            int number;
                            if (!int.TryParse(predicateWords[2], out number))
                            {
                                return BadRequest("Literal is not a number for arithmetic operation");
                            }

                            switch(predicateWords[1])
                            {
                                case "<":
                                    if (int.Parse(transaction.Mcc) < number)
                                    {
                                        transaction.Catcode = command.Catcode;
                                        await _transactionService.CategorizeTransactionAsync(int.Parse(transaction.Id), new TransactionCategorizeCommand(command.Catcode));
                                    }
                                    break;
                                case ">":
                                    if (int.Parse(transaction.Mcc) > number)
                                    {
                                        transaction.Catcode = command.Catcode;
                                        await _transactionService.CategorizeTransactionAsync(int.Parse(transaction.Id), new TransactionCategorizeCommand(command.Catcode));
                                    }
                                    break;
                                case "=":
                                    if (int.Parse(transaction.Mcc) == number)
                                    {
                                        transaction.Catcode = command.Catcode;
                                        await _transactionService.CategorizeTransactionAsync(int.Parse(transaction.Id), new TransactionCategorizeCommand(command.Catcode));
                                    }
                                    break;
                                default:
                                    return BadRequest("Invalid operation!");
                            }
                            break;
                        case "transaction-kind":
                            break;
                        default:
                            return BadRequest("Invalid column name!");
                    }


                }
            }

            return Ok();
        }

        protected IActionResult Index()
        {
            return View();
        }
    }
}
