using Microsoft.Extensions.Options.Contextual;

namespace Microsoft.eShopWeb.Web;

public class CatalogDisplayOptions
{
    public int ItemsPerPage { get; set; } = Constants.ITEMS_PER_PAGE;
}

public static class ContextualExtensions
{
    public static string GetOrCreateExperimentSession(this HttpContext ctx)
    {
        const string cookieName = "eShopExp";
        string? sessionId;
        if (!ctx.Request.Cookies.TryGetValue(cookieName, out sessionId))
        {
            sessionId = Guid.NewGuid().ToString();
            ctx.Response.Cookies.Append(cookieName, sessionId);
        }

        return sessionId;
    }

    public static StoreOptionsContext ExtractStoreOptionsContext(this HttpContext ctx)
    {
        return new StoreOptionsContext { SessionId = ctx.GetOrCreateExperimentSession() };
    }
}

[OptionsContext]
public partial struct StoreOptionsContext
{
    public string? SessionId { get; set; }
}
