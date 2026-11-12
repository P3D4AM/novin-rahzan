using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Viewer.Elements;
using Stimulsoft.Dashboard.Viewer.Images;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Viewer.Controls;

[ToolboxItem(false)]
public class StiViewStateActionButton : StiActionButton
{
	private StiUserViewState state;

	protected override void PaintBackground(Graphics g, Rectangle rect)
	{
		bool isHot = rect.Contains(((Control)this).PointToClient(Control.MousePosition));
		Color backColor = base.Colors.GetBackColor(isSelected: false, isHot);
		StiDrawing.FillRectangle(g, backColor, rect);
	}

	protected override void PaintBorder(Graphics g, Rectangle rect)
	{
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		StiHolderElementControl stiHolderElementControl = ((Control)this).Parent as StiHolderElementControl;
		IStiUserViewStates stiUserViewStates = stiHolderElementControl.Element as IStiUserViewStates;
		bool isHot = rect.Contains(((Control)this).PointToClient(Control.MousePosition));
		bool flag = stiUserViewStates != null && stiUserViewStates.SelectedViewStateKey == state.Key;
		if (flag)
		{
			rect.Width--;
			rect.Height--;
			Color backColor = base.Colors.GetBackColor(flag, isHot);
			Pen val = new Pen(backColor, (float)StiScale.I1);
			try
			{
				val.Alignment = (PenAlignment)1;
				g.DrawRectangle(val, rect);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
	}

	public StiViewStateActionButton(StiControlColorCollection colors, Point? position = null, bool isDarkStyle = false, StiUserViewState state = null)
		: base(colors, position)
	{
		this.state = state;
		if (state != null)
		{
			string text = state.SeriesType.ToString();
			if (isDarkStyle)
			{
				text = "White" + text;
			}
			((ButtonBase)this).Image = StiDashboardDrawingImages.GetDesignImage("Meters.Charts." + text);
			base.ToolTip = state.Name;
		}
	}
}
