using System.ComponentModel.DataAnnotations;

namespace Repro;

public class Two
{
    [Key]
    public int Id { get; set; }
    public List<One> Ones { get; set; } = null!;
    public List<OneTwo> OneTwos { get; set; } = null!;
}
