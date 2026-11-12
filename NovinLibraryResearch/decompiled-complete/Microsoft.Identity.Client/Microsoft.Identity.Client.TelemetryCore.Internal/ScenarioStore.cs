using System;
using System.Collections.Generic;

namespace Microsoft.Identity.Client.TelemetryCore.Internal;

internal class ScenarioStore : IScenarioStore
{
	private readonly Dictionary<string, ScenarioHolder> _scenarioIdToPropertyBag;

	private readonly object _lockScenarioIdToPropertyBag = new object();

	private readonly IErrorStore _errorStore;

	private readonly int _maxInactiveScenarioDurationMillis;

	public ScenarioStore(int maxInactiveScenarioDurationMillis, IErrorStore errorStore)
	{
		_scenarioIdToPropertyBag = new Dictionary<string, ScenarioHolder>();
		_maxInactiveScenarioDurationMillis = maxInactiveScenarioDurationMillis;
		_errorStore = errorStore;
	}

	public MatsScenario CreateScenario()
	{
		string text = MatsId.Create();
		PropertyBag propertyBag = new PropertyBag(EventType.Scenario, _errorStore);
		propertyBag.Add("uploadid", MatsId.Create());
		propertyBag.Add("scenarioid", text);
		lock (_lockScenarioIdToPropertyBag)
		{
			_scenarioIdToPropertyBag[text] = new ScenarioHolder(propertyBag);
		}
		return new MatsScenario(text, 0);
	}

	public IEnumerable<IPropertyBag> GetEventsForUpload()
	{
		lock (_lockScenarioIdToPropertyBag)
		{
			List<IPropertyBag> list = new List<IPropertyBag>();
			List<string> list2 = new List<string>();
			foreach (KeyValuePair<string, ScenarioHolder> item in _scenarioIdToPropertyBag)
			{
				ScenarioHolder value = item.Value;
				if (value.ShouldUpload)
				{
					list.Add(value.PropertyBag);
					list2.Add(item.Key);
				}
				else if ((DateTime.UtcNow - value.StartTime).TotalMilliseconds > (double)_maxInactiveScenarioDurationMillis)
				{
					list2.Add(item.Key);
				}
			}
			foreach (string item2 in list2)
			{
				_scenarioIdToPropertyBag.Remove(item2);
			}
			return list;
		}
	}

	public void ClearCompletedScenarios()
	{
		GetEventsForUpload();
	}

	public void NotifyActionCompleted(string scenarioId)
	{
		lock (_lockScenarioIdToPropertyBag)
		{
			if (!string.IsNullOrEmpty(scenarioId) && _scenarioIdToPropertyBag.TryGetValue(scenarioId, out var value))
			{
				value.ShouldUpload = true;
			}
		}
	}
}
