namespace TesserNet
{
    /// <summary>
    /// Enum for the OCR setting to be used.
    /// </summary>
    public enum OcrEngineMode
    {
        /// <summary>
        /// Only run the legacy Tesseract OCR.
        /// </summary>
        TesseractOnly = 0,

        /// <summary>
        /// Only run the new LSTM based OCR.
        /// </summary>
        LstmOnly = 1,

        /// <summary>
        /// Combine LSTM and the legacy Tesseract OCR.
        /// </summary>
        Combined = 2,

        /// <summary>
        /// The default setting (picks whatever is available).
        /// </summary>
        Default = 3,
    }
}
