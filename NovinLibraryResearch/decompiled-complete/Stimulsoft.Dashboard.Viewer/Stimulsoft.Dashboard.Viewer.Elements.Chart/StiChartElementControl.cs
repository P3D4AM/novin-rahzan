using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Dashboard.Components.Chart;
using Stimulsoft.Dashboard.Drawing.Atoms;
using Stimulsoft.Dashboard.Drawing.Helpers;
using Stimulsoft.Dashboard.Drawing.Painters;
using Stimulsoft.Dashboard.Interactions;
using Stimulsoft.Dashboard.Viewer.Controls.Helpers;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Chart.Geoms.Series.Pie;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Engine;

namespace Stimulsoft.Dashboard.Viewer.Elements.Chart;

[ToolboxItem(false)]
public class StiChartElementControl : StiAtomElementControl
{
	private Hashtable chartComponentToInteraction = new Hashtable();

	private Timer chartInteractionRecallTimer;

	private StiChartElement chartInteractionLatestChart;

	private Point chartInteractionPoint;

	private RectangleD chartInteractionRect;

	private StiChartToolTip toolTipMain;

	private StiHyperlinkToolTip toolTipHyperlink;

	private StiInteractiveHtmlToolTip toolTipHtml;

	private Point startPoint;

	internal StiChartElement ChartElement => base.Element as StiChartElement;

	protected StiChartDashboardInteraction Interaction => ChartElement?.DashboardInteraction as StiChartDashboardInteraction;

	protected StiChartAtom ChartAtom => base.Atom as StiChartAtom;

	internal bool IsPie3d => ChartAtom.Chart.Series.OfType<StiPie3dSeries>().Count() > 0;

	private bool IsAllowInteractive
	{
		get
		{
			if (ChartElement.Arguments.FirstOrDefault() == null)
			{
				return ChartElement.Series != null;
			}
			return true;
		}
	}

	private bool IsDrillAvailable => StiDrillDownHelper.IsDrillAvailable(base.Element);

	private bool IsDrillDownAvailable => StiDrillDownHelper.IsDrillDownAvailable(base.Element);

