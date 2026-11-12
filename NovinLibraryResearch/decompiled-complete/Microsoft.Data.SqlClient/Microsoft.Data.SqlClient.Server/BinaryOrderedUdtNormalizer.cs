using System;
using System.Data.SqlTypes;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace Microsoft.Data.SqlClient.Server;

internal sealed class BinaryOrderedUdtNormalizer : Normalizer
{
	internal readonly FieldInfoEx[] m_fieldsToNormalize;

	private int m_size;

	private byte[] m_PadBuffer;

	internal readonly object NullInstance;

	private bool m_isTopLevelUdt;

	internal bool IsNullable => NullInstance != null;

	internal override int Size
	{
		get
		{
			if (m_size != 0)
			{
				return m_size;
			}
			if (IsNullable && !m_isTopLevelUdt)
			{
				m_size = 1;
			}
			FieldInfoEx[] fieldsToNormalize = m_fieldsToNormalize;
			foreach (FieldInfoEx fieldInfoEx in fieldsToNormalize)
			{
				m_size += fieldInfoEx.normalizer.Size;
			}
			return m_size;
		}
	}

	[ReflectionPermission(SecurityAction.Assert, MemberAccess = true)]
	private FieldInfo[] GetFields(Type t)
	{
		return t.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
	}

	internal BinaryOrderedUdtNormalizer(Type t, bool isTopLevelUdt)
	{
		m_skipNormalize = false;
		if (m_skipNormalize)
		{
			m_isTopLevelUdt = true;
		}
		m_isTopLevelUdt = true;
		FieldInfo[] fields = GetFields(t);
		m_fieldsToNormalize = new FieldInfoEx[fields.Length];
		int num = 0;
		FieldInfo[] array = fields;
		foreach (FieldInfo fieldInfo in array)
		{
			int offset = Marshal.OffsetOf(fieldInfo.DeclaringType, fieldInfo.Name).ToInt32();
			m_fieldsToNormalize[num++] = new FieldInfoEx(fieldInfo, offset, Normalizer.GetNormalizer(fieldInfo.FieldType));
		}
		Array.Sort(m_fieldsToNormalize);
		if (m_isTopLevelUdt || !typeof(INullable).IsAssignableFrom(t))
		{
			return;
		}
		PropertyInfo property = t.GetProperty("Null", BindingFlags.Static | BindingFlags.Public);
		if (property == null || property.PropertyType != t)
		{
			FieldInfo field = t.GetField("Null", BindingFlags.Static | BindingFlags.Public);
			if (field == null || field.FieldType != t)
			{
				throw new Exception("could not find Null field/property in nullable type " + t);
			}
			NullInstance = field.GetValue(null);
		}
		else
		{
			NullInstance = property.GetValue(null, null);
		}
		m_PadBuffer = new byte[Size - 1];
	}

	internal void NormalizeTopObject(object udt, Stream s)
	{
		Normalize(null, udt, s);
	}

	internal object DeNormalizeTopObject(Type t, Stream s)
	{
		return DeNormalizeInternal(t, s);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private object DeNormalizeInternal(Type t, Stream s)
	{
		object obj = null;
		if (!m_isTopLevelUdt && typeof(INullable).IsAssignableFrom(t) && (byte)s.ReadByte() == 0)
		{
			obj = NullInstance;
			s.Read(m_PadBuffer, 0, m_PadBuffer.Length);
			return obj;
		}
		if (obj == null)
		{
			obj = Activator.CreateInstance(t);
		}
		FieldInfoEx[] fieldsToNormalize = m_fieldsToNormalize;
		foreach (FieldInfoEx fieldInfoEx in fieldsToNormalize)
		{
			fieldInfoEx.normalizer.DeNormalize(fieldInfoEx.fieldInfo, obj, s);
		}
		return obj;
	}

	internal override void Normalize(FieldInfo fi, object obj, Stream s)
	{
		object obj2 = ((!(fi == null)) ? GetValue(fi, obj) : obj);
		if (obj2 is INullable nullable && !m_isTopLevelUdt)
		{
			if (nullable.IsNull)
			{
				s.WriteByte(0);
				s.Write(m_PadBuffer, 0, m_PadBuffer.Length);
				return;
			}
			s.WriteByte(1);
		}
		FieldInfoEx[] fieldsToNormalize = m_fieldsToNormalize;
		foreach (FieldInfoEx fieldInfoEx in fieldsToNormalize)
		{
			fieldInfoEx.normalizer.Normalize(fieldInfoEx.fieldInfo, obj2, s);
		}
	}

	internal override void DeNormalize(FieldInfo fi, object recvr, Stream s)
	{
		SetValue(fi, recvr, DeNormalizeInternal(fi.FieldType, s));
	}
}
