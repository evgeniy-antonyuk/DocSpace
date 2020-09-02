import React from "react";
import { ModalDialog, Text, Button, TextInput, Box, Link } from "asc-web-components";
import { Trans } from 'react-i18next';

class ConsumerModalDialog extends React.Component {

    constructor(props) {
        super(props);
        this.state = {};
    }

    mapTokenNameToState = () => {
        const { consumers, selectedConsumer } = this.props;
        consumers
            .find((consumer) => consumer.name === selectedConsumer).props
            .map(p => this.setState(
                {
                    [`${p.name}`]: p.value
                }
            ))
    }

    onChangeHandler = e => {
        this.setState({
            [e.target.name]: e.target.value
        })
    }

    onSendValues = () => {
        this.props.onModalButtonClick();

        const prop = [];
        let i = 0;
        let stateLength = Object.keys(this.state).length;
        for (i = 0; i < stateLength; i++) {
            prop.push({
                name: Object.keys(this.state)[i],
                value: Object.values(this.state)[i]
            })
        }

        console.log([{
            name: this.props.selectedConsumer,
            props: prop
        }]);
    }

    componentDidMount() {
        this.mapTokenNameToState()
    }

    render() {

        const { consumers, selectedConsumer, onModalClose, dialogVisible, t, i18n } = this.props;
        const { onChangeHandler } = this;

        const bodyDescription = (
            <>
                <Text isBold={true}>{t("ThirdPartyHowItWorks")}</Text>
                <Text>{t("ThirdPartyBodyDescription")}</Text>
            </>
        );
        const supportTeamLink = (
            <>
                <Link
                    color="#316DAA"
                    isHovered={false}
                    target="_blank"
                    href="http://support.onlyoffice.com/ru">
                    Support Team
        </Link>
            </>
        );
        const bottomDescription = (
            <Trans i18nKey="ThirdPartyBottomDescription" i18n={i18n}>
                <Text>If you still have some questions on how to connect this service or need technical assistance, please feel free to contact our <Text isBold={true}>Support Team</Text></Text>
            </Trans>
        );

        const getConsumerName = () => {
            return consumers.find((consumer) => consumer.name === selectedConsumer).name;
        }
        const getInnerDescription = () => {
            return consumers.find((consumer) => consumer.name === selectedConsumer).instruction;
        }
        const getInputFields = () => {

            return consumers
                .find((consumer) => consumer.name === selectedConsumer)
                .props
                .map((prop, i) =>
                    <React.Fragment key={i}>
                        <Box displayProp="flex" flexDirection="column">
                            <Box>
                                <Text isBold={true}>{prop.title}:</Text>
                            </Box>
                            <Box>
                                <TextInput
                                    name={prop.name}
                                    placeholder={prop.title}
                                    isAutoFocussed={i === 0 && true}
                                    value={Object.values(this.state)[i]}
                                    onChange={onChangeHandler}
                                />
                            </Box>
                        </Box>
                    </React.Fragment>
                )
        }

        return (
            <ModalDialog
                visible={dialogVisible}
                headerContent={`${getConsumerName()}`}
                bodyContent={[
                    <Text>{getInnerDescription()}</Text>,
                    <Text>{bodyDescription}</Text>,
                    <React.Fragment>{getInputFields()}</React.Fragment>
                ]}
                footerContent={[
                    <Button
                        primary
                        size="medium"
                        label={t("ThirdPartyEnableButton")}
                        onClick={this.onSendValues} />,
                    <Text>{bottomDescription}</Text>
                ]}
                onClose={onModalClose}
            />
        )
    }
}

export default ConsumerModalDialog;