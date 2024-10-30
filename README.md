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
dotnet add package validate_national_Id

How to Use
1. Basic Validation
You can validate the format and values of a national ID using this library. Here’s an example of how to perform basic validation:

``` shell

class Program
{
    static void Main(string[] args)
    {
        string nationalId = "National ID";

        // Create an instance of ValidateNationalId with the factory
        ValidateNationalId validateNationalId = new ValidateNationalId(new NationalIdValidationStrategyFactory());

        // Create the request model for validation
        var requestModel = new RequestModelEgypt
        {
            NationalNumber = nationalId,
            birth_date = new DateOnly(1997, 8, 1), // Example birth date
            provincial_code = ProvincialCodeEgypt.Cairo, // Example province code
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
    }
}

