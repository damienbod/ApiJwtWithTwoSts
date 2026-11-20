---
name: dotnet
description: Work on this repository's .NET 11 projects while following its C# and validation conventions.
---

# .NET development

Use this skill when changing C# source or project files.

1. Identify which project owns the behavior before editing.
2. Keep changes compatible with `net11.0` and follow nearby conventions for namespaces, nullability, and startup code.
3. Do not modify the excluded backup Blazor project or unrelated authentication flows.
4. Build the affected project, then run `dotnet build --no-restore` and the existing test command when practical.
