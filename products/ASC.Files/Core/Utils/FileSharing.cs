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

namespace ASC.Web.Files.Utils;

[Scope]
public class FileSharingAceHelper<T>
{
    private readonly FileSecurity _fileSecurity;
    private readonly CoreBaseSettings _coreBaseSettings;
    private readonly FileUtility _fileUtility;
    private readonly UserManager _userManager;
    private readonly AuthContext _authContext;
    private readonly DocumentServiceHelper _documentServiceHelper;
    private readonly FileMarker _fileMarker;
    private readonly NotifyClient _notifyClient;
    private readonly GlobalFolderHelper _globalFolderHelper;
    private readonly FileSharingHelper _fileSharingHelper;
    private readonly FileTrackerHelper _fileTracker;

    public FileSharingAceHelper(
        FileSecurity fileSecurity,
        CoreBaseSettings coreBaseSettings,
        FileUtility fileUtility,
        UserManager userManager,
        AuthContext authContext,
        DocumentServiceHelper documentServiceHelper,
        FileMarker fileMarker,
        NotifyClient notifyClient,
        GlobalFolderHelper globalFolderHelper,
        FileSharingHelper fileSharingHelper,
        FileTrackerHelper fileTracker)
    {
        _fileSecurity = fileSecurity;
        _coreBaseSettings = coreBaseSettings;
        _fileUtility = fileUtility;
        _userManager = userManager;
        _authContext = authContext;
        _documentServiceHelper = documentServiceHelper;
        _fileMarker = fileMarker;
        _notifyClient = notifyClient;
        _globalFolderHelper = globalFolderHelper;
        _fileSharingHelper = fileSharingHelper;
        _fileTracker = fileTracker;
    }

    public async Task<bool> SetAceObjectAsync(List<AceWrapper> aceWrappers, FileEntry<T> entry, bool notify, string message)
    {
        if (entry == null)
        {
            throw new ArgumentNullException(FilesCommonResource.ErrorMassage_BadRequest);
        }

        if (!await _fileSharingHelper.CanSetAccessAsync(entry))
        {
            throw new SecurityException(FilesCommonResource.ErrorMassage_SecurityException);
        }

        var fileSecurity = _fileSecurity;

        var entryType = entry.FileEntryType;
        var recipients = new Dictionary<Guid, FileShare>();
        var usersWithoutRight = new List<Guid>();
        var changed = false;

        foreach (var w in aceWrappers.OrderByDescending(ace => ace.SubjectGroup))
        {
            var subjects = fileSecurity.GetUserSubjects(w.SubjectId);

            var ownerId = entry.RootFolderType == FolderType.USER ? entry.RootCreateBy : entry.CreateBy;
            if (entry.RootFolderType == FolderType.COMMON && subjects.Contains(Constants.GroupAdmin.ID)
                || ownerId == w.SubjectId)
            {
                continue;
            }

            var share = w.Share;

            if (w.SubjectId == FileConstant.ShareLinkId)
            {
                if (w.Share == FileShare.ReadWrite && _userManager.GetUsers(_authContext.CurrentAccount.ID).IsVisitor(_userManager))
                {
                    throw new SecurityException(FilesCommonResource.ErrorMassage_SecurityException);
                }

                if (_coreBaseSettings.Personal && !_fileUtility.CanWebView(entry.Title) && w.Share != FileShare.Restrict)
                {
                    throw new SecurityException(FilesCommonResource.ErrorMassage_BadRequest);
                }

                share = w.Share == FileShare.Restrict ? FileShare.None : w.Share;
            }

            await fileSecurity.ShareAsync(entry.Id, entryType, w.SubjectId, share);
            changed = true;

            if (w.SubjectId == FileConstant.ShareLinkId)
            {
                continue;
            }

            entry.Access = share;

            var listUsersId = new List<Guid>();

            if (w.SubjectGroup)
            {
                listUsersId = _userManager.GetUsersByGroup(w.SubjectId).Select(ui => ui.Id).ToList();
            }
            else
            {
                listUsersId.Add(w.SubjectId);
            }

            listUsersId.Remove(_authContext.CurrentAccount.ID);

            if (entryType == FileEntryType.File)
            {
                listUsersId.ForEach(uid => _fileTracker.ChangeRight(entry.Id, uid, true));
            }

            var addRecipient = share == FileShare.Read
                               || share == FileShare.CustomFilter
                               || share == FileShare.ReadWrite
                               || share == FileShare.Review
                               || share == FileShare.FillForms
                               || share == FileShare.Comment
                               || share == FileShare.None && entry.RootFolderType == FolderType.COMMON;
            var removeNew = share == FileShare.None && entry.RootFolderType == FolderType.USER
                            || share == FileShare.Restrict;
            listUsersId.ForEach(id =>
            {
                recipients.Remove(id);
                if (addRecipient)
                {
                    recipients.Add(id, share);
                }
                else if (removeNew)
                {
                    usersWithoutRight.Add(id);
                }
            });
        }

        if (entryType == FileEntryType.File)
        {
            await _documentServiceHelper.CheckUsersForDropAsync((File<T>)entry);
        }

        if (recipients.Count > 0)
        {
            if (entryType == FileEntryType.File
                || ((Folder<T>)entry).FoldersCount + ((Folder<T>)entry).FilesCount > 0
                || entry.ProviderEntry)
            {
                await _fileMarker.MarkAsNewAsync(entry, recipients.Keys.ToList());
            }

            if ((entry.RootFolderType == FolderType.USER
               || entry.RootFolderType == FolderType.Privacy)
               && notify)
            {
                await _notifyClient.SendShareNoticeAsync(entry, recipients, message);
            }
        }

        foreach (var userId in usersWithoutRight)
        {
            await _fileMarker.RemoveMarkAsNewAsync(entry, userId);
        }

        return changed;
    }

