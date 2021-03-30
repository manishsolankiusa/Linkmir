using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Linkmir.AzFunctions.ReportingServices;
using Linkmir.AzFunctions.DBContexts;

/*
 * Author   : Manish Solanki
 * Date     : March, 2021
 * All rights reserved.
 */

namespace Linkmir.AzFunctions
{
    public static class GetStats
    {
        [FunctionName("GetStats")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log, ExecutionContext context)
        {
            var config = new ConfigurationBuilder().SetBasePath(context.FunctionAppDirectory).AddJsonFile("local.settings.json", optional: true, reloadOnChange: true).AddEnvironmentVariables().Build();
            var connectionString = config.GetConnectionString("ConnectionString");

            ReportContext.connectionString = connectionString;

            string query = req.Query["query"];
            string link = req.QueryString.Value.Substring(6);

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            //name = name ?? data?.name;

            string responseMessage;
            SummaryReport summaryReport = new SummaryReport();
            string message = "";
            if (!string.IsNullOrEmpty(query) && summaryReport.Validate(query, ref message))
            {
                string summaryData = summaryReport.GetByDomain(query);

                responseMessage = JsonConvert.SerializeObject(new { criteria = query, resultxml = summaryData });

                return new OkObjectResult(responseMessage);
            }
            else if (!string.IsNullOrEmpty(link) && summaryReport.Validate(link, ref message))
            {
                string summaryData = summaryReport.GetByLink(link);

                responseMessage = JsonConvert.SerializeObject(new { link = link, resultxml = summaryData });

                return new OkObjectResult(responseMessage);
            }
            else
            {
                responseMessage = message;
                return new BadRequestObjectResult(responseMessage);
            }
        }
    }
}

