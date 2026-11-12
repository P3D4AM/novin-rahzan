using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Query.Internal;

public class BufferedDataReader : DbDataReader
{
	private class BufferedDataRecord
	{
		private enum TypeCase
		{
			Empty,
			Object,
			Bool,
			Byte,
			Char,
			DateTime,
			DateTimeOffset,
			Decimal,
			Double,
			Float,
			Guid,
			SByte,
			Short,
			Int,
			Long,
			UInt,
			ULong,
			UShort
		}

		private int _currentRowNumber = -1;

		private int _rowCount;

		private string[] _dataTypeNames;

		private Type[] _fieldTypes;

		private string[] _columnNames;

		private Lazy<Dictionary<string, int>> _fieldNameLookup;

		private int _rowCapacity = 1;

		private BitArray _bools;

		private bool[] _tempBools;

		private int _boolCount;

		private byte[] _bytes;

		private int _byteCount;

		private char[] _chars;

		private int _charCount;

		private DateTime[] _dateTimes;

		private int _dateTimeCount;

		private DateTimeOffset[] _dateTimeOffsets;

		private int _dateTimeOffsetCount;

		private decimal[] _decimals;

		private int _decimalCount;

		private double[] _doubles;

		private int _doubleCount;

		private float[] _floats;

		private int _floatCount;

		private Guid[] _guids;

		private int _guidCount;

		private short[] _shorts;

		private int _shortCount;

		private int[] _ints;

		private int _intCount;

		private long[] _longs;

		private int _longCount;

		private sbyte[] _sbytes;

		private int _sbyteCount;

		private uint[] _uints;

		private int _uintCount;

		private ushort[] _ushorts;

		private int _ushortCount;

		private ulong[] _ulongs;

		private int _ulongCount;

		private object[] _objects;

		private int _objectCount;

		private int[] _ordinalToIndexMap;

		private BitArray _nulls;

		private bool[] _tempNulls;

		private int _nullCount;

		private int[] _nullOrdinalToIndexMap;

		private TypeCase[] _columnTypeCases;

		private DbDataReader _underlyingReader;

		private IReadOnlyList<ReaderColumn> _columns;

		private int[] _indexMap;

		public bool IsDataReady { get; private set; }

		public bool HasRows => _rowCount > 0;

		public int FieldCount => _fieldTypes.Length;

		public string GetDataTypeName(int ordinal)
		{
			return _dataTypeNames[ordinal];
		}

		public Type GetFieldType(int ordinal)
		{
			return _fieldTypes[ordinal];
		}

		public string GetName(int ordinal)
		{
			return _columnNames[ordinal];
		}

		public int GetOrdinal(string name)
		{
			return _fieldNameLookup.Value[name];
		}

		public bool GetBoolean(int ordinal)
		{
			if (_columnTypeCases[ordinal] != TypeCase.Bool)
			{
				return GetFieldValue<bool>(ordinal);
			}
			return _bools[_currentRowNumber * _boolCount + _ordinalToIndexMap[ordinal]];
		}

		public byte GetByte(int ordinal)
		{
			if (_columnTypeCases[ordinal] != TypeCase.Byte)
			{
				return GetFieldValue<byte>(ordinal);
			}
			return _bytes[_currentRowNumber * _byteCount + _ordinalToIndexMap[ordinal]];
		}

		public char GetChar(int ordinal)
		{
			if (_columnTypeCases[ordinal] != TypeCase.Char)
			{
				return GetFieldValue<char>(ordinal);
			}
			return _chars[_currentRowNumber * _charCount + _ordinalToIndexMap[ordinal]];
		}

		public DateTime GetDateTime(int ordinal)
		{
			if (_columnTypeCases[ordinal] != TypeCase.DateTime)
			{
				return GetFieldValue<DateTime>(ordinal);
			}
			return _dateTimes[_currentRowNumber * _dateTimeCount + _ordinalToIndexMap[ordinal]];
		}

		public DateTimeOffset GetDateTimeOffset(int ordinal)
		{
			if (_columnTypeCases[ordinal] != TypeCase.DateTimeOffset)
			{
				return GetFieldValue<DateTimeOffset>(ordinal);
			}
			return _dateTimeOffsets[_currentRowNumber * _dateTimeOffsetCount + _ordinalToIndexMap[ordinal]];
		}

		public decimal GetDecimal(int ordinal)
		{
			if (_columnTypeCases[ordinal] != TypeCase.Decimal)
			{
				return GetFieldValue<decimal>(ordinal);
			}
			return _decimals[_currentRowNumber * _decimalCount + _ordinalToIndexMap[ordinal]];
		}

		public double GetDouble(int ordinal)
		{
			if (_columnTypeCases[ordinal] != TypeCase.Double)
			{
				return GetFieldValue<double>(ordinal);
			}
			return _doubles[_currentRowNumber * _doubleCount + _ordinalToIndexMap[ordinal]];
		}

		public float GetFloat(int ordinal)
		{
			if (_columnTypeCases[ordinal] != TypeCase.Float)
			{
				return GetFieldValue<float>(ordinal);
			}
			return _floats[_currentRowNumber * _floatCount + _ordinalToIndexMap[ordinal]];
		}

