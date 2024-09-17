﻿
using System;
using System.Collections.Generic;

namespace UmojaCampus.Shared.Wrapper
{
    public class PaginatedResult<T> 
    {
        public PaginatedResult(List<T> data)
        {
            Data = data ?? [];
        }

        public bool Succeeded { get; set; }
        public List<T> Data { get; set; } = [];
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }

        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;

        private PaginatedResult(bool succeeded, List<T> data = default, int count = 0, int page = 1, int pageSize = 10)
        {
            Data = data ?? [];
            CurrentPage = page;
            Succeeded = succeeded;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count / (double) pageSize);
            TotalCount = count;
        }

        public static PaginatedResult<T> Failure()
        {
            return new PaginatedResult<T>(false, default);
        }

        public static PaginatedResult<T> Success(List<T> data, int count, int page, int pageSize)
        {
            return new PaginatedResult<T>(true, data, count, page, pageSize);
        }          
    }
}
