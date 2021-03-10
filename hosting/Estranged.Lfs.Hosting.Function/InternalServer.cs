using System;
using System.Threading;
using System.Threading.Tasks;
using Estranged.Lfs.Hosting.AspNet;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http.Features;

namespace Estranged.Lfs.Hosting.Function
{
    public class InternalServer : IServer
    {
        private readonly IWebHost _hostInstance;
        private ApplicationWrapper _application;
        private bool _disposed = false;

        public static InternalServer Instance { get; set; }

        internal ApplicationWrapper Application
        {
            get => _application ?? throw new InvalidOperationException("The server has not been started or no web application was configured.");
        }

        static InternalServer()
        {
            var builder = new WebHostBuilder()
                .UseStartup<Startup>();

            Instance = new InternalServer(builder);
        }

        public InternalServer(IWebHostBuilder builder)
            : this(builder, new FeatureCollection())
        {
        }

        public InternalServer(IWebHostBuilder builder, IFeatureCollection features)
        {
            var host = builder.UseServer(this).Build();
            host.StartAsync().GetAwaiter().GetResult();
            _hostInstance = host;

            Features = features;
        }

        public IFeatureCollection Features { get; }

        public void Dispose()
        {
            if (!_disposed)
            {
                _hostInstance.Dispose();
                _disposed = true;
            }
        }

        public Task StartAsync<TContext>(IHttpApplication<TContext> application, CancellationToken cancellationToken)
        {
            _application = new ApplicationWrapper<TContext>(application, () =>
            {
                if (_disposed)
                {
                    throw new ObjectDisposedException(GetType().FullName);
                }
            });

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
