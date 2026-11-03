using System;
using System.Collections.Generic;
using PacketParser.Entities;

namespace PacketParser.Settings.Accounting;

public class ScaleEventAggregator
{
	private static Dictionary<string, ScaleEventAggregator> _instances = new Dictionary<string, ScaleEventAggregator>();

	public event EventHandler Changed;

	public static ScaleEventAggregator GetInstance(string contextGuid)
	{
		try
		{
			_instances.TryGetValue(contextGuid, out var value);
			if (value != null)
			{
				return value;
			}
			_instances.Remove(contextGuid);
			value = new ScaleEventAggregator();
			_instances.Add(contextGuid, value);
			return value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetInstance", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\ScaleSettings.cs", 27);
			return null;
		}
	}

	public static void RaiseChanged(string contextGuid, object sender, System.EventArgs e)
	{
		GetInstance(contextGuid)?.RaiseChanged_(sender, e);
	}

	private void RaiseChanged_(object sender, System.EventArgs e)
	{
		this.Changed?.Invoke(sender, e);
	}
}
