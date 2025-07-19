using System.ComponentModel.DataAnnotations;

namespace Ioc.Core
{
    public abstract class PublicBaseEntity
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }
        public bool IsDelete { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public Guid? CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
    }


    public static class AuditableEntityExtensions
    {
        public static void SetCreated(this PublicBaseEntity entity, Guid createdBy)
        {
            entity.CreatedBy = createdBy;
            entity.CreatedAt = DateTime.UtcNow;
        }

        public static void SetUpdated(this PublicBaseEntity entity, Guid updatedBy)
        {
            entity.ModifiedBy = updatedBy;
            entity.ModifiedDate = DateTime.UtcNow;
        }
    }
}
