using DDDEF.Models;

namespace DDDDomain.Documents
{
    public class DocumentManager
    {
        public DocumentItem NewDepartment(string displayName, string name, string path, string? description, int docStoreId)
        {
            return new DocumentItem(displayName, name, path, description, docStoreId);
        }
    }
}
