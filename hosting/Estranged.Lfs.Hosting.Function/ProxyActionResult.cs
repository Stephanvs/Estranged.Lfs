using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

namespace Estranged.Lfs.Hosting.Function
{
    public class ProxyActionResult : IActionResult
    {
        public async Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Features.Set<IServiceProvidersFeature>(null); // remove webjob host service provider

            await InternalServer.Instance.Application.ProcessRequestAsync(
                new Microsoft.AspNetCore.Hosting.Internal.HostinApplication.Context()
                {
                    HttpContext = context.HttpContext
                });
        }
    }
}
