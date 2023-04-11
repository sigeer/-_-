using Utility.Constants;

namespace DDDApplication.Contract
{
    public class FilterModel : PaginationModel
    {
        public string? Keyword { get; set; }
        public int StatusId { get; set; }
        public int TypeId { get; set; }

    }
}
