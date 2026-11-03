using System;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

public class StiContainerV1Builder : StiComponentV1Builder
{
	public static StiComponentsCollection GetClones(StiContainer masterContainer)
	{
		StiComponentsCollection components = masterContainer.Page.GetComponents();
		StiComponentsCollection stiComponentsCollection = new StiComponentsCollection(null);
		foreach (StiComponent item in components)
		{
			if (item is StiClone && ((StiClone)item).Container == masterContainer)
			{
				stiComponentsCollection.Add(item);
			}
		}
		return stiComponentsCollection;
	}

	public static StiContainer GetRenderContainer(StiContainer outContainer, StiComponent comp)
	{
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		StiContainer stiContainer = null;
		StiContainer stiContainer2 = comp as StiContainer;
		if (stiContainer2.ContainerInfoV1.IsColumn && comp.Page.Columns > 1 && comp.Page.PrintOnPreviousPage && comp.Page.PrintHeadersFootersFromPreviousPage)
		{
			foreach (StiComponent component in outContainer.Components)
			{
				if (component.Name == comp.Name)
				{
					return component as StiContainer;
				}
			}
		}
		RectangleD clientRectangle = comp.ClientRectangle;
		double x = ((RectangleD)(ref clientRectangle)).X;
		clientRectangle = comp.ClientRectangle;
		double y = ((RectangleD)(ref clientRectangle)).Y;
		clientRectangle = comp.ClientRectangle;
		double width = ((RectangleD)(ref clientRectangle)).Width;
		clientRectangle = comp.ClientRectangle;
		RectangleD rect = default(RectangleD);
		((RectangleD)(ref rect))._002Ector(x, y, width, ((RectangleD)(ref clientRectangle)).Height);
		stiContainer = new StiContainer(rect);
		stiContainer.GetPointer += comp.InvokeGetPointer;
		stiContainer.GetBookmark += comp.InvokeGetBookmark;
		stiContainer.GetTag += comp.InvokeGetTag;
		stiContainer.Click += comp.InvokeClick;
		stiContainer.MouseEnter += comp.InvokeMouseEnter;
		stiContainer.MouseLeave += comp.InvokeMouseLeave;
		stiContainer.AfterPrint += comp.InvokeAfterPrint;
		stiContainer.BeforePrint += comp.InvokeBeforePrint;
		stiContainer.MinSize = comp.MinSize;
		stiContainer.MaxSize = comp.MaxSize;
		stiContainer.DockStyle = comp.DockStyle;
		stiContainer.Name = comp.Name;
		stiContainer.ParentComponentIsBand = comp is StiBand;
		stiContainer.CanGrow = comp.CanGrow;
		stiContainer.CanShrink = comp.CanShrink;
		stiContainer.GrowToHeight = comp.GrowToHeight;
		stiContainer.ShiftMode = comp.ShiftMode;
		stiContainer.Printable = comp.Printable;
		stiContainer.ComponentStyle = comp.ComponentStyle;
		if (comp is StiContainer)
		{
			stiContainer.CollapsingIndex = ((StiContainer)comp).CollapsingIndex;
		}
		stiContainer.Guid = comp.Guid;
		stiContainer.ContainerInfoV1.IsColumn = stiContainer2.ContainerInfoV1.IsColumn;
		StiContainer stiContainer3 = stiContainer;
		object obj = stiContainer2.Brush.Clone();
		stiContainer3.Brush = (StiBrush)((obj is StiBrush) ? obj : null);
		StiContainer stiContainer4 = stiContainer;
		object obj2 = stiContainer2.Border.Clone();
		stiContainer4.Border = (StiBorder)((obj2 is StiBorder) ? obj2 : null);
		stiContainer.Interaction = comp.Interaction;
		return stiContainer;
	}

