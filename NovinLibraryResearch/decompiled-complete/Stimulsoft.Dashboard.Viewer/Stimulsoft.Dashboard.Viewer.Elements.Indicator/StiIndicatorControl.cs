using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Dashboard.Drawing.Atoms;
using Stimulsoft.Dashboard.Drawing.Painters;
using Stimulsoft.Dashboard.Interactions;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Helpers;

namespace Stimulsoft.Dashboard.Viewer.Elements.Indicator;

[ToolboxItem(false)]
public class StiIndicatorControl : StiAtomElementControl
{
	private StiHyperlinkToolTip toolTipHyperlink;

	private StiInteractiveHtmlToolTip toolTipHtml;

	public StiIndicatorAtom IndicatorAtom => base.Atom as StiIndicatorAtom;

	public IStiIndicatorElement IndicatorElement => base.Atom.Element as IStiIndicatorElement;

	protected string ToolTipString { get; set; }

	protected string HyperlinkString { get; set; }

	protected StiDashboardInteraction Interaction => (base.Element as IStiElementInteraction)?.DashboardInteraction as StiDashboardInteraction;

	protected override void OnPaint(PaintEventArgs e)
	{
		PaintAtom(e.Graphics);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		Point point = ((Control)this).PointToClient(Cursor.Position);
		StiIndicatorElementMouseOverHelper.SetMouseOverPoint(IndicatorElement, new PointD(point.X, point.Y));
		HyperlinkString = GetHyperlink();
		ToolTipString = GetToolTip();
		((Control)this).Invalidate();
		base.OnMouseMove(e);
		if (Interaction == null || (Interaction.OnClick == StiInteractionOnClick.None && Interaction.OnHover == StiInteractionOnHover.None))
		{
			return;
		}
		if (Interaction.OnHover == StiInteractionOnHover.ShowToolTip)
		{
			string text = ((!string.IsNullOrEmpty(ToolTipString)) ? ToolTipString : "");
			if (!string.IsNullOrEmpty(text))
			{
				if (!string.IsNullOrWhiteSpace(Interaction.ToolTip))
				{
					if (toolTipHtml == null)
					{
						toolTipHtml = new StiInteractiveHtmlToolTip();
					}
					if (toolTipHtml.HtmlText != text)
					{
						toolTipHtml.SetHtmlToolTip((Control)(object)this, text);
					}
				}
			}
			else
			{
				toolTipHtml?.Hide((Control)(object)this);
			}
		}
		string text2 = ((!string.IsNullOrEmpty(HyperlinkString)) ? HyperlinkString : "");
		if (Interaction.OnClick == StiInteractionOnClick.OpenHyperlink && !string.IsNullOrWhiteSpace(text2))
		{
			((Control)this).Cursor = Cursors.Hand;
		}
		if (Interaction.OnClick == StiInteractionOnClick.ShowDashboard)
		{
			((Control)this).Cursor = Cursors.Hand;
		}
		if (Interaction.OnHover == StiInteractionOnHover.ShowHyperlink && !string.IsNullOrWhiteSpace(text2))
		{
			if (toolTipHyperlink == null)
			{
				toolTipHyperlink = new StiHyperlinkToolTip();
			}
			if (((ToolTip)toolTipHyperlink).GetToolTip((Control)(object)this) != text2)
			{
				((ToolTip)toolTipHyperlink).SetToolTip((Control)(object)this, text2);
			}
		}
	}

	protected override void OnClick(EventArgs e)
	{
		((Control)this).OnClick(e);
		if (Interaction != null)
		{
			switch (Interaction.OnClick)
			{
			case StiInteractionOnClick.ShowDashboard:
				OnClick_ShowDashboard();
				break;
			case StiInteractionOnClick.OpenHyperlink:
				OnClick_OpenHyperlink();
				break;
			}
			((Control)this).Invalidate();
		}
	}

