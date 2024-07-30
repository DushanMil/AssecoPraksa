using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AssecoPraksa.Models
{
    [DisplayName("transaction-paged-list")]
    public class TransactionPagedList<T>
    {
        [JsonPropertyName("total-count")]
        public int TotalCount { get; set; }
        [JsonPropertyName("page-size")]
        public int PageSize { get; set; }
        [JsonPropertyName("page")]
        public int Page { get; set; }
        [JsonPropertyName("total-pages")]
        public int TotalPages { get; set; }
        [JsonPropertyName("sort-order")]
        public SortOrder SortOrder { get; set; }
        [JsonPropertyName("sort-by")]
        public string? SortBy { get; set; } = null!;
        public List<T> Items { get; set; }

    }
}