		public Guid GetGuid(int ordinal)
		{
			if (_columnTypeCases[ordinal] != TypeCase.Guid)
			{
				return GetFieldValue<Guid>(ordinal);
			}
			return _guids[_currentRowNumber * _guidCount + _ordinalToIndexMap[ordinal]];
		}

		public short GetInt16(int ordinal)
		{
			if (_columnTypeCases[ordinal] != TypeCase.Short)
			{
				return GetFieldValue<short>(ordinal);
			}
			return _shorts[_currentRowNumber * _shortCount + _ordinalToIndexMap[ordinal]];
		}

		public int GetInt32(int ordinal)
		{
			if (_columnTypeCases[ordinal] != TypeCase.Int)
			{
				return GetFieldValue<int>(ordinal);
			}
			return _ints[_currentRowNumber * _intCount + _ordinalToIndexMap[ordinal]];
		}

		public long GetInt64(int ordinal)
		{
			if (_columnTypeCases[ordinal] != TypeCase.Long)
			{
				return GetFieldValue<long>(ordinal);
			}
			return _longs[_currentRowNumber * _longCount + _ordinalToIndexMap[ordinal]];
		}

		public sbyte GetSByte(int ordinal)
		{
			if (_columnTypeCases[ordinal] != TypeCase.SByte)
			{
				return GetFieldValue<sbyte>(ordinal);
			}
			return _sbytes[_currentRowNumber * _sbyteCount + _ordinalToIndexMap[ordinal]];
		}

		public ushort GetUInt16(int ordinal)
		{
			if (_columnTypeCases[ordinal] != TypeCase.UShort)
			{
				return GetFieldValue<ushort>(ordinal);
			}
			return _ushorts[_currentRowNumber * _ushortCount + _ordinalToIndexMap[ordinal]];
		}

		public uint GetUInt32(int ordinal)
		{
			if (_columnTypeCases[ordinal] != TypeCase.UInt)
			{
				return GetFieldValue<uint>(ordinal);
			}
			return _uints[_currentRowNumber * _uintCount + _ordinalToIndexMap[ordinal]];
		}

		public ulong GetUInt64(int ordinal)
		{
			if (_columnTypeCases[ordinal] != TypeCase.ULong)
			{
				return GetFieldValue<ulong>(ordinal);
			}
			return _ulongs[_currentRowNumber * _ulongCount + _ordinalToIndexMap[ordinal]];
		}

		public object GetValue(int ordinal)
		{
			return GetFieldValue<object>(ordinal);
		}

		public int GetValues(object[] values)
		{
			throw new NotSupportedException();
		}

		public T GetFieldValue<T>(int ordinal)
		{
			return _columnTypeCases[ordinal] switch
			{
				TypeCase.Bool => (T)(object)GetBoolean(ordinal), 
				TypeCase.Byte => (T)(object)GetByte(ordinal), 
				TypeCase.Char => (T)(object)GetChar(ordinal), 
				TypeCase.DateTime => (T)(object)GetDateTime(ordinal), 
				TypeCase.DateTimeOffset => (T)(object)GetDateTimeOffset(ordinal), 
				TypeCase.Decimal => (T)(object)GetDecimal(ordinal), 
				TypeCase.Double => (T)(object)GetDouble(ordinal), 
				TypeCase.Float => (T)(object)GetFloat(ordinal), 
				TypeCase.Guid => (T)(object)GetGuid(ordinal), 
				TypeCase.Short => (T)(object)GetInt16(ordinal), 
				TypeCase.Int => (T)(object)GetInt32(ordinal), 
				TypeCase.Long => (T)(object)GetInt64(ordinal), 
				TypeCase.SByte => (T)(object)GetSByte(ordinal), 
				TypeCase.UShort => (T)(object)GetUInt16(ordinal), 
				TypeCase.UInt => (T)(object)GetUInt32(ordinal), 
				TypeCase.ULong => (T)(object)GetUInt64(ordinal), 
				TypeCase.Empty => default(T), 
				_ => (T)_objects[_currentRowNumber * _objectCount + _ordinalToIndexMap[ordinal]], 
			};
		}

		public Task<T> GetFieldValueAsync<T>(int ordinal, CancellationToken cancellationToken)
		{
			return Task.FromResult(GetFieldValue<T>(ordinal));
		}

		public bool IsDBNull(int ordinal)
		{
			return _nulls[_currentRowNumber * _nullCount + _nullOrdinalToIndexMap[ordinal]];
		}

		public Task<bool> IsDBNullAsync(int ordinal, CancellationToken cancellationToken)
		{
			return Task.FromResult(IsDBNull(ordinal));
		}

		public bool Read()
		{
			return IsDataReady = ++_currentRowNumber < _rowCount;
		}

		public Task<bool> ReadAsync(CancellationToken cancellationToken)
		{
			return Task.FromResult(Read());
		}

		public BufferedDataRecord Initialize([JetBrains.Annotations.NotNull] DbDataReader reader, [JetBrains.Annotations.NotNull] IReadOnlyList<ReaderColumn> columns)
		{
			_underlyingReader = reader;
			_columns = columns;
			ReadMetadata();
			InitializeFields();
			while (reader.Read())
			{
				ReadRow();
			}
			_bools = new BitArray(_tempBools);
			_tempBools = null;
			_nulls = new BitArray(_tempNulls);
			_tempNulls = null;
			_rowCount = _currentRowNumber + 1;
			_currentRowNumber = -1;
			_underlyingReader = null;
			_columns = null;
			return this;
		}

