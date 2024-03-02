using Excos.Options;
using Microsoft.Extensions.Options.Contextual;

namespace Microsoft.eShopWeb.Web;

public class CatalogDisplayOptions
{
    public int ItemsPerPage { get; set; } = Constants.ITEMS_PER_PAGE;

    public FeatureMetadata? FeatureMetadata { get; set; }
}

public static class ContextualExtensions
{
    public static Guid GetOrCreateExperimentSession(this HttpContext ctx)
    {
        const string cookieName = "eShopExp";
        string? sessionId;
        Guid parsedSessionId;
        if (!ctx.Request.Cookies.TryGetValue(cookieName, out sessionId))
        {
            parsedSessionId = Guid.NewGuid();
            sessionId = parsedSessionId.ToString();
            ctx.Response.Cookies.Append(cookieName, sessionId);
        }
        else
        {
            parsedSessionId = Guid.Parse(sessionId);
        }

        return parsedSessionId;
    }

    public static StoreOptionsContext ExtractStoreOptionsContext(this HttpContext ctx)
    {
        return new StoreOptionsContext { SessionId = ctx.GetOrCreateExperimentSession() };
    }
}

[OptionsContext]
public partial struct StoreOptionsContext
{
    public Guid SessionId { get; set; }
}
