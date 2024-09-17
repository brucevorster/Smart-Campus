using System.ComponentModel;

namespace UmojaCampus.Shared.Enums
{
    public enum ContentType
    {
        [Description("Pdf")]
        Pdf = 1,

        [Description("Word")]
        Word = 2,

        [Description("Video")]
        Video = 3,

        [Description("PowerPoint")]
        PowerPoint = 4
    }
}