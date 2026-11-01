using System;
using System.Collections.Generic;
using System.Net;
using PacketParser.Attendance.Entities;
using Serilog;
using TimeAttendanceDAL.Dal.Device;
using TimeAttendanceDAL.Dal.FLRPerson;

namespace TimeAttendanceDevices.FLRHysoon.DynamicHysoonRealTime;

public class HttpServerDynamicHysoon
{
	public delegate void RealTimeHttpListenerEventHandler(string message);

	private IDeviceLogOnPersonRepository _dalRepository;

	private HttpListener httpListener = new HttpListener();

	private bool stop;

	private RequestHelperDynamicHysoon RequestHelper;

	private ResponseHelperDynamicHysoon ResponseHelper;

	public List<Person> Persons { get; set; }

	public List<HardwareCodeCus> HardwareCodes { get; set; }

	public string ContextGuid { get; private set; }

	public event RealTimeHttpListenerEventHandler RealTimeLogEvent;

	public void Disconnect()
	{
		stop = true;
		httpListener.Close();
		httpListener = null;
	}

	public bool Setup(int port, List<Person> persons, List<HardwareCodeCus> hardwareCodes, ref string ErrorStr, IDeviceLogOnPersonRepository dalRepository)
	{
		try
		{
			Persons = persons;
			HardwareCodes = hardwareCodes;
			_dalRepository = dalRepository;
			if (port == 0)
			{
				stop = true;
				httpListener.Close();
				ErrorStr = "شماره پورت نادرست است.";
				return false;
			}
			stop = false;
			httpListener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
			string uriPrefix = $"http://*:{port}/";
			httpListener.Prefixes.Add(uriPrefix);
			httpListener.Start();
			Receive();
			ErrorStr = $"جهت دریافت تردد آنی، پورت {port} باز شد";
			return true;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			ErrorStr = ex.Message;
		}
		return false;
	}

	private void Receive()
	{
		try
		{
			if (!stop)
			{
				httpListener.BeginGetContext(EndReciver, null);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void EndReciver(IAsyncResult ar)
	{
		try
		{
			if (!stop)
			{
				HttpListenerContext context = httpListener.EndGetContext(ar);
				Dispatcher(context);
				Receive();
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public HttpServerDynamicHysoon(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public void Dispatcher(HttpListenerContext context)
	{
		HttpListenerRequest request = context.Request;
		HttpListenerResponse response = context.Response;
		RequestHelper = new RequestHelperDynamicHysoon(ContextGuid, request);
		RequestHelper.RealTimeLogEvent += RequestHelper_RealTimeLogEvent;
		ResponseHelper = new ResponseHelperDynamicHysoon(response);
		string text = request.RemoteEndPoint.ToString();
		if (string.IsNullOrEmpty(text))
		{
			text = request.UserHostAddress;
		}
		RequestHelper.DispatchResources(delegate(int state)
		{
			ResponseHelper.WriteToClient(state);
		}, text, _dalRepository);
	}

	private void RequestHelper_RealTimeLogEvent(string message)
	{
		this.RealTimeLogEvent?.Invoke(message);
	}
}
