
<img src="https://i.ibb.co/StD1vdZ/ezgif-1-48ccd4b6d3.gif"/>

This project adds a compact, JavaScript-free pagination component to Blazor WASM and Blazor Server applications.
<br/>
Please note that it is currently a work-in-progress. It has not yet been unit tested or published to NuGet.

<h3>Usage</h3>

The component is dependant on two parameters; `TotalPages` and `PageChanged`. `PageChanged`, as its name suggests, is an event that is triggered when the user changes page through one of the component's controls. It must be assigned a delegate otherwise an exception will be thrown at runtime.
<br/><br/>
Typical usage may look like the following example:

```
@using MShortt.Blazor.Pager

@foreach(var item in PageItems)
{
  //Razor code and HTML for displaying per-item data
}

<Pager TotalPages="TotalPages" PageChanged="SetPageDataAsync" />

@code {
  private int TotalPages;
  private IEnumerable<MyData> PageItems;
  
  protected override async Task OnInitializedAsync()
  {
    //Get the data for page one on load.
    await SetTotalPagesAsync();
    await SetPageDataAsync(1);
  }
  
  private async Task SetPageDataAsync(int page) 
  {
    //Refresh the total pages to keep the view in-sync with the backend
    await SetTotalPagesAsync();
    
    PageItems = //Call to fetch page data
  }
  
  private async Task SetTotalPagesAsync()
  {
    TotalPages = //Code to get total pages
  }
}
```

<h4>Programmatic Page Changes</h4>

Page changes that occur outside of the component will not trigger a `PageChanged` callback or be visibly reflected. The component exposes a public method, `ChangePageAsync`, to counter this.

```
<Pager @ref="PagerReference" Page="Page" TotalPages="TotalPages" PageChanged="SetPageDataAsync" />

@code {
  private Pager PagerReference;
  //Other fields and properties
  
  private async Task ProgramaticallyUpdatePagerAsync()
  {
    //Change page to 5.
    await PagerReference.ChangePageAsync(5);
  }
}
```

<h4>Additional Parameters</h4>

* `Page`: For setting an initial page number for the component (such as a page number that was cached)
* `Disabled`: If set to `true`, all controls cannot be interacted with by the user
* `HideIfNoResults`: If set to `false`, the pager will not be rendered if `TotalPages` is 0
* `ShowInputter`: If set to `false`, the manual input field for a page number will not be available for users

<h3>Styling</h3>

Apart from some minor positioning and sizing stuff, the component purposely has no styling as I don't want to force a particular look or feel on consumers. Each individual piece that makes up the component has a CSS class; these can all be given styling in the app's global stylesheet or scoped stylesheets.

* `mshortt-pager-btn`: All buttons
* `mshortt-pager-prev-btn`: The "Prev" button
* `mshortt-pager-next-btn`: The "Next" button
* `mshortt-pager-jumpback-btn`: The "<<" button
* `mshortt-pager-jumpahead-btn`: The ">>" button
* `mshortt-pager-inputter`: The manual input field
* `mshortt-pager-indicator`: The "Page X of Y" label
