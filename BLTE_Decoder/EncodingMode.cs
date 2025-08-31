namespace BLTE_Decoder;

public enum EncodingMode
{
    Unknown = 0,
    Plain = 'N',
    Zlib = 'Z',
    Lz4hc = '4',
    BLTE = 'F',
    Encrypted = 'E'
}
