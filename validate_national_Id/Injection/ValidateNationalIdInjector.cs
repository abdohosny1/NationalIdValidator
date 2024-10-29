using Microsoft.Extensions.DependencyInjection;
using validate_national_Id.Implementation;
using validate_national_Id.ImplementFactory;
using validate_national_Id.interfaces;

namespace validate_national_Id.Injection
{
    public static class ValidateNationalIdInjector
    {
        public static void AddNationalIdValidator(this IServiceCollection services)
        {
            // Register the NationalIdValidatorContext as a scoped service
            services.AddScoped<NationalIdValidatorContext>();

            // Register the factory for validation strategies
            services.AddSingleton<INationalIdValidationStrategyFactory, NationalIdValidationStrategyFactory>();

            // Register the ValidateNationalId service
            services.AddScoped<ValidateNationalId>();

            // Register specific strategies if needed, this is optional
            services.AddTransient<INationalIdValidationStrategy, EgyptNationalIdValidationStrategy>();
            // Add more strategies for other countries
        }
    }
}
