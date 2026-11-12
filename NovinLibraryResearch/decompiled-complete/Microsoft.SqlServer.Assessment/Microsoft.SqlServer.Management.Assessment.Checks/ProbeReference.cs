using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.Management.Assessment.Expressions;
using Newtonsoft.Json;

namespace Microsoft.SqlServer.Management.Assessment.Checks;

public class ProbeReference
{
	public string ProbeName { get; }

	public string ProbeNameAlias { get; }

	internal Dictionary<string, Expression> Parameters { get; }

	[JsonProperty("transform")]
	public IDataMorph Morph { get; set; }

	public ProbeReference(string probeName, string probeNameAlias = null)
	{
		ProbeName = probeName;
		ProbeNameAlias = probeNameAlias;
	}

	public ProbeReference(string probeName, IDictionary<string, Expression> parameters, string probeNameAlias = null, IDataMorph dataMorph = null)
	{
		ProbeName = probeName;
		ProbeNameAlias = probeNameAlias;
		if (parameters != null)
		{
			Parameters = new Dictionary<string, Expression>(parameters);
		}
		Morph = dataMorph;
	}

	public bool Equals(ProbeReference other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (string.CompareOrdinal(ProbeName, other.ProbeName) != 0 || string.CompareOrdinal(ProbeNameAlias, other.ProbeNameAlias) != 0)
		{
			return false;
		}
		if (Parameters == other.Parameters)
		{
			return true;
		}
		if (Parameters == null || other.Parameters == null)
		{
			return false;
		}
		if (Parameters.Count != other.Parameters.Count)
		{
			return false;
		}
		foreach (KeyValuePair<string, Expression> parameter in Parameters)
		{
			if (!other.Parameters.TryGetValue(parameter.Key, out var value))
			{
				return false;
			}
			if (parameter.Value == null)
			{
				if (value != null)
				{
					return false;
				}
			}
			else if (!parameter.Value.Equals(value))
			{
				return false;
			}
		}
		return true;
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		if (this == obj)
		{
			return true;
		}
		if (GetType() != obj.GetType())
		{
			return false;
		}
		return Equals((ProbeReference)obj);
	}

	public override int GetHashCode()
	{
		int num = ProbeName?.GetHashCode() ?? 0;
		num = ((num * 397) ^ ProbeNameAlias?.GetHashCode()).GetValueOrDefault();
		if (Parameters != null)
		{
			foreach (KeyValuePair<string, Expression> item in Parameters.OrderBy<KeyValuePair<string, Expression>, string>((KeyValuePair<string, Expression> i) => i.Key, StringComparer.Ordinal))
			{
				num = (num * 397) ^ item.Key.GetHashCode();
				num = ((num * 397) ^ item.Value?.GetHashCode()).GetValueOrDefault();
			}
		}
		return num;
	}
}
