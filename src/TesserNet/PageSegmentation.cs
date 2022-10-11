namespace TesserNet
{
    /// <summary>
    /// Indicates how page segmentation should be treated.
    /// </summary>
    public enum PageSegmentation
    {
        /// <summary>
        /// Orientation and script detection (OSD) only.
        /// </summary>
        Osd = 0,

        /// <summary>
        /// Automatic page segmentation with OSD.
        /// </summary>
        SegmentationOsd = 1,

        /// <summary>
        /// Automatic page segmentation, but no OSD, or OCR.
        /// </summary>
        Segmentation = 2,

        /// <summary>
        /// Fully automatic page segmentation, but no OSD. (Default).
        /// </summary>
        SegmentationOcr = 3,

        /// <summary>
        /// Assume a single column of text of variable sizes.
        /// </summary>
        Column = 4,

        /// <summary>
        /// Assume a single uniform block of vertically aligned text.
        /// </summary>
        VerticalBlock = 5,

        /// <summary>
        /// Assume a single uniform block of text.
        /// </summary>
        Block = 6,

        /// <summary>
        /// Treat the image as a single text line.
        /// </summary>
        Line = 7,

        /// <summary>
        /// Treat the image as a single word.
        /// </summary>
        Word = 8,

        /// <summary>
        /// Treat the image as a single word in a circle.
        /// </summary>
        WordCircle = 9,

        /// <summary>
        /// Treat the image as a single character.
        /// </summary>
        Character = 10,

        /// <summary>
        /// Sparse text. Find as much text as possible in no particular order.
        /// </summary>
        Sparse = 11,

        /// <summary>
        /// Sparse text with OSD.
        /// </summary>
        SparseOsd = 12,

        /// <summary>
        /// Raw line. Treat the image as a single text line, bypassing hacks that are Tesseract-specific.
        /// </summary>
        Raw = 13,
    }
}