	public void CheckBandsAtBottom(StiContainer masterContainer, StiComponent renderedComponent, StiContainer outContainer)
	{
		bool flag = RenderEmptyBands(masterContainer, renderedComponent as StiContainer);
		RenderGroupFootersAtBottom(masterContainer, !flag);
		RenderFootersAtBottom(masterContainer, !flag);
		RenderDataBandsAtBottom(masterContainer, outContainer, !flag);
		RenderHeadersAtBottom(masterContainer, !flag);
	}

	public void FirstComponent(StiContainer masterContainer)
	{
		foreach (StiComponent component in masterContainer.Components)
		{
			if (component.ComponentType == StiComponentType.Master)
			{
				masterContainer.ContainerInfoV1.CurrentComponent = component;
				return;
			}
		}
		masterContainer.ContainerInfoV1.CurrentComponent = null;
	}

	public void NextComponent(StiContainer masterContainer)
	{
		for (int i = masterContainer.Components.IndexOf(masterContainer.ContainerInfoV1.CurrentComponent) + 1; i < masterContainer.Components.Count; i++)
		{
			if (masterContainer.Components[i].ComponentType == StiComponentType.Master)
			{
				masterContainer.ContainerInfoV1.CurrentComponent = masterContainer.Components[i];
				return;
			}
		}
		masterContainer.ContainerInfoV1.CurrentComponent = null;
	}