	private void OnClick_ShowDashboard()
	{
		if (string.IsNullOrWhiteSpace(Interaction.DrillDownPageKey))
		{
			return;
		}
		StiPage stiPage = base.Element.Report.Pages.ToList().FirstOrDefault((StiPage p) => p.Guid == Interaction.DrillDownPageKey);
		if (stiPage != null)
		{
			Dictionary<string, object> constants = GetConstants();
			List<DictionaryEntry> parameters = Interaction.DrillDownParameters.Select((StiDashboardDrillDownParameter d) => new DictionaryEntry(d.Name, StiReportParser.Parse(d.Expression, base.Element, allowCache: false, constants))).ToList();
			StiComponent stiComponent = base.Element as StiComponent;
			string value = (string.IsNullOrWhiteSpace(stiComponent.Alias) ? stiComponent.Name : stiComponent.Alias);
			NewTab(stiPage, value, parameters);
		}
	}

	private void OnClick_OpenHyperlink()
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			string hyperlink = GetHyperlink();
			if (!string.IsNullOrEmpty(hyperlink) && !StiOptions.Engine.GlobalEvents.InvokeHyperlinkClick(base.Element, hyperlink))
			{
				StiProcess.Start(hyperlink);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show((IWin32Window)(object)((Control)this).FindForm(), ex.Message, Loc.Get("FormViewer", "title"));
		}
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		base.OnMouseLeave(e);
		toolTipHtml?.Hide((Control)(object)this);
		StiHyperlinkToolTip stiHyperlinkToolTip = toolTipHyperlink;
		if (stiHyperlinkToolTip != null)
		{
			((ToolTip)stiHyperlinkToolTip).Hide((IWin32Window)(object)this);
		}
		((Control)this).Cursor = Cursors.Default;
		StiIndicatorElementMouseOverHelper.ResetMouseOver(IndicatorElement);
		((Control)this).Invalidate();
	}

	public override void PaintAtom(Graphics g)
	{
		if (base.Atom != null)
		{
			StiElementGdiPainter stiElementGdiPainter = new StiElementGdiPainter();
			if (!stiElementGdiPainter.IsNoResult(base.Atom.Element, base.Atom.DataTable) && !stiElementGdiPainter.IsEmptyData(base.Atom.Element, base.Atom.DataTable))
			{
				base.Atom.Location = new RectangleD(0.0, 0.0, ((Control)this).Width, ((Control)this).Height);
				base.Atom.Paint(g);
			}
		}
	}

	public override void RefreshChild(StiDataTable dataTable)
	{
		base.Atom = StiElementAtom.Get(new RectangleD(0.0, 0.0, ((Control)this).Width, ((Control)this).Height), base.Element, dataTable) as StiIndicatorAtom;
	}

	public void RecalculateSize(Rectangle rect)
	{
		if (base.Atom != null)
		{
			RectangleF contentRectangle = IndicatorAtom.GetContentRectangle(RectangleD.CreateFromRectangle(rect));
			((Control)this).Width = (int)contentRectangle.Width;
			((Control)this).Height = (int)contentRectangle.Height;
		}
	}

	protected Dictionary<string, object> GetConstants()
	{
		StiIndicatorElementMouseOverData mouseOverData = StiIndicatorElementMouseOverHelper.GetMouseOverData(IndicatorElement);
		if (mouseOverData == null)
		{
			return null;
		}
		return new Dictionary<string, object>
		{
			{ "Series", mouseOverData.Series },
			{ "Target", mouseOverData.Target },
			{ "Value", mouseOverData.Value }
		};
	}

	private string GetHyperlink()
	{
		if (string.IsNullOrWhiteSpace(Interaction.Hyperlink))
		{
			return null;
		}
		Dictionary<string, object> constants = GetConstants();
		if (constants == null)
		{
			return null;
		}
		return StiReportParser.Parse(Interaction.Hyperlink, base.Element, allowCache: false, constants);
	}

	private string GetToolTip()
	{
		if (string.IsNullOrWhiteSpace(Interaction.ToolTip))
		{
			return null;
		}
		Dictionary<string, object> constants = GetConstants();
		if (constants == null)
		{
			return null;
		}
		return StiReportParser.Parse(Interaction.ToolTip, base.Element, allowCache: false, constants);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			((Component)(object)toolTipHyperlink)?.Dispose();
			((Component)(object)toolTipHtml)?.Dispose();
		}
		base.Dispose(disposing);
	}

	public StiIndicatorControl(IStiElement element)
		: base(element)
	{
	}
}
