using FluentAssertions.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using validate_national_Id.Enums;
using validate_national_Id.Implementation;
using validate_national_Id.interfaces;
using validate_national_Id.models;

namespace validate_national_Id_test
{
    public class ValidateNationalI_test
    {
        private readonly IValidateNationalId _validateNationalId; // Replace with your service class

        public ValidateNationalI_test()
        {
            _validateNationalId = new ValidateNationalI(); // Initialize your service
        }

        [Fact]
        public void GetInformationByNationalNumberEgypt_ValidNationalNumber_ReturnsCorrectInformation()
        {
            // Arrange
            string nationalNumber = "29110122689012"; // Example valid national number
            var expectedBirthDate = new DateOnly(1991, 10, 12);
            var expectedAge = DateTime.Now.Year - expectedBirthDate.Year;
            var expectedProvince = "Sohag"; // Adjust to your actual expected province name
            var expectedGender = "Male"; // Adjust based on the input

            // Act
            var result = _validateNationalId.GetInformationByNationalNumberEgypt(nationalNumber);

            // Assert
            Assert.Equal(expectedBirthDate, result.BirthDate);
            Assert.Equal(expectedAge, result.Age);
            Assert.Equal(expectedProvince, result.Provincial);
            Assert.Equal(expectedGender, result.Gender);
        }

        [Fact]
        public void GetInformationByNationalNumberEgypt_InvalidNationalNumber_Format_ThrowsArgumentException()
        {
            // Arrange
            string nationalNumber = "123"; // Invalid national number format

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _validateNationalId.GetInformationByNationalNumberEgypt(nationalNumber));
            Assert.Equal("Invalid National Number", exception.Message);
        }

        [Fact]
        public void GetInformationByNationalNumberEgypt_InvalidNationalNumber_Length_ThrowsArgumentException()
        {
            // Arrange
            string nationalNumber = "123456789012345"; // Too long

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _validateNationalId.GetInformationByNationalNumberEgypt(nationalNumber));
            Assert.Equal("Invalid National Number", exception.Message);
        }

        [Fact]
        public void GetInformationByNationalNumberEgypt_InvalidNationalNumber_NonNumeric_ThrowsArgumentException()
        {
            // Arrange
            string nationalNumber = "29ABC45678901"; // Contains non-numeric characters

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _validateNationalId.GetInformationByNationalNumberEgypt(nationalNumber));
            Assert.Equal("Invalid National Number", exception.Message);
        }

        [Fact]
        public void GetInformationByNationalNumberEgypt_InvalidBirthDate_ThrowsArgumentException()
        {
            // Arrange
            string nationalNumber = "29129999999999"; // Invalid date in the national number

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _validateNationalId.GetInformationByNationalNumberEgypt(nationalNumber));
            Assert.Equal("Invalid Birth Date in National Number", exception.Message);
        }


        [Fact]
        public void ValidateNationaNumberEgypt_AllValidationsSelected_ReturnsTrue()
        {
            // Arrange
            string nationalNumber = "29110122689012"; // Example valid national number
            var requestModel = new RequestModel
            {
                birth_date = new DateOnly(1991, 10, 12),
                provincial_code = ProvincialCode.Sohag, // Adjust as necessary
                is_male = true
            };

            // Act
            var isValid = _validateNationalId.ValidateNationaNumberEgypt(nationalNumber, requestModel);

            // Assert
            Assert.True(isValid);
        }


        [Fact]
        public void ValidateNationaNumberEgypt_PartialValidationsSelected_ReturnsTrue()
        {
            // Arrange
            string nationalNumber = "29110122689012"; // Example valid national number
            var requestModel = new RequestModel
            {
                birth_date = new DateOnly(1991, 10, 12),
                provincial_code = ProvincialCode.Dakahlia,
                is_male = true
            };

            var selectedValidations = new List<NationalNumberValidations>
        {
            NationalNumberValidations.ValidateGender,
            NationalNumberValidations.ValidateBirthDate
        };

            // Act
            var isValid = _validateNationalId.ValidateNationaNumberEgypt(nationalNumber, requestModel, selectedValidations);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void ValidateNationaNumberEgypt_GenderValidation_Fails_ReturnsFalse()
        {
            // Arrange
            string nationalNumber = "29123456789012"; // Example valid national number
            var requestModel = new RequestModel
            {
                birth_date = new DateOnly(1992, 1, 23),
                provincial_code = ProvincialCode.Dakahlia,
                is_male = false // This is female
            };

            // Act
            var isValid = _validateNationalId.ValidateNationaNumberEgypt(nationalNumber, requestModel);

            // Assert
            Assert.False(isValid);
        }
    }
}
