using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Analyzer.Core.Entities
{
    public class OutputFileContent
    {

        public OutputFileContent(FileContent content)
        {
            this.IdMostExpansiveSale = content.Sales.Find(e => e.Items.Any(x => (x.Price * x.Quantity) == e.Items.Max(i => i.Price * i.Quantity))).Id;
            //this.WorstSeller = content.Sales.Find(e => e.Items.Min(e.Items.Sum(t=>t.Price*t.Quantity))).SellerName;

            this.CustomerQuantity = content.Customers.Count;
            this.SellerQuantity = content.Sellers.Count;
        }

        public int IdMostExpansiveSale { get; private set; }
        public int SellerQuantity { get; private set; }
        public int CustomerQuantity { get; private set; }
        public string WorstSeller { get; private set; }
    }
}
