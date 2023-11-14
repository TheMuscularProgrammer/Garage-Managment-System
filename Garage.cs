using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageLogic
{
    public class Garage
    {
        private const byte k_VehicleStatusMinValue = 0;
        private const byte k_VehicleStatusMaxValue = 3;
        private readonly Dictionary<string, ClientInformation> r_Clients;

        public enum eVehicleStatus
        {
            Repair = 1,
            Repaired,
            PayUp,
            General
        }

        public Garage()
        {
            r_Clients = new Dictionary<string, ClientInformation>();
        }

        public Dictionary<string, ClientInformation> Clients
        {
            get { return r_Clients; }
        }

        public static StringBuilder GetStringOfEnumValues(Type i_EnumValuesTypes)
        {
            StringBuilder listOfEnumValues = new StringBuilder();

            foreach (object valueTypes in Enum.GetValues(i_EnumValuesTypes))
            {
                listOfEnumValues.Append(string.Format("{0}.{1}{2}", (int)(valueTypes), valueTypes, Environment.NewLine));
            }

            return listOfEnumValues;
        }

        public void InsertNewVehicleClient(string i_ClientName, string i_ClientPhoneNumber, string i_LicenseNumber, Vehicle i_ClientVehicle)
        {
            Clients.Add(i_LicenseNumber, new ClientInformation(i_ClientName, i_ClientPhoneNumber, i_ClientVehicle));
        }

        public bool VehicleIsAlreadyAtTheGarage(string i_LicenseNumber)
        {
            return Clients.ContainsKey(i_LicenseNumber);
        }

        private List<string> makeGeneralLicenseNumbersList()
        {
            List<string> vehiclesLicenseNumbersList = new List<string>();

            foreach (string licenseNumberKey in Clients.Keys)
            {
                vehiclesLicenseNumbersList.Add(licenseNumberKey);
            }

            return vehiclesLicenseNumbersList;
        }

        private void makeLicenseNumbersListAccordingToVehicleStatus(eVehicleStatus i_VehicleStatus, out List<string> o_GetTheLicenseNumberAccordingToTheCarStatus)
        {
            o_GetTheLicenseNumberAccordingToTheCarStatus = new List<string>();

            foreach (KeyValuePair<string, ClientInformation> client in Clients)
            {
                if (client.Value.ClientVehicleStatus == i_VehicleStatus)
                {
                    o_GetTheLicenseNumberAccordingToTheCarStatus.Add(client.Key);
                }
            }
        }

        public void displayTheVehicelsByTheCurrentStatusThatTheUserChoice(eVehicleStatus i_UserChoiceToDisplayCarsByStatus)
        {
            StringBuilder collectAndPrintAllTheVehielsByTheirStatus = new StringBuilder();
            LinkedList<string> licensOfVehiclesList = new LinkedList<string>();

            foreach (KeyValuePair<string, ClientInformation> client in Clients)
            {
                if (client.Value.ClientVehicleStatus == i_UserChoiceToDisplayCarsByStatus)
                {
                    licensOfVehiclesList.AddFirst(client.Key);
                }
            }

            if (licensOfVehiclesList.Count == 0)
            {
                collectAndPrintAllTheVehielsByTheirStatus.AppendFormat($"no data to show about vehicels in {i_UserChoiceToDisplayCarsByStatus} status yet.");
            }
            else
            {
                foreach (string licenNumber in licensOfVehiclesList)
                {
                    collectAndPrintAllTheVehielsByTheirStatus.AppendLine(licenNumber);
                    collectAndPrintAllTheVehielsByTheirStatus.AppendLine(Environment.NewLine);
                }
            }

            Console.WriteLine(collectAndPrintAllTheVehielsByTheirStatus.ToString());
        }

        public void UpdateStatusToVehicleByGivenNewStatusAndLicens(string i_LicensToGetVehicle, eVehicleStatus i_NewStatusToUpdate)
        {
            Clients[i_LicensToGetVehicle].ClientVehicleStatus = i_NewStatusToUpdate;
        }

        public void InfluateTheWheelsOfTheVehicle(string i_LicenNumberOfChoosenVehicle)
        {
            ClientInformation informationAboutTheVehicleToInfluateTheWheels = Clients[i_LicenNumberOfChoosenVehicle];

            informationAboutTheVehicleToInfluateTheWheels.ClientVehicle.InfluateTheAirPressureOfWheelsToTheMax();
        }

        public void CheckIfVehicleCanBeFuled(string i_LicenNumberOfChoosenVehicle)
        {
            FuelEngine engine = r_Clients[i_LicenNumberOfChoosenVehicle].ClientVehicle.Engine as FuelEngine;

            if (engine == null)
            {
                throw new ArgumentException("Invalid engine type, can`t fuel this vehicle");
            }
        }

        public void CheckIfVehicleCanBeCharged(string i_LicenNumberOfChoosenVehicle)
        {
            ElectricEngine engine = r_Clients[i_LicenNumberOfChoosenVehicle].ClientVehicle.Engine as ElectricEngine;

            if (engine == null)
            {
                throw new ArgumentException("Invalid engine type, can`t charge this vehicle");
            }
        }

        public void RefuelEngine(string i_LicenNumber, FuelEngine.eFuelType i_FuelType, float i_FuelToAdd)
        {
            FuelEngine engineToRefill = r_Clients[i_LicenNumber].ClientVehicle.Engine as FuelEngine;

            engineToRefill.Refueling(i_FuelToAdd, i_FuelType);
            r_Clients[i_LicenNumber].ClientVehicle.UpdateRemainEnergyPercent();
        }

        public void ChargeEngine(string i_LicenNumber, float i_MinutesToAdd)
        {
            ElectricEngine engineToRecharge = r_Clients[i_LicenNumber].ClientVehicle.Engine as ElectricEngine;

            engineToRecharge.RechargeBattery(i_MinutesToAdd / 60);
            r_Clients[i_LicenNumber].ClientVehicle.UpdateRemainEnergyPercent();
        }

        public void DisplayFullDetailsOfVehicleByGivenLicenNumber(string i_LicenToDisplayFullDetails)
        {
            ClientInformation FullDetailsOfVehcile = Clients[i_LicenToDisplayFullDetails];

            Console.WriteLine($"Client name: {FullDetailsOfVehcile.ClientName}, ClientPhoneNumber: {FullDetailsOfVehcile.ClientPhoneNumber}, Client Vehicle Status: {FullDetailsOfVehcile.ClientVehicleStatus}");
            Console.WriteLine(FullDetailsOfVehcile.ClientVehicle.ToString());
        }

        public class ClientInformation
        {
            public const byte k_DesiredAmountOfPhoneNumberDigits = 10;
            private string m_ClientName;
            private string m_ClientPhoneNumber;
            private eVehicleStatus m_ClientVehicleStatus;
            private Vehicle m_ClientVehicle;

            public ClientInformation(string i_ClientName, string i_ClientPhoneNumber, Vehicle i_ClientVehicle)
            {
                m_ClientName = i_ClientName;
                m_ClientPhoneNumber = i_ClientPhoneNumber;
                m_ClientVehicleStatus = eVehicleStatus.Repair;
                m_ClientVehicle = i_ClientVehicle;
            }

            public string ClientName
            {
                get { return m_ClientName; }
                set
                {
                    if (ValidationCheck.ValidationCheckForName(value) == true)
                    {
                        m_ClientName = value;
                    }
                }
            }

            public string ClientPhoneNumber
            {
                get { return m_ClientPhoneNumber; }
                set
                {
                    if (ValidationCheck.ValidationCheckForStringNumbers(value, k_DesiredAmountOfPhoneNumberDigits, null) == true)
                    {
                        m_ClientPhoneNumber = value;
                    }
                }
            }

            public eVehicleStatus ClientVehicleStatus
            {
                get { return m_ClientVehicleStatus; }
                set
                {
                    if (Enum.IsDefined(typeof(eVehicleStatus), value) == true)
                    {
                        m_ClientVehicleStatus = value;
                    }
                    else
                    {
                        throw new ValueOutOfRangeException(k_VehicleStatusMinValue, k_VehicleStatusMaxValue, "Vehicle status");
                    }
                }
            }

            public Vehicle ClientVehicle
            {
                get { return m_ClientVehicle; }
                set { m_ClientVehicle = value; }
            }
        }
    }
}
