using DDDEF.Models;

namespace DDDApplication.Contract.Documents
{
    public interface IDocumentService
    {
        List<DocumentItemDto> GetDocumentItems(List<DocumentItem> model);
        int? SaveDocument(List<DocumentItemDto>? fileList, int userId, int typeId, int? docStoreId = null);
        int NewStore(DocumentStoreModel model);
    }
}
