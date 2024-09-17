
namespace UmojaCampus.Business.Entities.Base
{
    public class BaseEntity
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string CreatedById { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string EditedById { get; set; }
        public DateTime EditedDateTime { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
