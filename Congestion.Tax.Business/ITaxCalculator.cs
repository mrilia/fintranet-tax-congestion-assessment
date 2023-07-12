using Congestion.Tax.Business.Models.Tax;

namespace Congestion.Tax.Business
{
    public interface ITaxCalculator
    {
        double GetTax(TaxCalculationRequest request);
    }
}
