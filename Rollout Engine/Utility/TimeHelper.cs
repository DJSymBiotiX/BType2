﻿using System;

namespace Rollout.Utility
{
    public class Time
    {
        public static TimeSpan ms(int ms)
        {
            return new TimeSpan(0, 0, 0, 0, ms);
        }

        public static TimeSpan s(int s)
        {
            return new TimeSpan(0, 0, 0, s);
        }
    }
}
