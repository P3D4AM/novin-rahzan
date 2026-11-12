using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Identity.Client.Core;
using Microsoft.Identity.Client.Utils;

namespace Microsoft.Identity.Client.Instance.Discovery;

internal class KnownMetadataProvider : IKnownMetadataProvider
{
	private static readonly IDictionary<string, InstanceDiscoveryMetadataEntry> s_knownEntries;

	private static readonly ISet<string> s_knownEnvironments;

	private static readonly ISet<string> s_knownPublicEnvironments;

	static KnownMetadataProvider()
	{
		s_knownEntries = new Dictionary<string, InstanceDiscoveryMetadataEntry>();
		s_knownEnvironments = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		s_knownPublicEnvironments = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		InstanceDiscoveryMetadataEntry entry2 = new InstanceDiscoveryMetadataEntry
		{
			Aliases = new string[4] { "login.microsoftonline.com", "login.windows.net", "login.microsoft.com", "sts.windows.net" },
			PreferredNetwork = "login.microsoftonline.com",
			PreferredCache = "login.windows.net"
		};
		InstanceDiscoveryMetadataEntry entry3 = new InstanceDiscoveryMetadataEntry
		{
			Aliases = new string[2] { "login.partner.microsoftonline.cn", "login.chinacloudapi.cn" },
			PreferredNetwork = "login.partner.microsoftonline.cn",
			PreferredCache = "login.partner.microsoftonline.cn"
		};
		InstanceDiscoveryMetadataEntry entry4 = new InstanceDiscoveryMetadataEntry
		{
			Aliases = new string[1] { "login.microsoftonline.de" },
			PreferredNetwork = "login.microsoftonline.de",
			PreferredCache = "login.microsoftonline.de"
		};
		InstanceDiscoveryMetadataEntry entry5 = new InstanceDiscoveryMetadataEntry
		{
			Aliases = new string[2] { "login.microsoftonline.us", "login.usgovcloudapi.net" },
			PreferredNetwork = "login.microsoftonline.us",
			PreferredCache = "login.microsoftonline.us"
		};
		InstanceDiscoveryMetadataEntry entry6 = new InstanceDiscoveryMetadataEntry
		{
			Aliases = new string[1] { "login-us.microsoftonline.com" },
			PreferredNetwork = "login-us.microsoftonline.com",
			PreferredCache = "login-us.microsoftonline.com"
		};
		AddToKnownCache(entry2);
		AddToKnownCache(entry3);
		AddToKnownCache(entry4);
		AddToKnownCache(entry5);
		AddToKnownCache(entry6);
		AddToPublicEnvironment(entry2);
		static void AddToKnownCache(InstanceDiscoveryMetadataEntry entry)
		{
			string[] aliases2 = entry.Aliases;
			foreach (string text in aliases2)
			{
				s_knownEntries[text] = entry;
				s_knownEnvironments.Add(text);
			}
		}
		static void AddToPublicEnvironment(InstanceDiscoveryMetadataEntry entry)
		{
			string[] aliases = entry.Aliases;
			foreach (string item in aliases)
			{
				s_knownPublicEnvironments.Add(item);
			}
		}
	}

	public static bool IsPublicEnvironment(string environment)
	{
		return s_knownPublicEnvironments.Contains(environment);
	}

	public InstanceDiscoveryMetadataEntry GetMetadata(string environment, IEnumerable<string> existingEnvironmentsInCache, ICoreLogger logger)
	{
		if (existingEnvironmentsInCache == null)
		{
			existingEnvironmentsInCache = Enumerable.Empty<string>();
		}
		if (existingEnvironmentsInCache.All((string e) => s_knownEnvironments.ContainsOrdinalIgnoreCase(e)))
		{
			s_knownEntries.TryGetValue(environment, out var value);
			logger.Verbose($"[Instance Discovery] Tried to use known metadata provider for {environment}. Success? {value != null}");
			return value;
		}
		logger.VerbosePii("[Instance Discovery] Could not use known metadata provider because at least one environment in the cache is not known. Environments in cache: " + string.Join(" ", existingEnvironmentsInCache) + " ", "[Instance Discovery] Could not use known metadata provider because at least one environment in the cache is not known");
		return null;
	}

	public static bool IsKnownEnvironment(string environment)
	{
		return s_knownEnvironments.Contains(environment);
	}

	public static IDictionary<string, InstanceDiscoveryMetadataEntry> GetAllEntriesForTest()
	{
		return s_knownEntries;
	}
}
