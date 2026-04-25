using UnityEngine;

namespace MGJ.Utility
{
    public static class MoneyFormattingUtility
    {
        public static string ToEuroString(this float value)
        {
            int wholeEuros = Mathf.FloorToInt(value);
            int cents = Mathf.FloorToInt((value * 100) % 100);

            return string.Format("{0},{1:D2} €", wholeEuros, cents);
        }
    }
}
