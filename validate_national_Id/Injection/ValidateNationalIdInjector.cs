using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using validate_national_Id.Implementation;
using validate_national_Id.interfaces;

namespace validate_national_Id.Injection
{
    internal static class ValidateNationalIdInjector
    {
        public static void AddInMemoryCaching(this IServiceCollection services)
        {

            services.AddSingleton<IValidateNationalId, IValidateNationalId>();
        }
    }
}
