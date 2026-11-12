using System.Text;

namespace Stimulsoft.Base.Excel;

internal class XlsBiffLabelCell : XlsBiffBlankCell
{
	private Encoding m_UseEncoding = Encoding.Default;

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

	public byte Length => ReadByte(6);

	public string Value
	{
		get
		{
			byte[] array = ReadArray(8, Length * (Helpers.IsSingleByteEncoding(m_UseEncoding) ? 1 : 2));
			return m_UseEncoding.GetString(array, 0, array.Length);
		}
	}

	internal XlsBiffLabelCell(byte[] bytes)
		: this(bytes, 0u)
	{
	}

	internal XlsBiffLabelCell(byte[] bytes, uint offset)
		: base(bytes, offset)
	{
	}
}
