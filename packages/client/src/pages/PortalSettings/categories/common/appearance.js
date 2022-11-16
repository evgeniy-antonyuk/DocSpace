import React, { useState, useEffect, useCallback, useMemo } from "react";
import { withTranslation } from "react-i18next";
import { withRouter } from "react-router";
import toastr from "@docspace/components/toast/toastr";
import { inject, observer } from "mobx-react";
import Button from "@docspace/components/button";
import Tooltip from "@docspace/components/tooltip";
import Text from "@docspace/components/text";
import TabContainer from "@docspace/components/tabs-container";
import Preview from "./Appearance/preview";

import ColorSchemeDialog from "./sub-components/colorSchemeDialog";

import DropDownItem from "@docspace/components/drop-down-item";
import DropDownContainer from "@docspace/components/drop-down";

import HexColorPickerComponent from "./sub-components/hexColorPicker";
import { isMobileOnly } from "react-device-detect";

import Loader from "./sub-components/loaderAppearance";

import { StyledComponent, StyledTheme } from "./Appearance/StyledApperance.js";
import { ReactSVG } from "react-svg";
import BreakpointWarning from "../../../../components/BreakpointWarning/index";
import ModalDialogDelete from "./sub-components/modalDialogDelete";
import hexToRgba from "hex-to-rgba";

