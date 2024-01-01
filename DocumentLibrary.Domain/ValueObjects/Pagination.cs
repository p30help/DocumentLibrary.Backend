using DocumentLibrary.Domain.Common;
using DocumentLibrary.Domain.Common.Exceptions;
using System.Diagnostics;

namespace DocumentLibrary.Domain.ValueObjects
{
    public class Pagination : ValueObject
    {
        public int PageSize { get; }

        public int PageNumber { get; }

        [DebuggerStepThrough]
        public Pagination(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0)
            {
                throw new InvalidValueObjectStateException("PageNumber is incorrect");
            }

            if (pageSize <= 0)
            {
                throw new InvalidValueObjectStateException("PageSize is incorrect");
            }

            if (pageSize > 100)
            {
                throw new InvalidValueObjectStateException("PageSize can not exceed more than 100");
            }

            PageSize = pageSize;
            PageNumber = pageNumber;
        }


        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return PageSize;
            yield return PageNumber;
        }

        public override string ToString()
        {
            return $"PgNum:{PageNumber}, PgSize:{PageSize}";
        }
    }
}