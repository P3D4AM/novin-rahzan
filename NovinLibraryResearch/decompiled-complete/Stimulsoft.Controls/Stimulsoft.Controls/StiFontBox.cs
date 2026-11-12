using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
[ToolboxBitmap(typeof(StiFontBox), "Toolbox.StiFontBox.bmp")]
public class StiFontBox : StiComboBox
{
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override bool UseFirstImage
	{
		get
		{
			return base.UseFirstImage;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string DisplayMember
	{
		get
		{
			return ((ListControl)this).DisplayMember;
		}
		set
		{
			((ListControl)this).DisplayMember = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public object DataSource
	{
		get
		{
			return ((ComboBox)this).DataSource;
		}
		set
		{
			((ComboBox)this).DataSource = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string ValueMember
	{
		get
		{
			return ((ListControl)this).ValueMember;
		}
		set
		{
			((ListControl)this).ValueMember = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public new ComboBoxStyle DropDownStyle
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return base.DropDownStyle;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			base.DropDownStyle = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override ImageList ImageList
	{
		get
		{
			return base.ImageList;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public ObjectCollection Items => ((ComboBox)this).Items;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override string Text
	{
		get
		{
			return ((ComboBox)this).Text;
		}
		set
		{
			((ComboBox)this).Text = value;
		}
	}

	[Category("Behavior")]
	public virtual Font SelectedFont
	{
		get
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Expected O, but got Unknown
			return new Font(((Control)this).Text, 8f);
		}
		set
		{
			((Control)this).Text = value.Name;
			((Control)this).Invalidate();
		}
	}

	private void Fill()
	{
		try
		{
			List<string> list = new List<string>();
			int num = 0;
			int selectedIndex = -1;
			foreach (FontFamily fontFamily in StiFontCollection.GetFontFamilies())
			{
				if (fontFamily.IsStyleAvailable((FontStyle)0))
				{
					list.Add(fontFamily.Name);
					if (((Control)this).Text == fontFamily.Name)
					{
						selectedIndex = num;
					}
					num++;
				}
			}
			((ComboBox)this).Items.Clear();
			ObjectCollection items = ((ComboBox)this).Items;
			object[] array = list.ToArray();
			items.AddRange(array);
			((ListControl)this).SelectedIndex = selectedIndex;
		}
		catch
		{
		}
	}

	protected override void OnMeasureItem(MeasureItemEventArgs e)
	{
		base.OnMeasureItem(e);
		Graphics graphics = e.Graphics;
		string text = Items[e.Index].ToString();
		e.ItemWidth = (int)graphics.MeasureString(text, ((Control)this).Font).Width + SystemInformation.HorizontalScrollBarThumbWidth + 35;
	}

	protected override void OnDrawItem(DrawItemEventArgs e)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Invalid comparison between Unknown and I4
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Invalid comparison between Unknown and I4
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Invalid comparison between Unknown and I4
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Expected O, but got Unknown
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Invalid comparison between Unknown and I4
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Expected O, but got Unknown
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		if (e.Index == -1 || !((Control)this).Enabled)
		{
			return;
		}
		string text = SelectedFont.Name;
		if (e.Index != -1)
		{
			text = Items[e.Index].ToString();
		}
		Rectangle bounds = e.Bounds;
		if ((e.State & 1) > 0)
		{
			bounds.Width--;
		}
		StiControlPaint.DrawItem(e.Graphics, bounds, e.State, text, null, e.Index, ((Control)this).Font, ((Control)this).BackColor, ((Control)this).ForeColor, 30, ((Control)this).RightToLeft);
		Rectangle rectangle = new Rectangle(e.Bounds.X, e.Bounds.Y, StiScaleUI.I(28), e.Bounds.Height - 1);
		Graphics graphics = e.Graphics;
		if ((e.State & 0x10) > 0 || (e.State & 1) > 0)
		{
			graphics.DrawRectangle(StiPens.SelectedText, rectangle);
		}
		if (!((Control)this).Enabled)
		{
			return;
		}
		StringFormat val = new StringFormat();
		try
		{
			val.LineAlignment = (StringAlignment)1;
			val.Alignment = (StringAlignment)1;
			val.FormatFlags = (StringFormatFlags)4096;
			if ((int)((Control)this).RightToLeft == 1)
			{
				val.FormatFlags = (StringFormatFlags)(val.FormatFlags | 1);
			}
			Font val2 = new Font(text, 10f);
			try
			{
				graphics.DrawString("ab", val2, StiBrushes.SelectedText, (RectangleF)rectangle, val);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public StiFontBox()
	{
		base.DrawMode = (DrawMode)2;
		((Control)this).Text = "Arial";
		((ComboBox)this).ItemHeight = 14;
		DropDownStyle = (ComboBoxStyle)2;
		Fill();
	}
}
