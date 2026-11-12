using System;
using System.Runtime.InteropServices;

namespace ADODB;

internal class ParametersToInternalParametersMarshaler : ICustomMarshaler
{
	private static ParametersToInternalParametersMarshaler m_me = null;

	public object MarshalNativeToManaged(IntPtr pNativeData)
	{
		if (pNativeData == (IntPtr)0)
		{
			throw new ArgumentNullException(GetType().FullName + " .MarshalNativeToManaged(IntPtr pNativeData)");
		}
		return new InternalParameters((Parameters)Marshal.GetObjectForIUnknown(pNativeData));
	}

	public IntPtr MarshalManagedToNative(object ManagedObj)
	{
		if (ManagedObj == null)
		{
			throw new ArgumentNullException(GetType().FullName + " .MarshalManagedToNative(IntPtr pNativeData)");
		}
		return ((InternalParameters)ManagedObj).GetParms();
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
			m_me = new ParametersToInternalParametersMarshaler();
		}
		return m_me;
	}
}
