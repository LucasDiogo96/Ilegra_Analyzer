using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Analyzer.Core.Entities
{
    public class SaleItem : Base
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_Id"></param>
        /// <param name="_Quantity"></param>
        /// <param name="_Price"></param>
        public SaleItem(string _Id, string _Quantity, string _Price)
        {
            this.Id = Convert.ToInt32(_Id);
            this.Quantity = Convert.ToInt32(_Quantity);
            this.Price = decimal.Parse(_Price, CultureInfo.InvariantCulture);
        }

        public SaleItem() { }

        /// <summary>
        /// props
        /// </summary>
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
