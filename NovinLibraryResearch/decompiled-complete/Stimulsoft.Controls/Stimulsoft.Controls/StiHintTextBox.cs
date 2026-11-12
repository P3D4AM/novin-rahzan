using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
[ToolboxBitmap(typeof(StiHintTextBox), "Toolbox.StiTextBox.bmp")]
public class StiHintTextBox : StiHintEditBase
{
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string SelectedText
	{
		get
		{
			return ((TextBoxBase)base.TextBox).SelectedText;
		}
		set
		{
			((TextBoxBase)base.TextBox).SelectedText = value;
		}
	}

	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Category("Appearance")]
	public HorizontalAlignment TextAlign
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return ((TextBox)base.TextBox).TextAlign;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((TextBox)base.TextBox).TextAlign = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool FixedHeight { get; set; } = true;


	[DefaultValue(false)]
	[Category("Behavior")]
	public bool AcceptsTab
	{
		get
		{
			return ((TextBoxBase)base.TextBox).AcceptsTab;
		}
		set
		{
			((TextBoxBase)base.TextBox).AcceptsTab = value;
		}
	}

	[DefaultValue(false)]
	[Category("Behavior")]
	public bool AcceptsReturn
	{
		get
		{
			return ((TextBox)base.TextBox).AcceptsReturn;
		}
		set
		{
			((TextBox)base.TextBox).AcceptsReturn = value;
		}
	}

	[DefaultValue(true)]
	[Category("Behavior")]
	public bool WordWrap
	{
		get
		{
			return ((TextBoxBase)base.TextBox).WordWrap;
		}
		set
		{
			((TextBoxBase)base.TextBox).WordWrap = value;
		}
	}

	[DefaultValue(false)]
	[Category("Behavior")]
	public bool Multiline
	{
		get
		{
			return ((TextBoxBase)base.TextBox).Multiline;
		}
		set
		{
			((TextBoxBase)base.TextBox).Multiline = value;
		}
	}

	[DefaultValue('\0')]
	[Category("CatBehavior")]
	public char PasswordChar
	{
		get
		{
			return ((TextBox)base.TextBox).PasswordChar;
		}
		set
		{
			((TextBox)base.TextBox).PasswordChar = value;
		}
	}

	public string Hint
	{
		get
		{
			return base.TextBox.Hint;
		}
		set
		{
			base.TextBox.Hint = value;
		}
	}

	public void SelectAll()
	{
		((TextBoxBase)base.TextBox).SelectAll();
	}

	protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		if (FixedHeight)
		{
			height = StiScaleUI.I(21);
		}
		((Control)this).SetBoundsCore(x, y, width, height, specified);
	}
}
