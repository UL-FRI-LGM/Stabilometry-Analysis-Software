using UnityEngine;

namespace StabilometryAnalysis
{
    using static Parameter;

    public static class Units
    {
        public static string
            NORMAL_UNIT = "cm",
            VELOCITY_UNIT = "cm/s",
            AREA_UNIT = "cm²";

        public static string GetUnit(Parameter parameter)
        {
            switch (parameter)
            {
                case (SWAY_PATH_TOTAL):
                    return NORMAL_UNIT;
                case (SWAY_PATH_AP):
                    return NORMAL_UNIT;
                case (SWAY_PATH_ML):
                    return NORMAL_UNIT;
                case (MEAN_SWAY_VELOCITY_TOTAL):
                    return VELOCITY_UNIT;
                case (MEAN_SWAY_VELOCITY_AP):
                    return VELOCITY_UNIT;
                case (MEAN_SWAY_VELOCITY_ML):
                    return VELOCITY_UNIT;
                case (SWAY_AVERAGE_AMPLITUDE_AP):
                    return NORMAL_UNIT;
                case (SWAY_AVERAGE_AMPLITUDE_ML):
                    return NORMAL_UNIT;
                case (SWAY_MAXIMAL_AMPLITUDE_AP):
                    return NORMAL_UNIT;
                case (SWAY_MAXIMAL_AMPLITUDE_ML):
                    return NORMAL_UNIT;
                case (MEAN_DISTANCE):
                    return NORMAL_UNIT;
                case (ELLIPSE_AREA):
                    return AREA_UNIT;
            }

            //else
            Debug.LogError($"Parameter {parameter} was not defined.");
            return "";
        }
    }
}