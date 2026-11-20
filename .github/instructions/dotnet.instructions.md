---
applyTo: "**/*.cs,**/*.csproj"
---

# .NET guidance

- Target `net11.0` for active projects and preserve the existing package-version conventions.
- Follow nearby formatting and naming. Prefer top-level statements in simple startup files and file-scoped namespaces in regular C# files.
- Preserve nullable-reference settings already chosen by each project; do not broaden unrelated nullable changes.
- Keep authentication behavior and configuration in the existing project boundaries.
- Validate with `dotnet build --no-restore` and run relevant existing tests.
