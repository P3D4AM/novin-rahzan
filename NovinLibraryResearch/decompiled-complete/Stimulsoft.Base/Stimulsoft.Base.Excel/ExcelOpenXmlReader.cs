using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Xml;

namespace Stimulsoft.Base.Excel;

public class ExcelOpenXmlReader : IExcelDataReader, IDataReader, IDisposable, IDataRecord
{
	private XlsxWorkbook _workbook;

	private bool _isValid;

	private bool _isClosed;

	private bool _isFirstRead;

	private string _exceptionMessage;

	private int _depth;

	private int _resultIndex;

	private int _emptyRowCount;

	private ZipWorker _zipWorker;

	private XmlReader _xmlReader;

	private Stream _sheetStream;

	private object[] _cellsValues;

	private object[] _savedCellsValues;

	private bool disposed;

	private bool _isFirstRowAsColumnNames;

	private const string COLUMN = "Column";

	private string instanceId = Guid.NewGuid().ToString();

	private CultureInfo enCulture = CultureInfo.GetCultureInfo("en-US");

	private List<int> _defaultDateTimeStyles;

	private List<int> _defaultTimeStyles;

	public bool IsFirstRowAsColumnNames
	{
		get
		{
			return _isFirstRowAsColumnNames;
		}
		set
		{
			_isFirstRowAsColumnNames = value;
		}
	}

	public bool IsValid => _isValid;

	public string ExceptionMessage => _exceptionMessage;

	public string Name
	{
		get
		{
			if (_resultIndex < 0 || _resultIndex >= ResultsCount)
			{
				return null;
			}
			return _workbook.Sheets[_resultIndex].Name;
		}
	}

	public int Depth => _depth;

	public int ResultsCount
	{
		get
		{
			if (_workbook != null)
			{
				return _workbook.Sheets.Count;
			}
			return -1;
		}
	}

	public bool IsClosed => _isClosed;

	public int FieldCount
	{
		get
		{
			if (_resultIndex < 0 || _resultIndex >= ResultsCount)
			{
				return -1;
			}
			return _workbook.Sheets[_resultIndex].ColumnsCount;
		}
	}

	public object this[int i] => _cellsValues[i];

	public int RecordsAffected
	{
		get
		{
			throw new NotSupportedException();
		}
	}

	public object this[string name]
	{
		get
		{
			throw new NotSupportedException();
		}
	}

	internal ExcelOpenXmlReader()
	{
		_isValid = true;
		_isFirstRead = true;
		_defaultDateTimeStyles = new List<int>(new int[12]
		{
			14, 15, 16, 17, 18, 19, 20, 21, 22, 45,
			46, 47
		});
		_defaultTimeStyles = new List<int>(new int[7] { 18, 19, 20, 21, 45, 46, 47 });
	}

	private void ReadGlobals()
	{
		_workbook = new XlsxWorkbook(_zipWorker.GetWorkbookStream(), _zipWorker.GetWorkbookRelsStream(), _zipWorker.GetSharedStringsStream(), _zipWorker.GetStylesStream());
		CheckDateTimeNumFmts(_workbook.Styles.NumFmts);
	}

	private void CheckDateTimeNumFmts(List<XlsxNumFmt> list)
	{
		if (list.Count == 0)
		{
			return;
		}
		foreach (XlsxNumFmt item in list)
		{
			if (string.IsNullOrEmpty(item.FormatCode))
			{
				continue;
			}
			string text = item.FormatCode.ToLower();
			int num;
			while ((num = text.IndexOf('"')) > 0)
			{
				int num2 = text.IndexOf('"', num + 1);
				if (num2 > 0)
				{
					text = text.Remove(num, num2 - num + 1);
				}
			}
			FormatReader formatReader = new FormatReader
			{
				FormatString = text
			};
			if (formatReader.IsDateFormatString())
			{
				_defaultDateTimeStyles.Add(item.Id);
				if (!text.Contains("y") && !text.Contains("mmm"))
				{
					_defaultTimeStyles.Add(item.Id);
				}
			}
		}
	}

	private void ReadSheetGlobals(XlsxWorksheet sheet)
	{
		if (_xmlReader != null)
		{
			_xmlReader.Close();
		}
		if (_sheetStream != null)
		{
			_sheetStream.Close();
		}
		_sheetStream = _zipWorker.GetWorksheetStream(sheet.Path);
		if (_sheetStream == null)
		{
			return;
		}
		_xmlReader = XmlReader.Create(_sheetStream);
		while (_xmlReader.Read())
		{
			if (_xmlReader.NodeType == XmlNodeType.Element && _xmlReader.Name == "dimension")
			{
				string attribute = _xmlReader.GetAttribute("ref");
				sheet.Dimension = new XlsxDimension(attribute);
				break;
			}
		}
		_xmlReader.ReadToFollowing("sheetData");
		if (_xmlReader.IsEmptyElement)
		{
			sheet.IsEmpty = true;
		}
	}