	protected override void OnMouseMove(MouseEventArgs e)
	{
		//IL_0393: Unknown result type (might be due to invalid IL or missing references)
		//IL_039d: Invalid comparison between Unknown and I4
		base.OnMouseMove(e);
		if (Interaction == null || (Interaction.OnClick == StiInteractionOnClick.None && Interaction.OnHover == StiInteractionOnHover.None))
		{
			return;
		}
		StiChartInteractionResult stiChartInteractionResult = ProcessChartInteractionOldLeaveNewEnter();
		string text = ((!string.IsNullOrEmpty(stiChartInteractionResult.ToolTipString)) ? stiChartInteractionResult.ToolTipString : "");
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
			else
			{
				try
				{
					JToken source = JToken.Parse(text);
					JToken value = source.Cast<KeyValuePair<string, JToken>>().FirstOrDefault((KeyValuePair<string, JToken> a) => a.Key == "StiColor").Value;
					JToken value2 = source.Cast<KeyValuePair<string, JToken>>().FirstOrDefault((KeyValuePair<string, JToken> a) => a.Key == "StiValue").Value;
					JToken value3 = source.Cast<KeyValuePair<string, JToken>>().FirstOrDefault((KeyValuePair<string, JToken> a) => a.Key == "StiArgument").Value;
					JToken value4 = source.Cast<KeyValuePair<string, JToken>>().FirstOrDefault((KeyValuePair<string, JToken> a) => a.Key == "StiWeight").Value;
					if (toolTipMain == null)
					{
						toolTipMain = new StiChartToolTip();
					}
					if (value2 != null)
					{
						toolTipMain.Value = value2.ToString();
					}
					if (value3 != null)
					{
						toolTipMain.Argument = value3.ToString();
					}
					if (value4 != null)
					{
						toolTipMain.Weight = value4.ToString();
					}
					if (value != null)
					{
						toolTipMain.ColorSeries = ColorTranslator.FromHtml(value.ToString());
					}
					if (value4 != null)
					{
						toolTipMain.Weight = Loc.Get("PropertyMain", "Weight") + ": " + toolTipMain.Weight;
						toolTipMain.Argument = Loc.Get("PropertyMain", "Argument") + ": " + toolTipMain.Argument;
					}
					if (((ToolTip)toolTipMain).GetToolTip((Control)(object)this) != text)
					{
						((ToolTip)toolTipMain).SetToolTip((Control)(object)this, text);
					}
				}
				catch
				{
					return;
				}
			}
		}
		else
		{
			StiChartToolTip stiChartToolTip = toolTipMain;
			if (stiChartToolTip != null)
			{
				((ToolTip)stiChartToolTip).Hide((IWin32Window)(object)this);
			}
			toolTipHtml?.Hide((Control)(object)this);
		}
		bool flag = true;
		string text2 = ((!string.IsNullOrEmpty(stiChartInteractionResult.HyperlinkString)) ? stiChartInteractionResult.HyperlinkString : "");
		if (Interaction.OnClick == StiInteractionOnClick.OpenHyperlink)
		{
			((Control)this).Cursor = ((!string.IsNullOrWhiteSpace(text2)) ? Cursors.Hand : Cursors.Default);
			flag = false;
		}
		if ((Interaction.OnClick == StiInteractionOnClick.ShowDashboard || Interaction.OnClick == StiInteractionOnClick.ApplyFilter || (Interaction.OnClick == StiInteractionOnClick.DrillDown && IsDrillAvailable) || IsDrillAvailable) && stiChartInteractionResult.Geom != null)
		{
			((Control)this).Cursor = Cursors.Hand;
			flag = false;
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
		if (flag)
		{
			((Control)this).Cursor = Cursors.Default;
		}
		if ((int)e.Button == 1048576)
		{
			_ = startPoint;
			float num = (float)(startPoint.X - e.X) / (float)StiScale.Factor;
			StiPie3dSeries stiPie3dSeries = ChartAtom.Chart.Series.OfType<StiPie3dSeries>().FirstOrDefault();
			if (stiPie3dSeries != null && num != 0f)
			{
				startPoint.X = e.X;
				stiPie3dSeries.StartAngle += num;
				((Control)this).Invalidate();
				StiChartInteractionHelper.ResetChartGeomCache();
			}
		}
	}

	protected override void OnMouseWheel(MouseEventArgs e)
	{
		((ScrollableControl)this).OnMouseWheel(e);
		StiPie3dSeries stiPie3dSeries = ChartAtom.Chart.Series.OfType<StiPie3dSeries>().FirstOrDefault();
		if (stiPie3dSeries != null)
		{
			stiPie3dSeries.StartAngle += e.Delta / 70;
			((Control)this).Invalidate();
			StiChartInteractionHelper.ResetChartGeomCache();
		}
	}

	protected override void OnClick(EventArgs e)
	{
		((Control)this).OnClick(e);
		if (((Control)this).Parent is StiHolderElementControl { IsDrillSelectionActivated: not false })
		{
			ProcessChartInteractionClick(selectOnClick: true);
		}
		else if ((IsAllowInteractive && Interaction != null) || (Interaction != null && !string.IsNullOrEmpty(Interaction.DrillDownPageKey)))
		{
			switch (Interaction.OnClick)
			{
			case StiInteractionOnClick.ApplyFilter:
				OnClick_ApplyFilter();
				break;
			case StiInteractionOnClick.ShowDashboard:
				OnClick_ShowDashboard();
				break;
			case StiInteractionOnClick.OpenHyperlink:
				OnClick_OpenHyperlink();
				break;
			case StiInteractionOnClick.DrillDown:
				OnClick_DrillDown();
				break;
			}
		}
	}

