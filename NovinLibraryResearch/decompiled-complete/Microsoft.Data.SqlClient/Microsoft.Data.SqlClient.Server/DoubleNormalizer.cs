using System;
using System.IO;
using System.Reflection;

namespace Microsoft.Data.SqlClient.Server;

internal sealed class DoubleNormalizer : Normalizer
{
	internal override int Size => 8;

	internal override void Normalize(FieldInfo fi, object obj, Stream s)
	{
		double num = (double)GetValue(fi, obj);
		byte[] bytes = BitConverter.GetBytes(num);
		if (!m_skipNormalize)
		{
			Array.Reverse((Array)bytes);
			if ((bytes[0] & 0x80) == 0)
			{
				bytes[0] ^= 128;
			}
			else if (num < 0.0)
			{
				FlipAllBits(bytes);
			}
		}
		s.Write(bytes, 0, bytes.Length);
	}

	internal override void DeNormalize(FieldInfo fi, object recvr, Stream s)
	{
		byte[] array = new byte[8];
		s.Read(array, 0, array.Length);
		if (!m_skipNormalize)
		{
			if ((array[0] & 0x80) > 0)
			{
				array[0] ^= 128;
			}
			else
			{
				FlipAllBits(array);
			}
			Array.Reverse((Array)array);
		}
		SetValue(fi, recvr, BitConverter.ToDouble(array, 0));
	}
}
