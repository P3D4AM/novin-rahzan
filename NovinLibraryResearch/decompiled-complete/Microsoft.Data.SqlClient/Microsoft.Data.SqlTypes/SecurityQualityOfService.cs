using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Microsoft.Data.SqlTypes;

internal class SecurityQualityOfService : SafeHandleZeroOrMinusOneIsInvalid
{
	private UnsafeNativeMethods.SECURITY_QUALITY_OF_SERVICE m_qos;

	private GCHandle m_hQos;

	public SecurityQualityOfService(UnsafeNativeMethods.SecurityImpersonationLevel impersonationLevel, bool effectiveOnly, bool dynamicTrackingMode)
		: base(ownsHandle: true)
	{
		Initialize(impersonationLevel, effectiveOnly, dynamicTrackingMode);
	}

	protected override bool ReleaseHandle()
	{
		if (m_hQos.IsAllocated)
		{
			m_hQos.Free();
		}
		handle = IntPtr.Zero;
		return true;
	}

	internal void Initialize(UnsafeNativeMethods.SecurityImpersonationLevel impersonationLevel, bool effectiveOnly, bool dynamicTrackingMode)
	{
		m_qos.length = (uint)Marshal.SizeOf(typeof(UnsafeNativeMethods.SECURITY_QUALITY_OF_SERVICE));
		m_qos.impersonationLevel = (int)impersonationLevel;
		m_qos.effectiveOnly = (byte)(effectiveOnly ? 1 : 0);
		m_qos.contextDynamicTrackingMode = (byte)(dynamicTrackingMode ? 1 : 0);
		IntPtr zero = IntPtr.Zero;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
		}
		finally
		{
			m_hQos = GCHandle.Alloc(m_qos, GCHandleType.Pinned);
			zero = m_hQos.AddrOfPinnedObject();
			if (zero != IntPtr.Zero)
			{
				SetHandle(zero);
			}
		}
	}
}
