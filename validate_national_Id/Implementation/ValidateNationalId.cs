using validate_national_Id.Enums;
using validate_national_Id.interfaces;
using validate_national_Id.models;

namespace validate_national_Id.Implementation
{
    public class ValidateNationalId
    {
        private readonly NationalIdValidatorContext _validatorContext;
        private readonly INationalIdValidationStrategyFactory _strategyFactory;

        public ValidateNationalId(INationalIdValidationStrategyFactory strategyFactory)
        {
            _validatorContext = new NationalIdValidatorContext();
            _strategyFactory = strategyFactory;
        }

        public ResponseModel ValidateNationalNumber(RequestModel requestModel, Country country, List<NationalNumberValidations> selectedValidations = null)
        {
            // Use the factory to set the appropriate strategy based on the country
            var strategy = _strategyFactory.Create(country);
            _validatorContext.SetValidationStrategy(strategy);

            return _validatorContext.ValidateNationalNumber(requestModel, selectedValidations);
        }

        public ValidationResult<InformationByNationalNumber> GetInformationByNationalNumber(string nationalNumber, Country country)
        {
            // Use the factory to set the appropriate strategy based on the country
            var strategy = _strategyFactory.Create(country);
            _validatorContext.SetValidationStrategy(strategy);

            return _validatorContext.GetInformationByNationalNumber(nationalNumber);
        }
    }




}
