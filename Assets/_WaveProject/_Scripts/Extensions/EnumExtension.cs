using System;

namespace WaveProject.Extensions
{
    public static class EnumExtension
    {
        public static bool HasFlagExcludingZero(this Enum currentEnum, Enum flag)
        {
            return currentEnum.Equals(Enum.Parse(flag.GetType(), "0")) && currentEnum.HasFlag(flag);
        }
    }
}