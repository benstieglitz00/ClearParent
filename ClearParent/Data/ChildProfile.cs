using System.ComponentModel.DataAnnotations;
namespace ClearParent.Data;
public sealed class ChildProfile
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string UserId { get; set; } = "";
    [Required, StringLength(100)] public string Name { get; set; } = "";
    [DataType(DataType.Date)] public DateOnly? Birthday { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
