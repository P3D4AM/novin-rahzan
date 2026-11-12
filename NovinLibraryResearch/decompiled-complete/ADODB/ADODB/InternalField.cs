using System.Reflection;
using System.Runtime.InteropServices;

namespace ADODB;

[ClassInterface(ClassInterfaceType.None)]
[DefaultMember("Value")]
public class InternalField : Field, Field20, _ADO
{
	internal InternalFields m_fields;

	internal Field m_field;

	public int Status => m_field.Status;

	public int ActualSize => m_field.ActualSize;

	public int Attributes
	{
		get
		{
			return m_field.Attributes;
		}
		set
		{
			m_field.Attributes = value;
		}
	}

	public object DataFormat
	{
		get
		{
			return m_field.DataFormat;
		}
		set
		{
			m_field.DataFormat = value;
		}
	}

	public int DefinedSize
	{
		get
		{
			return m_field.DefinedSize;
		}
		set
		{
			m_field.DefinedSize = value;
		}
	}

	public string Name => m_field.Name;

	public byte NumericScale
	{
		get
		{
			return m_field.NumericScale;
		}
		set
		{
			m_field.NumericScale = value;
		}
	}

	public object OriginalValue => m_field.OriginalValue;

	public byte Precision
	{
		get
		{
			return m_field.Precision;
		}
		set
		{
			m_field.Precision = value;
		}
	}

	public Properties Properties => m_field.Properties;

	public DataTypeEnum Type
	{
		get
		{
			return m_field.Type;
		}
		set
		{
			m_field.Type = value;
		}
	}

	public object UnderlyingValue => m_field.UnderlyingValue;

	public object Value
	{
		get
		{
			return m_field.Value;
		}
		set
		{
			m_field.Value = value;
		}
	}

	internal InternalField(Field field)
	{
		m_field = field;
	}

	public void AppendChunk(object Data)
	{
		m_field.AppendChunk(Data);
	}

	public object GetChunk(int Length)
	{
		return m_field.GetChunk(Length);
	}
}
