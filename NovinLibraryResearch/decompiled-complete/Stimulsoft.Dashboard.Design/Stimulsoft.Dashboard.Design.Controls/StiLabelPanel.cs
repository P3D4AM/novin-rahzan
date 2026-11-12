using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Dashboard.Design.Controls;

[ToolboxItem(false)]
public class StiLabelPanel : Panel
{
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	protected override Size DefaultSize => new Size(((Panel)this).DefaultSize.Width, 32);

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public override Color ForeColor
	{
		get
		{
			if (!StiUX.IsDark)
			{
				return Color.DimGray;
			}
			return StiUX.Foreground;
		}
		set
		{
		}
	}

	[Browsable(true)]
	public override string Text
	{
		get
		{
			return ((Panel)this).Text;
		}
		set
		{
			if (((Panel)this).Text != value)
			{
				((Panel)this).Text = value;
				((Control)this).Invalidate();
			}
		}
	}

	public ContentAlignment TextAlign { get; set; } = (ContentAlignment)16;


	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Invalid comparison between Unknown and I4
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Invalid comparison between Unknown and I4
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected I4, but got Unknown
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Invalid comparison between Unknown and I4
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Invalid comparison between Unknown and I4
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Invalid comparison between Unknown and I4
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Invalid comparison between Unknown and I4
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Invalid comparison between Unknown and I4
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Invalid comparison between Unknown and I4
		((Control)this).OnPaint(e);
		StringFormat val = new StringFormat();
		try
		{
			ContentAlignment textAlign = TextAlign;
			if ((int)textAlign <= 32)
			{
				switch (textAlign - 1)
				{
				default:
					if ((int)textAlign != 16)
					{
						if ((int)textAlign == 32)
						{
							val.Alignment = (StringAlignment)1;
							val.LineAlignment = (StringAlignment)1;
						}
					}
					else
					{
						val.Alignment = (StringAlignment)0;
						val.LineAlignment = (StringAlignment)1;
					}
					break;
				case 0:
					val.Alignment = (StringAlignment)0;
					val.LineAlignment = (StringAlignment)0;
					break;
				case 1:
					val.Alignment = (StringAlignment)1;
					val.LineAlignment = (StringAlignment)0;
					break;
				case 3:
					val.Alignment = (StringAlignment)2;
					val.LineAlignment = (StringAlignment)0;
					break;
				case 2:
					break;
				}
			}
			else if ((int)textAlign <= 256)
			{
				if ((int)textAlign != 64)
				{
					if ((int)textAlign == 256)
					{
						val.Alignment = (StringAlignment)0;
						val.LineAlignment = (StringAlignment)2;
					}
				}
				else
				{
					val.Alignment = (StringAlignment)2;
					val.LineAlignment = (StringAlignment)1;
				}
			}
			else if ((int)textAlign != 512)
			{
				if ((int)textAlign == 1024)
				{
					val.Alignment = (StringAlignment)2;
					val.LineAlignment = (StringAlignment)2;
				}
			}
			else
			{
				val.Alignment = (StringAlignment)1;
				val.LineAlignment = (StringAlignment)2;
			}
			StiDrawing.DrawString(e.Graphics, ((Control)this).Text, ((Control)this).Font, ((Control)this).ForeColor, new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height), val);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public StiLabelPanel()
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).Height = StiScaleUI.YYI(32.0);
	}
}
