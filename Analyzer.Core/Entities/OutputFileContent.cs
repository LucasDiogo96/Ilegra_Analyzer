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

            //set customer quantity
            this.CustomerQuantity = content.Customers.Count;
            //set sellet quantity
            this.SellerQuantity = content.Sellers.Count;
        }

        public Guid Identifier { get; private set; }
        public int IdMostExpansiveSale { get; private set; }
        public int SellerQuantity { get; private set; }
        public int CustomerQuantity { get; private set; }
        public string WorstSeller { get; private set; }
    }
}
