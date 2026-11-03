using System.Collections;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

public class StiPageV1Builder : StiContainerV1Builder
{
	internal void ProcessPageAfterRender(StiPage masterPage)
	{
		if (!masterPage.PageInfoV1.Processed)
		{
			return;
		}
		masterPage.PageInfoV1.Overlays = null;
		if (masterPage.Columns > 1)
		{
			foreach (StiComponent component in masterPage.Components)
			{
				if (component is StiContainer stiContainer && stiContainer.ContainerInfoV1.CurrentColumn > 0)
				{
					masterPage.Components.AddRange(stiContainer.Components);
					masterPage.Components.Remove(stiContainer);
					masterPage.Components.SetParent(masterPage);
					break;
				}
			}
			for (int i = 0; i < masterPage.Columns - 1; i++)
			{
				foreach (StiComponent component2 in masterPage.Components)
				{
					if (component2 is StiClone stiClone && stiClone.ContainerInfoV1.CurrentColumn > 0)
					{
						masterPage.Components.Remove(stiClone);
						break;
					}
				}
			}
		}
		masterPage.PageInfoV1.Processed = false;
	}

	internal void ProcessPageBeforeRender(StiPage masterPage)
	{
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		if (masterPage.PageInfoV1.Processed)
		{
			return;
		}
		foreach (StiComponent component2 in masterPage.Components)
		{
			if (component2 is StiOverlayBand component)
			{
				if (masterPage.PageInfoV1.Overlays == null)
				{
					masterPage.PageInfoV1.Overlays = new StiComponentsCollection(null);
				}
				masterPage.PageInfoV1.Overlays.Add(component);
			}
		}
		if (masterPage.Columns > 1)
		{
			StiContainer stiContainer = new StiContainer(new RectangleD(0.0, 0.0, 0.0, 0.0));
			stiContainer.ContainerInfoV1.CurrentColumn = 1;
			stiContainer.Name = "#Column#1";
			stiContainer.ContainerInfoV1.IsColumn = true;
			stiContainer.DockStyle = (masterPage.RightToLeft ? StiDockStyle.Right : StiDockStyle.Left);
			masterPage.Components.Add(stiContainer);
			stiContainer.Width = masterPage.GetColumnWidth() + masterPage.ColumnGaps;
			int num = 0;
			while (num < masterPage.Components.Count)
			{
				StiComponent stiComponent2 = masterPage.Components[num];
				if (stiContainer != stiComponent2)
				{
					if (stiComponent2.CanContainIn(stiContainer))
					{
						stiContainer.Components.Add(stiComponent2);
						masterPage.Components.Remove(stiComponent2);
						stiComponent2.Parent = stiContainer;
					}
					else
					{
						num++;
					}
				}
				else
				{
					num++;
				}
			}
			for (int i = 0; i < masterPage.Columns - 1; i++)
			{
				StiClone stiClone = new StiClone(new RectangleD(0.0, 0.0, masterPage.GetColumnWidth() + masterPage.ColumnGaps, 0.0));
				stiClone.ContainerInfoV1.IsColumn = true;
				stiClone.ContainerInfoV1.CurrentColumn = 2 + i;
				stiClone.Name = "#Column#" + (2 + i);
				stiClone.DockStyle = (masterPage.RightToLeft ? StiDockStyle.Right : StiDockStyle.Left);
				stiClone.Container = stiContainer;
				masterPage.Components.Add(stiClone);
			}
		}
		masterPage.PageInfoV1.Processed = true;
	}

	public override void Prepare(StiComponent masterComp)
	{
		StiPage stiPage = masterComp as StiPage;
		foreach (StiComponent component in stiPage.Components)
		{
			if (component is StiDataBand stiDataBand)
			{
				StiDataBandV1Builder stiDataBandV1Builder = StiV1Builder.GetBuilder(typeof(StiDataBand)) as StiDataBandV1Builder;
				stiDataBandV1Builder.SetDataIsPrepared(stiDataBand, value: false);
				stiDataBand.LineThrough = 1;
			}
		}
		base.Prepare(masterComp);
	}

	public override void UnPrepare(StiComponent masterComp)
	{
		base.UnPrepare(masterComp);
		StiPage stiPage = masterComp as StiPage;
		stiPage.PageInfoV1.Overlays = null;
	}

	public override bool InternalRender(StiComponent masterComp, ref StiComponent renderedComponent, StiContainer outContainer)
	{
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Invalid comparison between Unknown and I4
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Invalid comparison between Unknown and I4
		StiPage stiPage = masterComp as StiPage;
		stiPage.PageInfoV1.IsFirstDataBandOnPage = true;
		if (stiPage.PageInfoV1.Overlays != null)
		{
			ArrayList arrayList = new ArrayList();
			ArrayList arrayList2 = new ArrayList();
			ArrayList arrayList3 = new ArrayList();
			foreach (StiOverlayBand overlay in stiPage.PageInfoV1.Overlays)
			{
				overlay.Render(ref renderedComponent, outContainer);
				renderedComponent.DockStyle = StiDockStyle.None;
				if ((int)overlay.VertAlignment == 0)
				{
					arrayList.Add(renderedComponent);
				}
				else if ((int)overlay.VertAlignment == 1)
				{
					arrayList2.Add(renderedComponent);
				}
				else if ((int)overlay.VertAlignment == 2)
				{
					arrayList3.Add(renderedComponent);
				}
				renderedComponent = null;
			}
			double num = 0.0 - stiPage.Margins.Top;
			foreach (StiComponent item in arrayList)
			{
				item.Top = num;
				num += item.Height;
			}
			double num2 = stiPage.Height + stiPage.Margins.Bottom;
			foreach (StiComponent item2 in arrayList3)
			{
				item2.Top = num2 - item2.Height;
				num2 -= item2.Height;
			}
			double num3 = 0.0;
			foreach (StiComponent item3 in arrayList2)
			{
				num3 += item3.Height;
			}
			num = (stiPage.Height - num3) / 2.0;
			foreach (StiComponent item4 in arrayList2)
			{
				item4.Top = num;
				num += item4.Height;
			}
		}
		stiPage.PageInfoV1.DelimiterComponentsLeft.Clear();
		renderedComponent = outContainer;
		stiPage.InvokeEvents();
		bool result = base.RenderContainer(stiPage, ref renderedComponent, outContainer);
		if (outContainer != null)
		{
			outContainer.PointerValue = stiPage.PointerValue;
			outContainer.BookmarkValue = stiPage.BookmarkValue;
			outContainer.TagValue = stiPage.TagValue;
			outContainer.ToolTipValue = stiPage.ToolTipValue;
			outContainer.HyperlinkValue = stiPage.HyperlinkValue;
			if (outContainer is StiPage)
			{
				((StiPage)outContainer).ExcelSheetValue = stiPage.ExcelSheetValue;
			}
		}
		stiPage.PointerValue = null;
		stiPage.BookmarkValue = null;
		stiPage.TagValue = null;
		stiPage.ToolTipValue = null;
		stiPage.HyperlinkValue = null;
		return result;
	}
}
