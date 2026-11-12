using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Azure.Core.Pipeline;

internal readonly struct DiagnosticScope : IDisposable
{
	private class DiagnosticActivity : Activity
	{
		private List<Activity>? _links;

		public new IEnumerable<Activity> Links
		{
			get
			{
				IEnumerable<Activity> links = _links;
				return links ?? Array.Empty<Activity>();
			}
		}

		public DiagnosticActivity(string operationName)
			: base(operationName)
		{
		}

		public void AddLink(Activity activity)
		{
			if (_links == null)
			{
				_links = new List<Activity>();
			}
			_links.Add(activity);
		}
	}

	private readonly DiagnosticActivity? _activity;

	private readonly string _name;

	private readonly DiagnosticListener _source;

	public bool IsEnabled => _activity != null;

	internal DiagnosticScope(string name, DiagnosticListener source)
	{
		_name = name;
		_source = source;
		_activity = (_source.IsEnabled(_name) ? new DiagnosticActivity(_name) : null);
		_activity?.SetW3CFormat();
	}

	public void AddAttribute(string name, string value)
	{
		_activity?.AddTag(name, value);
	}

	public void AddAttribute<T>(string name, T value)
	{
		if (_activity != null && value != null)
		{
			AddAttribute(name, value.ToString() ?? string.Empty);
		}
	}

	public void AddAttribute<T>(string name, T value, Func<T, string> format)
	{
		if (_activity != null)
		{
			AddAttribute(name, format(value));
		}
	}

	public void AddLink(string id, IDictionary<string, string>? attributes = null)
	{
		if (_activity == null)
		{
			return;
		}
		Activity activity = new Activity("LinkedActivity");
		activity.SetW3CFormat();
		activity.SetParentId(id);
		if (attributes != null)
		{
			foreach (KeyValuePair<string, string> attribute in attributes)
			{
				activity.AddTag(attribute.Key, attribute.Value);
			}
		}
		_activity.AddLink(activity);
	}

	public void Start()
	{
		if (_activity != null)
		{
			_source.StartActivity(_activity, _activity);
		}
	}

	public void SetStartTime(DateTime dateTime)
	{
		_activity?.SetStartTime(dateTime);
	}

	public void Dispose()
	{
		if (_activity != null)
		{
			if (_source != null)
			{
				_source.StopActivity(_activity, null);
			}
			else
			{
				_activity?.Stop();
			}
		}
	}

	public void Failed(Exception e)
	{
		if (_activity != null)
		{
			_source?.Write(_activity.OperationName + ".Exception", e);
		}
	}
}
