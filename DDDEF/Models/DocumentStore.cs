namespace DDDEF.Models
{
    public class DocumentStore
    {
        private DocumentStore() { }

        public DocumentStore(int? creatorId, int typeId, string? description = null)
        {
            Creator = creatorId;
            TypeId = typeId;
            CreateTime = DateTime.Now;
            Description = description;
        }
        public int Id { get; set; }
        public int? Creator { get; set; }
        public int? TypeId { get; set; }
        public DateTime CreateTime { get; set; }
        public string? Description { get; set; }
    }
}
