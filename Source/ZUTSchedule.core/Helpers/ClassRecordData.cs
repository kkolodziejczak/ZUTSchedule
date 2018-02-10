using System;
using System.Collections.Generic;
using System.Text;

namespace ZUTSchedule.core
{

    public static class ClassRecordData
    {
        public static short Date { get; } = 0;
        public static short StartTime { get; } = 1;
        public static short EndTime { get; } = 2;
        public static short Place { get; } = 3;
        public static short Course { get; } = 4;
        public static short CourseType { get; } = 5;
        public static short Teacher { get; } = 6;
    }
}
