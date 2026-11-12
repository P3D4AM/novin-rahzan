using System;
using System.Collections.Generic;
using System.Data;
using ExcelDataReader.Core;

namespace ExcelDataReader;

internal abstract class ExcelDataReader<TWorkbook, TWorksheet> : IExcelDataReader, IDataReader, IDisposable, IDataRecord where TWorkbook : IWorkbook<TWorksheet> where TWorksheet : IWorksheet
{
	private IEnumerator<TWorksheet> _worksheetIterator;

	private IEnumerator<Row> _rowIterator;

	private IEnumerator<TWorksheet> _cachedWorksheetIterator;

	private List<TWorksheet> _cachedWorksheets;

	public string Name => _worksheetIterator?.Current?.Name;

	public string CodeName => _worksheetIterator?.Current?.CodeName;

	public string VisibleState => _worksheetIterator?.Current?.VisibleState;

	public HeaderFooter HeaderFooter => _worksheetIterator?.Current?.HeaderFooter;

	public CellRange[] MergeCells => _worksheetIterator?.Current?.MergeCells;

	public int Depth { get; private set; }

	public int ResultsCount => Workbook?.ResultsCount ?? (-1);

	public bool IsClosed { get; private set; }

	public int FieldCount => _worksheetIterator?.Current?.FieldCount ?? 0;

	public int RowCount => _worksheetIterator?.Current?.RowCount ?? 0;

	public int RecordsAffected
	{
		get
		{
			throw new NotSupportedException();
		}
	}

	public double RowHeight => _rowIterator?.Current.Height ?? 0.0;

	protected TWorkbook Workbook { get; set; }

	protected Cell[] RowCells { get; set; }

	public object this[int i] => GetValue(i);

	public object this[string name]
	{
		get
		{
			throw new NotSupportedException();
		}
	}

	~ExcelDataReader()
	{
		Dispose(disposing: false);
	}

	public bool GetBoolean(int i)
	{
		return (bool)GetValue(i);
	}

	public byte GetByte(int i)
	{
		return (byte)GetValue(i);
	}

	public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
	{
		throw new NotSupportedException();
	}

	public char GetChar(int i)
	{
		return (char)GetValue(i);
	}

	public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
	{
		throw new NotSupportedException();
	}

	public IDataReader GetData(int i)
	{
		throw new NotSupportedException();
	}

	public string GetDataTypeName(int i)
	{
		throw new NotSupportedException();
	}

	public DateTime GetDateTime(int i)
	{
		return (DateTime)GetValue(i);
	}

	public decimal GetDecimal(int i)
	{
		return (decimal)GetValue(i);
	}

	public double GetDouble(int i)
	{
		return (double)GetValue(i);
	}

	public Type GetFieldType(int i)
	{
		return GetValue(i)?.GetType();
	}

	public float GetFloat(int i)
	{
		return (float)GetValue(i);
	}

	public Guid GetGuid(int i)
	{
		return (Guid)GetValue(i);
	}

	public short GetInt16(int i)
	{
		return (short)GetValue(i);
	}

	public int GetInt32(int i)
	{
		return (int)GetValue(i);
	}

	public long GetInt64(int i)
	{
		return (long)GetValue(i);
	}

	public string GetName(int i)
	{
		throw new NotSupportedException();
	}

	public int GetOrdinal(string name)
	{
		throw new NotSupportedException();
	}

	public DataTable GetSchemaTable()
	{
		throw new NotSupportedException();
	}

	public string GetString(int i)
	{
		return (string)GetValue(i);
	}

	public object GetValue(int i)
	{
		if (RowCells == null)
		{
			throw new InvalidOperationException("No data exists for the row/column.");
		}
		return RowCells[i]?.Value;
	}

	public int GetValues(object[] values)
	{
		throw new NotSupportedException();
	}

	public bool IsDBNull(int i)
	{
		return GetValue(i) == null;
	}

	public string GetNumberFormatString(int i)
	{
		if (RowCells == null)
		{
			throw new InvalidOperationException("No data exists for the row/column.");
		}
		if (RowCells[i] == null)
		{
			return null;
		}
		return _worksheetIterator?.Current?.GetNumberFormatString(RowCells[i].NumberFormatIndex)?.FormatString;
	}

