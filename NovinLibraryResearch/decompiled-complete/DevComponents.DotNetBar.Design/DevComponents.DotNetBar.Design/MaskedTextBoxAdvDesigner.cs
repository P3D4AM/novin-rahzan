using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.Design;

public class MaskedTextBoxAdvDesigner : ControlDesigner
{
	private DesignerActionListCollection _Actions;

	private DesignerVerbCollection _Verbs;

	public override SelectionRules SelectionRules => (SelectionRules)(((ControlDesigner)this).SelectionRules & -4);

	public char PasswordChar
	{
		get
		{
			MaskedTextBoxAdv maskedTextBoxAdv = ((ControlDesigner)this).Control as MaskedTextBoxAdv;
			if (maskedTextBoxAdv.UseSystemPasswordChar)
			{
				maskedTextBoxAdv.UseSystemPasswordChar = false;
				char passwordChar = maskedTextBoxAdv.PasswordChar;
				maskedTextBoxAdv.UseSystemPasswordChar = true;
				return passwordChar;
			}
			return maskedTextBoxAdv.PasswordChar;
		}
		set
		{
			MaskedTextBoxAdv maskedTextBoxAdv = ((ControlDesigner)this).Control as MaskedTextBoxAdv;
			maskedTextBoxAdv.PasswordChar = value;
		}
	}

	public string Text
	{
		get
		{
			MaskedTextBoxAdv maskedTextBoxAdv = ((ControlDesigner)this).Control as MaskedTextBoxAdv;
			if (string.IsNullOrEmpty(maskedTextBoxAdv.Mask))
			{
				return ((Control)maskedTextBoxAdv).Text;
			}
			return maskedTextBoxAdv.MaskedTextProvider.ToString(includePrompt: false, includeLiterals: false);
		}
		set
		{
			MaskedTextBoxAdv maskedTextBoxAdv = ((ControlDesigner)this).Control as MaskedTextBoxAdv;
			if (string.IsNullOrEmpty(maskedTextBoxAdv.Mask))
			{
				((Control)maskedTextBoxAdv).Text = value;
				return;
			}
			bool resetOnSpace = maskedTextBoxAdv.ResetOnSpace;
			bool resetOnPrompt = maskedTextBoxAdv.ResetOnPrompt;
			bool skipLiterals = maskedTextBoxAdv.SkipLiterals;
			maskedTextBoxAdv.ResetOnSpace = true;
			maskedTextBoxAdv.ResetOnPrompt = true;
			maskedTextBoxAdv.SkipLiterals = true;
			((Control)maskedTextBoxAdv).Text = value;
			maskedTextBoxAdv.ResetOnSpace = resetOnSpace;
			maskedTextBoxAdv.ResetOnPrompt = resetOnPrompt;
			maskedTextBoxAdv.SkipLiterals = skipLiterals;
		}
	}

	public override DesignerActionListCollection ActionLists
	{
		get
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			if (_Actions == null)
			{
				_Actions = new DesignerActionListCollection();
				_Actions.Add((DesignerActionList)(object)new MaskedTextBoxAdvDesignerActionList(this));
			}
			return _Actions;
		}
	}

	public override DesignerVerbCollection Verbs
	{
		get
		{
			if (_Verbs == null)
			{
				_Verbs = new DesignerVerbCollection();
				_Verbs.Add(new DesignerVerb("Set Mask...", OnVerbSetMask));
			}
			return _Verbs;
		}
	}

	public override void InitializeNewComponent(IDictionary defaultValues)
	{
		((ControlDesigner)this).InitializeNewComponent(defaultValues);
		MaskedTextBoxAdv maskedTextBoxAdv = ((ControlDesigner)this).Control as MaskedTextBoxAdv;
		maskedTextBoxAdv.BackgroundStyle.Class = ElementStyleClassKeys.TextBoxBorderKey;
		maskedTextBoxAdv.ButtonClear.Visible = true;
		((Control)maskedTextBoxAdv).Height = maskedTextBoxAdv.PreferredHeight;
		((Control)maskedTextBoxAdv).Text = "";
		maskedTextBoxAdv.Style = eDotNetBarStyle.StyleManagerControlled;
	}

	protected override void PreFilterProperties(IDictionary properties)
	{
		((ControlDesigner)this).PreFilterProperties(properties);
		string[] array = new string[2] { "Text", "PasswordChar" };
		Attribute[] attributes = new Attribute[0];
		for (int i = 0; i < array.Length; i++)
		{
			PropertyDescriptor propertyDescriptor = (PropertyDescriptor)properties[array[i]];
			if (propertyDescriptor != null)
			{
				properties[array[i]] = TypeDescriptor.CreateProperty(typeof(MaskedTextBoxAdvDesigner), propertyDescriptor, attributes);
			}
		}
	}

	private void OnVerbSetMask(object sender, EventArgs e)
	{
		new MaskedTextBoxAdvDesignerActionList(this).SetMask();
	}
}
