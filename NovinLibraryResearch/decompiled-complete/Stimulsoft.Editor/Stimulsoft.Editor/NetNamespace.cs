namespace Stimulsoft.Editor;

public class NetNamespace : INetNamespace
{
	private string nspace = string.Empty;

	private string alias = string.Empty;

	private bool system;

	public string Namespace
	{
		get
		{
			return nspace;
		}
		set
		{
			nspace = value;
		}
	}

	public string Alias
	{
		get
		{
			return alias;
		}
		set
		{
			alias = value;
		}
	}

	public bool System
	{
		get
		{
			return system;
		}
		set
		{
			system = value;
		}
	}

	public NetNamespace(string nspace, bool system)
	{
		this.nspace = nspace;
		this.system = system;
	}

	public string GetName()
	{
		if (!(alias != string.Empty))
		{
			return nspace;
		}
		return alias;
	}
}
