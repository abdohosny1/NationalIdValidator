using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using validate_national_Id.Enums;
using validate_national_Id.models;

namespace validate_national_Id.interfaces
{
    public interface IValidateNationalId
    {
        bool ValidateNationaNumberEgypt(string nationalNumber, RequestModel requestModel, List<NationalNumberValidations> selectedValidations = null);
            InformationByNationalNumberEgypt GetInformationByNationalNumberEgypt(string nationalNumber);
    }
}
