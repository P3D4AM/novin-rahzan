using System;
using System.Diagnostics;
using System.Reflection;
using PacketParser.Entities;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace PacketParser.Performance;

public class CallerEnricher : ILogEventEnricher
{
	public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Expected O, but got Unknown
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Expected O, but got Unknown
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Expected O, but got Unknown
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		int num = 3;
		while (true)
		{
			try
			{
				StackFrame stackFrame = new StackFrame(num);
				if (!stackFrame.HasMethod())
				{
					logEvent.AddPropertyIfAbsent(new LogEventProperty("Caller", (LogEventPropertyValue)new ScalarValue((object)"<unknown method>")));
					logEvent.AddPropertyIfAbsent(new LogEventProperty("FuncName", (LogEventPropertyValue)new ScalarValue((object)"<unknown method>")));
					break;
				}
				MethodBase method = stackFrame.GetMethod();
				if (method.DeclaringType.Assembly != typeof(Log).Assembly && method.DeclaringType.Name != "WebErrorLog")
				{
					string text = method.GetMethodDeclaringName() + "." + method.GetMethodName();
					logEvent.AddPropertyIfAbsent(new LogEventProperty("Caller", (LogEventPropertyValue)new ScalarValue((object)text)));
					logEvent.AddPropertyIfAbsent(new LogEventProperty("FuncName", (LogEventPropertyValue)new ScalarValue((object)method.GetMethodName())));
					logEvent.AddPropertyIfAbsent(new LogEventProperty("ClassName", (LogEventPropertyValue)new ScalarValue((object)method.GetMethodDeclaringName())));
					break;
				}
				num = checked(num + 1);
			}
			catch (Exception ex)
			{
				WebTelegramMessage.GetNp_ErrorHandler_bot().Sent2web("خطا در Enrich\r\n" + ex.Message, null, "Enrich", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\CallerEnricher.cs", 44);
				WebErrorLog.Instance.StartLog(ex, "", "Enrich", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\CallerEnricher.cs", 45);
			}
		}
	}
}
