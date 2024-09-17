using System.ComponentModel;
namespace UmojaCampus.Shared.Enums
{
    public enum AssessmentType
    {
        [Description("Quiz")]
        Quiz = 1,

        [Description("Test")]
        Test = 2,

        [Description ("Group Activity")]
        GroupActivity = 3,

        [Description("Formative Assessment")]
        FormativeAssignment = 4,

        [Description("Summative Assessment")]
        SummativeAssignment = 5,

    }
}
