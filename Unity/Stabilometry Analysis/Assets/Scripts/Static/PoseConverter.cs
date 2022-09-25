using static StabilometryAnalysis.Pose;
using UnityEngine;

namespace StabilometryAnalysis
{
    public static class PoseConverter
    {
        #region Variables
        private const string
            BothLegsJoinedParallelString = "Both Legs Joined Parallel",
            BothLegs30AngleString = "Both Legs 30 Angle",
            BothLegsParallelApartString = "Both Legs Parallel Apart",
            TandemLeftFrontString = "Tandem Left Front",
            TandemRightFrontString = "Tandem Right Front",
            LeftLegString = "Left Leg",
            RightLegString = "Right Leg";
        #endregion

        public static string PoseToString(Pose pose)
        {
            switch (pose)
            {
                case (BOTH_LEGS_JOINED_PARALLEL):
                    return BothLegsJoinedParallelString;
                case (BOTH_LEGS_30_ANGLE):
                    return BothLegs30AngleString;
                case (BOTH_LEGS_PARALLEL_APART):
                    return BothLegsParallelApartString;
                case (TANDEM_LEFT_FRONT):
                    return TandemLeftFrontString;
                case (TANDEM_RIGHT_FRONT):
                    return TandemRightFrontString;
                case (LEFT_LEG):
                    return LeftLegString;
                case (RIGHT_LEG):
                    return RightLegString;
                default:
                    Debug.LogError($"Pose {pose} is not defined.");
                    return "Error";
            }
        }

        public static Pose StringToPose(string poseString)
        {
            switch (poseString)
            {
                case (BothLegsJoinedParallelString):
                    return BOTH_LEGS_JOINED_PARALLEL;
                case (BothLegs30AngleString):
                    return BOTH_LEGS_30_ANGLE;
                case (BothLegsParallelApartString):
                    return BOTH_LEGS_PARALLEL_APART;
                case (TandemLeftFrontString):
                    return TANDEM_LEFT_FRONT;
                case (TandemRightFrontString):
                    return TANDEM_RIGHT_FRONT;
                case (LeftLegString):
                    return LEFT_LEG;
                case (RightLegString):
                    return RIGHT_LEG;
                default:
                    Debug.LogError($"{poseString} is not defined.");
                    return BOTH_LEGS_JOINED_PARALLEL;
            }
        }
    }
}