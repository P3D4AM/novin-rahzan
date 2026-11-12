using System;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.BarCodes;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Components.Gauge;
using Stimulsoft.Report.Components.Table;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Gauge.Helpers;
using Stimulsoft.Report.Maps;

namespace Stimulsoft.Report.Design.Tools;

[StiToolbox(false)]
public class StiCreateToolService : StiToolService
{
	private static Cursor cursorPen;

	public override string ServiceName => Loc.Get("Toolbox", "Create");

	internal bool ShiftMode { get; set; }

	public StiCreateAction Action { get; set; }

	public override int Position => 2;

	public bool DragDropAction { get; set; }

	public virtual StiComponent ComponentForCreating { get; set; }

	public string TextForComponent { get; set; } = "";


	public string UndoRedoTextForComponent { get; set; } = "";


	internal static Cursor CursorPen
	{
		get
		{
			if (StiScale.Factor > 2.0)
			{
				return Cursors.Cross;
			}
			if (cursorPen == (Cursor)null)
			{
				cursorPen = ((StiScale.Factor > 1.0) ? StiCursorUtils.GetCursor("Stimulsoft.Report", "Stimulsoft.Report.Cursors.Pen_x2.cur") : StiCursorUtils.GetCursor("Stimulsoft.Report", "Stimulsoft.Report.Cursors.Pen.cur"));
			}
			return cursorPen;
		}
	}

	protected override void OnActivated(EventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		base.OnActivated(e);
		ShiftMode = (Control.ModifierKeys & 0x10000) > 0;
	}