		public async Task<BufferedDataRecord> InitializeAsync([JetBrains.Annotations.NotNull] DbDataReader reader, [JetBrains.Annotations.NotNull] IReadOnlyList<ReaderColumn> columns, CancellationToken cancellationToken)
		{
			_underlyingReader = reader;
			_columns = columns;
			ReadMetadata();
			InitializeFields();
			while (await reader.ReadAsync(cancellationToken))
			{
				ReadRow();
			}
			_bools = new BitArray(_tempBools);
			_tempBools = null;
			_nulls = new BitArray(_tempNulls);
			_tempNulls = null;
			_rowCount = _currentRowNumber + 1;
			_currentRowNumber = -1;
			_underlyingReader = null;
			_columns = null;
			return this;
		}

		private void ReadRow()
		{
			_currentRowNumber++;
			if (_rowCapacity == _currentRowNumber)
			{
				DoubleBufferCapacity();
			}
			for (int i = 0; i < FieldCount; i++)
			{
				ReaderColumn column = _columns[i];
				int num = _nullOrdinalToIndexMap[i];
				switch (_columnTypeCases[i])
				{
				case TypeCase.Bool:
					if (num != -1)
					{
						bool flag;
						_tempNulls[_currentRowNumber * _nullCount + num] = (flag = _underlyingReader.IsDBNull(i));
						if (!flag)
						{
							ReadBool(_underlyingReader, i, column);
						}
					}
					else
					{
						ReadBool(_underlyingReader, i, column);
					}
					continue;
				case TypeCase.Byte:
					if (num != -1)
					{
						bool flag;
						_tempNulls[_currentRowNumber * _nullCount + num] = (flag = _underlyingReader.IsDBNull(i));
						if (!flag)
						{
							ReadByte(_underlyingReader, i, column);
						}
					}
					else
					{
						ReadByte(_underlyingReader, i, column);
					}
					continue;
				case TypeCase.Char:
					if (num != -1)
					{
						bool flag;
						_tempNulls[_currentRowNumber * _nullCount + num] = (flag = _underlyingReader.IsDBNull(i));
						if (!flag)
						{
							ReadChar(_underlyingReader, i, column);
						}
					}
					else
					{
						ReadChar(_underlyingReader, i, column);
					}
					continue;
				case TypeCase.DateTime:
					if (num != -1)
					{
						bool flag;
						_tempNulls[_currentRowNumber * _nullCount + num] = (flag = _underlyingReader.IsDBNull(i));
						if (!flag)
						{
							ReadDateTime(_underlyingReader, i, column);
						}
					}
					else
					{
						ReadDateTime(_underlyingReader, i, column);
					}
					continue;
				case TypeCase.DateTimeOffset:
					if (num != -1)
					{
						bool flag;
						_tempNulls[_currentRowNumber * _nullCount + num] = (flag = _underlyingReader.IsDBNull(i));
						if (!flag)
						{
							ReadDateTimeOffset(_underlyingReader, i, column);
						}
					}
					else
					{
						ReadDateTimeOffset(_underlyingReader, i, column);
					}
					continue;
				case TypeCase.Decimal:
					if (num != -1)
					{
						bool flag;
						_tempNulls[_currentRowNumber * _nullCount + num] = (flag = _underlyingReader.IsDBNull(i));
						if (!flag)
						{
							ReadDecimal(_underlyingReader, i, column);
						}
					}
					else
					{
						ReadDecimal(_underlyingReader, i, column);
					}
					continue;
				case TypeCase.Double:
					if (num != -1)
					{
						bool flag;
						_tempNulls[_currentRowNumber * _nullCount + num] = (flag = _underlyingReader.IsDBNull(i));
						if (!flag)
						{
							ReadDouble(_underlyingReader, i, column);
						}
					}
					else
					{
						ReadDouble(_underlyingReader, i, column);
					}
					continue;
				case TypeCase.Float:
					if (num != -1)
					{
						bool flag;
						_tempNulls[_currentRowNumber * _nullCount + num] = (flag = _underlyingReader.IsDBNull(i));
						if (!flag)
						{
							ReadFloat(_underlyingReader, i, column);
						}
					}
					else
					{
						ReadFloat(_underlyingReader, i, column);
					}
					continue;
				case TypeCase.Guid:
					if (num != -1)
					{
						bool flag;
						_tempNulls[_currentRowNumber * _nullCount + num] = (flag = _underlyingReader.IsDBNull(i));
						if (!flag)
						{
							ReadGuid(_underlyingReader, i, column);
						}
					}
					else
					{
						ReadGuid(_underlyingReader, i, column);
					}
					continue;
				case TypeCase.Short:
					if (num != -1)
					{
						bool flag;
						_tempNulls[_currentRowNumber * _nullCount + num] = (flag = _underlyingReader.IsDBNull(i));
						if (!flag)
						{
							ReadShort(_underlyingReader, i, column);
						}
					}
					else
					{
						ReadShort(_underlyingReader, i, column);
					}
					continue;
				case TypeCase.Int:
					if (num != -1)
					{
						bool flag;
						_tempNulls[_currentRowNumber * _nullCount + num] = (flag = _underlyingReader.IsDBNull(i));
						if (!flag)
						{
							ReadInt(_underlyingReader, i, column);
						}
					}
					else
					{
						ReadInt(_underlyingReader, i, column);
					}
					continue;
				case TypeCase.Long:
					if (num != -1)
					{
						bool flag;
						_tempNulls[_currentRowNumber * _nullCount + num] = (flag = _underlyingReader.IsDBNull(i));
						if (!flag)
						{
							ReadLong(_underlyingReader, i, column);
						}
					}
					else
					{
						ReadLong(_underlyingReader, i, column);
					}
					continue;
				case TypeCase.SByte:
					if (num != -1)
					{
						bool flag;
						_tempNulls[_currentRowNumber * _nullCount + num] = (flag = _underlyingReader.IsDBNull(i));
						if (!flag)
						{
							ReadSByte(_underlyingReader, i, column);
						}
					}
					else
					{
						ReadSByte(_underlyingReader, i, column);
					}
					continue;
				case TypeCase.UShort:
					if (num != -1)
					{
						bool flag;
						_tempNulls[_currentRowNumber * _nullCount + num] = (flag = _underlyingReader.IsDBNull(i));
						if (!flag)
						{
							ReadUShort(_underlyingReader, i, column);
						}
					}
					else
					{
						ReadUShort(_underlyingReader, i, column);
					}
					continue;
				case TypeCase.UInt:
					if (num != -1)
					{
						bool flag;
						_tempNulls[_currentRowNumber * _nullCount + num] = (flag = _underlyingReader.IsDBNull(i));
						if (!flag)
						{
							ReadUInt(_underlyingReader, i, column);
						}
					}
					else
					{
						ReadUInt(_underlyingReader, i, column);
					}
					continue;
				case TypeCase.ULong:
					if (num != -1)
					{
						bool flag;
						_tempNulls[_currentRowNumber * _nullCount + num] = (flag = _underlyingReader.IsDBNull(i));
						if (!flag)
						{
							ReadULong(_underlyingReader, i, column);
						}
					}
					else
					{
						ReadULong(_underlyingReader, i, column);
					}
					continue;
				case TypeCase.Empty:
					continue;
				}
				if (num != -1)
				{
					bool flag;
					_tempNulls[_currentRowNumber * _nullCount + num] = (flag = _underlyingReader.IsDBNull(i));
					if (!flag)
					{
						ReadObject(_underlyingReader, i, column);
					}
				}
				else
				{
					ReadObject(_underlyingReader, i, column);
				}
			}
		}

