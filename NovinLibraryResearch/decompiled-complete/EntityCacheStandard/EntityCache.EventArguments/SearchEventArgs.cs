using System;

namespace EntityCache.EventArguments;

public class SearchEventArgs : EventArgs
{
	public string SearchString { get; private set; }

	public SearchEventArgs(string searchString)
	{
		SearchString = searchString;
	}
}
