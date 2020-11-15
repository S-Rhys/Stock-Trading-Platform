namespace Blackfinch.StockTradingPlatform.Data.Models
{
    public class Company
    {
        public Company(string name, string symbol)
        {
            Name = name;
            Symbol = symbol;
            SharesIssued = 0;
        }

        public string Name { get; set; }
        public string Symbol { get; set; }
        public int SharesIssued { get; set; }
    }
}