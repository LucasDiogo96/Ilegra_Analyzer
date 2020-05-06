using System;
using System.Collections.Generic;

namespace Analyzer.Core.Entities
{
    public class FileContent : Base
    {
        public FileContent()
        {
            Identifier = Guid.NewGuid();
            Sales = new List<Sale>();
            Sellers = new List<Seller>();
            Customers = new List<Customer>();
        }
        public Guid Identifier { get; set; }
        public List<Sale> Sales { get; set; }
        public List<Seller> Sellers { get; set; }
        public List<Customer> Customers { get; set; }
    }
}
