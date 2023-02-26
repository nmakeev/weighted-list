using System;

namespace Tests
{
    public static class Helpers
    {
        public static bool FloatEquals(float x, float y, float tolerance)
        {
            var diff = Math.Abs(x - y);
            return diff <= tolerance ||
                   diff <= Math.Max(Math.Abs(x), Math.Abs(y)) * tolerance;
        }
    }
}