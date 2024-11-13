using System.Text;

namespace Aide;

public static class Byte
{
    public static string TextParse(IEnumerable<byte> chunk)
    {
        byte[] array = chunk.ToArray();
        StringBuilder sb = new();
        int len = array.Length;
        for (int i = 0; i < len; i++)
        {
            int c = i + 1;
            string strByte = array[i]
                .ToString()
                .PadLeft(3, '0');

            sb.Append(strByte);

            if (c.DivisibleBy(16))
            {
                sb.Append('\n');
                continue;
            }

            sb.Append(' ');

            if (c.DivisibleBy(4))
                sb.Append("  ");
        }

        string result = sb.ToString();
        return result;
    }

    public static string TextParseHex(IEnumerable<byte> chunk, bool full = false)
    {
        byte[] array = chunk.ToArray();
        StringBuilder sb = new();
        int len = array.Length;

        string byteToHex(params byte[] b)
        {
            string result = Convert
                .ToHexString(b)
                .PadLeft(2, '0');

            if (!full)
                return result;

            return $"0x{result}";
        }

        for (int i = 0; i < len; i++)
        {
            int c = i + 1;
            string strByte = byteToHex(array[i]);

            sb.Append(strByte);

            if (c.DivisibleBy(16))
            {
                sb.Append('\n');
                continue;
            }

            sb.Append(' ');

            if (c.DivisibleBy(4))
                sb.Append("  ");
        }

        string result = sb.ToString();
        return result;
    }
}
