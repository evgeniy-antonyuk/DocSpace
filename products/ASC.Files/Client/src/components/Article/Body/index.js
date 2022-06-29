import React from "react";
import styled from "styled-components";
import { inject, observer } from "mobx-react";
import { withRouter } from "react-router";
import { setDocumentTitle } from "../../../helpers/utils";
import config from "../../../../package.json";
import { AppServerConfig } from "@appserver/common/constants";
import Items from "./Items";
import { isMobile, tablet } from "@appserver/components/utils/device";
import FilesFilter from "@appserver/common/api/files/filter";
import SettingsItem from "./SettingsItem";
import AccountsItem from "./AccountsItem";
import { combineUrl } from "@appserver/common/utils";
import { isDesktop, isTablet, isMobileOnly } from "react-device-detect";
import ThirdPartyList from "./ThirdPartyList";
import DownloadAppList from "./DownloadAppList";
import Banner from "./Banner";
import { showLoader, hideLoader } from "@appserver/common/utils";
import Loaders from "@appserver/common/components/Loaders";
import withLoader from "../../../HOCs/withLoader";
import { withTranslation } from "react-i18next";
import { FolderType } from "@appserver/common/constants";
import RoomsFilter from "@appserver/common/api/rooms/filter";

const StyledBlock = styled.div`
  padding: 0 20px;

  @media ${tablet} {
    padding: ${(props) => (props.showText ? "0 16px" : 0)};
  }
`;

const ArticleBodyContent = (props) => {
  const {
    personal,
    docspace,
    firstLoad,
    showText,
    isDesktopClient,
    enableThirdParty,
    isVisitor,
    FirebaseHelper,
    theme,
  } = props;

  const campaigns = (localStorage.getItem("campaigns") || "")
    .split(",")
    .filter((campaign) => campaign.length > 0);

  const onClick = React.useCallback((data, type) => {
    const {
      toggleArticleOpen,
      setIsLoading,
      fetchFiles,
      fetchRooms,
      homepage,
      history,
    } = props;

    const filesSection = window.location.pathname.indexOf("/filter") > 0;

    if (filesSection) {
      setIsLoading(true);
    } else {
      showLoader();
    }

    if (type === FolderType.Rooms || type === FolderType.Archive) {
      fetchRooms(data, null, true, false)
        .then(() => {
          if (filesSection) {
            const filter = RoomsFilter.getDefault();

            const urlFilter = filter.toUrlParams();

            history.push(
              combineUrl(
                AppServerConfig.proxyURL,
                homepage,
                `/rooms?${urlFilter}`
              )
            );
          }
        })
        .finally(() => {
          if (isMobileOnly || isMobile()) {
            toggleArticleOpen();
          }
          if (filesSection) {
            setIsLoading(false);
          } else {
            hideLoader();
          }
        });
    } else {
      fetchFiles(data, null, true, false)
        .then(() => {
          if (!filesSection) {
            const filter = FilesFilter.getDefault();

            filter.folder = data;

            const urlFilter = filter.toUrlParams();

            history.push(
              combineUrl(
                AppServerConfig.proxyURL,
                homepage,
                `/filter?${urlFilter}`
              )
            );
          }
        })
        .catch((err) => toastr.error(err))
        .finally(() => {
          if (isMobileOnly || isMobile()) {
            toggleArticleOpen();
          }
          if (filesSection) {
            setIsLoading(false);
          } else {
            hideLoader();
          }
        });
    }
  }, []);

  const onShowNewFilesPanel = React.useCallback((folderId) => {
    props.setNewFilesPanelVisible(true, [`${folderId}`]);
  }, []);

  return (
    <>
      <Items
        onClick={onClick}
        onBadgeClick={onShowNewFilesPanel}
        showText={showText}
      />
      <AccountsItem />
      {!personal && !firstLoad && <SettingsItem />}
      {!isDesktopClient && showText && !docspace && (
        <StyledBlock showText={showText}>
          {enableThirdParty && !isVisitor && <ThirdPartyList />}
          <DownloadAppList theme={theme} />
          {(isDesktop || isTablet) &&
            personal &&
            !firstLoad &&
            campaigns.length > 0 && <Banner FirebaseHelper={FirebaseHelper} />}
        </StyledBlock>
      )}
    </>
  );
};

export default inject(
  ({
    auth,
    filesStore,
    roomsStore,
    treeFoldersStore,
    selectedFolderStore,
    dialogsStore,
    settingsStore,
  }) => {
    const {
      fetchFiles,
      setIsLoading,
      setFirstLoad,
      firstLoad,
      isLoading,
      isLoaded,
    } = filesStore;

    const { fetchRooms } = roomsStore;

    const { treeFolders, setTreeFolders } = treeFoldersStore;

    const { setNewFilesPanelVisible } = dialogsStore;
    const isArticleLoading = (!isLoaded || isLoading) && firstLoad;
    const {
      showText,
      articleOpen,

      toggleArticleOpen,

      personal,
      docspace,

      isDesktopClient,
      FirebaseHelper,
      theme,
    } = auth.settingsStore;

    const selectedFolderTitle = selectedFolderStore.title;

    selectedFolderTitle
      ? setDocumentTitle(selectedFolderTitle)
      : setDocumentTitle();

    return {
      toggleArticleOpen,
      treeFolders,
      showText,
      articleOpen,
      enableThirdParty: settingsStore.enableThirdParty,
      isVisitor: auth.userStore.user.isVisitor,
      homepage: config.homepage,

      fetchRooms,

      personal,
      docspace,

      isArticleLoading,
      setIsLoading,
      setFirstLoad,
      fetchFiles,

      setTreeFolders,
      setNewFilesPanelVisible,
      firstLoad,
      isDesktopClient,
      FirebaseHelper,
      theme,
    };
  }
)(
  withRouter(
    withTranslation([])(
      withLoader(observer(ArticleBodyContent))(<Loaders.ArticleFolder />)
    )
  )
);
