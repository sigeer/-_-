using Utility.Constants;

namespace DDDApplication.Contract
{
    public class FilterModel : PaginationModel
    {
        public string? Keyword { get; set; }
    }
}
