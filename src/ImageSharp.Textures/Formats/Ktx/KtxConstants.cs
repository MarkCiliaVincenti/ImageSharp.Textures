// Copyright (c) Six Labors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Collections.Generic;

namespace SixLabors.ImageSharp.Textures.Formats.Ktx
{
    internal static class KtxConstants
    {
        /// <summary>
        /// The size of a KTX header in bytes.
        /// </summary>
        public const int KtxHeaderSize = 52;

        /// <summary>
        /// The list of mimetypes that equate to a dds file.
        /// </summary>
        public static readonly IEnumerable<string> MimeTypes = new[] { "image/ktx" };

        /// <summary>
        /// The list of file extensions that equate to a ktx file.
        /// </summary>
        public static readonly IEnumerable<string> FileExtensions = new[] { "ktx" };

        /// <summary>
        /// Gets the magic bytes identifying a ktx texture.
        /// </summary>
        public static ReadOnlySpan<byte> MagicBytes => new byte[]
        {
            0xAB, // «
            0x4B, // K
            0x54, // T
            0x58, // X
            0x20, // " "
            0x31, // 1
            0x31, // 1
            0xBB, // »
            0x0D, // \r
            0x0A, // \r
            0x1A,
            0x0A, // \r
        };
    }
}
