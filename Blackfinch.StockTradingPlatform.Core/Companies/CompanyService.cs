using System.Collections.Generic;
using Blackfinch.StockTradingPlatform.Core.Orders;
using Blackfinch.StockTradingPlatform.Data.Contexts;
using Blackfinch.StockTradingPlatform.Data.DTO;
using Blackfinch.StockTradingPlatform.Data.Models;

namespace Blackfinch.StockTradingPlatform.Core.Companies
{
    public interface ICompanyService
    {
        public CompanyDto GetCompanyBySymbol(string symbol);

        public CompanyDto GetCompanyByName(string name);

        public List<CompanyDto> GetCompanies();

        public CompanyDto AddCompany(string name, string symbol);
        public bool RemoveCompany(CompanyDto companyDto);

        public OrderDto IssueShares(CompanyDto companyDto, decimal priceInPoundSterling, int quantity);
    }

    public class CompanyService : ICompanyService
    {
        private readonly ICompanyContext _companyContext;
        private readonly IOrderService _orderService;

        public CompanyService(ICompanyContext companyContext, IOrderService orderService)
        {
            _companyContext = companyContext;
            _orderService = orderService;
        }

        public CompanyDto GetCompanyBySymbol(string symbol)
        {
            return _companyContext.GetCompanyBySymbol(symbol);
        }

        public CompanyDto GetCompanyByName(string name)
        {
            return _companyContext.GetCompanyByName(name);
        }

        public List<CompanyDto> GetCompanies()
        {
            return _companyContext.GetCompanies();
        }

        public CompanyDto AddCompany(string name, string symbol)
        {
            var companyDtoToAdd = new CompanyDto(name, symbol);
            return _companyContext.AddCompany(new CompanyDto(name, symbol)) ? companyDtoToAdd : null;
        }

        public bool RemoveCompany(CompanyDto companyDto)
        {
            return _companyContext.RemoveCompany(companyDto);
        }

        public OrderDto IssueShares(CompanyDto companyDto, decimal priceInPoundSterling, int quantity)
        {
            _companyContext.IssueShares(companyDto, quantity);
            return _orderService.PlaceOrder(companyDto.Symbol, priceInPoundSterling, priceInPoundSterling, quantity,
                OrderType.Sell);
        }
    }
}