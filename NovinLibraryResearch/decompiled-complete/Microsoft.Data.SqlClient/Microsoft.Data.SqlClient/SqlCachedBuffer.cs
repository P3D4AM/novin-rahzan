using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Permissions;
using System.Xml;

namespace Microsoft.Data.SqlClient;

internal sealed class SqlCachedBuffer : INullable
{
	public static readonly SqlCachedBuffer Null = new SqlCachedBuffer();

	private const int _maxChunkSize = 2048;

	private List<byte[]> _cachedBytes;

	internal List<byte[]> CachedBytes => _cachedBytes;

	public bool IsNull
	{
		get
		{
			if (_cachedBytes != null)
			{
				return false;
			}
			return true;
		}
	}

	private SqlCachedBuffer()
	{
	}

	private SqlCachedBuffer(List<byte[]> cachedBytes)
	{
		_cachedBytes = cachedBytes;
	}

	internal static bool TryCreate(SqlMetaDataPriv metadata, TdsParser parser, TdsParserStateObject stateObj, out SqlCachedBuffer buffer)
	{
		int num = 0;
		List<byte[]> list = new List<byte[]>();
		buffer = null;
		if (!parser.TryPlpBytesLeft(stateObj, out var left))
		{
			return false;
		}
		while (left != 0L)
		{
			do
			{
				num = (int)((left > 2048) ? 2048 : left);
				byte[] buff = new byte[num];
				if (!stateObj.TryReadPlpBytes(ref buff, 0, num, out num))
				{
					return false;
				}
				if (list.Count == 0)
				{
					AddByteOrderMark(buff, list);
				}
				list.Add(buff);
				left -= (ulong)num;
			}
			while (left != 0);
			if (!parser.TryPlpBytesLeft(stateObj, out left))
			{
				return false;
			}
			if (left == 0)
			{
				break;
			}
		}
		buffer = new SqlCachedBuffer(list);
		return true;
	}

	private static void AddByteOrderMark(byte[] byteArr, List<byte[]> cachedBytes)
	{
		if (byteArr.Length < 2 || byteArr[0] != 223 || byteArr[1] != byte.MaxValue)
		{
			cachedBytes.Add(TdsEnums.XMLUNICODEBOMBYTES);
		}
	}

	internal Stream ToStream()
	{
		return new SqlCachedStream(this);
	}

	public override string ToString()
	{
		if (IsNull)
		{
			throw new SqlNullValueException();
		}
		if (_cachedBytes.Count == 0)
		{
			return string.Empty;
		}
		SqlXml sqlXml = new SqlXml(ToStream());
		return sqlXml.Value;
	}

	internal SqlString ToSqlString()
	{
		if (IsNull)
		{
			return SqlString.Null;
		}
		string data = ToString();
		return new SqlString(data);
	}

	internal SqlXml ToSqlXml()
	{
		return new SqlXml(ToStream());
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal XmlReader ToXmlReader()
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
		xmlReaderSettings.ConformanceLevel = ConformanceLevel.Fragment;
		MethodInfo method = typeof(XmlReader).GetMethod("CreateSqlReader", BindingFlags.Static | BindingFlags.NonPublic);
		object[] parameters = new object[3]
		{
			ToStream(),
			xmlReaderSettings,
			null
		};
		((CodeAccessPermission)new ReflectionPermission((ReflectionPermissionFlag)2)).Assert();
		try
		{
			return (XmlReader)method.Invoke(null, parameters);
		}
		finally
		{
			CodeAccessPermission.RevertAssert();
		}
	}
}
