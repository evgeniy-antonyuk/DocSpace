import React from 'react';
import { mount, shallow } from 'enzyme';
import EmailInput from '.';
import { EmailSettings } from '../../utils/email/';


const baseProps = {
  id: 'emailInputId',
  name: 'emailInputName',
  value: '',
  size: 'base',
  scale: false,
  isDisabled: false,
  isReadOnly: false,
  maxLength: 255,
  placeholder: 'email',
  onChange: () => jest.fn(),
  onValidateInput: () => jest.fn()
}

describe('<EmailInput />', () => {
  it('renders without error', () => {
    const wrapper = mount(<EmailInput {...baseProps} />);

    expect(wrapper).toExist();
  });

  it('re-render test', () => {
    const wrapper = shallow(<EmailInput {...baseProps} />).instance();

    const shouldUpdate = wrapper.shouldComponentUpdate({
      id: 'newEmailInputId',
      name: 'emailInputName',
      value: '',
      size: 'base',
      scale: false,
      isDisabled: false,
      isReadOnly: false,
      maxLength: 255,
      placeholder: 'email',
      onValidateInput: () => jest.fn()
    }, wrapper.state);

    expect(shouldUpdate).toBe(true);
  });

  it('re-render after changing emailSettings prop', () => {

    const emailSettings = new EmailSettings();
    const wrapper = shallow(<EmailInput {...baseProps} emailSettings={emailSettings} />);
    const instance = wrapper.instance();

    emailSettings.allowName = true;
    const shouldUpdate = instance.shouldComponentUpdate({
      emailSettings
    }, wrapper.state);

    expect(shouldUpdate).toBe(true);
    expect(wrapper.state('emailSettings')).toBe(emailSettings);
  });
  it('isValidEmail is "true" after deleting value', () => {

    const wrapper = mount(<EmailInput {...baseProps} />);

    const event = { target: { value: "test" } };

    wrapper.simulate('change', event);

    expect(wrapper.state('isValidEmail')).toBe(false);

    const emptyValue = { target: { value: "" } };

    wrapper.simulate('change', emptyValue);

    expect(wrapper.state('isValidEmail')).toBe(true);
  });

  it('not re-render test', () => {
    const wrapper = shallow(<EmailInput {...baseProps} />).instance();

    const shouldUpdate = wrapper.shouldComponentUpdate(wrapper.props, wrapper.state);

    expect(shouldUpdate).toBe(false);
  });

  it('passed valid email: simple@example.com', () => {

    const onValidateInput = jest.fn(isValidEmail => {
      expect(isValidEmail).toEqual(true);
    });

    const wrapper = mount(<EmailInput {...baseProps} onValidateInput={onValidateInput} />);

    const event = { target: { value: "simple@example.com" } };

    wrapper.simulate('change', event);
  });
  it('passed valid email: disposable.style.email.with+symbol@example.com', () => {

    const onValidateInput = jest.fn(isValidEmail => {
      expect(isValidEmail).toEqual(true);
    });

    const wrapper = mount(<EmailInput {...baseProps} onValidateInput={onValidateInput} />);

    const event = { target: { value: "disposable.style.email.with+symbol@example.com" } };

    wrapper.simulate('change', event);
  });
  it('passed valid email: user.name+tag+sorting@example.com', () => {

    const onValidateInput = jest.fn(isValidEmail => {
      expect(isValidEmail).toEqual(true);
    });

    const wrapper = mount(<EmailInput {...baseProps} onValidateInput={onValidateInput} />);

    const event = { target: { value: "user.name+tag+sorting@example.com" } };

    wrapper.simulate('change', event);
  });
  it('passed valid email with one-letter local-part: x@example.com', () => {

    const onValidateInput = jest.fn(isValidEmail => {
      expect(isValidEmail).toEqual(true);
    });

    const wrapper = mount(<EmailInput {...baseProps} onValidateInput={onValidateInput} />);

    const event = { target: { value: "x@example.com" } };

    wrapper.simulate('change', event);
  });

  it('passed valid email, local domain name with no TLD: admin@mailserver1', () => {

    const onValidateInput = jest.fn(isValidEmail => {
      expect(isValidEmail).toEqual(false);
    });


    const wrapper = mount(<EmailInput {...baseProps} onValidateInput={onValidateInput} />);

    const event = { target: { value: "admin@mailserver1" } };

    wrapper.simulate('change', event);
  });
  it('passed valid email, local domain name with no TLD: admin@mailserver1 (settings: allowLocalDomainName = true)', () => {

    const onValidateInput = jest.fn(isValidEmail => {
      expect(isValidEmail).toEqual(true);
    });

    const emailSettings = new EmailSettings();
    emailSettings.allowLocalDomainName = true;
    const wrapper = mount(<EmailInput {...baseProps} onValidateInput={onValidateInput} emailSettings={emailSettings} />);

    const event = { target: { value: "admin@mailserver1" } };

    wrapper.simulate('change', event);
  });
  it('passed valid email (one-letter domain name): example@s.example', () => {

    const onValidateInput = jest.fn(isValidEmail => {
      expect(isValidEmail).toEqual(true);
    });

    const wrapper = mount(<EmailInput {...baseProps} onValidateInput={onValidateInput} />);

    const event = { target: { value: "example@s.example" } };

    wrapper.simulate('change', event);
  });

  it('passed valid email (space between the quotes): " "@example.org', () => {

    const onValidateInput = jest.fn(isValidEmail => {
      expect(isValidEmail).toEqual(false);
    });

    const wrapper = mount(<EmailInput {...baseProps} onValidateInput={onValidateInput} />);

    const event = { target: { value: '" "@example.org' } };

    wrapper.simulate('change', event);
  });
  it('passed valid email (space between the quotes): " "@example.org (settings: allowSpaces = true, allowStrictLocalPart = false)', () => {

    const onValidateInput = jest.fn(isValidEmail => {
      expect(isValidEmail).toEqual(true);
    });
    const emailSettings = new EmailSettings();
    emailSettings.allowSpaces = true;
    emailSettings.allowStrictLocalPart = false;

    const wrapper = mount(<EmailInput {...baseProps} onValidateInput={onValidateInput} emailSettings={emailSettings} />);

    const event = { target: { value: '" "@example.org' } };

    wrapper.simulate('change', event);
  });
  it('passed valid email (quoted double dot): "john..doe"@example.org)', () => {

    const onValidateInput = jest.fn(isValidEmail => {
      expect(isValidEmail).toEqual(false);
    });
    const wrapper = mount(<EmailInput {...baseProps} onValidateInput={onValidateInput} />);

    const event = { target: { value: '"john..doe"@example.org' } };

    wrapper.simulate('change', event);
  });

  it('passed valid email (quoted double dot): "john..doe"@example.org (settings: allowSpaces = true, allowStrictLocalPart = false)', () => {

    const onValidateInput = jest.fn(isValidEmail => {
      expect(isValidEmail).toEqual(true);
    });
    const emailSettings = new EmailSettings();
    emailSettings.allowSpaces = true;
    emailSettings.allowStrictLocalPart = false;
    const wrapper = mount(<EmailInput {...baseProps} onValidateInput={onValidateInput} emailSettings={emailSettings} />);

    const event = { target: { value: '"john..doe"@example.org' } };

    wrapper.simulate('change', event);
  });

  it('passed valid email (bangified host route used for uucp mailers): mailhost!username@example.org', () => {

    const onValidateInput = jest.fn(isValidEmail => {
      expect(isValidEmail).toEqual(false);
    });

    const wrapper = mount(<EmailInput {...baseProps} onValidateInput={onValidateInput} />);

    const event = { target: { value: "mailhost!username@example.org" } };

    wrapper.simulate('change', event);
  });
  it('passed valid email (bangified host route used for uucp mailers): mailhost!username@example.org (object settings: allowStrictLocalPart = false)', () => {

    const onValidateInput = jest.fn(isValidEmail => {
      expect(isValidEmail).toEqual(true);
    });
    const emailSettings = {
      allowStrictLocalPart: false
    }
    const wrapper = mount(<EmailInput {...baseProps} onValidateInput={onValidateInput} emailSettings={emailSettings} />);

    const event = { target: { value: "mailhost!username@example.org" } };

    wrapper.simulate('change', event);
  });
  it('passed valid email (% escaped mail route to user@example.com via example.org): user%example.com@example.org)', () => {

    const onValidateInput = jest.fn(isValidEmail => {
      expect(isValidEmail).toEqual(true);
    });
    const emailSettings = new EmailSettings();
    emailSettings.allowStrictLocalPart = false;
    const wrapper = mount(<EmailInput {...baseProps} onValidateInput={onValidateInput} emailSettings={emailSettings} />);

    const event = { target: { value: "user%example.com@example.org" } };

    wrapper.simulate('change', event);
  });
  it('passed valid email with punycode symbols in domain: example@джpумлатест.bрфa', () => {

    const onValidateInput = jest.fn(isValidEmail => {
      expect(isValidEmail).toEqual(false);
    });

    const wrapper = mount(<EmailInput {...baseProps} onValidateInput={onValidateInput} />);

    const event = { target: { value: "example@джpумлатест.bрфa" } };

    wrapper.simulate('change', event);
  });

  it('passed valid email with punycode symbols in local part: mañana@example.com', () => {

    const onValidateInput = jest.fn(isValidEmail => {
      expect(isValidEmail).toEqual(false);
    });

    const wrapper = mount(<EmailInput {...baseProps} onValidateInput={onValidateInput} />);

    const event = { target: { value: "mañana@example.com" } };

    wrapper.simulate('change', event);
  });

  it('passed valid email with punycode symbols in local part and domain: mañana@mañana.com', () => {

    const onValidateInput = jest.fn(isValidEmail => {
      expect(isValidEmail).toEqual(false);
    });

    const wrapper = mount(<EmailInput {...baseProps} onValidateInput={onValidateInput} />);

    const event = { target: { value: "mañana@mañana.com" } };

    wrapper.simulate('change', event);
  });
  it('passed valid email with punycode symbols in local part and domain: mañana@mañana.com (settings: allowDomainPunycode=true)', () => {

    const onValidateInput = jest.fn(isValidEmail => {
      expect(isValidEmail).toEqual(false);
    });
    const emailSettings = new EmailSettings();
    emailSettings.allowDomainPunycode = true;
    const wrapper = mount(<EmailInput {...baseProps} onValidateInput={onValidateInput} emailSettings={emailSettings} />);

    const event = { target: { value: "mañana@mañana.com" } };

    wrapper.simulate('change', event);
  });
  it('passed valid email with punycode symbols in local part and domain: mañana@mañana.com (settings: allowLocalPartPunycode=true)', () => {

    const onValidateInput = jest.fn(isValidEmail => {
      expect(isValidEmail).toEqual(false);
    });
    const emailSettings = new EmailSettings();
    emailSettings.allowLocalPartPunycode = true;
    const wrapper = mount(<EmailInput {...baseProps} onValidateInput={onValidateInput} emailSettings={emailSettings} />);

    const event = { target: { value: "mañana@mañana.com" } };

    wrapper.simulate('change', event);
  });
  it('passed valid email with punycode symbols in local part and domain: mañana@mañana.com (settings: allowDomainPunycode=true, allowLocalPartPunycode=true)', () => {

    const onValidateInput = jest.fn(isValidEmail => {
      expect(isValidEmail).toEqual(false);
    });
    const emailSettings = new EmailSettings();
    emailSettings.allowLocalPartPunycode = true;
    emailSettings.allowDomainPunycode = true;
    const wrapper = mount(<EmailInput {...baseProps} onValidateInput={onValidateInput} emailSettings={emailSettings} />);

    const event = { target: { value: "mañana@mañana.com" } };

    wrapper.simulate('change', event);
  });
  it('passed valid email with punycode symbols in local part and domain: mañana@mañana.com (settings: allowDomainPunycode=true, allowLocalPartPunycode=true, allowStrictLocalPart=false)', () => {

    const onValidateInput = jest.fn(isValidEmail => {
      expect(isValidEmail).toEqual(true);
    });
    const emailSettings = new EmailSettings();
    emailSettings.allowLocalPartPunycode = true;
    emailSettings.allowDomainPunycode = true;
    emailSettings.allowStrictLocalPart = false;
    const wrapper = mount(<EmailInput {...baseProps} onValidateInput={onValidateInput} emailSettings={emailSettings} />);

    const event = { target: { value: "mañana@mañana.com" } };

    wrapper.simulate('change', event);
  });
  it('passed valid email with IP address in domain: user@[127.0.0.1]', () => {

    const onValidateInput = jest.fn(isValidEmail => {
      expect(isValidEmail).toEqual(false);
    });
    const wrapper = mount(<EmailInput {...baseProps} onValidateInput={onValidateInput} />);

    const event = { target: { value: "user@[127.0.0.1]" } };

    wrapper.simulate('change', event);
  });

  it('passed valid email with IP address in domain: user@[127.0.0.1] (settings: allowDomainIp = true)', () => {

    const onValidateInput = jest.fn(isValidEmail => {
      expect(isValidEmail).toEqual(true);
    });

    const emailSettings = { allowDomainIp: true };
    const wrapper = mount(<EmailInput {...baseProps} onValidateInput={onValidateInput} emailSettings={emailSettings} />);

    const event = { target: { value: "user@[127.0.0.1]" } };

    wrapper.simulate('change', event);
  });
  it('passed valid email with Name (RFC 5322): "Jack Bowman" <jack@fogcreek.com>', () => {

    const onValidateInput = jest.fn(isValidEmail => {
      expect(isValidEmail).toEqual(false);
    });

    const wrapper = mount(<EmailInput {...baseProps} onValidateInput={onValidateInput} />);

    const event = { target: { value: '"Jack Bowman" <jack@fogcreek.com>' } };

    wrapper.simulate('change', event);
  });
  it('passed valid email with Name (RFC 5322): "Jack Bowman" <jack@fogcreek.com> (instance of EmailSettings: allowName = true)', () => {

    const onValidateInput = jest.fn(isValidEmail => {
      expect(isValidEmail).toEqual(true);
    });

    const emailSettings = new EmailSettings();
    emailSettings.allowName = true;
    const wrapper = mount(<EmailInput {...baseProps} onValidateInput={onValidateInput} emailSettings={emailSettings} />);

    const event = { target: { value: '"Jack Bowman" <jack@fogcreek.com>' } };

    wrapper.simulate('change', event);
  });
  it('passed valid email with Name (RFC 5322): Bob <bob@example.com>', () => {

    const onValidateInput = jest.fn(isValidEmail => {
      expect(isValidEmail).toEqual(false);
    });

    const wrapper = mount(<EmailInput {...baseProps} onValidateInput={onValidateInput} />);

    const event = { target: { value: 'Bob <bob@example.com>' } };

    wrapper.simulate('change', event);
  });
  it('passed valid email with Name (RFC 5322): Bob <bob@example.com> (instance of EmailSettings: allowName = true)', () => {

    const onValidateInput = jest.fn(isValidEmail => {
      expect(isValidEmail).toEqual(true);
    });

    const emailSettings = new EmailSettings();
    emailSettings.allowName = true;
    const wrapper = mount(<EmailInput {...baseProps} onValidateInput={onValidateInput} emailSettings={emailSettings} />);

    const event = { target: { value: 'Bob <bob@example.com>' } };

    wrapper.simulate('change', event);
  });

  it('passed invalid email (no @ character): Abc.example.com', () => {

    const onValidateInput = jest.fn(isValidEmail => {
      expect(isValidEmail).toEqual(false);
    });

    const wrapper = mount(<EmailInput {...baseProps} onValidateInput={onValidateInput} />);

    const event = { target: { value: "Abc.example.com" } };

    wrapper.simulate('change', event);
  });
  it('passed invalid email (only one @ is allowed outside quotation marks): A@b@c@example.com', () => {

    const onValidateInput = jest.fn(isValidEmail => {
      expect(isValidEmail).toEqual(false);
    });

    const wrapper = mount(<EmailInput {...baseProps} onValidateInput={onValidateInput} />);

    const event = { target: { value: "A@b@c@example.com" } };

    wrapper.simulate('change', event);
  });
  it('passed invalid email (none of the special characters in this local-part are allowed outside quotation marks): a"b(c)d,e:f;g<h>i[j\k]l@example.com', () => {

    const onValidateInput = jest.fn(isValidEmail => {
      expect(isValidEmail).toEqual(false);
    });

    const wrapper = mount(<EmailInput {...baseProps} onValidateInput={onValidateInput} />);

    const event = { target: { value: 'a"b(c)d,e:f;g<h>i[j\k]l@example.com' } };

    wrapper.simulate('change', event);
  });
  it('passed invalid email (none of the special characters in this local-part are allowed outside quotation marks): a"b(c)d,e:f;g<h>i[j\k]l@example.com', () => {

    const onValidateInput = jest.fn(isValidEmail => {
      expect(isValidEmail).toEqual(false);
    });

    const wrapper = mount(<EmailInput {...baseProps} onValidateInput={onValidateInput} />);

    const event = { target: { value: 'a"b(c)d,e:f;g<h>i[j\k]l@example.com' } };

    wrapper.simulate('change', event);
  });
  it('passed invalid email (quoted strings must be dot separated or the only element making up the local-part): just"not"right@example.com', () => {

    const onValidateInput = jest.fn(isValidEmail => {
      expect(isValidEmail).toEqual(false);
    });

    const emailSettings = new EmailSettings();
    emailSettings.allowSpaces = true;
    emailSettings.allowStrictLocalPart = false;
    const wrapper = mount(<EmailInput {...baseProps} onValidateInput={onValidateInput} emailSettings={emailSettings} />);

    const event = { target: { value: 'just"not"right@example.com' } };

    wrapper.simulate('change', event);
  });
  it('passed invalid email  (spaces, quotes, and backslashes may only exist when within quoted strings and preceded by a backslash): this is"not\allowed@example.com', () => {

    const onValidateInput = jest.fn(isValidEmail => {
      expect(isValidEmail).toEqual(false);
    });

    const emailSettings = new EmailSettings();
    emailSettings.allowSpaces = true;
    emailSettings.allowStrictLocalPart = false;
    const wrapper = mount(<EmailInput {...baseProps} onValidateInput={onValidateInput} emailSettings={emailSettings} />);

    const event = { target: { value: 'this is"not\allowed@example.com' } };

    wrapper.simulate('change', event);
  });
  it('passed invalid email (even if escaped (preceded by a backslash), spaces, quotes, and backslashes must still be contained by quotes): this\ still\"not\\allowed@example.com', () => {

    const onValidateInput = jest.fn(isValidEmail => {
      expect(isValidEmail).toEqual(false);
    });

    const emailSettings = new EmailSettings();
    emailSettings.allowSpaces = true;
    emailSettings.allowStrictLocalPart = false;
    const wrapper = mount(<EmailInput {...baseProps} onValidateInput={onValidateInput} emailSettings={emailSettings} />);

    const event = { target: { value: 'this\ still\"not\\allowed@example.com' } };

    wrapper.simulate('change', event);
  });
  it('passed invalid email (local part is longer than 64 characters): 1234567890123456789012345678901234567890123456789012345678901234+x@example.com', () => {

    const onValidateInput = jest.fn(isValidEmail => {
      expect(isValidEmail).toEqual(false);
    });

    const wrapper = mount(<EmailInput {...baseProps} onValidateInput={onValidateInput} />);

    const event = { target: { value: '1234567890123456789012345678901234567890123456789012345678901234+x@example.com' } };

    wrapper.simulate('change', event);
  });

  it('accepts id', () => {
    const wrapper = mount(
      <EmailInput {...baseProps} id="testId" />
    );

    expect(wrapper.prop('id')).toEqual('testId');
  });

  it('accepts className', () => {
    const wrapper = mount(
      <EmailInput {...baseProps} className="test" />
    );

    expect(wrapper.prop('className')).toEqual('test');
  });

  it('accepts style', () => {
    const wrapper = mount(
      <EmailInput {...baseProps} style={{ color: 'red' }} />
    );

    expect(wrapper.getDOMNode().style).toHaveProperty('color', 'red');
  });
});
