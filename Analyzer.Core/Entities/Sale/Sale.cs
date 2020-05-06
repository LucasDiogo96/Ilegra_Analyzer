using System.Collections.Generic;

namespace Analyzer.Core.Entities
{
    public class Sale : Base
    {
        public List<SaleItem> Items { get; set; }
        public string SellerName { get; set; }
    }
}
