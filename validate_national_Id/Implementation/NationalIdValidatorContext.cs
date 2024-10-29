using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using validate_national_Id.Enums;
using validate_national_Id.interfaces;
using validate_national_Id.models;

namespace validate_national_Id.Implementation
{
    internal class NationalIdValidatorContext
    {
        private INationalIdValidationStrategy _validationStrategy;

        // Method to set the strategy based on the country
        public void SetValidationStrategy(INationalIdValidationStrategy strategy)
        {
            _validationStrategy = strategy;
        }

        public ResponseModel ValidateNationalNumber( RequestModel requestModel, List<NationalNumberValidations> selectedValidations = null)
        {
            if (_validationStrategy == null)
            {
                throw new InvalidOperationException("Validation strategy not set.");
            }
            return _validationStrategy.ValidateNationalNumber( requestModel, selectedValidations);
        }

        public ValidationResult<InformationByNationalNumber> GetInformationByNationalNumber(string nationalNumber)
        {
            if (_validationStrategy == null)
            {
                throw new InvalidOperationException("Validation strategy not set.");
            }
            return _validationStrategy.GetInformationByNationalNumber(nationalNumber);
        }
    }
}
