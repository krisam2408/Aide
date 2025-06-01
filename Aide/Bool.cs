namespace Aide;

public static class Bool
{
    public static bool AndCheck(this bool[] args) => !args.Contains(false);

    public static bool And(params bool[] args) => !args.Contains(false);

    public static bool ExistsIn<T>(this T refValue, params T[] compare) => compare.Contains(refValue);

    public static bool OrCheck(this bool[] args) => args.Contains(true);

    public static bool Or(params bool[] args) => args.Contains(true);

    public static bool AnyNull(params object?[] args)
    {
        foreach(object? obj in args) 
            if(obj is null)
                return true;
        return false;
    }

    public static int ToInt(this bool b)
    {
        if (b)
            return 1;
        return 0;
    }

    public static bool ToBool(this int i) => i == 1;

    public static string StringifyAsInt(this bool b)
    {
        if(b)
            return "1";
        return "0";
    }
}
