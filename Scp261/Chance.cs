using System;
using System.Linq;

namespace Scp261;

public class WeightedChanceExecutor(params WeightedChanceParam[] parameters)
{
    private readonly Random _random = new();

    private WeightedChanceParam[] Parameters { get; } = parameters;

    private double RatioSum
    {
        get { return Parameters.Sum(p => p.Ratio); }
    }

    public void Execute()
    {
        double numericValue = _random.NextDouble() * RatioSum;
        foreach (WeightedChanceParam parameter in Parameters)
        {
            numericValue -= parameter.Ratio;
            if (!(numericValue <= 0))
                continue;

            parameter.Func();
            return;
        }
    }
}

public class WeightedChanceParam(Action func, double ratio)
{
    public Action Func { get; } = func;

    public double Ratio { get; } = ratio;
}