	private void OnClick_ApplyFilter()
	{
		StiChartInteractionResult stiChartInteractionResult = ProcessChartInteractionClick(selectOnClick: true);
		if (!StiActionButtonsHelper.GetFilterMultiOption(base.Element))
		{
			RemoveFilter(stiChartInteractionResult.Geom);
		}
		RefreshFilter();
	}

	private void OnClick_ShowDashboard()
	{
		StiChartInteractionResult stiChartInteractionResult = ProcessChartInteractionClick(selectOnClick: false);
		if (stiChartInteractionResult?.InteractionData == null || string.IsNullOrWhiteSpace(Interaction.DrillDownPageKey))
		{
			return;
		}
		StiPage stiPage = base.Element.Report.Pages.ToList().FirstOrDefault((StiPage p) => p.Guid == Interaction.DrillDownPageKey);
		if (stiPage != null)
		{
			Dictionary<string, object> constants = GetConstants(stiChartInteractionResult);
			List<DictionaryEntry> list = Interaction.DrillDownParameters.Select((StiDashboardDrillDownParameter d) => new DictionaryEntry(d.Name, StiReportParser.Parse(d.Expression, base.Element, allowCache: false, constants))).ToList();
			string value = stiChartInteractionResult.InteractionData.Argument.ToString();
			List<DictionaryEntry> list2 = list.Where((DictionaryEntry p) => p.Key?.ToString().ToLowerInvariant() == "reportalias").ToList();
			if (list2.Count > 0)
			{
				value = list2[0].Value.ToString();
			}
			NewTab(stiPage, value, list);
		}
	}

	private void OnClick_OpenHyperlink()
	{
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			StiChartInteractionResult stiChartInteractionResult = ProcessChartInteractionClick(selectOnClick: false);
			if (!string.IsNullOrEmpty(stiChartInteractionResult?.HyperlinkString) && !StiOptions.Engine.GlobalEvents.InvokeHyperlinkClick(base.Element, stiChartInteractionResult?.HyperlinkString))
			{
				StiProcess.Start(stiChartInteractionResult.HyperlinkString);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show((IWin32Window)(object)((Control)this).FindForm(), ex.Message, Loc.Get("FormViewer", "title"));
		}
	}

	private void OnClick_DrillDown()
	{
		if (IsDrillDownAvailable && ProcessChartInteractionClick(selectOnClick: true)?.InteractionData != null)
		{
			StiDrillDownHelper.DrillDown(ChartElement, GetFilters(isDrillDown: true));
			RemoveFilter();
			RefreshViewer();
		}
	}

	private void AnimationEngine_Animation(StiComponent component)
	{
		if (ChartAtom.Chart == component)
		{
			((Control)this).Invalidate();
		}
	}

	private void AnimationEngine_ReverseEnd()
	{
		base.AnimationEngine.Speed = 0.5;
		RefreshChild(base.AnimationEngine.Tag as StiDataTable);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		((Control)this).OnMouseDown(e);
		startPoint = e.Location;
		Point pts = ((Control)this).PointToClient(Cursor.Position);
		RectangleD contentRect = GetContentRect();
		ProcessChartInteractionMouseDown(pts, ChartElement, contentRect, calledFromTimer: false);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		((Control)this).OnMouseUp(e);
		ProcessChartInteractionMouseUp();
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		base.OnMouseLeave(e);
		ProcessChartInteractionMouseLeave();
		StiChartToolTip stiChartToolTip = toolTipMain;
		if (stiChartToolTip != null)
		{
			((ToolTip)stiChartToolTip).Hide((IWin32Window)(object)this);
		}
		toolTipHtml?.Hide((Control)(object)this);
		StiHyperlinkToolTip stiHyperlinkToolTip = toolTipHyperlink;
		if (stiHyperlinkToolTip != null)
		{
			((ToolTip)stiHyperlinkToolTip).Hide((IWin32Window)(object)this);
		}
		((Control)this).Cursor = Cursors.Default;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		base.OnPaint(e);
		base.AnimationEngine.Continue();
	}

