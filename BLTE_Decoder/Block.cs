using System;

namespace BLTE_Decoder;

/// <summary>
/// Represents a BLTE data block, containing encoding information, hashes, and raw data.
/// </summary>
public class Block(byte[] data)
{
    /// <summary>
    /// Gets the encoding mode of the block, determined by <see cref="encodingModeChar"/>.
    /// </summary>
    public EncodingMode EncodingMode
    {
        get
        {
            return Enum.IsDefined(typeof(EncodingMode), (int)encodingModeChar)
                ? (EncodingMode)encodingModeChar
                : EncodingMode.Unknown;
        }
    }

    /// <summary>
    /// Gets or sets the hash of the block (16 bytes). Used for data integrity verification.
    /// </summary>
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

    /// <summary>
    /// Gets or sets the hash of the uncompressed block data (16 bytes). Used for verifying original data integrity.
    /// </summary>
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

    /// <summary>
    /// The character representing the encoding mode of the block (e.g., 'N', 'Z', '4', 'F', 'E').
    /// </summary>
    public char encodingModeChar = 'N';

    /// <summary>
    /// The logical size of the block (size of the original data before encoding).
    /// </summary>
    public uint logicalSize = 0;

    /// <summary>
    /// The raw data of the block (byte array).
    /// </summary>
    public byte[] data = data??throw new ArgumentNullException(nameof(data));

    /// <summary>
    /// Backing field for the <see cref="Hash"/> property.
    /// </summary>
    private byte[] hash = new byte[16];

    /// <summary>
    /// Backing field for the <see cref="UncompressedHash"/> property.
    /// </summary>
    private byte[] uncompressedHash = new byte[16];

    /// <summary>
    /// Returns a string representation of the block, including encoding mode, size, and hashes.
    /// </summary>
    /// <returns>A string with block information.</returns>
    public override string ToString() => $"Block(EncodingMode={EncodingMode}," +
        $" LogicalSize={logicalSize}," +
        $" Hash={Convert.ToHexString(Hash)}," +
        $" UncompressedHash={Convert.ToHexString(UncompressedHash)})";
}