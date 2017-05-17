namespace MockTypeBuilder.Constraints
{
    public class NumberConstraints
    {
        public NumberType Type { get; set; }
        public long MinValue { get; set; }
        public long MaxValue { get; set; }
        public ushort NoOfDecimalPlaces { get; set; }
    }
}