	private Dictionary<string, object> GetConstants(StiChartInteractionResult chartInteraction)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("Argument", chartInteraction.InteractionData.Argument);
		dictionary.Add("Value", chartInteraction.InteractionData.Value);
		dictionary.Add("EndValue", chartInteraction.InteractionData.EndValue);
		dictionary.Add("Series", chartInteraction.InteractionData.Series);
		return dictionary;
	}

	public override void RefreshChild(StiDataTable dataTable)
	{
		if (base.Child != null)
		{
			StiElementGdiPainter stiElementGdiPainter = new StiElementGdiPainter();
			base.Child.Visible = !stiElementGdiPainter.IsNoResult(base.Element, dataTable);
		}
		RectangleD contentRect = GetContentRect();
		base.IsShowNoResult = dataTable == null;
		base.IsShowEmptyData = dataTable?.IsEmpty ?? false;
		if (base.Atom.DataTable == dataTable)
		{
			base.Atom.Location = contentRect;
		}
		else if (base.AnimationEngine.IsReverse)
		{
			StiChartAtom stiChartAtom = StiElementAtom.Get(contentRect, base.Element, dataTable, base.AnimationEngine) as StiChartAtom;
			if (stiChartAtom.Chart.Series.Count <= 0 || ChartAtom.Chart.Series.Count <= 0 || !StiChartHelper.GetAnimationСompatibilitySeries(stiChartAtom.Chart.Series[0], ChartAtom.Chart.Series[0]))
			{
				ChartAtom.Chart.PreviousAnimations.Clear();
			}
			base.AnimationEngine.Tag = dataTable;
		}
		else
		{
			((IStiChartElement)base.Element).PreviousAnimations = ChartAtom.Chart.PreviousAnimations;
			base.Atom = StiElementAtom.Get(contentRect, base.Element, dataTable, base.AnimationEngine);
			base.AnimationEngine.Start();
		}
		StiChartElementInteractionHelper.ResetChartGeomCache();
		((Control)this).Invalidate();
	}

	internal List<StiDataFilterRule> GetFilters(bool isDrillDown = false)
	{
		RectangleD contentRect = GetContentRect();
		float zoom = GetZoom();
		IEnumerable<StiSeriesElementGeom> enumerable = from g in StiChartElementInteractionHelper.GetSeriesElementGeomsIsSelected(ChartAtom.Chart, ChartElement, contentRect, zoom)
			where g.Interaction != null
			select g;
		if (!IsAllowInteractive || enumerable == null)
		{
			return null;
		}
		List<StiDataFilterRule> filterList = new List<StiDataFilterRule>();
		enumerable.ToList().ForEach(delegate(StiSeriesElementGeom x)
		{
			filterList.AddRange(StiChartElementFilterHelper.GetElementGeomFilter(ChartElement, x, isDrillDown));
		});
		return filterList;
	}

	private void RefreshFilter()
	{
		List<StiDataFilterRule> filters = GetFilters();
		if (filters != null)
		{
			bool flag = filters == null || filters.Count == 0;
			bool flag2 = ChartElement.UserFilters == null || ChartElement.UserFilters.Count == 0;
			if (!(flag && flag2))
			{
				ChartElement.UserFilters = filters;
				RefreshViewerData();
			}
		}
	}

	public override void RemoveFilter()
	{
		RemoveFilter(null);
	}

	private void RemoveFilter(IStiGeomInteraction skipGeom)
	{
		RectangleD contentRect = GetContentRect();
		float zoom = GetZoom();
		if (skipGeom is StiPie3dMainSideSeriesElementGeom stiPie3dMainSideSeriesElementGeom)
		{
			string hashKey = stiPie3dMainSideSeriesElementGeom.GetHashKey();
			List<StiSeriesElementGeom> seriesElementGeomsIsSelected = StiChartElementInteractionHelper.GetSeriesElementGeomsIsSelected(ChartAtom.Chart, ChartElement, contentRect, zoom);
			seriesElementGeomsIsSelected.Where((StiSeriesElementGeom g) => g.GetHashKey() != hashKey).ToList().ForEach(delegate(StiSeriesElementGeom g)
			{
				g.IsSelected = false;
			});
		}
		else
		{
			List<StiSeriesElementGeom> seriesElementGeomsIsSelected2 = StiChartElementInteractionHelper.GetSeriesElementGeomsIsSelected(ChartAtom.Chart, ChartElement, contentRect, zoom);
			seriesElementGeomsIsSelected2.Where((StiSeriesElementGeom g) => g != skipGeom).ToList().ForEach(delegate(StiSeriesElementGeom g)
			{
				g.IsSelected = false;
			});
		}
		((Control)this).Invalidate();
	}

	public void ResetGeomSelection()
	{
		RectangleD contentRect = GetContentRect();
		float zoom = GetZoom();
		List<StiSeriesElementGeom> seriesElementGeomsIsSelected = StiChartElementInteractionHelper.GetSeriesElementGeomsIsSelected(ChartAtom.Chart, ChartElement, contentRect, zoom);
		seriesElementGeomsIsSelected.ToList().ForEach(delegate(StiSeriesElementGeom g)
		{
			g.IsSelected = false;
		});
	}

	public override void SwitchFilterMultiOption()
	{
		if (!StiActionButtonsHelper.GetFilterMultiOption(base.Element))
		{
			RectangleD contentRect = GetContentRect();
			float zoom = GetZoom();
			List<StiSeriesElementGeom> seriesElementGeomsIsSelected = StiChartElementInteractionHelper.GetSeriesElementGeomsIsSelected(ChartAtom.Chart, ChartElement, contentRect, zoom);
			seriesElementGeomsIsSelected.Skip(1).ToList().ForEach(delegate(StiSeriesElementGeom g)
			{
				g.IsSelected = false;
			});
			RefreshFilter();
			((Control)this).Invalidate();
		}
	}

	private void ProcessChartInteractionDispose()
	{
		StiChartInteractionHelper.ResetChartGeomCache();
		try
		{
			Timer obj = chartInteractionRecallTimer;
			if (obj != null)
			{
				obj.Stop();
			}
			((Component)(object)chartInteractionRecallTimer)?.Dispose();
			chartInteractionRecallTimer = null;
		}
		catch
		{
		}
	}

	private RectangleD GetContentRect()
	{
		return new RectangleD(0.0, 0.0, ((Control)this).Width, ((Control)this).Height);
	}

	private float GetZoom()
	{
		return 1f;
	}

	private void StopRecallTimer()
	{
		Timer obj = chartInteractionRecallTimer;
		if (obj != null)
		{
			obj.Stop();
		}
	}

	private void ProcessChartInteractionMouseUp()
	{
		Point point = ((Control)this).PointToClient(Cursor.Position);
		RectangleD contentRect = GetContentRect();
		StopRecallTimer();
		if (ChartAtom == null)
		{
			return;
		}
		float zoom = GetZoom();
		StiCellGeom chartGeom;
		IStiGeomInteraction interaction = StiChartElementInteractionHelper.GetInteraction(ChartAtom.Chart, ChartElement, contentRect, point.X, point.Y, out chartGeom, zoom * (float)StiScale.X);
		if (interaction != null)
		{
			StiInteractionOptions stiInteractionOptions = new StiInteractionOptions();
			interaction.InvokeMouseUp(stiInteractionOptions);
			if (stiInteractionOptions.UpdateContext)
			{
				((Control)this).Invalidate();
				StiChartInteractionHelper.ResetChartGeomCache();
			}
		}
	}

	private void ProcessChartInteractionMouseDown(Point pts, StiChartElement chartElement, RectangleD rect, bool calledFromTimer)
	{
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Expected O, but got Unknown
		float zoom = GetZoom();
		if (ChartAtom == null)
		{
			return;
		}
		StiCellGeom chartGeom;
		IStiGeomInteraction interaction = StiChartElementInteractionHelper.GetInteraction(ChartAtom.Chart, chartElement, rect, pts.X, pts.Y, out chartGeom, zoom * (float)StiScale.X);
		if (interaction == null)
		{
			return;
		}
		StiInteractionOptions stiInteractionOptions = new StiInteractionOptions();
		if (calledFromTimer)
		{
			stiInteractionOptions.IsRecalled = true;
		}
		if (interaction is StiCellGeom geom && chartGeom != null)
		{
			RectangleF rect2 = chartGeom.GetRect(geom);
			stiInteractionOptions.MousePoint = new PointF((float)((double)((float)pts.X - rect2.X) - rect.X), (float)((double)((float)pts.Y - rect2.Y) - rect.Y));
		}
		interaction.InvokeMouseDown(stiInteractionOptions);
		if (!stiInteractionOptions.UpdateContext)
		{
			return;
		}
		((Control)this).Invalidate();
		StiChartInteractionHelper.ResetChartGeomCache();
		if (!stiInteractionOptions.RecallEvent)
		{
			return;
		}
		if (!calledFromTimer)
		{
			if (chartInteractionRecallTimer == null)
			{
				chartInteractionRecallTimer = new Timer();
			}
			else
			{
				StopRecallTimer();
			}
			chartInteractionRecallTimer.Interval = (int)stiInteractionOptions.RecallTime.TotalMilliseconds;
			chartInteractionRecallTimer.Tick += delegate
			{
				ProcessChartInteractionMouseDown(chartInteractionPoint, chartInteractionLatestChart, chartInteractionRect, calledFromTimer: true);
			};
			chartInteractionLatestChart = chartElement;
			chartInteractionPoint = pts;
			chartInteractionRect = rect;
			chartInteractionRecallTimer.Start();
		}
		else if (chartInteractionRecallTimer != null)
		{
			chartInteractionRecallTimer.Stop();
			chartInteractionRecallTimer.Interval = (int)stiInteractionOptions.RecallTime.TotalMilliseconds;
			chartInteractionRecallTimer.Start();
		}
	}

	private StiChartInteractionResult ProcessChartInteractionOldLeaveNewEnter()
	{
		Point point = ((Control)this).PointToClient(Cursor.Position);
		RectangleD contentRect = GetContentRect();
		StiChartInteractionResult stiChartInteractionResult = new StiChartInteractionResult();
		if (ChartAtom == null)
		{
			return stiChartInteractionResult;
		}
		float zoom = GetZoom();
		StiCellGeom chartGeom;
		IStiGeomInteraction interaction = StiChartElementInteractionHelper.GetInteraction(ChartAtom.Chart, ChartElement, contentRect, point.X, point.Y, out chartGeom, zoom * (float)StiScale.X);
		IStiGeomInteraction stiGeomInteraction = chartComponentToInteraction[ChartElement] as IStiGeomInteraction;
		if (stiGeomInteraction != interaction)
		{
			bool flag = false;
			if (stiGeomInteraction != null)
			{
				StiInteractionOptions stiInteractionOptions = new StiInteractionOptions();
				stiGeomInteraction.InvokeMouseLeave(stiInteractionOptions);
				if (stiInteractionOptions.UpdateContext)
				{
					flag = true;
				}
				StopRecallTimer();
			}
			if (interaction != null)
			{
				StiInteractionOptions stiInteractionOptions2 = new StiInteractionOptions();
				interaction.InvokeMouseEnter(stiInteractionOptions2);
				if (stiInteractionOptions2.UpdateContext)
				{
					flag = true;
				}
				stiChartInteractionResult.ToolTipString = stiInteractionOptions2.InteractionToolTip;
				stiChartInteractionResult.HyperlinkString = stiInteractionOptions2.InteractionHyperlink;
				stiChartInteractionResult.InteractionData = stiInteractionOptions2.SeriesInteractionData;
			}
			chartComponentToInteraction[ChartElement] = interaction;
			if (flag)
			{
				((Control)this).Invalidate();
			}
		}
		else if (interaction != null)
		{
			StiInteractionOptions stiInteractionOptions3 = new StiInteractionOptions();
			interaction.InvokeMouseEnter(stiInteractionOptions3);
			stiChartInteractionResult.ToolTipString = stiInteractionOptions3.InteractionToolTip;
			stiChartInteractionResult.HyperlinkString = stiInteractionOptions3.InteractionHyperlink;
			stiChartInteractionResult.InteractionData = stiInteractionOptions3.SeriesInteractionData;
		}
		return stiChartInteractionResult;
	}

	private void ProcessChartInteractionMouseLeave()
	{
		if (chartComponentToInteraction[ChartElement] is IStiGeomInteraction stiGeomInteraction)
		{
			StiInteractionOptions stiInteractionOptions = new StiInteractionOptions();
			stiGeomInteraction.InvokeMouseLeave(stiInteractionOptions);
			StopRecallTimer();
			if (stiInteractionOptions.UpdateContext)
			{
				((Control)this).Invalidate();
			}
		}
	}

	private StiChartInteractionResult ProcessChartInteractionClick(bool selectOnClick)
	{
		Point point = ((Control)this).PointToClient(Cursor.Position);
		RectangleD contentRect = GetContentRect();
		StiChartInteractionResult stiChartInteractionResult = new StiChartInteractionResult();
		if (ChartAtom == null)
		{
			return stiChartInteractionResult;
		}
		float zoom = GetZoom();
		StiCellGeom chartGeom;
		IStiGeomInteraction interaction = StiChartElementInteractionHelper.GetInteraction(ChartAtom.Chart, ChartElement, contentRect, point.X, point.Y, out chartGeom, zoom * (float)StiScale.X, typeof(StiConstantLinesVerticalGeom));
		if (interaction == null)
		{
			return stiChartInteractionResult;
		}
		StiInteractionOptions stiInteractionOptions = new StiInteractionOptions();
		stiInteractionOptions.SelectOnClick = selectOnClick;
		interaction.InvokeClick(stiInteractionOptions);
		if (stiInteractionOptions.UpdateContext)
		{
			((Control)this).Invalidate();
			StiChartInteractionHelper.ResetChartGeomCache();
		}
		stiChartInteractionResult.HyperlinkString = stiInteractionOptions.InteractionHyperlink;
		stiChartInteractionResult.InteractionData = stiInteractionOptions.SeriesInteractionData;
		stiChartInteractionResult.Geom = interaction;
		return stiChartInteractionResult;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			((Component)(object)toolTipMain)?.Dispose();
			((Component)(object)toolTipHyperlink)?.Dispose();
			((Component)(object)toolTipHtml)?.Dispose();
			ProcessChartInteractionDispose();
			if (base.AnimationEngine != null)
			{
				base.AnimationEngine.Animation -= AnimationEngine_Animation;
				base.AnimationEngine.ReverseEnd -= AnimationEngine_ReverseEnd;
				base.AnimationEngine.Dispose();
			}
		}
		base.Dispose(disposing);
	}

	public StiChartElementControl(IStiElement element)
		: base(element)
	{
		base.AnimationEngine.Animation += AnimationEngine_Animation;
		base.AnimationEngine.ReverseEnd += AnimationEngine_ReverseEnd;
	}
}
