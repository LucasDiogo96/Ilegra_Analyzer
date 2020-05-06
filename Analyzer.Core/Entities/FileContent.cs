using System.Collections.Generic;

namespace Analyzer.Core.Entities
{
    public class FileContent : Base
    {
        public List<Sale> Sales { get; set; }
        public List<Seller> Sellers { get; set; }
        public List<Seller> Customers { get; set; }
    }
}
