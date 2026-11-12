using System;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.XEvent;

public abstract class SessionProviderBase : ISessionProvider
{
	private const string SessionStart = "STATE=START";

	private const string SessionStop = "STATE=STOP";

	private readonly string eventSessionScript;

	private readonly Func<string, string> createSessionScript;

	private Session session;

	protected SessionProviderBase(Session session, string eventSessionScript, Func<string, string> createSessionScript)
	{
		this.session = session;
		this.eventSessionScript = eventSessionScript;
		this.createSessionScript = createSessionScript;
	}

	public ISfcScript GetCreateScript()
	{
		return CreateScript("CREATE " + eventSessionScript);
	}

	public ISfcScript GetAlterScript()
	{
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		StringBuilder stringBuilder3 = new StringBuilder();
		StringBuilder stringBuilder4 = new StringBuilder();
		StringBuilder stringBuilder5 = new StringBuilder();
		session.Events.AppendAlterScripts(stringBuilder2, stringBuilder3);
		session.Targets.AppendAlterScripts(stringBuilder4, stringBuilder5);
		string value = "ALTER " + eventSessionScript;
		if (stringBuilder3.Length > 0)
		{
			stringBuilder.AppendLine(value).AppendLine(stringBuilder3.ToString());
		}
		if (stringBuilder5.Length > 0)
		{
			stringBuilder.AppendLine(value).AppendLine(stringBuilder5.ToString());
		}
		if (stringBuilder4.Length > 0)
		{
			stringBuilder.AppendLine(value).AppendLine(stringBuilder4.ToString());
		}
		AppendOptionsAlterScript(stringBuilder);
		if (stringBuilder2.Length > 0)
		{
			stringBuilder.AppendLine(value).AppendLine(stringBuilder2.ToString());
		}
		return new SfcTSqlScript(stringBuilder.ToString().TrimEnd(Array.Empty<char>()));
	}

	public ISfcScript GetDropScript()
	{
		return new SfcTSqlScript("DROP " + eventSessionScript);
	}

	public void ValidateAlter()
	{
		bool flag = false;
		foreach (Event @event in session.Events)
		{
			if (XEUtils.ToBeCreated(@event.State))
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			return;
		}
		string text = "dummy_session" + Guid.NewGuid();
		try
		{
			SfcTSqlScript script = GetCreateScript(text) as SfcTSqlScript;
			((ISfcDomain)session.Parent).GetExecutionEngine().Execute(script);
		}
		catch (Exception innerException)
		{
			throw new XEventException(ExceptionTemplates.AlterValidationFailure, innerException);
		}
		finally
		{
			session.Parent.Sessions[text]?.Drop();
		}
	}

	public void Start()
	{
		string batch = "ALTER " + eventSessionScript + "STATE=START";
		((ISfcDomain)session.Parent).GetExecutionEngine().Execute(new SfcTSqlScript(batch));
	}

	public void Stop()
	{
		string batch = "ALTER " + eventSessionScript + "STATE=STOP";
		((ISfcDomain)session.Parent).GetExecutionEngine().Execute(new SfcTSqlScript(batch));
	}

	private ISfcScript CreateScript(string createStatment)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(createStatment);
		string value = string.Join(",", (from ISessionObject so in session.Events.Where((Event ev) => XEUtils.ToBeCreated(ev.State))
			select Environment.NewLine + so.GetCreateScript()).ToArray());
		stringBuilder.Append(value);
		string value2 = string.Join(",", (from ISessionObject so in session.Targets.Where((Target tgt) => XEUtils.ToBeCreated(tgt.State))
			select Environment.NewLine + so.GetCreateScript()).ToArray());
		stringBuilder.Append(value2);
		string optionString = GetOptionString(create: true);
		if (optionString.Length != 0)
		{
			stringBuilder.AppendLine();
			stringBuilder.Append("WITH (");
			stringBuilder.Append(optionString);
			stringBuilder.Append(")");
		}
		return new SfcTSqlScript(stringBuilder.ToString());
	}

	private ISfcScript GetCreateScript(string sessionName)
	{
		string createStatment = createSessionScript(sessionName);
		return CreateScript(createStatment);
	}

	private string GetOptionString(bool create)
	{
		bool flag = create && session.State == SfcObjectState.Existing;
		StringBuilder stringBuilder = new StringBuilder(256);
		if (flag || session.Properties["MaxMemory"].Dirty)
		{
			stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "MAX_MEMORY={0} KB,", session.MaxMemory);
		}
		if (flag || session.Properties["EventRetentionMode"].Dirty)
		{
			string empty = string.Empty;
			stringBuilder.AppendFormat(arg0: session.EventRetentionMode switch
			{
				Session.EventRetentionModeEnum.AllowMultipleEventLoss => "ALLOW_MULTIPLE_EVENT_LOSS", 
				Session.EventRetentionModeEnum.NoEventLoss => "NO_EVENT_LOSS", 
				_ => "ALLOW_SINGLE_EVENT_LOSS", 
			}, provider: CultureInfo.InvariantCulture, format: "EVENT_RETENTION_MODE={0},");
		}
		if (flag || session.Properties["MaxDispatchLatency"].Dirty)
		{
			stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "MAX_DISPATCH_LATENCY={0} SECONDS,", session.MaxDispatchLatency);
		}
		if (flag || session.Properties["MaxEventSize"].Dirty)
		{
			stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "MAX_EVENT_SIZE={0} KB,", session.MaxEventSize);
		}
		if (flag || session.Properties["MemoryPartitionMode"].Dirty)
		{
			string value = string.Empty;
			switch (session.MemoryPartitionMode)
			{
			case Session.MemoryPartitionModeEnum.None:
				value = "NONE";
				break;
			case Session.MemoryPartitionModeEnum.PerNode:
				value = "PER_NODE";
				break;
			case Session.MemoryPartitionModeEnum.PerCpu:
				value = "PER_CPU";
				break;
			}
			stringBuilder.Append("MEMORY_PARTITION_MODE=");
			stringBuilder.Append(value);
			stringBuilder.Append(",");
		}
		if (flag || session.Properties["TrackCausality"].Dirty)
		{
			stringBuilder.Append("TRACK_CAUSALITY=");
			stringBuilder.Append(session.TrackCausality ? "ON" : "OFF");
			stringBuilder.Append(",");
		}
		if (flag || session.Properties["AutoStart"].Dirty)
		{
			stringBuilder.Append("STARTUP_STATE=");
			stringBuilder.Append(session.AutoStart ? "ON" : "OFF");
			stringBuilder.Append(",");
		}
		if (stringBuilder.Length > 0)
		{
			stringBuilder.Remove(stringBuilder.Length - 1, 1);
		}
		return stringBuilder.ToString();
	}

	private void AppendOptionsAlterScript(StringBuilder sessionAlterScripts)
	{
		string optionString = GetOptionString(create: false);
		if (optionString.Length != 0)
		{
			if (sessionAlterScripts.Length == 0)
			{
				sessionAlterScripts.Append("ALTER ");
				sessionAlterScripts.AppendLine(eventSessionScript);
			}
			sessionAlterScripts.Append(" WITH (");
			sessionAlterScripts.Append(optionString);
			sessionAlterScripts.AppendLine(") ");
		}
	}
}
