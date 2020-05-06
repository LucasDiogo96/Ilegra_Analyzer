namespace Analyzer.Core.Entities
{
    public class Customer : BasePerson
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_Identity"></param>
        /// <param name="_Name"></param>
        /// <param name="Sallary"></param>
        public Customer(string _Identity, string _Name, string _BusinessArea)
        {
            this.Identity = _Identity;
            this.Name = _Name;
            this.BusinessArea = _BusinessArea;
        }

        public Customer() { }

        /// <summary>
        /// props
        /// </summary>
        public string BusinessArea { get; set; }
    }
}
