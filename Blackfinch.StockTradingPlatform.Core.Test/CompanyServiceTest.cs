using System.Linq;
using Blackfinch.StockTradingPlatform.Core.Companies;
using Blackfinch.StockTradingPlatform.Core.Orders;
using Blackfinch.StockTradingPlatform.Data.Contexts;
using NUnit.Framework;

namespace Blackfinch.StockTradingPlatform.Core.Test
{
    public class CompanyServiceTests
    {
        private ICompanyService _companyService;

        private IOrderService _orderService;

        [SetUp]
        public void Setup()
        {
            var orderContext = new OrderContext();
            var companyContext = new CompanyContext();

            _orderService = new OrderService(orderContext);
            _companyService = new CompanyService(companyContext, _orderService);
        }

        [Test]
        public void WhenFirstStarted_ThereAreNoCompanies()
        {
            Assert.IsEmpty(_companyService.GetCompanies());
        }

        [Test]
        public void WhenCompanyIsCreated_CompanyExists()
        {
            _companyService.AddCompany("SEALAND", "SEA");
            var companyDto = _companyService.GetCompanyBySymbol("SEA");
            //Company exists if returned value is not null
            Assert.IsNotNull(companyDto);
        }

        [Test]
        public void WhenCompanyIsCreated_ItContainsZeroShares()
        {
            var companyDto = _companyService.GetCompanyBySymbol("SEA") == null
                ? _companyService.AddCompany("SEALAND", "SEA")
                : _companyService.GetCompanyBySymbol("SEA");
            Assert.AreEqual(0, companyDto.SharesIssued);
        }

        [Test]
        public void WhenCompanyIsCreated_ItCanBeRetrievedViaSymbol()
        {
            _companyService.AddCompany("SEALAND", "SEA");
            Assert.IsNotNull(_companyService.GetCompanyBySymbol("SEA"));
        }

        [Test]
        public void WhenRetrievingCompanyThatDoesntExist_ReturningValueIsNull()
        {
            Assert.IsNull(_companyService.GetCompanyBySymbol("NULL"));
        }

        [Test]
        public void WhenCompanyIsRemoved_ItIsRemoved()
        {
            var companyDto = _companyService.AddCompany("CompanyToDelete", "DEL");
            //Company exists
            Assert.IsNotNull(_companyService.GetCompanies().Where(company => company.Symbol == companyDto.Symbol));
            _companyService.RemoveCompany(companyDto);
            Assert.IsEmpty(_companyService.GetCompanies().Where(company => company.Symbol == "DEL"));
        }

        [Test]
        public void WhenCompanyIssuesShares_SharesAmountIsIncreasedAndOrderIsCreated()
        {
            _companyService.AddCompany("SEALAND", "SEA");
            var orderDto = _companyService.IssueShares(_companyService.GetCompanyBySymbol("SEA"), 40m, 1000);
            Assert.AreEqual(1000, _companyService.GetCompanyBySymbol("SEA").SharesIssued);
            Assert.IsNotNull(_orderService.GetOrder(orderDto.Id));
        }
    }
}