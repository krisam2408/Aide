using Aide.Color;

namespace Aide.Charts.DataTransfer;

public sealed class MultipleDataset : BaseDataset
{
    public List<double[]> Values { get; set; } = [];

    public MultipleDataset(string[] labels, IColor[] colors) : base(labels, colors) { }
    
    public MultipleDataset(string[] labels) : base(labels) { }

    public void AddLineData(double[] data) => Values.Add(data);

    internal override void Validate()
    {
        if (Values.Count == 0)
            throw new ArgumentNullException();

        foreach (double[] line in Values)
            if (line.Length != Count)
                throw new ArgumentOutOfRangeException();
    }
}
