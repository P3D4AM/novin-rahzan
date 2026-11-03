using System;
using Stimulsoft.Base;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

internal class StiBreakableHelper
{
	internal StiEngine Engine { get; }

	internal bool IsCanBreak(StiContainer container)
	{
		IStiBreakable parentBand = container.ContainerInfoV2.ParentBand;
		if (parentBand != null && parentBand.CanBreak)
		{
			return true;
		}
		parentBand = container;
		return parentBand.CanBreak;
	}

	internal bool IsNeedBreak(StiContainer container)
	{
		if (Engine.ContainerForRender is StiPage { UnlimitedHeight: not false, UnlimitedBreakable: false } && (container.ContainerInfoV2?.ParentBand == null || !container.ContainerInfoV2.ParentBand.Components.ToList().Exists((StiComponent c) => c is IStiCrossTab)))
		{
			return false;
		}
		if (container.Height > Engine.FreeSpace)
		{
			return IsCanBreak(container);
		}
		return false;
	}

	internal StiContainer Break(StiContainer originalContainer)
	{
		StiContainerInfoV2 containerInfoV = originalContainer.ContainerInfoV2;
		double num = originalContainer.Height;
		foreach (StiComponent component in originalContainer.Components)
		{
			num = Math.Min(num, component.Top);
		}
		int count = originalContainer.Components.Count;
		StiContainer stiContainer = StiComponentDivider.BreakContainer(Engine.FreeSpace, originalContainer);
		stiContainer.ContainerInfoV2.IgnoreResetPageNumber = true;
		bool flag = num == 0.0 && count > 0 && originalContainer.Components.Count == 0;
		stiContainer.Name = ((flag && containerInfoV != null && (containerInfoV.DataSourceRow != null || containerInfoV.BusinessObjectCurrent != null)) ? ("Continued_" + originalContainer.Name) : "Continued");
		stiContainer.Height -= originalContainer.Height;
		originalContainer.Name = ((!flag && containerInfoV != null && (containerInfoV.DataSourceRow != null || containerInfoV.BusinessObjectCurrent != null)) ? ("Breaked_" + originalContainer.Name) : "Breaked");
		if (containerInfoV.ParentBand is StiTableOfContents)
		{
			originalContainer.Name = containerInfoV.ParentBand.Name;
		}
		originalContainer.Height = Engine.FreeSpace;
		if (!flag)
		{
			Engine.FreeSpace += originalContainer.Height;
			Engine.RenderContainer(originalContainer);
			Engine.FreeSpace -= originalContainer.Height;
		}
		Engine.NewDestination(!flag);
		stiContainer.CanGrow = true;
		StiContainerHelper.CheckSize(stiContainer);
		return stiContainer;
	}

	internal void SetCanBreak(StiContainer container)
	{
		container.CanBreak = true;
		for (int i = 0; i < container.Components.Count; i++)
		{
			StiComponent stiComponent = container.Components[i];
			if (stiComponent is IStiBreakable stiBreakable)
			{
				if (!stiBreakable.CanBreak && Engine?.Report != null)
				{
					StiComponent stiComponent2 = Engine.Report[stiComponent.Name] as StiComponent;
					if (stiComponent2 == null && Engine.Report.CalculationMode == StiCalculationMode.Interpretation)
					{
						stiComponent2 = Engine.Report.GetComponentByName(stiComponent.Name);
					}
					if (stiComponent2 != null && ((StiService)stiComponent2).Properties == ((StiService)stiComponent).Properties && ((StiService)stiComponent).Properties != null)
					{
						_003F val = stiComponent;
						object obj = ((StiService)stiComponent).Properties.Clone();
						((StiService)val).Properties = (StiRepositoryItems)((obj is StiRepositoryItems) ? obj : null);
					}
				}
				stiBreakable.CanBreak = true;
			}
			if (stiComponent is StiContainer canBreak)
			{
				SetCanBreak(canBreak);
			}
		}
	}

	internal StiContainer ProcessBreakable(StiContainer container)
	{
		if (Engine.IsCrossBandsMode)
		{
			return container;
		}
		bool flag = false;
		while (IsNeedBreak(container))
		{
			double height = container.Height;
			container = Break(container);
			if (height != container.Height)
			{
				continue;
			}
			StiContainer stiContainer = StiComponentDivider.BreakContainer(Engine.FreeSpace, ((StiService)container).Clone() as StiContainer);
			StiContainerHelper.CheckSize(stiContainer);
			if (container.Height == stiContainer.Height)
			{
				if (flag)
				{
					break;
				}
				SetCanBreak(container);
				flag = true;
			}
		}
		return container;
	}

	internal StiBreakableHelper(StiEngine engine)
	{
		Engine = engine;
	}
}
