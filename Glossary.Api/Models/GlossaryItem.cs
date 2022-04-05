namespace Glossary.Api.Models
{
    public class GlossaryItem
    {
        public int Id { get; set; }

        public string Term { get; set; } = null!;

        public string Definition { get; set; } = null!;
    }
}
