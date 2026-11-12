using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
[ToolboxBitmap(typeof(StiTextBox), "Toolbox.StiTextBox.bmp")]
public class StiMaskedTextBox : StiMaskedEditBase
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
			return base.TextBox.TextAlign;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			base.TextBox.TextAlign = value;
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
			return base.TextBox.AcceptsTab;
		}
		set
		{
			base.TextBox.AcceptsTab = value;
		}
	}

	[DefaultValue("")]
	public string Mask
	{
		get
		{
			return base.TextBox.Mask;
		}
		set
		{
			base.TextBox.Mask = value;
		}
	}

	[DefaultValue(true)]
	[Category("Behavior")]
	public bool WordWrap
	{
		get
		{
			return base.TextBox.WordWrap;
		}
		set
		{
			base.TextBox.WordWrap = value;
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
			return base.TextBox.PasswordChar;
		}
		set
		{
			base.TextBox.PasswordChar = value;
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
