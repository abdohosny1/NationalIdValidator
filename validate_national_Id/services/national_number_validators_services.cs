using System;
using validate_national_Id.models;

namespace validate_national_Id.services
{
    public static class national_number_validators_services
    {
        public static bool validation_national_number(this string national_number, request_model request_Model)
        {
            // Length = 14 and only numbers
            if (string.IsNullOrEmpty(national_number) || national_number.Length != 14 || !long.TryParse(national_number, out _))
            {
                return false;
            }

            // Century validation
            var birth_year = request_Model.birth_date.Year;
            var century_code = int.Parse(national_number.Substring(0, 1));
            if (century_code != 2 && century_code != 3)
            {
                return false;
            }

            if (century_code == 2 && (birth_year < 1900 || birth_year > 1999))
            {
                return false;
            }

            if (century_code == 3 && (birth_year < 2000 || birth_year > 2099))
            {
                return false;
            }

            // Birth date validation
            var birth_date_string = national_number.Substring(1, 6);
            var year = int.Parse(birth_date_string.Substring(0, 2));
            var month = int.Parse(birth_date_string.Substring(2, 2));
            var day = int.Parse(birth_date_string.Substring(4, 2));

            year += century_code == 3 ? 2000 : 1900;

            if (!DateOnly.TryParse($"{year}-{month}-{day}", out DateOnly birth_date) || birth_date != request_Model.birth_date)
            {
                return false;
            }

            // Province code validation
            var province_code = national_number.Substring(7, 2);
            if (!int.TryParse(province_code, out int province_value) || province_value != (int)request_Model.provincial_code)
            {
                return false;
            }

            // Gender validation
            var gender_code = int.Parse(national_number.Substring(12, 1));
            if ((request_Model.is_male && gender_code % 2 == 0) || (!request_Model.is_male && gender_code % 2 != 0))
            {
                return false;
            }

            return true;
        }
    }
}
