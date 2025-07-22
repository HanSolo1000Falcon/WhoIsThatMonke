using System.Collections.Generic;

namespace WhoIsThatMonke.Classes
{
    public class OffsetCalculatorCoolKidzOnly
    {
        private List<bool> boolsForDaSools = new List<bool>();

        public void AddBool(bool value) => boolsForDaSools.Add(value);
        public void ClearBoolsForDaSools() => boolsForDaSools.Clear();

        public float CalculateOffsetCoolKidz()
        {
            int offset = 2;
            foreach (bool value in boolsForDaSools)
            {
                if (value)
                    offset++;
            }

            return offset;
        }
    }
}