namespace BlazorClient.Server;

public static class SecurityHeadersDefinitions
{
    private static HeaderPolicyCollection? policy;

    public static HeaderPolicyCollection GetHeaderPolicyCollection(bool isDev, string? idpHost1, string? idpHost2)
    {
        ArgumentNullException.ThrowIfNull(idpHost1);
        ArgumentNullException.ThrowIfNull(idpHost2);

        // Avoid building a new HeaderPolicyCollection on every request for performance reasons.
        // Where possible, cache and reuse HeaderPolicyCollection instances.
        if (policy != null) return policy;

        policy = new HeaderPolicyCollection()
            .AddFrameOptionsDeny()
            .AddContentTypeOptionsNoSniff()
            .AddReferrerPolicyStrictOriginWhenCrossOrigin()
            .AddCrossOriginOpenerPolicy(builder => builder.SameOrigin())
            .AddCrossOriginResourcePolicy(builder => builder.SameOrigin())
            .AddCrossOriginEmbedderPolicy(builder => builder.RequireCorp())
            .AddContentSecurityPolicy(builder =>
            {
                builder.AddObjectSrc().None();
                builder.AddBlockAllMixedContent();
                builder.AddImgSrc().Self().From("data:");
                builder.AddFormAction().Self().From(idpHost1).From(idpHost2);
                builder.AddFontSrc().Self();
                builder.AddStyleSrc().Self();
                builder.AddBaseUri().Self();
                builder.AddFrameAncestors().None();

                // due to Blazor
                builder.AddScriptSrc()
                    // .Self() Add this if you want to use the visual studio debugging tools
                    .WithNonce()
                    .UnsafeEval();
            })
            .RemoveServerHeader()
            .AddPermissionsPolicyWithDefaultSecureDirectives();

        if (!isDev)
        {
            // maxage = one year in seconds
            policy.AddStrictTransportSecurityMaxAgeIncludeSubDomains();
        }

        return policy;
    }
}