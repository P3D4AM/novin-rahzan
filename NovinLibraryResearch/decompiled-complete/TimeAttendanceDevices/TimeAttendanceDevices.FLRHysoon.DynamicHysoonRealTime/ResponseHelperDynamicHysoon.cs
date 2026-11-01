using System;
using System.IO;
using System.Net;

namespace TimeAttendanceDevices.FLRHysoon.DynamicHysoonRealTime;

public class ResponseHelperDynamicHysoon
{
	private HttpListenerResponse response;

	private Stream OutputStream;

	public Stream _OutputStream
	{
		get
		{
			return OutputStream;
		}
		set
		{
			OutputStream = value;
		}
	}

	public ResponseHelperDynamicHysoon(HttpListenerResponse response)
	{
		this.response = response;
		OutputStream = response.OutputStream;
	}

	public void WriteToClient(int state)
	{
		response.StatusCode = 200;
		try
		{
			using (new StreamWriter(OutputStream))
			{
				response.Headers["response_code"] = "OK";
				response.Headers["trans_id"] = "100";
			}
		}
		catch (Exception)
		{
		}
	}
}
