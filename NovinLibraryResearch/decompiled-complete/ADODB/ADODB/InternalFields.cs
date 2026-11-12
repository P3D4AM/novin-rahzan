using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace ADODB;

[ClassInterface(ClassInterfaceType.None)]
public class InternalFields : Fields, Fields20, Fields15, _Collection, IEnumerable
{
	internal Fields m_fields = null;

	public int Count => m_fields.Count;

	public Field this[object index]
	{
		get
		{
			InternalField internalField = new InternalField(m_fields[index]);
			internalField.m_fields = this;
			return internalField;
		}
	}

	internal InternalFields(Fields fields)
	{
		m_fields = fields;
	}

	internal IntPtr GetFields()
	{
		return Marshal.GetIUnknownForObject(m_fields);
	}

	public IEnumerator GetEnumerator()
	{
		return new InternalEnumerator(m_fields.GetEnumerator(), this);
	}

	public void Refresh()
	{
		m_fields.Refresh();
	}

	public void _Append(string Name, DataTypeEnum Type, int DefinedSize, FieldAttributeEnum Attrib)
	{
		m_fields._Append(Name, Type, DefinedSize, Attrib);
	}

	public void Delete(object Index)
	{
		m_fields.Delete(Index);
	}

	public void Append(string Name, DataTypeEnum Type, int DefinedSize, FieldAttributeEnum Attrib, object FieldValue)
	{
		m_fields.Append(Name, Type, DefinedSize, Attrib, FieldValue);
	}

	public void Update()
	{
		m_fields.Update();
	}

	public void Resync(ResyncEnum ResyncValues)
	{
		m_fields.Resync(ResyncValues);
	}

	public void CancelUpdate()
	{
		m_fields.CancelUpdate();
	}
}
