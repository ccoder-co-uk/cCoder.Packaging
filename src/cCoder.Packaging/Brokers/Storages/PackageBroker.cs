using cCoder.Data;
using cCoder.Data.Extensions;
using cCoder.Data.Models.CMS;
using cCoder.Data.Models.DMS;
using cCoder.Data.Models.Packaging;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


namespace cCoder.Packaging.Brokers.Storages;

public interface IPackageBroker
{
    IQueryable<Package> GetAllPackages(bool ignoreFilters);
    ValueTask<Package> AddPackageAsync(Package entity);
    ValueTask<Package> UpdatePackageAsync(Package entity);
    ValueTask<int> DeletePackageAsync(Package entity);
    ValueTask DeleteAllPackagesAsync(IEnumerable<Package> items);
    Package ExportRoles(int appId);
    Package ExportFolderRoles(int appId);
    Package ExportLayouts(int appId);
    Package ExportTemplates(int appId);
    Package ExportComponents(int appId);
    Package ExportScripts(int appId);
    Package ExportResources(int appId);
    Package ExportPages(int appId);
    Package ExportFlowDefinitions(int appId);
    Package ExportPageRoles(int appId);
    Package ExportCalendars(int appId);
    Package ExportCalendarEvents(int appId);
}

public class PackageBroker(ICoreContextFactory coreContextFactory) : IPackageBroker
{

    public IQueryable<Package> GetAllPackages(bool ignoreFilters)
    {
        CoreDataContext coreDataContext = coreContextFactory.CreateCoreContext();
        return ignoreFilters
            ? coreDataContext.Packages.IgnoreQueryFilters()
            : coreDataContext.Packages;
    }

    public async ValueTask<Package> AddPackageAsync(Package entity)
    {
        using CoreDataContext coreDataContext = coreContextFactory.CreateCoreContext();
        Package result = (await coreDataContext.Packages.AddAsync(entity)).Entity;
        _ = await coreDataContext.SaveChangesAsync();
        return result;
    }

    public async ValueTask<Package> UpdatePackageAsync(Package entity)
    {
        using CoreDataContext coreDataContext = coreContextFactory.CreateCoreContext();
        Package result = coreDataContext.Packages.Update(entity).Entity;
        _ = await coreDataContext.SaveChangesAsync();
        return result;
    }

    public async ValueTask<int> DeletePackageAsync(Package entity)
    {
        using CoreDataContext coreDataContext = coreContextFactory.CreateCoreContext();
        coreDataContext.Packages.Remove(entity);
        return await coreDataContext.SaveChangesAsync();
    }

    public async ValueTask DeleteAllPackagesAsync(IEnumerable<Package> items)
    {
        if (items == null || !items.Any())
            return;

        using CoreDataContext coreDataContext = coreContextFactory.CreateCoreContext();
        coreDataContext.Packages.RemoveRange(items);
        _ = await coreDataContext.SaveChangesAsync();
    }

    public int? GetAppId(Package entity)
    {
        return null;
    }

    public Package ExportRoles(int appId)
    {
        using CoreDataContext coreDataContext = coreContextFactory.CreateCoreContext();
        JsonSerializerSettings serializerSettings = CreateSerializerSettings();

        Role[] roles = coreDataContext.Roles
            .Where(role => role.AppId == appId)
            .Include(role => role.Users)
                .ThenInclude(userRole => userRole.User)
            .ToArray();

        return new Package("Roles")
        {
            Items =
            [
                new PackageItem
                {
                    Type = "Core/Role",
                    Data = roles.Select(role => new { role.Name, role.Privs }).ToJson(serializerSettings),
                },
            ],
        };
    }

    public Package ExportFolderRoles(int appId)
    {
        using CoreDataContext coreDataContext = coreContextFactory.CreateCoreContext();
        JsonSerializerSettings serializerSettings = CreateSerializerSettings();

        Folder[] folders = coreDataContext.Folders
            .Where(folder => folder.AppId == appId)
            .Include(folder => folder.Roles)
                .ThenInclude(folderRole => folderRole.Role)
            .ToArray();

        return new Package("FolderRoles")
        {
            Items =
            [
                new PackageItem
                {
                    Type = "Core/FolderRole",
                    Data = folders.SelectMany(
                        folder => folder.Roles,
                        (folder, folderRole) => new { folder.Path, folderRole.Role.Name }
                    )
                    .ToArray()
                    .ToJson(serializerSettings),
                },
            ],
        };
    }

