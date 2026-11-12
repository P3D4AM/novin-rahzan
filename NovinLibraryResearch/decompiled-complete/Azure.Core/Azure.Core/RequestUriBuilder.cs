using System;
using System.Text;

namespace Azure.Core;

public class RequestUriBuilder
{
	private const char QuerySeparator = '?';

	private const char PathSeparator = '/';

	private readonly StringBuilder _pathAndQuery = new StringBuilder();

	private int _queryIndex = -1;

	private Uri? _uri;

	private int _port;

	private string? _host;

	private string? _scheme;

	public string? Scheme
	{
		get
		{
			return _scheme;
		}
		set
		{
			ResetUri();
			_scheme = value;
		}
	}

	public string? Host
	{
		get
		{
			return _host;
		}
		set
		{
			ResetUri();
			_host = value;
		}
	}

	public int Port
	{
		get
		{
			return _port;
		}
		set
		{
			ResetUri();
			_port = value;
		}
	}

	public string Query
	{
		get
		{
			if (!HasQuery)
			{
				return string.Empty;
			}
			return _pathAndQuery.ToString(_queryIndex, _pathAndQuery.Length - _queryIndex);
		}
		set
		{
			ResetUri();
			if (HasQuery)
			{
				_pathAndQuery.Remove(_queryIndex, _pathAndQuery.Length - _queryIndex);
				_queryIndex = -1;
			}
			if (!string.IsNullOrEmpty(value))
			{
				_queryIndex = _pathAndQuery.Length;
				if (value[0] != '?')
				{
					_pathAndQuery.Append('?');
				}
				_pathAndQuery.Append(value);
			}
		}
	}

	public string Path
	{
		get
		{
			if (!HasQuery)
			{
				return _pathAndQuery.ToString();
			}
			return _pathAndQuery.ToString(0, _queryIndex);
		}
		set
		{
			if (HasQuery)
			{
				_pathAndQuery.Remove(0, _queryIndex);
				_pathAndQuery.Insert(0, value);
				_queryIndex = value.Length;
			}
			else
			{
				_pathAndQuery.Remove(0, _pathAndQuery.Length);
				_pathAndQuery.Append(value);
			}
		}
	}

	private bool HasQuery => _queryIndex != -1;

	private int QueryLength
	{
		get
		{
			if (!HasQuery)
			{
				return 0;
			}
			return _pathAndQuery.Length - _queryIndex;
		}
	}

	private int PathLength
	{
		get
		{
			if (!HasQuery)
			{
				return _pathAndQuery.Length;
			}
			return _queryIndex;
		}
	}

	public string PathAndQuery => _pathAndQuery.ToString();

	private bool HasDefaultPortForScheme
	{
		get
		{
			if (Port != 80 || !string.Equals(Scheme, "http", StringComparison.InvariantCultureIgnoreCase))
			{
				if (Port == 443)
				{
					return string.Equals(Scheme, "https", StringComparison.InvariantCultureIgnoreCase);
				}
				return false;
			}
			return true;
		}
	}

	public void Reset(Uri value)
	{
		Scheme = value.Scheme;
		Host = value.Host;
		Port = value.Port;
		Path = value.AbsolutePath;
		Query = value.Query;
		_uri = value;
	}

	public Uri ToUri()
	{
		if (_uri == null)
		{
			_uri = new Uri(ToString());
		}
		return _uri;
	}

	public void AppendQuery(string name, string value)
	{
		AppendQuery(name, value, escapeValue: true);
	}

	public void AppendQuery(string name, string value, bool escapeValue)
	{
		ResetUri();
		if (!HasQuery)
		{
			_queryIndex = _pathAndQuery.Length;
			_pathAndQuery.Append('?');
		}
		else if (QueryLength != 1 || _pathAndQuery[_queryIndex] != '?')
		{
			_pathAndQuery.Append('&');
		}
		_pathAndQuery.Append(name);
		_pathAndQuery.Append('=');
		if (escapeValue && !string.IsNullOrEmpty(value))
		{
			value = Uri.EscapeDataString(value);
		}
		_pathAndQuery.Append(value);
	}

	public void AppendPath(string value)
	{
		AppendPath(value, escape: true);
	}

	public void AppendPath(string value, bool escape)
	{
		if (string.IsNullOrEmpty(value))
		{
			return;
		}
		ResetUri();
		int num = 0;
		if (PathLength == 1 && _pathAndQuery[0] == '/' && value[0] == '/')
		{
			num = 1;
		}
		if (HasQuery)
		{
			string text = value.Substring(num, value.Length - num);
			if (escape)
			{
				text = Uri.EscapeDataString(text);
			}
			_pathAndQuery.Insert(_queryIndex, text);
			_queryIndex += value.Length;
		}
		else if (escape)
		{
			string stringToEscape = value.Substring(num, value.Length - num);
			stringToEscape = Uri.EscapeDataString(stringToEscape);
			_pathAndQuery.Append(stringToEscape);
		}
		else
		{
			_pathAndQuery.Append(value, num, value.Length - num);
		}
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Scheme);
		stringBuilder.Append("://");
		stringBuilder.Append(Host);
		if (!HasDefaultPortForScheme)
		{
			stringBuilder.Append(':');
			stringBuilder.Append(Port);
		}
		if (_pathAndQuery.Length == 0 || _pathAndQuery[0] != '/')
		{
			stringBuilder.Append('/');
		}
		if (!HasQuery)
		{
			stringBuilder.Append((object?)_pathAndQuery);
		}
		else
		{
			stringBuilder.Append(_pathAndQuery.ToString(0, _queryIndex));
			stringBuilder.Append(_pathAndQuery.ToString(_queryIndex, _pathAndQuery.Length - _queryIndex));
		}
		return stringBuilder.ToString();
	}

	private void ResetUri()
	{
		_uri = null;
	}
}
