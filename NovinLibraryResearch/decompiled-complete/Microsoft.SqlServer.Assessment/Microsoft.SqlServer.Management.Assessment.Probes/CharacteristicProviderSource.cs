using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Microsoft.SqlServer.Management.Assessment.Probes;

internal class CharacteristicProviderSource
{
	[DebuggerDisplay("{Provider.GetDisplayName(Key, Arguments, Specification)}")]
	internal struct Call
	{
		public readonly string Key;

		public readonly ICharacteristicProvider Provider;

		public readonly Dictionary<string, string> Arguments;

		public HashSet<string> Specification;

		public Call(string key, ICharacteristicProvider provider, Dictionary<string, string> arguments, HashSet<string> specification)
		{
			Key = key;
			Provider = provider;
			Arguments = arguments;
			Specification = specification;
		}

		public Task Update(RunSpace runSpace)
		{
			if (Provider == null)
			{
				runSpace.FailedCharacteristics.Add(new CharacteristicFailure
				{
					Subject = new CharacteristicCollection(Key, Specification),
					Reason = new CharacteristicCollection(Key, Specification)
				});
				return Task.CompletedTask;
			}
			return Provider.UpdateCharacteristics(runSpace, Key, Arguments, Specification);
		}

		public CharacteristicCollection GetImplied(RunSpace runSpace)
		{
			return Provider?.GetDependencies(runSpace, Arguments, Specification) ?? CharacteristicCollection.Empty;
		}
	}

	internal class CallPriorityComparer : IComparer<Call>
	{
		public static readonly CallPriorityComparer Instance = new CallPriorityComparer();

		public int Compare(Call x, Call y)
		{
			return -(x.Provider?.Priority ?? 0).CompareTo(y.Provider?.Priority ?? 0);
		}
	}

	private Dictionary<Regex, ICharacteristicProvider> Registry { get; } = new Dictionary<Regex, ICharacteristicProvider>();


	public void Add(ICharacteristicProvider provider)
	{
		Registry.Add(provider.Pattern, provider);
	}

	public bool TryGetProvider(string key, out ICharacteristicProvider provider, out Dictionary<string, string> arguments)
	{
		foreach (KeyValuePair<Regex, ICharacteristicProvider> item in Registry)
		{
			Match match = item.Key.Match(key);
			if (!match.Success)
			{
				continue;
			}
			provider = item.Value;
			arguments = new Dictionary<string, string>();
			string[] groupNames = item.Key.GetGroupNames();
			foreach (string text in groupNames)
			{
				Group group = match.Groups[text];
				if (group != null && group.Success)
				{
					arguments.Add(text, group.Value);
				}
			}
			return true;
		}
		provider = null;
		arguments = null;
		return false;
	}

	public async Task Update(RunSpace runSpace, CharacteristicCollection characteristicQuery)
	{
		List<Call> list = characteristicQuery.Select(CreateCall).ToList();
		for (int i = 0; i < list.Count; i++)
		{
			AddCall(list, list[i], runSpace);
		}
		list.Sort(CallPriorityComparer.Instance);
		await GetData(runSpace, list).ConfigureAwait(continueOnCapturedContext: false);
	}

	private void AddCall(IList<Call> list, Call item, RunSpace runSpace)
	{
		AddImplied(list, item, runSpace);
		foreach (Call item2 in list)
		{
			if (IsMatch(item2, item))
			{
				item2.Specification.UnionWith(item.Specification);
				return;
			}
		}
		list.Add(item);
	}

	private void AddImplied(IList<Call> list, Call item, RunSpace runSpace)
	{
		CharacteristicCollection implied = item.GetImplied(runSpace);
		foreach (KeyValuePair<string, HashSet<string>> item2 in implied)
		{
			AddCall(list, CreateCall(item2), runSpace);
		}
	}

	private static bool IsMatch(Call existing, Call fresh)
	{
		return string.CompareOrdinal(existing.Key, fresh.Key) == 0;
	}

	private static async Task GetData(RunSpace runSpace, List<Call> calls)
	{
		foreach (Call call in calls)
		{
			await call.Update(runSpace).ConfigureAwait(continueOnCapturedContext: false);
		}
	}

	private Call CreateCall(KeyValuePair<string, HashSet<string>> pair)
	{
		return CreateCall(pair.Key, pair.Value);
	}

	private Call CreateCall(string key, HashSet<string> specification)
	{
		if (!TryGetProvider(key, out var provider, out var arguments))
		{
			return default(Call);
		}
		return new Call(key, provider, arguments, specification);
	}
}
