using DDDDomain.Shared.EntityProperty;

namespace DDDEF.Models
{
    public class DocumentStore: IAuditCreator, IAuditCreateTime
    {
        private DocumentStore() { }

        public DocumentStore(int creatorId, int typeId, string? description = null)
        {
            Creator = creatorId;
            TypeId = typeId;
            CreationTime = DateTime.Now;
            Description = description;
        }
        public int Id { get; set; }
        public int Creator { get; set; }
        public int TypeId { get; set; }
        public DateTime CreationTime { get; set; }
        public string? Description { get; set; }
    }
}