    public Package ExportLayouts(int appId)
    {
        using CoreDataContext coreDataContext = coreContextFactory.CreateCoreContext();
        JsonSerializerSettings serializerSettings = CreateSerializerSettings();

        return new Package("Layouts")
        {
            Items =
            [
                new PackageItem
                {
                    Type = "Core/Layout",
                    Data = coreDataContext
                        .Layouts
                        .Where(layout => layout.AppId == appId)
                        .Select(layout => new
                        {
                            layout.Name,
                            layout.HeaderHtml,
                            layout.Html,
                            layout.Script,
                            layout.LastUpdated,
                        })
                        .ToJson(serializerSettings),
                },
            ],
        };
    }

    public Package ExportTemplates(int appId)
    {
        using CoreDataContext coreDataContext = coreContextFactory.CreateCoreContext();
        JsonSerializerSettings serializerSettings = CreateSerializerSettings();

        return new Package("Templates")
        {
            Items =
            [
                new PackageItem
                {
                    Type = "Core/Template",
                    Data = coreDataContext
                        .Templates
                        .Where(template => template.AppId == appId)
                        .Select(template => new
                        {
                            template.Name,
                            template.ResourceKey,
                            template.RawString,
                            template.LastUpdated,
                        })
                        .ToJson(serializerSettings),
                },
            ],
        };
    }

    public Package ExportComponents(int appId)
    {
        using CoreDataContext coreDataContext = coreContextFactory.CreateCoreContext();
        JsonSerializerSettings serializerSettings = CreateSerializerSettings();

        return new Package("Components")
        {
            Items =
            [
                new PackageItem
                {
                    Type = "Core/Component",
                    Data = coreDataContext
                        .Components
                        .Where(component => component.AppId == appId)
                        .Select(component => new
                        {
                            component.Name,
                            component.Key,
                            component.ResourceKey,
                            component.Script,
                            component.Content,
                            component.LastUpdated,
                        })
                        .ToJson(serializerSettings),
                },
            ],
        };
    }

    public Package ExportScripts(int appId)
    {
        using CoreDataContext coreDataContext = coreContextFactory.CreateCoreContext();
        JsonSerializerSettings serializerSettings = CreateSerializerSettings();

        return new Package("Scripts")
        {
            Items =
            [
                new PackageItem
                {
                    Type = "Core/Script",
                    Data = coreDataContext
                        .Scripts
                        .Where(script => script.AppId == appId)
                        .Select(script => new
                        {
                            script.Name,
                            script.Content,
                            script.LastUpdated,
                        })
                        .ToJson(serializerSettings),
                },
            ],
        };
    }

    public Package ExportResources(int appId)
    {
        using CoreDataContext coreDataContext = coreContextFactory.CreateCoreContext();
        JsonSerializerSettings serializerSettings = CreateSerializerSettings();

        return new Package("Resources")
        {
            Items =
            [
                new PackageItem
                {
                    Type = "Core/Resource",
                    Data = coreDataContext
                        .Resources
                        .Where(resource => resource.AppId == appId)
                        .Select(resource => new
                        {
                            resource.Culture,
                            resource.Key,
                            resource.Name,
                            resource.DisplayName,
                            resource.ShortDisplayName,
                            resource.Description,
                            resource.LastUpdated,
                        })
                        .ToJson(serializerSettings),
                },
            ],
        };
    }

    public Package ExportPages(int appId)
    {
        using CoreDataContext coreDataContext = coreContextFactory.CreateCoreContext();
        JsonSerializerSettings serializerSettings = CreateSerializerSettings();

        List<Page> appPages = coreDataContext.Pages
            .Where(page => page.AppId == appId)
            .Include(page => page.Contents)
            .Include(page => page.PageInfo)
            .AsNoTracking()
            .ToList();

        Dictionary<int, Page> pageDictionary = appPages.ToDictionary(page => page.Id);

        foreach (Page page in appPages)
            if (
                page.ParentId is not null
                && pageDictionary.TryGetValue(page.ParentId.Value, out Page parent)
            )
                page.Parent = parent;

        return new Package("Pages")
        {
            Items =
            [
                new PackageItem
                {
                    Type = "Core/Page",
                    Data = appPages
                        .Select(page =>
                        {
                            Page rootPage = page;
                            while (rootPage.ParentId is not null)
                                rootPage = rootPage.Parent;

                            if (page.ParentId is not null && string.IsNullOrEmpty(rootPage.Path))
                                page.Path = $"/{page.Path}";

                            return new
                            {
                                page.Path,
                                page.Name,
                                page.ResourceKey,
                                page.ShowOnMenus,
                                page.Order,
                                page.LastUpdated,
                                page.Layout,
                                Contents = page
                                    .Contents.Select(content => new
                                    {
                                        content.CultureId,
                                        content.Name,
                                        content.Html,
                                    })
                                    .ToArray(),
                                PageInfo = page
                                    .PageInfo.Select(info => new
                                    {
                                        info.CultureId,
                                        info.Description,
                                        info.Keywords,
                                        info.Title,
                                    })
                                    .ToArray(),
                            };
                        })
                        .ToJson(serializerSettings),
                },
            ],
        };
    }

