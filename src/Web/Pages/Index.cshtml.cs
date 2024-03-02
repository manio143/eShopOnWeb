using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.Web.Services;
using Microsoft.eShopWeb.Web.ViewModels;
using Microsoft.Extensions.Options.Contextual;

namespace Microsoft.eShopWeb.Web.Pages;

public class IndexModel : PageModel
{
    private readonly ICatalogViewModelService _catalogViewModelService;
    private readonly IContextualOptions<CatalogDisplayOptions> _contextualOptions;
    private readonly IExperimentationService _experimentationService;

    public IndexModel(
        ICatalogViewModelService catalogViewModelService,
        IContextualOptions<CatalogDisplayOptions> contextualOptions,
        IExperimentationService experimentationService)
    {
        _catalogViewModelService = catalogViewModelService;
        _contextualOptions = contextualOptions;
        _experimentationService = experimentationService;
    }

    public required CatalogIndexViewModel CatalogModel { get; set; } = new CatalogIndexViewModel();

    public async Task OnGet(CatalogIndexViewModel catalogModel, int? pageId)
    {
        var context = HttpContext.ExtractStoreOptionsContext();
        var options = await _contextualOptions.GetAsync(context, default);

        if (options.FeatureMetadata is not null)
        {
            foreach (var feature in options.FeatureMetadata.Features)
            {
                await _experimentationService.SaveExperimentAssignmentAsync(context.SessionId, feature.FeatureName, feature.VariantId);
            }
        }

        CatalogModel = await _catalogViewModelService.GetCatalogItems(pageId ?? 0, options.ItemsPerPage, catalogModel.BrandFilterApplied, catalogModel.TypesFilterApplied);
    }
}
