using System.Collections;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ShortcutTableEntry
{
	public eShortcut Shortcut;

	public Hashtable Items;

	public ShortcutTableEntry(eShortcut key)
	{
		Shortcut = key;
		Items = new Hashtable();
	}

	public override int GetHashCode()
	{
		return (int)Shortcut;
	}
}
