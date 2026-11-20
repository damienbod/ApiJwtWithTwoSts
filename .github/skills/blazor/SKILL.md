---
name: blazor
description: Make secure, repository-consistent changes to the hosted Blazor WebAssembly application.
---

# Blazor development

Use this skill when changing the hosted Blazor application.

1. Keep browser UI and browser-safe services in `BlazorClient/Client`; keep OIDC configuration and server authentication in `BlazorClient/Server`.
2. Never expose client secrets, private credentials, or server-only token operations to WebAssembly.
3. Preserve the T1 and T2 sign-in flows and callback routes.
4. Build `BlazorClient/Server/BlazorClient.Server.csproj` to validate changes.
