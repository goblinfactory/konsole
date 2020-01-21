namespace Konsole.Tests.TestClasses
{
    public partial class FormTests
    {
        internal class MixedNumClass
        {
            public double DoubleField { get; set; }
            public double? DoubleNull { get; set; }
            public int IntMinValue { get; set; }
            public int? IntNull { get; set; }
            public int? IntField { get; set; }
            public decimal DecimalMinValue { get; set; }
            public decimal? DecimalMaxValue { get; set; }
            public decimal? DecimalField { get; set; }
            public float FloatField { get; set; }
            public float FloatMaxValue { get; set; }
            public float? FloatMinValue { get; set; }
            public float? FloatNull { get; set; }
            public float? FloatEpsilon { get; set; }
        }
    }
}