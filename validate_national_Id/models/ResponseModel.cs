namespace validate_national_Id.models
{
    public class ResponseModel
    {
        public bool IsValid { get; set; }
        public string Message { get; set; }

    }

    public class ValidationResult<T>
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public T Data { get; set; }
    }

}
