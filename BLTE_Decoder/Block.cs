using System;

namespace BLTE_Decoder;

public class Block
{
    public EncodingMode EncodingMode
    {
        get
        {
            return Enum.IsDefined(typeof(EncodingMode), (int)encodingModeChar)
                ? (EncodingMode)encodingModeChar
                : EncodingMode.Unknown;
        }
    }
    public byte[] Hash
    {
        get => hash;
        set
        {
            if (value.Length != 16)
                throw new ArgumentException("Hash must be 16 bytes long.");
            hash = value;
        }
    }
    public byte[] UncompressedHash
    {
        get => uncompressedHash;
        set
        {
            if (value.Length != 16)
                throw new ArgumentException("Uncompressed hash must be 16 bytes long.");
            uncompressedHash = value;
        }
    }

    public char encodingModeChar = 'N';
    public uint logicalSize;
    public byte[] rawData = [];

    private byte[] hash = [];
    private byte[] uncompressedHash = [];

    public override string ToString() => $"Block(EncodingMode={EncodingMode}," +
        $" LogicalSize={logicalSize}," +
        $" Hash={Convert.ToHexString(Hash)}," +
        $" UncompressedHash={Convert.ToHexString(UncompressedHash)})";
}