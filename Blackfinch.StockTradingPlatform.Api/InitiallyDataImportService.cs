using System.Threading;
using System.Threading.Tasks;
using Blackfinch.StockTradingPlatform.Core.Companies;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Blackfinch.StockTradingPlatform.Api
{
    public class InitiallyDataImportService : IHostedService
    {
        private readonly ICompanyService _companyService;
        private readonly ILogger<InitiallyDataImportService> _logger;

        public InitiallyDataImportService(ILogger<InitiallyDataImportService> logger, ICompanyService companyService)
        {
            _logger = logger;
            _companyService = companyService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Beginning import of demo data...");
            var amazonDto = _companyService.AddCompany("Amazon", "AMZN");
            var blackfinchDto = _companyService.AddCompany("Blackfinch Group", "BLKFNCH");
            var teslaDto = _companyService.AddCompany("Tesla", "TSLA");
            _logger.LogDebug("Companies added...");

            _companyService.IssueShares(amazonDto, 40, 100000);
            _companyService.IssueShares(blackfinchDto, 60, 600);
            _companyService.IssueShares(teslaDto, 420, 10000);
            //Auto list shares sell ?

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}