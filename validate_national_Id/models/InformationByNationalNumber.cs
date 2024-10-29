namespace validate_national_Id.models
{
    public class InformationByNationalNumber
    {
        public DateOnly BirthDate { get; set; }
        public int Age { get; set; }
        public string Provincial { get; set; }
        public string Gender { get; set; }
    }

    public class InformationByNationalNumberEgypt : InformationByNationalNumber
    {

    }

}
