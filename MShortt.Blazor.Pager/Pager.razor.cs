using Microsoft.AspNetCore.Components;

namespace MShortt.Blazor.Pager;

public partial class Pager : IDisposable
{
    #region Fields
    private readonly CancellationTokenSource CTS;
    private int JumpAmount;
    #endregion

    #region Backing Fields
    private int _page;
    private int _totalPages;
    private EventCallback<int> _pageChanged;
    #endregion

    #region Parameters
    [Parameter]
    public int Page 
    {
        get => _page;
        set
        {
            _page = value < 1 ? 1 : value;
        }
    }

    [Parameter]
    public int TotalPages
    {
        get => _totalPages;
        set
        {
            if(_totalPages != value)
            {
                _totalPages = value < 1 ? 1 : value;
                SetJumpAmount();
            }
        }
    }

    [Parameter]
    public EventCallback<int> PageChanged
    {
        get => _pageChanged;
        set => _pageChanged = value.HasDelegate ? value : throw new ArgumentOutOfRangeException(nameof(_pageChanged));
    }
    #endregion

    #region Properties
    private bool AllowJumping 
    { 
        get => TotalPages >= 3;
    } 
    #endregion

    #region Constructors
    public Pager()
    {
        CTS = new();
        Page = 1;
        TotalPages = 1;
        ShowInputter = true;
        HideIfNoResults = true;
    }
    #endregion

    #region Methods
    private async Task JumpPagesAsync(bool backwards)
    {
        int page = backwards ? Page - JumpAmount : Page + JumpAmount;
        await ChangePageAsync(page);
    }

    public async Task ChangePageAsync(int page)
    {
        if (page > TotalPages)
            page = TotalPages;

        Page = page;

        await PageChanged.InvokeAsync(Page);
    }

    private void SetJumpAmount()
    {
        if (TotalPages > 1)
        {
            int powersOfTen = TotalPages.ToString().Count() - 1;
            if(powersOfTen > 1)
            {
                JumpAmount = TotalPages / 10;
            }

            else
            {
                JumpAmount = TotalPages / 2;
            }
        }

        else
        {
            JumpAmount = 1;
        }
    }

    public void Dispose()
    {
        CTS.Dispose();
    }
    #endregion
}
