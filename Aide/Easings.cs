using MathD = System.Math;

namespace Aide;

public static class Easings
{
    public static double Lerp(double a, double b, double t)
    {
        t = t.Clamp(0, 1);
        return a + (b - a) * t;
    }

    public static double LerpTValue(double a, double b, double l)
    {
        if (a == b)
            return l;
        return (l - a) / (b - a);
    }

    public static double InSine(double t)
    {
        t = t.Clamp(0, 1);
        return 1 - MathD.Cos((t * MathD.PI) * 0.5);
    }

    public static double InSineTValue(double l) => throw new NotImplementedException();

    public static double OutSine(double t)
    {
        t = t.Clamp(0, 1);
        return 1 - MathD.Sin((t * MathD.PI) * 0.5);
    }

    public static double OutSineTValue(double l) => throw new NotImplementedException();

    public static double InOutSine(double t)
    {
        t = t.Clamp(0, 1);
        return -(MathD.Cos(MathD.PI * t) - 1) * 0.5;
    }

    public static double InOutSineTValue(double l) => throw new NotImplementedException();

    public static double InQuad(double t) 
    {
        t = t.Clamp(0, 1);
        return MathD.Pow(t, 2);
    }

    public static double InQuadTValue(double l) => throw new NotImplementedException();

    public static double OutQuad(double t)
    {
        t = t.Clamp(0, 1);
        return 1 - MathD.Pow(1 - t, 2);
    }

    public static double OutQuadTValue(double l) => throw new NotImplementedException();

    public static double InOutQuad(double t)
    {
        t = t.Clamp(0, 1);
        
        if(t < 0.5)
            return 2 * MathD.Pow(t, 2);

        return 1 - MathD.Pow(-2 * t + 2, 2) * 0.5;
    }

    public static double InOutQuadTValue(double l) => throw new NotImplementedException();

    public static double InCubic(double t)
    {
        t = t.Clamp(0, 1);
        return MathD.Pow(t, 3);
    }

    public static double InCubicTValue(double l) => throw new NotImplementedException();

    public static double OutCubic(double t)
    {
        t = t.Clamp(0, 1);
        return 1 - MathD.Pow(1 - t, 3);
    }

    public static double OutCubicTValue(double l) => throw new NotImplementedException();

    public static double InOutCubic(double t)
    {
        t = t.Clamp(0, 1);

        if(t < 0.5)
            return 4 * MathD.Pow(t, 3);

        return 1 - MathD.Pow(-2 * t + 2, 3) * 0.5;
    }

    public static double InOutCubicTValue(double l) => throw new NotImplementedException();

    public static double InQuart(double t)
    {
        t = t.Clamp(0, 1);
        return MathD.Pow(t, 4);
    }

    public static double InQuartTValue(double l) => throw new NotImplementedException();

    public static double OutQuart(double t)
    {
        t = t.Clamp(0, 1);
        return 1 - MathD.Pow(1 - t, 4);
    }

    public static double OutQuartTValue(double l) => throw new NotImplementedException();

    public static double InOutQuart(double t)
    {
        t = t.Clamp(0, 1);

        if(t < 0.5)
            return 8 * MathD.Pow(t, 4);

        return 1 - MathD.Pow(-2 * t + 2, 4) * 0.5;
    }

    public static double InOutQuartTValue(double l) => throw new NotImplementedException();

    public static double InQuint(double t)
    {
        t = t.Clamp(0, 1);
        return MathD.Pow(t, 5);
    }

    public static double InQuintTValue(double l) => throw new NotImplementedException();

    public static double OutQuint(double t)
    {
        t = t.Clamp(0, 1);
        return 1 - MathD.Pow(1 - t, 5);
    }

    public static double OutQuintTValue(double l) => throw new NotImplementedException();

    public static double InOutQuint(double t)
    {
        t = t.Clamp(0, 1);
        
        if(t < 0.5)
            return 16 * MathD.Pow(t, 5);

        return 1 - MathD.Pow(-2 * t + 2, 5) * 0.5;
    }

    public static double InOutQuintTValue(double l) => throw new NotImplementedException();

    public static double InExpo(double t)
    {
        t = t.Clamp(0, 1);

        if (t == 0)
            return 0;

        return MathD.Pow(2, 10 * t - 10);
    }

    public static double InExpoTValue(double l) => throw new NotImplementedException();

    public static double OutExpo(double t)
    {
        t = t.Clamp(0, 1);

        if (t == 1)
            return 1;

        return 1 - MathD.Pow(2, -10 * t);
    }

    public static double OutExpoTValue(double l) => throw new NotImplementedException();

    public static double InOutExpo(double t)
    {
        t = t.Clamp(0, 1);

        if (t == 0)
            return 0;

        if (t < 0.5)
            return MathD.Pow(2, 20 * t - 10) * 0.5;

        if (t < 1)
            return (2 - MathD.Pow(2, -20 * t + 10) * 0.5);

        return 1;
    }

