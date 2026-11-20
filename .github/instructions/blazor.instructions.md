---
applyTo: "BlazorClient/**/*.razor,BlazorClient/**/*.cs"
---

# Blazor guidance

- Keep browser-executed code in `BlazorClient/Client` and server-only authentication and hosting behavior in `BlazorClient/Server`.
- Do not move client secrets, server credentials, or privileged token handling into the WebAssembly client.
- Preserve the separate T1 and T2 authentication flows and their callback paths.
- Follow existing component and code-behind patterns; validate changes by building `BlazorClient/Server/BlazorClient.Server.csproj`.
