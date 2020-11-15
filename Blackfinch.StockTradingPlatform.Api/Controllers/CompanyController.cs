using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Blackfinch.StockTradingPlatform.Core.Companies;
using Blackfinch.StockTradingPlatform.Core.Orders;
using Blackfinch.StockTradingPlatform.Data.DTO;
using Blackfinch.StockTradingPlatform.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blackfinch.StockTradingPlatform.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly IOrderService _orderService;

        public CompanyController(ICompanyService companyService, IOrderService orderService)
        {
            _companyService = companyService;
            _orderService = orderService;
        }

        [HttpGet("all")]
        public List<CompanyDto> GetAll()
        {
            return _companyService.GetCompanies();
        }

        [HttpPost("add")]
        public ActionResult<CompanyDto> AddCompany(string name, string symbol)
        {
            if (_companyService.GetCompanyBySymbol(symbol) != null)
                return NotFound($"Company already exists with symbol: '{symbol}'");
            if (_companyService.GetCompanyByName(name) != null)
                return NotFound($"Company already exists with name: '{name}'");
            return _companyService.AddCompany(name, symbol);
        }

        [HttpPost("removeBySymbol")]
        public ActionResult<bool> RemoveCompanyBySymbol([Required] string symbol)
        {
            var companyDto = _companyService.GetCompanyBySymbol(symbol);
            if (companyDto == null) return NotFound($"Company not found with symbol: '{symbol}'");
            return _companyService.RemoveCompany(companyDto)
                ? Ok("Company removed")
                : StatusCode(500, "Failed to remove company");
        }

        [HttpPost("issueShares/{company}")]
        public OrderDto IssueShare(int amount, decimal priceInPound, string symbol)
        {
            return _orderService.PlaceOrder(symbol, priceInPound, priceInPound, amount, OrderType.Sell);
        }
    }
}