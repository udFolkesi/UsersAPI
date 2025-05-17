

namespace Core.Entities.Abstractions
{
    public class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? RevokedOn { get; set; }
        public string? RevokedBy { get; set; }

        public bool IsActive => RevokedOn == null;
    }
}
