using System;
using System.Runtime.InteropServices;

namespace ADODB;

internal class ParameterToInternalParameterMarshaler : ICustomMarshaler
{
	private static ParameterToInternalParameterMarshaler m_me = null;

	public object MarshalNativeToManaged(IntPtr pNativeData)
	{
		if (pNativeData == (IntPtr)0)
		{
			throw new ArgumentNullException(GetType().FullName + " .MarshalNativeToManaged(IntPtr pNativeData)");
		}
		return new InternalParameter((Parameter)Marshal.GetObjectForIUnknown(pNativeData));
	}

	public IntPtr MarshalManagedToNative(object ManagedObj)
	{
		if (ManagedObj == null)
		{
			throw new ArgumentNullException(GetType().FullName + " .MarshalManagedToNative(IntPtr pNativeData)");
		}
		return Marshal.GetIUnknownForObject(((InternalParameter)ManagedObj).GetParam());
	}

	public void CleanUpNativeData(IntPtr pNativeData)
	{
		Marshal.Release(pNativeData);
	}

	public void CleanUpManagedData(object o)
	{
	}

	public int GetNativeDataSize()
	{
		return -1;
	}

	public static ICustomMarshaler GetInstance(string cookie)
	{
		if (m_me == null)
		{
			return new ParameterToInternalParameterMarshaler();
		}
		return m_me;
	}
}
