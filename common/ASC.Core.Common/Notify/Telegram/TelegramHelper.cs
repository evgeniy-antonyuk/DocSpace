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

namespace ASC.Core.Common.Notify;

[Scope]
public class TelegramHelper
{
    public enum RegStatus
    {
        NotRegistered,
        Registered,
        AwaitingConfirmation
    }

    private readonly ConsumerFactory _consumerFactory;
    private readonly CachedTelegramDao _cachedTelegramDao;
    private readonly TelegramServiceClient _telegramServiceClient;
    private readonly ILogger _logger;

    public TelegramHelper(
        ConsumerFactory consumerFactory,
        IOptionsSnapshot<CachedTelegramDao> cachedTelegramDao,
        TelegramServiceClient telegramServiceClient,
        ILogger<TelegramHelper> logger)
    {
        _consumerFactory = consumerFactory;
        _cachedTelegramDao = cachedTelegramDao.Value;
        _telegramServiceClient = telegramServiceClient;
        _logger = logger;
    }

    public string RegisterUser(Guid userId, int tenantId)
    {
        var token = GenerateToken(userId);

        _telegramServiceClient.RegisterUser(userId.ToString(), tenantId, token);

        return GetLink(token);
    }

    public void SendMessage(NotifyMessage msg)
    {
        _telegramServiceClient.SendMessage(msg);
    }

    public bool CreateClient(int tenantId, string token, int tokenLifespan, string proxy)
    {
        var client = InitClient(token, proxy);
        if (TestingClient(client))
        {
            _telegramServiceClient.CreateOrUpdateClient(tenantId, token, tokenLifespan, proxy);

            return true;
        }
        else
        {
            return false;
        }
    }

    public RegStatus UserIsConnected(Guid userId, int tenantId)
    {
        if (_cachedTelegramDao.GetUser(userId, tenantId) != null)
        {
            return RegStatus.Registered;
        }

        return IsAwaitingRegistration(userId, tenantId) ? RegStatus.AwaitingConfirmation : RegStatus.NotRegistered;
    }

    public string CurrentRegistrationLink(Guid userId, int tenantId)
    {
        var token = GetCurrentToken(userId, tenantId);
        if (token == null || token.Length == 0)
        {
            return string.Empty;
        }

        return GetLink(token);
    }

    public void DisableClient(int tenantId)
    {
        _telegramServiceClient.DisableClient(tenantId);
    }

    public void Disconnect(Guid userId, int tenantId)
    {
        _cachedTelegramDao.Delete(userId, tenantId);
    }

    private bool IsAwaitingRegistration(Guid userId, int tenantId)
    {
        return GetCurrentToken(userId, tenantId) != null;
    }

    private string GetCurrentToken(Guid userId, int tenantId)
    {
        return _telegramServiceClient.RegistrationToken(userId.ToString(), tenantId);
    }

    private string GenerateToken(Guid userId)
    {
        var id = userId.ToByteArray();
        var d = BitConverter.GetBytes(DateTime.Now.Ticks);

        var buf = id.Concat(d).ToArray();

        using var sha = SHA256.Create();

        return Convert.ToBase64String(sha.ComputeHash(buf))
            .Replace('+', '-').Replace('/', '_').Replace("=", ""); // make base64 url safe
    }

    private string GetLink(string token)
    {
        var tgProvider = (ITelegramLoginProvider)_consumerFactory.GetByKey("telegram");
        var botname = tgProvider == null ? default : tgProvider.TelegramBotName;
        if (string.IsNullOrEmpty(botname))
        {
            return null;
        }

        return $"t.me/{botname}?start={token}";
    }

    public bool TestingClient(TelegramBotClient telegramBotClient)
    {
        try
        {
            if (!telegramBotClient.TestApiAsync().GetAwaiter().GetResult())
            {
                return false;
            }
        }
        catch (Exception e)
        {
            _logger.LogDebug(e, "Couldn't test api connection");

            return false;
        }

        return true;
    }

    public TelegramBotClient InitClient(string token, string proxy)
    {
        return string.IsNullOrEmpty(proxy) ? new TelegramBotClient(token) : new TelegramBotClient(token, new WebProxy(proxy));
    }
}
