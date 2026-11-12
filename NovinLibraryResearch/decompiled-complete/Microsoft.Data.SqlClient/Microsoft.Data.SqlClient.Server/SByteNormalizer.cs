using System.IO;
using System.Reflection;

namespace Microsoft.Data.SqlClient.Server;

internal sealed class SByteNormalizer : Normalizer
{
	internal override int Size => 1;

	internal override void Normalize(FieldInfo fi, object obj, Stream s)
	{
		sbyte b = (sbyte)GetValue(fi, obj);
		byte b2 = (byte)b;
		if (!m_skipNormalize)
		{
			b2 = (byte)(b2 ^ 0x80u);
		}
		s.WriteByte(b2);
	}

	internal override void DeNormalize(FieldInfo fi, object recvr, Stream s)
	{
		byte b = (byte)s.ReadByte();
		if (!m_skipNormalize)
		{
			b = (byte)(b ^ 0x80u);
		}
		sbyte b2 = (sbyte)b;
		SetValue(fi, recvr, b2);
	}
}
