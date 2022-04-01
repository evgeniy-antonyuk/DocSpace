import { FileType } from "@appserver/common/constants";
import { LANGUAGE } from "@appserver/common/constants";
import Link from "@appserver/components/link";
import Text from "@appserver/components/text";
import Tooltip from "@appserver/components/tooltip";
import React, { useEffect, useState } from "react";
import { ReactSVG } from "react-svg";
import {
  StyledAccess,
  StyledAccessItem,
  StyledOpenSharingPanel,
  StyledProperties,
  StyledSubtitle,
  StyledThumbnail,
  StyledTitle,
} from "./styles/styles.js";

const moment = require("moment");

const SingleItem = (props) => {
  const {
    t,
    selectedItem,
    onSelectItem,
    setSharingPanelVisible,
    getFolderInfo,
    getIcon,
    getFolderIcon,
    getShareUsers,
    dontShowSize,
    dontShowLocation,
    dontShowAccess,
  } = props;

  let updateSubscription = true;
  const [item, setItem] = useState({
    id: "",
    isFolder: false,
    title: "",
    iconUrl: "",
    thumbnailUrl: "",
    properties: [],
    access: {
      owner: {
        img: "",
        link: "",
      },
      others: [],
    },
  });

  const updateItemsInfo = async (selectedItem) => {
    const getItemIcon = (item, size) => {
      return item.isFolder
        ? getFolderIcon(item.providerKey, size)
        : getIcon(size, item.fileExst || ".file");
    };

    const getSingleItemProperties = (item) => {
      const styledLink = (text, href) => (
        <Link className="property-content" href={href} isHovered={true}>
          {text}
        </Link>
      );

      const styledText = (text) => (
        <Text className="property-content">{text}</Text>
      );

      const parseAndFormatDate = (date) => {
        return moment(date)
          .locale(localStorage.getItem(LANGUAGE))
          .format("DD.MM.YY hh:mm A");
      };

      const getItemType = (fileType) => {
        switch (fileType) {
          case FileType.Unknown:
            return t("Common:Unknown");
          case FileType.Archive:
            return t("Common:Archive");
          case FileType.Video:
            return t("Common:Video");
          case FileType.Audio:
            return t("Common:Audio");
          case FileType.Image:
            return t("Common:Image");
          case FileType.Spreadsheet:
            return t("Home:Spreadsheet");
          case FileType.Presentation:
            return t("Home:Presentation");
          case FileType.Document:
            return t("Home:Document");
          default:
            return t("Home:Folder");
        }
      };

      const itemSize = item.isFolder
        ? `${t("Translations:Folders")}: ${item.foldersCount} | ${t(
            "Translations:Files"
          )}: ${item.filesCount}`
        : item.contentLength;

      const itemType = getItemType(item.fileType);

      let result = [
        {
          id: "Owner",
          title: t("Common:Owner"),
          content: styledLink(
            item.createdBy?.displayName,
            item.createdBy?.profileUrl
          ),
        },
        // {
        //   id: "Location",
        //   title: t("InfoPanel:Location"),
        //   content: styledText("..."),
        // },
        {
          id: "Type",
          title: t("Common:Type"),
          content: styledText(itemType),
        },
        {
          id: "Size",
          title: t("Common:Size"),
          content: styledText(itemSize),
        },
        {
          id: "ByLastModifiedDate",
          title: t("Home:ByLastModifiedDate"),
          content: styledText(parseAndFormatDate(item.updated)),
        },
        {
          id: "LastModifiedBy",
          title: t("LastModifiedBy"),
          content: styledLink(
            item.updatedBy?.displayName,
            item.updatedBy?.profileUrl
          ),
        },
        {
          id: "ByCreationDate",
          title: t("Home:ByCreationDate"),
          content: styledText(parseAndFormatDate(item.created)),
        },
      ];

      if (item.isFolder) return result;

      result.splice(3, 0, {
        id: "FileExtension",
        title: t("FileExtension"),
        content: styledText(
          item.fileExst ? item.fileExst.split(".")[1].toUpperCase() : "-"
        ),
      });

      result.push(
        {
          id: "Versions",
          title: t("Versions"),
          content: styledText(item.version),
        },
        {
          id: "Comments",
          title: t("Comments"),
          content: styledText(item.comment),
        }
      );

      return result;
    };

    const displayedItem = {
      id: selectedItem.id,
      isFolder: selectedItem.isFolder,
      title: selectedItem.title,
      iconUrl: getItemIcon(selectedItem, 32),
      thumbnailUrl: selectedItem.thumbnailUrl || getItemIcon(selectedItem, 96),
      properties: getSingleItemProperties(selectedItem),
      access: {
        owner: {
          img: selectedItem.createdBy?.avatarSmall,
          link: selectedItem.createdBy?.profileUrl,
        },
        others: [],
      },
    };

    setItem(displayedItem);
    await loadAsyncData(displayedItem, selectedItem);
  };

  const loadAsyncData = async (displayedItem, selectedItem) => {
    if (!updateSubscription) return;

    const updateLoadedItemProperties = async (displayedItem, selectedItem) => {
      const parentFolderId = selectedItem.isFolder
        ? selectedItem.parentId
        : selectedItem.folderId;

      const noLocationProperties = [...displayedItem.properties].filter(
        (dip) => dip.id !== "Location"
      );

      let result;
      await getFolderInfo(parentFolderId)
        .catch(() => {
          result = noLocationProperties;
        })
        .then((data) => {
          if (!data) {
            result = noLocationProperties;
            return;
          }
          result = [...displayedItem.properties].map((dip) =>
            dip.id === "Location"
              ? {
                  id: "Location",
                  title: t("Location"),
                  content: (
                    <Link
                      className="property-content"
                      href={`/products/files/filter?folder=${parentFolderId}`}
                      isHovered={true}
                    >
                      {data.title}
                    </Link>
                  ),
                }
              : dip
          );
        });

      return result;
    };

    const updateLoadedItemAccess = async (selectedItem) => {
      const accesses = await getShareUsers(
        [selectedItem.isFolder ? selectedItem.parentId : selectedItem.folderId],
        [selectedItem.id]
      );

      const result = {
        owner: {},
        others: [],
      };

      accesses.forEach((access) => {
        let key = access.sharedTo.id,
          img = access.sharedTo.avatarSmall,
          link = access.sharedTo.profileUrl,
          name = access.sharedTo.displayName || access.sharedTo.name,
          { manager } = access.sharedTo;

        if (access.isOwner) result.owner = { key, img, link, name };
        else {
          if (access.sharedTo.email)
            result.others.push({ key, type: "user", img, link, name });
          else if (access.sharedTo.manager)
            result.others.push({ key, type: "group", name, manager });
        }
      });

      result.others = result.others.sort((a) => (a.type === "group" ? -1 : 1));
      return result;
    };

    // const properties = await updateLoadedItemProperties(
    //   displayedItem,
    //   selectedItem
    // );

    if (dontShowAccess) {
      setItem({
        ...displayedItem,
        properties: properties,
      });
      return;
    }

    const access = await updateLoadedItemAccess(selectedItem);
    setItem({
      ...displayedItem,
      // properties: properties,
      access: access,
    });
  };

  const openSharingPanel = () => {
    const { id, isFolder } = item;
    onSelectItem({ id, isFolder });
    setSharingPanelVisible(true);
  };

  useEffect(() => {
    if (selectedItem.id !== item.id && updateSubscription)
      updateItemsInfo(selectedItem);
    return () => (updateSubscription = false);
  }, [selectedItem]);

  return (
    <>
      <StyledTitle>
        <ReactSVG className="icon" src={item.iconUrl} />
        <Text className="text">{item.title}</Text>
      </StyledTitle>

      {selectedItem.thumbnailUrl ? (
        <StyledThumbnail>
          <img src={item.thumbnailUrl} alt="" />
        </StyledThumbnail>
      ) : (
        <div className="no-thumbnail-img-wrapper">
          <ReactSVG className="no-thumbnail-img" src={item.thumbnailUrl} />
        </div>
      )}

      <StyledSubtitle>
        <Text fontWeight="600" fontSize="14px">
          {t("SystemProperties")}
        </Text>
      </StyledSubtitle>

      <StyledProperties>
        {item.properties.map((p) => {
          if (dontShowSize && p.id === "Size") return;
          if (dontShowLocation && p.id === "Location") return;
          return (
            <div key={p.title} className="property">
              <Text className="property-title">{p.title}</Text>
              {p.content}
            </div>
          );
        })}
      </StyledProperties>

      {!dontShowAccess && item.access && (
        <>
          <StyledSubtitle>
            <Text fontWeight="600" fontSize="14px">
              {t("WhoHasAccess")}
            </Text>
          </StyledSubtitle>

          <StyledAccess>
            <Tooltip
              id="access-item-tooltip"
              getContent={(dataTip) =>
                dataTip ? <Text fontSize="13px">{dataTip}</Text> : null
              }
            />

            <StyledAccessItem>
              <div
                data-for="access-item-tooltip"
                className="access-item-tooltip"
                data-tip={item.access.owner.name}
              >
                <div className="item-user">
                  <a href={item.access.owner.link}>
                    <img src={item.access.owner.img} />
                  </a>
                </div>
              </div>
            </StyledAccessItem>

            {item.access.others.length > 0 && <div className="divider"></div>}

            {item.access.others.map((item, i) => {
              if (i < 3)
                return (
                  <div key={item.key}>
                    <StyledAccessItem>
                      <div
                        data-for="access-item-tooltip"
                        data-tip={item.name}
                        className="access-item-tooltip"
                      >
                        {item.type === "user" ? (
                          <div className="item-user">
                            <a href={item.link}>
                              <img src={item.img} />
                            </a>
                          </div>
                        ) : (
                          <div className="item-group">
                            <span>{item.name.substr(0, 2).toUpperCase()}</span>
                          </div>
                        )}
                      </div>
                    </StyledAccessItem>
                  </div>
                );
            })}

            {item.access.others.length > 3 && (
              <div className="show-more-users" onClick={openSharingPanel}>
                {`+ ${item.access.others.length - 3} ${t("Members")}`}
              </div>
            )}
          </StyledAccess>
          <StyledOpenSharingPanel onClick={openSharingPanel}>
            {t("OpenSharingSettings")}
          </StyledOpenSharingPanel>
        </>
      )}
    </>
  );
};

export default SingleItem;
