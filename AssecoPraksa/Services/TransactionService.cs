
namespace AssecoPraksa.Services
{

    public class TransactionService : ITransactionService
    {
        // pomocna funkcija za parsiranje CSV fajla
        private List<String> ParseCsv(string csvData)
        {
            var records = new List<String>();
            using (var reader = new StringReader(csvData))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var values = line.Split(',');
                    /*
                    var entity = new String
                    {
                        Property1 = values[0],
                        Property2 = values[1],
                        // Map other properties
                    };
                    
                    records.Add(entity);
                    */
                }
            }
            return records;
        }


        public async Task<bool> importTransactionsFromCSV(IFormFile csvFile)
        {
            
            using (var stream = new StreamReader(csvFile.OpenReadStream()))
            {
                // Parsing a CSV file
                var csvData = await stream.ReadToEndAsync();
                var records = ParseCsv(csvData);


                // save to database

            }

            return true;
        }
    }
}