		private void ReadMetadata()
		{
			int fieldCount = _underlyingReader.FieldCount;
			string[] array = new string[fieldCount];
			Type[] array2 = new Type[fieldCount];
			string[] array3 = new string[fieldCount];
			for (int i = 0; i < fieldCount; i++)
			{
				array[i] = _underlyingReader.GetDataTypeName(i);
				array2[i] = _underlyingReader.GetFieldType(i);
				array3[i] = _underlyingReader.GetName(i);
			}
			_dataTypeNames = array;
			_fieldTypes = array2;
			_columnNames = array3;
			_fieldNameLookup = new Lazy<Dictionary<string, int>>(CreateNameLookup, isThreadSafe: false);
			Dictionary<string, int> CreateNameLookup()
			{
				Dictionary<string, int> dictionary = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
				for (int j = 0; j < _columnNames.Length; j++)
				{
					dictionary[_columnNames[j]] = j;
				}
				return dictionary;
			}
		}

		private void InitializeFields()
		{
			int fieldCount = FieldCount;
			if (FieldCount < _columns.Count)
			{
				throw new InvalidOperationException("The underlying reader doesn't have as many fields as expected.");
			}
			_columnTypeCases = Enumerable.Repeat(TypeCase.Empty, fieldCount).ToArray();
			_ordinalToIndexMap = Enumerable.Repeat(-1, fieldCount).ToArray();
			if (_columns.Count > 0 && _columns[0].Name != null)
			{
				Dictionary<string, int> value = _fieldNameLookup.Value;
				_indexMap = new int[_columns.Count];
				ReaderColumn[] array = new ReaderColumn[fieldCount];
				for (int i = 0; i < _columns.Count; i++)
				{
					ReaderColumn readerColumn = _columns[i];
					if (!value.TryGetValue(readerColumn.Name, out var value2))
					{
						throw new InvalidOperationException(RelationalStrings.FromSqlMissingColumn(readerColumn.Name));
					}
					array[value2] = readerColumn;
					_indexMap[i] = value2;
				}
				_columns = array;
			}
			if (FieldCount != _columns.Count)
			{
				ReaderColumn[] array2 = new ReaderColumn[fieldCount];
				for (int j = 0; j < _columns.Count; j++)
				{
					array2[j] = _columns[j];
				}
				_columns = array2;
			}
			for (int k = 0; k < fieldCount; k++)
			{
				ReaderColumn readerColumn2 = _columns[k];
				if (readerColumn2 != null)
				{
					Type type = readerColumn2.Type;
					if (type == typeof(bool))
					{
						_columnTypeCases[k] = TypeCase.Bool;
						_ordinalToIndexMap[k] = _boolCount;
						_boolCount++;
					}
					else if (type == typeof(byte))
					{
						_columnTypeCases[k] = TypeCase.Byte;
						_ordinalToIndexMap[k] = _byteCount;
						_byteCount++;
					}
					else if (type == typeof(char))
					{
						_columnTypeCases[k] = TypeCase.Char;
						_ordinalToIndexMap[k] = _charCount;
						_charCount++;
					}
					else if (type == typeof(DateTime))
					{
						_columnTypeCases[k] = TypeCase.DateTime;
						_ordinalToIndexMap[k] = _dateTimeCount;
						_dateTimeCount++;
					}
					else if (type == typeof(DateTimeOffset))
					{
						_columnTypeCases[k] = TypeCase.DateTimeOffset;
						_ordinalToIndexMap[k] = _dateTimeOffsetCount;
						_dateTimeOffsetCount++;
					}
					else if (type == typeof(decimal))
					{
						_columnTypeCases[k] = TypeCase.Decimal;
						_ordinalToIndexMap[k] = _decimalCount;
						_decimalCount++;
					}
					else if (type == typeof(double))
					{
						_columnTypeCases[k] = TypeCase.Double;
						_ordinalToIndexMap[k] = _doubleCount;
						_doubleCount++;
					}
					else if (type == typeof(float))
					{
						_columnTypeCases[k] = TypeCase.Float;
						_ordinalToIndexMap[k] = _floatCount;
						_floatCount++;
					}
					else if (type == typeof(Guid))
					{
						_columnTypeCases[k] = TypeCase.Guid;
						_ordinalToIndexMap[k] = _guidCount;
						_guidCount++;
					}
					else if (type == typeof(short))
					{
						_columnTypeCases[k] = TypeCase.Short;
						_ordinalToIndexMap[k] = _shortCount;
						_shortCount++;
					}
					else if (type == typeof(int))
					{
						_columnTypeCases[k] = TypeCase.Int;
						_ordinalToIndexMap[k] = _intCount;
						_intCount++;
					}
					else if (type == typeof(long))
					{
						_columnTypeCases[k] = TypeCase.Long;
						_ordinalToIndexMap[k] = _longCount;
						_longCount++;
					}
					else if (type == typeof(sbyte))
					{
						_columnTypeCases[k] = TypeCase.SByte;
						_ordinalToIndexMap[k] = _sbyteCount;
						_sbyteCount++;
					}
					else if (type == typeof(ushort))
					{
						_columnTypeCases[k] = TypeCase.UShort;
						_ordinalToIndexMap[k] = _ushortCount;
						_ushortCount++;
					}
					else if (type == typeof(uint))
					{
						_columnTypeCases[k] = TypeCase.UInt;
						_ordinalToIndexMap[k] = _uintCount;
						_uintCount++;
					}
					else if (type == typeof(ulong))
					{
						_columnTypeCases[k] = TypeCase.ULong;
						_ordinalToIndexMap[k] = _ulongCount;
						_ulongCount++;
					}
					else
					{
						_columnTypeCases[k] = TypeCase.Object;
						_ordinalToIndexMap[k] = _objectCount;
						_objectCount++;
					}
				}
			}
			_tempBools = new bool[_rowCapacity * _boolCount];
			_bytes = new byte[_rowCapacity * _byteCount];
			_chars = new char[_rowCapacity * _charCount];
			_dateTimes = new DateTime[_rowCapacity * _dateTimeCount];
			_dateTimeOffsets = new DateTimeOffset[_rowCapacity * _dateTimeOffsetCount];
			_decimals = new decimal[_rowCapacity * _decimalCount];
			_doubles = new double[_rowCapacity * _doubleCount];
			_floats = new float[_rowCapacity * _floatCount];
			_guids = new Guid[_rowCapacity * _guidCount];
			_shorts = new short[_rowCapacity * _shortCount];
			_ints = new int[_rowCapacity * _intCount];
			_longs = new long[_rowCapacity * _longCount];
			_sbytes = new sbyte[_rowCapacity * _sbyteCount];
			_ushorts = new ushort[_rowCapacity * _ushortCount];
			_uints = new uint[_rowCapacity * _uintCount];
			_ulongs = new ulong[_rowCapacity * _ulongCount];
			_objects = new object[_rowCapacity * _objectCount];
			_nullOrdinalToIndexMap = Enumerable.Repeat(-1, fieldCount).ToArray();
			for (int l = 0; l < fieldCount; l++)
			{
				ReaderColumn readerColumn3 = _columns[l];
				if (readerColumn3 != null && readerColumn3.IsNullable)
				{
					_nullOrdinalToIndexMap[l] = _nullCount;
					_nullCount++;
				}
			}
			_tempNulls = new bool[_rowCapacity * _nullCount];
		}

