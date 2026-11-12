using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Controls;

namespace Stimulsoft.Report.Design.Controls;

[ToolboxItem(false)]
internal class StiHatchStyleBox : StiComboBox
{
	private HatchStyle hatchStyle;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public ObjectCollection Items => ((ComboBox)this).Items;

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
			base.ImageList = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int ItemHeight
	{
		get
		{
			return ((ComboBox)this).ItemHeight;
		}
		set
		{
			((ComboBox)this).ItemHeight = value;
		}
	}

	[Browsable(false)]
	[Category("Behavior")]
	public HatchStyle HatchStyle
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return hatchStyle;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			hatchStyle = value;
			((Control)this).Invalidate();
		}
	}

	protected override void OnDrawItem(DrawItemEventArgs e)
	{
		if (e.Index > -1 && e.Index < Items.Count)
		{
			StiHatchStyleHelper.DrawHatchStyle(e);
		}
	}

	protected override void OnMeasureItem(MeasureItemEventArgs e)
	{
		if (e.Index > -1 && e.Index < Items.Count)
		{
			e.ItemWidth = StiHatchStyleHelper.GetWidthOfHatchStyle(Graphics.FromHwnd(((Control)this).Handle), ((Control)this).Font, e.Index);
		}
	}

	protected override void OnSelectedIndexChanged(EventArgs e)
	{
		((ComboBox)this).OnSelectedIndexChanged(e);
		if (((ListControl)this).SelectedIndex != -1)
		{
			HatchStyle = StiBrushes.HatchStyles[((ListControl)this).SelectedIndex];
			((Control)this).Invalidate();
		}
	}

	private void InitHatchStyles()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		HatchStyle[] hatchStyles = StiBrushes.HatchStyles;
		foreach (HatchStyle val in hatchStyles)
		{
			Items.Add((object)val);
		}
	}

	public StiHatchStyleBox()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		HatchStyle = (HatchStyle)0;
		InitHatchStyles();
		((ComboBox)this).SelectedItem = HatchStyle;
		ItemHeight = 18;
		DropDownStyle = (ComboBoxStyle)2;
	}
}
