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

public class TextBoxXDesigner : ControlDesigner
{
	private DesignerActionListCollection m_ActionLists;

	public string Text
	{
		get
		{
			return ((ControlDesigner)this).Control.Text;
		}
		set
		{
			((ControlDesigner)this).Control.Text = value;
			((TextBoxBase)(TextBoxX)(object)((ControlDesigner)this).Control).Select(0, 0);
		}
	}

	public char PasswordChar
	{
		get
		{
			Control control = ((ControlDesigner)this).Control;
			TextBox val = (TextBox)(object)((control is TextBox) ? control : null);
			if (val.UseSystemPasswordChar)
			{
				val.UseSystemPasswordChar = false;
				char passwordChar = val.PasswordChar;
				val.UseSystemPasswordChar = true;
				return passwordChar;
			}
			return val.PasswordChar;
		}
		set
		{
			Control control = ((ControlDesigner)this).Control;
			TextBox val = (TextBox)(object)((control is TextBox) ? control : null);
			val.PasswordChar = value;
		}
	}

	public override SelectionRules SelectionRules
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			SelectionRules selectionRules = ((ControlDesigner)this).SelectionRules;
			object component = ((ComponentDesigner)this).Component;
			selectionRules = (SelectionRules)(selectionRules | 0xF);
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(component)["Multiline"];
			if (propertyDescriptor != null)
			{
				object value = propertyDescriptor.GetValue(component);
				if (value is bool && !(bool)value)
				{
					PropertyDescriptor propertyDescriptor2 = TypeDescriptor.GetProperties(component)["AutoSize"];
					if (propertyDescriptor2 != null)
					{
						object value2 = propertyDescriptor2.GetValue(component);
						if (value2 is bool && (bool)value2)
						{
							selectionRules = (SelectionRules)(selectionRules & -4);
						}
					}
				}
			}
			return selectionRules;
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
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Invalid comparison between Unknown and I4
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Expected O, but got Unknown
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Invalid comparison between Unknown and I4
			ArrayList arrayList = ((ControlDesigner)this).SnapLines as ArrayList;
			int num = GetTextBaseline(((ControlDesigner)this).Control, (ContentAlignment)1);
			BorderStyle val = (BorderStyle)2;
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(((ComponentDesigner)this).Component)["BorderStyle"];
			if (propertyDescriptor != null)
			{
				val = (BorderStyle)propertyDescriptor.GetValue(((ComponentDesigner)this).Component);
			}
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
		SetDesignTimeDefaults();
	}

	private void SetDesignTimeDefaults()
	{
		PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(((ComponentDesigner)this).Component)["Text"];
		if (propertyDescriptor != null && (object)propertyDescriptor.PropertyType == typeof(string) && !propertyDescriptor.IsReadOnly && propertyDescriptor.IsBrowsable)
		{
			propertyDescriptor.SetValue(((ComponentDesigner)this).Component, "");
		}
		TextBoxX textBoxX = ((ControlDesigner)this).Control as TextBoxX;
		textBoxX.Border.Class = ElementStyleClassKeys.TextBoxBorderKey;
		textBoxX.SetAutoHeight();
		textBoxX.PreventEnterBeep = true;
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
				properties[array[i]] = TypeDescriptor.CreateProperty(typeof(TextBoxXDesigner), propertyDescriptor, attributes);
			}
		}
		array = new string[1] { "Text" };
		attributes = new Attribute[0];
		for (int j = 0; j < array.Length; j++)
		{
			PropertyDescriptor propertyDescriptor2 = (PropertyDescriptor)properties[array[j]];
			if (propertyDescriptor2 != null)
			{
				properties[array[j]] = TypeDescriptor.CreateProperty(typeof(TextBoxXDesigner), propertyDescriptor2, attributes);
			}
		}
	}

	public void ResetText()
	{
		((ControlDesigner)this).Control.Text = "";
	}

	public bool ShouldSerializeText()
	{
		return TypeDescriptor.GetProperties(typeof(TextBoxX))["Text"].ShouldSerializeValue(((ComponentDesigner)this).Component);
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
