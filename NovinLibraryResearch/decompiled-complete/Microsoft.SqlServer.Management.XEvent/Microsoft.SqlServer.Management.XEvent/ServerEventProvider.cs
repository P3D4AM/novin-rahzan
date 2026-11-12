using System.Globalization;
using System.Text;

namespace Microsoft.SqlServer.Management.XEvent;

internal class ServerEventProvider : IEventProvider
{
	private Event xevent;

	public ServerEventProvider(Event parent)
	{
		xevent = parent;
	}

	public string GetCreateScript()
	{
		StringBuilder stringBuilder = new StringBuilder(128);
		stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "ADD EVENT {0}", xevent.ScriptName));
		StringBuilder stringBuilder2 = new StringBuilder();
		int num = 0;
		foreach (Action action in xevent.Actions)
		{
			if (XEUtils.ToBeCreated(action.State))
			{
				num++;
				stringBuilder2.Append(action.GetScriptCreate() + ",");
			}
		}
		if (xevent.HasCustomizableField() || num > 0 || (xevent.PredicateExpression != null && string.Empty != xevent.PredicateExpression.Trim()))
		{
			stringBuilder.Append("(");
			if (xevent.HasCustomizableField())
			{
				stringBuilder.Append("SET ");
				foreach (EventField eventField in xevent.EventFields)
				{
					if (eventField.Value != null)
					{
						BaseXEStore parent = xevent.Parent.Parent;
						EventColumnInfo eventColumnInfo = parent.ObjectInfoSet.Get<EventInfo>(xevent.Name).EventColumnInfoSet[eventField.Name];
						stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "{0}={1},", eventField.Name, parent.FormatFieldValue(eventField.Value.ToString(), eventColumnInfo.TypePackageID, eventColumnInfo.TypeName)));
					}
				}
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
			}
			if (num > 0)
			{
				stringBuilder.AppendLine();
				stringBuilder.Append("    ACTION(");
				stringBuilder.Append((object?)stringBuilder2);
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
				stringBuilder.Append(")");
			}
			if (xevent.PredicateExpression != null && string.Empty != xevent.PredicateExpression.Trim())
			{
				stringBuilder.AppendLine();
				stringBuilder.Append("    WHERE ");
				stringBuilder.Append(xevent.PredicateExpression);
			}
			stringBuilder.Append(")");
		}
		return stringBuilder.ToString();
	}

	public string GetDropScript()
	{
		return string.Format(CultureInfo.InvariantCulture, "DROP EVENT {0}", xevent.ScriptName);
	}
}
