
using validate_national_Id.Enums;
using validate_national_Id.Implementation;
using validate_national_Id.interfaces;

namespace validate_national_Id.ImplementFactory
{
    public class NationalIdValidationStrategyFactory : INationalIdValidationStrategyFactory
    {
        public INationalIdValidationStrategy Create(Country country)
        {
            return country switch
            {
                Country.Egypt => new EgyptNationalIdValidationStrategy(),
                // Add more cases for other countries and their corresponding strategies
                _ => throw new NotSupportedException($"Validation strategy for country {country} is not supported.")
            };
        }
    }
}
