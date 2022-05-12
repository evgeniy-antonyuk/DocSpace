import React, { useState, useEffect, useRef } from "react";
import styled from "styled-components";
import commonIconsStyles from "@appserver/components/utils/common-icons-style";
import { PlusIcon, TrashIcon } from "./svg";
import Link from "@appserver/components/link";
import TextInput from "@appserver/components/text-input";
import { Base } from "@appserver/components/themes";

const StyledPlusIcon = styled(PlusIcon)`
  ${commonIconsStyles}

  path {
    fill: ${(props) => props.theme.studio.settings.iconFill};
  }
`;

StyledPlusIcon.defaultProps = { theme: Base };

const StyledTrashIcon = styled(TrashIcon)`
  ${commonIconsStyles}
  cursor: pointer;
`;

const StyledInputWrapper = styled.div`
  display: flex;
  flex-direction: row;
  gap: 10px;
  align-items: center;
  margin-bottom: 8px;
  width: 350px;

  @media (max-width: 375px) {
    width: 100%;
  }
`;

const StyledAddWrapper = styled.div`
  display: flex;
  flex-direction: row;
  gap: 6px;
  align-items: center;
  cursor: pointer;
`;

const usePrevious = (value) => {
  const ref = useRef();
  useEffect(() => {
    ref.current = value;
  }, [value]);
  return ref.current;
};

const UserFields = (props) => {
  const {
    className,
    buttonLabel,
    onChangeInput,
    onDeleteInput,
    onClickAdd,
    inputs,
    regexp,
  } = props;

  const [errors, setErrors] = useState(new Array(inputs.length).fill(false));
  const prevInputs = usePrevious(inputs.length);

  useEffect(() => {
    if (inputs.length > prevInputs) setErrors([...errors, false]);
  }, [inputs]);

  const onBlur = (index) => {
    let newErrors = Array.from(errors);
    newErrors[index] = true;
    setErrors(newErrors);
  };

  const onFocus = (index) => {
    let newErrors = Array.from(errors);
    newErrors[index] = false;
    setErrors(newErrors);
  };

  const onDelete = (index) => {
    let newErrors = Array.from(errors);
    newErrors.splice(index, 1);
    setErrors(newErrors);

    onDeleteInput(index);
  };

  return (
    <div className={className}>
      {inputs ? (
        inputs.map((input, index) => {
          const error = !regexp.test(input);

          return (
            <StyledInputWrapper key={`domain-input-${index}`}>
              <TextInput
                id={`domain-input-${index}`}
                isAutoFocussed={true}
                value={input}
                onChange={(e) => onChangeInput(e, index)}
                onBlur={() => onBlur(index)}
                onFocus={() => onFocus(index)}
                hasError={errors[index] && error}
              />
              <StyledTrashIcon size="medium" onClick={() => onDelete(index)} />
            </StyledInputWrapper>
          );
        })
      ) : (
        <></>
      )}

      <StyledAddWrapper onClick={onClickAdd}>
        <StyledPlusIcon size="small" />
        <Link type="action" isHovered={true}>
          {buttonLabel}
        </Link>
      </StyledAddWrapper>
    </div>
  );
};

export default UserFields;
