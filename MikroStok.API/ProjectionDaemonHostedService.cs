using System.Threading;
using System.Threading.Tasks;
using Baseline.Dates;
using Marten;
using Marten.Events.Projections.Async;
using Microsoft.Extensions.Hosting;

namespace MikroStok.API
{
    public class ProjectionDaemonHostedService : IHostedService
    {
        private readonly IDocumentStore _documentStore;
        private IDaemon _projectionDaemon;

        public ProjectionDaemonHostedService(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _projectionDaemon = _documentStore.BuildProjectionDaemon(settings: new DaemonSettings
            {
                LeadingEdgeBuffer = 50.Milliseconds(),
                FetchingCooldown = 50.Milliseconds()
            });
            _projectionDaemon.StartAll();
            
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _projectionDaemon.StopAll();
            _projectionDaemon.Dispose();
            
            return Task.CompletedTask;
        }
    }
}