    public Task RemoveAceAsync(IAsyncEnumerable<FileEntry<T>> entries)
    {
        var fileSecurity = _fileSecurity;

        return entries.ForEachAwaitAsync(
            async entry =>
            {
                if (entry.RootFolderType != FolderType.USER && entry.RootFolderType != FolderType.Privacy
                        || Equals(entry.RootId, _globalFolderHelper.FolderMy)
                        || Equals(entry.RootId, await _globalFolderHelper.FolderPrivacyAsync))
                {
                    return;
                }

                var entryType = entry.FileEntryType;
                await fileSecurity.ShareAsync(entry.Id, entryType, _authContext.CurrentAccount.ID,
                     entry.RootFolderType == FolderType.USER
                        ? fileSecurity.DefaultMyShare
                        : fileSecurity.DefaultPrivacyShare);

                if (entryType == FileEntryType.File)
                {
                    await _documentServiceHelper.CheckUsersForDropAsync((File<T>)entry);
                }

                await _fileMarker.RemoveMarkAsNewAsync(entry);
            });
    }
}

[Scope]
public class FileSharingHelper
{
    public FileSharingHelper(
        Global global,
        GlobalFolderHelper globalFolderHelper,
        FileSecurity fileSecurity,
        AuthContext authContext,
        UserManager userManager)
    {
        _global = global;
        _globalFolderHelper = globalFolderHelper;
        _fileSecurity = fileSecurity;
        _authContext = authContext;
        _userManager = userManager;
    }

    private readonly Global _global;
    private readonly GlobalFolderHelper _globalFolderHelper;
    private readonly FileSecurity _fileSecurity;
    private readonly AuthContext _authContext;
    private readonly UserManager _userManager;

    public async Task<bool> CanSetAccessAsync<T>(FileEntry<T> entry)
    {
        return
            entry != null
            && (entry.RootFolderType == FolderType.COMMON && _global.IsAdministrator
                || !_userManager.GetUsers(_authContext.CurrentAccount.ID).IsVisitor(_userManager)
                    && (entry.RootFolderType == FolderType.USER
                        && (Equals(entry.RootId, _globalFolderHelper.FolderMy) || await _fileSecurity.CanEditAsync(entry))
                        || entry.RootFolderType == FolderType.Privacy
                            && entry is File<T>
                            && (Equals(entry.RootId, await _globalFolderHelper.FolderPrivacyAsync) || await _fileSecurity.CanEditAsync(entry))));
    }
}

[Scope]
public class FileSharing
{
    private readonly ILogger _logger;
    private readonly Global _global;
    private readonly FileSecurity _fileSecurity;
    private readonly AuthContext _authContext;
    private readonly UserManager _userManager;
    private readonly DisplayUserSettingsHelper _displayUserSettingsHelper;
    private readonly FileShareLink _fileShareLink;
    private readonly IDaoFactory _daoFactory;
    private readonly FileSharingHelper _fileSharingHelper;

    public FileSharing(
        Global global,
        FileSecurity fileSecurity,
        AuthContext authContext,
        UserManager userManager,
        ILogger<FileSharing> logger,
        DisplayUserSettingsHelper displayUserSettingsHelper,
        FileShareLink fileShareLink,
        IDaoFactory daoFactory,
        FileSharingHelper fileSharingHelper)
    {
        _global = global;
        _fileSecurity = fileSecurity;
        _authContext = authContext;
        _userManager = userManager;
        _displayUserSettingsHelper = displayUserSettingsHelper;
        _fileShareLink = fileShareLink;
        _daoFactory = daoFactory;
        _fileSharingHelper = fileSharingHelper;
        _logger = logger;
    }

