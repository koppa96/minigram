using System.Collections.Generic;

namespace Minigram.Application.Abstractions.Dtos
{
    public class PagedListDto<T>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public List<T> Data { get; set; }
    }
}