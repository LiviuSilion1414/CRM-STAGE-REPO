namespace PlannerCRM.Client.Components.Pagination;

public partial class Paginator : ComponentBase
{
    [Parameter] public int CollectionSize { get; set; }
    [Parameter] public int Offset { get; set; } = 5;
    [Parameter] public EventCallback<(int, int)> Paginate { get; set; }

    private List<PagingLink> _pages;
    private int _elementsToSkip;
    private int _pageNumber;
    private int _totalPages;

    protected override void OnInitialized()
    {
        _elementsToSkip = 0;
        _pageNumber = 1;
        _pages = new();

        _totalPages = CalculateTotalPages(CollectionSize, Offset);

        Enumerable
            .Range(1, _totalPages)
            .ToList()
            .ForEach(pageNumber => _pages.Add(new PagingLink(pageNumber, false)));
    }

    private static int CalculateTotalPages(int collectionSize, int offset)
    {
        if (collectionSize < offset)
        {
            return 1;
        }

        return (collectionSize > offset && (collectionSize % offset > 0))
            ? (collectionSize / offset) + 1
            : collectionSize / offset;
    }

    private string SetClass(int pageNumber)
    {
        return pageNumber == _pageNumber
            ? CssClass.Active
            : CssClass.Empty;
    }

    private async Task Previous()
    {
        if (_elementsToSkip >= Offset)
        {
            _elementsToSkip -= Offset;
            _pageNumber--;

            await InvokeCallbackAsync(Paginate, _elementsToSkip, Offset);
        }

    }

    private async Task Next()
    {
        if (_elementsToSkip < CollectionSize && ((_elementsToSkip + Offset) <= CollectionSize))
        {
            _elementsToSkip += Offset;
            _pageNumber++;

            await InvokeCallbackAsync(Paginate, _elementsToSkip, Offset);
        }
    }

    private static async Task InvokeCallbackAsync(EventCallback<(int, int)> callback, int skip, int take)
        => await callback.InvokeAsync((skip, take));
}

internal class PagingLink
{
    internal int PageNumber { get; set; }
    internal bool Active { get; set; }

    internal PagingLink(int pageNumber, bool active)
    {
        PageNumber = pageNumber;
        Active = active;
    }
}