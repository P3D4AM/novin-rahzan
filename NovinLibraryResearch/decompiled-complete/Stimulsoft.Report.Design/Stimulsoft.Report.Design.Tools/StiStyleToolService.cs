using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Design.Tools;

[StiServiceBitmap(typeof(StiStyleToolService), "Stimulsoft.Report.Design.Bmp.Tools.Style.bmp")]
public class StiStyleToolService : StiToolService
{
	private static Cursor cursorGetStyle;

	private static Cursor cursorSetStyle;

	public override string ServiceName => Loc.Get("Toolbox", "Style");

	public override int Position => 5;

	public override Keys Shortcut => (Keys)117;

	public StiStyleToolAction Action { get; set; }

	public static Cursor CursorGetStyle
	{
		get
		{
			if (cursorGetStyle == (Cursor)null)
			{
				cursorGetStyle = StiCursorUtils.GetCursor("Stimulsoft.Report", "Stimulsoft.Report.Cursors.StyleGet.cur");
			}
			return cursorGetStyle;
		}
	}

	public static Cursor CursorSetStyle
	{
		get
		{
			if (cursorSetStyle == (Cursor)null)
			{
				cursorSetStyle = StiCursorUtils.GetCursor("Stimulsoft.Report", "Stimulsoft.Report.Cursors.StyleSet.cur");
			}
			return cursorSetStyle;
		}
	}

	protected override void OnActivated(EventArgs e)
	{
		base.OnActivated(e);
		Action = StiStyleToolAction.SetStyle;
		StiComponentsCollection selectedComponents = base.CurrentPage.GetSelectedComponents();
		if (selectedComponents.Count > 0)
		{
			StiComponent stiComponent = selectedComponents[0];
			if (stiComponent is IStiDashboardElementStyle stiDashboardElementStyle)
			{
				Designer.CurrentDashboardStyleIdent = stiDashboardElementStyle.Style;
				Designer.CurrentDashboardCustomStyleName = stiDashboardElementStyle.CustomStyleName;
				Designer.CurrentDashboardElementType = stiComponent.GetType();
			}
			Designer.CurrentStyle = StiBaseStyle.GetStyle(stiComponent);
			(stiComponent as IStiCopyStyleExt)?.InvokeGetCopyStyle();
			Action = StiStyleToolAction.SetStyle;
		}
	}

	public override void OnClick(object sender, EventArgs e)
	{
		base.OnClick(sender, e);
		if (Action == StiStyleToolAction.GetStyle)
		{
			Point point = ((Control)Designer.CurrentPageView.View).PointToClient(Cursor.Position);
			StiComponent componentInPoint = base.CurrentPage.GetComponentInPoint(Designer.XToPage(point.X), Designer.YToPage(point.Y));
			if (componentInPoint != null && !(componentInPoint is StiPage))
			{
				if (componentInPoint is IStiDashboardElementStyle)
				{
					Designer.CurrentDashboardStyleIdent = ((IStiDashboardElementStyle)componentInPoint).Style;
					Designer.CurrentDashboardCustomStyleName = ((IStiDashboardElementStyle)componentInPoint).CustomStyleName;
					Designer.CurrentDashboardElementType = componentInPoint.GetType();
				}
				Designer.CurrentStyle = StiBaseStyle.GetStyle(componentInPoint);
				(componentInPoint as IStiCopyStyleExt)?.InvokeGetCopyStyle();
				Action = StiStyleToolAction.SetStyle;
			}
			return;
		}
		Designer.UndoRedoSave(Loc.Get("Toolbox", "Style"), onlyCurrentPage: true);
		Point point2 = ((Control)Designer.CurrentPageView.View).PointToClient(Cursor.Position);
		StiComponent componentInPoint2 = base.CurrentPage.GetComponentInPoint(Designer.XToPage(point2.X), Designer.YToPage(point2.Y));
		if (componentInPoint2 == null || componentInPoint2 is StiPage)
		{
			return;
		}
		StiComponentsCollection selectedComponentsOnPage = Designer.SelectedComponentsOnPage;
		if (selectedComponentsOnPage != null && selectedComponentsOnPage.Count > 0 && !string.IsNullOrEmpty(selectedComponentsOnPage[0].ComponentStyle))
		{
			componentInPoint2.ComponentStyle = selectedComponentsOnPage[0].ComponentStyle;
		}
		else
		{
			componentInPoint2.ComponentStyle = string.Empty;
			if (componentInPoint2 is IStiDashboardElementStyle stiDashboardElementStyle)
			{
				if (Designer.CurrentDashboardStyleIdent == StiElementStyleIdent.Custom)
				{
					if (componentInPoint2.GetType() == Designer.CurrentDashboardElementType)
					{
						stiDashboardElementStyle.Style = StiElementStyleIdent.Custom;
						stiDashboardElementStyle.CustomStyleName = Designer.CurrentDashboardCustomStyleName;
					}
				}
				else
				{
					stiDashboardElementStyle.Style = Designer.CurrentDashboardStyleIdent;
					stiDashboardElementStyle.CustomStyleName = null;
				}
			}
			Designer.SetStyleToComponent(componentInPoint2);
			(componentInPoint2 as IStiCopyStyleExt)?.InvokeSetCopyStyle();
		}
		((Control)Designer).Refresh();
	}

	public static StiStyleToolService GetStyleToolService()
	{
		return GetStyleToolService(null);
	}

	public static StiStyleToolService GetStyleToolService(StiDesignerControl designer)
	{
		if (designer == null)
		{
			return StiDesignerOptions.Tools.FirstOrDefault((StiToolService c) => c.GetType() == typeof(StiStyleToolService)) as StiStyleToolService;
		}
		return designer.Services.GetServices(typeof(StiToolService)).OfType<StiStyleToolService>().FirstOrDefault();
	}

	public override Cursor GetCursor()
	{
		if (Action != 0)
		{
			return CursorSetStyle;
		}
		return CursorGetStyle;
	}
}
