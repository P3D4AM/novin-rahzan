using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.Design.Behavior;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.Design;

public class TextBoxDropDownDesigner : ControlDesigner
{
	private DesignerActionListCollection m_ActionLists;

	public override SelectionRules SelectionRules => (SelectionRules)(((ControlDesigner)this).SelectionRules & -4);

	public string Text
	{
		get
		{
			return ((ControlDesigner)this).Control.Text;
		}
		set
		{
			((ControlDesigner)this).Control.Text = value;
			((TextBoxDropDown)(object)((ControlDesigner)this).Control).Select(0, 0);
		}
	}

	public char PasswordChar
	{
		get
		{
			TextBoxDropDown textBoxDropDown = ((ControlDesigner)this).Control as TextBoxDropDown;
			if (textBoxDropDown.UseSystemPasswordChar)
			{
				textBoxDropDown.UseSystemPasswordChar = false;
				char passwordChar = textBoxDropDown.PasswordChar;
				textBoxDropDown.UseSystemPasswordChar = true;
				return passwordChar;
			}
			return textBoxDropDown.PasswordChar;
		}
		set
		{
			TextBoxDropDown textBoxDropDown = ((ControlDesigner)this).Control as TextBoxDropDown;
			textBoxDropDown.PasswordChar = value;
		}
	}

	public override DesignerActionListCollection ActionLists
	{
		get
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			if (m_ActionLists == null)
			{
				m_ActionLists = new DesignerActionListCollection();
				object obj = ((object)this).GetType().Assembly.CreateInstance("System.Windows.Forms.Design.TextBoxActionList", ignoreCase: false, BindingFlags.NonPublic, null, new object[1] { this }, null, null);
				if (obj != null)
				{
					m_ActionLists.Add((DesignerActionList)((obj is DesignerActionList) ? obj : null));
				}
			}
			return m_ActionLists;
		}
	}

	public override IList SnapLines
	{
		get
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Invalid comparison between Unknown and I4
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Expected O, but got Unknown
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Invalid comparison between Unknown and I4
			ArrayList arrayList = ((ControlDesigner)this).SnapLines as ArrayList;
			int num = GetTextBaseline(((ControlDesigner)this).Control, (ContentAlignment)1);
			BorderStyle val = (BorderStyle)1;
			if ((int)val == 1 || (int)val == 0)
			{
				num += 2;
			}
			else if ((int)val == 2)
			{
				num += 3;
			}
			arrayList.Add((object?)new SnapLine((SnapLineType)6, num, (SnapLinePriority)2));
			return arrayList;
		}
	}

	public override void InitializeNewComponent(IDictionary defaultValues)
	{
		((ControlDesigner)this).InitializeNewComponent(defaultValues);
		TextBoxDropDown textBoxDropDown = ((ControlDesigner)this).Control as TextBoxDropDown;
		textBoxDropDown.BackgroundStyle.Class = ElementStyleClassKeys.TextBoxBorderKey;
		textBoxDropDown.ButtonDropDown.Visible = true;
		((Control)textBoxDropDown).Height = textBoxDropDown.PreferredHeight;
		((Control)textBoxDropDown).Text = "";
		textBoxDropDown.Style = eDotNetBarStyle.StyleManagerControlled;
	}

	protected override void PreFilterProperties(IDictionary properties)
	{
		((ControlDesigner)this).PreFilterProperties(properties);
		string[] array = new string[1] { "PasswordChar" };
		Attribute[] attributes = new Attribute[0];
		for (int i = 0; i < array.Length; i++)
		{
			PropertyDescriptor propertyDescriptor = (PropertyDescriptor)properties[array[i]];
			if (propertyDescriptor != null)
			{
				properties[array[i]] = TypeDescriptor.CreateProperty(typeof(TextBoxDropDownDesigner), propertyDescriptor, attributes);
			}
		}
		array = new string[1] { "Text" };
		attributes = new Attribute[0];
		for (int j = 0; j < array.Length; j++)
		{
			PropertyDescriptor propertyDescriptor2 = (PropertyDescriptor)properties[array[j]];
			if (propertyDescriptor2 != null)
			{
				properties[array[j]] = TypeDescriptor.CreateProperty(typeof(TextBoxDropDownDesigner), propertyDescriptor2, attributes);
			}
		}
	}

	public void ResetText()
	{
		((ControlDesigner)this).Control.Text = "";
	}

	public bool ShouldSerializeText()
	{
		return TypeDescriptor.GetProperties(typeof(TextBoxDropDownDesigner))["Text"].ShouldSerializeValue(((ComponentDesigner)this).Component);
	}

	private static int GetTextBaseline(Control ctrl, ContentAlignment alignment)
	{
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		Rectangle clientRectangle = ctrl.ClientRectangle;
		int num = 0;
		int num2 = 0;
		Graphics val = ctrl.CreateGraphics();
		try
		{
			IntPtr hdc = val.GetHdc();
			IntPtr hObject = ctrl.Font.ToHfont();
			try
			{
				IntPtr hObject2 = WinApi.SelectObject(hdc, hObject);
				WinApi.TEXTMETRIC tEXTMETRIC = new WinApi.TEXTMETRIC();
				WinApi.GetTextMetrics(new HandleRef(ctrl, hdc), tEXTMETRIC);
				num = tEXTMETRIC.tmAscent + 1;
				num2 = tEXTMETRIC.tmHeight;
				WinApi.SelectObject(hdc, hObject2);
			}
			finally
			{
				WinApi.DeleteObject(hObject);
				val.ReleaseHdc(hdc);
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		if ((alignment & 7) != 0)
		{
			return clientRectangle.Top + num;
		}
		if ((alignment & 0x70) != 0)
		{
			return clientRectangle.Top + clientRectangle.Height / 2 - num2 / 2 + num;
		}
		return clientRectangle.Bottom - num2 + num;
	}
}
