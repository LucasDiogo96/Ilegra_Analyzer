using System;
using System.Collections.Generic;
using System.Text;

namespace Analyzer.Core.Entities
{
    public class SaleItem : Base
    {
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
