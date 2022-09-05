import React from "react";
import styled from "styled-components";
import { inject } from "mobx-react";
import { withTranslation } from "react-i18next";

import { ContextMenuButton } from "@docspace/components";
import ContextHelper from "./Body/helpers/ContextHelper";

const StyledInfoPanelContextOptions = styled.div`
  margin-left: auto;
`;

const InfoPanelContextOptions = (props) => {
  const contextHelper = new ContextHelper(props);

  return (
    <StyledInfoPanelContextOptions>
      <ContextMenuButton
        zIndex={402}
        className="option-button"
        directionX="right"
        iconName="images/vertical-dots.react.svg"
        size={15}
        isFill
        getData={contextHelper.getItemContextOptions}
        isDisabled={false}
      />
    </StyledInfoPanelContextOptions>
  );
};

export default inject(({ filesStore, contextOptionsStore }) => {
  const { getFilesContextOptions: getContextOptions } = filesStore;
  const {
    getFilesContextOptions: getContextOptionActions,
  } = contextOptionsStore;

  return {
    getContextOptions,
    getContextOptionActions,
  };
})(
  withTranslation([
    "Files",
    "Common",
    "Translations",
    "InfoPanel",
    "SharingPanel",
  ])(InfoPanelContextOptions)
);
