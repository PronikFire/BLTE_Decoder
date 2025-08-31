using System;
using System.Buffers.Binary;
using System.Linq;
using System.Text;

namespace BLTE_Decoder;

public static class BLTE
{
    public static Block[] Decode(byte[] data, out byte tableFormat)
    {
        ArgumentNullException.ThrowIfNull(data);

        if (data.Length < 12)
            throw new ArgumentException("Data is too short to be a valid BLTE file.");

        if (Encoding.UTF8.GetString(data[..4]) != "BLTE")
            throw new ArgumentException("Invalid BLTE data");

        uint headerSize = BinaryPrimitives.ReadUInt32BigEndian(data.AsSpan(4, 4));
        tableFormat = data[8];

        if (tableFormat != 0xF && tableFormat != 0x10)
            throw new ArgumentException("Unsupported BLTE table format or data is corrupted.");

        uint numBlocks = BinaryPrimitives.ReadUInt16BigEndian(data.AsSpan(9, 3));
        numBlocks = numBlocks << 8 | data[11];

        if (numBlocks == 0)
            throw new ArgumentException("No blocks to decode or data is corrupted.");

        Block[] blocks = new Block[numBlocks];

        int blockSize = tableFormat == 0x0F ? 24 : 40;

        int curDataPos = (int)headerSize;
        for (int i = 0; i < numBlocks; i++)
        {
            int curHeaderPos = 12 + i * blockSize;

            uint rawSize = BinaryPrimitives.ReadUInt32BigEndian(data.AsSpan(curHeaderPos, 4));

            Block block = new()
            {
                encodingModeChar = (char)data[curDataPos],
                rawData = data[(curDataPos + 1)..(curDataPos + (int)rawSize)],
                logicalSize = BinaryPrimitives.ReadUInt32BigEndian(data.AsSpan(curHeaderPos + 4, 4)),
                Hash = data[(curHeaderPos + 8)..(curHeaderPos + 24)]
            };
            if (tableFormat == 0x10)
                block.UncompressedHash = data[(curHeaderPos + 24)..(curHeaderPos + 40)];

            curDataPos += (int)rawSize;
            blocks[i] = block;
        }
        return blocks;
    }

    public static byte[] Encode(Block[] blocks, byte tabletFormat)
    {
        ArgumentNullException.ThrowIfNull(blocks);

        if (blocks.Length == 0)
            throw new ArgumentException("No blocks to encode.");

        if (tabletFormat != 0x0F && tabletFormat != 0x10)
            throw new ArgumentException("Unsupported BLTE table format.");

        uint headerSize = 12 + (uint)blocks.Length * (uint)(tabletFormat == 0x0F ? 24 : 40);
        uint totalSize = headerSize + (uint)blocks.Sum(b => (uint)b.rawData.Length + 1);

        byte[] result = new byte[totalSize];

        Array.Copy(Encoding.UTF8.GetBytes("BLTE"), 0, result, 0, 4);
        BinaryPrimitives.WriteUInt32BigEndian(result.AsSpan(4, 4), headerSize);
        result[8] = tabletFormat;
        BinaryPrimitives.WriteUInt16BigEndian(result.AsSpan(9, 3), (ushort)(blocks.Length >> 8));
        result[11] = (byte)((uint)blocks.Length & 0xFF);

        int blockSize = tabletFormat == 0x0F ? 24 : 40;

        int cuDataPos = (int)headerSize;
        for (int i = 0; i < blocks.Length; i++)
        {
            Block block = blocks[i];

            int curHeaderPos = 12 + i * blockSize;
            BinaryPrimitives.WriteUInt32BigEndian(result.AsSpan(curHeaderPos, 4), (uint)block.rawData.Length + 1);
            BinaryPrimitives.WriteUInt32BigEndian(result.AsSpan(curHeaderPos + 4, 4), block.logicalSize);
            Array.Copy(block.Hash, 0, result, curHeaderPos + 8, 16);
            if (tabletFormat == 0x10)
                Array.Copy(block.UncompressedHash, 0, result, curHeaderPos + 24, 16);

            result[cuDataPos] = (byte)block.encodingModeChar;
            Array.Copy(block.rawData, 0, result, cuDataPos + 1, block.rawData.Length);
            cuDataPos += block.rawData.Length + 1;
        }
        return result;
    }
}