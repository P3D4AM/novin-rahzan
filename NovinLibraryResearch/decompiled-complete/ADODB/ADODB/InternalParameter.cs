using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace ADODB;

[ClassInterface(ClassInterfaceType.AutoDispatch)]
[DefaultMember("Value")]
public class InternalParameter : Parameter, _Parameter, _ADO
{
	internal InternalParameters m_parms;

	internal Parameter m_parm;

	[DispId(8)]
	public int Attributes
	{
		get
		{
			return m_parm.Attributes;
		}
		set
		{
			m_parm.Attributes = value;
		}
	}

	[DispId(3)]
	public ParameterDirectionEnum Direction
	{
		get
		{
			return m_parm.Direction;
		}
		set
		{
			m_parm.Direction = value;
		}
	}

	[DispId(1)]
	public string Name
	{
		get
		{
			return m_parm.Name;
		}
		set
		{
			m_parm.Name = value;
		}
	}

	[DispId(5)]
	public byte NumericScale
	{
		get
		{
			return m_parm.NumericScale;
		}
		set
		{
			m_parm.NumericScale = value;
		}
	}

	[DispId(4)]
	public byte Precision
	{
		get
		{
			return m_parm.Precision;
		}
		set
		{
			m_parm.Precision = value;
		}
	}

	[DispId(6)]
	public int Size
	{
		get
		{
			return m_parm.Size;
		}
		set
		{
			m_parm.Size = value;
		}
	}

	[DispId(2)]
	public DataTypeEnum Type
	{
		get
		{
			return m_parm.Type;
		}
		set
		{
			m_parm.Type = value;
		}
	}

	[DispId(0)]
	public object Value
	{
		get
		{
			return m_parm.Value;
		}
		set
		{
			m_parm.Value = value;
		}
	}

	[DispId(500)]
	public Properties Properties => m_parm.Properties;

	internal InternalParameter(Parameter parm)
	{
		m_parm = parm;
	}

	[DispId(7)]
	public void AppendChunk(object Data)
	{
		m_parm.AppendChunk(Data);
	}

	internal Parameter GetParm()
	{
		return m_parm;
	}

	internal IntPtr GetParam()
	{
		return Marshal.GetIUnknownForObject(m_parm);
	}
}
