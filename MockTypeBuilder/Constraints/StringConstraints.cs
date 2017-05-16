namespace MockTypeBuilder.Constraints
{
    public class StringConstraints
    {
        public CaseType Case { get; set; }
        public bool UseSpecialCharacters { get; set; }
        public uint MinCharacters { get; set; }
        public uint MaxCharacters { get; set; }
        public bool UseSpaces { get; set; }
    }
}
