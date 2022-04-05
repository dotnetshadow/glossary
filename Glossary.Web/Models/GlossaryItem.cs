using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Glossary.Web.Models
{
    public class GlossaryItem
    {
        public int Id { get; set; }

        [DisplayName("Term")]
        [Required, StringLength(50)]
        public string? Term { get; set; }

        [DisplayName("Definition")]
        [Required, StringLength(256)]
        public string? Definition { get; set; }
    }
}
