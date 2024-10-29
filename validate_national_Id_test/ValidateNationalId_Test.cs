using System;
using System.Collections.Generic;
using validate_national_Id.Enums;
using validate_national_Id.Implementation;
using validate_national_Id.ImplementFactory;
using validate_national_Id.models;
using Xunit;

public class ValidateNationalId_Test
{
    private readonly ValidateNationalId _validateNationalId;

    public ValidateNationalId_Test()
    {
        // Create a new instance of the factory
        var strategyFactory = new NationalIdValidationStrategyFactory();
        // Pass the factory to the ValidateNationalId constructor
        _validateNationalId = new ValidateNationalId(strategyFactory);
    }

    [Fact]
    public void ValidateNationalNumber_MissingBirthDate_ShouldReturnFailure()
    {
        // Arrange
        var requestModel = new RequestModelEgypt
        {
            NationalNumber = "30001010100123",
            // Missing birth date
            is_male = true,
            provincial_code = ProvincialCodeEgypt.Cairo
        };

        // Act
        var result = _validateNationalId.ValidateNationalNumber(requestModel, Country.Egypt);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal("Invalid request model for Egyptian national ID.", result.Message);
    }

    [Fact]
    public void ValidateNationalNumber_MissingisMale_ShouldReturnFailure()
    {
        // Arrange
        var requestModel = new RequestModelEgypt
        {
            NationalNumber = "30001010100123",
            birth_date = new DateOnly(1991, 10, 12), // Use DateOnly for birth_date
            provincial_code = ProvincialCodeEgypt.Cairo
        };

        // Act
        var result = _validateNationalId.ValidateNationalNumber(requestModel, Country.Egypt);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal("Invalid request model for Egyptian national ID.", result.Message);
    }

    [Fact]
    public void ValidateNationalNumber_Missing_provincial_code_ShouldReturnFailure()
    {
        // Arrange
        var requestModel = new RequestModelEgypt
        {
            NationalNumber = "30001010100123",
            birth_date = new DateOnly(1991, 10, 12), // Use DateOnly for birth_date
            is_male = true,
        };

        // Act
        var result = _validateNationalId.ValidateNationalNumber(requestModel, Country.Egypt);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal("Invalid request model for Egyptian national ID.", result.Message);
    }

    [Fact]
    public void ValidateNationalNumber_Missing_provincial_code_But_No_MakeValidate_ShouldReturnFailure()
    {
        // Arrange
        var requestModel = new RequestModelEgypt
        {
            NationalNumber = "30001010100123",
            birth_date = new DateOnly(1991, 10, 12), // Use DateOnly for birth_date
            is_male = true,
        };

        var nationalNumberValidations = new List<NationalNumberValidations> {
            NationalNumberValidations.ValidateBirthDate,
            NationalNumberValidations.ValidateProvince,
            NationalNumberValidations.ValidateGender,
            
        };

        // Act
        var result = _validateNationalId.ValidateNationalNumber(requestModel, Country.Egypt, nationalNumberValidations);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal("Invalid request model for Egyptian national ID.", result.Message);
    }

    [Fact]
    public void ValidateNationalNumber_Missing_birth_date_But_No_MakeValidate_ShouldReturnFailure()
    {
        // Arrange
        var requestModel = new RequestModelEgypt
        {
            NationalNumber = "30001010100123",
            provincial_code = ProvincialCodeEgypt.Sohag, // Assuming this is valid
            //birth_date = new DateOnly(19year: 91, 10, 12), // Use DateOnly for birth_date
            is_male = true,
        };

        var nationalNumberValidations = new List<NationalNumberValidations> {
            NationalNumberValidations.ValidateBirthDate,
            NationalNumberValidations.ValidateProvince,
            NationalNumberValidations.ValidateGender,

        };

        // Act
        var result = _validateNationalId.ValidateNationalNumber(requestModel, Country.Egypt, nationalNumberValidations);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal("Invalid request model for Egyptian national ID.", result.Message);
    }

    [Fact]
    public void ValidateNationalNumber_Missing_is_male_But_No_MakeValidate_ShouldReturnFailure()
    {
        // Arrange
        var requestModel = new RequestModelEgypt
        {
            NationalNumber = "30001010100123",
            provincial_code = ProvincialCodeEgypt.Sohag, // Assuming this is valid
            birth_date = new DateOnly(1991, 10, 12), // Use DateOnly for birth_date
            //is_male = true,
        };

        var nationalNumberValidations = new List<NationalNumberValidations> {
            NationalNumberValidations.ValidateBirthDate,
            NationalNumberValidations.ValidateProvince,
            NationalNumberValidations.ValidateGender,

        };

        // Act
        var result = _validateNationalId.ValidateNationalNumber(requestModel, Country.Egypt, nationalNumberValidations);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal("Invalid request model for Egyptian national ID.", result.Message);
    }



    [Fact]
    public void ValidateNationalNumber_ValidEgyptianNationalID_ReturnsValidResponse()
    {
        // Arrange
        var requestModel = new RequestModelEgypt
        {
            NationalNumber = "29110122689012", // Example valid national number
            birth_date = new DateOnly(1991, 10, 12), // Use DateOnly for birth_date
            provincial_code = ProvincialCodeEgypt.Sohag, // Assuming this is valid
            is_male = true
        };

        // Act
        var response = _validateNationalId.ValidateNationalNumber(requestModel, Country.Egypt);

        // Assert
        Assert.True(response.IsValid);
        Assert.Equal("Validation succeeded.", response.Message);
    }

    [Fact]
    public void ValidateNationalNumber_InvalidLength_ReturnsInvalidResponse()
    {
        // Arrange
        var requestModel = new RequestModelEgypt
        {
            NationalNumber = "3001234567890", // Invalid length
            birth_date = new DateOnly(2000, 1, 1), // Use DateOnly for birth_date
            provincial_code = ProvincialCodeEgypt.Cairo,
            is_male = true
        };

        // Act
        var response = _validateNationalId.ValidateNationalNumber(requestModel, Country.Egypt);

        // Assert
        Assert.False(response.IsValid);
        Assert.Equal("National number must be 14 digits long and numeric.", response.Message);
    }

    [Fact]
    public void ValidateNationalNumber_InvalidCenturyCode_ReturnsInvalidResponse()
    {
        // Arrange
        var requestModel = new RequestModelEgypt
        {
            NationalNumber = "40012345678901", // Invalid century code
            birth_date = new DateOnly(2000, 1, 1), // Use DateOnly for birth_date
            provincial_code = ProvincialCodeEgypt.Cairo,
            is_male = true
        };

        // Act
        var response = _validateNationalId.ValidateNationalNumber(requestModel, Country.Egypt);

        // Assert
        Assert.False(response.IsValid);
        Assert.Equal("Invalid century code in national number.", response.Message);
    }

    // Add more test cases as needed to cover other validations
}
