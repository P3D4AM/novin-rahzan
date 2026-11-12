using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
public class StiListBoxV2 : ListBox
{
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override int ItemHeight
	{
		get
		{
			return StiScaleUI.YYI(28.0);
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
	public override Color ForeColor
	{
		get
		{
			if (!StiUX.IsDark)
			{
				return ((ListBox)this).ForeColor;
			}
			return StiUX.ItemForeground;
		}
		set
		{
			if (!StiUX.IsDark)
			{
				((ListBox)this).ForeColor = value;
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override Color BackColor
	{
		get
		{
			if (!StiUX.IsDark)
			{
				return ((ListBox)this).BackColor;
			}
			return StiUX.InputBackground;
		}
		set
		{
			if (!StiUX.IsDark)
			{
				((ListBox)this).BackColor = value;
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsBaseOwnerDraw { get; set; }

	protected override void OnDrawItem(DrawItemEventArgs e)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Invalid comparison between Unknown and I4
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		if (IsBaseOwnerDraw)
		{
			((ListBox)this).OnDrawItem(e);
		}
		else if (e.Index != -1)
		{
			Graphics graphics = e.Graphics;
			bool flag = (e.State & 1) > 0;
			Color color = (flag ? SystemColors.MenuHighlight : Color.White);
			Color color2 = (flag ? SystemColors.HighlightText : SystemColors.ControlText);
			if (StiUX.IsDark)
			{
				color = (flag ? StiUX.ItemBackgroundSelected : StiUX.InputBackground);
				color2 = (flag ? StiUX.ItemForegroundSelected : StiUX.ItemForeground);
			}
			Rectangle bounds = e.Bounds;
			bounds.Inflate(-StiScaleUI.I2, -StiScaleUI.I1);
			if (e.Index == 0)
			{
				bounds.Y++;
				bounds.Height--;
			}
			StiDrawing.FillRectangle(graphics, color, bounds);
			TextFormatFlags val = (TextFormatFlags)32804;
			int i = StiScaleUI.I4;
			bounds.X += i;
			bounds.Width -= i;
			if (e.Index >= 0 && e.Index < ((ListBox)this).Items.Count)
			{
				TextRenderer.DrawText((IDeviceContext)(object)graphics, ((ListBox)this).Items[e.Index].ToString(), ((Control)this).Font, bounds, color2, val);
			}
		}
	}

	public StiListBoxV2()
	{
		((Control)this).SetStyle((ControlStyles)131072, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((ListBox)this).BorderStyle = (BorderStyle)0;
		((ListBox)this).DrawMode = (DrawMode)1;
		((ListBox)this).IntegralHeight = false;
	}
}
