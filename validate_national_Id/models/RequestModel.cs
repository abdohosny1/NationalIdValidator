using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using validate_national_Id.Enums;

namespace validate_national_Id.models
{

    public class RequestModel
    {

        public DateOnly birth_date { get; set; }
        public ProvincialCode provincial_code { get; set; }
        public bool is_male { get; set; }
    }

    public class InformationByNationalNumberEgypt
    {
        public DateOnly BirthDate { get; set; }
        public int Age { get; set; }
        public string Provincial { get; set; }
        public string Gender { get; set; }
    }
}
