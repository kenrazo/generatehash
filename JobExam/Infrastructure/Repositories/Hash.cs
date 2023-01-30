using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Repositories
{
    [Table("Hash")]
    public class Hash
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string Sha1 { get; set; }
    }
}