    public Task<bool> CanSetAccessAsync<T>(FileEntry<T> entry)
    {
        return _fileSharingHelper.CanSetAccessAsync(entry);
    }

    public async Task<List<AceWrapper>> GetSharedInfoAsync<T>(FileEntry<T> entry)
    {
        if (entry == null)
        {
            throw new ArgumentNullException(FilesCommonResource.ErrorMassage_BadRequest);
        }

        if (!await CanSetAccessAsync(entry))
        {
            _logger.LogError("User {userId} can't get shared info for {fileEntryType} {entryId}", _authContext.CurrentAccount.ID, entry.FileEntryType == FileEntryType.File ? "file" : "folder", entry.Id);

            throw new SecurityException(FilesCommonResource.ErrorMassage_SecurityException);
        }

        var linkAccess = FileShare.Restrict;
        var result = new List<AceWrapper>();

        var fileSecurity = _fileSecurity;
        var shares = await fileSecurity.GetSharesAsync(entry);

        var records = shares
            .GroupBy(r => r.Subject)
            .Select(g => g.OrderBy(r => r.Level)
                          .ThenBy(r => r.Level)
                          .ThenByDescending(r => r.Share, new FileShareRecord.ShareComparer()).FirstOrDefault());

        foreach (var r in records)
        {
            if (r.Subject == FileConstant.ShareLinkId)
            {
                linkAccess = r.Share;
                continue;
            }

            var u = _userManager.GetUsers(r.Subject);
            var isgroup = false;
            var title = u.DisplayUserName(false, _displayUserSettingsHelper);
            var share = r.Share;

            if (u.Id == Constants.LostUser.Id)
            {
                var g = _userManager.GetGroupInfo(r.Subject);
                isgroup = true;
                title = g.Name;

                if (g.ID == Constants.GroupAdmin.ID)
                {
                    title = FilesCommonResource.Admin;
                }

                if (g.ID == Constants.GroupEveryone.ID)
                {
                    title = FilesCommonResource.Everyone;
                }

                if (g.ID == Constants.LostGroupInfo.ID)
                {
                    await fileSecurity.RemoveSubjectAsync<T>(r.Subject);

                    continue;
                }
            }
            else if (u.IsVisitor(_userManager) && new FileShareRecord.ShareComparer().Compare(FileShare.Read, share) > 0)
            {
                share = FileShare.Read;
            }

            var w = new AceWrapper
            {
                SubjectId = r.Subject,
                SubjectName = title,
                SubjectGroup = isgroup,
                Share = share,
                Owner =
                        entry.RootFolderType == FolderType.USER
                            ? entry.RootCreateBy == r.Subject
                            : entry.CreateBy == r.Subject,
                LockedRights = r.Subject == _authContext.CurrentAccount.ID
            };

            result.Add(w);
        }

        if (entry.FileEntryType == FileEntryType.File && result.All(w => w.SubjectId != FileConstant.ShareLinkId)
            && entry.FileEntryType == FileEntryType.File
            && !((File<T>)entry).Encrypted)
        {
            var w = new AceWrapper
            {
                SubjectId = FileConstant.ShareLinkId,
                Link = _fileShareLink.GetLink((File<T>)entry),
                SubjectGroup = true,
                Share = linkAccess,
                Owner = false
            };

            result.Add(w);
        }

        if (!result.Any(w => w.Owner))
        {
            var ownerId = entry.RootFolderType == FolderType.USER ? entry.RootCreateBy : entry.CreateBy;
            var w = new AceWrapper
            {
                SubjectId = ownerId,
                SubjectName = _global.GetUserName(ownerId),
                SubjectGroup = false,
                Share = FileShare.ReadWrite,
                Owner = true
            };

            result.Add(w);
        }

        if (result.Any(w => w.SubjectId == _authContext.CurrentAccount.ID))
        {
            result.Single(w => w.SubjectId == _authContext.CurrentAccount.ID).LockedRights = true;
        }

        if (entry.RootFolderType == FolderType.COMMON)
        {
            if (result.All(w => w.SubjectId != Constants.GroupAdmin.ID))
            {
                var w = new AceWrapper
                {
                    SubjectId = Constants.GroupAdmin.ID,
                    SubjectName = FilesCommonResource.Admin,
                    SubjectGroup = true,
                    Share = FileShare.ReadWrite,
                    Owner = false,
                    LockedRights = true,
                };

                result.Add(w);
            }
            if (result.All(w => w.SubjectId != Constants.GroupEveryone.ID))
            {
                var w = new AceWrapper
                {
                    SubjectId = Constants.GroupEveryone.ID,
                    SubjectName = FilesCommonResource.Everyone,
                    SubjectGroup = true,
                    Share = fileSecurity.DefaultCommonShare,
                    Owner = false,
                    DisableRemove = true
                };

                result.Add(w);
            }
        }

        return result;
    }

