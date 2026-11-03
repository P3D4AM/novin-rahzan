using System.Collections;

namespace Stimulsoft.Report.Dictionary;

public class StiRowsCollection : IEnumerable, IEnumerator
{
	private StiDataSource dataSource;

	object IEnumerator.Current => new StiRow(dataSource, dataSource.Position);

	public StiRow this[int rowIndex] => new StiRow(dataSource, rowIndex);

	public int Count => dataSource.Count;

	IEnumerator IEnumerable.GetEnumerator()
	{
		return this;
	}

	bool IEnumerator.MoveNext()
	{
		dataSource.Next();
		return !dataSource.IsEof;
	}

	void IEnumerator.Reset()
	{
		dataSource.First();
	}

	public StiRowsCollection(StiDataSource dataSource)
	{
		this.dataSource = dataSource;
	}
}