		private void DoubleBufferCapacity()
		{
			_rowCapacity <<= 1;
			bool[] array = new bool[_tempBools.Length << 1];
			Array.Copy(_tempBools, array, _tempBools.Length);
			_tempBools = array;
			byte[] array2 = new byte[_bytes.Length << 1];
			Array.Copy(_bytes, array2, _bytes.Length);
			_bytes = array2;
			char[] array3 = new char[_chars.Length << 1];
			Array.Copy(_chars, array3, _chars.Length);
			_chars = array3;
			DateTime[] array4 = new DateTime[_dateTimes.Length << 1];
			Array.Copy(_dateTimes, array4, _dateTimes.Length);
			_dateTimes = array4;
			DateTimeOffset[] array5 = new DateTimeOffset[_dateTimeOffsets.Length << 1];
			Array.Copy(_dateTimeOffsets, array5, _dateTimeOffsets.Length);
			_dateTimeOffsets = array5;
			decimal[] array6 = new decimal[_decimals.Length << 1];
			Array.Copy(_decimals, array6, _decimals.Length);
			_decimals = array6;
			double[] array7 = new double[_doubles.Length << 1];
			Array.Copy(_doubles, array7, _doubles.Length);
			_doubles = array7;
			float[] array8 = new float[_floats.Length << 1];
			Array.Copy(_floats, array8, _floats.Length);
			_floats = array8;
			Guid[] array9 = new Guid[_guids.Length << 1];
			Array.Copy(_guids, array9, _guids.Length);
			_guids = array9;
			short[] array10 = new short[_shorts.Length << 1];
			Array.Copy(_shorts, array10, _shorts.Length);
			_shorts = array10;
			int[] array11 = new int[_ints.Length << 1];
			Array.Copy(_ints, array11, _ints.Length);
			_ints = array11;
			long[] array12 = new long[_longs.Length << 1];
			Array.Copy(_longs, array12, _longs.Length);
			_longs = array12;
			sbyte[] array13 = new sbyte[_sbytes.Length << 1];
			Array.Copy(_sbytes, array13, _sbytes.Length);
			_sbytes = array13;
			ushort[] array14 = new ushort[_ushorts.Length << 1];
			Array.Copy(_ushorts, array14, _ushorts.Length);
			_ushorts = array14;
			uint[] array15 = new uint[_uints.Length << 1];
			Array.Copy(_uints, array15, _uints.Length);
			_uints = array15;
			ulong[] array16 = new ulong[_ulongs.Length << 1];
			Array.Copy(_ulongs, array16, _ulongs.Length);
			_ulongs = array16;
			object[] array17 = new object[_objects.Length << 1];
			Array.Copy(_objects, array17, _objects.Length);
			_objects = array17;
			bool[] array18 = new bool[_tempNulls.Length << 1];
			Array.Copy(_tempNulls, array18, _tempNulls.Length);
			_tempNulls = array18;
		}

