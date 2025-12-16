using Aide.Color;

namespace Aide.Charts.DataTransfer;

public sealed class VectorDataset : BaseDataset
{
    public Vector[] Values { get; set; } = [];

    public VectorDataset(string[] labels, IColor[] colors) : base(labels, colors) { }

    public VectorDataset(string[] labels) : base(labels) { }

    internal override void Validate()
    {
        if (Values.Length == 0)
            throw new ArgumentNullException();

        if (Values.Length != Count)
            throw new ArgumentOutOfRangeException();
    }
}
