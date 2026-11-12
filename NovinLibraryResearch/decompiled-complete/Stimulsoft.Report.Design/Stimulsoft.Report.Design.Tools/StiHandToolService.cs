using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Design.Controls;

namespace Stimulsoft.Report.Design.Tools;

[StiServiceBitmap(typeof(StiHandToolService), "Stimulsoft.Report.Design.Bmp.Tools.Hand.bmp")]
[Obsolete]
[EditorBrowsable(EditorBrowsableState.Never)]
public class StiHandToolService : StiToolService
{
	private Point lastPos;

	private static Cursor cursorHand;

	private static Cursor cursorHandMove;

	public override string ServiceName => Loc.Get("Toolbox", "Hand");

	public override int Position => 3;

	public override Keys Shortcut => (Keys)119;

	public StiHandAction Action { get; set; }

	public static Cursor CursorHand
	{
		get
		{
			if (StiScale.Factor != 1.0)
			{
				return Cursors.Hand;
			}
			if (cursorHand == (Cursor)null)
			{
				cursorHand = StiCursorUtils.GetCursor("Stimulsoft.Report", "Stimulsoft.Report.Cursors.Hand.cur");
			}
			return cursorHand;
		}
	}

	public static Cursor CursorHandMove
	{
		get
		{
			if (StiScale.Factor > 2.0)
			{
				return Cursors.Hand;
			}
			if (cursorHandMove != (Cursor)null)
			{
				return cursorHandMove;
			}
			if (cursorHandMove == (Cursor)null)
			{
				cursorHandMove = ((StiScale.Factor == 1.0) ? StiCursorUtils.GetCursor("Stimulsoft.Report", "Stimulsoft.Report.Cursors.HandMove.cur") : StiCursorUtils.GetCursor("Stimulsoft.Report", "Stimulsoft.Report.Cursors.HandMove_x2.cur"));
			}
			return cursorHandMove;
		}
	}

	public override void OnMouseDown(object sender, MouseEventArgs e)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Invalid comparison between Unknown and I4
		base.OnMouseDown(sender, e);
		if ((int)e.Button == 1048576)
		{
			Action = StiHandAction.HandMove;
			((Control)Designer.CurrentPageView.View).Cursor = GetCursor();
			lastPos = Cursor.Position;
		}
	}

	public override void OnMouseUp(object sender, MouseEventArgs e)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Invalid comparison between Unknown and I4
		base.OnMouseUp(sender, e);
		if ((int)e.Button == 1048576)
		{
			Action = StiHandAction.Hand;
		}
	}

	public override void OnMouseMove(object sender, MouseEventArgs e)
	{
		base.OnMouseMove(sender, e);
		if (Action != StiHandAction.HandMove)
		{
			return;
		}
		StiPageViewControl currentPageView = Designer.CurrentPageView;
		if (((Control)currentPageView.HScrollBar).Visible)
		{
			int value = ((ScrollBar)currentPageView.HScrollBar).Value;
			value += lastPos.X - Cursor.Position.X;
			if (value < ((ScrollBar)currentPageView.HScrollBar).Minimum)
			{
				value = ((ScrollBar)currentPageView.HScrollBar).Minimum;
			}
			if (value > ((ScrollBar)currentPageView.HScrollBar).Maximum - ((ScrollBar)currentPageView.HScrollBar).LargeChange)
			{
				value = ((ScrollBar)currentPageView.HScrollBar).Maximum - ((ScrollBar)currentPageView.HScrollBar).LargeChange;
			}
			((ScrollBar)currentPageView.HScrollBar).Value = value;
		}
		if (((Control)currentPageView.VScrollBar).Visible)
		{
			int value2 = ((ScrollBar)currentPageView.VScrollBar).Value;
			value2 += lastPos.Y - Cursor.Position.Y;
			if (value2 < ((ScrollBar)currentPageView.VScrollBar).Minimum)
			{
				value2 = ((ScrollBar)currentPageView.VScrollBar).Minimum;
			}
			if (value2 > ((ScrollBar)currentPageView.VScrollBar).Maximum - ((ScrollBar)currentPageView.VScrollBar).LargeChange)
			{
				value2 = ((ScrollBar)currentPageView.VScrollBar).Maximum - ((ScrollBar)currentPageView.VScrollBar).LargeChange;
			}
			((ScrollBar)currentPageView.VScrollBar).Value = value2;
		}
		lastPos.X = Cursor.Position.X;
		lastPos.Y = Cursor.Position.Y;
		Designer.CurrentPageView.OnHorScroll(this, null);
		Designer.CurrentPageView.OnVerScroll(this, null);
	}

	public override Cursor GetCursor()
	{
		if (Action != 0)
		{
			return CursorHandMove;
		}
		return CursorHand;
	}
}