		private void ReadBool(DbDataReader reader, int ordinal, ReaderColumn column)
		{
			_tempBools[_currentRowNumber * _boolCount + _ordinalToIndexMap[ordinal]] = ((ReaderColumn<bool>)column).GetFieldValue(reader, _indexMap);
		}

		private void ReadByte(DbDataReader reader, int ordinal, ReaderColumn column)
		{
			_bytes[_currentRowNumber * _byteCount + _ordinalToIndexMap[ordinal]] = ((ReaderColumn<byte>)column).GetFieldValue(reader, _indexMap);
		}

		private void ReadChar(DbDataReader reader, int ordinal, ReaderColumn column)
		{
			_chars[_currentRowNumber * _charCount + _ordinalToIndexMap[ordinal]] = ((ReaderColumn<char>)column).GetFieldValue(reader, _indexMap);
		}

		private void ReadDateTime(DbDataReader reader, int ordinal, ReaderColumn column)
		{
			_dateTimes[_currentRowNumber * _dateTimeCount + _ordinalToIndexMap[ordinal]] = ((ReaderColumn<DateTime>)column).GetFieldValue(reader, _indexMap);
		}

		private void ReadDateTimeOffset(DbDataReader reader, int ordinal, ReaderColumn column)
		{
			_dateTimeOffsets[_currentRowNumber * _dateTimeOffsetCount + _ordinalToIndexMap[ordinal]] = ((ReaderColumn<DateTimeOffset>)column).GetFieldValue(reader, _indexMap);
		}

		private void ReadDecimal(DbDataReader reader, int ordinal, ReaderColumn column)
		{
			_decimals[_currentRowNumber * _decimalCount + _ordinalToIndexMap[ordinal]] = ((ReaderColumn<decimal>)column).GetFieldValue(reader, _indexMap);
		}

		private void ReadDouble(DbDataReader reader, int ordinal, ReaderColumn column)
		{
			_doubles[_currentRowNumber * _doubleCount + _ordinalToIndexMap[ordinal]] = ((ReaderColumn<double>)column).GetFieldValue(reader, _indexMap);
		}

		private void ReadFloat(DbDataReader reader, int ordinal, ReaderColumn column)
		{
			_floats[_currentRowNumber * _floatCount + _ordinalToIndexMap[ordinal]] = ((ReaderColumn<float>)column).GetFieldValue(reader, _indexMap);
		}

