namespace DDDApplication.Contract.Documents
{
    public class DocumentStoreModel
    {
        public string? Description { get; set; }
        public int Creator { get; set; }
        public int TypeId { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
