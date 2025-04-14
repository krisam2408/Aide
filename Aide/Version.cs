using System.Text;

namespace Aide;

public class Version
{
    private readonly int[] m_version;

    public Version(string? version)
    {
        if (string.IsNullOrWhiteSpace(version))
            version = "1.1.1";

        string[] parts = version.Split('.');

        List<int> result = [];

        foreach (string part in parts)
        {
            if (int.TryParse(part, out int num))
                result.Add(num);
        }

        m_version = result.ToArray();
    }

    public string GetNumber()
    {
        int len = m_version.Length;
        StringBuilder builder = new();
        for (int i = 0; i < len; i++)
        {
            if (i == 0)
            {
                builder.Append(m_version[i]);
                continue;
            }

            builder.Append($".{m_version[i]}");
        }

        return builder.ToString();
    }

    public string GetShortVersion()
    {
        return $"v{GetNumber()}";
    }

    public string GetLongVersion()
    {
        return $"Version {GetNumber()}";
    }
}
