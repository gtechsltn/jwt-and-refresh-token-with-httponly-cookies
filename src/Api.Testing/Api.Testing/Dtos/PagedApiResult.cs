using System.Collections.Generic;
using System.Net;

namespace Api.Testing.Dtos
{
    public class PagedApiResult<T> : ApiResult
    {
        public PagedApiResult()
        {
            Success = true;
            Status = HttpStatusCode.OK;
        }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public long TotalRecords { get; set; }

        public long TotalPage => PageSize == 0 ? 1 : ((TotalRecords - 1) / PageSize + 1);

        public bool HasNextPage => PageIndex < TotalPage;

        public bool HasPreviousPage => PageIndex > 1;

        public IReadOnlyCollection<T> Data { get; set; }
    }
}