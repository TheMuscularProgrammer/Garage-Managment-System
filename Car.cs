using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageLogic
{
    public class Car : Vehicle
    {
        private const byte k_CarColorMinValue = 1;
        private const byte k_CarColorMaxValue = 4;
        private const byte k_CarDoorsNumberMinValue = 1;
        private const byte k_CarDoorsNumberMaxValue = 4;
        private const float k_CarWheelsMaxAirPressure = 33;
        private eCarColor m_Color;
        private eCarDoorsNumber m_DoorsNumber;
        private static readonly List<string> sr_QuestionsForUser = new List<string> { "car's color", "car's doors number" };
        private const byte k_IndexForAskCarColor = 0;
        private const byte k_IndexForAskCarDoorsNumber = 1;
        private const byte k_IndexForGetCarColor = 4;
        private const byte k_IndexForGetCarDoorsNumber = 5;

        public enum eCarColor
        {
            White = 1,
            Black,
            Yellow,
            Red
        }

        public enum eCarDoorsNumber
        {
            Two = 1,
            Three,
            Four,
            Five
        }

        public Car(string i_LicenseNumber, Engine i_Engine) : base(i_LicenseNumber, eWheelsNumber.Five, k_CarWheelsMaxAirPressure, i_Engine) { }

        public eCarColor Color
        {
            get { return m_Color; }
            set
            {
                if (Enum.IsDefined(typeof(eCarColor), value) == true)
                {
                    m_Color = value;
                }
                else
                {
                    throw new ValueOutOfRangeException(k_CarColorMinValue, k_CarColorMaxValue, sr_QuestionsForUser[k_IndexForAskCarColor]);
                }
            }
        }

        public eCarDoorsNumber DoorsNumber
        {
            get { return m_DoorsNumber; }
            set
            {
                if (Enum.IsDefined(typeof(eCarDoorsNumber), value) == true)
                {
                    m_DoorsNumber = value;
                }
                else
                {
                    throw new ValueOutOfRangeException(k_CarDoorsNumberMinValue, k_CarDoorsNumberMaxValue,sr_QuestionsForUser[k_IndexForAskCarDoorsNumber]);
                }
            }
        }

        public override List<QuestionForVehicleInformation> AskForDataToVehicle()
        {
            List<QuestionForVehicleInformation> questionsForCarInfo = new List<QuestionForVehicleInformation>();

            questionsForCarInfo.AddRange(base.AskForDataToVehicle());
            questionsForCarInfo.Add(new QuestionForVehicleInformation(sr_QuestionsForUser[k_IndexForAskCarColor], QuestionForVehicleInformation.eValidationCheckType.ValidRangeCheck, k_CarColorMinValue, k_CarColorMaxValue));
            questionsForCarInfo.Add(new QuestionForVehicleInformation(sr_QuestionsForUser[k_IndexForAskCarDoorsNumber], QuestionForVehicleInformation.eValidationCheckType.ValidRangeCheck, k_CarDoorsNumberMinValue, k_CarDoorsNumberMaxValue));

            return questionsForCarInfo;
        }

        public override bool SetRemainingVehicleDetails(List<string> i_CurrentInfoToVehicle)
        {
            bool carDitailsSetSuccessfully = true;

            base.SetRemainingVehicleDetails(i_CurrentInfoToVehicle);
            eCarColor carColorInput = (eCarColor)(Enum.Parse(typeof(eCarColor), i_CurrentInfoToVehicle[k_IndexForGetCarColor]));
            eCarDoorsNumber carDoorsNumberInput = (eCarDoorsNumber)(Enum.Parse(typeof(eCarColor), i_CurrentInfoToVehicle[k_IndexForGetCarDoorsNumber]));

            if (Enum.IsDefined(typeof(eCarColor), carColorInput) == true && Enum.IsDefined(typeof(eCarDoorsNumber), carDoorsNumberInput) == true)
            {
                Color = carColorInput;
                DoorsNumber = carDoorsNumberInput;
            }
            else
            {
                throw new Exception(string.Format($"Worng input. You must enter number between {k_CarColorMinValue} to {k_CarColorMaxValue}."));
            }

            return carDitailsSetSuccessfully;
        }

        public override string ToString()
        {
            StringBuilder carDitails = new StringBuilder();

            carDitails.Append(base.ToString());
            carDitails.Append(string.Format($"Car's color is '{Color.ToString()}', there is {(int)(DoorsNumber) + 1} doors."));

            return carDitails.ToString();
        }
    }
}
