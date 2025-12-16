using Aide.Color;

namespace Aide.Charts.DataTransfer;

public sealed class SingleDataset : BaseDataset
{
    public double[] Values { get; set; } = [];

    public SingleDataset(string[] labels, IColor[] colors):base(labels, colors) { }

    public SingleDataset(string[] labels):base(labels) { }

    public void AddData(double[] data) => Values = data; 
    
    internal override void Validate()
    {
        if (Values.Length == 0)
            throw new ArgumentNullException();

        if (Values.Length != Count)
            throw new ArgumentOutOfRangeException();
    }
}
