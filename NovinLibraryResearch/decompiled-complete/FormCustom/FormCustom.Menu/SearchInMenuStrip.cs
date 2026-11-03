using System;

namespace FormCustom.Menu;

public class SearchInMenuStrip
{
	public bool Enable { get; set; }

	public string ShortText { get; set; }

	public string Text { get; set; }

	public Action Func { get; set; }
}
