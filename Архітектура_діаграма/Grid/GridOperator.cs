using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GridService.Models.Grid
{
    public class GridOperator
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(255)] public string Username { get; set; }

        [MaxLength(128)] public string Password { get; set; }

        [MaxLength(48)] public string AccessLevel { get; set; }
    }
}
