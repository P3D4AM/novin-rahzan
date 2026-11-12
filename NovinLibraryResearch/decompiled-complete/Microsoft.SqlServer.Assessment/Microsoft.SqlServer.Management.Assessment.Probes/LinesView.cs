using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.SqlServer.Management.Assessment.Probes;

internal class LinesView : IDataView, IReadOnlyList<IDataRow>, IReadOnlyCollection<IDataRow>, IEnumerable<IDataRow>, IEnumerable
{
	internal class Line : IDataRow
	{
		private readonly string str;

		private readonly string lineColumnName;

		public Line(string str, string lineColumnName)
		{
			this.str = str;
			this.lineColumnName = lineColumnName;
		}

		public bool TryGetData(string column, out object value)
		{
			if (column == lineColumnName)
			{
				value = str;
				return true;
			}
			value = null;
			return false;
		}
	}

	private readonly string lineColumnName;

	public List<Line> Lines { get; }

	public int Count => Lines.Count;

	public IDataRow this[int index] => Lines[index];

	public LinesView(IEnumerable<string> lines, string lineColumnName)
	{
		this.lineColumnName = lineColumnName;
		Lines = lines.Select(CreateLine).ToList();
	}

	private Line CreateLine(string str)
	{
		return new Line(str, lineColumnName);
	}

	public IEnumerator<IDataRow> GetEnumerator()
	{
		return Lines.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
