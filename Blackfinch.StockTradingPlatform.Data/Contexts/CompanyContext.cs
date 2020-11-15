using System;
using System.Collections.Generic;
using System.Linq;
using Blackfinch.StockTradingPlatform.Data.DTO;
using Blackfinch.StockTradingPlatform.Data.Models;

namespace Blackfinch.StockTradingPlatform.Data.Contexts
{
    public interface ICompanyContext
    {
        public CompanyDto GetCompanyBySymbol(string symbol);

        public CompanyDto GetCompanyByName(string name);

        public List<CompanyDto> GetCompanies();

        public bool AddCompany(CompanyDto companyDto);
        public bool RemoveCompany(CompanyDto companyDto);

        public bool IssueShares(CompanyDto companyDto, int quantity);
    }

    public class CompanyContext : ICompanyContext
    {
        private readonly List<Company> _companies = new List<Company>();

        public CompanyDto GetCompanyBySymbol(string symbol)
        {
            try
            {
                return new CompanyDto(_companies.First(company => company.Symbol == symbol));
            }
            catch (InvalidOperationException e)
            {
                return null;
            }
        }

        public CompanyDto GetCompanyByName(string name)
        {
            try
            {
                return new CompanyDto(_companies.First(company => company.Name == name));
            }
            catch (InvalidOperationException e)
            {
                return null;
            }
        }

        public List<CompanyDto> GetCompanies()
        {
            return _companies.Select(company => new CompanyDto(company)).ToList();
        }

        public bool AddCompany(CompanyDto companyDto)
        {
            _companies.Add(new Company(companyDto.Name, companyDto.Symbol));
            return true;
        }

        public bool RemoveCompany(CompanyDto companyDto)
        {
            //Suggestion: Remove Orders with company too?
            return _companies.Remove(_companies.First(company => company.Symbol == companyDto.Symbol));
        }

        public bool IssueShares(CompanyDto companyDto, int quantity)
        {
            try
            {
                var company = _companies.First(companyItem => companyItem.Symbol == companyDto.Symbol);
                company.SharesIssued += quantity;
                return true;
            }
            catch (InvalidOperationException e)
            {
                return false;
            }
        }
    }
}