		private void ReadGuid(DbDataReader reader, int ordinal, ReaderColumn column)
		{
			_guids[_currentRowNumber * _guidCount + _ordinalToIndexMap[ordinal]] = ((ReaderColumn<Guid>)column).GetFieldValue(reader, _indexMap);
		}

		private void ReadShort(DbDataReader reader, int ordinal, ReaderColumn column)
		{
			_shorts[_currentRowNumber * _shortCount + _ordinalToIndexMap[ordinal]] = ((ReaderColumn<short>)column).GetFieldValue(reader, _indexMap);
		}

		private void ReadInt(DbDataReader reader, int ordinal, ReaderColumn column)
		{
			_ints[_currentRowNumber * _intCount + _ordinalToIndexMap[ordinal]] = ((ReaderColumn<int>)column).GetFieldValue(reader, _indexMap);
		}

		private void ReadLong(DbDataReader reader, int ordinal, ReaderColumn column)
		{
			_longs[_currentRowNumber * _longCount + _ordinalToIndexMap[ordinal]] = ((ReaderColumn<long>)column).GetFieldValue(reader, _indexMap);
		}

		private void ReadSByte(DbDataReader reader, int ordinal, ReaderColumn column)
		{
			_sbytes[_currentRowNumber * _sbyteCount + _ordinalToIndexMap[ordinal]] = ((ReaderColumn<sbyte>)column).GetFieldValue(reader, _indexMap);
		}

		private void ReadUShort(DbDataReader reader, int ordinal, ReaderColumn column)
		{
			_ushorts[_currentRowNumber * _ushortCount + _ordinalToIndexMap[ordinal]] = ((ReaderColumn<ushort>)column).GetFieldValue(reader, _indexMap);
		}

		private void ReadUInt(DbDataReader reader, int ordinal, ReaderColumn column)
		{
			_uints[_currentRowNumber * _uintCount + _ordinalToIndexMap[ordinal]] = ((ReaderColumn<uint>)column).GetFieldValue(reader, _indexMap);
		}

		private void ReadULong(DbDataReader reader, int ordinal, ReaderColumn column)
		{
			_ulongs[_currentRowNumber * _ulongCount + _ordinalToIndexMap[ordinal]] = ((ReaderColumn<ulong>)column).GetFieldValue(reader, _indexMap);
		}

		private void ReadObject(DbDataReader reader, int ordinal, ReaderColumn column)
		{
			_objects[_currentRowNumber * _objectCount + _ordinalToIndexMap[ordinal]] = ((ReaderColumn<object>)column).GetFieldValue(reader, _indexMap);
		}
	}

	private DbDataReader _underlyingReader;

	private List<BufferedDataRecord> _bufferedDataRecords = new List<BufferedDataRecord>();

	private BufferedDataRecord _currentResultSet;

	private int _currentResultSetNumber;

	private int _recordsAffected;

	private bool _disposed;

	private bool _isClosed;

	public override int RecordsAffected => _recordsAffected;

	public override object this[string name]
	{
		get
		{
			throw new NotSupportedException();
		}
	}

	public override object this[int ordinal]
	{
		get
		{
			throw new NotSupportedException();
		}
	}

	public override int Depth
	{
		get
		{
			throw new NotSupportedException();
		}
	}

	public override int FieldCount => _currentResultSet.FieldCount;

	public override bool HasRows => _currentResultSet.HasRows;

	public override bool IsClosed => _isClosed;

	public BufferedDataReader([JetBrains.Annotations.NotNull] DbDataReader reader)
	{
		_underlyingReader = reader;
	}

	[Conditional("DEBUG")]
	private void AssertReaderIsOpen()
	{
		if (_underlyingReader != null)
		{
			throw new InvalidOperationException("The reader wasn't initialized");
		}
		if (_isClosed)
		{
			throw new InvalidOperationException("The reader is closed.");
		}
	}

	[Conditional("DEBUG")]
	private void AssertReaderIsOpenWithData()
	{
		if (!_currentResultSet.IsDataReady)
		{
			throw new InvalidOperationException("The reader doesn't have any data.");
		}
	}

	[Conditional("DEBUG")]
	private void AssertFieldIsReady(int ordinal)
	{
		if (0 > ordinal || ordinal > _currentResultSet.FieldCount)
		{
			throw new IndexOutOfRangeException();
		}
	}

	public virtual BufferedDataReader Initialize([JetBrains.Annotations.NotNull] IReadOnlyList<ReaderColumn> columns)
	{
		if (_underlyingReader == null)
		{
			return this;
		}
		try
		{
			do
			{
				_bufferedDataRecords.Add(new BufferedDataRecord().Initialize(_underlyingReader, columns));
			}
			while (_underlyingReader.NextResult());
			_recordsAffected = _underlyingReader.RecordsAffected;
			_currentResultSet = _bufferedDataRecords[_currentResultSetNumber];
			return this;
		}
		finally
		{
			_underlyingReader.Dispose();
			_underlyingReader = null;
		}
	}

