using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageLogic
{
    public class ElectricEngine : Engine
    {
        public ElectricEngine(float i_MaxCapacityOfEnergy) : base(i_MaxCapacityOfEnergy) { }

        public override void GetEnergy(float i_Energy, FuelEngine.eFuelType i_FuelType = 0)
        {
            base.GetEnergy(i_Energy);
        }

        public override string ToString()
        {
            StringBuilder electricEngineDetails = new StringBuilder();

            electricEngineDetails.Append(string.Format("Electric engine details:{0}", Environment.NewLine));
            electricEngineDetails.Append(base.ToString());
            electricEngineDetails.Append(string.Format("{0} hours of battery left.", CurrentStatusOfEnergy));

            return electricEngineDetails.ToString();
        }

        public void RechargeBattery(float i_addBatteryHours)
        {
            float updatedBatteryAmount = i_addBatteryHours + m_CurrentAmountOfEnergy;

            if (updatedBatteryAmount <= r_MaxCapacityOfEnergy)
            {
                m_CurrentAmountOfEnergy = updatedBatteryAmount;
            }
            else
            {
                throw new ValueOutOfRangeException(k_MinCapacityOfEnergy, (r_MaxCapacityOfEnergy - m_CurrentAmountOfEnergy), "Engine capacity");
            }
        }
    }
}
