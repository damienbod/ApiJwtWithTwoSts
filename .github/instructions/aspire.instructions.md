---
applyTo: "ApiJwtWithTwoSts.AppHost/**/*.cs,ApiJwtWithTwoSts.AppHost/**/*.csproj"
---

# Aspire guidance

- Model runnable projects and their dependencies in `ApiJwtWithTwoSts.AppHost/AppHost.cs`.
- Preserve the existing HTTPS launch profiles: the application URLs are also used as OpenID Connect authorities, JWT issuers, and client callback origins.
- Do not add service references or service discovery unless the applications are migrated from their current fixed URLs.
- Keep the AppHost buildable through the repository's standard `dotnet build` workflow.