	public virtual async Task<BufferedDataReader> InitializeAsync([JetBrains.Annotations.NotNull] IReadOnlyList<ReaderColumn> columns, CancellationToken cancellationToken)
	{
		if (_underlyingReader == null)
		{
			return this;
		}
		try
		{
			do
			{
				List<BufferedDataRecord> bufferedDataRecords = _bufferedDataRecords;
				bufferedDataRecords.Add(await new BufferedDataRecord().InitializeAsync(_underlyingReader, columns, cancellationToken));
			}
			while (await _underlyingReader.NextResultAsync(cancellationToken));
			_recordsAffected = _underlyingReader.RecordsAffected;
			_currentResultSet = _bufferedDataRecords[_currentResultSetNumber];
			return this;
		}
		finally
		{
			_underlyingReader.Dispose();
			_underlyingReader = null;
		}
	}

	public static bool IsSupportedValueType(Type type)
	{
		if (!(type == typeof(int)) && !(type == typeof(bool)) && !(type == typeof(Guid)) && !(type == typeof(byte)) && !(type == typeof(char)) && !(type == typeof(DateTime)) && !(type == typeof(DateTimeOffset)) && !(type == typeof(decimal)) && !(type == typeof(double)) && !(type == typeof(float)) && !(type == typeof(short)) && !(type == typeof(long)) && !(type == typeof(uint)) && !(type == typeof(ushort)) && !(type == typeof(ulong)))
		{
			return type == typeof(sbyte);
		}
		return true;
	}

	public override void Close()
	{
		_bufferedDataRecords = null;
		_isClosed = true;
		DbDataReader underlyingReader = _underlyingReader;
		if (underlyingReader != null)
		{
			_underlyingReader = null;
			underlyingReader.Dispose();
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (!_disposed && disposing && !IsClosed)
		{
			Close();
		}
		_disposed = true;
		base.Dispose(disposing);
	}

	public override bool GetBoolean(int ordinal)
	{
		return _currentResultSet.GetBoolean(ordinal);
	}

	public override byte GetByte(int ordinal)
	{
		return _currentResultSet.GetByte(ordinal);
	}

	public override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
	{
		throw new NotSupportedException();
	}

	public override char GetChar(int ordinal)
	{
		return _currentResultSet.GetChar(ordinal);
	}

	public override long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
	{
		throw new NotSupportedException();
	}

	public override DateTime GetDateTime(int ordinal)
	{
		return _currentResultSet.GetDateTime(ordinal);
	}

	public override decimal GetDecimal(int ordinal)
	{
		return _currentResultSet.GetDecimal(ordinal);
	}

	public override double GetDouble(int ordinal)
	{
		return _currentResultSet.GetDouble(ordinal);
	}

	public override float GetFloat(int ordinal)
	{
		return _currentResultSet.GetFloat(ordinal);
	}

	public override Guid GetGuid(int ordinal)
	{
		return _currentResultSet.GetGuid(ordinal);
	}

	public override short GetInt16(int ordinal)
	{
		return _currentResultSet.GetInt16(ordinal);
	}

	public override int GetInt32(int ordinal)
	{
		return _currentResultSet.GetInt32(ordinal);
	}

	public override long GetInt64(int ordinal)
	{
		return _currentResultSet.GetInt64(ordinal);
	}

	public override string GetString(int ordinal)
	{
		return _currentResultSet.GetFieldValue<string>(ordinal);
	}

	public override T GetFieldValue<T>(int ordinal)
	{
		return _currentResultSet.GetFieldValue<T>(ordinal);
	}

	public override Task<T> GetFieldValueAsync<T>(int ordinal, CancellationToken cancellationToken)
	{
		return _currentResultSet.GetFieldValueAsync<T>(ordinal, cancellationToken);
	}

	public override object GetValue(int ordinal)
	{
		return _currentResultSet.GetValue(ordinal);
	}

	public override int GetValues(object[] values)
	{
		return _currentResultSet.GetValues(values);
	}

	public override string GetDataTypeName(int ordinal)
	{
		return _currentResultSet.GetDataTypeName(ordinal);
	}

	public override Type GetFieldType(int ordinal)
	{
		return _currentResultSet.GetFieldType(ordinal);
	}

	public override string GetName(int ordinal)
	{
		return _currentResultSet.GetName(ordinal);
	}

	public override int GetOrdinal(string name)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(name, "name");
		return _currentResultSet.GetOrdinal(name);
	}

	public override bool IsDBNull(int ordinal)
	{
		return _currentResultSet.IsDBNull(ordinal);
	}

	public override Task<bool> IsDBNullAsync(int ordinal, CancellationToken cancellationToken)
	{
		return _currentResultSet.IsDBNullAsync(ordinal, cancellationToken);
	}

	public override IEnumerator GetEnumerator()
	{
		throw new NotSupportedException();
	}

	public override DataTable GetSchemaTable()
	{
		throw new NotSupportedException();
	}

	public override bool NextResult()
	{
		if (++_currentResultSetNumber < _bufferedDataRecords.Count)
		{
			_currentResultSet = _bufferedDataRecords[_currentResultSetNumber];
			return true;
		}
		_currentResultSet = null;
		return false;
	}

	public override Task<bool> NextResultAsync(CancellationToken cancellationToken)
	{
		return Task.FromResult(NextResult());
	}

	public override bool Read()
	{
		return _currentResultSet.Read();
	}

	public override Task<bool> ReadAsync(CancellationToken cancellationToken)
	{
		return _currentResultSet.ReadAsync(cancellationToken);
	}
}
