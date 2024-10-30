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
