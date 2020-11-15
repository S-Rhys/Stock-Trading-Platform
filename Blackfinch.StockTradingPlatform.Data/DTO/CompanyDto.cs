using Blackfinch.StockTradingPlatform.Data.Models;

namespace Blackfinch.StockTradingPlatform.Data.DTO
{
    public class CompanyDto
    {
        public CompanyDto(Company company)
        {
            Symbol = company.Symbol;
            Name = company.Name;
            SharesIssued = company.SharesIssued;
        }

        public CompanyDto(string name, string symbol)
        {
            Name = name;
            Symbol = symbol;
            SharesIssued = 0;
        }

        public string Name { get; }
        public string Symbol { get; }

        public int SharesIssued { get; }
    }
}