const Appearance = (props) => {
  const {
    appearanceTheme,
    selectedThemeId,
    sendAppearanceTheme,
    getAppearanceTheme,
    currentColorScheme,
    theme,
    deleteAppearanceTheme,
    tReady,
    t,
  } = props;

  const [previewTheme, setPreviewTheme] = useState("Light theme");

  const [showColorSchemeDialog, setShowColorSchemeDialog] = useState(false);

  const [headerColorSchemeDialog, setHeaderColorSchemeDialog] = useState(
    "Edit color scheme"
  );

  const [currentColorAccent, setCurrentColorAccent] = useState(null);
  const [currentColorButtons, setCurrentColorButtons] = useState(null);

  const [openHexColorPickerAccent, setOpenHexColorPickerAccent] = useState(
    false
  );
  const [openHexColorPickerButtons, setOpenHexColorPickerButtons] = useState(
    false
  );

  const [appliedColorAccent, setAppliedColorAccent] = useState("#AABBCC");
  const [appliedColorButtons, setAppliedColorButtons] = useState("#AABBCC");

  const [changeCurrentColorAccent, setChangeCurrentColorAccent] = useState(
    false
  );
  const [changeCurrentColorButtons, setChangeCurrentColorButtons] = useState(
    false
  );

  const [viewMobile, setViewMobile] = useState(false);

  const [showSaveButtonDialog, setShowSaveButtonDialog] = useState(false);

  const [isEditDialog, setIsEditDialog] = useState(false);
  const [isAddThemeDialog, setIsAddThemeDialog] = useState(false);

  const [previewAccent, setPreviewAccent] = useState(
    currentColorScheme.main.accent
  );

  const [colorCheckImg, setColorCheckImg] = useState(
    currentColorScheme.text.accent
  );
  const [colorCheckImgHover, setColorCheckImgHover] = useState(
    currentColorScheme.text.accent
  );

  const [selectThemeId, setSelectThemeId] = useState(selectedThemeId);

  const [isDisabledSaveButton, setIsDisabledSaveButton] = useState(true);

  const [abilityAddTheme, setAbilityAddTheme] = useState(true);

  const [isDisabledEditButton, setIsDisabledEditButton] = useState(true);
  const [isDisabledDeleteButton, setIsDisabledDeleteButton] = useState(true);
  const [isShowDeleteButton, setIsShowDeleteButton] = useState(false);

  const [visibleDialog, setVisibleDialog] = useState(false);

  const checkImg = (
    <ReactSVG className="check-img" src="static/images/check.white.svg" />
  );

  const array_items = useMemo(
    () => [
      {
        key: "0",
        title: t("Profile:LightTheme"),
        content: (
          <Preview
            previewTheme={previewTheme}
            previewAccent={previewAccent}
            selectThemeId={selectThemeId}
            themePreview="Light"
          />
        ),
      },
      {
        key: "1",
        title: t("Profile:DarkTheme"),
        content: (
          <Preview
            previewTheme={previewTheme}
            previewAccent={previewAccent}
            selectThemeId={selectThemeId}
            themePreview="Dark"
          />
        ),
      },
    ],
    [previewAccent, previewTheme, selectThemeId, tReady]
  );

  useEffect(() => {
    if (appearanceTheme.length === 11) {
      setAbilityAddTheme(false);
    } else {
      setAbilityAddTheme(true);
    }

    if (appearanceTheme.length === 8) {
      setIsShowDeleteButton(false);
    } else {
      setIsShowDeleteButton(true);
    }
  }, [appearanceTheme.length, setAbilityAddTheme, setIsShowDeleteButton]);

  useEffect(() => {
    onCheckView();
    window.addEventListener("resize", onCheckView);

    const standard = document.querySelector(".standard");
    const custom = document.querySelector(".custom");

    standard.addEventListener("mouseover", (e) => {
      onColorCheckImgHover(e.target.id);
    });

    custom.addEventListener("mouseover", (e) => {
      onColorCheckImgHover(e.target.id);
    });

    return () => window.removeEventListener("resize", onCheckView);
  }, []);

  useEffect(() => {
    const colorCheckImg = appearanceTheme.find(
      (theme) => theme.id == selectThemeId
    ).text.accent;

    setColorCheckImg(colorCheckImg);

    if (appearanceTheme.find((theme) => theme.id == selectThemeId).name) {
      setIsDisabledEditButton(true);
      setIsDisabledDeleteButton(true);
      return;
    }

    setIsDisabledEditButton(false);
    setIsDisabledDeleteButton(false);
  }, [selectThemeId]);

  useEffect(() => {
    if (selectThemeId === selectedThemeId) {
      setIsDisabledSaveButton(true);
    } else {
      setIsDisabledSaveButton(false);
    }

    if (
      changeCurrentColorAccent &&
      changeCurrentColorButtons &&
      isAddThemeDialog
    ) {
      setShowSaveButtonDialog(true);
    }

    if (
      (changeCurrentColorAccent || changeCurrentColorButtons) &&
      isEditDialog
    ) {
      setShowSaveButtonDialog(true);
    }

    if (
      !changeCurrentColorAccent &&
      !changeCurrentColorButtons &&
      isEditDialog
    ) {
      setShowSaveButtonDialog(false);
    }
  }, [
    selectedThemeId,
    selectThemeId,
    changeCurrentColorAccent,
    changeCurrentColorButtons,
    isAddThemeDialog,
    isEditDialog,
    previewAccent,
  ]);

  const onColorCheckImgHover = useCallback(
    (id) => {
      if (!id) return;

      const colorCheckImg = appearanceTheme.find((theme) => theme.id == id).text
        .accent;

      setColorCheckImgHover(colorCheckImg);
    },
    [appearanceTheme]
  );

  const onCheckView = () => {
    if (isMobileOnly || window.innerWidth < 600) {
      setViewMobile(true);
    } else {
      setViewMobile(false);
    }
  };

  const onColorSelection = (item) => {
    setPreviewAccent(item.main.accent);
    setSelectThemeId(item.id);
  };

  const onShowCheck = useCallback(
    (colorNumber) => {
      return selectThemeId && selectThemeId === colorNumber && checkImg;
    },
    [selectThemeId, checkImg]
  );

  const onChangePreviewTheme = (e) => {
    setPreviewTheme(e.title);
  };

  const onSave = useCallback(async () => {
    setIsDisabledSaveButton(true);

    if (!selectThemeId) return;

    try {
      await sendAppearanceTheme({ selected: selectThemeId });
      await getAppearanceTheme();

      toastr.success(t("Settings:SuccessfullySaveSettingsMessage"));
    } catch (error) {
      toastr.error(error);
    }
  }, [
    selectThemeId,
    setIsDisabledSaveButton,
    sendAppearanceTheme,
    getAppearanceTheme,
  ]);

  // Open HexColorPicker
  const onClickColor = (e) => {
    if (e.target.id === "accent") {
      setOpenHexColorPickerAccent(true);
      setOpenHexColorPickerButtons(false);
    } else {
      setOpenHexColorPickerButtons(true);
      setOpenHexColorPickerAccent(false);
    }
  };

  const onClickDeleteModal = useCallback(async () => {
    try {
      await deleteAppearanceTheme(selectThemeId);
      await getAppearanceTheme();

      if (selectedThemeId !== selectThemeId) {
        setSelectThemeId(selectedThemeId);
        setPreviewAccent(currentColorScheme.main.accent);
      }

      if (selectedThemeId === selectThemeId) {
        setSelectThemeId(appearanceTheme[0].id);
        setPreviewAccent(appearanceTheme[0].main.accent);
      }

      setVisibleDialog(false);

      toastr.success(t("Settings:SuccessfullySaveSettingsMessage"));
    } catch (error) {
      toastr.error(error);
    }
  }, [
    selectThemeId,
    setVisibleDialog,
    deleteAppearanceTheme,
    getAppearanceTheme,
  ]);

  const onCloseColorSchemeDialog = () => {
    setShowColorSchemeDialog(false);

    setOpenHexColorPickerAccent(false);
    setOpenHexColorPickerButtons(false);

    setChangeCurrentColorAccent(false);
    setChangeCurrentColorButtons(false);

    setIsEditDialog(false);
    setIsAddThemeDialog(false);

    setShowSaveButtonDialog(false);

    setCurrentColorAccent(null);
    setCurrentColorButtons(null);

    setAppliedColorAccent("#AABBCC");
    setAppliedColorButtons("#AABBCC");
  };

  const onAddTheme = () => {
    if (!abilityAddTheme) return;
    setIsAddThemeDialog(true);

    setHeaderColorSchemeDialog("New color scheme");

    setShowColorSchemeDialog(true);
  };

  const onClickEdit = () => {
    appearanceTheme.map((item) => {
      if (item.id === selectThemeId) {
        setCurrentColorAccent(item.main.accent.toUpperCase());
        setCurrentColorButtons(item.main.buttons.toUpperCase());

        setAppliedColorAccent(item.main.accent.toUpperCase());
        setAppliedColorButtons(item.main.buttons.toUpperCase());
      }
    });

    setIsEditDialog(true);

    setHeaderColorSchemeDialog("Edit color scheme");

    setShowColorSchemeDialog(true);
  };

  const onCloseHexColorPickerAccent = useCallback(() => {
    setOpenHexColorPickerAccent(false);
    if (!currentColorAccent) return;
    setAppliedColorAccent(currentColorAccent);
  }, [currentColorAccent, setOpenHexColorPickerAccent, setAppliedColorAccent]);

  const onCloseHexColorPickerButtons = useCallback(() => {
    setOpenHexColorPickerButtons(false);
    if (!currentColorButtons) return;
    setAppliedColorButtons(currentColorButtons);
  }, [
    currentColorButtons,
    setOpenHexColorPickerButtons,
    setAppliedColorButtons,
  ]);

  const getTextColor = (color) => {
    const black = "#333333";
    const white = "#FFFFFF";

    const rgba = hexToRgba(color)
      .replace("rgba(", "")
      .replace(")", "")
      .split(", ");

    const r = rgba[0];
    const g = rgba[1];
    const b = rgba[2];

    const textColor =
      (r * 299 + g * 587 + b * 114) / 1000 > 128 ? black : white;

    return textColor;
  };

  const onAppliedColorAccent = useCallback(() => {
    if (appliedColorAccent.toUpperCase() !== currentColorAccent) {
      setChangeCurrentColorAccent(true);
    }

    setCurrentColorAccent(appliedColorAccent);

    setOpenHexColorPickerAccent(false);
  }, [
    appliedColorAccent,
    currentColorAccent,
    setChangeCurrentColorAccent,
    setOpenHexColorPickerAccent,
  ]);

  const onAppliedColorButtons = useCallback(() => {
    if (appliedColorButtons.toUpperCase() !== currentColorButtons) {
      setChangeCurrentColorButtons(true);
    }

    setCurrentColorButtons(appliedColorButtons);

    setOpenHexColorPickerButtons(false);
  }, [
    appliedColorButtons,
    currentColorButtons,
    setChangeCurrentColorButtons,
    setOpenHexColorPickerButtons,
  ]);

  const onSaveNewThemes = useCallback(
    async (theme) => {
      try {
        await sendAppearanceTheme({ themes: [theme] });
        await getAppearanceTheme();

        toastr.success(t("Settings:SuccessfullySaveSettingsMessage"));
      } catch (error) {
        toastr.error(error);
      }
    },
    [sendAppearanceTheme, getAppearanceTheme]
  );

  const onSaveChangedThemes = useCallback(
    async (editTheme) => {
      try {
        await sendAppearanceTheme({ themes: [editTheme] });
        await getAppearanceTheme();
        setPreviewAccent(editTheme.main.accent);

        toastr.success(t("Settings:SuccessfullySaveSettingsMessage"));
      } catch (error) {
        toastr.error(error);
      }
    },
    [sendAppearanceTheme, getAppearanceTheme]
  );

  const onSaveColorSchemeDialog = () => {
    const textColorAccent = getTextColor(currentColorAccent);
    const textColorButtons = getTextColor(currentColorButtons);

    if (isAddThemeDialog) {
      // Saving a new custom theme
      const theme = {
        main: {
          accent: currentColorAccent,
          buttons: currentColorButtons,
        },
        text: {
          accent: textColorAccent,
          buttons: textColorButtons,
        },
      };

      onSaveNewThemes(theme);

      setCurrentColorAccent(null);
      setCurrentColorButtons(null);

      onCloseColorSchemeDialog();

      return;
    }

    // Editing themes
    const editTheme = {
      id: selectThemeId,
      main: {
        accent: currentColorAccent,
        buttons: currentColorButtons,
      },
      text: {
        accent: textColorAccent,
        buttons: textColorButtons,
      },
    };

    onSaveChangedThemes(editTheme);

    setCurrentColorAccent(appliedColorAccent);
    setCurrentColorButtons(appliedColorButtons);

    onCloseColorSchemeDialog();
  };

  const nodeHexColorPickerButtons = (
    <DropDownContainer
      directionX="right"
      manualY="62px"
      withBackdrop={false}
      isDefaultMode={false}
      open={openHexColorPickerButtons}
      clickOutsideAction={onCloseHexColorPickerButtons}
    >
      <DropDownItem className="drop-down-item-hex">
        <HexColorPickerComponent
          id="buttons-hex"
          onCloseHexColorPicker={onCloseHexColorPickerButtons}
          onAppliedColor={onAppliedColorButtons}
          color={appliedColorButtons}
          setColor={setAppliedColorButtons}
          viewMobile={viewMobile}
        />
      </DropDownItem>
    </DropDownContainer>
  );

  const nodeHexColorPickerAccent = (
    <DropDownContainer
      directionX="right"
      manualY="62px"
      withBackdrop={false}
      isDefaultMode={false}
      open={openHexColorPickerAccent}
      clickOutsideAction={onCloseHexColorPickerAccent}
      viewMobile={viewMobile}
    >
      <DropDownItem className="drop-down-item-hex">
        <HexColorPickerComponent
          id="accent-hex"
          onCloseHexColorPicker={onCloseHexColorPickerAccent}
          onAppliedColor={onAppliedColorAccent}
          color={appliedColorAccent}
          setColor={setAppliedColorAccent}
          viewMobile={viewMobile}
        />
      </DropDownItem>
    </DropDownContainer>
  );

  const textTooltip = () => {
    return (
      <Text fontSize="12px" noSelect>
        You can only create 3 custom themes. To create a new one, you must
        delete one of the previous themes.
      </Text>
    );
  };

  return viewMobile ? (
    <BreakpointWarning sectionName={t("Settings:Appearance")} />
  ) : !tReady ? (
    <Loader />
  ) : (
    <>
      <ModalDialogDelete
        visible={visibleDialog}
        onClose={() => setVisibleDialog(false)}
        onClickDelete={onClickDeleteModal}
      />

      <StyledComponent colorCheckImg={colorCheckImg}>
        <div className="header">{t("Common:Color")}</div>

        <div className="theme-standard">
          <div className="theme-name">{t("Common:Standard")}</div>

          <div className="theme-container standard">
            {appearanceTheme.map((item, index) => {
              if (!item.name) return;
              return (
                <StyledTheme
                  key={index}
                  id={item.id}
                  colorCheckImgHover={colorCheckImgHover}
                  style={{ background: item.main.accent }}
                  onClick={() => onColorSelection(item)}
                >
                  {onShowCheck(item.id)}

                  <ReactSVG
                    className="check-hover"
                    src="static/images/check.white.svg"
                  />
                </StyledTheme>
              );
            })}
          </div>
        </div>

        <div className="theme-custom">
          <div className="theme-name">Custom</div>

          <div className="theme-container">
            <div className="custom">
              {appearanceTheme.map((item, index) => {
                if (item.name) return;
                return (
                  <StyledTheme
                    key={index}
                    id={item.id}
                    style={{ background: item.main.accent }}
                    colorCheckImgHover={colorCheckImgHover}
                    onClick={() => onColorSelection(item)}
                  >
                    {onShowCheck(item.id)}
                    <ReactSVG
                      className="check-hover"
                      src="static/images/check.white.svg"
                    />
                  </StyledTheme>
                );
              })}
            </div>

            <div
              data-for="theme-add"
              data-tip="tooltip"
              className="box theme-add"
              onClick={() => onAddTheme()}
            />
            {!abilityAddTheme && (
              <Tooltip
                id="theme-add"
                offsetBottom={0}
                offsetRight={130}
                effect="solid"
                place="bottom"
                getContent={textTooltip}
                maxWidth="300px"
              />
            )}
          </div>
        </div>

        <ColorSchemeDialog
          onClickColor={onClickColor}
          currentColorAccent={currentColorAccent}
          currentColorButtons={currentColorButtons}
          nodeHexColorPickerAccent={nodeHexColorPickerAccent}
          nodeHexColorPickerButtons={nodeHexColorPickerButtons}
          visible={showColorSchemeDialog}
          onClose={onCloseColorSchemeDialog}
          header={headerColorSchemeDialog}
          viewMobile={viewMobile}
          openHexColorPickerButtons={openHexColorPickerButtons}
          openHexColorPickerAccent={openHexColorPickerAccent}
          showSaveButtonDialog={showSaveButtonDialog}
          onSaveColorSchemeDialog={onSaveColorSchemeDialog}
        />
        <div className="header preview-header">{t("Common:Preview")}</div>
        <TabContainer elements={array_items} onSelect={onChangePreviewTheme} />

        <div className="buttons-container">
          <Button
            className="button"
            label="Save"
            onClick={onSave}
            primary
            size="small"
            isDisabled={isDisabledSaveButton}
          />

          <Button
            className="button"
            label="Edit current theme"
            onClick={onClickEdit}
            size="small"
            isDisabled={isDisabledEditButton}
          />
          {isShowDeleteButton && (
            <Button
              className="button"
              label="Delete theme"
              onClick={() => setVisibleDialog(true)}
              size="small"
              isDisabled={isDisabledDeleteButton}
            />
          )}
        </div>
      </StyledComponent>
    </>
  );
};

export default inject(({ auth }) => {
  const { settingsStore } = auth;
  const {
    appearanceTheme,
    selectedThemeId,
    sendAppearanceTheme,
    getAppearanceTheme,
    currentColorScheme,
    deleteAppearanceTheme,
    theme,
  } = settingsStore;

  return {
    appearanceTheme,
    selectedThemeId,
    sendAppearanceTheme,
    getAppearanceTheme,
    currentColorScheme,
    deleteAppearanceTheme,
    theme,
  };
})(
  withTranslation(["Profile", "Common", "Settings"])(
    withRouter(observer(Appearance))
  )
);
