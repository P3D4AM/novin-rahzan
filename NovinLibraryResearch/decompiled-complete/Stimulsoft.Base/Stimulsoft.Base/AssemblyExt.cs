using System.Reflection;

namespace Stimulsoft.Base;

public static class AssemblyExt
{
	public static bool TryLoad(string assembly)
	{
		try
		{
			Assembly.Load(assembly);
			return true;
		}
		catch
		{
			return false;
		}
	}
}