	private bool ReadSheetRow(XlsxWorksheet sheet)
	{
		if (_xmlReader == null)
		{
			return false;
		}
		if (_emptyRowCount != 0)
		{
			_cellsValues = new object[sheet.ColumnsCount];
			_emptyRowCount--;
			_depth++;
			return true;
		}
		if (_savedCellsValues != null)
		{
			_cellsValues = _savedCellsValues;
			_savedCellsValues = null;
			_depth++;
			return true;
		}
		if ((_xmlReader.NodeType == XmlNodeType.Element && _xmlReader.Name == "row") || _xmlReader.ReadToFollowing("row"))
		{
			_cellsValues = new object[sheet.ColumnsCount];
			string attribute = _xmlReader.GetAttribute("r");
			if (!string.IsNullOrWhiteSpace(attribute))
			{
				int num = int.Parse(attribute);
				if (num != _depth + 1)
				{
					_emptyRowCount = num - _depth - 1;
				}
			}
			bool flag = false;
			string text = string.Empty;
			string text2 = string.Empty;
			string empty = string.Empty;
			int val = 0;
			int val2 = _depth + 1;
			while (_xmlReader.Read() && _xmlReader.Depth != 2)
			{
				if (_xmlReader.NodeType == XmlNodeType.Element)
				{
					flag = false;
					if (_xmlReader.Name == "c")
					{
						text = _xmlReader.GetAttribute("s");
						text2 = _xmlReader.GetAttribute("t");
						empty = _xmlReader.GetAttribute("r");
						if (!string.IsNullOrWhiteSpace(empty))
						{
							XlsxDimension.XlsxDim(empty, out val, out val2);
						}
						else
						{
							val++;
						}
					}
					else if (_xmlReader.Name == "v")
					{
						flag = true;
					}
				}
				if (!(_xmlReader.NodeType == XmlNodeType.Text && flag))
				{
					continue;
				}
				object obj = _xmlReader.Value;
				if (double.TryParse(obj.ToString(), NumberStyles.Number | NumberStyles.AllowExponent, enCulture, out var result))
				{
					obj = result;
				}
				if (text2 != null && text2 == "s")
				{
					obj = Helpers.ConvertEscapeChars(_workbook.SST[int.Parse(obj.ToString())]);
				}
				else if (text2 == "b")
				{
					obj = _xmlReader.Value == "1";
				}
				else if (text != null)
				{
					XlsxXf xlsxXf = _workbook.Styles.CellXfs[int.Parse(text)];
					if (xlsxXf.ApplyNumberFormat && obj != null && obj.ToString() != string.Empty && IsDateTimeStyle(xlsxXf.NumFmtId))
					{
						obj = (((!(result >= 0.0) || !(result < 1.0)) && (!IsTimeStyle(xlsxXf.NumFmtId) || !(result >= 0.0) || !(result < 1000.0))) ? Helpers.ConvertFromOATime(result) : ((object)TimeSpan.FromDays(result)));
					}
					else if (xlsxXf.NumFmtId == 49)
					{
						obj = obj.ToString();
					}
				}
				if (val - 1 < _cellsValues.Length)
				{
					_cellsValues[val - 1] = obj;
				}
			}
			if (_emptyRowCount > 0)
			{
				_savedCellsValues = _cellsValues;
				return ReadSheetRow(sheet);
			}
			_depth++;
			return true;
		}
		_xmlReader.Close();
		if (_sheetStream != null)
		{
			_sheetStream.Close();
		}
		return false;
	}

	private bool InitializeSheetRead()
	{
		if (ResultsCount <= 0)
		{
			return false;
		}
		ReadSheetGlobals(_workbook.Sheets[_resultIndex]);
		if (_workbook.Sheets[_resultIndex].Dimension == null)
		{
			return false;
		}
		_isFirstRead = false;
		_depth = 0;
		_emptyRowCount = 0;
		return true;
	}

	private bool IsDateTimeStyle(int styleId)
	{
		return _defaultDateTimeStyles.Contains(styleId);
	}

	private bool IsTimeStyle(int styleId)
	{
		return _defaultTimeStyles.Contains(styleId);
	}

	public void Initialize(Stream fileStream)
	{
		_zipWorker = new ZipWorker();
		_zipWorker.Extract(fileStream);
		if (!_zipWorker.IsValid)
		{
			_isValid = false;
			_exceptionMessage = _zipWorker.ExceptionMessage;
			Close();
		}
		else
		{
			ReadGlobals();
		}
	}

	public DataSet AsDataSet()
	{
		return AsDataSet(convertOADateTime: true);
	}