    public static double InOutExpoTValue(double l) => throw new NotImplementedException();

    public static double InCirc(double t)
    {
        t = t.Clamp(0, 1);
        return MathD.Sqrt(1 - MathD.Pow(t, 2));
    }

    public static double InCircTValue(double l) => throw new NotImplementedException();

    public static double OutCirc(double t)
    {
        t = t.Clamp(0, 1);
        return MathD.Sqrt(1 - MathD.Pow(t - 1, 2));
    }

    public static double OutCircTValue(double l) => throw new NotImplementedException();

    public static double InOutCirc(double t)
    {
        t = t.Clamp(0, 1);

        if (t < 0.5)
            return (1 - MathD.Sqrt(1 - MathD.Pow(2 * t, 2))) * 0.5;

        return (MathD.Sqrt(1 - MathD.Pow(-2 * t + 2, 2))) * 0.5;
    }

    public static double InOutCircTValue(double l) => throw new NotImplementedException();

    private const double bc0 = 1.70158;
    private const double bc1 = bc0 + 1;
    private const double bc2 = bc0 + 1.525;

    public static double InBack(double t)
    {
        t = t.Clamp(0, 1);
        return bc1 * MathD.Pow(t, 3) - bc0 * MathD.Pow(t, 2);
    }

    public static double InBackTValue(double l) => throw new NotImplementedException();

    public static double OutBack(double t)
    {
        t = t.Clamp(0, 1);
        return 1 + bc1 * MathD.Pow(t - 1, 3) + bc0 * MathD.Pow(t - 1, 2);
    }

    public static double OutBackTValue(double l) => throw new NotImplementedException();

    public static double InOutBack(double t)
    {
        t = t.Clamp(0, 1);

        if (t < 0.5)
            return (MathD.Pow(2 * t, 2) * ((bc2 + 1) * 2 * t - bc2)) * 0.5;

        return (MathD.Pow(2 * t - 2, 2) * ((bc2 + 1) * (t * 2 - 2) + bc2) + 2) * 0.5;
    }

    public static double InOutBackTValue(double l) => throw new NotImplementedException();

    private const double ec0 = (2 * MathD.PI) * 0.3333;
    private const double ec1 = (2 * MathD.PI) * 0.2222;

    public static double InElastic(double t)
    {
        t = t.Clamp(0, 1);

        if (t == 0)
            return 0;

        if (t < 1)
            return -MathD.Pow(2, 10 * t - 10) * MathD.Sin((t * 10 - 10.75) * ec0);

        return 1;
    }

    public static double InElasticTValue(double l) => throw new NotImplementedException();

    public static double OutElastic(double t)
    {
        t = t.Clamp(0, 1);

        if(t == 0) 
            return 0;

        if (t < 1)
            return MathD.Pow(2, -10 * t) * MathD.Sin((t * 10 - 0.75) * ec0) + 1;

        return 1;
    }

    public static double OutElasticTValue(double l) => throw new NotImplementedException();

    public static double InOutElastic(double t)
    {
        t = t.Clamp(0, 1);

        if (t == 0)
            return 0;

        if (t < 0.5)
            return -(MathD.Pow(2, 20 * t - 10) * MathD.Sin((20 * t - 11.125) * ec1)) * 0.5;

        if(t < 1)
            return (MathD.Pow(2, -20 * t + 10) * MathD.Sin((20 * t - 11.125) * ec1)) * 0.5 + 1;

        return 1;
    }

    public static double InOutElasticTValue(double l) => throw new NotImplementedException();

    private const double cn0 = 7.5625;
    private static readonly double[] cn1 =
    [
        0.363636, // 0 -> 1
        0.545454, // 1 -> 1.5
        0.727272, // 2 -> 2
        0.818181, // 3 -> 2.25
        0.909090, // 4 -> 2.5
        0.954545  // 5 -> 2.625
    ];

    public static double InBounce(double t)
    {
        t = t.Clamp(0, 1);

        return 1 - OutBounce(1 - t);
    }

    public static double InBounceTValue(double l) => throw new NotImplementedException();

    public static double OutBounce(double t)
    {
        t = t.Clamp(0, 1);

        if (t < cn1[0])
            return cn0 * MathD.Pow(t, 2);

        double tt;
        if (t < cn1[2])
        {
            tt = t - cn1[1];
            return cn0 * tt * t + 0.75;
        }

        if (t < cn1[4])
        {
            tt = t - cn1[3];
            return cn0 * tt * t + 0.9375;
        }

        tt = t - cn1[5];
        return cn0 * tt * t + 0.984375;
    }

    public static double OutBounceTValue(double l) => throw new NotImplementedException();

    public static double InOutBounce(double t)
    {
        t = t.Clamp(0, 1);

        if (t < 0.5)
            return (1 - OutBounce(1 - 2 * t)) * 0.5;

        return (1 + OutBounce(2 * t - 1)) * 0.5;
    }

    public static double InOutBounceTValue(double l) => throw new NotImplementedException();
}
