---
name: aspire
description: Maintain Aspire orchestration for the API, identity providers, and web clients in this repository.
---

# Aspire orchestration

Use this skill when changing `ApiJwtWithTwoSts.AppHost`.

1. Declare application resources and their dependencies in `AppHost.cs`.
2. Keep the existing HTTPS ports and Development launch profiles: issuer validation and OIDC callbacks rely on them.
3. Do not wire Aspire service discovery into an application unless its fixed URLs are migrated as part of the same change.
4. Keep the AppHost compatible with the standard solution restore and build commands.
