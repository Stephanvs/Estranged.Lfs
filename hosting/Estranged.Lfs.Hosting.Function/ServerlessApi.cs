using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Estranged.Lfs.Hosting.Function
{
    public static class ServerlessApi
    {
        [FunctionName("Proxy")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, Route = "{*all}")]
            HttpRequest req,
            ILogger log)
        {
            return new ProxyActionResult();
        }
    }
}
