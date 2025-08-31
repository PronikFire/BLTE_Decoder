# BLTE_Decoder

## Overview

BLTE_Decoder is a C# library for decoding and encoding BLTE files. BLTE is a file format used in Blizzard games for efficient storage and streaming of game assets. This project provides tools for both reading and creating BLTE files.

## Features

- **BLTE Decoding**: Extract and read data from BLTE formatted files.
- **BLTE Encoding**: Create BLTE compliant archives from raw data.
- **Pure C# Implementation**: No external dependencies required.

## Getting Started

### Prerequisites

- .NET 6.0 or higher (can be adapted for older versions)

### Installation

Clone the repository:

```bash
git clone https://github.com/PronikFire/BLTE_Decoder.git
```

Add the BLTE_Decoder project to your solution or build it as a library.

### Usage

#### Decoding a BLTE File

```csharp
using BLTE_Decoder;

// Load your BLTE file as a byte array
byte[] blteData = File.ReadAllBytes("path/to/file.blte");

Block[] decoded = BLTE.Decode(blteData, out byte tableFormat);

// Use decoded data as needed
```
> [!IMPORTANT]
> After decoding you get raw data, if it is compressed you will need to decompress it. What kind of data you have you can find out from EncodingMode.

#### Encoding Data to BLTE

```csharp
using BLTE_Decoder;

Block[] blocks = ... // your data

byte tableFormat = 0x0F;
byte[] blteEncoded = BLTE.Encode(blocks, tableFormat);

// Save to file or stream
File.WriteAllBytes("output.blte", blteEncoded);
```

> [!IMPORTANT]
> If you are going to encode the data, you need to set Hash and LogicalSize to Block. If you don't do this, the data will be encoded incorrectly. UncompressedHash only needs to be set if you are going to encode with TableFormat equal to 0x10.

## References

- [BLTE Format Documentation (WoWDev Wiki)](https://wowdev.wiki/BLTE)
