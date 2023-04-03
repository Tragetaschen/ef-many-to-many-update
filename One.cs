using System.ComponentModel.DataAnnotations;

namespace Repro;

public class One
{
    [Key]
    public int Id { get; set; }
    public List<Two> Twos { get; set; } = null!;
    public List<OneTwo> OneTwos { get; set; } = null!;
}
