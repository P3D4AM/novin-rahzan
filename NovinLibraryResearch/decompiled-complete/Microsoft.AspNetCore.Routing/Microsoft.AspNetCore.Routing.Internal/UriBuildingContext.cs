using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;

namespace Microsoft.AspNetCore.Routing.Internal;

[DebuggerDisplay("{DebuggerToString(),nq}")]
public class UriBuildingContext
{
	private readonly StringBuilder _uri;

	private readonly List<BufferValue> _buffer;

	private readonly UrlEncoder _urlEncoder;

	private bool _hasEmptySegment;

	private int _lastValueOffset;

	public SegmentState BufferState { get; private set; }

	public SegmentState UriState { get; private set; }

	public TextWriter Writer { get; }

	public UriBuildingContext(UrlEncoder urlEncoder)
	{
		_urlEncoder = urlEncoder;
		_uri = new StringBuilder();
		_buffer = new List<BufferValue>();
		Writer = new StringWriter(_uri);
		_lastValueOffset = -1;
		BufferState = SegmentState.Beginning;
		UriState = SegmentState.Beginning;
	}

	public bool Accept(string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			if (UriState == SegmentState.Inside || BufferState == SegmentState.Inside)
			{
				return false;
			}
			_hasEmptySegment = true;
			return true;
		}
		if (_hasEmptySegment)
		{
			return false;
		}
		for (int i = 0; i < _buffer.Count; i++)
		{
			if (_buffer[i].RequiresEncoding)
			{
				_urlEncoder.Encode(Writer, _buffer[i].Value);
			}
			else
			{
				_uri.Append(_buffer[i].Value);
			}
		}
		_buffer.Clear();
		if (UriState == SegmentState.Beginning && BufferState == SegmentState.Beginning && _uri.Length != 0)
		{
			_uri.Append("/");
		}
		BufferState = SegmentState.Inside;
		UriState = SegmentState.Inside;
		_lastValueOffset = _uri.Length;
		if (_uri.Length == 0 && value.Length > 0 && value[0] == '/')
		{
			_uri.Append("/");
			_urlEncoder.Encode(Writer, value, 1, value.Length - 1);
		}
		else
		{
			_urlEncoder.Encode(Writer, value);
		}
		return true;
	}

	public void Remove(string literal)
	{
		_uri.Length = _lastValueOffset;
		_lastValueOffset = -1;
	}

	public bool Buffer(string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			if (BufferState == SegmentState.Inside)
			{
				return false;
			}
			_hasEmptySegment = true;
			return true;
		}
		if (_hasEmptySegment)
		{
			return false;
		}
		if (UriState == SegmentState.Inside)
		{
			return Accept(value);
		}
		if (UriState == SegmentState.Beginning && BufferState == SegmentState.Beginning)
		{
			if (_uri.Length != 0 || _buffer.Count != 0)
			{
				_buffer.Add(new BufferValue("/", requiresEncoding: false));
			}
			BufferState = SegmentState.Inside;
		}
		_buffer.Add(new BufferValue(value, requiresEncoding: true));
		return true;
	}

	public void EndSegment()
	{
		BufferState = SegmentState.Beginning;
		UriState = SegmentState.Beginning;
	}

	public void Clear()
	{
		_uri.Clear();
		if (_uri.Capacity > 128)
		{
			_uri.Capacity = 128;
		}
		_buffer.Clear();
		if (_buffer.Capacity > 8)
		{
			_buffer.Capacity = 8;
		}
		_hasEmptySegment = false;
		_lastValueOffset = -1;
		BufferState = SegmentState.Beginning;
		UriState = SegmentState.Beginning;
	}

	public override string ToString()
	{
		if (_uri.Length > 0 && _uri[0] != '/')
		{
			_uri.Insert(0, '/');
		}
		return _uri.ToString();
	}

	private string DebuggerToString()
	{
		return string.Format("{{Accepted: '{0}' Buffered: '{1}'}}", _uri, string.Join("", _buffer));
	}
}
