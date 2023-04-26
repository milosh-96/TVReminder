using System.ComponentModel.DataAnnotations;

namespace TVReminder.Web.Entities
{
    public class Channel : INamedEntity
    {
        public int Id { get; set; }

        [StringLength(50)]
        [Required]
        public string Name { get; set; } = "Unnamed";
    }
}
