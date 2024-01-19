using DDDEF.Models;
using DDDUtility;

namespace DDDApplication.Contract.Documents
{
    public class DocumentItemDto : FileObjectInfoWithUrl
    {
        public int Id { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsDeleted { get; set; }
        public int DocStoreId { get; set; }
        public string? Description { get; set; }
        public DocumentItemDto() { }
        public DocumentItemDto(DocumentItem item)
        {
            Id = item.Id;
            FileName = item.Name;
            DisplayName = item.DisplayName;
            Path = item.Path;
            CreationTime = item.CreationTime;
            IsDeleted = item.IsDeleted;
            Description = item.Description;
            DocStoreId = item.DocStoreId;
        }
    }
}
