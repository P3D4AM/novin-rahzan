using System;
using System.IO;
using SharpCompress.Compressors.LZMA.RangeCoder;
using SharpCompress.Compressors.PPMd.H;
using SharpCompress.Compressors.PPMd.I1;

namespace SharpCompress.Compressors.PPMd;

public class PpmdStream : Stream
{
	private readonly PpmdProperties _properties;

	private readonly Stream _stream;

	private readonly bool _compress;

	private readonly Model _model;

	private readonly ModelPpm _modelH;

	private readonly Decoder _decoder;

	private long _position;

	private bool _isDisposed;

	public override bool CanRead => !_compress;

	public override bool CanSeek => false;

	public override bool CanWrite => _compress;

	public override long Length
	{
		get
		{
			throw new NotSupportedException();
		}
	}

	public override long Position
	{
		get
		{
			return _position;
		}
		set
		{
			throw new NotSupportedException();
		}
	}

	public PpmdStream(PpmdProperties properties, Stream stream, bool compress)
	{
		_properties = properties;
		_stream = stream;
		_compress = compress;
		if (properties.Version == PpmdVersion.I1)
		{
			_model = new Model();
			if (compress)
			{
				_model.EncodeStart(properties);
			}
			else
			{
				_model.DecodeStart(stream, properties);
			}
		}
		if (properties.Version == PpmdVersion.H)
		{
			_modelH = new ModelPpm();
			if (compress)
			{
				throw new NotImplementedException();
			}
			_modelH.DecodeInit(stream, properties.ModelOrder, properties.AllocatorSize);
		}
		if (properties.Version == PpmdVersion.H7Z)
		{
			_modelH = new ModelPpm();
			if (compress)
			{
				throw new NotImplementedException();
			}
			_modelH.DecodeInit(null, properties.ModelOrder, properties.AllocatorSize);
			_decoder = new Decoder();
			_decoder.Init(stream);
		}
	}

	public override void Flush()
	{
	}

	protected override void Dispose(bool isDisposing)
	{
		if (!_isDisposed)
		{
			_isDisposed = true;
			if (isDisposing && _compress)
			{
				_model.EncodeBlock(_stream, new MemoryStream(), final: true);
			}
			base.Dispose(isDisposing);
		}
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		if (_compress)
		{
			return 0;
		}
		int i = 0;
		if (_properties.Version == PpmdVersion.I1)
		{
			i = _model.DecodeBlock(_stream, buffer, offset, count);
		}
		if (_properties.Version == PpmdVersion.H)
		{
			for (; i < count; i++)
			{
				int num;
				if ((num = _modelH.DecodeChar()) < 0)
				{
					break;
				}
				buffer[offset++] = (byte)num;
			}
		}
		if (_properties.Version == PpmdVersion.H7Z)
		{
			for (; i < count; i++)
			{
				int num2;
				if ((num2 = _modelH.DecodeChar(_decoder)) < 0)
				{
					break;
				}
				buffer[offset++] = (byte)num2;
			}
		}
		_position += i;
		return i;
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		throw new NotSupportedException();
	}

	public override void SetLength(long value)
	{
		throw new NotSupportedException();
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		if (_compress)
		{
			_model.EncodeBlock(_stream, new MemoryStream(buffer, offset, count), final: false);
		}
	}
}
