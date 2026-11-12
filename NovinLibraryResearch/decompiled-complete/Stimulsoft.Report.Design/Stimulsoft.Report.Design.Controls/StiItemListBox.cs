using System.ComponentModel;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Design.Components;

namespace Stimulsoft.Report.Design.Controls;

[ToolboxItem(false)]
public class StiItemListBox : ListBox
{
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override int ItemHeight
	{
		get
		{
			return StiScale.YYI(32.0);
		}
		set
		{
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override DrawMode DrawMode
	{
		get
		{
			return (DrawMode)1;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public BorderStyle BorderStyle
	{
		get
		{
			return (BorderStyle)0;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IntegralHeight
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsVerticalScrollBarVisible => StiListBoxPainter.IsVerticalScrollbarVisible((Control)(object)this);

	public StiItemListBox()
	{
		((Control)this).SetStyle((ControlStyles)131072, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)2, true);
		((ListBox)this).BorderStyle = (BorderStyle)0;
		((ListBox)this).DrawMode = (DrawMode)2;
		if (StiUX.IsDark)
		{
			((ListBox)this).BackColor = StiUX.ButtonBackground;
		}
	}
}
