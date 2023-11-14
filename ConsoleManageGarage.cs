using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using GarageLogic;

namespace ConsoleUI
{
    public class ConsoleManageGarage
    {
        private const byte k_InitValue = 0;
        private const byte k_MinLicenseNumberLength = 1;
        private const byte k_PhoneNumberLength = 1;
        private static readonly Garage sr_Garage = new GarageLogic.Garage();

        public static Garage Garage
        {
            get { return sr_Garage; }
        }

        private enum eSystemMenuOption
        {
            InsertNewVehicle = 1,
            DisplayLicenseNumbers,
            ChangeVehicleStatus,
            InflateAirWheelsToMaximum,
            RefuelingRegularVehicle,
            RechargeElectricVehicle,
            DisplayVehicleDetailsByLicenseNumbers,
            Exit
        }

        public static void StartGarageManageSystem()
        {
            Console.WriteLine(string.Format(@"Welcome to our garage! Please follow the following menu: "));
            showGarageManageSystemMenu();
        }

        private static void showGarageManageSystemMenu()
        {
            string systemManu;
            eSystemMenuOption userCurrentChoice = k_InitValue;

            systemManu = string.Format(@"===========================================================================
1. Insert a new car to the garage.
2. Display garage vehicle license numbers.
3. Change vehicle status.
4. Inflate air in a vehicle's wheels to the maximum.
5. Refuel a vehicle that is powered by fuel.
6. Charge an electric vehicle.
7. Display Vehicle Details By License Number.
8. Exit
===========================================================================");
            string invalidOperationMsg = string.Format("As a result, the operation you attempted was not successful. Please try again!");
            while (userCurrentChoice != eSystemMenuOption.Exit)
            {
                try
                {
                    Console.WriteLine(systemManu);
                    getUserMenuChoice(out userCurrentChoice);
                    executUserChoice(userCurrentChoice);
                }
                catch (FormatException i_FormatException)
                {
                    Console.WriteLine(string.Format(@"{0} {1}", i_FormatException.Message, invalidOperationMsg));
                }
                catch (ArgumentException i_ArgumentException)
                {
                    Console.WriteLine(string.Format(@"{0} {1}", i_ArgumentException.Message, invalidOperationMsg));
                }
                catch (ValueOutOfRangeException i_ValueOutOfRangeException)
                {
                    Console.WriteLine(string.Format(@"{0} {1}", i_ValueOutOfRangeException.Message, invalidOperationMsg));
                }
                catch (Exception i_Exception)
                {
                    Console.WriteLine(string.Format(@"{0} {1}", i_Exception.Message, invalidOperationMsg));
                }
                finally
                {
                    Thread.Sleep(4000);
                    Console.Clear();
                }
            }
        }

        private static void getUserMenuChoice(out eSystemMenuOption o_GetTheUserChoice)
        {
            string msg;

            bool isByteInput = false;
            bool vaildInput = false;
            eSystemMenuOption userCurrentChoice = k_InitValue;

            msg = string.Format(@"Please select number from 1 to 8 that represents the action you want to perform in the garage: ");
            Console.Write(msg);
            while (vaildInput == false)
            {
                isByteInput = Enum.TryParse(Console.ReadLine(), out userCurrentChoice);
                if (isByteInput == true)
                {
                    vaildInput = Enum.IsDefined(typeof(eSystemMenuOption), userCurrentChoice);
                }

                if (vaildInput == false)
                {
                    Console.Write("Invaild input, Let's try again: ");
                }
            }

            o_GetTheUserChoice = userCurrentChoice;
        }

        private static void executUserChoice(eSystemMenuOption i_UserCurrentChoice)
        {
            switch (i_UserCurrentChoice)
            {
                case eSystemMenuOption.InsertNewVehicle:
                    {
                        insertVehicleToGarage();
                        break;
                    }

                case eSystemMenuOption.DisplayLicenseNumbers:
                    {
                        displayLicenseNumbers();
                        break;
                    }

                case eSystemMenuOption.ChangeVehicleStatus:
                    {
                        changeVehicleStatus();
                        break;
                    }

                case eSystemMenuOption.InflateAirWheelsToMaximum:
                    {
                        inflateAirWheelsToMaximum();
                        break;
                    }

                case eSystemMenuOption.RefuelingRegularVehicle:
                    {
                        refuelingRegularVehicle();
                        break;
                    }

                case eSystemMenuOption.RechargeElectricVehicle:
                    {
                        rechargeElectricVehicle();
                        break;
                    }

                case eSystemMenuOption.DisplayVehicleDetailsByLicenseNumbers:
                    {
                        displayVehicleDetailsByLicenseNumbers();
                        break;
                    }

                case eSystemMenuOption.Exit:
                    {
                        Console.WriteLine("You decided to get out of the garage, have a wonderful day!");
                        break;
                    }
            }
        }

        private static void insertVehicleToGarage()
        {
            string licenseNumberToCreateNewVehicle = getLicenseNumber();
            bool isTheVehicleAlredyInTheGarage = Garage.VehicleIsAlreadyAtTheGarage(licenseNumberToCreateNewVehicle);

            if (isTheVehicleAlredyInTheGarage == true)
            {
                Console.WriteLine("Vehicle is already at the garag! It is in repair.");
            }
            else
            {
                createNewVehicle(licenseNumberToCreateNewVehicle);
            }
        }

        private static void createNewVehicle(string io_LicenseNumber)
        {
            Vehicle newVehicleToCreate = null;
            List<Vehicle.QuestionForVehicleInformation> vehicleInformationQuestion;
            List<string> vehicleInformationAnswers = new List<string>();
            string clientName, clientPhoneNumber, vehicleType;
            bool isVaildAnswers = false;
            bool vehicleDitailsSetSuccessfully = false;

            vehicleType = getClientVehicleType();
            newVehicleToCreate = VehicleCreator.CreateVehicle(vehicleType, io_LicenseNumber);
            getClientDetailsForNewVehicle(out clientName, out clientPhoneNumber);
            vehicleInformationQuestion = newVehicleToCreate.AskForDataToVehicle();
            foreach (Vehicle.QuestionForVehicleInformation question in vehicleInformationQuestion)
            {
                vehicleInformationAnswers.Add(getAnswerToVehicleInformationQuestion(question, out isVaildAnswers));
            }

            vehicleDitailsSetSuccessfully = newVehicleToCreate.SetRemainingVehicleDetails(vehicleInformationAnswers);
            if (vehicleDitailsSetSuccessfully != false)
            {
                Garage.InsertNewVehicleClient(clientName, clientPhoneNumber, io_LicenseNumber, newVehicleToCreate);
                Console.WriteLine("Vehicle was successfully insert to the garage!");
            }
        }

        private static string getAnswerToVehicleInformationQuestion(Vehicle.QuestionForVehicleInformation i_question, out bool o_IsVaildAnswer)
        {
            string informationAnswer;
            string selectFromEnumMsg = string.Format($"Please select type of {i_question.r_AskForData} from the following list: {Environment.NewLine}");
            string defaultMsg = string.Format($"Please enter vehicle {i_question.r_AskForData}: ");

            switch (i_question.r_AskForData)
            {
                case "car's color":
                    {
                        Console.Write(string.Format($"{selectFromEnumMsg}{Garage.GetStringOfEnumValues(typeof(Car.eCarColor))}(insert your chosen number): "));
                        break;
                    }
                case "car's doors number":
                    {
                        Console.Write(string.Format($"{selectFromEnumMsg}{Garage.GetStringOfEnumValues(typeof(Car.eCarDoorsNumber))}(insert your chosen number): "));
                        break;
                    {
                        Console.Write(string.Format($"{selectFromEnumMsg}{Garage.GetStringOfEnumValues(typeof(Motorcycle.eMotorcycleLicenseType))}(insert your chosen number): "));
                        break;
                    }
                    }
                case "license type":
                    {
                        Console.Write(string.Format(@"{0}{1}(insert your chosen number): ", selectFromEnumMsg, Garage.GetStringOfEnumValues(typeof(Motorcycle.eMotorcycleLicenseType))));
                        break;
                    }
                case "is carrier dangerous materials":
                    {
                        Console.Write($"Does your vehicle {i_question.r_AskForData}? answer true or false: ");
                        break;
                    }
                default:
                    {
                        Console.Write(defaultMsg);
                        break;
                    }
            }

            informationAnswer = Console.ReadLine();
            o_IsVaildAnswer = i_question.AnswerValidationCheck(informationAnswer);

            return informationAnswer;
        }

        private static string getClientVehicleType()
        {
            string inputVehicleType = null;
            bool vehicleTypeInRange = false;

            Console.Write(string.Format($"Please select the type of vehicle you would like to create from the following list:{Environment.NewLine}{Garage.GetStringOfEnumValues(typeof(VehicleCreator.eVehicleType))}(insert your chosen number): "));
            while (vehicleTypeInRange == false)
            {
                inputVehicleType = Console.ReadLine();
                vehicleTypeInRange = VehicleCreator.IsVehicleTypeInRange(inputVehicleType);

                if (vehicleTypeInRange == false)
                {
                    Console.Write("Wrong input choice. Let't try again: ");
                }
            }

            return inputVehicleType;
        }

        private static Vehicle createVehicle(string i_LicenseNumber)
        {
            string chosenVehicleType;
            Vehicle vehicleToCreate = null;

            while (vehicleToCreate == null)
            {
                Console.WriteLine(string.Format($"Please choose vehicle type (enter from the following  indexes): {Environment.NewLine}{(typeof(VehicleCreator.eVehicleType))}"));
                chosenVehicleType = Console.ReadLine();
                vehicleToCreate = VehicleCreator.CreateVehicle(chosenVehicleType, i_LicenseNumber);
            }

            return vehicleToCreate;
        }

        private static void getClientDetailsForNewVehicle(out string o_ClientName, out string o_ClientPhoneNumber)
        {
            o_ClientName = getClientName();
            o_ClientPhoneNumber = getClientPhoneNumber();
        }

        private static string getClientName()
        {
            string clientName;

            Console.Write("Please enter vehicle's client name: ");
            clientName = Console.ReadLine();
            ValidationCheck.ValidationCheckForName(clientName);

            return clientName;
        }

        private static string getClientPhoneNumber()
        {
            string clientPhoneNumber;
            string errorMsg = "phone need to be numbers";

            Console.Write("Please enter vehicle's client phone number: ");
            clientPhoneNumber = Console.ReadLine();
            ValidationCheck.ValidationCheckForStringNumbers(clientPhoneNumber, k_PhoneNumberLength, errorMsg);

            return clientPhoneNumber;
        }

        private static string getLicenseNumber()
        {
            string licenseNumber;
            string invalidLicenseNumberMsg = "licen need to be just numbers or letters";

            Console.Write("Please enter the license number of your vehicle: ");
            licenseNumber = Console.ReadLine();
            ValidationCheck.ValidationCheckForStringNumbers(licenseNumber, k_MinLicenseNumberLength, invalidLicenseNumberMsg);

            return licenseNumber;
        }

        private static float getInputInRange(string i_Messege, float i_Min, float i_Max)
        {
            string userChoiceString;
            float userChoice;

            Console.Write(i_Messege);
            userChoiceString = Console.ReadLine();
            while (!float.TryParse(userChoiceString, out userChoice) || userChoice < i_Min || userChoice > i_Max)
            {
                Console.Write(string.Format($"Invalid input! please insert a number between {i_Min} to {i_Max}: "));
                userChoiceString = Console.ReadLine();
            }

            return userChoice;
        }

        private static void displayLicenseNumbers()
        {
            string userChoice;
            Garage.eVehicleStatus userChoiceToDisplayVehicelsInGarage;

            Console.Write(string.Format($"Please enter the status of the vehicles you want to display:{Environment.NewLine}1.Repair{Environment.NewLine}2.Repaired{Environment.NewLine}3.PayUp{Environment.NewLine}"));
            userChoice = Console.ReadLine();
            checkIfTheDataIsValid(userChoice);
            userChoiceToDisplayVehicelsInGarage = (Garage.eVehicleStatus)(Enum.Parse(typeof(Garage.eVehicleStatus), userChoice));
            Console.WriteLine("this is the vehicels in the status you choose: ");
            Garage.displayTheVehicelsByTheCurrentStatusThatTheUserChoice(userChoiceToDisplayVehicelsInGarage);
        }

        private static void checkIfTheDataIsValid(string i_UserChoice)
        {
            if (i_UserChoice.Length != 1)
            {
                throw new Exception("you have only 3 options. press 1 for repair. press 2 for repaired. press 3 for payup.");
            }

            if (int.Parse(i_UserChoice) != 1 && int.Parse(i_UserChoice) != 2 && int.Parse(i_UserChoice) != 3)
            {
                throw new Exception("you have only 3 options. press 1 for repair. press 2 for repaired. press 3 for payup.");
            }
        }

        private static void changeVehicleStatus()
        {
            string licensNumberOfVehicleToUpdateHisStatus;
            string vehicleNewStatusFromUser;
            string errorMesage = "licen need to be only digit and numbers.";
            Garage.eVehicleStatus vehicleNewStatus;

            Console.WriteLine("please enter a licens number: ");
            licensNumberOfVehicleToUpdateHisStatus = Console.ReadLine();
            ValidationCheck.ValidationCheckForStringNumbers(licensNumberOfVehicleToUpdateHisStatus, k_MinLicenseNumberLength, errorMesage);
            Console.WriteLine(string.Format($"please enter a new status to the vehicle you choose:{Environment.NewLine}1.chagne it to repair{Environment.NewLine}2.chagne it to repaired{Environment.NewLine}3.chagne it to payup."));
            vehicleNewStatusFromUser = Console.ReadLine();
            checkIfTheInputIsValidAndUpdateStatus(vehicleNewStatusFromUser, out vehicleNewStatus);
            Garage.UpdateStatusToVehicleByGivenNewStatusAndLicens(licensNumberOfVehicleToUpdateHisStatus, vehicleNewStatus);
            Console.WriteLine("The Status changed!");
        }

        private static void checkIfTheInputIsValidAndUpdateStatus(string i_UserInput, out Garage.eVehicleStatus o_StatusToUpdate)
        {
            o_StatusToUpdate = Garage.eVehicleStatus.General;
            if (int.Parse(i_UserInput) != 1 && int.Parse(i_UserInput) != 2 && int.Parse(i_UserInput) != 3)
            {
                throw new Exception("you have only 3 options. press 1 for repair. press 2 for repaired. press 3 for payup.");
            }

            if (int.Parse(i_UserInput) == 1)
            {
                o_StatusToUpdate = Garage.eVehicleStatus.Repair;
            }

            if (int.Parse(i_UserInput) == 2)
            {
                o_StatusToUpdate = Garage.eVehicleStatus.Repaired;
            }

            if (int.Parse(i_UserInput) == 3)
            {
                o_StatusToUpdate = Garage.eVehicleStatus.PayUp;
            }
        }

        private static void inflateAirWheelsToMaximum()
        {
            string licenNumberOfVehicleToInflateTheWheels;

            Console.WriteLine("please enter a licen number of the vehicle that you want to inflate the air wheels to maximum.");
            licenNumberOfVehicleToInflateTheWheels = Console.ReadLine();
            ValidationCheck.ValidationCheckForStringNumbers(licenNumberOfVehicleToInflateTheWheels, k_MinLicenseNumberLength, "licen need to be number or letters");
            Garage.InfluateTheWheelsOfTheVehicle(licenNumberOfVehicleToInflateTheWheels);
            Console.WriteLine("the air wheels are influante to maximum. drive safe :)");
        }

        private static void refuelingRegularVehicle()
        {
            string licenNumberOfVehicleToRefuel;
            FuelEngine.eFuelType fuelType;
            float fuelToAdd;

            Console.WriteLine("please enter a licen number of the vehicle that you want to refuel");
            licenNumberOfVehicleToRefuel = Console.ReadLine();
            if (sr_Garage.VehicleIsAlreadyAtTheGarage(licenNumberOfVehicleToRefuel))
            {
                float fillAmount = sr_Garage.Clients[licenNumberOfVehicleToRefuel].ClientVehicle.Engine.GetEmptySpaceInEngineInLiter();
                try
                {
                    sr_Garage.CheckIfVehicleCanBeFuled(licenNumberOfVehicleToRefuel);
                    try
                    {
                        fuelType = (FuelEngine.eFuelType)getInputInRange(
@"Please choose fuel type:
1. Solar
2. Octan95
3. Octan96
4. Octan98
",
                    1,
                    Enum.GetNames(typeof(FuelEngine.eFuelType)).Length);
                        fuelToAdd = getInputInRange(string.Format("Please fill in amount of liters to fuel the engine between {0} to {1}: ", 0, fillAmount),
                                    0,
                                    fillAmount);
                        sr_Garage.RefuelEngine(licenNumberOfVehicleToRefuel, fuelType, fuelToAdd);
                        Console.WriteLine("Engine has been fueled");
                    }
                    catch (ValueOutOfRangeException i_ValueOutOfRangeException)
                    {
                        Console.WriteLine(i_ValueOutOfRangeException.Message);
                    }
                }
                catch (ArgumentException i_ArgumentException)
                {
                    Console.WriteLine(i_ArgumentException.Message);
                }
            }
            else
            {
                Console.WriteLine("Vehicle doesnt exist");
            }
        }

        private static void rechargeElectricVehicle()
        {
            string licenNumberOfVehicleToReCharge;
            float fillAmount;

            Console.WriteLine("please enter a licen number of the vehicle that you want to recharge");
            licenNumberOfVehicleToReCharge = Console.ReadLine();
            if (sr_Garage.VehicleIsAlreadyAtTheGarage(licenNumberOfVehicleToReCharge))
            {
                 fillAmount = sr_Garage.Clients[licenNumberOfVehicleToReCharge].ClientVehicle.Engine.GetEmptySpaceInEngineInMinutes();
                try
                {
                    sr_Garage.CheckIfVehicleCanBeCharged(licenNumberOfVehicleToReCharge);
                    try
                    {
                        sr_Garage.ChargeEngine(licenNumberOfVehicleToReCharge, getInputInRange(string.Format("Please fill in number of minutes to recharge the engine between {0} to {1}: ", 0, fillAmount), 0, fillAmount));
                        Console.WriteLine("Engine has been charged");
                    }

                    catch (ValueOutOfRangeException i_ValueOutOfRangeException)
                    {
                        Console.WriteLine(i_ValueOutOfRangeException.Message);
                    }
                }

                catch (ArgumentException i_ArgumentException)
                {
                    Console.WriteLine(i_ArgumentException.Message);
                }
            }
            else
            {
                Console.WriteLine("Vehicle doesnt exist");
            }
        }

        private static void displayVehicleDetailsByLicenseNumbers()
        {
            string licenOfVehicleToDisplayHisFullDetails;

            Console.WriteLine("please enter the licen number of the vehcile that you want to get the full details: ");
            licenOfVehicleToDisplayHisFullDetails = Console.ReadLine();
            ValidationCheck.ValidationCheckForStringNumbers(licenOfVehicleToDisplayHisFullDetails, k_MinLicenseNumberLength, "licen need to be letters or numbers only.");
            Garage.DisplayFullDetailsOfVehicleByGivenLicenNumber(licenOfVehicleToDisplayHisFullDetails);
        }
    }
}


