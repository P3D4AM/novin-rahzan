using System.Collections;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

internal class StiThreads
{
	internal bool IsActive { get; set; }

	internal int CurrentPage { get; set; } = -1;


	internal int CurrentColumn { get; set; } = -1;


	internal string DestinationName { get; set; }

	internal StiEngine Engine { get; }

	internal void NewPage()
	{
		Engine.ContainerForRender = null;
		CurrentColumn++;
		Engine.ContainerForRender = GetDestinationContainer();
		if (Engine.ContainerForRender != null)
		{
			return;
		}
		CurrentColumn = 1;
		CurrentPage++;
		if (CurrentPage >= Engine.Report.RenderedPages.Count)
		{
			try
			{
				Engine.Report.Engine = Engine.MasterEngine;
				Engine.Report.Engine.NewPage();
			}
			finally
			{
				Engine.Report.Engine = Engine;
			}
		}
		Engine.ContainerForRender = GetDestinationContainer();
	}

	internal void SelectThreadFromContainer(StiContainer container)
	{
		if (Engine.DenyChangeThread)
		{
			return;
		}
		string text = null;
		StiContainer stiContainer = null;
		stiContainer = ((container.ContainerInfoV2.ParentBand != null) ? container.ContainerInfoV2.ParentBand.Parent : container.Parent);
		text = stiContainer.Name;
		if (stiContainer is StiPage)
		{
			if (Engine.MasterEngine != null)
			{
				Engine.Report.Engine = Engine.MasterEngine;
			}
			return;
		}
		StiEngine stiEngine = ((Engine.MasterEngine == null) ? Engine : Engine.MasterEngine);
		StiEngine stiEngine2 = stiEngine.SlaveEngines[text] as StiEngine;
		if (stiEngine2 == null)
		{
			stiEngine2 = stiEngine.Threads.CreateContainerEngine(text, Engine.Report, stiEngine, (stiEngine.Page != null) ? stiEngine.Page.PageInfoV2.IndexOfStartRenderedPages : 0);
			stiEngine.SlaveEngines[text] = stiEngine2;
			stiEngine2.PrintOnAllPagesIgnoreList = stiEngine.PrintOnAllPagesIgnoreList;
		}
		Engine.Report.Engine = stiEngine2;
	}

	internal StiEngine CreateContainerEngine(string destinationName, StiReport report, StiEngine masterEngine, int indexOfStartRenderedPages)
	{
		StiEngine stiEngine = new StiEngine(report);
		stiEngine.MasterEngine = masterEngine;
		stiEngine.Threads.IsActive = true;
		stiEngine.Threads.CurrentPage = indexOfStartRenderedPages;
		stiEngine.Threads.CurrentColumn = 1;
		stiEngine.Threads.DestinationName = destinationName;
		stiEngine.TemplatePage = report.Engine.TemplatePage;
		stiEngine.TemplateContainer = stiEngine.Threads.GetTemplateContainer();
		stiEngine.ParserConversionStore = (Hashtable)report.Engine.ParserConversionStore.Clone();
		if (indexOfStartRenderedPages != -1)
		{
			stiEngine.ContainerForRender = stiEngine.Threads.GetDestinationContainer();
		}
		else
		{
			stiEngine.ContainerForRender = stiEngine.Threads.GetDestinationContainer(masterEngine.ContainerForRender, stiEngine.Threads.DestinationName, stiEngine.Threads.CurrentColumn);
		}
		stiEngine.NewList();
		if (indexOfStartRenderedPages == -1)
		{
			stiEngine.FreeSpace = 100000000000.0;
		}
		return stiEngine;
	}

	public StiContainer GetTemplateContainer()
	{
		return GetTemplateContainer(Engine.TemplatePage, DestinationName);
	}

	private StiContainer GetTemplateContainer(StiContainer template, string name)
	{
		lock (((ICollection)template.Components).SyncRoot)
		{
			foreach (StiComponent component in template.Components)
			{
				if (component is StiContainer stiContainer)
				{
					if (stiContainer.Name == name)
					{
						return stiContainer;
					}
					StiContainer templateContainer = GetTemplateContainer(stiContainer, name);
					if (templateContainer != null)
					{
						return templateContainer;
					}
				}
			}
		}
		return null;
	}

	public StiContainer GetDestinationContainer()
	{
		StiPage stiPage = null;
		stiPage = ((Engine.Report.RenderedPages.Count <= CurrentPage) ? Engine.Report.RenderedPages[Engine.Report.RenderedPages.Count - 1] : Engine.Report.RenderedPages[CurrentPage]);
		return GetDestinationContainer(stiPage, DestinationName, CurrentColumn);
	}

	private StiContainer GetDestinationContainer(StiContainer container, string name, int columnNumber)
	{
		lock (((ICollection)container.Components).SyncRoot)
		{
			foreach (StiComponent component in container.Components)
			{
				if (component is StiContainer stiContainer)
				{
					if (stiContainer.Name == name && stiContainer.ContainerInfoV2.RenderStep == columnNumber)
					{
						return stiContainer;
					}
					StiContainer destinationContainer = GetDestinationContainer(stiContainer, name, columnNumber);
					if (destinationContainer != null)
					{
						return destinationContainer;
					}
				}
			}
		}
		return null;
	}

	public StiThreads(StiEngine engine)
	{
		Engine = engine;
	}
}
