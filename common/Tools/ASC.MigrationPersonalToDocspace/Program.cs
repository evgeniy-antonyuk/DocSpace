﻿// (c) Copyright Ascensio System SIA 2010-2022
//
// This program is a free software product.
// You can redistribute it and/or modify it under the terms
// of the GNU Affero General Public License (AGPL) version 3 as published by the Free Software
// Foundation. In accordance with Section 7(a) of the GNU AGPL its Section 15 shall be amended
// to the effect that Ascensio System SIA expressly excludes the warranty of non-infringement of
// any third-party rights.
//
// This program is distributed WITHOUT ANY WARRANTY, without even the implied warranty
// of MERCHANTABILITY or FITNESS FOR A PARTICULAR  PURPOSE. For details, see
// the GNU AGPL at: http://www.gnu.org/licenses/agpl-3.0.html
//
// You can contact Ascensio System SIA at Lubanas st. 125a-25, Riga, Latvia, EU, LV-1021.
//
// The  interactive user interfaces in modified source and object code versions of the Program must
// display Appropriate Legal Notices, as required under Section 5 of the GNU AGPL version 3.
//
// Pursuant to Section 7(b) of the License you must retain the original Product logo when
// distributing the program. Pursuant to Section 7(e) we decline to grant you any rights under
// trademark law for use of our trademarks.
//
// All the Product's GUI elements, including illustrations and icon sets, as well as technical writing
// content are licensed under the terms of the Creative Commons Attribution-ShareAlike 4.0
// International. See the License terms at http://creativecommons.org/licenses/by-sa/4.0/legalcode

using ASC.Common.Utils;

using Autofac.Extensions.DependencyInjection;

var options = new WebApplicationOptions
{
    Args = args,
    ContentRootPath = WindowsServiceHelpers.IsWindowsService() ? AppContext.BaseDirectory : default
};

var builder = WebApplication.CreateBuilder(options);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.WebHost.ConfigureAppConfiguration((hostContext, config) =>
{
    config.AddJsonFile($"appsettings.json", true);
    var buildedConfig = config.Build();

    var path = buildedConfig["pathToConf"];
    try
    {
        if (!Path.IsPathRooted(path))
        {
            path = Path.GetFullPath(CrossPlatform.PathCombine(hostContext.HostingEnvironment.ContentRootPath, path));
        }

        config.SetBasePath(path);

        config.AddJsonFile($"storage.json", false)
        .AddCommandLine(args);
    }
    catch (Exception ex)
    {
        throw new Exception("Wrong pathToConf, change pathToConf in appsettings.json");
    }
});

var config = builder.Configuration;

builder.WebHost.ConfigureServices((hostContext, services) =>
{
    services.AddScoped<EFLoggerFactory>();
    services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    services.AddHttpClient();
    services.AddBaseDbContextPool<AccountLinkContext>();
    services.AddBaseDbContextPool<BackupsContext>();
    services.AddBaseDbContextPool<FilesDbContext>();
    services.AddBaseDbContextPool<CoreDbContext>();
    services.AddBaseDbContextPool<TenantDbContext>();
    services.AddBaseDbContextPool<UserDbContext>();
    services.AddBaseDbContextPool<TelegramDbContext>();
    services.AddBaseDbContextPool<CustomDbContext>();
    services.AddBaseDbContextPool<WebstudioDbContext>();
    services.AddBaseDbContextPool<InstanceRegistrationContext>();
    services.AddBaseDbContextPool<IntegrationEventLogContext>();
    services.AddBaseDbContextPool<FeedDbContext>();
    services.AddBaseDbContextPool<MessagesContext>();
    services.AddBaseDbContextPool<WebhooksDbContext>();
    services.AddAutoMapper(BaseStartup.GetAutoMapperProfileAssemblies());
    services.AddMemoryCache();
    services.AddSingleton<IEventBus, MockEventBusRabbitMQ>();
    services.AddCacheNotify(config);

    var diHelper = new DIHelper();
    diHelper.Configure(services);

    diHelper.TryAdd<TempStream>();
    diHelper.TryAdd<DbFactory>();
    diHelper.TryAdd<ModuleProvider>();
    diHelper.TryAdd<DbTenantService>();
    diHelper.TryAdd<StorageFactoryConfig>();
    diHelper.TryAdd<StorageFactory>();
    diHelper.TryAdd<DiscDataStore>();
    diHelper.TryAdd<S3Storage>();
});

var app = builder.Build();

var tenant = Int32.Parse(args[0]);
var userName = args[1];
var region = args[2];

var migrationCreator = new MigrationCreator(app.Services, tenant, userName, region);
migrationCreator.Create();

var migrationRunner = new MigrationRunner(app.Services.CreateScope().ServiceProvider, userName + ".tar.gz", region);
migrationRunner.Run();

Directory.GetFiles(AppContext.BaseDirectory).Where(f => f.Contains(".tar")).ToList().ForEach(File.Delete);

if (Directory.Exists(AppContext.BaseDirectory + "\\temp"))
{
    Directory.Delete(AppContext.BaseDirectory + "\\temp");
}