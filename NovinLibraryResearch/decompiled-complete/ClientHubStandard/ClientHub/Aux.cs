using System.Collections.Generic;
using PacketParser.Connection;

namespace ClientHub;

public static class Aux
{
	public static List<ClientConfig> CopyAll(this IEnumerable<ClientConfig> configs, bool different = true)
	{
		List<ClientConfig> list = new List<ClientConfig>();
		foreach (ClientConfig config in configs)
		{
			list.Add(config.Copy());
		}
		return list;
	}
}
