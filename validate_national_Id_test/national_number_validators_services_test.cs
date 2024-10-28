using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using validate_national_Id.Enums;
using validate_national_Id.models;
using validate_national_Id.services;

namespace validate_national_Id_test
{
    public class national_number_validators_services_test
    {
        [Theory]
        [InlineData("29504150101901","1995-04-15",provincial_code.Cairo,false,true)]

        //14 length
        [InlineData("295041501019", "1995-04-15", provincial_code.Cairo, false, false)]
        [InlineData("2950415010190121", "1995-04-15", provincial_code.Cairo, false, false)]

        //Only numbers
        [InlineData("2950415010190a", "1995-04-15", provincial_code.Cairo, false, false)]

         // Century
        [InlineData("19504150101803", "1995-04-15", provincial_code.Cairo, false, false)]
        [InlineData("32004150101823", "2020-04-15", provincial_code.Cairo, false, true)]
        [InlineData("29504150101803", "1995-04-15", provincial_code.Cairo, false, true)]
        [InlineData("42004150101823", "2020-04-15", provincial_code.Cairo, false, false)]
        [InlineData("39504150101823", "1995-04-15", provincial_code.Cairo, false, false)]
        [InlineData("22104150101823", "2021-04-15", provincial_code.Cairo, false, false)]

    // YYMMDD I
        [InlineData("29504150101812", "1995-04-15", provincial_code.Cairo, true, true)]
        [InlineData("30209130101812", "2002-09-13", provincial_code.Cairo, true, true)]
        [InlineData("29504150101892", "1995-04-15", provincial_code.Cairo, true, true)]
        [InlineData("30212240101892", "2002-12-23", provincial_code.Cairo, true, false)]

        // Provices
        [InlineData("30209050101851", "2002-09-05", provincial_code.Cairo, true, true)]
        [InlineData("30209050201851", "2002-09-05", provincial_code.Alexandria, true, true)]
        [InlineData("30209050301851", "2002-09-05", provincial_code.PortSaid, true, true)]
        [InlineData("30209050401851", "2002-09-05", provincial_code.Suez, true, true)]
        [InlineData("30209051101851", "2002-09-05", provincial_code.Damietta, true, true)]
        [InlineData("30209051201851", "2002-09-05", provincial_code.Dakahlia, true, true)]
        [InlineData("30209051301851", "2002-09-05", provincial_code.Sharqia, true, true)]
        [InlineData("30209051401851", "2002-09-05", provincial_code.Qalyubia, true, true)]
        [InlineData("30209051501851", "2002-09-05", provincial_code.KafrElSheikh, true, true)]
        [InlineData("30209051601851", "2002-09-05", provincial_code.Gharbia, true, true)]
        [InlineData("30209051701851", "2002-09-05", provincial_code.Monufia, true, true)]
        [InlineData("30209051801851", "2002-09-05", provincial_code.Beheira, true, true)]
        [InlineData("30209051901851", "2002-09-05", provincial_code.Ismailia, true, true)]
        [InlineData("30209052101851", "2002-09-05", provincial_code.Giza, true, true)]
        [InlineData("30209052201851", "2002-09-05", provincial_code.BeniSuef, true, true)]
        [InlineData("30209052301851", "2002-09-05", provincial_code.Fayoum, true, true)]
        [InlineData("30209052401851", "2002-09-05", provincial_code.Minya, true, true)]
        [InlineData("30209052501851", "2002-09-05", provincial_code.Asyut, true, true)]
        [InlineData("30209052601851", "2002-09-05", provincial_code.Sohag, true, true)]
        [InlineData("30209052701851", "2002-09-05", provincial_code.Qena, true, true)]
        [InlineData("30209052801851", "2002-09-05", provincial_code.Aswan, true, true)]
        [InlineData("30209052901851", "2002-09-05", provincial_code.Luxor, true, true)]
        [InlineData("30209053101851", "2002-09-05", provincial_code.RedSea, true, true)]
        [InlineData("30209053201851", "2002-09-05", provincial_code.NewValley, true, true)]
        [InlineData("30209053301851", "2002-09-05", provincial_code.Matruh, true, true)]
        [InlineData("30209053401851", "2002-09-05", provincial_code.NorthSinai, true, true)]
        [InlineData("30209053501851", "2002-09-05", provincial_code.SouthSinai, true, true)]
        [InlineData("30209058801851", "2002-09-05", provincial_code.OutsideTheRepublic, true, true)]
        [InlineData("30209050501851", "2002-09-05", provincial_code.OutsideTheRepublic, true, false)]


        // Male (male)
        [InlineData("29504150301891", "1995-04-15", provincial_code.PortSaid, true, true)]
        [InlineData("29504150301801", "1995-04-15", provincial_code.PortSaid, false, true)]
        [InlineData("29504150301811", "1995-04-15", provincial_code.PortSaid, true, true)]
        [InlineData("29504150301831", "1995-04-15", provincial_code.PortSaid, true, true)]
        [InlineData("29504150301851", "1995-04-15", provincial_code.PortSaid, true, true)]
        [InlineData("29504150301871", "1995-04-15", provincial_code.PortSaid, true, true)]
        [InlineData("29504150301871", "1995-04-15", provincial_code.PortSaid, false, false)]
        // Female (even)
        [InlineData("29504150301821", "1995-04-15", provincial_code.PortSaid, false, true)]
        [InlineData("29504150301841", "1995-04-15", provincial_code.PortSaid, false, true)]
        [InlineData("29504150301861", "1995-04-15", provincial_code.PortSaid, false, true)]
        [InlineData("29504150301881", "1995-04-15", provincial_code.PortSaid, false, true)]
        [InlineData("29504150301881", "1995-04-15", provincial_code.PortSaid, true, false)]
        public void validateNationalNumber_should_ReturnExpectedValude
            (string national_number, string birth_date, provincial_code provincial_code, bool is_male,bool expected_result)
        {
            //Arrange + Act
            var request_model = new request_model
            (
                DateOnly.Parse(birth_date), provincial_code,is_male
            );

            //Assert
            national_number.validation_national_number(request_model).Should().Be(expected_result);
        }
    }
}
