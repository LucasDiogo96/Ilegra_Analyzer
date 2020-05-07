using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Analyzer.Core.Entities
{
    public class Sale : Base
    {
        public Sale(string _SaleId, string LineItems, string _SellerName)
        {
            this.Id = Convert.ToInt32(_SaleId);
            this.SellerName = _SellerName;
            this.Items = GetItems(LineItems);
            this.Amount = GetAmount();
        }

        public Sale() { }

        /// <summary>
        /// props
        /// </summary>
        /// 
        public decimal Amount { get; set; }
        public List<SaleItem> Items { get; set; }
        public string SellerName { get; set; }


        #region Entity Methods
        protected decimal GetAmount()
        {
            return this.Items.Sum(e => e.Price * e.Quantity);
        }


        protected List<SaleItem> GetItems(string LineItems)
        {
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

            return Items;
        }
        #endregion

    }
}
