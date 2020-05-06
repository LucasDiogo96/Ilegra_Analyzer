using Analyzer.CrossCutting.Lib.Attributes;

namespace Analyzer.Core.Enums
{
    public enum ClassificatorEnum
    {
        [ClassificatorStatus("001", "Seller")]
        Seller = 1,
        [ClassificatorStatus("002", "Customer")]
        Customer = 2,
        [ClassificatorStatus("003", "Sale")]
        Sale = 3
    }
}
