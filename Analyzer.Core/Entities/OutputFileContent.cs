using System;
using System.Linq;

namespace Analyzer.Core.Entities
{
    public class OutputFileContent
    {

        public OutputFileContent(FileContent content)
        {
            //file Identifier
            this.Identifier = content.Identifier;

            //Most Expansive Sale Id
            this.IdMostExpansiveSale = content.Sales.OrderByDescending(item => item.Amount).First().Id;

            ////Worst Seller
            this.WorstSeller = (from p in content.Sales
                                group p by p.SellerName into g
                                select new
                                {
                                    Seller = g.Key,
                                    Amount = g.Sum(g => g.Amount)
                                }).OrderBy(c => c.Amount).FirstOrDefault().Seller;

            //set customer quantity group by document
            this.CustomerQuantity = (from p in content.Customers
                                     group p by p.Identity into g
                                     select new
                                     {
                                         Identity = g.Key,
                                     }).Count();


            //set sellers quantity group by document
            this.SellerQuantity = (from p in content.Sellers
                                   group p by p.Identity into g
                                   select new
                                   {
                                       Identity = g.Key,
                                   }).Count();
        }

        public Guid Identifier { get; private set; }
        public int IdMostExpansiveSale { get; private set; }
        public int SellerQuantity { get; private set; }
        public int CustomerQuantity { get; private set; }
        public string WorstSeller { get; private set; }
    }
}