	public bool InternalCoreRenderContainer(StiContainer masterContainer, ref StiComponent renderedComponent, StiContainer outContainer)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		bool result = true;
		masterContainer.IsRendered = true;
		if (renderedComponent == null)
		{
			renderedComponent = GetRenderContainer(outContainer, masterContainer);
		}
		if (renderedComponent != outContainer)
		{
			outContainer.Components.Add(renderedComponent);
			if (renderedComponent.Dockable)
			{
				renderedComponent.DisplayRectangle = renderedComponent.DockToContainer(renderedComponent.DisplayRectangle);
			}
		}
		foreach (StiComponent component in masterContainer.Components)
		{
			if (component.ComponentType == StiComponentType.Simple || component.ComponentType == StiComponentType.Static)
			{
				component.ParentBookmark = masterContainer.CurrentBookmark;
				if (!component.Render(renderedComponent as StiContainer))
				{
					result = false;
				}
			}
		}
		StiContainerHelper.CheckSize(renderedComponent);
		if (outContainer is StiPage && masterContainer is StiPage)
		{
			StiPage stiPage = outContainer as StiPage;
			RectangleD dockRegion = outContainer.GetDockRegion(outContainer);
			stiPage.PageInfoV1.TopPageContentPosition = ((RectangleD)(ref dockRegion)).Y;
			stiPage.PageInfoV1.BottomPageContentPosition = ((RectangleD)(ref dockRegion)).Bottom;
		}
		do
		{
			if (masterContainer.ContainerInfoV1.CurrentComponent == null)
			{
				FirstComponent(masterContainer);
				if (masterContainer.ContainerInfoV1.CurrentComponent == null)
				{
					return result;
				}
			}
			masterContainer.ContainerInfoV1.CurrentComponent.ParentBookmark = masterContainer.CurrentBookmark;
			if (!masterContainer.ContainerInfoV1.CurrentComponent.Render(renderedComponent as StiContainer))
			{
				return false;
			}
			NextComponent(masterContainer);
		}
		while (masterContainer.ContainerInfoV1.CurrentComponent != null);
		return result;
	}

	public virtual bool InternalRenderContainer(StiContainer masterContainer, ref StiComponent renderedComponent, StiContainer outContainer)
	{
		bool result = InternalCoreRenderContainer(masterContainer, ref renderedComponent, outContainer);
		CheckBandsAtBottom(masterContainer, renderedComponent, outContainer);
		return result;
	}

	public virtual bool RenderContainer(StiContainer masterContainer, ref StiComponent renderedComponent, StiContainer outContainer)
	{
		bool flag = true;
		flag = InternalRenderContainer(masterContainer, ref renderedComponent, outContainer);
		if (masterContainer is StiBand && !(masterContainer is StiChildBand))
		{
			StiBandV1Builder.RenderChilds(masterContainer as StiBand, (StiContainer)renderedComponent);
		}
		if (!(masterContainer is StiPage))
		{
			renderedComponent.InvokeEvents();
		}
		return flag;
	}

	private bool RenderEmptyBands(StiContainer masterContainer, StiContainer outContainer)
	{
		StiEmptyBand stiEmptyBand = null;
		StiComponentsCollection stiComponentsCollection = null;
		foreach (StiComponent component in masterContainer.Components)
		{
			if (component is StiEmptyBand { Height: >0.0, IsEnabled: not false } stiEmptyBand2)
			{
				if (stiComponentsCollection == null)
				{
					stiComponentsCollection = new StiComponentsCollection();
				}
				stiComponentsCollection.Add(stiEmptyBand2);
			}
		}
		if (stiComponentsCollection != null && stiComponentsCollection.Count > 0)
		{
			stiEmptyBand = stiComponentsCollection[0] as StiEmptyBand;
		}
		if (stiEmptyBand != null)
		{
			StiEmptyBandsV1Helper.Render(masterContainer.Report, outContainer, stiEmptyBand);
			return true;
		}
		return false;
	}

	private void RenderDataBandsAtBottom(StiContainer masterContainer, StiContainer outContainer, bool dockToBottom)
	{
		if (masterContainer.ContainerInfoV1.BottomRenderedParentsDataBands == null)
		{
			return;
		}
		foreach (StiDataBand bottomRenderedParentsDataBand in masterContainer.ContainerInfoV1.BottomRenderedParentsDataBands)
		{
			if (!bottomRenderedParentsDataBand.PrintAtBottom)
			{
				continue;
			}
			StiComponentsCollection stiComponentsCollection = masterContainer.ContainerInfoV1.BottomRenderedDataBands[bottomRenderedParentsDataBand] as StiComponentsCollection;
			for (int num = stiComponentsCollection.Count - 1; num >= 0; num--)
			{
				StiComponent stiComponent = stiComponentsCollection[num];
				StiContainer parent = stiComponent.Parent;
				if (parent != null && parent.Components.IndexOf(stiComponent) != -1)
				{
					parent.Components.Remove(stiComponent);
					parent.Components.Add(stiComponent);
					if (dockToBottom)
					{
						stiComponent.DockStyle = StiDockStyle.Bottom;
					}
				}
			}
		}
		masterContainer.ContainerInfoV1.BottomRenderedParentsDataBands.Clear();
		masterContainer.ContainerInfoV1.BottomRenderedParentsDataBands = null;
		masterContainer.ContainerInfoV1.BottomRenderedDataBands.Clear();
		masterContainer.ContainerInfoV1.BottomRenderedDataBands = null;
	}

	private void RenderHeadersAtBottom(StiContainer masterContainer, bool dockToBottom)
	{
		if (masterContainer.ContainerInfoV1.BottomRenderedParentsHeaders == null)
		{
			return;
		}
		foreach (StiHeaderBand bottomRenderedParentsHeader in masterContainer.ContainerInfoV1.BottomRenderedParentsHeaders)
		{
			if (!bottomRenderedParentsHeader.PrintAtBottom)
			{
				continue;
			}
			StiComponentsCollection stiComponentsCollection = masterContainer.ContainerInfoV1.BottomRenderedHeaders[bottomRenderedParentsHeader] as StiComponentsCollection;
			for (int num = stiComponentsCollection.Count - 1; num >= 0; num--)
			{
				StiComponent stiComponent = stiComponentsCollection[num];
				StiContainer parent = stiComponent.Parent;
				if (parent != null && parent.Components.IndexOf(stiComponent) != -1)
				{
					parent.Components.Remove(stiComponent);
					parent.Components.Add(stiComponent);
					if (dockToBottom)
					{
						stiComponent.DockStyle = StiDockStyle.Bottom;
					}
					break;
				}
			}
		}
		masterContainer.ContainerInfoV1.BottomRenderedParentsHeaders.Clear();
		masterContainer.ContainerInfoV1.BottomRenderedParentsHeaders = null;
		masterContainer.ContainerInfoV1.BottomRenderedHeaders.Clear();
		masterContainer.ContainerInfoV1.BottomRenderedHeaders = null;
	}

	private void RenderFootersAtBottom(StiContainer masterContainer, bool dockToBottom)
	{
		if (masterContainer.ContainerInfoV1.BottomRenderedParentsFooters == null)
		{
			return;
		}
		if (!dockToBottom)
		{
			for (int num = masterContainer.ContainerInfoV1.BottomRenderedParentsFooters.Count - 1; num >= 0; num--)
			{
				StiFooterBand footer = masterContainer.ContainerInfoV1.BottomRenderedParentsFooters[num] as StiFooterBand;
				RenderOneFooterAtBottom(masterContainer, dockToBottom, footer);
			}
		}
		else
		{
			foreach (StiFooterBand bottomRenderedParentsFooter in masterContainer.ContainerInfoV1.BottomRenderedParentsFooters)
			{
				RenderOneFooterAtBottom(masterContainer, dockToBottom, bottomRenderedParentsFooter);
			}
		}
		masterContainer.ContainerInfoV1.BottomRenderedParentsFooters.Clear();
		masterContainer.ContainerInfoV1.BottomRenderedParentsFooters = null;
		masterContainer.ContainerInfoV1.BottomRenderedFooters.Clear();
		masterContainer.ContainerInfoV1.BottomRenderedFooters = null;
	}

	private void RenderOneFooterAtBottom(StiContainer masterContainer, bool dockToBottom, StiFooterBand footer)
	{
		if (!footer.PrintAtBottom)
		{
			return;
		}
		StiComponentsCollection stiComponentsCollection = masterContainer.ContainerInfoV1.BottomRenderedFooters[footer] as StiComponentsCollection;
		for (int num = stiComponentsCollection.Count - 1; num >= 0; num--)
		{
			StiComponent stiComponent = stiComponentsCollection[num];
			StiContainer parent = stiComponent.Parent;
			if (parent != null && parent.Components.IndexOf(stiComponent) != -1)
			{
				parent.Components.Remove(stiComponent);
				parent.Components.Add(stiComponent);
				if (dockToBottom)
				{
					stiComponent.DockStyle = StiDockStyle.Bottom;
				}
				break;
			}
		}
	}

	private void RenderGroupFootersAtBottom(StiContainer masterContainer, bool dockToBottom)
	{
		if (masterContainer.ContainerInfoV1.BottomRenderedParentsGroupFooters == null)
		{
			return;
		}
		if (!dockToBottom)
		{
			for (int num = masterContainer.ContainerInfoV1.BottomRenderedParentsGroupFooters.Count - 1; num >= 0; num--)
			{
				StiGroupFooterBand footer = masterContainer.ContainerInfoV1.BottomRenderedParentsGroupFooters[num] as StiGroupFooterBand;
				RenderOneGroupFooterAtBottom(masterContainer, dockToBottom, footer);
			}
		}
		else
		{
			foreach (StiGroupFooterBand bottomRenderedParentsGroupFooter in masterContainer.ContainerInfoV1.BottomRenderedParentsGroupFooters)
			{
				RenderOneGroupFooterAtBottom(masterContainer, dockToBottom, bottomRenderedParentsGroupFooter);
			}
		}
		masterContainer.ContainerInfoV1.BottomRenderedParentsGroupFooters.Clear();
		masterContainer.ContainerInfoV1.BottomRenderedParentsGroupFooters = null;
		masterContainer.ContainerInfoV1.BottomRenderedGroupFooters.Clear();
		masterContainer.ContainerInfoV1.BottomRenderedGroupFooters = null;
	}

	private void RenderOneGroupFooterAtBottom(StiContainer masterContainer, bool dockToBottom, StiGroupFooterBand footer)
	{
		if (!footer.PrintAtBottom)
		{
			return;
		}
		StiComponentsCollection stiComponentsCollection = masterContainer.ContainerInfoV1.BottomRenderedGroupFooters[footer] as StiComponentsCollection;
		for (int num = stiComponentsCollection.Count - 1; num >= 0; num--)
		{
			StiComponent stiComponent = stiComponentsCollection[num];
			StiContainer parent = stiComponent.Parent;
			if (parent != null && parent.Components.IndexOf(stiComponent) != -1)
			{
				parent.Components.Remove(stiComponent);
				parent.Components.Add(stiComponent);
				if (dockToBottom)
				{
					stiComponent.DockStyle = StiDockStyle.Bottom;
				}
				break;
			}
		}
	}

	public override void Prepare(StiComponent masterComp)
	{
		base.Prepare(masterComp);
		StiContainer stiContainer = masterComp as StiContainer;
		foreach (StiComponent component in stiContainer.Components)
		{
			component.Prepare();
		}
		stiContainer.ContainerInfoV1.CurrentComponent = null;
		stiContainer.ContainerInfoV1.CloneComponents = GetClones(stiContainer);
	}

	public override void UnPrepare(StiComponent masterComp)
	{
		base.UnPrepare(masterComp);
		StiContainer stiContainer = masterComp as StiContainer;
		foreach (StiComponent component in stiContainer.Components)
		{
			component.UnPrepare();
		}
	}

	public override bool InternalRender(StiComponent masterComp, ref StiComponent renderedComponent, StiContainer outContainer)
	{
		StiContainer stiContainer = masterComp as StiContainer;
		if (stiContainer.Name == "#Column#1")
		{
			stiContainer.Page.InvokeColumnBeginRender(stiContainer, EventArgs.Empty);
		}
		if (stiContainer.ContainerInfoV1.CurrentColumn != 0)
		{
			stiContainer.Report.Column = stiContainer.ContainerInfoV1.CurrentColumn;
		}
		bool result = stiContainer.RenderContainer(ref renderedComponent, outContainer);
		((StiContainer)renderedComponent).ContainerInfoV1.CurrentColumn = 1;
		if (stiContainer.Name == "#Column#1")
		{
			stiContainer.Page.InvokeColumnEndRender(stiContainer, EventArgs.Empty);
		}
		if (stiContainer.ContainerInfoV1.CloneComponents != null && stiContainer.ContainerInfoV1.CloneComponents.Count > 0)
		{
			bool flag = false;
			int num = 2;
			foreach (StiClone cloneComponent in stiContainer.ContainerInfoV1.CloneComponents)
			{
				cloneComponent.ContainerInfoV1.CurrentColumn = num;
				StiComponent renderedComponent2 = null;
				if (cloneComponent.Name.StartsWith("#Column#", StringComparison.InvariantCulture))
				{
					stiContainer.Page.InvokeColumnBeginRender(stiContainer, EventArgs.Empty);
				}
				bool flag2 = cloneComponent.Render(ref renderedComponent2, outContainer);
				if (cloneComponent.Name.StartsWith("#Column#", StringComparison.InvariantCulture))
				{
					stiContainer.Page.InvokeColumnEndRender(stiContainer, EventArgs.Empty);
				}
				if (flag2)
				{
					flag = true;
				}
				num++;
			}
			if (flag)
			{
				result = true;
			}
		}
		if (stiContainer.ContainerInfoV1.CurrentColumn != 0)
		{
			stiContainer.Report.Column = 0;
		}
		return result;
	}
}
