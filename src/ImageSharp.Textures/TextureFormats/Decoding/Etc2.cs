// Copyright (c) Six Labors.
// Licensed under the Apache License, Version 2.0.

using System;

namespace SixLabors.ImageSharp.Textures.TextureFormats.Decoding
{
    /// <summary>
    /// Texture compressed with ETC2.
    /// </summary>
    internal struct Etc2 : IBlock<Etc2>
    {
        /// <inheritdoc/>
        public int BitsPerPixel => 24;

        /// <inheritdoc/>
        public byte PixelDepthBytes => 3;

        /// <inheritdoc/>
        public byte DivSize => 1;

        /// <inheritdoc/>
        public byte CompressedBytesPerBlock => 8;

        /// <inheritdoc/>
        public bool Compressed => true;

        /// <inheritdoc/>
        public Image GetImage(byte[] blockData, int width, int height)
        {
            byte[] decompressedData = this.Decompress(blockData, width, height);
            return Image.LoadPixelData<ImageSharp.PixelFormats.Rgb24>(decompressedData, width, height);
        }

        /// <inheritdoc/>
        public byte[] Decompress(byte[] blockData, int width, int height)
        {
            int extraX = 4 - (width % 4);
            int extraY = 4 - (height % 4);
            byte[] decompressedData = new byte[(width + extraX) * (height + extraY) * 3];
            byte[] decodedPixels = new byte[16 * 3];
            Span<byte> decodedPixelSpan = decodedPixels.AsSpan();
            var blockDataIdx = 0;

            for (int y = 0; y < height; y += 4)
            {
                for (int x = 0; x < width; x += 4)
                {
                    EtcDecoder.DecodeEtc2Block(blockData.AsSpan(blockDataIdx, 8), decodedPixelSpan);

                    var decodedPixelSpanIdx = 0;
                    for (int b = 0; b < 4; b++)
                    {
                        for (int a = 0; a < 4; a++)
                        {
                            var imageX = x + b;
                            var imageY = y + a;
                            var offset = (imageY * width * 3) + (imageX * 3);
                            decompressedData[offset] = decodedPixelSpan[decodedPixelSpanIdx++];
                            decompressedData[offset + 1] = decodedPixelSpan[decodedPixelSpanIdx++];
                            decompressedData[offset + 2] = decodedPixelSpan[decodedPixelSpanIdx++];
                        }
                    }

                    blockDataIdx += 8;
                }
            }

            return decompressedData;
        }
    }
}
