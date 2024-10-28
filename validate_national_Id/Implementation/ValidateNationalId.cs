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
    public class ValidateNationalI : IValidateNationalId
    {
        public InformationByNationalNumberEgypt GetInformationByNationalNumberEgypt(string nationalNumber)
        {
            // Validate the national number format
            if (string.IsNullOrEmpty(nationalNumber) || nationalNumber.Length != 14 || !long.TryParse(nationalNumber, out _))
            {
                throw new ArgumentException("Invalid National Number");
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
                throw new ArgumentException("Invalid Birth Date in National Number");
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
            var province = Enum.IsDefined(typeof(ProvincialCode), provinceCode)
                ? ((ProvincialCode)provinceCode).ToString()
                : "Unknown Province";

            // Extract gender code and determine gender
            var genderCode = int.Parse(nationalNumber.Substring(12, 1));
            var gender = genderCode % 2 == 0 ? "Female" : "Male";

            // Create and return the result
            return new InformationByNationalNumberEgypt
            {
                BirthDate = birthDate,
                Age = age,
                Provincial = province,
                Gender = gender
            };
        }
        public bool ValidateNationaNumberEgypt(string nationalNumber, RequestModel requestModel, List<NationalNumberValidations> selectedValidations = null)
        {
            // Length = 14 and only numbers
            if (string.IsNullOrEmpty(nationalNumber) || nationalNumber.Length != 14 || !long.TryParse(nationalNumber, out _))
            {
                return false;
            }

            // If selectedValidations is null or empty, we validate everything
            bool validateAll = selectedValidations == null || !selectedValidations.Any();

            // Century validation
            if (validateAll || selectedValidations.Contains(NationalNumberValidations.ValidateCentury))
            {
                var birthYear = requestModel.birth_date.Year;
                var centuryCode = int.Parse(nationalNumber.Substring(0, 1));
                if (centuryCode != 2 && centuryCode != 3)
                {
                    return false;
                }

                if (centuryCode == 2 && (birthYear < 1900 || birthYear > 1999))
                {
                    return false;
                }

                if (centuryCode == 3 && (birthYear < 2000 || birthYear > 2099))
                {
                    return false;
                }
            }

            // Birth date validation
            if (validateAll || selectedValidations.Contains(NationalNumberValidations.ValidateBirthDate))
            {
                var birthDateString = nationalNumber.Substring(1, 6);
                var year = int.Parse(birthDateString.Substring(0, 2));
                var month = int.Parse(birthDateString.Substring(2, 2));
                var day = int.Parse(birthDateString.Substring(4, 2));

                year += int.Parse(nationalNumber.Substring(0, 1)) == 3 ? 2000 : 1900;

                if (!DateOnly.TryParse($"{year}-{month}-{day}", out DateOnly birthDate) || birthDate != requestModel.birth_date)
                {
                    return false;
                }
            }

            // Province code validation
            if (validateAll || selectedValidations.Contains(NationalNumberValidations.ValidateProvince))
            {
                var provinceCode = nationalNumber.Substring(7, 2);
                if (!int.TryParse(provinceCode, out int provinceValue) || provinceValue != (int)requestModel.provincial_code)
                {
                    return false;
                }
            }

            // Gender validation
            if (validateAll || selectedValidations.Contains(NationalNumberValidations.ValidateGender))
            {
                var genderCode = int.Parse(nationalNumber.Substring(12, 1));
                if ((requestModel.is_male && genderCode % 2 == 0) || (!requestModel.is_male && genderCode % 2 != 0))
                {
                    return false;
                }
            }

            return true;
        }


    }

}
