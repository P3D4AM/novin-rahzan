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
using Stimulsoft.Base.Meters;
using Stimulsoft.Controls;
using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Components.RegionMap;
using Stimulsoft.Dashboard.Drawing.Atoms;
using Stimulsoft.Dashboard.Drawing.Helpers;
using Stimulsoft.Dashboard.Interactions;
using Stimulsoft.Dashboard.Viewer.Controls;
using Stimulsoft.Dashboard.Viewer.Controls.Helpers;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Functions;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Maps;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Dashboard.Viewer.Elements.RegionMap;

[ToolboxItem(false)]
public class StiRegionMapElementControl : StiAtomElementControl
{
	private int moveCount;

	private const float minZoom = 0.2f;

	private const float maxZoom = 5f;

	private StiMapInteractionContainer interactionContainer;

	private StiMapToolTip toolTipMain;

	private StiHyperlinkToolTip toolTipHyperlink;

	private StiInteractiveHtmlToolTip toolTipHtml;

	private StiZoomPlusActionButton buttonZoomPlus;

	private StiZoomMinusActionButton buttonZoomMinus;

	private StiZoomResetActionButton buttonZoomReset;

	private Point lastUpPoint;

	private string columnPath;

	protected StiRegionMapAtom RegionMapAtom => base.Atom as StiRegionMapAtom;

	protected StiRegionMapElement RegionMapElement => base.Element as StiRegionMapElement;

	protected StiDashboardInteraction Interaction => RegionMapElement?.DashboardInteraction as StiDashboardInteraction;

	public string ColumnPath
	{
		get
		{
			if (columnPath != null)
			{
				return columnPath;
			}
			if (RegionMapElement.KeyMeter == null)
			{
				return null;
			}
			StiDataColumn stiDataColumn = StiDataExpressionHelper.GetDataColumnFromExpression(base.Element.Page as StiDashboard, RegionMapElement.KeyMeter.Expression) as StiDataColumn;
			if (stiDataColumn?.DataSource == null)
			{
				return null;
			}
			columnPath = stiDataColumn.DataSource.Name + "." + stiDataColumn.Name;
			return columnPath;
		}
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		((Control)this).OnMouseUp(e);
		moveCount = 0;
		lastUpPoint = default(Point);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		((Control)this).OnMouseDown(e);
		moveCount = 0;
		lastUpPoint = ((Control)this).PointToClient(Cursor.Position);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Invalid comparison between Unknown and I4
		base.OnMouseMove(e);
		if (RegionMapAtom == null)
		{
			return;
		}
		InitInteractionContainer();
		Point pos = ((Control)this).PointToClient(Cursor.Position);
		StiRegionMapZoomHelper.StiMapState stiMapState = StiRegionMapZoomHelper.Get(RegionMapElement);
		string text = interactionContainer?.GetKeyByPosition(pos, stiMapState.LastDrawingZoom, stiMapState.MoveOffset, new RectangleD(0.0, 0.0, ((Control)this).Width, ((Control)this).Height));
		if ((int)e.Button == 1048576)
		{
			moveCount++;
			if (lastUpPoint.X != 0 && lastUpPoint.Y != 0)
			{
				int x;
				int y;
				if (stiMapState.Zoom == 1f)
				{
					x = stiMapState.MoveOffset.X + pos.X - lastUpPoint.X;
					y = stiMapState.MoveOffset.Y + pos.Y - lastUpPoint.Y;
				}
				else
				{
					x = stiMapState.MoveOffset.X + (int)((float)(pos.X - lastUpPoint.X) / stiMapState.Zoom);
					y = stiMapState.MoveOffset.Y + (int)((float)(pos.Y - lastUpPoint.Y) / stiMapState.Zoom);
				}
				stiMapState.MoveOffset = new Point(x, y);
				((Control)this).Refresh();
			}
			lastUpPoint = pos;
		}
		if (RegionMapAtom.MouseOverMapKey == text)
		{
			return;
		}
		RegionMapAtom.MouseOverMapKey = null;
		if (text == null)
		{
			((Control)this).Refresh();
			HideToolTips();
			return;
		}
		IStiDashboardInteraction dashboardInteraction = RegionMapElement.DashboardInteraction;
		if (dashboardInteraction.OnHover == StiInteractionOnHover.ShowToolTip)
		{
			OnMouseMoveOnHover_ShowToolTip(text, dashboardInteraction);
		}
		if (dashboardInteraction.OnClick == StiInteractionOnClick.OpenHyperlink)
		{
			OnMouseMoveOnClick_OpenHyperlink(text);
		}
		if (dashboardInteraction.OnHover == StiInteractionOnHover.ShowHyperlink)
		{
			OnMouseMoveOnClick_ShowHyperlink(text);
		}
	}

