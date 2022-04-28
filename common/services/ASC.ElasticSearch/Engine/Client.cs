// (c) Copyright Ascensio System SIA 2010-2022
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

namespace ASC.ElasticSearch;

[Singletone]
public class Client
{
    public ElasticClient Instance
    {
        get
        {
            if (_client != null)
            {
                return _client;
            }

            lock (_locker)
            {
                if (_client != null)
                {
                    return _client;
                }

                using var scope = _serviceProvider.CreateScope();
                var CoreConfiguration = _serviceProvider.GetService<CoreConfiguration>();
                var launchSettings = CoreConfiguration.GetSection<Settings>(Tenant.DefaultTenant) ?? _settings;

                var uri = new Uri(string.Format("{0}://{1}:{2}", launchSettings.Scheme, launchSettings.Host, launchSettings.Port));
                var settings = new ConnectionSettings(new SingleNodeConnectionPool(uri))
                    .RequestTimeout(TimeSpan.FromMinutes(5))
                    .MaximumRetries(10)
                    .ThrowExceptions();

                if (_logger.IsEnabled(Microsoft.Extensions.Logging.LogLevel.Trace))
                {
                    settings.DisableDirectStreaming().PrettyJson().EnableDebugMode(r =>
                    {
                        //Log.Trace(r.DebugInformation);

                        //if (r.RequestBodyInBytes != null)
                        //{
                        //    Log.TraceFormat("Request: {0}", Encoding.UTF8.GetString(r.RequestBodyInBytes));
                        //}

                        if (r.HttpStatusCode != null && (r.HttpStatusCode == 403 || r.HttpStatusCode == 500) && r.ResponseBodyInBytes != null)
                        {
                            _logger.LogTrace("Response: {response}", Encoding.UTF8.GetString(r.ResponseBodyInBytes));
                        }
                    });
                }

                try
                {
                    if (Ping(new ElasticClient(settings)))
                    {
                        _client = new ElasticClient(settings);

                        _client.Ingest.PutPipeline("attachments", p => p
                        .Processors(pp => pp
                            .Attachment<Attachment>(a => a.Field("document.data").TargetField("document.attachment"))
                        ));
                    }

                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Client");
                }

                return _client;
            }
        }
    }

    private static volatile ElasticClient _client;
    private static readonly object _locker = new object();
    private readonly ILogger _logger;
    private readonly Settings _settings;
    private readonly IServiceProvider _serviceProvider;

    public Client(ILoggerProvider option, IServiceProvider serviceProvider, Settings settings)
    {
        _logger = option.CreateLogger("ASC.Indexer");
        _settings = settings;
        _serviceProvider = serviceProvider;
    }

    public bool Ping()
    {
        return Ping(Instance);
    }

    private bool Ping(ElasticClient elasticClient)
    {
        if (elasticClient == null)
        {
            return false;
        }

        var result = elasticClient.Ping(new PingRequest());

        _logger.LogDebug("Ping {ping}", result.DebugInformation);

        return result.IsValid;
    }
}
