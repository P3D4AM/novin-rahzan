using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Assessment.Probes;

namespace Microsoft.SqlServer.Management.Assessment.Tools;

internal class XpRegXTool : DependentTool<ISqlTool>, IRegistryTool, ITool
{
	private const string SingleValueQuery = "EXEC xp_regread @hive, @key, @valueName";

	private const string SingleValueInstanceQuery = "EXEC xp_instance_regread @hive, @key, @valueName";

	private const string EnumKeysQuery = "EXEC xp_regenumkeys @hive, @parent";

	private const string EnumKeysInstanceQuery = "EXEC xp_instance_regenumkeys @hive, @parent";

	private static readonly CharacteristicCollection StaticRequirements = new CharacteristicCollection
	{
		{
			"object permission on xp_regread",
			new HashSet<string> { "EXECUTE" }
		},
		{
			"object permission on xp_regenumkeys",
			new HashSet<string> { "EXECUTE" }
		},
		{
			"object permission on xp_instance_regread",
			new HashSet<string> { "EXECUTE" }
		},
		{
			"object permission on xp_instance_regenumkeys",
			new HashSet<string> { "EXECUTE" }
		}
	};

	public XpRegXTool(RunSpace runSpace)
		: base(runSpace)
	{
	}

	public static IRegistryTool Create(RunSpace runSpace)
	{
		return new XpRegXTool(runSpace);
	}

	public async Task<List<Dictionary<string, object>>> Execute(string hive, string keyPattern, IReadOnlyList<string> valueNames, bool instance)
	{
		List<Dictionary<string, object>> view = new List<Dictionary<string, object>>();
		if (base.BaseTool != null)
		{
			using DbCommand command = base.BaseTool.CreateCommand(instance ? "EXEC xp_instance_regread @hive, @key, @valueName" : "EXEC xp_regread @hive, @key, @valueName");
			command.AddParameter("@hive", hive);
			DbParameter keyParameter = command.AddParameter("@key", string.Empty);
			DbParameter valueNameParameter = command.AddParameter("@valueName", string.Empty);
			Dictionary<string, string> keyList = new Dictionary<string, string>();
			await CollectKeys(hive, keyPattern, keyList, instance).ConfigureAwait(continueOnCapturedContext: false);
			foreach (KeyValuePair<string, string> item in keyList)
			{
				await QueryKey(item.Key, item.Value, valueNames, view, command, keyParameter, valueNameParameter).ConfigureAwait(continueOnCapturedContext: false);
			}
		}
		return view;
	}

	public async Task<Dictionary<string, string>> EnumKeys(string hive, string keyPattern, bool instance)
	{
		Dictionary<string, string> keys = new Dictionary<string, string>();
		await CollectKeys(hive, keyPattern.TrimEnd(new char[1] { '\\' }) + "\\*", keys, instance).ConfigureAwait(continueOnCapturedContext: false);
		return keys;
	}

	public override ICharacteristicCollection GetDependencies(RunSpace runSpace)
	{
		return CharacteristicCollection.Union(base.GetDependencies(runSpace), StaticRequirements);
	}

	protected override async Task<bool> IsChannelAvailable(RunSpace runSpace)
	{
		if (base.BaseTool == null || !runSpace.Meets(StaticRequirements))
		{
			return false;
		}
		using DbCommand command = base.BaseTool.CreateCommand("EXEC xp_regread @hive, @key, @valueName");
		command.AddParameter("@hive", "HKEY_LOCAL_MACHINE");
		command.AddParameter("@key", "SYSTEM\\CurrentControlSet\\Control");
		command.AddParameter("@valueName", "CurrentUser");
		try
		{
			using DbDataReader dbDataReader = await command.ExecuteReaderAsync().ConfigureAwait(continueOnCapturedContext: false);
			return dbDataReader.HasRows;
		}
		catch (Exception ex)
		{
			if (ex is StackOverflowException || ex is OutOfMemoryException)
			{
				throw;
			}
			return false;
		}
	}

	private async Task CollectKeys(string hive, string keyPattern, Dictionary<string, string> keyList, bool instance, string keyName = "")
	{
		int num = keyPattern.IndexOf('*');
		if (num < 0)
		{
			keyList.Add(keyName, keyPattern);
			return;
		}
		string parent = keyPattern.Substring(0, num);
		Dictionary<string, string> subKeys = new Dictionary<string, string>();
		int i;
		for (i = num + 1; keyPattern.Length > i && keyPattern[i] == '\\'; i++)
		{
		}
		string patternTail = keyPattern.Substring(i);
		using (DbCommand command = base.BaseTool.CreateCommand(instance ? "EXEC xp_instance_regenumkeys @hive, @parent" : "EXEC xp_regenumkeys @hive, @parent"))
		{
			command.AddParameter("@hive", hive);
			command.AddParameter("@parent", parent);
			using DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(continueOnCapturedContext: false);
			if (reader.HasRows)
			{
				while (await reader.ReadAsync().ConfigureAwait(continueOnCapturedContext: false))
				{
					if (reader[0] is string path)
					{
						string value = Path.Combine(parent, path, patternTail);
						string key = Path.Combine(keyName, path);
						subKeys.Add(key, value);
					}
				}
			}
		}
		foreach (KeyValuePair<string, string> item in subKeys)
		{
			await CollectKeys(hive, item.Value, keyList, instance, item.Key).ConfigureAwait(continueOnCapturedContext: false);
		}
	}

	private static async Task QueryKey(string keyName, string keyPath, IEnumerable<string> valueNames, List<Dictionary<string, object>> view, DbCommand command, DbParameter keyParameter, DbParameter valueNameParameter)
	{
		keyParameter.Value = keyPath;
		List<List<Dictionary<string, object>>> heap = new List<List<Dictionary<string, object>>>();
		foreach (string valueName in valueNames)
		{
			await QueryName(valueName, command, valueNameParameter, heap).ConfigureAwait(continueOnCapturedContext: false);
		}
		Func<Dictionary<string, object>, Dictionary<string, object>, Dictionary<string, object>> aggregator = CollectionUtil.UnionWith;
		IEnumerable<Dictionary<string, object>> enumerable = from r in AlgebraicUtil.GetCombinations(heap, aggregator)
			where r.Count > 0
			select r;
		if (!string.IsNullOrEmpty(keyName))
		{
			enumerable = enumerable.Select(delegate(Dictionary<string, object> r)
			{
				r.Add("RegistryKeyName", keyName);
				return r;
			});
		}
		view.AddRange(enumerable);
	}

	private static async Task QueryName(string name, DbCommand command, DbParameter valueNameParameter, List<List<Dictionary<string, object>>> heap)
	{
		List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();
		valueNameParameter.Value = name;
		using (DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(continueOnCapturedContext: false))
		{
			if (reader.HasRows)
			{
				while (await reader.ReadAsync().ConfigureAwait(continueOnCapturedContext: false))
				{
					switch (reader.FieldCount)
					{
					case 2:
						items.Add(new Dictionary<string, object> { 
						{
							name,
							reader[1].Normalize()
						} });
						break;
					case 3:
					{
						object value = ((reader[2] == DBNull.Value) ? reader[1].Normalize() : reader[2].Normalize());
						items.Add(new Dictionary<string, object> { { name, value } });
						break;
					}
					}
				}
			}
		}
		if (items.Count > 0)
		{
			heap.Add(items);
		}
	}
}
