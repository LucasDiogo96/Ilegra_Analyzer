using System;
using System.ComponentModel;

namespace Analyzer.CrossCutting.Lib.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class ClassificatorStatusAttribute : DescriptionAttribute
    {
        public ClassificatorStatusAttribute(string identifier, string description)
        {
            this.Identifier = identifier;
            this.Description = description;
        }

        public string Identifier { get; set; }

        public string Description { get; set; }
    }
}
