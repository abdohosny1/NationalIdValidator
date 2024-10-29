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
        public DateOnly? birth_date { get; set; }
        public bool? is_male { get; set; }
        public string NationalNumber { get; set; }

    }
    public class RequestModelEgypt : RequestModel
    {

        public ProvincialCodeEgypt? provincial_code { get; set; }
    }


}