	public override void OnMouseDown(object sender, MouseEventArgs e)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Invalid comparison between Unknown and I4
		base.OnMouseDown(sender, e);
		if (ComponentForCreating == null)
		{
			DragDropAction = false;
			Designer.SelectTool.Activate();
		}
		else
		{
			if ((int)e.Button != 1048576)
			{
				return;
			}
			StiComponent componentForCreating = ComponentForCreating;
			StiComponentDesigner componentDesigner = StiComponentDesigner.GetComponentDesigner(Designer, componentForCreating.GetType());
			StiComponent stiComponent = componentDesigner.CreateComponent(componentForCreating.GetType(), RectangleD.Empty);
			StiOptions.Engine.GlobalEvents.InvokeComponentCreated(Designer, new StiComponentCreationEventArgs(Designer, stiComponent));
			stiComponent.Dockable = false;
			if (componentForCreating is StiChart && ((StiChart)componentForCreating).Series.Count > 0)
			{
				IStiSeries value = ((StiChart)componentForCreating).Series[0];
				((StiChart)stiComponent).Series.Add(value);
			}
			if (componentForCreating is StiBarCode)
			{
				((StiBarCode)stiComponent).BarCodeType = ((StiBarCode)componentForCreating).BarCodeType;
				((StiBarCode)stiComponent).Code.Value = ((StiBarCode)componentForCreating).BarCodeType.DefaultCodeValue;
			}
			if (componentForCreating is StiMap)
			{
				((StiMap)stiComponent).MapIdent = StiSettings.GetStr("DbsMapEditor", "LastMapID", "USA");
			}
			if (componentForCreating is IStiRegionMapElement)
			{
				((IStiRegionMapElement)stiComponent).MapIdent = StiSettings.GetStr("DbsMapEditor", "LastMapID", "USA");
			}
			if (componentForCreating is StiGauge)
			{
				StiGauge stiGauge = (StiGauge)componentForCreating;
				if (StiGaugeV2InitHelper.AllowOldEditor)
				{
					if (stiGauge.Scales.Count > 0)
					{
						if (stiGauge.Scales[0] is StiLinearScale)
						{
							StiGaugeHelper.DefaultLinearGauge((StiGauge)stiComponent, Designer.Report);
						}
						else
						{
							StiGaugeHelper.DefaultRadialGauge((StiGauge)stiComponent, Designer.Report);
						}
					}
				}
				else
				{
					StiGauge stiGauge2 = (StiGauge)stiComponent;
					stiGauge2.Type = stiGauge.Type;
					StiGaugeV2InitHelper.Init(stiGauge2, stiGauge2.Type, addRange: true, skipText: false);
				}
			}
			if (stiComponent is IStiShape && ComponentForCreating is StiShape)
			{
				((StiShape)stiComponent).ShapeType = ((StiShape)ComponentForCreating).ShapeType;
			}
			if (stiComponent is IStiShapeElement && ComponentForCreating is IStiShapeElement)
			{
				((IStiShapeElement)stiComponent).ShapeType = ((IStiShapeElement)ComponentForCreating).ShapeType;
			}
			if (stiComponent is IStiText && !string.IsNullOrEmpty(TextForComponent))
			{
				((IStiText)stiComponent).Text = TextForComponent;
			}
			if (!string.IsNullOrEmpty(UndoRedoTextForComponent))
			{
				Designer.UndoRedoSave(UndoRedoTextForComponent, onlyCurrentPage: true);
			}
			else
			{
				Designer.UndoRedoSave(Loc.GetMain("Create") + " " + stiComponent.LocalizedName, onlyCurrentPage: true);
			}
			stiComponent.Select();
			if (!(stiComponent is StiContainer) && !(stiComponent is IStiIgnoryStyle))
			{
				if (Designer.UseLastFormat)
				{
					Designer.SetStyleToComponent(stiComponent);
				}
				if (stiComponent is IStiDashboardElementStyle)
				{
					((IStiDashboardElementStyle)stiComponent).Style = StiElementStyleIdent.Auto;
				}
			}
			TextForComponent = string.Empty;
			double value2 = Designer.XToPage(e.X);
			double value3 = Designer.YToPage(e.Y);
			double x = StiAlignValue.AlignToGrid(value2, base.CurrentPage.GridSize, base.Info.AlignToGrid);
			double y = StiAlignValue.AlignToGrid(value3, base.CurrentPage.GridSize, base.Info.AlignToGrid);
			PointD pointD = new PointD(x, y);
			if (stiComponent is StiTable)
			{
				RectangleD rectangleD = base.CurrentPage.Unit.ConvertFromHInches(stiComponent.DefaultClientRectangle);
				stiComponent.ClientRectangle = new RectangleD(pointD.X, pointD.Y, rectangleD.Width, rectangleD.Height);
			}
			else
			{
				stiComponent.ClientRectangle = new RectangleD(pointD.X, pointD.Y, 0.0, 0.0);
			}
			base.CurrentPage.Components.Add(stiComponent);
			Designer.PageView.StartPos = ((Control)Designer.CurrentPageView.View).PointToScreen(new Point(e.X, e.Y));
			Designer.SelectTool.SkipUndoRedoOperation = true;
			Designer.SelectTool.DragDropAction = DragDropAction;
			Designer.SelectTool.CreatedComponent = stiComponent;
			Designer.SelectTool.IsCreatingComponent = true;
			Designer.SelectTool.IsShiftMode = ShiftMode;
			Designer.SelectTool.Activate();
			Designer.SelectTool.TableHelper = null;
			Designer.SelectTool.TableHelperV2 = null;
			if (stiComponent is StiTable)
			{
				Designer.SelectTool.Action = StiAction.Move;
			}
			else
			{
				Designer.SelectTool.Action = StiAction.SizeRightBottom;
			}
			base.Info.IsComponentsMoving = true;
			base.CurrentPage.ResetSelection();
			stiComponent.Select();
			DragDropAction = false;
		}
	}

	public override Cursor GetCursor()
	{
		return CursorPen;
	}

	public override string GetStatusBarInformation()
	{
		if (Action != StiCreateAction.SizeComponent)
		{
			return base.GetStatusBarInformation();
		}
		RectangleD rectangleD = base.CurrentPage.SelectedRectangle.AlignToGrid(base.CurrentPage.GridSize, base.Info.AlignToGrid & Designer.PageView.IsAlignSelection).Normalize();
		return string.Format("{0}:{1:F}  {2}:{3:F}  {4}:{5:F}  {6}:{7:F}", "X", rectangleD.X, "Y", rectangleD.Y, Loc.GetMain("Width"), rectangleD.Width, Loc.GetMain("Height"), rectangleD.Height);
	}
}
