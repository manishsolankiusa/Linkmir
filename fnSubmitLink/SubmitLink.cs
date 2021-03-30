using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Linkmir.AzFunctions.DBContexts;
using Linkmir.AzFunctions.Models;
using Linkmir.AzFunctions.Repository;

/*
 * Author   : Manish Solanki
 * Date     : March, 2021
 * All rights reserved.
 */

namespace Linkmir.AzFunctions.SubmitLink
{
    public static class SubmitLink
    {
        [FunctionName("SubmitLink")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log, ExecutionContext context)
        {
            var config = new ConfigurationBuilder().SetBasePath(context.FunctionAppDirectory).AddJsonFile("local.settings.json", optional: true, reloadOnChange: true).AddEnvironmentVariables().Build();
            var connectionString = config.GetConnectionString("ConnectionString");

            //log.LogInformation("Connection String:" + connectionString);
            //Console.WriteLine(connectionString);
            LinkContext.connectionString = connectionString;

            string linkOriginal = req.QueryString.Value.Substring(6);

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            //linkOriginal = linkOriginal ?? data?.name;

            Link link = new Link();
            link.LinkOriginal = linkOriginal;
            
            string message = "";
            string responseMessage;
            if (link.ValidateOriginal(ref message))
            {
                LinkRepository linkRepository = new LinkRepository(new LinkContext());
                linkRepository.InsertLink(link);

                //Console.WriteLine(link.Status);

                responseMessage = JsonConvert.SerializeObject(new { Domain = linkOriginal, ShortenedLink = "https://www.linkmir.com/" + link.LinkShort, Status = link.Status } );

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
