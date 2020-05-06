using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Analyzer.Core.Entities
{
    public class Sale : Base
    {
        public Sale(string _SaleId, string LineItems, string _SellerName)
        {
            this.Id = Convert.ToInt32(_SaleId);
            this.SellerName = _SellerName;

            //get items
            string[] items = Regex.Replace(LineItems, @"[\[\]']", "").Split(",");

            this.Items = new List<SaleItem>();

            for (int i = 0; i < items.Length; i++)
            {
                //split item lines
                string[] item = items[i].Split("-");
                //add on constructor
                this.Items.Add(new SaleItem(item[0], item[1], item[2]));
            }

        }


        public Sale() { }

        /// <summary>
        /// props
        /// </summary>
        public List<SaleItem> Items { get; set; }
        public string SellerName { get; set; }
    }
}
