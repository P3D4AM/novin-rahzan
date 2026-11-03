using System;
using System.Collections;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Report.BarCodes;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.ShapeTypes;
using Stimulsoft.Report.Components.Table;
using Stimulsoft.Report.CrossTab;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Painters.Context.Animation;

namespace Stimulsoft.Report.Painters;

public abstract class StiPainter
{
	public delegate StiPainter GetWpfComponentPainterDelegate(Type type);

	public delegate StiShapeTypePainter GetWpfShapeTypePainterDelegate(Type type);

	public delegate StiIndicatorTypePainter GetWpfIndicatorTypePainterDelegate(Type type);

	private static Hashtable typeToGdiPainter = new Hashtable();

	private static Hashtable typeToWpfPainter = new Hashtable();

	private static object lockObject = new object();

	internal StiAnimationEngine AnimationEngine { get; set; }

	public static StiPainter GetPainter(Type type, StiGuiMode guiMode)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Invalid comparison between Unknown and I4
		lock (lockObject)
		{
			Hashtable hashtable = (((int)guiMode == 0) ? typeToGdiPainter : typeToWpfPainter);
			StiPainter stiPainter = hashtable[type] as StiPainter;
			if (stiPainter != null)
			{
				return stiPainter;
			}
			if ((int)guiMode != 0)
			{
				if ((int)guiMode == 1)
				{
					stiPainter = GetWpfComponentTypePainter(type);
				}
			}
			else
			{
				stiPainter = GetGdiComponentTypePainter(type);
			}
			if (stiPainter == null)
			{
				throw new Exception($"StiPainter for '{type}' is not found!");
			}
			hashtable[type] = stiPainter;
			return stiPainter;
		}
	}

	private static StiPainter GetWpfComponentTypePainter(Type type)
	{
		StiPainter stiPainter = ((StiWpfPainterDelegates.ComponentPainter != null) ? StiWpfPainterDelegates.ComponentPainter(type) : null);
		if (stiPainter != null)
		{
			return stiPainter;
		}
		string text = ((type.GetCustomAttributes(typeof(StiWpfPainterAttribute), inherit: true).FirstOrDefault() is StiWpfPainterAttribute stiWpfPainterAttribute) ? stiWpfPainterAttribute.PainterTypeName : null);
		return StiActivator.CreateObject(text, new object[0]) as StiPainter;
	}

	private static StiComponentGdiPainter GetGdiComponentTypePainter(Type type)
	{
		if (type == typeof(StiChart))
		{
			return new StiChartGdiPainter();
		}
		if (type == typeof(StiClone))
		{
			return new StiCloneGdiPainter();
		}
		if (type == typeof(StiCheckBox))
		{
			return new StiCheckBoxGdiPainter();
		}
		if (type == typeof(StiTableCellCheckBox))
		{
			return new StiTableCellCheckBoxGdiPainter();
		}
		if (type == typeof(StiContourText))
		{
			return new StiContourTextGdiPainter();
		}
		if (type == typeof(StiCrossTab))
		{
			return new StiCrossTabGdiPainter();
		}
		if (type == typeof(StiHorizontalLinePrimitive))
		{
			return new StiHorizontalLinePrimitiveGdiPainter();
		}
		if (type == typeof(StiImage))
		{
			return new StiImageGdiPainter();
		}
		if (type == typeof(StiTableCellImage))
		{
			return new StiTableCellImageGdiPainter();
		}
		if (type == typeof(StiPage))
		{
			return new StiPageGdiPainter();
		}
		if (type == typeof(StiPointPrimitive) || type == typeof(StiEndPointPrimitive) || type == typeof(StiStartPointPrimitive))
		{
			return new StiPointPrimitiveGdiPainter();
		}
		if (type == typeof(StiRoundedRectanglePrimitive))
		{
			return new StiRoundedRectanglePrimitiveGdiPainter();
		}
		if (type == typeof(StiRectanglePrimitive))
		{
			return new StiRectanglePrimitiveGdiPainter();
		}
		if (type == typeof(StiRichText))
		{
			return new StiRichTextGdiPainter();
		}
		if (type == typeof(StiTableCellRichText))
		{
			return new StiTableCellRichTextGdiPainter();
		}
		if (type == typeof(StiShape))
		{
			return new StiShapeGdiPainter();
		}
		if (type == typeof(StiSubReport))
		{
			return new StiSubReportGdiPainter();
		}
		if (type == typeof(StiTextInCells))
		{
			return new StiTextInCellsGdiPainter();
		}
		if (type == typeof(StiTableCell))
		{
			return new StiTableCellTextGdiPainter();
		}
		if (type == typeof(StiUndefinedComponent))
		{
			return new StiUndefinedComponentGdiPainter();
		}
		if (type == typeof(StiVerticalLinePrimitive))
		{
			return new StiVerticalLinePrimitiveGdiPainter();
		}
		if (type == typeof(StiBand) || type == typeof(StiOverlayBand) || type == typeof(StiColumnFooterBand) || type == typeof(StiColumnHeaderBand) || type == typeof(StiPageFooterBand) || type == typeof(StiPageHeaderBand) || type == typeof(StiReportSummaryBand) || type == typeof(StiReportTitleBand) || type == typeof(StiChildBand) || type == typeof(StiDataBand) || type == typeof(StiTable) || type == typeof(StiFooterBand) || type == typeof(StiGroupFooterBand) || type == typeof(StiGroupHeaderBand) || type == typeof(StiHeaderBand) || type == typeof(StiEmptyBand))
		{
			return new StiBandGdiPainter();
		}
		if (type == typeof(StiContainer) || type == typeof(StiPanel) || type == typeof(StiFooterMarkerContainer))
		{
			return new StiContainerGdiPainter();
		}
		if (type == typeof(StiZipCode))
		{
			return new StiZipCodeGdiPainter();
		}
		if (type == typeof(StiView))
		{
			return new StiViewGdiPainter();
		}
		if (type == typeof(StiBarCode))
		{
			return new StiBarCodeGdiPainter();
		}
		if (type == typeof(StiComponent))
		{
			return new StiComponentGdiPainter();
		}
		if (type == typeof(StiText) || type == typeof(StiSystemText))
		{
			return new StiTextGdiPainter();
		}
		if (type == typeof(StiCrossRowTotal) || type == typeof(StiCrossColumnTotal) || type == typeof(StiCrossSummary) || type == typeof(StiCrossSummaryHeader) || type == typeof(StiCrossTitle) || type == typeof(StiCrossTotal))
		{
			return new StiTextGdiPainter();
		}
		if (type == typeof(StiCrossColumn) || type == typeof(StiCrossRow))
		{
			return new StiCrossHeaderGdiPainter();
		}
		if (type == typeof(StiForm))
		{
			return new StiFormGdiPainter();
		}
		if (type == typeof(StiButtonControl))
		{
			return new StiButtonControlGdiPainter();
		}
		if (type == typeof(StiCheckBoxControl))
		{
			return new StiCheckBoxControlGdiPainter();
		}
		if (type == typeof(StiCheckedListBoxControl))
		{
			return new StiCheckedListBoxControlGdiPainter();
		}
		if (type == typeof(StiComboBoxControl) || type == typeof(StiLookUpBoxControl))
		{
			return new StiComboBoxControlGdiPainter();
		}
		if (type == typeof(StiCustomControl))
		{
			return new StiCustomControlGdiPainter();
		}
		if (type == typeof(StiDateTimePickerControl))
		{
			return new StiDateTimePickerControlGdiPainter();
		}
		if (type == typeof(StiGridControl))
		{
			return new StiGridControlGdiPainter();
		}
		if (type == typeof(StiGroupBoxControl))
		{
			return new StiGroupBoxControlGdiPainter();
		}
		if (type == typeof(StiLabelControl))
		{
			return new StiLabelControlGdiPainter();
		}
		if (type == typeof(StiListBoxControl))
		{
			return new StiListBoxControlGdiPainter();
		}
		if (type == typeof(StiListViewControl))
		{
			return new StiListViewControlGdiPainter();
		}
		if (type == typeof(StiNumericUpDownControl))
		{
			return new StiNumericUpDownControlGdiPainter();
		}
		if (type == typeof(StiPanelControl))
		{
			return new StiPanelControlGdiPainter();
		}
		if (type == typeof(StiPictureBoxControl))
		{
			return new StiPictureBoxControlGdiPainter();
		}
		if (type == typeof(StiRadioButtonControl))
		{
			return new StiRadioButtonControlGdiPainter();
		}
		if (type == typeof(StiRichTextBoxControl))
		{
			return new StiRichTextBoxControlGdiPainter();
		}
		if (type == typeof(StiTextBoxControl))
		{
			return new StiTextBoxControlGdiPainter();
		}
		if (type == typeof(StiTreeViewControl))
		{
			return new StiTreeViewControlGdiPainter();
		}
		if (!(type.GetCustomAttributes(typeof(StiGdiPainterAttribute), inherit: true).FirstOrDefault() is StiGdiPainterAttribute stiGdiPainterAttribute))
		{
			return null;
		}
		return StiActivator.CreateObject(stiGdiPainterAttribute.PainterTypeName, new object[0]) as StiComponentGdiPainter;
	}

	public static StiShapeTypePainter GetShapePainter(Type type, StiGuiMode guiMode)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Invalid comparison between Unknown and I4
		lock (lockObject)
		{
			Hashtable hashtable = (((int)guiMode == 0) ? typeToGdiPainter : typeToWpfPainter);
			StiShapeTypePainter stiShapeTypePainter = hashtable[type] as StiShapeTypePainter;
			if (stiShapeTypePainter != null)
			{
				return stiShapeTypePainter;
			}
			if ((int)guiMode != 0)
			{
				if ((int)guiMode == 1)
				{
					stiShapeTypePainter = GetWpfShapeTypePainter(type);
				}
			}
			else
			{
				stiShapeTypePainter = GetGdiShapeTypePainter(type);
			}
			if (stiShapeTypePainter == null)
			{
				throw new Exception($"StiPainter for '{type}' is not found!");
			}
			hashtable[type] = stiShapeTypePainter;
			return stiShapeTypePainter;
		}
	}

	private static StiShapeTypePainter GetWpfShapeTypePainter(Type type)
	{
		StiShapeTypePainter stiShapeTypePainter = ((StiWpfPainterDelegates.ShapePainter != null) ? StiWpfPainterDelegates.ShapePainter(type) : null);
		if (stiShapeTypePainter != null)
		{
			return stiShapeTypePainter;
		}
		string text = ((type.GetCustomAttributes(typeof(StiWpfShapeTypePainterAttribute), inherit: true).FirstOrDefault() is StiWpfShapeTypePainterAttribute stiWpfShapeTypePainterAttribute) ? stiWpfShapeTypePainterAttribute.PainterTypeName : null);
		return StiActivator.CreateObject(text, new object[0]) as StiShapeTypePainter;
	}

	private static StiShapeTypePainter GetGdiShapeTypePainter(Type type)
	{
		if (type == typeof(StiArrowShapeType))
		{
			return new StiArrowGdiShapeTypePainter();
		}
		if (type == typeof(StiDivisionShapeType))
		{
			return new StiDivisionGdiShapeTypePainter();
		}
		if (type == typeof(StiChevronShapeType))
		{
			return new StiChevronGdiShapeTypePainter();
		}
		if (type == typeof(StiComplexArrowShapeType))
		{
			return new StiComplexArrowGdiShapeTypePainter();
		}
		if (type == typeof(StiDiagonalDownLineShapeType))
		{
			return new StiDiagonalDownLineGdiShapeTypePainter();
		}
		if (type == typeof(StiDiagonalUpLineShapeType))
		{
			return new StiDiagonalUpLineGdiShapeTypePainter();
		}
		if (type == typeof(StiEqualShapeType))
		{
			return new StiEqualGdiShapeTypePainter();
		}
		if (type == typeof(StiFlowchartCardShapeType))
		{
			return new StiFlowchartCardGdiShapeTypePainter();
		}
		if (type == typeof(StiFlowchartCollateShapeType))
		{
			return new StiFlowchartCollateGdiShapeTypePainter();
		}
		if (type == typeof(StiFlowchartDecisionShapeType))
		{
			return new StiFlowchartDecisionGdiShapeTypePainter();
		}
		if (type == typeof(StiFlowchartManualInputShapeType))
		{
			return new StiFlowchartManualInputGdiShapeTypePainter();
		}
		if (type == typeof(StiFlowchartOffPageConnectorShapeType))
		{
			return new StiFlowchartOffPageConnectorGdiShapeTypePainter();
		}
		if (type == typeof(StiFlowchartPreparationShapeType))
		{
			return new StiFlowchartPreparationGdiShapeTypePainter();
		}
		if (type == typeof(StiFlowchartSortShapeType))
		{
			return new StiFlowchartSortGdiShapeTypePainter();
		}
		if (type == typeof(StiFrameShapeType))
		{
			return new StiFrameGdiShapeTypePainter();
		}
		if (type == typeof(StiHorizontalLineShapeType))
		{
			return new StiHorizontalLineGdiShapeTypePainter();
		}
		if (type == typeof(StiLeftAndRightLineShapeType))
		{
			return new StiLeftAndRightLineGdiShapeTypePainter();
		}
		if (type == typeof(StiMinusShapeType))
		{
			return new StiMinusGdiShapeTypePainter();
		}
		if (type == typeof(StiMultiplyShapeType))
		{
			return new StiMultiplyGdiShapeTypePainter();
		}
		if (type == typeof(StiOctagonShapeType))
		{
			return new StiOctagonGdiShapeTypePainter();
		}
		if (type == typeof(StiOvalShapeType))
		{
			return new StiOvalGdiShapeTypePainter();
		}
		if (type == typeof(StiParallelogramShapeType))
		{
			return new StiParallelogramGdiShapeTypePainter();
		}
		if (type == typeof(StiPlusShapeType))
		{
			return new StiPlusGdiShapeTypePainter();
		}
		if (type == typeof(StiRectangleShapeType))
		{
			return new StiRectangleGdiShapeTypePainter();
		}
		if (type == typeof(StiRegularPentagonShapeType))
		{
			return new StiRegularPentagonGdiShapeTypePainter();
		}
		if (type == typeof(StiRoundedRectangleShapeType))
		{
			return new StiRoundedRectangleGdiShapeTypePainter();
		}
		if (type == typeof(StiSnipDiagonalSideCornerRectangleShapeType))
		{
			return new StiSnipDiagonalSideCornerRectangleGdiShapeTypePainter();
		}
		if (type == typeof(StiSnipSameSideCornerRectangleShapeType))
		{
			return new StiSnipSameSideCornerRectangleGdiShapeTypePainter();
		}
		if (type == typeof(StiTopAndBottomLineShapeType))
		{
			return new StiTopAndBottomLineGdiShapeTypePainter();
		}
		if (type == typeof(StiTrapezoidShapeType))
		{
			return new StiTrapezoidGdiShapeTypePainter();
		}
		if (type == typeof(StiTriangleShapeType))
		{
			return new StiTriangleGdiShapeTypePainter();
		}
		if (type == typeof(StiVerticalLineShapeType))
		{
			return new StiVerticalLineGdiShapeTypePainter();
		}
		if (type == typeof(StiBentArrowShapeType))
		{
			return new StiBentArrowGdiShapeTypePainter();
		}
		if (!(type.GetCustomAttributes(typeof(StiGdiShapeTypePainterAttribute), inherit: true).FirstOrDefault() is StiGdiShapeTypePainterAttribute stiGdiShapeTypePainterAttribute))
		{
			return null;
		}
		return StiActivator.CreateObject(stiGdiShapeTypePainterAttribute.PainterTypeName, new object[0]) as StiShapeTypePainter;
	}

	public static StiIndicatorTypePainter GetIndicatorPainter(Type type, StiGuiMode guiMode)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Invalid comparison between Unknown and I4
		lock (lockObject)
		{
			Hashtable hashtable = (((int)guiMode == 0) ? typeToGdiPainter : typeToWpfPainter);
			StiIndicatorTypePainter stiIndicatorTypePainter = hashtable[type] as StiIndicatorTypePainter;
			if (stiIndicatorTypePainter != null)
			{
				return stiIndicatorTypePainter;
			}
			if ((int)guiMode != 0)
			{
				if ((int)guiMode == 1)
				{
					stiIndicatorTypePainter = GetWpfIndicatorTypePainter(type);
				}
			}
			else
			{
				stiIndicatorTypePainter = GetGdiIndicatorTypePainter(type);
			}
			if (stiIndicatorTypePainter == null)
			{
				throw new Exception($"StiPainter for '{type}' is not found!");
			}
			hashtable[type] = stiIndicatorTypePainter;
			return stiIndicatorTypePainter;
		}
	}

	private static StiIndicatorTypePainter GetWpfIndicatorTypePainter(Type type)
	{
		StiIndicatorTypePainter stiIndicatorTypePainter = ((StiWpfPainterDelegates.IndicatorPainter != null) ? StiWpfPainterDelegates.IndicatorPainter(type) : null);
		if (stiIndicatorTypePainter != null)
		{
			return stiIndicatorTypePainter;
		}
		string text = ((type.GetCustomAttributes(typeof(StiWpfIndicatorTypePainterAttribute), inherit: true).FirstOrDefault() is StiWpfIndicatorTypePainterAttribute stiWpfIndicatorTypePainterAttribute) ? stiWpfIndicatorTypePainterAttribute.PainterTypeName : null);
		return StiActivator.CreateObject(text, new object[0]) as StiIndicatorTypePainter;
	}

	private static StiIndicatorTypePainter GetGdiIndicatorTypePainter(Type type)
	{
		if (type == typeof(StiIconSetIndicator))
		{
			return new StiIconSetGdiIndicatorTypePainter();
		}
		if (type == typeof(StiDataBarIndicator))
		{
			return new StiDataBarGdiIndicatorTypePainter();
		}
		if (!(type.GetCustomAttributes(typeof(StiGdiIndicatorTypePainterAttribute), inherit: true).FirstOrDefault() is StiGdiIndicatorTypePainterAttribute stiGdiIndicatorTypePainterAttribute))
		{
			return null;
		}
		return StiActivator.CreateObject(stiGdiIndicatorTypePainterAttribute.PainterTypeName, new object[0]) as StiIndicatorTypePainter;
	}

	public abstract Image GetImage(StiComponent component, ref float zoom, StiExportFormat format);

	public abstract void Paint(StiComponent component, StiPaintEventArgs e);

	public abstract void PaintSelection(StiComponent component, StiPaintEventArgs e);

	public abstract void PaintHighlight(StiComponent component, StiPaintEventArgs e);

	public abstract Bitmap GetThumbnail(StiComponent component, int width, int height, bool isDesignTime);
}
