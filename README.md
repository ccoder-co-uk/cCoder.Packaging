# cCoder.Packaging

`cCoder.Packaging` contains the Packaging domain for the cCoder platform.

## Functionality

Packaging provides a small domain for grouping portable data into packages and
package items. It is intended to support migration-style movement of data
between cCoder systems, while keeping the package metadata and package item
payloads queryable through the normal API stack.

Consumers get:

- Package CRUD through OData.
- Package item CRUD within the context of a package.
- Export orchestration for known cCoder domains such as content, documents,
  workflow, scheduling, and app security.
- Import orchestration hooks that raise package import events.
- A simple authenticated Web app for manually testing package and package item
  CRUD.

Full migration scenarios need a deliberate source and target environment, so
this repository does not introduce a synthetic two-database integration harness.
The Web UI and acceptance tests provide the baseline proof that the package API
surface starts and responds correctly.

## Contents

- `src/cCoder.Packaging`
  The main library package published to NuGet.
- `src/cCoder.Packaging.Tests`
  Unit tests for the domain.
- `src/Apps/Packaging.Web`
  A simple Web app hosting the Packaging API and manual testing UI.
- `src/Apps/Packaging.Web.AcceptanceTests`
  HTTP acceptance tests for the Packaging Web app.

## Configuration

`Packaging.Web` uses the normal application configuration pipeline:

- `appsettings.json`
- `appsettings.{Environment}.json`
- environment variables

Required settings are:

- `ConnectionStrings__Core`
- `ConnectionStrings__SSO`
- `Settings__DecryptionKey`

The local test UI is available at `/tools/index.html`, and `/Health` returns a
fixed `OK` response for basic health checks.

## Build

```powershell
dotnet build src/cCoder.Packaging.sln -v minimal
```

## Test

```powershell
dotnet test src/cCoder.Packaging.sln -v minimal --no-build
```

## Run The Web App

```powershell
dotnet run --project src/Apps/Packaging.Web/Packaging.Web.csproj --launch-profile https
```

Then open:

- `https://localhost:7173/tools/index.html`
- `https://localhost:7173/swagger`
- `https://localhost:7173/Health`

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
