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

namespace ASC.Files.Core;

public enum FolderType
{
    DEFAULT = 0,
    COMMON = 1,
    BUNCH = 2,
    TRASH = 3,
    USER = 5,
    SHARE = 6,
    Projects = 8,
    Favorites = 10,
    Recent = 11,
    Templates = 12,
    Privacy = 13,
}

public interface IFolder
{
    public FolderType FolderType { get; set; }
    public int TotalFiles { get; set; }
    public int TotalSubFolders { get; set; }
    public bool Shareable { get; set; }
    public int NewForMe { get; set; }
    public string FolderUrl { get; set; }
}

[DebuggerDisplay("{Title} ({ID})")]
[Transient]
public class Folder<T> : FileEntry<T>, IFolder
{
    public FolderType FolderType { get; set; }
    public int TotalFiles { get; set; }
    public int TotalSubFolders { get; set; }
    public bool Shareable { get; set; }
    public int NewForMe { get; set; }
    public string FolderUrl { get; set; }
    public override bool IsNew
    {
        get => Convert.ToBoolean(NewForMe);
        set => NewForMe = Convert.ToInt32(value);
    }

    public Folder()
    {
        Title = string.Empty;
        FileEntryType = FileEntryType.Folder;
    }

    public Folder(FileHelper fileHelper, Global global) : this()
    {
        FileHelper = fileHelper;
        Global = global;
    }

    public override string UniqID => $"folder_{ID}";
}
