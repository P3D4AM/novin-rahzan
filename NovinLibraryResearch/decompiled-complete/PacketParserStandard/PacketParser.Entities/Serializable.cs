using System;
using System.Threading;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace PacketParser.Entities;

public class Serializable<T> : IDisposable
{
	private bool _isDisposed;

	[XmlIgnore]
	[JsonIgnore]
	public bool IsDisposed => _isDisposed;

	public static T FromString(string data)
	{
		return data.FromJson<T>(sendErrorIfException: true, "FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Entities\\Serializable.cs", 14);
	}

	public virtual string ToString(bool isIndented = false)
	{
		if (IsDisposed)
		{
			return "";
		}
		try
		{
			return Json.ToStringJson(this, isIndented);
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (ThreadAbortException)
		{
			throw;
		}
		catch (Exception ex3)
		{
			WebErrorLog.Instance.StartLog(ex3, "", "ToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Entities\\Serializable.cs", 26);
			return "";
		}
	}

	public override string ToString()
	{
		if (!IsDisposed)
		{
			return Json.ToStringJson(this);
		}
		return "";
	}

	public virtual string ToString(JsonSerializerSettings options)
	{
		if (!IsDisposed)
		{
			return Json.ToStringJson(this, options);
		}
		return "";
	}

	public virtual void Dispose()
	{
		Dispose(disposing: true);
	}

	public virtual void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Entities\\Serializable.cs", 40);
		}
	}
}
