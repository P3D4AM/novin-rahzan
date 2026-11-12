using System;
using System.IO;
using System.Reflection;

namespace Microsoft.Data.SqlClient.Server;

internal sealed class ULongNormalizer : Normalizer
{
	internal override int Size => 8;

	internal override void Normalize(FieldInfo fi, object obj, Stream s)
	{
		byte[] bytes = BitConverter.GetBytes((ulong)GetValue(fi, obj));
		if (!m_skipNormalize)
		{
			Array.Reverse((Array)bytes);
		}
		s.Write(bytes, 0, bytes.Length);
	}

	internal override void DeNormalize(FieldInfo fi, object recvr, Stream s)
	{
		byte[] array = new byte[8];
		s.Read(array, 0, array.Length);
		if (!m_skipNormalize)
		{
			Array.Reverse((Array)array);
		}
		SetValue(fi, recvr, BitConverter.ToUInt64(array, 0));
	}
}
