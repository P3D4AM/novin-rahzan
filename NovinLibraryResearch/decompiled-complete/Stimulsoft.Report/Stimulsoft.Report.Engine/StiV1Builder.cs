using System;
using System.Collections;
using Stimulsoft.Base;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.CrossTab;

namespace Stimulsoft.Report.Engine;

public abstract class StiV1Builder : StiBuilder
{
	private static Hashtable typeToV1Builder = new Hashtable();

	public static StiV1Builder GetBuilder(Type componentType)
	{
		StiV1Builder stiV1Builder = typeToV1Builder[componentType] as StiV1Builder;
		if (stiV1Builder == null)
		{
			if (!StiOptions.Engine.FullTrust)
			{
				if (componentType == typeof(StiChart) || componentType.IsSubclassOf(typeof(StiChart)))
				{
					return new StiChartV1Builder();
				}
				if (componentType == typeof(StiClone) || componentType.IsSubclassOf(typeof(StiClone)))
				{
					return new StiCloneV1Builder();
				}
				if (componentType == typeof(StiCrossDataBand) || componentType.IsSubclassOf(typeof(StiCrossDataBand)))
				{
					return new StiCrossDataBandV1Builder();
				}
				if (componentType == typeof(StiCrossTab) || componentType.IsSubclassOf(typeof(StiCrossTab)))
				{
					return new StiCrossTabV1Builder();
				}
				if (componentType == typeof(StiDataBand) || componentType.IsSubclassOf(typeof(StiDataBand)))
				{
					return new StiDataBandV1Builder();
				}
				if (componentType == typeof(StiFooterBand) || componentType.IsSubclassOf(typeof(StiFooterBand)))
				{
					return new StiFooterBandV1Builder();
				}
				if (componentType == typeof(StiGroupFooterBand) || componentType.IsSubclassOf(typeof(StiGroupFooterBand)))
				{
					return new StiGroupFooterBandV1Builder();
				}
				if (componentType == typeof(StiGroupHeaderBand) || componentType.IsSubclassOf(typeof(StiGroupHeaderBand)))
				{
					return new StiGroupHeaderBandV1Builder();
				}
				if (componentType == typeof(StiHeaderBand) || componentType.IsSubclassOf(typeof(StiHeaderBand)))
				{
					return new StiHeaderBandV1Builder();
				}
				if (componentType == typeof(StiHierarchicalBand) || componentType.IsSubclassOf(typeof(StiHierarchicalBand)))
				{
					return new StiHierarchicalBandV1Builder();
				}
				if (componentType == typeof(StiImage) || componentType.IsSubclassOf(typeof(StiImage)))
				{
					return new StiImageV1Builder();
				}
				if (componentType == typeof(StiPage) || componentType.IsSubclassOf(typeof(StiPage)))
				{
					return new StiPageV1Builder();
				}
				if (componentType == typeof(StiPointPrimitive) || componentType.IsSubclassOf(typeof(StiPointPrimitive)))
				{
					return new StiPointPrimitiveV1Builder();
				}
				if (componentType == typeof(StiReportSummaryBand) || componentType.IsSubclassOf(typeof(StiReportSummaryBand)))
				{
					return new StiReportSummaryBandV1Builder();
				}
				if (componentType == typeof(StiReportTitleBand) || componentType.IsSubclassOf(typeof(StiReportTitleBand)))
				{
					return new StiReportTitleBandV1Builder();
				}
				if (componentType == typeof(StiSubReport) || componentType.IsSubclassOf(typeof(StiSubReport)))
				{
					return new StiSubReportV1Builder();
				}
				if (componentType == typeof(StiTextInCells) || componentType.IsSubclassOf(typeof(StiTextInCells)))
				{
					return new StiTextInCellsV1Builder();
				}
				if (componentType == typeof(StiSimpleText) || componentType.IsSubclassOf(typeof(StiSimpleText)))
				{
					return new StiSimpleTextV1Builder();
				}
				if (componentType == typeof(StiWinControl) || componentType.IsSubclassOf(typeof(StiWinControl)))
				{
					return new StiWinControlV1Builder();
				}
				if (componentType == typeof(StiView) || componentType.IsSubclassOf(typeof(StiView)))
				{
					return new StiViewV1Builder();
				}
				if (componentType == typeof(StiBand) || componentType.IsSubclassOf(typeof(StiBand)))
				{
					return new StiBandV1Builder();
				}
				if (componentType == typeof(StiContainer) || componentType.IsSubclassOf(typeof(StiContainer)))
				{
					return new StiContainerV1Builder();
				}
				if (componentType == typeof(StiComponent) || componentType.IsSubclassOf(typeof(StiComponent)))
				{
					return new StiComponentV1Builder();
				}
			}
			if (componentType.GetCustomAttributes(typeof(StiV1BuilderAttribute), inherit: true) is StiV1BuilderAttribute[] array && array.Length != 0)
			{
				stiV1Builder = StiActivator.CreateObject(array[0].BuilderTypeName, new object[0]) as StiV1Builder;
			}
			if (stiV1Builder == null)
			{
				throw new Exception("StiBuilder for '" + componentType.ToString() + "' is not found!");
			}
			typeToV1Builder[componentType] = stiV1Builder;
		}
		return stiV1Builder;
	}

	public abstract bool InternalRender(StiComponent masterComp, ref StiComponent renderedComponent, StiContainer outContainer);

	public abstract bool Render(StiComponent masterComp, ref StiComponent renderedComponent, StiContainer outContainer);

	public abstract bool Render(StiComponent masterComp, StiContainer outContainer);

	public abstract void Prepare(StiComponent masterComp);

	public abstract void UnPrepare(StiComponent masterComp);
}
