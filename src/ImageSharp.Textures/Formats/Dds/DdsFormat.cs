// Copyright (c) Six Labors.
// Licensed under the Apache License, Version 2.0.

using System.Collections.Generic;

namespace SixLabors.ImageSharp.Textures.Formats.Dds
{
    /// <summary>
    /// Registers the image encoders, decoders and mime type detectors for the png format.
    /// </summary>
    public sealed class DdsFormat : ITextureFormat
    {
        private DdsFormat()
        {
        }

        /// <summary>
        /// Gets the current instance.
        /// </summary>
        public static DdsFormat Instance { get; } = new DdsFormat();

        /// <inheritdoc/>
        public string Name => "DDS";

        /// <inheritdoc/>
        public string DefaultMimeType => "image/vnd.ms-dds";

        /// <inheritdoc/>
        public IEnumerable<string> MimeTypes => DdsConstants.MimeTypes;

        /// <inheritdoc/>
        public IEnumerable<string> FileExtensions => DdsConstants.FileExtensions;
    }
}
