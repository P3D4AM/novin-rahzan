using System;
using System.Net;

namespace PacketParser.Exceptions;

public class UnrecoverableWebException : Exception
{
	public HttpStatusCode StatusCode { get; set; }

	public UnrecoverableWebException(HttpStatusCode code)
	{
		StatusCode = code;
	}
}
