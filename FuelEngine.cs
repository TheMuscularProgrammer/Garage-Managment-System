using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageLogic
{
    public class FuelEngine : Engine
    {
        private readonly eFuelType r_FualType;

        public enum eFuelType
        {
            Soler = 1,
            Octan95,
            Octan96,
            Octan98
        }

        public FuelEngine(float i_MaxCapacityOfEnergy, eFuelType i_FualType) : base(i_MaxCapacityOfEnergy)
        {
            r_FualType = i_FualType;
        }

        public eFuelType FualType
        {
            get { return r_FualType; }
        }

        public override string ToString()
        {
            StringBuilder FuelEngineDetails = new StringBuilder();

            FuelEngineDetails.Append(string.Format($"Fuel engine details:{Environment.NewLine}"));
            FuelEngineDetails.Append(base.ToString());
            FuelEngineDetails.Append(string.Format($"Fuel type is: {r_FualType}, current fuel amount is {m_CurrentAmountOfEnergy} liters."));

            return FuelEngineDetails.ToString();
        }

        public void Refueling(float i_FuelLiterToAdd, eFuelType i_FuelType)
        {
            float updatedFuelAmount = i_FuelLiterToAdd + m_CurrentAmountOfEnergy;

            if (r_FualType == i_FuelType)
            {
                if (updatedFuelAmount <= r_MaxCapacityOfEnergy)
                {
                    m_CurrentAmountOfEnergy = updatedFuelAmount;
                }
                else
                {
                    throw new ValueOutOfRangeException(k_MinCapacityOfEnergy, (r_MaxCapacityOfEnergy - m_CurrentAmountOfEnergy), "Engine capacity");
                }
            }
            else
            {
                throw new ArgumentException("Invalid fuel type");
            }
        }
    }
}
