import styled from "styled-components";

import { tablet } from "../utils/device";
import Base from "../themes/base";

const StyledRow = styled.div`
  cursor: default;

  min-height: ${(props) => props.theme.row.minHeight};
  width: ${(props) => props.theme.row.width};
  border-bottom: ${(props) => props.theme.row.borderBottom};

  display: flex;
  flex-direction: row;
  flex-wrap: nowrap;

  justify-content: flex-start;
  align-items: center;
  align-content: center;
`;
StyledRow.defaultProps = { theme: Base };

const StyledContent = styled.div`
  display: flex;
  flex-basis: 100%;

  min-width: ${(props) => props.theme.row.minWidth};

  @media ${tablet} {
    white-space: nowrap;
    overflow: ${(props) => props.theme.row.overflow};
    text-overflow: ${(props) => props.theme.row.textOverflow};
  }
`;
StyledContent.defaultProps = { theme: Base };

const StyledCheckbox = styled.div`
  flex: 0 0 16px;
`;

const StyledElement = styled.div`
  flex: 0 0 auto;
  display: flex;
  margin-right: ${(props) => props.theme.row.element.marginRight};
  margin-left: ${(props) => props.theme.row.element.marginLeft};
  user-select: none;
`;
StyledElement.defaultProps = { theme: Base };

const StyledContentElement = styled.div`
  margin-top: 6px;
  user-select: none;
`;

const StyledOptionButton = styled.div`
  display: flex;
  width: ${(props) => props.spacerWidth && props.spacerWidth};
  justify-content: flex-end;

  .expandButton > div:first-child {
    padding: ${(props) => props.theme.row.optionButton.padding};

    margin-right: 0px;

    @media (min-width: 1024px) {
      margin-right: -1px;
    }
    @media (max-width: 516px) {
      padding-left: 10px;
    }
  }

  //margin-top: -1px;
  @media ${tablet} {
    margin-top: unset;
  }
`;
StyledOptionButton.defaultProps = { theme: Base };

export {
  StyledOptionButton,
  StyledContentElement,
  StyledElement,
  StyledCheckbox,
  StyledContent,
  StyledRow,
};