    public Package ExportFlowDefinitions(int appId)
    {
        using CoreDataContext coreDataContext = coreContextFactory.CreateCoreContext();
        JsonSerializerSettings serializerSettings = CreateSerializerSettings();

        return new Package("Workflows")
        {
            Items =
            [
                new PackageItem
                {
                    Type = "Core/FlowDefinition",
                    Data = coreDataContext
                        .FlowDefinitions
                        .Include(flow => flow.App)
                        .Where(flow => flow.AppId == appId)
                        .Select(flow => new
                        {
                            ProcessName = flow.App.Name,
                            flow.Name,
                            flow.ReportingComponentName,
                            flow.InstanceReportingComponentName,
                            flow.Description,
                            flow.DefinitionJson,
                            flow.ConfigJson,
                            flow.LastUpdated,
                        })
                        .ToJson(serializerSettings),
                },
            ],
        };
    }

    public Package ExportPageRoles(int appId)
    {
        using CoreDataContext coreDataContext = coreContextFactory.CreateCoreContext();
        JsonSerializerSettings serializerSettings = CreateSerializerSettings();

        Role[] roleData = coreDataContext.Roles
            .Where(role => role.AppId == appId)
            .ToArray();

        Dictionary<Guid, string> roleNamesById = roleData.ToDictionary(role => role.Id, role => role.Name);

        Page[] pages = coreDataContext.Pages
            .Where(page => page.AppId == appId)
            .Include(page => page.Roles)
            .ToArray();

        ExportPageRoleInfo[] pageRoles = pages
            .Where(page => page.Roles != null)
            .SelectMany(page =>
                page.Roles
                    .Where(pageRole => roleNamesById.ContainsKey(pageRole.RoleId))
                    .Select(pageRole => new ExportPageRoleInfo
                    {
                        Path = page.Path,
                        Role = roleNamesById[pageRole.RoleId],
                    })
            )
            .ToArray();

        return new Package("PageRoles")
        {
            Items =
            [
                new PackageItem
                {
                    Type = "Core/PageRole",
                    Data = pageRoles.ToJson(serializerSettings),
                },
            ],
        };
    }

    public Package ExportCalendars(int appId)
    {
        using CoreDataContext coreDataContext = coreContextFactory.CreateCoreContext();
        JsonSerializerSettings serializerSettings = CreateSerializerSettings();

        return new Package("Calendars")
        {
            Items =
            [
                new PackageItem
                {
                    Type = "Core/Calendar",
                    Data = coreDataContext
                        .Calendars
                        .Where(calendar => calendar.AppId == appId)
                        .Select(calendar => new { calendar.Name, calendar.Description })
                        .ToJson(serializerSettings),
                },
            ],
        };
    }

    public Package ExportCalendarEvents(int appId)
    {
        using CoreDataContext coreDataContext = coreContextFactory.CreateCoreContext();
        JsonSerializerSettings serializerSettings = CreateSerializerSettings();

        return new Package("CalendarEvents")
        {
            Items =
            [
                new PackageItem
                {
                    Type = "Core/CalendarEvent",
                    Data = coreDataContext
                        .Events
                        .Include(calendarEvent => calendarEvent.Calendar)
                        .Where(calendarEvent => calendarEvent.Calendar.AppId == appId)
                        .Select(calendarEvent => new
                        {
                            CalendarName = calendarEvent.Calendar.Name,
                            calendarEvent.Name,
                            calendarEvent.Start,
                            calendarEvent.Description,
                            calendarEvent.DurationInTicks,
                        })
                        .ToJson(serializerSettings),
                },
            ],
        };
    }

    private static JsonSerializerSettings CreateSerializerSettings()
    {
        JsonSerializerSettings serializerSettings = ObjectExtensions.GetJSONSettings();
        serializerSettings.TypeNameHandling = TypeNameHandling.None;
        return serializerSettings;
    }

    private class ExportPageRoleInfo
    {
        public string Path { get; set; }
        public string Role { get; set; }
    }
}


