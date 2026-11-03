using System.CodeDom;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.CodeDom;

internal class StiCodeDomDatabases
{
	internal static void Serialize(StiCodeDomSerializator serializator, StiReport report)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Expected O, but got Unknown
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Expected O, but got Unknown
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Expected O, but got Unknown
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Expected O, but got Unknown
		//IL_0103: Expected O, but got Unknown
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Expected O, but got Unknown
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Expected O, but got Unknown
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Expected O, but got Unknown
		//IL_0127: Expected O, but got Unknown
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Expected O, but got Unknown
		if (report.Dictionary.Databases.Count <= 0)
		{
			return;
		}
		foreach (StiDatabase database in report.Dictionary.Databases)
		{
			CodeExpression val = (CodeExpression)new CodeObjectCreateExpression(((object)database).GetType(), serializator.GetArguments(((object)database).GetType(), database));
			serializator.memberMethod.Statements.Add((CodeExpression)new CodeMethodInvokeExpression((CodeExpression)new CodePropertyReferenceExpression((CodeExpression)new CodePropertyReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), "Dictionary"), "Databases"), "Add", (CodeExpression[])(object)new CodeExpression[1] { val }));
			if (database is StiSqlDatabase && !string.IsNullOrWhiteSpace(database.Name) && !string.IsNullOrWhiteSpace((database as StiSqlDatabase).ConnectionStringEncrypted))
			{
				serializator.memberMethod.Statements.Add((CodeStatement)new CodeAssignStatement((CodeExpression)new CodePropertyReferenceExpression((CodeExpression)new CodeCastExpression(typeof(StiSqlDatabase), (CodeExpression)new CodeIndexerExpression((CodeExpression)new CodePropertyReferenceExpression((CodeExpression)new CodePropertyReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), "Dictionary"), "Databases"), (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)new CodePrimitiveExpression((object)database.Name) })), "ConnectionStringEncrypted"), (CodeExpression)new CodePrimitiveExpression((object)(database as StiSqlDatabase).ConnectionStringEncrypted)));
			}
			AddEvent(serializator, report, database.Name, database.ConnectingEvent);
			AddEvent(serializator, report, database.Name, database.ConnectedEvent);
			AddEvent(serializator, report, database.Name, database.DisconnectingEvent);
			AddEvent(serializator, report, database.Name, database.DisconnectedEvent);
		}
	}

	internal static void AddEvent(StiCodeDomSerializator serializator, StiReport report, string databaseName, StiEvent ev)
	{
		if (ev.Script.Length > 0)
		{
			string script = ev.Script;
			script = StiCodeDomFunctions.ParseFunctions(serializator, script);
			string eventName = ((databaseName.Length <= 0) ? (report.GetReportName() + "_" + ev) : (databaseName + "_" + ev));
			string parent = ((report.ScriptLanguage == StiReportLanguageType.CSharp || report.ScriptLanguage == StiReportLanguageType.JS) ? ("Dictionary.Databases[\"" + databaseName + "\"]") : ("Dictionary.Databases.Item(\"" + databaseName + "\")"));
			serializator.GenAddEvent(eventName, parent, ev);
			serializator.GenEventMethod(eventName, script, ev, databaseName);
		}
	}
}
