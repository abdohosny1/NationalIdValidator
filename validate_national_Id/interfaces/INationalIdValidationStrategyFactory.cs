using validate_national_Id.Enums;

namespace validate_national_Id.interfaces
{
    public interface INationalIdValidationStrategyFactory
    {
        INationalIdValidationStrategy Create(Country country);
    }
}
