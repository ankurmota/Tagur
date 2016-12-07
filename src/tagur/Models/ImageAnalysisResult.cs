using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tagur.Models
{
    public class ImageAnalysisResult
    {
        public string id { get; set; }
        public string caption { get; set; }
        public List<string> tags { get; set; }
        public ImageAnalysisInfo details { get; set; }
    }

    public class ImageAnalysisInfo
    {
        public Category[] categories { get; set; }
        public Adult adult { get; set; }
        public Tag[] tags { get; set; }
        public Description description { get; set; }
        public string requestId { get; set; }
        public Metadata metadata { get; set; }
        public Color color { get; set; }
        public Imagetype imageType { get; set; }
    }

    public class Adult
    {
        public bool isAdultContent { get; set; }
        public bool isRacyContent { get; set; }
        public float adultScore { get; set; }
        public float racyScore { get; set; }
    }

    public class Description
    {
        public string[] tags { get; set; }
        public Caption[] captions { get; set; }
    }

    public class Caption
    {
        public string text { get; set; }
        public float confidence { get; set; }
    }

    public class Metadata
    {
        public int width { get; set; }
        public int height { get; set; }
        public string format { get; set; }
    }

    public class Color
    {
        public string dominantColorForeground { get; set; }
        public string dominantColorBackground { get; set; }
        public string[] dominantColors { get; set; }
        public string accentColor { get; set; }
        public bool isBWImg { get; set; }
    }

    public class Imagetype
    {
        public int clipArtType { get; set; }
        public int lineDrawingType { get; set; }
    }

    public class Category
    {
        public string name { get; set; }
        public float score { get; set; }
    }

    public class Tag
    {
        public string name { get; set; }
        public float confidence { get; set; }
        public string hint { get; set; }
    }
}
