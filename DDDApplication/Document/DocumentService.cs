using DDDApplication.Contract.Documents;
using DDDEF;
using DDDEF.Models;

namespace DDDApplication.Document
{
    public class DocumentService : IDocumentService
    {
        readonly StorageDbContext _dbContext;
        public DocumentService(StorageDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public int? SaveDocument(List<DocumentItemDto>? fileList, int userId, int typeId, int? docStoreId)
        {
            fileList = fileList ?? new List<DocumentItemDto>();

            if (fileList.Count == 0)
                return null;
            if (docStoreId == null)
            {
                var store = new DocumentStore(userId, typeId);
                _dbContext.DocumentStores.Add(store);
                _dbContext.SaveChanges();
                docStoreId = store.Id;
            }

            var endata = _dbContext.DocumentItems.Where(x => x.DocStoreId == docStoreId).ToList();

            endata.ForEach(x =>
            {
                if (!fileList.Any(y => y.Id == x.Id))
                {
                    x.IsDeleted = true;
                }
            });
            _dbContext.DocumentItems.AddRange(fileList.Where(x => x.Id == 0).Select(x => new DocumentItem
            (x.DisplayName, x.FileName, x.Path, x.Description, docStoreId)));
            _dbContext.SaveChanges();
            return docStoreId;
        }
        public List<DocumentItemDto> GetDocumentItems(List<DocumentItem> model)
        {
            return model.Select(x => new DocumentItemDto(x)).ToList();
        }
        public int NewStore(DocumentStoreModel model)
        {
            var obj = new DocumentStore(model.Creator, model.TypeId, model.Description);
            _dbContext.DocumentStores.Add(obj);
            return _dbContext.SaveChanges() > 0 ? obj.Id : -1;
        }
    }
}
