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
    public class EgyptNationalIdValidationStrategy : INationalIdValidationStrategy
    {
        public ResponseModel ValidateNationalNumber(RequestModel requestModel, List<NationalNumberValidations> selectedValidations = null)
        {
            var egyptRequestModel = requestModel as RequestModelEgypt;

            // Check if the request model is valid
            if (egyptRequestModel == null  
                || selectedValidations is null && (!egyptRequestModel.provincial_code.HasValue 
                 || !egyptRequestModel.birth_date.HasValue
                 || !egyptRequestModel.is_male.HasValue) )
               
            {
                return new ResponseModel { IsValid = false, Message = "Invalid request model for Egyptian national ID." };
            }

            if(selectedValidations is not null &&
              (  (selectedValidations.Contains(NationalNumberValidations.ValidateProvince) && !egyptRequestModel.provincial_code.HasValue)
                || (selectedValidations.Contains(NationalNumberValidations.ValidateBirthDate) && !egyptRequestModel.birth_date.HasValue)
                || (selectedValidations.Contains(NationalNumberValidations.ValidateGender) && !egyptRequestModel.is_male.HasValue)))
            {
                return new ResponseModel { IsValid = false, Message = "Invalid request model for Egyptian national ID." };

            }

            // Length = 14 and only numbers
            if (string.IsNullOrEmpty(egyptRequestModel.NationalNumber) ||
                egyptRequestModel.NationalNumber.Length != 14 ||
                !long.TryParse(egyptRequestModel.NationalNumber, out _))
            {
                return new ResponseModel { IsValid = false, Message = "National number must be 14 digits long and numeric." };
            }

            // If selectedValidations is null or empty, we validate everything
            bool validateAll = selectedValidations == null || !selectedValidations.Any();

            // Century validation
            if (validateAll || selectedValidations.Contains(NationalNumberValidations.ValidateCentury))
            {
                var birthYear = egyptRequestModel.birth_date.Value.Year;
                var centuryCode = int.Parse(egyptRequestModel.NationalNumber.Substring(0, 1));
                if (centuryCode != 2 && centuryCode != 3)
                {
                    return new ResponseModel { IsValid = false, Message = "Invalid century code in national number." };
                }

                if (centuryCode == 2 && (birthYear < 1900 || birthYear > 1999))
                {
                    return new ResponseModel { IsValid = false, Message = "Birth year is not in the valid range for century code 2." };
                }

                if (centuryCode == 3 && (birthYear < 2000 || birthYear > 2099))
                {
                    return new ResponseModel { IsValid = false, Message = "Birth year is not in the valid range for century code 3." };
                }
            }

            // Birth date validation
            if (validateAll || selectedValidations.Contains(NationalNumberValidations.ValidateBirthDate))
            {
                var birthDateString = egyptRequestModel.NationalNumber.Substring(1, 6);
                int year = int.Parse(birthDateString.Substring(0, 2));
                int month = int.Parse(birthDateString.Substring(2, 2));
                int day = int.Parse(birthDateString.Substring(4, 2));

                year += int.Parse(egyptRequestModel.NationalNumber.Substring(0, 1)) == 3 ? 2000 : 1900;

                if (!DateOnly.TryParse($"{year}-{month:D2}-{day:D2}", out DateOnly birthDate) ||
                    birthDate != egyptRequestModel.birth_date)
                {
                    return new ResponseModel { IsValid = false, Message = "Birth date in the national number does not match the provided birth date." };
                }
            }

            // Province code validation
            if (validateAll || selectedValidations.Contains(NationalNumberValidations.ValidateProvince))
            {
                var provinceCode = egyptRequestModel.NationalNumber.Substring(7, 2);
                if (!int.TryParse(provinceCode, out int provinceValue) || provinceValue != (int)egyptRequestModel.provincial_code)
                {
                    return new ResponseModel { IsValid = false, Message = "Province code in national number does not match the provided province code." };
                }
            }

            // Gender validation
            if (validateAll || selectedValidations.Contains(NationalNumberValidations.ValidateGender))
            {
                var genderCode = int.Parse(egyptRequestModel.NationalNumber.Substring(12, 1));
                if ((egyptRequestModel.is_male.Value && genderCode % 2 == 0) ||
                    (!egyptRequestModel.is_male.Value && genderCode % 2 != 0))
                {
                    return new ResponseModel { IsValid = false, Message = "Gender code in national number does not match the provided gender." };
                }
            }

            // If all validations pass, return a successful result
            return new ResponseModel { IsValid = true, Message = "Validation succeeded." };
        }

        public ValidationResult<InformationByNationalNumber> GetInformationByNationalNumber(string nationalNumber)
        {
            // Validate the national number format
            if (string.IsNullOrEmpty(nationalNumber) || nationalNumber.Length != 14 || !long.TryParse(nationalNumber, out _))
            {
                return new ValidationResult<InformationByNationalNumber>
                {
                    IsSuccess = false,
                    ErrorMessage = "Invalid National Number"
                };
            }

            // Extract century code and determine the birth year
            var centuryCode = int.Parse(nationalNumber.Substring(0, 1));
            int year = int.Parse(nationalNumber.Substring(1, 2));
            int month = int.Parse(nationalNumber.Substring(3, 2));
            int day = int.Parse(nationalNumber.Substring(5, 2));

            year += centuryCode == 3 ? 2000 : 1900;

            // Error handling for birth date parsing
            if (!DateOnly.TryParse($"{year}-{month:D2}-{day:D2}", out DateOnly birthDate))
            {
                return new ValidationResult<InformationByNationalNumber>
                {
                    IsSuccess = false,
                    ErrorMessage = "Invalid Birth Date in National Number"
                };
            }

            // Calculate the age
            int age = DateTime.Now.Year - birthDate.Year;
            var todayDateOnly = DateOnly.FromDateTime(DateTime.Now);
            if (birthDate > todayDateOnly.AddYears(-age))
            {
                age--;
            }

            // Extract province code and map it to the province enum
            var provinceCode = int.Parse(nationalNumber.Substring(7, 2));
            var province = Enum.IsDefined(typeof(ProvincialCodeEgypt), provinceCode)
                ? ((ProvincialCodeEgypt)provinceCode).ToString()
                : "Unknown Province";

            // Extract gender code and determine gender
            var genderCode = int.Parse(nationalNumber.Substring(12, 1));
            var gender = genderCode % 2 == 0 ? "Female" : "Male";

            // Create and return the result
            return new ValidationResult<InformationByNationalNumber>
            {
                IsSuccess = true,
                Data = new InformationByNationalNumberEgypt
                {
                    BirthDate = birthDate,
                    Age = age,
                    Provincial = province,
                    Gender = gender
                }
            };
        }
    }
}
