using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageLogic
{
    public class Motorcycle : Vehicle
    {
        private const byte k_MinLicenseTypeValue = 1;
        private const byte k_MaxLicenseTypeValue = 4;
        private const int k_MinMotorcycleEngineCapacity = 1;
        private const float k_MotorcycleWheelsMaxAirPressure = 31;
        private eMotorcycleLicenseType m_LicenseType;
        private int m_EngineCapacity;
        private static readonly List<string> sr_QuestionsForUser = new List<string> { "license type", "engine capacity" };
        private const int k_IndexToAskLicenType = 0;
        private const int k_IndexToAskEngineCapacity = 1;
        private const int k_IndexToGetAnswerAboutLicenType = 4;
        private const int k_IndexToGetAnswerAboutEngineCapacity = 5;

        public enum eMotorcycleLicenseType
        {
            A1 = 1,
            A2,
            AA,
            B1
        }

        public Motorcycle(string i_LicenseNumber, Engine i_Engine) : base(i_LicenseNumber, eWheelsNumber.Two, k_MotorcycleWheelsMaxAirPressure, i_Engine) { }

        public int EngineCapacity
        {
            get { return m_EngineCapacity; }
            set
            {
                if (value >= k_MinMotorcycleEngineCapacity)
                {
                    m_EngineCapacity = value;
                }
                else
                {
                    throw new ValueOutOfRangeException(k_MinMotorcycleEngineCapacity);
                }
            }
        }

        public eMotorcycleLicenseType LicenseType
        {
            get { return m_LicenseType; }
            set
            {
                if (Enum.IsDefined(typeof(eMotorcycleLicenseType), value) == true)
                {
                    m_LicenseType = value;
                }
                else
                {
                    throw new ValueOutOfRangeException(k_MinLicenseTypeValue, k_MaxLicenseTypeValue, "License type");
                }
            }
        }

        public override List<QuestionForVehicleInformation> AskForDataToVehicle()
        {
            List<QuestionForVehicleInformation> questionsForMotorcycleInfo = new List<QuestionForVehicleInformation>();

            questionsForMotorcycleInfo.AddRange(base.AskForDataToVehicle());
            questionsForMotorcycleInfo.Add(new QuestionForVehicleInformation(sr_QuestionsForUser[k_IndexToAskLicenType], QuestionForVehicleInformation.eValidationCheckType.ValidRangeCheck, k_MinLicenseTypeValue, k_MaxLicenseTypeValue));
            questionsForMotorcycleInfo.Add(new QuestionForVehicleInformation(sr_QuestionsForUser[k_IndexToAskEngineCapacity], QuestionForVehicleInformation.eValidationCheckType.ValidRangeCheck, k_MinMotorcycleEngineCapacity));

            return questionsForMotorcycleInfo;
        }

        public override bool SetRemainingVehicleDetails(List<string> i_CurrentInfoToVehicle)
        {
            bool motorcycleDitailsSetSuccessfully = true;
            eMotorcycleLicenseType licenseTypeInput = (eMotorcycleLicenseType)(Enum.Parse(typeof(eMotorcycleLicenseType), i_CurrentInfoToVehicle[k_IndexToGetAnswerAboutLicenType]));
            int engineCapacityInput;

            base.SetRemainingVehicleDetails(i_CurrentInfoToVehicle);
            if (Enum.IsDefined(typeof(eMotorcycleLicenseType), licenseTypeInput) == true && int.TryParse(i_CurrentInfoToVehicle[k_IndexToGetAnswerAboutEngineCapacity], out engineCapacityInput) == true)
            {
                LicenseType = licenseTypeInput;
                EngineCapacity = engineCapacityInput;
            }
            else
            {
                throw new Exception(string.Format($"Worng input. You must enter number between {k_MinLicenseTypeValue} to {k_MaxLicenseTypeValue}."));
            }

            return motorcycleDitailsSetSuccessfully;
        }

        public override string ToString()
        {
            StringBuilder motorcycleDitails = new StringBuilder();

            motorcycleDitails.Append(base.ToString());
            motorcycleDitails.Append(string.Format($"Motorcycle engine capacity: {EngineCapacity} cc, license type: {LicenseType.ToString()}."));

            return motorcycleDitails.ToString();
        }
    }
}
