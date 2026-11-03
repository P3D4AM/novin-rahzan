using System;
using System.Collections;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

public class StiContainerV2Builder : StiComponentV2Builder
{
	public virtual StiContainer GetRenderContainer(StiComponent comp, Type type = null)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		StiContainer stiContainer;
		if (type != null)
		{
			stiContainer = Activator.CreateInstance(type, comp.ClientRectangle) as StiContainer;
		}
		else if (comp is StiPanel)
		{
			stiContainer = new StiPanel(comp.ClientRectangle);
			((StiPanel)stiContainer).Columns = ((StiPanel)comp).Columns;
		}
		else
		{
			stiContainer = new StiContainer(comp.ClientRectangle);
		}
		if (!comp.IsGetPointerHandlerEmpty)
		{
			stiContainer.GetPointer += comp.InvokeGetPointer;
		}
		if (!comp.IsGetBookmarkHandlerEmpty)
		{
			stiContainer.GetBookmark += comp.InvokeGetBookmark;
		}
		if (!comp.IsGetTagHandlerEmpty)
		{
			stiContainer.GetTag += comp.InvokeGetTag;
		}
		if (!comp.IsClickHandlerEmpty)
		{
			stiContainer.Click += comp.InvokeClick;
		}
		if (!comp.IsMouseEnterHandlerEmpty)
		{
			stiContainer.MouseEnter += comp.InvokeMouseEnter;
		}
		if (!comp.IsMouseLeaveHandlerEmpty)
		{
			stiContainer.MouseLeave += comp.InvokeMouseLeave;
		}
		stiContainer.AfterPrint += comp.InvokeAfterPrint;
		stiContainer.BeforePrint += comp.InvokeBeforePrint;
		stiContainer.MinSize = comp.MinSize;
		stiContainer.MaxSize = comp.MaxSize;
		stiContainer.DockStyle = comp.DockStyle;
		stiContainer.Name = comp.Name;
		stiContainer.ParentComponentIsBand = comp is StiBand;
		stiContainer.ParentComponentIsCrossBand = comp is StiCrossDataBand;
		stiContainer.CanBreak = ((StiContainer)comp).CanBreak;
		stiContainer.CanGrow = comp.CanGrow;
		stiContainer.CanShrink = comp.CanShrink;
		stiContainer.GrowToHeight = comp.GrowToHeight;
		stiContainer.ShiftMode = comp.ShiftMode;
		stiContainer.Printable = comp.Printable;
		stiContainer.PrintOn = comp.PrintOn;
		stiContainer.ComponentStyle = comp.ComponentStyle;
		stiContainer.CurrentBookmark = comp.CurrentBookmark;
		stiContainer.ParentBookmark = comp.ParentBookmark;
		stiContainer.CurrentPointer = comp.CurrentPointer;
		stiContainer.ParentPointer = comp.ParentPointer;
		stiContainer.Guid = comp.Guid;
		stiContainer.Interaction = comp.Interaction;
		stiContainer.DrillDownParameters = comp.DrillDownParameters;
		if (comp is StiGroupHeaderBand)
		{
			stiContainer.CollapsingIndex = ((StiGroupHeaderBand)comp).CollapsingIndex;
			stiContainer.CollapsedValue = ((StiGroupHeaderBand)comp).CollapsedValue;
		}
		if (comp is StiDataBand)
		{
			stiContainer.CollapsingIndex = ((StiDataBand)comp).CollapsingIndex;
			stiContainer.CollapsedValue = ((StiDataBand)comp).CollapsedValue;
			stiContainer.CollapsingTreePath = ((StiDataBand)comp).CollapsingTreePath;
		}
		if (comp is StiBand stiBand)
		{
			if (stiBand.BandInfoV2.ForceCanBreak)
			{
				stiContainer.CanBreak = true;
			}
			if (stiBand.BandInfoV2.ForceCanGrow)
			{
				stiContainer.CanGrow = true;
			}
		}
		StiContainer stiContainer2 = comp as StiContainer;
		StiContainer stiContainer3 = stiContainer;
		object obj = stiContainer2.Brush.Clone();
		stiContainer3.Brush = (StiBrush)((obj is StiBrush) ? obj : null);
		StiContainer stiContainer4 = stiContainer;
		object obj2 = stiContainer2.Border.Clone();
		stiContainer4.Border = (StiBorder)((obj2 is StiBorder) ? obj2 : null);
		if (StiOptions.Export.OptimizeDataOnlyMode)
		{
			stiContainer.ComponentPlacement = comp.ComponentPlacement;
		}
		return stiContainer;
	}

	public override StiComponent InternalRender(StiComponent masterComp)
	{
		StiContainer stiContainer = masterComp as StiContainer;
		bool flag = false;
		StiContainer renderContainer = GetRenderContainer(stiContainer);
		if (stiContainer.Report.CalculationMode == StiCalculationMode.Interpretation)
		{
			renderContainer.Pointer = stiContainer.Pointer;
			renderContainer.Bookmark = stiContainer.Bookmark;
			renderContainer.Hyperlink = stiContainer.Hyperlink;
			renderContainer.Tag = stiContainer.Tag;
		}
		if (!(stiContainer is IStiComponentsOwnerRenderer))
		{
			lock (((ICollection)stiContainer.Components).SyncRoot)
			{
				foreach (StiComponent component in stiContainer.Components)
				{
					if (component.ParentBookmark == null)
					{
						component.ParentBookmark = stiContainer.CurrentBookmark;
					}
					if (component.ParentPointer == null)
					{
						component.ParentPointer = stiContainer.CurrentPointer;
					}
					if (component.ComponentType != 0)
					{
						continue;
					}
					component.ParentBookmark = stiContainer.CurrentBookmark;
					component.ParentPointer = stiContainer.CurrentPointer;
					StiComponent stiComponent2 = component.Render();
					if (stiComponent2 == null)
					{
						continue;
					}
					if (stiComponent2 is StiContainer stiContainer2 && stiContainer2.ContainerInfoV2.SetSegmentPerWidth != -1)
					{
						renderContainer.ContainerInfoV2.SetSegmentPerWidth = stiContainer2.ContainerInfoV2.SetSegmentPerWidth;
					}
					renderContainer.Components.Add(stiComponent2);
					if (!StiOptions.Engine.AllowInteractionInChartWithComponents || !(stiComponent2 is StiChart stiChart) || stiChart.ChartInfoV2.InteractiveComps == null)
					{
						continue;
					}
					foreach (StiText interactiveComp in stiChart.ChartInfoV2.InteractiveComps)
					{
						interactiveComp.Left += stiComponent2.Left;
						interactiveComp.Top += stiComponent2.Top;
						renderContainer.Components.Add(interactiveComp);
					}
				}
			}
		}
		if (!stiContainer.Report.Engine.DenyRenderMasterComponentsInContainer && !(stiContainer is IStiComponentsOwnerRenderer))
		{
			lock (((ICollection)stiContainer.Components).SyncRoot)
			{
				foreach (StiComponent component2 in stiContainer.Components)
				{
					component2.ParentBookmark = stiContainer.CurrentBookmark;
					component2.ParentPointer = stiContainer.CurrentPointer;
					if (component2.ComponentType == StiComponentType.Master)
					{
						component2.Render();
						if (component2 is StiDataBand)
						{
							flag = true;
						}
					}
				}
			}
		}
		if (flag)
		{
			bool skipStaticBands = false;
			StiContainer stiContainer3 = stiContainer;
			while (true)
			{
				if (stiContainer3 is StiPageHeaderBand || stiContainer3 is StiPageFooterBand || stiContainer3 is StiReportTitleBand || stiContainer3 is StiReportSummaryBand)
				{
					skipStaticBands = true;
					break;
				}
				if (stiContainer3 == null || stiContainer3 is StiPage)
				{
					break;
				}
				stiContainer3 = stiContainer3.Parent;
			}
			StiSubReportsHelper.RenderDataBandsInContainer(renderContainer, stiContainer, skipStaticBands);
		}
		double width = renderContainer.Width;
		StiContainerHelper.CheckSize(renderContainer);
		if (!renderContainer.ParentComponentIsCrossBand)
		{
			renderContainer.Width = width;
		}
		return renderContainer;
	}
}
