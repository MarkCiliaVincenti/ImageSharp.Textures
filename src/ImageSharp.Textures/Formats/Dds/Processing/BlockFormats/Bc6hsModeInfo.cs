// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

using SixLabors.ImageSharp.Textures.Formats.Dds.Processing.PixelFormats;

namespace SixLabors.ImageSharp.Textures.Formats.Dds.Processing
{
    internal struct Bc6hsModeInfo
    {
        public byte UMode;
        public readonly byte UPartitions;
        public readonly bool BTransformed;
        public readonly byte UIndexPrec;
        public readonly LdrColorA[][] RgbaPrec; // [Constants.BC6H_MAX_REGIONS][2];

        public Bc6hsModeInfo(byte uM, byte uP, bool bT, byte uI, LdrColorA[][] prec)
        {
            this.UMode = uM;
            this.UPartitions = uP;
            this.BTransformed = bT;
            this.UIndexPrec = uI;
            this.RgbaPrec = prec;
        }
    }
}