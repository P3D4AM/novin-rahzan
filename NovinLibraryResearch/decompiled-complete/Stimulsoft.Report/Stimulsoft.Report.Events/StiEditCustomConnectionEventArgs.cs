using System;
using System.Windows.Forms;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Events;

public class StiEditCustomConnectionEventArgs : EventArgs
{
	public StiDatabase Database { get; }

	public StiDictionary Dictionary { get; }

	public object Designer { get; }

	public bool IsNewConnection { get; }

	public DialogResult? DialogResult { get; set; }

	public StiEditCustomConnectionEventArgs(StiDatabase database, StiDictionary dictionary, object designer, bool isNewConnection)
	{
		Database = database;
		Dictionary = dictionary;
		Designer = designer;
		IsNewConnection = isNewConnection;
	}
}
