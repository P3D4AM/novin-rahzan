using System;
using System.Net;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace PacketParser.Entities;

public class WebResponse<T>
{
	[Obsolete("please use HttpClietnStatusCode")]
	public ResponseStatus ResponseStatus { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public HttpStatusCode HttpClietnStatusCode { get; set; }

	public string Description { get; set; }

	public T Data { get; set; }

	public WebResponse()
	{
	}

	[Obsolete("please use another overload HttpClietnStatusCode")]
	public WebResponse(ResponseStatus status, string description = "", T data = default(T))
	{
		try
		{
			ResponseStatus = status;
			Description = description;
			Data = data;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebResponse.cs", 31);
		}
	}

	public WebResponse(HttpStatusCode status, string description = "", T data = default(T))
	{
		try
		{
			HttpClietnStatusCode = status;
			Description = description;
			Data = data;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebResponse.cs", 42);
		}
	}
}
