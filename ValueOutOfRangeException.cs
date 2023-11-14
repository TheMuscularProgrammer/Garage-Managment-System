using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageLogic
{
    public class ValueOutOfRangeException : Exception
    {
        private const string k_MsgToPresent = "{0} is out of range. Must insert value between {1} to {2}.";
        private float m_MaxValue;
        private float m_MinValue;

        public ValueOutOfRangeException(float i_MinValue, float i_MaxValue, string i_WrongData) : base(string.Format(k_MsgToPresent, i_WrongData, i_MinValue, i_MaxValue))
        {
            m_MaxValue = i_MaxValue;
            m_MinValue = i_MinValue;
        }

        public ValueOutOfRangeException(float i_MinValue) : base(string.Format("Invalid input! Please enter number bigger than {0}. ", i_MinValue))
        {
            m_MinValue = i_MinValue;
        }
    }
}
