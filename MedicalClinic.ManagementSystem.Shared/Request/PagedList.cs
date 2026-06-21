namespace MedicalClinic.ManagementSystem.Shared.Request;

public class PagedList<T>
{
    public IReadOnlyList<T> Items { get; }
    public MetaData MetaData { get; }

    public PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
    {
        MetaData = new MetaData
        (
            currentPage: pageNumber,
            totalPages: (int)Math.Ceiling(count / (double)pageSize),
            pageSize: pageSize,
            totalCount: count
        );

        Items = new List<T>(items).AsReadOnly();
    }
}
