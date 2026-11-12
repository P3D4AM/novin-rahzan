using System;
using System.Runtime.InteropServices;

namespace Microsoft.SqlServer.Management.Assessment.Tools;

[StructLayout(LayoutKind.Sequential, Size = 1)]
internal struct ToolKey
{
	private static readonly ListBiDictionary<Type, string> Rep = new ListBiDictionary<Type, string>
	{
		{
			typeof(ISqlTool),
			"SQL"
		},
		{
			typeof(ICmdShellTool),
			"CmdShell"
		},
		{
			typeof(IPowerShellTool),
			"PowerShell"
		},
		{
			typeof(IWmiTool),
			"WMI"
		},
		{
			typeof(IRegistryTool),
			"Registry"
		},
		{
			typeof(IAzureTool),
			"Azure"
		}
	};

	public const string Sql = "SQL";

	public const string CmdShell = "CmdShell";

	public const string PowerShell = "PowerShell";

	public const string Wmi = "WMI";

	public const string Registry = "Registry";

	public const string Azure = "Azure";

	public static bool TryGetValue(string key, out Type channelType)
	{
		return Rep.TryGetValue(key, out channelType);
	}

	public static bool TryGetValue(Type channelType, out string key)
	{
		return Rep.TryGetValue(channelType, out key);
	}

	public static string From(Type toolType)
	{
		if (Rep.TryGetValue(toolType, out var y))
		{
			return y;
		}
		throw new ArgumentOutOfRangeException("toolType");
	}

	public static Type From(string key)
	{
		if (Rep.TryGetValue(key, out var x))
		{
			return x;
		}
		throw new ArgumentOutOfRangeException("key");
	}
}
