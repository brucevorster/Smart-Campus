using System;

namespace UmojaCampus.Shared.Helpers
{
    public abstract class PagedResultBase
    {
        public int CurrentPage { get; set; }
        public int PagedCount { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }

        public int FirstRowOnPage
        {
            get { return (CurrentPage - 1) * PageSize + 1; }
        }

        public int LastRowOnPage
        {
            get { return Math.Min(CurrentPage * PageSize, RowCount); }
        }
    }
}
