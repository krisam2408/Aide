namespace Aide.Color;

public struct HexCode
{
    public int Red { get; private set; }
    public int Green { get; private set; }
    public int Blue { get; private set; }
    public string Code { get; private set; }

    public HexCode(string code)
    {
        if (code[0] != '#')
            throw new FormatException("Hexcode out of format");

        string decode = code.Length switch
        {
            4 => $"{code[1]}{code[1]}{code[2]}{code[2]}{code[3]}{code[3]}",
            7 => $"{code[1]}{code[2]}{code[3]}{code[4]}{code[5]}{code[6]}",
            _ => throw new FormatException("Hexcode out of format")
        };

        string[] channelHex = { decode[..2], decode[2..4], decode[4..6] };
        byte[] channel = channelHex
            .Select(h => Convert.ToByte(h, 16))
            .ToArray();

        Code = code;
        Red = channel[0]; 
        Green = channel[1]; 
        Blue = channel[2];
    }
}
