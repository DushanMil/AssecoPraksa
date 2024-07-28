using System.ComponentModel;

namespace AssecoPraksa.Models
{
    [DisplayName("transaction-paged-list")]
    public class TransactionPagedList<T>
    {
        [DisplayName("total-count")]
        public int TotalCount { get; set; }
        [DisplayName("page-size")]
        public int PageSize { get; set; }
        [DisplayName("page")]
        public int Page { get; set; }
        [DisplayName("total-pages")]
        public int TotalPages { get; set; }
        [DisplayName("sort-order")]
        public SortOrder SortOrder { get; set; }
        [DisplayName("sort-by")]
        public string? SortBy { get; set; } = null!;
        public List<T> Items { get; set; }

    }
}
