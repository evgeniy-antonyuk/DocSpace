import React from "react";
import Text from "@docspace/components/text";
import Link from "@docspace/components/link";
import TextInput from "@docspace/components/text-input";
import FieldContainer from "@docspace/components/field-container";

const LinkBlock = (props) => {
  const {
    t,
    isLoading,
    shareLink,
    linkNameValue,
    setLinkNameValue,
    linkValue,
    setLinkValue,
  } = props;

  const onChangeLinkName = (e) => {
    setLinkNameValue(e.target.value);
  };

  const onShortenClick = () => {
    alert("api in progress");
    // setLinkValue
  };

  return (
    <div className="edit-link_link-block">
      <Text className="edit-link-text" fontSize="13px" fontWeight={600}>
        {t("LinkName")}
      </Text>
      <Text className="edit-link_required-icon" color="#F24724">
        *
      </Text>

      <TextInput
        scale
        size="base"
        withBorder
        isAutoFocussed={false}
        className="edit-link_name-input"
        value={linkNameValue}
        onChange={onChangeLinkName}
        placeholder={t("ExternalLink")}
        isDisabled={isLoading}
      />

      <TextInput
        scale
        size="base"
        withBorder
        isDisabled
        isReadOnly
        className="edit-link_link-input"
        value={linkValue}
        placeholder={t("ExternalLink")}
      />

      <Link
        fontSize="13px"
        fontWeight={600}
        isHovered
        type="action"
        isDisabled={isLoading}
        onClick={onShortenClick}
      >
        {t("Shorten")}
      </Link>
    </div>
  );
};

export default LinkBlock;
