using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageLogic
{
    public class ValidationCheck
    {
        public const byte k_NameMinLength = 1;

        public static bool ValidationCheckForName(string i_TestedModelName)
        {
            bool nameIsLegal = false;

            foreach (char letterToCheck in i_TestedModelName)
            {
                if (char.IsLetter(letterToCheck) == false)
                {
                    throw new ArgumentException(string.Format($"Name must be a english letters."));
                }
            }

            if (i_TestedModelName.Length >= k_NameMinLength)
            {
                nameIsLegal = true;
            }
            else
            {
                throw new ArgumentException(string.Format($"Name must be at list {k_NameMinLength} character."));
            }

            return nameIsLegal;
        }

        public static bool ValidationCheckForStringNumbers(string i_TestedStringNumbers, byte i_DesiredStringLength, string i_ErrorMsg)
        {
            bool stringIsLegal = false;

            if (i_TestedStringNumbers.Length >= i_DesiredStringLength)
            {
                stringIsLegal = true;

                foreach (Char licenseNumberDigit in i_TestedStringNumbers)
                {
                    if (Char.IsDigit(licenseNumberDigit) == false)
                    {
                        stringIsLegal = false;
                        break;
                    }
                }
            }
            else
            {
                throw new Exception(i_ErrorMsg);
            }

            return stringIsLegal;
        }

        public static bool ValidationCheckForAmount(string i_TestedAmount, out float io_FloatAmount)
        {
            bool amountIsLegal = float.TryParse(i_TestedAmount, out io_FloatAmount);

            if (amountIsLegal == false)
            {
                throw new FormatException();
            }

            return amountIsLegal;
        }

        public static bool ValidationBooleanDataCheck(string i_TestedBooleanValue)
        {
            bool isDataBoolean = false;

            if (i_TestedBooleanValue == "true" || i_TestedBooleanValue == "false")
            {
                isDataBoolean = true;
            }

            return isDataBoolean;
        }

        public static bool ValidationCheckForDataRangeValue(string i_TestedDataValue, Vehicle.QuestionForVehicleInformation i_ValueRange)
        {
            float dataInFloat;
            bool isDataINRange = false;
            bool dateIsLegal = ValidationCheckForAmount(i_TestedDataValue, out dataInFloat);

            if (dateIsLegal == true)
            {
                if (dataInFloat <= i_ValueRange.r_MaxRange && dataInFloat >= i_ValueRange.r_MinRange)
                {
                    isDataINRange = true;
                }
            }

            return isDataINRange;
        }
    }
}
