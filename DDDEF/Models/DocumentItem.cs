using DDDEF.Controllers;

namespace DDDEF.Models
{
    public class DocumentItem : ISoftDelete, ICreationTime
    {
        private DocumentItem() { }
        public DocumentItem(string displayName, string name, string path, string? description, int docStoreId)
        {
            DisplayName = displayName;
            Name = name;
            Path = path;
            Description = description;
            DocStoreId = docStoreId;
            CreationTime = DateTime.Now;
            IsDeleted = false;
        }

        public int Id { get; set; }
        public string DisplayName { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Path { get; set; } = null!;
        public string? Description { get; set; }
        public int DocStoreId { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
