using System;
using System.Threading;

namespace Microsoft.AspNetCore.Http.Features;

public class HttpRequestIdentifierFeature : IHttpRequestIdentifierFeature
{
	private static readonly string _encode32Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUV";

	private static long _requestId = DateTime.UtcNow.Ticks;

	private string _id;

	public string TraceIdentifier
	{
		get
		{
			if (_id == null)
			{
				_id = GenerateRequestId(Interlocked.Increment(ref _requestId));
			}
			return _id;
		}
		set
		{
			_id = value;
		}
	}

	private unsafe static string GenerateRequestId(long id)
	{
		return new string((char*)stackalloc char[13]
		{
			_encode32Chars[(int)(id >> 60) & 0x1F],
			_encode32Chars[(int)(id >> 55) & 0x1F],
			_encode32Chars[(int)(id >> 50) & 0x1F],
			_encode32Chars[(int)(id >> 45) & 0x1F],
			_encode32Chars[(int)(id >> 40) & 0x1F],
			_encode32Chars[(int)(id >> 35) & 0x1F],
			_encode32Chars[(int)(id >> 30) & 0x1F],
			_encode32Chars[(int)(id >> 25) & 0x1F],
			_encode32Chars[(int)(id >> 20) & 0x1F],
			_encode32Chars[(int)(id >> 15) & 0x1F],
			_encode32Chars[(int)(id >> 10) & 0x1F],
			_encode32Chars[(int)(id >> 5) & 0x1F],
			_encode32Chars[(int)id & 0x1F]
		}, 0, 13);
	}
}