	public int GetNumberFormatIndex(int i)
	{
		if (RowCells == null)
		{
			throw new InvalidOperationException("No data exists for the row/column.");
		}
		if (RowCells[i] == null)
		{
			return -1;
		}
		return RowCells[i].NumberFormatIndex;
	}

	public double GetColumnWidth(int i)
	{
		if (i >= FieldCount)
		{
			throw new ArgumentException($"Column at index {i} does not exist.", "i");
		}
		Col[] array = _worksheetIterator?.Current?.ColumnWidths ?? null;
		double? num = null;
		if (array != null)
		{
			int num2 = 0;
			while (num2 < array.Length && !num.HasValue)
			{
				Col col = array[num2];
				if (i >= col.Min && i <= col.Max)
				{
					num = (col.Hidden ? 0.0 : col.Width);
				}
				else
				{
					num2++;
				}
			}
		}
		return num ?? 8.43;
	}

	public void Reset()
	{
		_worksheetIterator?.Dispose();
		_rowIterator?.Dispose();
		_worksheetIterator = null;
		_rowIterator = null;
		ResetSheetData();
		if (Workbook != null)
		{
			_worksheetIterator = ReadWorksheetsWithCache().GetEnumerator();
			if (!_worksheetIterator.MoveNext())
			{
				_worksheetIterator.Dispose();
				_worksheetIterator = null;
			}
			else
			{
				_rowIterator = _worksheetIterator.Current.ReadRows().GetEnumerator();
			}
		}
	}

	public virtual void Close()
	{
		if (!IsClosed)
		{
			_worksheetIterator?.Dispose();
			_rowIterator?.Dispose();
			_worksheetIterator = null;
			_rowIterator = null;
			RowCells = null;
			IsClosed = true;
		}
	}

	public bool NextResult()
	{
		if (_worksheetIterator == null)
		{
			return false;
		}
		ResetSheetData();
		_rowIterator?.Dispose();
		_rowIterator = null;
		if (!_worksheetIterator.MoveNext())
		{
			_worksheetIterator.Dispose();
			_worksheetIterator = null;
			return false;
		}
		_rowIterator = _worksheetIterator.Current.ReadRows().GetEnumerator();
		return true;
	}

	public bool Read()
	{
		if (_worksheetIterator == null || _rowIterator == null)
		{
			return false;
		}
		if (!_rowIterator.MoveNext())
		{
			_rowIterator.Dispose();
			_rowIterator = null;
			return false;
		}
		ReadCurrentRow();
		Depth++;
		return true;
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		if (disposing)
		{
			Close();
		}
	}

	private IEnumerable<TWorksheet> ReadWorksheetsWithCache()
	{
		if (_cachedWorksheets != null)
		{
			foreach (TWorksheet cachedWorksheet in _cachedWorksheets)
			{
				yield return cachedWorksheet;
			}
			if (_cachedWorksheetIterator == null)
			{
				yield break;
			}
		}
		else
		{
			_cachedWorksheets = new List<TWorksheet>();
		}
		if (_cachedWorksheetIterator == null)
		{
			_cachedWorksheetIterator = Workbook.ReadWorksheets().GetEnumerator();
		}
		while (_cachedWorksheetIterator.MoveNext())
		{
			_cachedWorksheets.Add(_cachedWorksheetIterator.Current);
			yield return _cachedWorksheetIterator.Current;
		}
		_cachedWorksheetIterator.Dispose();
		_cachedWorksheetIterator = null;
	}

	private void ResetSheetData()
	{
		Depth = -1;
		RowCells = null;
	}

	private void ReadCurrentRow()
	{
		if (RowCells == null)
		{
			RowCells = new Cell[FieldCount];
		}
		Array.Clear(RowCells, 0, RowCells.Length);
		foreach (Cell cell in _rowIterator.Current.Cells)
		{
			if (cell.ColumnIndex < RowCells.Length)
			{
				RowCells[cell.ColumnIndex] = cell;
			}
		}
	}
}
