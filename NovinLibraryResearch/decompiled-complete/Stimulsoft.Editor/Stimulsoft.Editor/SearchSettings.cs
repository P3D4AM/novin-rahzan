using System;
using System.Collections;

namespace Stimulsoft.Editor;

public class SearchSettings : PersistentSettings
{
	private ArrayList searchList;

	private ArrayList replaceList;

	private SearchOptions searchOptions;

	private bool clearBookmarks;

	public SearchOptions SearchOptions
	{
		get
		{
			return searchOptions;
		}
		set
		{
			searchOptions = value;
		}
	}

	public bool ClearBookmarks
	{
		get
		{
			return clearBookmarks;
		}
		set
		{
			clearBookmarks = value;
		}
	}

	public IList SearchList => searchList;

	public IList ReplaceList => replaceList;

	public override object XmlInfo
	{
		get
		{
			return new XmlSearchSettingsInfo(this);
		}
		set
		{
			((XmlSearchSettingsInfo)value).FixupReferences(this);
		}
	}

	public SearchSettings()
	{
		searchList = new ArrayList();
		replaceList = new ArrayList();
		searchOptions = EditConsts.DefaultSearchOptions;
	}

	private void CopyList(ArrayList fromList, ArrayList toList)
	{
		toList.Clear();
		foreach (object from in fromList)
		{
			toList.Add(from);
		}
	}

	public override void Assign(IPersistentSettings source)
	{
		if (source is SearchSettings)
		{
			SearchSettings searchSettings = (SearchSettings)source;
			CopyList(searchSettings.searchList, searchList);
			CopyList(searchSettings.replaceList, replaceList);
			searchOptions = searchSettings.searchOptions;
		}
	}

	public override Type GetXmlType()
	{
		return typeof(XmlSearchSettingsInfo);
	}
}
