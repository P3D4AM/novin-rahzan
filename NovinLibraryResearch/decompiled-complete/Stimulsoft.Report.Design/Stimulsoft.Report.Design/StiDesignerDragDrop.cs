using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.BarCodes;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Gauge.Helpers;
using Stimulsoft.Report.Maps;

namespace Stimulsoft.Report.Design;

public sealed class StiDesignerDragDrop
{
	public static string DragText { get; private set; }

	public static bool IgnoreAddSelectedToProperties { get; set; }

	public static void StartDragDrop(StiDesignerControl designer, Control dragDropSender)
	{
		StiComponent componentForCreating = designer.CreateTool.ComponentForCreating;
		if (componentForCreating != null)
		{
			StartDragDrop(designer, componentForCreating, dragDropSender, string.Empty);
		}
	}

	public static void StartDragDrop(StiDesignerControl designer, StiComponent componentForCreate, Control dragDropSender)
	{
		StartDragDrop(designer, componentForCreate, dragDropSender, string.Empty);
	}

	public static void StartDragDrop(StiDesignerControl designer, StiComponent componentForCreate, Control dragDropSender, string textForTextComponent)
	{
		StartDragDrop(designer, componentForCreate, dragDropSender, textForTextComponent, null);
	}

	public static void StartDragDrop(StiDesignerControl designer, StiComponent componentForCreate, Control dragDropSender, string textForTextComponent, string textForLabel)
	{
		//IL_0833: Unknown result type (might be due to invalid IL or missing references)
		if (componentForCreate is IStiElement)
		{
			textForLabel = null;
		}
		if (designer.TabControl.SelectedTabIndex >= designer.Report.Pages.Count || designer.Report.Info.IsComponentsMoving)
		{
			return;
		}
		designer.Report.GetCurrentPage().ResetSelection();
		designer.SelectTool.Activate();
		designer.SelectTool.Action = StiAction.Move;
		designer.SelectTool.IsDragDropComponent = true;
		designer.Report.Info.IsComponentsMoving = true;
		StiComponentDesigner componentDesigner = StiComponentDesigner.GetComponentDesigner(designer, componentForCreate.GetType());
		StiComponent stiComponent = componentDesigner.CreateComponent(componentForCreate.GetType(), RectangleD.Empty);
		if (componentForCreate is StiMap)
		{
			((StiMap)stiComponent).MapIdent = StiSettings.GetStr("DbsMapEditor", "LastMapID", "USA");
		}
		if (componentForCreate is IStiRegionMapElement)
		{
			((IStiRegionMapElement)stiComponent).MapIdent = StiSettings.GetStr("DbsMapEditor", "LastMapID", "USA");
		}
		if (componentForCreate is IStiBarCode)
		{
			((IStiBarCode)stiComponent).BarCodeType = ((IStiBarCode)componentForCreate).BarCodeType;
		}
		if (componentForCreate is StiChart stiChart)
		{
			StiChart stiChart2 = stiComponent as StiChart;
			if (stiChart.Series.Count > 0)
			{
				StiSeries stiSeries = (StiSeries)StiActivator.CreateObject(stiChart.Series[0].GetType());
				stiChart2.Series.Add(stiSeries);
				stiSeries.Core.ApplyStyle(stiChart2.Style, stiChart2.Style.Core.GetColorBySeries(stiSeries, stiSeries.Core?.SeriesColors));
			}
		}
		StiGauge stiGauge = componentForCreate as StiGauge;
		if (stiGauge != null)
		{
			((StiGauge)stiComponent).Type = ((StiGauge)componentForCreate).Type;
		}
		if (componentForCreate is StiShape)
		{
			((StiShape)stiComponent).ShapeType = ((StiShape)componentForCreate).ShapeType;
		}
		if (componentForCreate is IStiShapeElement stiShapeElement)
		{
			((IStiShapeElement)stiComponent).ShapeType = stiShapeElement.ShapeType;
		}
		StiOptions.Engine.GlobalEvents.InvokeComponentCreated(designer, new StiComponentCreationEventArgs(designer, stiComponent));
		if (stiComponent is IStiTextFormat && ((IStiTextFormat)componentForCreate).TextFormat != null)
		{
			((IStiTextFormat)stiComponent).TextFormat = ((IStiTextFormat)componentForCreate).TextFormat.Clone() as StiFormatService;
		}
		if (stiComponent is IStiTitleElement && componentForCreate is IStiTitleElement)
		{
			((IStiTitleElement)stiComponent).Title.Text = ((IStiTitleElement)componentForCreate).Title.Text;
		}
		if (stiComponent is StiImage stiImage)
		{
			StiImage stiImage2 = componentForCreate as StiImage;
			stiImage.DataColumn = stiImage2.DataColumn;
			stiImage.ImageURL.Value = stiImage2.ImageURL.Value;
			if (stiImage2.ImageData != null)
			{
				if (stiImage.ImageData == null)
				{
					stiImage.ImageData = new StiImageDataExpression();
				}
				stiImage.ImageData.Value = stiImage2.ImageData.Value;
			}
		}
		if (stiComponent is StiRichText stiRichText)
		{
			StiRichText stiRichText2 = componentForCreate as StiRichText;
			stiRichText.DataUrl.Value = stiRichText2.DataUrl.Value;
		}
		if (stiComponent is StiSubReport stiSubReport)
		{
			StiSubReport stiSubReport2 = componentForCreate as StiSubReport;
			stiSubReport.SubReportUrl = stiSubReport2.SubReportUrl;
		}
		if (stiComponent is IStiComboBoxElement stiComboBoxElement)
		{
			IStiComboBoxElement stiComboBoxElement2 = componentForCreate as IStiComboBoxElement;
			stiComboBoxElement.AddKeyMeter(stiComboBoxElement2.GetKeyMeter());
		}
		if (stiComponent is IStiDatePickerElement stiDatePickerElement)
		{
			IStiDatePickerElement stiDatePickerElement2 = componentForCreate as IStiDatePickerElement;
			stiDatePickerElement.AddValueMeter(stiDatePickerElement2.GetValueMeter());
			stiDatePickerElement.SelectionMode = stiDatePickerElement2.SelectionMode;
		}
		if (stiComponent is IStiImageElement stiImageElement)
		{
			IStiImageElement stiImageElement2 = componentForCreate as IStiImageElement;
			stiImageElement.ImageHyperlink = stiImageElement2.ImageHyperlink;
		}
		if (stiComponent is IStiTableElement stiTableElement)
		{
			IStiTableElement stiTableElement2 = componentForCreate as IStiTableElement;
			stiComponent.Width = ((StiComponent)stiTableElement2).Width;
			stiTableElement.CreateMeters(stiTableElement2);
		}
		if (stiComponent is IStiCheckBoxControl)
		{
			((IStiCheckBoxControl)stiComponent).Text = ((IStiCheckBoxControl)componentForCreate).Text;
			((IStiCheckBoxControl)stiComponent).CheckedBinding = ((IStiCheckBoxControl)componentForCreate).CheckedBinding.Replace("{", "").Replace("}", "");
		}
		if (stiComponent is StiCheckBox)
		{
			((StiCheckBox)stiComponent).Checked.Value = ((StiCheckBox)componentForCreate).Checked.Value;
		}
		if (!string.IsNullOrEmpty(componentForCreate.Name))
		{
			stiComponent.Name = componentForCreate.Name;
		}
		stiComponent.Select();
		stiComponent.Dockable = false;
		StiComponent stiComponent2 = null;
		if (textForLabel != null)
		{
			if (designer.IsFormSelected)
			{
				stiComponent2 = StiDialogsProvider.GetProvider(designer.Report).CreateLabelControl() as StiComponent;
				StiOptions.Engine.GlobalEvents.InvokeComponentCreated(designer, new StiComponentCreationEventArgs(designer, stiComponent2));
				((IStiLabelControl)stiComponent2).Text = textForLabel;
			}
			else if (designer.IsDashboardSelected)
			{
				stiComponent2 = StiDashboardCreator.CreateTextElement(designer.Report) as StiComponent;
				((IStiTextElement)stiComponent2).Text = textForLabel;
				StiOptions.Engine.GlobalEvents.InvokeComponentCreated(designer, new StiComponentCreationEventArgs(designer, stiComponent2));
			}
			else
			{
				componentDesigner = StiComponentDesigner.GetComponentDesigner(designer, StiOptions.Designer.ComponentsTypes.SimpleComponents.Text);
				stiComponent2 = componentDesigner.CreateComponent(StiOptions.Designer.ComponentsTypes.SimpleComponents.Text, RectangleD.Empty) as StiText;
				StiOptions.Engine.GlobalEvents.InvokeComponentCreated(designer, new StiComponentCreationEventArgs(designer, stiComponent2));
				((StiText)stiComponent2).Text = textForLabel;
			}
			stiComponent2.Select();
			stiComponent2.Dockable = false;
		}
		if (!string.IsNullOrEmpty(textForTextComponent))
		{
			if (stiComponent is IStiText && !(stiComponent is StiRichText))
			{
				((IStiText)stiComponent).Text = textForTextComponent;
			}
			else if (stiComponent is IStiLabelControl)
			{
				((IStiLabelControl)stiComponent).Text = textForTextComponent;
			}
			else if (stiComponent is IStiTextElement)
			{
				((IStiTextElement)stiComponent).Text = textForTextComponent;
			}
			else if (stiComponent is IStiTextBoxControl)
			{
				((IStiTextBoxControl)stiComponent).Text = string.Empty;
				((IStiTextBoxControl)stiComponent).TextBinding = textForTextComponent.Replace("{", "").Replace("}", "");
			}
		}
		if (componentForCreate is StiDataBand)
		{
			if (((StiDataBand)componentForCreate).DataSourceName != null)
			{
				((StiDataBand)stiComponent).DataSourceName = ((StiDataBand)componentForCreate).DataSourceName;
			}
			else
			{
				((StiDataBand)stiComponent).BusinessObjectGuid = ((StiDataBand)componentForCreate).BusinessObjectGuid;
			}
		}
		if (designer.UseLastFormat && !(stiComponent is StiContainer) && !(stiComponent is IStiIgnoryStyle))
		{
			if (stiComponent is IStiDashboardElementStyle)
			{
				((IStiDashboardElementStyle)stiComponent).Style = designer.CurrentDashboardStyleIdent;
			}
			else
			{
				designer.CurrentStyle.SetStyleToComponent(stiComponent);
			}
			if (stiComponent2 != null)
			{
				designer.CurrentStyle.SetStyleToComponent(stiComponent2);
			}
		}
		RectangleD clientRectangle = designer.Report.GetCurrentPage().Unit.ConvertFromHInches(componentForCreate.DefaultClientRectangle).AlignToGrid(designer.Report.GetCurrentPage().GridSize, designer.Report.Info.AlignToGrid);
		PointD currentCursorPos = designer.PageView.GetCurrentCursorPos();
		clientRectangle.X = currentCursorPos.X;
		clientRectangle.Y = currentCursorPos.Y;
		if (stiComponent.Width != 0.0)
		{
			clientRectangle.Width = stiComponent.Width;
		}
		if (stiComponent.Height != 0.0)
		{
			clientRectangle.Height = stiComponent.Height;
		}
		stiComponent.ClientRectangle = clientRectangle;
		if (stiComponent2 != null)
		{
			stiComponent2.ClientRectangle = clientRectangle;
			stiComponent2.Left -= clientRectangle.Width;
			if (stiComponent2 is StiText)
			{
				((StiText)stiComponent2).AutoWidth = true;
			}
			stiComponent2.Page = designer.Report.GetCurrentPage();
			SizeD actualSize = stiComponent2.GetActualSize();
			if (stiComponent2 is StiText)
			{
				((StiText)stiComponent2).AutoWidth = false;
			}
			stiComponent2.Width = StiAlignValue.AlignToMaxGrid(actualSize.Width, designer.Report.Info.GridSize, designer.Report.Info.AlignToGrid);
			stiComponent2.Left = clientRectangle.Left - stiComponent2.Width;
			stiComponent2.Page = null;
			if (stiComponent2 is IStiLabelControl)
			{
				stiComponent2.Height = 20.0;
			}
		}
		if (stiGauge != null)
		{
			StiGaugeV2InitHelper.Init(stiComponent as StiGauge, stiGauge.Type, addRange: true, skipText: false);
		}
		designer.PageView.StartPos = Cursor.Position;
		designer.PageView.DraggingComponent = stiComponent;
		designer.PageView.DraggingLabelComponent = stiComponent2;
		DragText = textForTextComponent;
		dragDropSender.DoDragDrop((object)dragDropSender, (DragDropEffects)2);
		DragText = null;
		designer.Report.Info.IsComponentsMoving = false;
		designer.Builder.Redraw();
		if (!IgnoreAddSelectedToProperties)
		{
			designer.AddSelectedToPropertyGrid(updateSelectedObjects: true);
		}
		IgnoreAddSelectedToProperties = false;
	}
}
