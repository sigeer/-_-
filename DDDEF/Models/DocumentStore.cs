using DDDEF.Controllers;

namespace DDDEF.Models
{
    public class DocumentStore : ICreator, ICreationTime
    {
        private DocumentStore() { }

        public DocumentStore(int creatorId, int typeId, string? description = null)
        {
            CreatorId = creatorId;
            TypeId = typeId;
            CreationTime = DateTime.Now;
            Description = description;
        }
        public int Id { get; set; }
        public int CreatorId { get; set; }
        public int TypeId { get; set; }
        public DateTime CreationTime { get; set; }
        public string? Description { get; set; }
    }
}
