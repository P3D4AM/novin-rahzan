namespace Stimulsoft.Editor;

public class XmlSearchSettingsInfo
{
	private SearchSettings owner;

	private SearchOptions searchOptions = EditConsts.DefaultSearchOptions;

	private string[] searchList = new string[0];

	private string[] replaceList = new string[0];

	public string[] SearchList
	{
		get
		{
			if (owner != null)
			{
				string[] array = new string[owner.SearchList.Count];
				for (int i = 0; i < owner.SearchList.Count; i++)
				{
					array[i] = (string)owner.SearchList[i];
				}
				return array;
			}
			return searchList;
		}
		set
		{
			searchList = value;
			if (owner != null)
			{
				owner.SearchList.Clear();
				foreach (string value2 in value)
				{
					owner.SearchList.Add(value2);
				}
			}
		}
	}

	public string[] ReplaceList
	{
		get
		{
			if (owner != null)
			{
				string[] array = new string[owner.ReplaceList.Count];
				for (int i = 0; i < owner.ReplaceList.Count; i++)
				{
					array[i] = (string)owner.ReplaceList[i];
				}
				return array;
			}
			return replaceList;
		}
		set
		{
			replaceList = value;
			if (owner != null)
			{
				owner.ReplaceList.Clear();
				foreach (string value2 in value)
				{
					owner.ReplaceList.Add(value2);
				}
			}
		}
	}

	public SearchOptions SearchOptions
	{
		get
		{
			if (owner == null)
			{
				return searchOptions;
			}
			return owner.SearchOptions;
		}
		set
		{
			searchOptions = value;
			if (owner != null)
			{
				owner.SearchOptions = value;
			}
		}
	}

	public XmlSearchSettingsInfo()
	{
	}

	public XmlSearchSettingsInfo(SearchSettings owner)
		: this()
	{
		this.owner = owner;
	}

	public virtual void Load()
	{
		if (owner != null)
		{
			searchList = SearchList;
			replaceList = ReplaceList;
			searchOptions = SearchOptions;
		}
	}

	public virtual void FixupReferences(SearchSettings owner)
	{
		this.owner = owner;
		SearchList = searchList;
		ReplaceList = replaceList;
		SearchOptions = searchOptions;
	}
}
