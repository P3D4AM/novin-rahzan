using System.Collections.Generic;

namespace FormCustom.Menu;

public interface ISearchInMenu
{
	List<SearchInMenuStrip> ListItems { get; set; }
}
