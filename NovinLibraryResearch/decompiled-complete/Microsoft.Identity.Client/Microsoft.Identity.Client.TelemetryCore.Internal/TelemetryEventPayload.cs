using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Identity.Json.Linq;

namespace Microsoft.Identity.Client.TelemetryCore.Internal;

internal class TelemetryEventPayload : ITelemetryEventPayload
{
	private readonly Dictionary<string, bool> _boolData = new Dictionary<string, bool>();

	private readonly Dictionary<string, long> _int64Data = new Dictionary<string, long>();

	private readonly Dictionary<string, int> _intData = new Dictionary<string, int>();

	private readonly Dictionary<string, string> _stringData = new Dictionary<string, string>();

	public IReadOnlyDictionary<string, bool> BoolValues => _boolData;

	public IReadOnlyDictionary<string, long> Int64Values => _int64Data;

	public IReadOnlyDictionary<string, int> IntValues => _intData;

	public IReadOnlyDictionary<string, string> StringValues => _stringData;

	public string Name { get; }

	public static ITelemetryEventPayload Create(string name, PropertyBagContents contents)
	{
		TelemetryEventPayload telemetryEventPayload = new TelemetryEventPayload(name);
		telemetryEventPayload.SetStringData(contents.StringProperties);
		telemetryEventPayload.SetIntData(contents.IntProperties);
		telemetryEventPayload.SetInt64Data(contents.Int64Properties);
		telemetryEventPayload.SetBoolData(contents.BoolProperties);
		return telemetryEventPayload;
	}

	public string ToJsonString()
	{
		JObject jObject = new JObject();
		jObject["name"] = Name;
		JObject jObject2 = new JObject();
		foreach (KeyValuePair<string, bool> boolDatum in _boolData)
		{
			jObject2[boolDatum.Key] = boolDatum.Value;
		}
		foreach (KeyValuePair<string, long> int64Datum in _int64Data)
		{
			jObject2[int64Datum.Key] = int64Datum.Value;
		}
		foreach (KeyValuePair<string, int> intDatum in _intData)
		{
			jObject2[intDatum.Key] = intDatum.Value;
		}
		foreach (KeyValuePair<string, string> stringDatum in _stringData)
		{
			jObject2[stringDatum.Key] = stringDatum.Value;
		}
		jObject["data"] = jObject2;
		return jObject.ToString();
	}

	private void SetBoolData(ConcurrentDictionary<string, bool> data)
	{
		_boolData.Clear();
		foreach (KeyValuePair<string, bool> datum in data)
		{
			_boolData[datum.Key] = datum.Value;
		}
	}

	private void SetInt64Data(ConcurrentDictionary<string, long> data)
	{
		_int64Data.Clear();
		foreach (KeyValuePair<string, long> datum in data)
		{
			_int64Data[datum.Key] = datum.Value;
		}
	}

	private void SetIntData(ConcurrentDictionary<string, int> data)
	{
		_intData.Clear();
		foreach (KeyValuePair<string, int> datum in data)
		{
			_intData[datum.Key] = datum.Value;
		}
	}

	private void SetStringData(ConcurrentDictionary<string, string> data)
	{
		_stringData.Clear();
		foreach (KeyValuePair<string, string> datum in data)
		{
			_stringData[datum.Key] = datum.Value;
		}
	}

	private TelemetryEventPayload(string name)
	{
		Name = name;
	}
}
