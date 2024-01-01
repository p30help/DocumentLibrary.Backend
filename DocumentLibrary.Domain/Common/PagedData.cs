namespace DocumentLibrary.Domain.Common
{
    public class PagedData<T>
    {
        public IEnumerable<T> Data { get; }
        public int CurrentPageNumber { get; }
        public int TotalCount { get; }

        public PagedData(IEnumerable<T> data, int currentPageNumber, int totalCount)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data), "Data can not be null");
            }

            if (currentPageNumber <= 0)
            {
                throw new ArgumentException("Page number must be positive", nameof(totalCount));
            }

            if (totalCount < 0)
            {
                throw new ArgumentException("Total count can not be negative", nameof(totalCount));
            }

            if(data != null && data.Any() && totalCount == 0)
            {
                throw new ArgumentException("Total count is not correct", nameof(totalCount));
            }

            Data = data;
            CurrentPageNumber = currentPageNumber;
            TotalCount = totalCount;
        }
    }
}
