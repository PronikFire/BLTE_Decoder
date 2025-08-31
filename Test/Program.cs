using BLTE_Decoder;
using System;
using System.Text;

namespace Test;

internal class Program
{
    static void Main()
    {
        Block[] blocks =
        [
            new(Encoding.UTF8.GetBytes("Test1"))
            {
                encodingModeChar = 'N',
                logicalSize = (uint)"Test1".Length,
                Hash = Encoding.UTF8.GetBytes("1111111111111111"),
                UncompressedHash = Encoding.UTF8.GetBytes("2222222222222222"),
            },
            new(Encoding.UTF8.GetBytes("Test2"))
            {
                encodingModeChar = 'Z',
                logicalSize = (uint)"Test2".Length,
                Hash = Encoding.UTF8.GetBytes("3333333333333333"),
                UncompressedHash = Encoding.UTF8.GetBytes("4444444444444444"),
            }
        ];
        Console.WriteLine(string.Join<Block>('\n', BLTE.Decode(BLTE.Encode(blocks, 0x10), out _)));
    }
}
