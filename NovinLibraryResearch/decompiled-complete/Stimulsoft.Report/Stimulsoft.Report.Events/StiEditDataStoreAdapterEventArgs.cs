using System;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Events;

public class StiEditDataStoreAdapterEventArgs : EventArgs
{
	public object Designer { get; }

	public StiDataAdapterService Adapter { get; }

	public StiDictionary Dictionary { get; }

	public StiDataSource DataSource { get; }

	public bool? DialogResult { get; set; }

	public StiEditDataStoreAdapterEventArgs(object designer, StiDataAdapterService adapter, StiDictionary dictionary, StiDataSource dataSource)
	{
		Adapter = adapter;
		Dictionary = dictionary;
		DataSource = dataSource;
		Designer = designer;
	}
}
