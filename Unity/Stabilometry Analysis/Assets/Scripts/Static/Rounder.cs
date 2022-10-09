using System;

namespace StabilometryAnalysis
{
    public static class Rounder
    {
        /// <summary>
        /// Roudns to two decimal numbers. If the number is smaller than 0.1 then it rounds to the last two non 0 numbers.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static float RoundFloat(float number)
        { 
            int roundingNumber = GetRoundingNumber(number);

            if (roundingNumber < 0)
                return 0;

            //else
            return (float)Math.Round(number, roundingNumber, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// Calculates how many times the number has to be multiplied by 10 to get a number that is equal greater than 10.
        /// This is to get at least 2 numbered decimals.
        /// </summary>
        /// <param name="number">Number must be positive.</param>
        /// <returns></returns>
        private static int GetRoundingNumber(float number)
        {
            if (number >= 0.01f)
                return 2;

            //else

            if (number == 0)
                return -1;

            // else

            int result = 0;
            float tempNumber = number;

            while (result < 9 && tempNumber < 10)
            {
                tempNumber *= 10;
                result++;
            }

            if (result == 9)
                return -1;

            //else
            return result;
        }
            
    }
}