    public async Task<List<AceWrapper>> GetSharedInfoAsync<T>(IEnumerable<T> fileIds, IEnumerable<T> folderIds)
    {
        if (!_authContext.IsAuthenticated)
        {
            throw new InvalidOperationException(FilesCommonResource.ErrorMassage_SecurityException);
        }

        var result = new List<AceWrapper>();

        var fileDao = _daoFactory.GetFileDao<T>();
        var files = await fileDao.GetFilesAsync(fileIds).ToListAsync();

        var folderDao = _daoFactory.GetFolderDao<T>();
        var folders = await folderDao.GetFoldersAsync(folderIds).ToListAsync();

        var entries = files.Cast<FileEntry<T>>().Concat(folders.Cast<FileEntry<T>>());

        foreach (var entry in entries)
        {
            IEnumerable<AceWrapper> acesForObject;
            try
            {
                acesForObject = await GetSharedInfoAsync(entry);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "GetSharedInfo");

                throw new InvalidOperationException(e.Message, e);
            }

            foreach (var aceForObject in acesForObject)
            {
                var duplicate = result.FirstOrDefault(ace => ace.SubjectId == aceForObject.SubjectId);
                if (duplicate == null)
                {
                    if (result.Count > 0)
                    {
                        aceForObject.Owner = false;
                        aceForObject.Share = FileShare.Varies;
                    }

                    continue;
                }

                if (duplicate.Share != aceForObject.Share)
                {
                    aceForObject.Share = FileShare.Varies;
                }
                if (duplicate.Owner != aceForObject.Owner)
                {
                    aceForObject.Owner = false;
                    aceForObject.Share = FileShare.Varies;
                }

                result.Remove(duplicate);
            }

            var withoutAce = result.Where(ace =>
                                            acesForObject.FirstOrDefault(aceForObject =>
                                                                        aceForObject.SubjectId == ace.SubjectId) == null);
            foreach (var ace in withoutAce)
            {
                ace.Share = FileShare.Varies;
            }

            var notOwner = result.Where(ace =>
                                        ace.Owner &&
                                        acesForObject.FirstOrDefault(aceForObject =>
                                                                        aceForObject.Owner
                                                                        && aceForObject.SubjectId == ace.SubjectId) == null);
            foreach (var ace in notOwner)
            {
                ace.Owner = false;
                ace.Share = FileShare.Varies;
            }

            result.AddRange(acesForObject);
        }


        var ownerAce = result.FirstOrDefault(ace => ace.Owner);
        result.Remove(ownerAce);

        var meAce = result.FirstOrDefault(ace => ace.SubjectId == _authContext.CurrentAccount.ID);
        result.Remove(meAce);

        AceWrapper linkAce = null;
        if (entries.Count() > 1)
        {
            result.RemoveAll(ace => ace.SubjectId == FileConstant.ShareLinkId);
        }
        else
        {
            linkAce = result.FirstOrDefault(ace => ace.SubjectId == FileConstant.ShareLinkId);
        }

        result.Sort((x, y) => string.Compare(x.SubjectName, y.SubjectName));

        if (ownerAce != null)
        {
            result = new List<AceWrapper> { ownerAce }.Concat(result).ToList();
        }
        if (meAce != null)
        {
            result = new List<AceWrapper> { meAce }.Concat(result).ToList();
        }
        if (linkAce != null)
        {
            result.Remove(linkAce);
            result = new List<AceWrapper> { linkAce }.Concat(result).ToList();
        }

        return new List<AceWrapper>(result);
    }

    public async Task<List<AceShortWrapper>> GetSharedInfoShortFileAsync<T>(T fileID)
    {
        var aces = await GetSharedInfoAsync(new List<T> { fileID }, new List<T>());

        return GetAceShortWrappers(aces);
    }

    public async Task<List<AceShortWrapper>> GetSharedInfoShortFolderAsync<T>(T folderId)
    {
        var aces = await GetSharedInfoAsync(new List<T>(), new List<T> { folderId });

        return GetAceShortWrappers(aces);
    }

    private List<AceShortWrapper> GetAceShortWrappers(List<AceWrapper> aces)
    {
        return new List<AceShortWrapper>(aces
            .Where(aceWrapper => !aceWrapper.SubjectId.Equals(FileConstant.ShareLinkId) || aceWrapper.Share != FileShare.Restrict)
            .Select(aceWrapper => new AceShortWrapper(aceWrapper)));
    }
}
