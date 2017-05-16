namespace MockTypeBuilder.Constraints
{
    public class StringConstraints : IConstraints
    {
        public CaseType Case { get; set; }
        public bool UseSpecialCharacters { get; set; }
        public int MinCharacters { get; set; }
        public int MaxCharacters { get; set; }
        public bool UseSpaces { get; set; }
    }
}
