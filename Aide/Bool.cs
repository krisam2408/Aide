namespace Aide;

public static class Bool
{
    public static bool AndCheck(this bool[] args) => !args.Contains(false);

    public static bool And(params bool[] args) => !args.Contains(false);

    public static bool ExistsIn<T>(this T refValue, params T[] compare) => compare.Contains(refValue);

    public static bool OrCheck(this bool[] args) => args.Contains(true);

    public static bool Or(params bool[] args) => args.Contains(true);
}
