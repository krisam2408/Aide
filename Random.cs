namespace Aide;

public static class Random
{
    private static readonly System.Random m_random = new();

    public static DateTime RandomDate(DateTime start, DateTime end)
    {
        int year = m_random.Next(start.Year, end.Year + 1);

        int handleMonth()
        {
            if (year == end.Year)
                return m_random.Next(1, end.Month - 1);
            return m_random.Next(0, 12) + 1;
        }
        int month = handleMonth();

        int handleDay()
        {
            if (Bool.And(year == end.Year, month == end.Month))
                return m_random.Next(1, end.Day - 1);
            return m_random.Next(0, DateTime.DaysInMonth(year, month)) + 1;
        }
        int day = handleDay();

        int handleHour()
        {
            if (Bool.And(year == end.Year, month == end.Month, day == end.Day))
                return m_random.Next(0, end.Hour);
            return m_random.Next(0, 24);
        }
        int hour = handleHour();

        int handleMinute()
        {
            bool[] checks =
            [
                year == end.Year,
                month == end.Month,
                day == end.Day,
                hour == end.Hour
            ];

            if (Bool.AndCheck(checks))
                return m_random.Next(1, end.Minute);
            return m_random.Next(0, 60);
        }
        int minute = handleMinute();

        int handleSecond()
        {
            bool[] checks =
            [
                year == end.Year,
                month == end.Month,
                day == end.Day,
                hour == end.Hour,
                minute == end.Minute
            ];

            if (Bool.AndCheck(checks))
                return m_random.Next(1, end.Second);

            return m_random.Next(0, 60);
        }
        int second = handleSecond();

        DateTime result = new(year, month, day, hour, minute, second);
        return result;
    }

    public static TimeSpan RandomTimeSpan(TimeSpan min, TimeSpan max)
    {
        int minS = (int)min.TotalSeconds;
        int maxS = (int)max.TotalSeconds;

        int rndS = m_random.Next(minS, maxS + 1);
        TimeSpan result = new(0, 0, rndS);
        return result;
    }

    public static int RandomInt(int min, int max)
    {
        return m_random.Next(min, max + 1);
    }

    public static bool RandomFloat(float trueRatio)
    {
        trueRatio = trueRatio.ClampToOne();
        float result = m_random.Next(101) / 100f;

        return result <= trueRatio;
    }

    public static T RandomItem<T>(this IEnumerable<T> list)
    {
        T[] array = list.ToArray();
        int index = m_random.Next(array.Length);
        return array[index];
    }
}
