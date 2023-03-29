namespace Final_Survivors.Environment
{
    public static class EnvironmentState
    {
        private static bool isDay = true;
        private static bool isPause = false;
        private static bool isIntroduction = true;

        public static bool GetIsIntroduction()
        {
            return isIntroduction;
        }

        public static void SetIsIntroduction(bool value)
        {
            isIntroduction = value;
        }

        public static bool GetIsDay()
        {
            return isDay;
        }

        public static void SetIsDay(bool value)
        {
            isDay = value;
        }

        public static bool GetIsPause()
        {
            return isPause;
        }

        public static void SetIsPause(bool value)
        {
            isPause = value;
        }
    }
}
