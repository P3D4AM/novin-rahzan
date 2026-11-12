using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Dashboard.Interactions;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Viewer.Elements;

[ToolboxItem(false)]
public abstract class StiSimpleAtomElementControl : StiAtomElementControl
{
	private StiHyperlinkToolTip toolTipHyperlink;

	private StiInteractiveHtmlToolTip toolTipHtml;

	protected StiDashboardInteraction Interaction => (base.Element as IStiElementInteraction)?.DashboardInteraction as StiDashboardInteraction;

	private string ToolTipString { get; set; }

	private string HyperlinkString { get; set; }

	protected bool IsInteractionSpecified
	{
		get
		{
			if (Interaction.OnClick != StiInteractionOnClick.OpenHyperlink || string.IsNullOrWhiteSpace(HyperlinkString))
			{
				return Interaction.OnClick == StiInteractionOnClick.ShowDashboard;
			}
			return true;
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
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
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			string text = StiReportParser.Parse(Interaction?.Hyperlink, base.Element);
			if (!string.IsNullOrEmpty(text) && !StiOptions.Engine.GlobalEvents.InvokeHyperlinkClick(base.Element, text))
			{
				StiProcess.Start(text);
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
	}

	protected virtual Dictionary<string, object> GetConstants()
	{
		return new Dictionary<string, object>();
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

	public StiSimpleAtomElementControl(IStiElement element)
		: base(element)
	{
		ToolTipString = StiReportParser.Parse(Interaction.ToolTip, element);
		HyperlinkString = StiReportParser.Parse(Interaction.Hyperlink, element);
	}
}
