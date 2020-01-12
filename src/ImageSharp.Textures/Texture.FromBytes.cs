// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.IO;
using SixLabors.ImageSharp.Textures.Formats;

namespace SixLabors.ImageSharp.Textures
{
    /// <content>
    /// Adds static methods allowing the creation of new image from a byte array.
    /// </content>
    public abstract partial class Texture
    {
        /// <summary>
        /// By reading the header on the provided byte array this calculates the images format.
        /// </summary>
        /// <param name="data">The byte array containing encoded image data to read the header from.</param>
        /// <returns>The format or null if none found.</returns>
        public static ITextureFormat DetectFormat(byte[] data)
        {
            return DetectFormat(Configuration.Default, data);
        }

        /// <summary>
        /// By reading the header on the provided byte array this calculates the images format.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <param name="data">The byte array containing encoded image data to read the header from.</param>
        /// <returns>The mime type or null if none found.</returns>
        public static ITextureFormat DetectFormat(Configuration config, byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                return DetectFormat(config, stream);
            }
        }



        /// <summary>
        /// By reading the header on the provided byte array this calculates the images format.
        /// </summary>
        /// <param name="data">The byte array containing encoded image data to read the header from.</param>
        /// <returns>The format or null if none found.</returns>
        public static ITextureFormat DetectFormat(ReadOnlySpan<byte> data)
        {
            return DetectFormat(Configuration.Default, data);
        }

        /// <summary>
        /// By reading the header on the provided byte array this calculates the images format.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <param name="data">The byte array containing encoded image data to read the header from.</param>
        /// <returns>The mime type or null if none found.</returns>
        public static ITextureFormat DetectFormat(Configuration config, ReadOnlySpan<byte> data)
        {
            int maxHeaderSize = config.MaxHeaderSize;
            if (maxHeaderSize <= 0)
            {
                return null;
            }

            foreach (ITextureFormatDetector detector in config.ImageFormatsManager.FormatDetectors)
            {
                ITextureFormat f = detector.DetectFormat(data);

                if (f != null)
                {
                    return f;
                }
            }

            return default;
        }

        /// <summary>
        /// Load a new instance of <see cref="Texture"/> from the given encoded byte array.
        /// </summary>
        /// <param name="data">The byte array containing image data.</param>
        /// <param name="format">The detected format.</param>
        /// <returns>The <see cref="Texture"/>.</returns>
        public static Texture Load(byte[] data, out ITextureFormat format) =>
            Load(Configuration.Default, data, out format);

        /// <summary>
        /// Load a new instance of <see cref="Texture"/> from the given encoded byte array.
        /// </summary>
        /// <param name="data">The byte array containing encoded image data.</param>
        /// <param name="decoder">The decoder.</param>
        /// <returns>The <see cref="Texture"/>.</returns>
        public static Texture Load(byte[] data, ITextureDecoder decoder) => Load(Configuration.Default, data, decoder);

        /// <summary>
        /// Load a new instance of <see cref="Texture"/> from the given encoded byte array.
        /// </summary>
        /// <param name="config">The config for the decoder.</param>
        /// <param name="data">The byte array containing encoded image data.</param>
        /// <returns>The <see cref="Texture"/>.</returns>
        public static Texture Load(Configuration config, byte[] data) => Load(config, data, out _);

        /// <summary>
        /// Load a new instance of <see cref="Texture"/> from the given encoded byte array.
        /// </summary>
        /// <param name="config">The config for the decoder.</param>
        /// <param name="data">The byte array containing image data.</param>
        /// <param name="decoder">The decoder.</param>
        /// <returns>The <see cref="Texture"/>.</returns>
        public static Texture Load(Configuration config, byte[] data, ITextureDecoder decoder)
        {
            using (var stream = new MemoryStream(data))
            {
                return Load(config, stream, decoder);
            }
        }

        /// <summary>
        /// Load a new instance of <see cref="Texture"/> from the given encoded byte array.
        /// </summary>
        /// <param name="config">The config for the decoder.</param>
        /// <param name="data">The byte array containing image data.</param>
        /// <param name="format">The mime type of the decoded image.</param>
        /// <returns>The <see cref="Texture"/>.</returns>
        public static Texture Load(Configuration config, byte[] data, out ITextureFormat format)
        {
            using (var stream = new MemoryStream(data))
            {
                return Load(config, stream, out format);
            }
        }

        /// <summary>
        /// Load a new instance of <see cref="Texture"/> from the given encoded byte span.
        /// </summary>
        /// <param name="data">The byte span containing image data.</param>
        /// <returns>The <see cref="Texture"/>.</returns>
        public static Texture Load(ReadOnlySpan<byte> data) => Load(Configuration.Default, data);

        /// <summary>
        /// Load a new instance of <see cref="Texture"/> from the given encoded byte span.
        /// </summary>
        /// <param name="data">The byte span containing image data.</param>
        /// <param name="decoder">The decoder.</param>
        /// <returns>The <see cref="Texture"/>.</returns>
        public static Texture Load(ReadOnlySpan<byte> data, ITextureDecoder decoder) =>
            Load(Configuration.Default, data, decoder);

        /// <summary>
        /// Load a new instance of <see cref="Texture"/> from the given encoded byte array.
        /// </summary>
        /// <param name="data">The byte span containing image data.</param>
        /// <param name="format">The detected format.</param>
        /// <returns>The <see cref="Texture"/>.</returns>
        public static Texture Load(ReadOnlySpan<byte> data, out ITextureFormat format) =>
            Load(Configuration.Default, data, out format);

        /// <summary>
        /// Decodes a new instance of <see cref="Texture"/> from the given encoded byte span.
        /// </summary>
        /// <param name="config">The configuration options.</param>
        /// <param name="data">The byte span containing image data.</param>
        /// <returns>The <see cref="Texture"/>.</returns>
        public static Texture Load(Configuration config, ReadOnlySpan<byte> data) => Load(config, data, out _);

        /// <summary>
        /// Load a new instance of <see cref="Texture"/> from the given encoded byte span.
        /// </summary>
        /// <param name="config">The Configuration.</param>
        /// <param name="data">The byte span containing image data.</param>
        /// <param name="decoder">The decoder.</param>
        /// <returns>The <see cref="Texture"/>.</returns>
        public static unsafe Texture Load(
            Configuration config,
            ReadOnlySpan<byte> data,
            ITextureDecoder decoder)
        {
            fixed (byte* ptr = &data.GetPinnableReference())
            {
                using (var stream = new UnmanagedMemoryStream(ptr, data.Length))
                {
                    return Load(config, stream, decoder);
                }
            }
        }

        /// <summary>
        /// Load a new instance of <see cref="Texture"/> from the given encoded byte span.
        /// </summary>
        /// <param name="config">The configuration options.</param>
        /// <param name="data">The byte span containing image data.</param>
        /// <param name="format">The <see cref="ITextureFormat"/> of the decoded image.</param>>
        /// <returns>The <see cref="Texture"/>.</returns>
        public static unsafe Texture Load(
            Configuration config,
            ReadOnlySpan<byte> data,
            out ITextureFormat format)
        {
            fixed (byte* ptr = &data.GetPinnableReference())
            {
                using (var stream = new UnmanagedMemoryStream(ptr, data.Length))
                {
                    return Load(config, stream, out format);
                }
            }
        }
    }
}
