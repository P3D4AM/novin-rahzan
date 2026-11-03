using System;
using System.Collections.Generic;

namespace PacketParser.PosToPc.Sadad.Classes;

public static class Config
{
	private static readonly string ConfigFileName;

	private static byte[] ConfigFileHash;

	private static Dictionary<string, Dictionary<string, object>> _transactionDictionary;

	public static T GetModemConfig<T>(string pId) where T : new()
	{
		try
		{
			if (!(typeof(T) == typeof(Serial)))
			{
				_ = typeof(T) != typeof(Lan);
				return new T();
			}
			return new T();
		}
		catch (Exception exception)
		{
			Utility.LogPcPos(exception);
			return new T();
		}
	}
}
