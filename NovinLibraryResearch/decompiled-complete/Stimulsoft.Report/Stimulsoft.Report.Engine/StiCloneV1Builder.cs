using System.Collections;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

public class StiCloneV1Builder : StiContainerV1Builder
{
	private void SetParent(StiClone masterClone)
	{
		masterClone.CloneInfoV1.ResBottomRenderedFooters = masterClone.ContainerInfoV1.BottomRenderedFooters;
		masterClone.CloneInfoV1.ResBottomRenderedGroupFooters = masterClone.ContainerInfoV1.BottomRenderedGroupFooters;
		masterClone.CloneInfoV1.ResBottomRenderedDataBands = masterClone.ContainerInfoV1.BottomRenderedDataBands;
		masterClone.CloneInfoV1.ResBottomRenderedHeaders = masterClone.ContainerInfoV1.BottomRenderedHeaders;
		masterClone.CloneInfoV1.ResBottomRenderedParentsFooters = masterClone.ContainerInfoV1.BottomRenderedParentsFooters;
		masterClone.CloneInfoV1.ResBottomRenderedParentsGroupFooters = masterClone.ContainerInfoV1.BottomRenderedParentsGroupFooters;
		masterClone.CloneInfoV1.ResBottomRenderedParentsDataBands = masterClone.ContainerInfoV1.BottomRenderedParentsDataBands;
		masterClone.CloneInfoV1.ResBottomRenderedParentsHeaders = masterClone.ContainerInfoV1.BottomRenderedParentsHeaders;
		masterClone.CloneInfoV1.ResLastDataBand = masterClone.ContainerInfoV1.LastDataBand;
		masterClone.ParentBookmark = masterClone.Container.ParentBookmark;
		masterClone.CurrentBookmark = masterClone.Container.CurrentBookmark;
		double num = masterClone.Width / masterClone.Container.Width;
		if (masterClone.ScaleHor)
		{
			masterClone.CloneInfoV1.WidthHor = new Hashtable();
			masterClone.CloneInfoV1.LeftHor = new Hashtable();
		}
		StiComponentsCollection components = masterClone.Container.GetComponents();
		foreach (StiComponent item in components)
		{
			if (item.Parent == masterClone.Container)
			{
				item.Parent = masterClone;
			}
			item.Page = masterClone.Page;
			if (masterClone.ScaleHor)
			{
				masterClone.CloneInfoV1.WidthHor[item] = item.Width;
				masterClone.CloneInfoV1.LeftHor[item] = item.Left;
				item.Width *= num;
				item.Left *= num;
			}
		}
	}

	private void RestoreParent(StiClone masterClone)
	{
		masterClone.ContainerInfoV1.BottomRenderedFooters = masterClone.CloneInfoV1.ResBottomRenderedFooters;
		masterClone.ContainerInfoV1.BottomRenderedGroupFooters = masterClone.CloneInfoV1.ResBottomRenderedGroupFooters;
		masterClone.ContainerInfoV1.BottomRenderedDataBands = masterClone.CloneInfoV1.ResBottomRenderedDataBands;
		masterClone.ContainerInfoV1.BottomRenderedHeaders = masterClone.CloneInfoV1.ResBottomRenderedHeaders;
		masterClone.ContainerInfoV1.BottomRenderedParentsFooters = masterClone.CloneInfoV1.ResBottomRenderedParentsFooters;
		masterClone.ContainerInfoV1.BottomRenderedParentsGroupFooters = masterClone.CloneInfoV1.ResBottomRenderedParentsGroupFooters;
		masterClone.ContainerInfoV1.BottomRenderedParentsDataBands = masterClone.CloneInfoV1.ResBottomRenderedParentsDataBands;
		masterClone.ContainerInfoV1.BottomRenderedParentsHeaders = masterClone.CloneInfoV1.ResBottomRenderedParentsHeaders;
		masterClone.ContainerInfoV1.LastDataBand = masterClone.CloneInfoV1.ResLastDataBand;
		StiComponentsCollection components = masterClone.Container.GetComponents();
		foreach (StiComponent item in components)
		{
			if (item.Parent == masterClone)
			{
				item.Parent = masterClone.Container;
			}
			item.Page = masterClone.Container.Page;
			if (masterClone.ScaleHor)
			{
				item.Width = (double)masterClone.CloneInfoV1.WidthHor[item];
				item.Left = (double)masterClone.CloneInfoV1.LeftHor[item];
			}
		}
	}

	public override bool InternalRender(StiComponent masterComp, ref StiComponent renderedComponent, StiContainer outContainer)
	{
		StiClone stiClone = masterComp as StiClone;
		if (stiClone.Container != null)
		{
			SetParent(stiClone);
			bool result = base.InternalRender((StiComponent)stiClone, ref renderedComponent, outContainer);
			RestoreParent(stiClone);
			CheckBandsAtBottom(stiClone, renderedComponent, outContainer);
			return result;
		}
		return true;
	}
}
