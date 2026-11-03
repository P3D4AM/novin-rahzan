using System.Collections;

namespace DevComponents.DotNetBar;

internal class ᯣ
{
	public eShortcut ᯤ;

	public Hashtable ᯥ;

	public ᯣ(eShortcut key)
	{
		ᯤ = key;
		ᯥ = new Hashtable();
	}

	public override int GetHashCode()
	{
		return (int)ᯤ;
	}
}
