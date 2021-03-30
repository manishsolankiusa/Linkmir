using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
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

namespace Linkmir.AzFunctions.GetLink
{
    public static class GetLink
    {
        [FunctionName("GetLink")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log, ExecutionContext context)
        {
            var config = new ConfigurationBuilder().SetBasePath(context.FunctionAppDirectory).AddJsonFile("local.settings.json", optional: true, reloadOnChange: true).AddEnvironmentVariables().Build();
            var connectionString = config.GetConnectionString("ConnectionString");

            LinkContext.connectionString = connectionString;

            string linkShort = req.QueryString.Value;

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            //name = name ?? data?.name;

            Link link = new Link();
            link.LinkShort = linkShort.Substring(0, 2) == "?=" ? linkShort.Substring(2) : linkShort;

            string message = "";
            string responseMessage;
            if (link.ValidateShort(ref message))
            {
                LinkRepository linkRepository = new LinkRepository(new LinkContext());
                linkRepository.GetLinkByShortName(link);

                //Console.WriteLine(link.Status);

                responseMessage = JsonConvert.SerializeObject(new { Domain = link.LinkValidated, ShortenedLink = "https://www.linkmir.com/" + link.LinkShort, Status = link.Status } );

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

