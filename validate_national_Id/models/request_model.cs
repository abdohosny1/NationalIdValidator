using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using validate_national_Id.Enums;

namespace validate_national_Id.models
{
 

    public record request_model(DateOnly birth_date , provincial_code provincial_code, bool is_male);
}
