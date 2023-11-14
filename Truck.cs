using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageLogic
{
    class Truck : Vehicle
    {
        private const float k_LogicalPossibleMinCarryingWeight = 0;
        private const float k_TruckWheelsMaxAirPressure = 26;
        private float m_MaxCarryingWeight;
        private bool m_IsCarrierDangerousMaterials;
        private static readonly List<string> sr_QuestionsForUser = new List<string> { "is carrier dangerous materials", "maximum carrying weight" };
        private const int k_IndexToGetQuestionAboutIfTheTruckIsCarryingDangerousMaterials = 0;
        private const int k_IndexToGetQuestionAboutIfTheTruckMaxWeight = 1;
        private const int k_IndexToGetAnswerAboutIfTheTruckIsCarryingDangerousMaterials = 4;
        private const int k_IndexToGetAnswerAboutIfTheTruckMaxWeight = 5;

        public Truck(string i_LicenseNumber, Engine i_Engine) : base(i_LicenseNumber, eWheelsNumber.Fourteen, k_TruckWheelsMaxAirPressure, i_Engine) { }

        public float MaxCarryingWeight
        {
            get { return m_MaxCarryingWeight; }
            set
            {
                if (value > 0)
                {
                    m_MaxCarryingWeight = value;
                }
                else
                {
                    throw new ArgumentException("Maximum carrying Weight must be bigger then 0.");
                }
            }
        }

        public bool IsCarrierDangerousMaterials
        {
            get { return m_IsCarrierDangerousMaterials; }
            set { m_IsCarrierDangerousMaterials = value; }
        }

        public override List<QuestionForVehicleInformation> AskForDataToVehicle()
        {
            List<QuestionForVehicleInformation> questionsForTruckInfo = new List<QuestionForVehicleInformation>();

            questionsForTruckInfo.AddRange(base.AskForDataToVehicle());
            questionsForTruckInfo.Add(new QuestionForVehicleInformation(sr_QuestionsForUser[k_IndexToGetQuestionAboutIfTheTruckIsCarryingDangerousMaterials], QuestionForVehicleInformation.eValidationCheckType.ValidBooleanCheck));
            questionsForTruckInfo.Add(new QuestionForVehicleInformation(sr_QuestionsForUser[k_IndexToGetQuestionAboutIfTheTruckMaxWeight], QuestionForVehicleInformation.eValidationCheckType.ValidRangeCheck, (int)k_LogicalPossibleMinCarryingWeight, (int)m_MaxCarryingWeight));

            return questionsForTruckInfo;
        }

        public override bool SetRemainingVehicleDetails(List<string> i_CurrentInfoToVehicle)
        {
            bool truckDitailsSetSuccessfully = true;
            bool isCarrierDangerousMaterialsInput;
            float maxCarryingWeightInput;
            base.SetRemainingVehicleDetails(i_CurrentInfoToVehicle);

            if (bool.TryParse(i_CurrentInfoToVehicle[k_IndexToGetAnswerAboutIfTheTruckIsCarryingDangerousMaterials], out isCarrierDangerousMaterialsInput) == true & float.TryParse(i_CurrentInfoToVehicle[k_IndexToGetAnswerAboutIfTheTruckMaxWeight], out maxCarryingWeightInput) == true)
            {
                IsCarrierDangerousMaterials = isCarrierDangerousMaterialsInput;
                MaxCarryingWeight = maxCarryingWeightInput;
            }
            else
            {
                truckDitailsSetSuccessfully = false;
            }

            return truckDitailsSetSuccessfully;
        }

        public override string ToString()
        {
            StringBuilder truckDitails = new StringBuilder();

            truckDitails.Append(base.ToString());
            truckDitails.Append(string.Format($"Truck's maximum carrying weight is {MaxCarryingWeight} kg,"));
            if (m_IsCarrierDangerousMaterials == true)
            {
                truckDitails.Append(string.Format($"And he is carrying dangerous materials."));
            }
            else
            {
                truckDitails.Append(string.Format($"And he is NOT carrying dangerous materials."));
            }

            return truckDitails.ToString();
        }
    }
}
