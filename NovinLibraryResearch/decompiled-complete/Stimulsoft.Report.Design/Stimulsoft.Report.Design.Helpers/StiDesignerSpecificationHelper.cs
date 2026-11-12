using System;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base.Server;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Table;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Maps;

namespace Stimulsoft.Report.Design.Helpers;

internal class StiDesignerSpecificationHelper
{
	private static List<Type> beginnerHideComponents;

	private static List<Type> beginnerHideDatabase;

	private static List<Type> creatorHideDatabase;

	public static List<Type> BeginnerHideComponents
	{
		get
		{
			if (beginnerHideComponents == null)
			{
				List<Type> list = new List<Type>();
				list.Add(typeof(StiGauge));
				list.Add(typeof(StiMap));
				list.Add(typeof(StiTextInCells));
				list.Add(typeof(StiPanelControl));
				list.Add(typeof(StiClone));
				list.Add(typeof(StiSubReport));
				list.Add(typeof(StiTable));
				list.Add(typeof(StiColumnHeaderBand));
				list.Add(typeof(StiColumnFooterBand));
				list.Add(typeof(StiHierarchicalBand));
				list.Add(typeof(StiChildBand));
				list.Add(typeof(StiEmptyBand));
				list.Add(typeof(StiOverlayBand));
				beginnerHideComponents = list;
			}
			return beginnerHideComponents;
		}
	}

	public static List<Type> BeginnerHideDatabase
	{
		get
		{
			if (beginnerHideDatabase == null)
			{
				List<Type> list = new List<Type>();
				list.Add(typeof(StiBusinessObjectAdapterService));
				list.Add(typeof(StiCrossTabAdapterService));
				list.Add(typeof(StiDataTableAdapterService));
				list.Add(typeof(StiDataViewAdapterService));
				list.Add(typeof(StiVirtualAdapterService));
				list.Add(typeof(StiUserAdapterService));
				beginnerHideDatabase = list;
			}
			return beginnerHideDatabase;
		}
	}

	public static List<Type> CreatorHideDatabase
	{
		get
		{
			if (creatorHideDatabase == null)
			{
				List<Type> list = new List<Type>();
				list.Add(typeof(StiBusinessObjectAdapterService));
				list.Add(typeof(StiDataTableAdapterService));
				list.Add(typeof(StiDataViewAdapterService));
				list.Add(typeof(StiUserAdapterService));
				creatorHideDatabase = list;
			}
			return creatorHideDatabase;
		}
	}

	public static IEnumerable<StiComponent> CheckShowComponents(IEnumerable<StiComponent> mainList)
	{
		if (StiAccountSettings.CurrentAccount == null || !StiOptions.Designer.IsBeginnerDesigner)
		{
			return mainList;
		}
		return mainList.Where((StiComponent x) => !BeginnerHideComponents.Contains(x.GetType()));
	}

	public static IEnumerable<StiDataAdapterService> CheckShowDatabases(IEnumerable<StiDataAdapterService> mainList)
	{
		if (StiAccountSettings.CurrentAccount == null)
		{
			return mainList;
		}
		if (StiOptions.Designer.IsBeginnerDesigner)
		{
			return mainList.Where((StiDataAdapterService x) => !BeginnerHideDatabase.Contains(x.GetType()));
		}
		if (StiOptions.Designer.IsBICreatorDesigner)
		{
			return mainList.Where((StiDataAdapterService x) => !CreatorHideDatabase.Contains(x.GetType()));
		}
		return mainList;
	}
}
