using System.Collections.Generic;

namespace Model.Smart_Currency_Converter
{
    public sealed class ImageAnalysisResultObject
    {
        public string Status { get; set; }
        public List<RecognitionResults> RecognitionResults { get; set; }
    }

    public sealed class RecognitionResults
    {
        public List<Lines> Lines { get; set; }
    }

    public sealed class Lines
    {
        public string Text { get; set; }
    }
}
