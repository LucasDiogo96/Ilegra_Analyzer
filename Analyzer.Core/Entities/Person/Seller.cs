using System;

namespace Analyzer.Core.Entities
{
    public class Seller : BasePerson
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_Identity"></param>
        /// <param name="_Name"></param>
        /// <param name="Sallary"></param>
        public Seller(string _Identity, string _Name, string _Sallary)
        {
            this.Identity = _Identity;
            this.Name = _Name;
            this.Sallary = Convert.ToDecimal(_Sallary);
        }

        public Seller() { }
        /// <summary>
        /// props
        /// </summary>
        public decimal Sallary { get; set; }
    }
}