	public DataSet AsDataSet(bool convertOADateTime)
	{
		if (!_isValid)
		{
			return null;
		}
		DataSet dataSet = new DataSet();
		for (int i = 0; i < _workbook.Sheets.Count; i++)
		{
			DataTable dataTable = new DataTable(_workbook.Sheets[i].Name);
			ReadSheetGlobals(_workbook.Sheets[i]);
			if (_workbook.Sheets[i].Dimension == null)
			{
				continue;
			}
			_depth = 0;
			_emptyRowCount = 0;
			if (!_isFirstRowAsColumnNames)
			{
				for (int j = 0; j < _workbook.Sheets[i].ColumnsCount; j++)
				{
					dataTable.Columns.Add(null, typeof(object));
				}
			}
			else
			{
				if (!ReadSheetRow(_workbook.Sheets[i]))
				{
					continue;
				}
				for (int k = 0; k < _cellsValues.Length; k++)
				{
					if (_cellsValues[k] != null && _cellsValues[k].ToString().Length > 0)
					{
						Helpers.AddColumnHandleDuplicate(dataTable, _cellsValues[k].ToString());
					}
					else
					{
						Helpers.AddColumnHandleDuplicate(dataTable, "Column" + k);
					}
				}
			}
			dataTable.BeginLoadData();
			while (ReadSheetRow(_workbook.Sheets[i]))
			{
				dataTable.Rows.Add(_cellsValues);
			}
			if (dataTable.Rows.Count > 0)
			{
				dataSet.Tables.Add(dataTable);
			}
			dataTable.EndLoadData();
		}
		dataSet.AcceptChanges();
		Helpers.FixDataTypes(dataSet);
		return dataSet;
	}

	public void Close()
	{
		_isClosed = true;
		if (_xmlReader != null)
		{
			_xmlReader.Close();
		}
		if (_sheetStream != null)
		{
			_sheetStream.Close();
		}
		if (_zipWorker != null)
		{
			_zipWorker.Dispose();
		}
	}

	public bool NextResult()
	{
		if (_resultIndex >= ResultsCount - 1)
		{
			return false;
		}
		_resultIndex++;
		_isFirstRead = true;
		return true;
	}

	public bool Read()
	{
		if (!_isValid)
		{
			return false;
		}
		if (_isFirstRead && !InitializeSheetRead())
		{
			return false;
		}
		return ReadSheetRow(_workbook.Sheets[_resultIndex]);
	}

	public bool GetBoolean(int i)
	{
		if (IsDBNull(i))
		{
			return false;
		}
		return bool.Parse(_cellsValues[i].ToString());
	}

	public DateTime GetDateTime(int i)
	{
		if (IsDBNull(i))
		{
			return DateTime.MinValue;
		}
		try
		{
			return (DateTime)_cellsValues[i];
		}
		catch (InvalidCastException)
		{
			return DateTime.MinValue;
		}
	}

	public decimal GetDecimal(int i)
	{
		if (IsDBNull(i))
		{
			return decimal.MinValue;
		}
		return decimal.Parse(_cellsValues[i].ToString());
	}

	public double GetDouble(int i)
	{
		if (IsDBNull(i))
		{
			return double.MinValue;
		}
		return double.Parse(_cellsValues[i].ToString());
	}

	public float GetFloat(int i)
	{
		if (IsDBNull(i))
		{
			return float.MinValue;
		}
		return float.Parse(_cellsValues[i].ToString());
	}

	public short GetInt16(int i)
	{
		if (IsDBNull(i))
		{
			return short.MinValue;
		}
		return short.Parse(_cellsValues[i].ToString());
	}

	public int GetInt32(int i)
	{
		if (IsDBNull(i))
		{
			return int.MinValue;
		}
		return int.Parse(_cellsValues[i].ToString());
	}

	public long GetInt64(int i)
	{
		if (IsDBNull(i))
		{
			return long.MinValue;
		}
		return long.Parse(_cellsValues[i].ToString());
	}

	public string GetString(int i)
	{
		if (IsDBNull(i))
		{
			return null;
		}
		return _cellsValues[i].ToString();
	}

	public object GetValue(int i)
	{
		return _cellsValues[i];
	}

	public bool IsDBNull(int i)
	{
		if (_cellsValues[i] != null)
		{
			return DBNull.Value == _cellsValues[i];
		}
		return true;
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		if (disposed)
		{
			return;
		}
		if (disposing)
		{
			if (_xmlReader != null)
			{
				((IDisposable)_xmlReader).Dispose();
			}
			if (_sheetStream != null)
			{
				_sheetStream.Dispose();
			}
			if (_zipWorker != null)
			{
				_zipWorker.Dispose();
			}
		}
		_zipWorker = null;
		_xmlReader = null;
		_sheetStream = null;
		_workbook = null;
		_cellsValues = null;
		_savedCellsValues = null;
		disposed = true;
	}

	~ExcelOpenXmlReader()
	{
		Dispose(disposing: false);
	}

	public DataTable GetSchemaTable()
	{
		throw new NotSupportedException();
	}

	public byte GetByte(int i)
	{
		throw new NotSupportedException();
	}

	public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
	{
		throw new NotSupportedException();
	}

	public char GetChar(int i)
	{
		throw new NotSupportedException();
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

	public Type GetFieldType(int i)
	{
		throw new NotSupportedException();
	}

	public Guid GetGuid(int i)
	{
		throw new NotSupportedException();
	}

	public string GetName(int i)
	{
		throw new NotSupportedException();
	}

	public int GetOrdinal(string name)
	{
		throw new NotSupportedException();
	}

	public int GetValues(object[] values)
	{
		throw new NotSupportedException();
	}
}
