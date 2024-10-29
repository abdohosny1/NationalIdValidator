using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using validate_national_Id.Enums;
using validate_national_Id.models;

namespace validate_national_Id.interfaces
{
    public interface INationalIdValidationStrategy
    {
        ResponseModel ValidateNationalNumber(RequestModel requestModel, List<NationalNumberValidations> selectedValidations = null);
        ValidationResult<InformationByNationalNumber> GetInformationByNationalNumber(string nationalNumber);
    }
}
