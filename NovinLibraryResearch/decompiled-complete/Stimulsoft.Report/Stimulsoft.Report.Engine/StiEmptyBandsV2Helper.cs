using System.Collections;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

internal class StiEmptyBandsV2Helper
{
	private StiEmptyBand emptyBand;

	internal StiEngine Engine { get; }

	public void Register(StiComponentsCollection emptyBands)
	{
		if (emptyBands != null && emptyBands.Count > 0)
		{
			emptyBand = emptyBands[0] as StiEmptyBand;
		}
	}

	public void Clear()
	{
		emptyBand = null;
	}

	private StiContainer CreateEmptyBandContainer(StiContainer containerForRender)
	{
		Engine.Report.Line++;
		Engine.Report.LineThrough++;
		emptyBand.InvokeRendering();
		emptyBand.ParentBookmark = containerForRender.CurrentBookmark;
		emptyBand.ParentPointer = containerForRender.CurrentPointer;
		StiComponent stiComponent = emptyBand.Render();
		if (stiComponent == null)
		{
			return null;
		}
		StiOddEvenStylesHelper.ApplyOddEvenStyles(Engine.Report, emptyBand, stiComponent as StiContainer);
		stiComponent.DockStyle = StiDockStyle.None;
		stiComponent.Left = Engine.PositionX;
		stiComponent.Top = Engine.PositionY;
		return stiComponent as StiContainer;
	}

	public void Render(StiContainer containerForRender, StiContainer selectedContainer)
	{
		if (emptyBand == null)
		{
			return;
		}
		Engine.IsDynamicBookmarksMode = true;
		try
		{
			emptyBand.InvokeBeginRender();
			int num = -1;
			if (containerForRender != null)
			{
				num = containerForRender.Components.IndexOf(selectedContainer);
			}
			if (num == -1)
			{
				num = containerForRender.Components.Count;
			}
			StiContainer stiContainer = null;
			StiContainer stiContainer2;
			do
			{
				stiContainer2 = CreateEmptyBandContainer(containerForRender);
				if (stiContainer2 == null)
				{
					break;
				}
				if (stiContainer2.Height == 0.0)
				{
					stiContainer2.Height = Engine.Report.Unit.ConvertFromHInches(10.0);
				}
				if (!(stiContainer2.Height <= Engine.FreeSpace))
				{
					break;
				}
				Engine.PositionY += stiContainer2.Height;
				Engine.FreeSpace -= stiContainer2.Height;
				containerForRender.Components.Insert(num, stiContainer2);
				num++;
				stiContainer = stiContainer2;
				bool flag = stiContainer2.DoBookmark();
				stiContainer2.DoPointer(!flag);
			}
			while (!(Engine.FreeSpace > 999999999.0));
			if (emptyBand.SizeMode == StiEmptySizeMode.DecreaseLastRow)
			{
				stiContainer2.Height = Engine.FreeSpace;
				Engine.PositionY += stiContainer2.Height;
				Engine.FreeSpace -= stiContainer2.Height;
				containerForRender.Components.Insert(num, stiContainer2);
				bool flag2 = stiContainer2.DoBookmark();
				stiContainer2.DoPointer(!flag2);
			}
			else if (emptyBand.SizeMode == StiEmptySizeMode.IncreaseLastRow)
			{
				if (stiContainer == null)
				{
					if (Engine.FreeSpace > 0.0)
					{
						stiContainer2 = CreateEmptyBandContainer(containerForRender);
						stiContainer2.Top = Engine.PositionY;
						stiContainer2.Height = Engine.FreeSpace;
					}
				}
				else
				{
					stiContainer.Height += Engine.FreeSpace;
				}
			}
			else if (emptyBand.SizeMode == StiEmptySizeMode.AlignFooterToTop)
			{
				lock (((ICollection)containerForRender.Components).SyncRoot)
				{
					foreach (StiComponent component in containerForRender.Components)
					{
						if (component.Top > Engine.PositionY && !(component is StiCrossLinePrimitive))
						{
							component.Top -= Engine.FreeSpace;
						}
					}
				}
			}
			emptyBand.InvokeEndRender();
		}
		finally
		{
			Engine.IsDynamicBookmarksMode = false;
		}
	}

	internal StiEmptyBandsV2Helper(StiEngine engine)
	{
		Engine = engine;
	}
}
