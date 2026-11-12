using System;
using System.Collections.Concurrent;

namespace Microsoft.Data.SqlClient;

internal sealed class SQLFallbackDNSCache
{
	private static readonly SQLFallbackDNSCache _SQLFallbackDNSCache = new SQLFallbackDNSCache();

	private static readonly int initialCapacity = 101;

	private ConcurrentDictionary<string, SQLDNSInfo> DNSInfoCache;

	public static SQLFallbackDNSCache Instance => _SQLFallbackDNSCache;

	private SQLFallbackDNSCache()
	{
		int concurrencyLevel = 4 * Environment.ProcessorCount;
		DNSInfoCache = new ConcurrentDictionary<string, SQLDNSInfo>(concurrencyLevel, initialCapacity, StringComparer.OrdinalIgnoreCase);
	}

	internal bool AddDNSInfo(SQLDNSInfo item)
	{
		if (item != null)
		{
			if (DNSInfoCache.ContainsKey(item.FQDN))
			{
				DeleteDNSInfo(item.FQDN);
			}
			return DNSInfoCache.TryAdd(item.FQDN, item);
		}
		return false;
	}

	internal bool DeleteDNSInfo(string FQDN)
	{
		SQLDNSInfo value;
		return DNSInfoCache.TryRemove(FQDN, out value);
	}

	internal bool GetDNSInfo(string FQDN, out SQLDNSInfo result)
	{
		return DNSInfoCache.TryGetValue(FQDN, out result);
	}

	internal bool IsDuplicate(SQLDNSInfo newItem)
	{
		if (newItem != null && GetDNSInfo(newItem.FQDN, out var result))
		{
			if (newItem.AddrIPv4 == result.AddrIPv4 && newItem.AddrIPv6 == result.AddrIPv6)
			{
				return newItem.Port == result.Port;
			}
			return false;
		}
		return false;
	}
}
