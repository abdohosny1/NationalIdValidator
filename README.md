# NationalIdValidator

[![NuGet version](https://badge.fury.io/nu/validate_national_Id.svg)](https://www.nuget.org/packages/validate_national_Id)

The `NationalIdValidator` library provides robust validation and information extraction functionalities for national IDs across various countries. It follows the **Strategy Design Pattern** to allow easy extension of validation strategies for different countries.

## Features

- **Validation of National ID**: Validate length, century code, birth date, province code, and gender using customizable validation strategies.
- **Information Extraction**: Extract birth date, age, province, and gender information from a valid national ID.
- **Customizable Validation**: Choose which validations to apply (e.g., validate birth date, province, or gender separately).

## Installation

Install via NuGet Package Manager Console:

```shell
dotnet add package NationalIdValidator --version 1.0.0
```

## How to Use
## 1. Basic Validation
You can validate the format and values of a national ID using this library. Here’s an example of how to perform basic validation:

``` shell

        string nationalId = "nationalId"; // Example National ID

        // Create an instance of ValidateNationalId with the factory
        ValidateNationalId validateNationalId = new ValidateNationalId(new NationalIdValidationStrategyFactory());

        // Create the request model for validation
        var requestModel = new RequestModelEgypt
        {
            NationalNumber = nationalId,
            birth_date = new DateOnly(1991, 10, 12), // Example birth date
            provincial_code = ProvincialCodeEgypt.Sohag, // Example province code
            is_male = true // Example gender
        };

        // Perform validation
        var response = validateNationalId.ValidateNationalNumber(requestModel, Country.Egypt);

        // Display the validation result
        if (response.IsValid)
        {
            Console.WriteLine("The National ID is valid!");
        }
        else
        {
            Console.WriteLine($"Validation failed: {response.Message}");
        }

        // Check information extraction
        var info = validateNationalId.GetInformationByNationalNumber(nationalId, Country.Egypt);
        if (info.IsSuccess)
        {
            Console.WriteLine($"Age: {info.Data.Age}, Birth Date: {info.Data.BirthDate}, Province: {info.Data.Provincial}, Gender: {info.Data.Gender}");
        }
        else
        {
            Console.WriteLine($"Information extraction failed: {info.ErrorMessage}");
        }
```
## 2. Customizable Validation
You can specify which validations to perform by using the selectedValidations parameter. For example:
```shell
var selectedValidations = new List<NationalNumberValidations>
{
    NationalNumberValidations.ValidateBirthDate,
    NationalNumberValidations.ValidateProvince
};

var response = validateNationalId.ValidateNationalNumber(requestModel, Country.Egypt, selectedValidations);
```
## 3. ASP.NET Core Integration
To integrate NationalIdValidator into your ASP.NET Core application, register the service in program.cs:

```shell
builder.Services.AddNationalIdValidator();
```

Then, inject ValidateNationalId into your controllers:
```shell
public class ValidatorController : Controller
{
    private readonly ValidateNationalId validateNationalId;

    public ValidatorController(ValidateNationalId validateNationalId)
    {
        this.validateNationalId = validateNationalId;
    }

    public IActionResult Index()
    {
        return View();
    }
}

```
Contact
For inquiries or feedback, please reach out to abdelaliemhosny18@gmail.com.

    

