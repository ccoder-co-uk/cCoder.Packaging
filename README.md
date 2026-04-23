# cCoder.Packaging

`cCoder.Packaging` contains the Packaging domain for the cCoder platform.

## Contents

- `src/cCoder.Packaging`
  The main library package published to NuGet.
- `src/cCoder.Packaging.Tests`
  Unit tests for the domain.

## Build

```powershell
dotnet build src/cCoder.Packaging.sln -v minimal
```

## Test

```powershell
dotnet test src/cCoder.Packaging.sln -v minimal --no-build
```

## Package

The NuGet package produced by this repository is:

- `cCoder.Packaging`

## Publishing

GitHub Actions is configured to publish the main package using NuGet trusted publishing.

Before the first publish, configure a trusted publishing policy on nuget.org for:

- Repository owner: `ccoder-co-uk`
- Repository: `cCoder.Packaging`
- Workflow file: `publish.yml`

The workflow also expects a `NUGET_USER` repository secret containing the nuget.org profile name used during trusted publishing login.
