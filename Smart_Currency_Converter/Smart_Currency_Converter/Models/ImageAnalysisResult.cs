using System.Collections.Generic;

namespace Model.Smart_Currency_Converter
{
    public sealed class ImageAnalysisResult
    {
        public string Status { get; set; }
        public IList<RecognitionResults> RecognitionResults { get; set; }
    }

    public sealed class RecognitionResults
    {
        public IList<Lines> Lines { get; set; }
    }

    public sealed class Lines
    {
        public string Text { get; set; }
    }
}
