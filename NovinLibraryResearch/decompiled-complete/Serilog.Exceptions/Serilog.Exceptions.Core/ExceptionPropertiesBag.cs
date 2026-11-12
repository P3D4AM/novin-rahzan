using System;
using System.Collections.Generic;
using Serilog.Exceptions.Filters;

namespace Serilog.Exceptions.Core;

internal class ExceptionPropertiesBag : IExceptionPropertiesBag
{
	private const int AcceptableNumberOfSameNameProperties = 5;

	private readonly Exception exception;

	private readonly IExceptionPropertyFilter? filter;

	private readonly Dictionary<string, object?> properties = new Dictionary<string, object>();

	private bool resultsCollected;

	public ExceptionPropertiesBag(Exception exception, IExceptionPropertyFilter? filter = null)
	{
		this.exception = exception ?? throw new ArgumentNullException("exception");
		this.filter = filter;
	}

	public IReadOnlyDictionary<string, object?> GetResultDictionary()
	{
		resultsCollected = true;
		return properties;
	}

	public void AddProperty(string key, object? value)
	{
		if (key == null)
		{
			throw new ArgumentNullException("key");
		}
		if (resultsCollected)
		{
			throw new InvalidOperationException("Cannot add exception property '" + key + "' to bag, after results were already collected");
		}
		if (filter == null || !filter.ShouldPropertyBeFiltered(exception, key, value))
		{
			AddPairToProperties(key, value);
		}
	}

	public bool ContainsProperty(string key)
	{
		return properties.ContainsKey(key);
	}

	private static string GetReplacementKey(string key)
	{
		return key + "$";
	}

	private void AddPairToProperties(string key, object? value)
	{
		key = MakeSureKeyIsUnique(key);
		properties.Add(key, value);
	}

	private string MakeSureKeyIsUnique(string key)
	{
		int num = 0;
		while (properties.ContainsKey(key) && num < 5)
		{
			key = GetReplacementKey(key);
			num++;
		}
		return key;
	}
}