	protected override void OnMouseWheel(MouseEventArgs e)
	{
		((ScrollableControl)this).OnMouseWheel(e);
		ChangeZoom((float)e.Delta / 120f * 0.05f);
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		base.OnMouseLeave(e);
		HideToolTips();
	}

	protected override void OnClick(EventArgs e)
	{
		((Control)this).OnClick(e);
		if (moveCount > 4)
		{
			moveCount = 0;
			return;
		}
		IStiDashboardInteraction dashboardInteraction = RegionMapElement.DashboardInteraction;
		switch (dashboardInteraction.OnClick)
		{
		case StiInteractionOnClick.ApplyFilter:
			OnClick_ApplyFilter();
			break;
		case StiInteractionOnClick.OpenHyperlink:
			OnClick_OpenHyperlink();
			break;
		case StiInteractionOnClick.ShowDashboard:
			OnClick_ShowDashboard();
			break;
		case StiInteractionOnClick.DrillDown:
			OnClick_DrillDown();
			break;
		}
	}

	protected override void OnSizeChanged(EventArgs e)
	{
		((Control)this).OnSizeChanged(e);
		if (buttonZoomPlus != null)
		{
			int num = ((Control)buttonZoomPlus).Height * 3 + StiScale.XXI(8.0);
			int num2 = (((Control)this).Height - num) / 2;
			if (num2 < 0)
			{
				((Control)buttonZoomPlus).Visible = false;
				((Control)buttonZoomMinus).Visible = false;
				((Control)buttonZoomReset).Visible = false;
				return;
			}
			((Control)buttonZoomPlus).Left = StiScale.XXI(10.0);
			((Control)buttonZoomMinus).Left = StiScale.XXI(10.0);
			((Control)buttonZoomReset).Left = StiScale.XXI(10.0);
			((Control)buttonZoomPlus).Top = num2;
			num2 += ((Control)buttonZoomPlus).Height + StiScale.XXI(4.0);
			((Control)buttonZoomMinus).Top = num2;
			num2 += ((Control)buttonZoomPlus).Height + StiScale.XXI(4.0);
			((Control)buttonZoomReset).Top = num2;
			((Control)buttonZoomPlus).Visible = true;
			((Control)buttonZoomMinus).Visible = true;
			((Control)buttonZoomReset).Visible = true;
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		base.OnPaint(e);
		base.AnimationEngine?.Continue();
	}

	private void AnimationEngine_Animation(StiComponent component)
	{
		if (RegionMapAtom.Element == component)
		{
			((Control)this).Invalidate();
		}
	}

	private void ChangeZoom(float value)
	{
		StiRegionMapZoomHelper.StiMapState stiMapState = StiRegionMapZoomHelper.Get(RegionMapElement);
		float num = stiMapState.Zoom + value;
		if (num > 5f)
		{
			num = 5f;
		}
		if (num < 0.2f)
		{
			num = 0.2f;
		}
		if (stiMapState.Zoom != num)
		{
			if (buttonZoomPlus != null)
			{
				((Control)buttonZoomPlus).Enabled = num < 5f;
				((Control)buttonZoomMinus).Enabled = num > 0.2f;
			}
			stiMapState.Zoom = num;
			((Control)this).Refresh();
		}
	}

	private Dictionary<string, object> GetConstants(StiMapData data)
	{
		return new Dictionary<string, object>
		{
			{
				"Ident",
				GetToolTipIdent(data)
			},
			{
				"Value",
				(data.Value != null) ? data.Value : ""
			}
		};
	}

	private string GetToolTip(StiMapData data)
	{
		IStiDashboardInteraction dashboardInteraction = RegionMapElement.DashboardInteraction;
		Dictionary<string, object> constants = GetConstants(data);
		return StiReportParser.Parse(dashboardInteraction.ToolTip, base.Element, allowCache: false, constants);
	}

	private string GetHyperlink(StiMapData data)
	{
		IStiDashboardInteraction dashboardInteraction = RegionMapElement.DashboardInteraction;
		if (string.IsNullOrWhiteSpace(dashboardInteraction.Hyperlink))
		{
			return null;
		}
		Dictionary<string, object> constants = GetConstants(data);
		return StiReportParser.Parse(dashboardInteraction.Hyperlink, base.Element, allowCache: false, constants);
	}

	public override void RefreshChild(StiDataTable dataTable)
	{
		List<string> interactiveCheckedState = GetInteractiveCheckedState();
		base.RefreshChild(dataTable);
		if (RegionMapAtom.CheckedMapKeys == null)
		{
			RegionMapAtom.CheckedMapKeys = new List<string>();
		}
		if (interactiveCheckedState != null)
		{
			RegionMapAtom.CheckedMapKeys.AddRange(interactiveCheckedState);
		}
		base.AnimationEngine?.Start();
		columnPath = null;
	}

	public override void PaintAtom(Graphics g)
	{
		if (!base.IsActiveProgress && base.Atom != null && (base.IsInitialized || RegionMapElement.DataFrom != StiMapSource.DataColumns))
		{
			base.Atom.Location = new RectangleD(0.0, 0.0, ((Control)this).Width, ((Control)this).Height);
			base.Atom.Paint(g);
		}
	}

	private static string GetToolTipIdent(StiMapData data)
	{
		if (!string.IsNullOrEmpty(data.Name))
		{
			return data.Name;
		}
		return data.Key;
	}

	private void HideToolTips()
	{
		StiMapToolTip stiMapToolTip = toolTipMain;
		if (stiMapToolTip != null)
		{
			((ToolTip)stiMapToolTip).Hide((IWin32Window)(object)this);
		}
		toolTipHtml?.Hide((Control)(object)this);
		StiHyperlinkToolTip stiHyperlinkToolTip = toolTipHyperlink;
		if (stiHyperlinkToolTip != null)
		{
			((ToolTip)stiHyperlinkToolTip).Hide((IWin32Window)(object)this);
		}
		RegionMapAtom.MouseOverMapKey = null;
	}

	private string GetToolTipValueText(StiMapData data)
	{
		if (RegionMapElement.ShowValue && !string.IsNullOrEmpty(data.Value))
		{
			string text = null;
			if (RegionMapElement.ShortValue && double.TryParse(data.Value, out var result))
			{
				text = StiAbbreviationNumberFormatHelper.Format(result);
			}
			if (text == null)
			{
				text = data.Value;
			}
			return Loc.Get("FormFormatEditor", "BooleanValue").Replace(":", string.Empty) + " = " + text;
		}
		return null;
	}

	private string GetToolTipTotalText(StiMapData data)
	{
		if (RegionMapElement.MapType == StiMapType.Group || RegionMapElement.MapType == StiMapType.HeatmapWithGroup)
		{
			IEnumerable<StiMapData> enumerable = RegionMapAtom.Context?.MapData.Where((StiMapData x) => x.Group == data.Group && x.Value != null);
			double num = 0.0;
			foreach (StiMapData item in enumerable)
			{
				if (double.TryParse(item.Value, out var result))
				{
					num += result;
				}
			}
			string text = StiAbbreviationNumberFormatHelper.Format(num);
			return StiLocalization.Get("PropertyMain", "Total") + " = " + text;
		}
		return null;
	}

	public override void RefreshStatus(bool checkProgressStatus)
	{
		base.RefreshStatus(checkProgressStatus);
		if (base.AnimationEngine != null && !base.AnimationEngine.IsRunning())
		{
			base.AnimationEngine.Start();
			((Control)this).Invalidate();
		}
	}

	private void RefreshFilter()
	{
		if (RegionMapAtom.DataTable == null)
		{
			return;
		}
		string path = ColumnPath;
		IStiMeter item = RegionMapAtom.DataTable.Meters.FirstOrDefault((IStiMeter m) => m.Key == RegionMapElement.KeyMeter.Key);
		int num = RegionMapAtom.DataTable.Meters.IndexOf(item);
		if (num != -1)
		{
			List<string> list = new StiMapKeyHelper().ConvertMapKeysToIsoAlpha2(RegionMapAtom?.CheckedMapKeys, RegionMapElement.MapIdent, RegionMapElement.Language, RegionMapElement.Report);
			IEnumerable<string> enumerable2;
			if (list == null)
			{
				IEnumerable<string> enumerable = new List<string>();
				enumerable2 = enumerable;
			}
			else
			{
				enumerable2 = list.SelectMany(Funcs.GetIso2ConvertedValues);
			}
			IEnumerable<string> enumerable3 = enumerable2;
			RegionMapElement.UserFilters = RegionMapAtom?.CheckedMapKeys.Select((string k) => new StiDataFilterRule(path, StiDataFilterCondition.MapEqualTo, k)).ToList();
			RefreshViewerData();
		}
	}

	public List<string> GetInteractiveCheckedState()
	{
		return RegionMapAtom.CheckedMapKeys.ToList();
	}

	private void InitInteractionContainer()
	{
		if (interactionContainer == null)
		{
			interactionContainer = StiGdiMapContextPainter.RenderInteractionContainer(RegionMapElement.Report, RegionMapElement.MapIdent, RegionMapElement.Language);
		}
	}

	public override void RemoveFilter()
	{
		RegionMapAtom.CheckedMapKeys?.Clear();
	}

	public override void SwitchFilterMultiOption()
	{
		if (!StiActionButtonsHelper.GetFilterMultiOption(base.Element) && RegionMapAtom.CheckedMapKeys != null && RegionMapAtom.CheckedMapKeys.Count > 1)
		{
			RegionMapAtom.CheckedMapKeys = RegionMapAtom.CheckedMapKeys.Take(1).ToList();
			RefreshFilter();
		}
	}

	private void OnMouseMoveOnHover_ShowToolTip(string key, IStiDashboardInteraction interaction)
	{
		RegionMapAtom.MouseOverMapKey = key;
		((Control)this).Refresh();
		StiMapData stiMapData = RegionMapAtom.Context?.MapData?.FirstOrDefault((StiMapData x) => x.Key == key);
		if (stiMapData == null)
		{
			HideToolTips();
			return;
		}
		if (!string.IsNullOrWhiteSpace(interaction.ToolTip))
		{
			if (toolTipHtml == null)
			{
				toolTipHtml = new StiInteractiveHtmlToolTip();
			}
			string toolTip = GetToolTip(stiMapData);
			if (toolTipHtml.HtmlText != toolTip)
			{
				toolTipHtml.SetHtmlToolTip((Control)(object)this, toolTip);
			}
			return;
		}
		if (toolTipMain == null)
		{
			toolTipMain = new StiMapToolTip();
		}
		toolTipMain.ColorState = ((StiRegionMapAtom)base.Atom).Context.GetGeomBrush(stiMapData);
		toolTipMain.StateName = GetToolTipIdent(stiMapData);
		toolTipMain.ValueText = GetToolTipValueText(stiMapData);
		toolTipMain.TotalText = GetToolTipTotalText(stiMapData);
		if (((ToolTip)toolTipMain).GetToolTip((Control)(object)this) != key)
		{
			((ToolTip)toolTipMain).SetToolTip((Control)(object)this, key);
		}
	}

	private void OnMouseMoveOnClick_OpenHyperlink(string key)
	{
		StiMapData stiMapData = RegionMapAtom.Context?.MapData?.FirstOrDefault((StiMapData x) => x.Key == key);
		if (stiMapData != null)
		{
			string hyperlink = GetHyperlink(stiMapData);
			((Control)this).Cursor = ((!string.IsNullOrWhiteSpace(hyperlink)) ? Cursors.Hand : Cursors.Default);
		}
		else
		{
			((Control)this).Cursor = Cursors.Default;
		}
	}

	private void OnMouseMoveOnClick_ShowHyperlink(string key)
	{
		StiMapData stiMapData = RegionMapAtom.Context?.MapData?.FirstOrDefault((StiMapData x) => x.Key == key);
		if (stiMapData != null)
		{
			string hyperlink = GetHyperlink(stiMapData);
			if (!string.IsNullOrWhiteSpace(hyperlink))
			{
				if (toolTipHyperlink == null)
				{
					toolTipHyperlink = new StiHyperlinkToolTip();
				}
				if (((ToolTip)toolTipHyperlink).GetToolTip((Control)(object)this) != hyperlink)
				{
					((ToolTip)toolTipHyperlink).SetToolTip((Control)(object)this, hyperlink);
				}
				return;
			}
		}
		HideToolTips();
	}

	private void OnClick_ApplyFilter()
	{
		if (ColumnPath == null)
		{
			return;
		}
		InitInteractionContainer();
		Point pos = ((Control)this).PointToClient(Cursor.Position);
		StiRegionMapZoomHelper.StiMapState stiMapState = StiRegionMapZoomHelper.Get(RegionMapElement);
		string keyByPosition = interactionContainer.GetKeyByPosition(pos, stiMapState.LastDrawingZoom, stiMapState.MoveOffset, new RectangleD(0.0, 0.0, ((Control)this).Width, ((Control)this).Height));
		if (keyByPosition == null)
		{
			return;
		}
		if (StiActionButtonsHelper.GetFilterMultiOption(base.Element))
		{
			if (RegionMapAtom.CheckedMapKeys.Contains(keyByPosition))
			{
				RegionMapAtom.CheckedMapKeys.Remove(keyByPosition);
			}
			else
			{
				RegionMapAtom.CheckedMapKeys.Add(keyByPosition);
			}
		}
		else if (RegionMapAtom.CheckedMapKeys.Contains(keyByPosition) && RegionMapAtom.CheckedMapKeys.Count == 1)
		{
			RegionMapAtom.CheckedMapKeys.Remove(keyByPosition);
		}
		else
		{
			RegionMapAtom.CheckedMapKeys.Clear();
			RegionMapAtom.CheckedMapKeys.Add(keyByPosition);
		}
		((Control)this).Invalidate();
		RefreshFilter();
	}

	private void OnClick_OpenHyperlink()
	{
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		Point pos = ((Control)this).PointToClient(Cursor.Position);
		StiRegionMapZoomHelper.StiMapState stiMapState = StiRegionMapZoomHelper.Get(RegionMapElement);
		string key = interactionContainer.GetKeyByPosition(pos, stiMapState.LastDrawingZoom, stiMapState.MoveOffset, new RectangleD(0.0, 0.0, ((Control)this).Width, ((Control)this).Height));
		if (key == null)
		{
			return;
		}
		StiMapData stiMapData = RegionMapAtom.Context?.MapData?.FirstOrDefault((StiMapData x) => x.Key == key);
		if (stiMapData == null)
		{
			return;
		}
		try
		{
			string hyperlink = GetHyperlink(stiMapData);
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

	private void OnClick_ShowDashboard()
	{
		Point pos = ((Control)this).PointToClient(Cursor.Position);
		StiRegionMapZoomHelper.StiMapState stiMapState = StiRegionMapZoomHelper.Get(RegionMapElement);
		string key = interactionContainer.GetKeyByPosition(pos, stiMapState.LastDrawingZoom, stiMapState.MoveOffset, new RectangleD(0.0, 0.0, ((Control)this).Width, ((Control)this).Height));
		if (key == null)
		{
			return;
		}
		StiMapData stiMapData = RegionMapAtom.Context?.MapData?.FirstOrDefault((StiMapData x) => x.Key == key);
		if (stiMapData == null)
		{
			return;
		}
		StiPage stiPage = base.Element.Report.Pages.ToList().FirstOrDefault((StiPage p) => p.Guid == Interaction.DrillDownPageKey);
		if (stiPage != null)
		{
			Dictionary<string, object> constants = GetConstants(stiMapData);
			List<DictionaryEntry> parameters = Interaction.DrillDownParameters.Select((StiDashboardDrillDownParameter d) => new DictionaryEntry(d.Name, StiReportParser.Parse(d.Expression, base.Element, allowCache: false, constants))).ToList();
			NewTab(stiPage, stiMapData.Key, parameters);
		}
	}

	private void OnClick_DrillDown()
	{
	}

	private void ButtonZoomPlus_Click(object sender, EventArgs e)
	{
		ChangeZoom(0.1f);
	}

	private void ButtonZoomMinus_Click(object sender, EventArgs e)
	{
		ChangeZoom(-0.1f);
	}

	private void ButtonZoomReset_Click(object sender, EventArgs e)
	{
		StiRegionMapZoomHelper.StiMapState stiMapState = StiRegionMapZoomHelper.Get(RegionMapElement);
		stiMapState.Zoom = 1f;
		stiMapState.MoveOffset = new Point(0, 0);
		if (buttonZoomPlus != null)
		{
			((Control)buttonZoomPlus).Enabled = true;
			((Control)buttonZoomMinus).Enabled = true;
		}
		((Control)this).Refresh();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			interactionContainer?.Dispose();
			((Component)(object)toolTipMain)?.Dispose();
			((Component)(object)toolTipHtml)?.Dispose();
			((Component)(object)toolTipHyperlink)?.Dispose();
			if (buttonZoomPlus != null)
			{
				((Control)buttonZoomPlus).Click -= ButtonZoomPlus_Click;
			}
			if (buttonZoomMinus != null)
			{
				((Control)buttonZoomMinus).Click -= ButtonZoomMinus_Click;
			}
			if (buttonZoomReset != null)
			{
				((Control)buttonZoomReset).Click -= ButtonZoomReset_Click;
			}
			if (base.AnimationEngine != null)
			{
				base.AnimationEngine.Animation -= AnimationEngine_Animation;
				base.AnimationEngine.Dispose();
			}
		}
		base.Dispose(disposing);
	}

	public StiRegionMapElementControl(IStiElement element)
		: base(element)
	{
		if (StiOptions.Viewer.Map.ShowZoomPanel)
		{
			StiZoomPlusActionButton stiZoomPlusActionButton = new StiZoomPlusActionButton(StiControlColorCollection.GetActionColors(element), default(Point));
			((Control)stiZoomPlusActionButton).Visible = false;
			buttonZoomPlus = stiZoomPlusActionButton;
			((Control)buttonZoomPlus).Click += ButtonZoomPlus_Click;
			StiZoomMinusActionButton stiZoomMinusActionButton = new StiZoomMinusActionButton(StiControlColorCollection.GetActionColors(element), default(Point));
			((Control)stiZoomMinusActionButton).Visible = false;
			buttonZoomMinus = stiZoomMinusActionButton;
			((Control)buttonZoomMinus).Click += ButtonZoomMinus_Click;
			StiZoomResetActionButton stiZoomResetActionButton = new StiZoomResetActionButton(StiControlColorCollection.GetActionColors(element), default(Point));
			((Control)stiZoomResetActionButton).Visible = false;
			buttonZoomReset = stiZoomResetActionButton;
			((Control)buttonZoomReset).Click += ButtonZoomReset_Click;
			((Control)this).Controls.Add((Control)(object)buttonZoomPlus);
			((Control)this).Controls.Add((Control)(object)buttonZoomMinus);
			((Control)this).Controls.Add((Control)(object)buttonZoomReset);
		}
		base.AnimationEngine.Animation += AnimationEngine_Animation;
		base.AnimationEngine.Start();
	}
}
