using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.eShopWeb.Web.Services;
using Microsoft.eShopWeb.Web.ViewModels;
using Microsoft.Extensions.Options.Contextual;

namespace Microsoft.eShopWeb.Web.Pages;

public class IndexModel : PageModel
{
    private readonly ICatalogViewModelService _catalogViewModelService;
    private readonly IContextualOptions<CatalogDisplayOptions> _contextualOptions;

    public IndexModel(
        ICatalogViewModelService catalogViewModelService,
        IContextualOptions<CatalogDisplayOptions> contextualOptions)
    {
        _catalogViewModelService = catalogViewModelService;
        _contextualOptions = contextualOptions;
    }

    public required CatalogIndexViewModel CatalogModel { get; set; } = new CatalogIndexViewModel();

    public async Task OnGet(CatalogIndexViewModel catalogModel, int? pageId)
    {
        var options = await _contextualOptions.GetAsync(HttpContext.ExtractStoreOptionsContext(), default);
        CatalogModel = await _catalogViewModelService.GetCatalogItems(pageId ?? 0, options.ItemsPerPage, catalogModel.BrandFilterApplied, catalogModel.TypesFilterApplied);
    }
}
