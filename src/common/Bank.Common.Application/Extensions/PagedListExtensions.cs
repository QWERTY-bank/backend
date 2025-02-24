using X.PagedList;

namespace Bank.Common.Application.Extensions
{
    public static class PagedListExtensions
    {
        public static PagedListWithMetadata<T> AddMetaData<T>(this IPagedList<T> list)
            where T : class
        {
            return new PagedListWithMetadata<T>(list);
        }

        public class PagedListWithMetadata<T>
            where T : class
        {
            public IPagedList<T> Results { get; set; }
            public PagedListMetaData Pagination { get; set; }

            public PagedListWithMetadata(IPagedList<T> list)
            {
                Results = list;
                Pagination = new PagedListMetaData
                {
                    PageNumber = list.PageNumber,
                    PageSize = list.PageSize,
                    PageCount = list.PageCount
                };
            }
        }

        public class PagedListMetaData
        {
            /// <summary>
            /// Текущая страница
            /// </summary>
            public int PageNumber { get; set; }

            /// <summary>
            /// Размер страницы
            /// </summary>
            public int PageSize { get; set; }

            /// <summary>
            /// Количество страниц
            /// </summary>
            public int PageCount { get; set; }
        }
    }
}
