using System.Text;

namespace Stimulsoft.Base.Excel;

internal class XlsBiffFormulaString : XlsBiffRecord
{
	private Encoding m_UseEncoding = Encoding.Default;

	private const int LEADING_BYTES_COUNT = 3;

	public Encoding UseEncoding
	{
		get
		{
			return m_UseEncoding;
		}
		set
		{
			m_UseEncoding = value;
		}
	}

	public ushort Length => ReadUInt16(0);

	public string Value
	{
		get
		{
			if (ReadUInt16(1) != 0)
			{
				return Encoding.Unicode.GetString(m_bytes, m_readoffset + 3, Length * 2);
			}
			return m_UseEncoding.GetString(m_bytes, m_readoffset + 3, Length);
		}
	}

	internal XlsBiffFormulaString(byte[] bytes)
		: this(bytes, 0u)
	{
	}

	internal XlsBiffFormulaString(byte[] bytes, uint offset)
		: base(bytes, offset)
	{
	}
}
