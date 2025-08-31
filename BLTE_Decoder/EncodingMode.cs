namespace BLTE_Decoder;

/// <summary>
/// Specifies the encoding mode for a BLTE data block.
/// </summary>
public enum EncodingMode
{
    /// <summary>
    /// Unknown encoding mode.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// Plain (no compression) encoding mode. Represented by 'N'.
    /// </summary>
    Plain = 'N',

    /// <summary>
    /// Zlib compression encoding mode. Represented by 'Z'.
    /// </summary>
    Zlib = 'Z',

    /// <summary>
    /// LZ4 high compression encoding mode. Represented by '4'.
    /// </summary>
    Lz4hc = '4',

    /// <summary>
    /// BLTE internal encoding mode. Represented by 'F'.
    /// </summary>
    BLTE = 'F',

    /// <summary>
    /// Encrypted encoding mode. Represented by 'E'.
    /// </summary>
    Encrypted = 'E'
}
