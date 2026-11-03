using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

internal class StiColumnsOnDataBand
{
	internal StiEngine Engine { get; }

	public bool Enabled { get; set; }

	internal StiColumnsContainer RenderColumns(StiDataBand dataBand)
	{
		if (dataBand.Columns > 1)
		{
			StiColumnsContainer stiColumnsContainer = CreateColumns(dataBand);
			Engine.RenderContainer(stiColumnsContainer);
			Enabled = true;
			return stiColumnsContainer;
		}
		return null;
	}

	internal StiColumnsContainer GetColumns(StiContainer container = null)
	{
		if (Engine.ContainerForRender == null)
		{
			return null;
		}
		int count = Engine.ContainerForRender.Components.Count;
		if (count <= 0)
		{
			return null;
		}
		for (int num = count - 1; num >= 0; num--)
		{
			StiComponent stiComponent = Engine.ContainerForRender.Components[num];
			if (stiComponent is StiColumnsContainer)
			{
				if (container != null && "Columns" + container.Name != stiComponent.Name)
				{
					return null;
				}
				return stiComponent as StiColumnsContainer;
			}
		}
		return null;
	}

	public StiColumnsContainer CreateColumns(StiDataBand dataBand)
	{
		return new StiColumnsContainer
		{
			Name = "Columns" + dataBand.Name,
			Width = dataBand.Parent.Width,
			Height = 0.0,
			Columns = dataBand.Columns,
			ColumnDirection = dataBand.ColumnDirection,
			ColumnGaps = dataBand.ColumnGaps,
			ColumnWidth = dataBand.GetColumnWidth(),
			MinRowsInColumn = dataBand.MinRowsInColumn,
			RightToLeft = dataBand.RightToLeft
		};
	}

	public StiColumnsOnDataBand(StiEngine engine)
	{
		Engine = engine;
	}
}
