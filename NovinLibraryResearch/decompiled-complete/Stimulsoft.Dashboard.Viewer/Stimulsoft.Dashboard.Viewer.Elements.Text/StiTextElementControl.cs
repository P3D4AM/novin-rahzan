using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Controls;
using Stimulsoft.Dashboard.Drawing.Atoms;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report;
using Stimulsoft.Report.Controls;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;

namespace Stimulsoft.Dashboard.Viewer.Elements.Text;

[ToolboxItem(false)]
public class StiTextElementControl : StiSimpleAtomElementControl
{
	private Timer timer;

	private StiHyperlinkToolTip toolTipTextHyperlink;

	public override void CreateChild()
	{
		base.CreateChild();
		if (base.Element is IStiTextElement textElement)
		{
			CreateTimeExpressionTimer(textElement);
		}
	}

	private void CreateTimeExpressionTimer(IStiTextElement textElement)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		if (StiExpressionHelper.IsTimeExpression(textElement.Text) && timer == null)
		{
			timer = new Timer
			{
				Interval = 1000
			};
			timer.Tick += TimeExpressionTimer_Tick;
			timer.Start();
		}
	}

	public override void RefreshStatus(bool checkProgressStatus)
	{
		if (StiOptions.Engine.DashboardTextElementExpressionParser == StiParserType.DataParser)
		{
			base.RefreshStatus(checkProgressStatus);
		}
		else if (base.Element.IsDefined)
		{
			RefreshChild(null);
		}
	}

	protected override Dictionary<string, object> GetConstants()
	{
		string text = (base.Atom as StiTextAtom).Text;
		string value = new StiHtmlTextHelper()?.GetSimpleText(text, StiDashboardStyleHelper.GetForeColor(base.Element));
		return new Dictionary<string, object> { { "Value", value } };
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		base.OnMouseMove(e);
		if (base.IsInteractionSpecified)
		{
			return;
		}
		Cursor @default = Cursors.Default;
		string hyperlinkUnderMouse = (base.Atom as StiTextAtom).GetHyperlinkUnderMouse(e.Location);
		if (hyperlinkUnderMouse != null)
		{
			@default = Cursors.Hand;
			if (toolTipTextHyperlink == null)
			{
				toolTipTextHyperlink = new StiHyperlinkToolTip();
			}
			if (((ToolTip)toolTipTextHyperlink).GetToolTip((Control)(object)this) != hyperlinkUnderMouse)
			{
				((ToolTip)toolTipTextHyperlink).SetToolTip((Control)(object)this, hyperlinkUnderMouse);
			}
		}
		else
		{
			StiHyperlinkToolTip stiHyperlinkToolTip = toolTipTextHyperlink;
			if (stiHyperlinkToolTip != null)
			{
				((ToolTip)stiHyperlinkToolTip).Hide((IWin32Window)(object)this);
			}
			@default = Cursors.Default;
		}
		((Control)this).Cursor = @default;
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		base.OnMouseLeave(e);
		if (!base.IsInteractionSpecified)
		{
			StiHyperlinkToolTip stiHyperlinkToolTip = toolTipTextHyperlink;
			if (stiHyperlinkToolTip != null)
			{
				((ToolTip)stiHyperlinkToolTip).Hide((IWin32Window)(object)this);
			}
			((Control)this).Cursor = Cursors.Default;
		}
	}

	protected override void OnClick(EventArgs e)
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		base.OnClick(e);
		if (base.IsInteractionSpecified)
		{
			return;
		}
		try
		{
			Point mousePosition = ((Control)this).PointToClient(Cursor.Position);
			string hyperlinkUnderMouse = (base.Atom as StiTextAtom).GetHyperlinkUnderMouse(mousePosition);
			if (hyperlinkUnderMouse != null && !StiOptions.Engine.GlobalEvents.InvokeHyperlinkClick(base.Element, hyperlinkUnderMouse))
			{
				StiProcess.Start(hyperlinkUnderMouse);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TimeExpressionTimer_Tick(object sender, EventArgs e)
	{
		base.Atom.Render();
		((Control)this).Invalidate();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			((Component)(object)toolTipTextHyperlink)?.Dispose();
		}
		if (timer != null)
		{
			timer.Tick -= TimeExpressionTimer_Tick;
		}
		base.Dispose(disposing);
	}

	public StiTextElementControl(IStiElement element)
		: base(element)
	{
	}
}
