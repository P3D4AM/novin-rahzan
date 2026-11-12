using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Microsoft.Data.Common;

[SuppressUnmanagedCodeSecurity]
internal static class UnsafeNativeMethods
{
	[ComImport]
	[Guid("00000567-0000-0010-8000-00AA006D2EA4")]
	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	[SuppressUnmanagedCodeSecurity]
	internal interface ADORecordConstruction
	{
		[return: MarshalAs(UnmanagedType.Interface)]
		object get_Row();
	}

	[ComImport]
	[Guid("00000283-0000-0010-8000-00AA006D2EA4")]
	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	[SuppressUnmanagedCodeSecurity]
	internal interface ADORecordsetConstruction
	{
		[return: MarshalAs(UnmanagedType.Interface)]
		object get_Rowset();

		[Obsolete("not used", true)]
		void put_Rowset();

		IntPtr get_Chapter();
	}

	[ComImport]
	[Guid("0C733A64-2A1C-11CE-ADE5-00AA0044773D")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[SuppressUnmanagedCodeSecurity]
	internal interface ICommandWithParameters
	{
		[Obsolete("not used", true)]
		void GetParameterInfo();

		[Obsolete("not used", true)]
		void MapParameterNames();
	}

	[ComImport]
	[Guid("2206CCB1-19C1-11D1-89E0-00C04FD7A829")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[SuppressUnmanagedCodeSecurity]
	internal interface IDataInitialize
	{
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal struct Trustee
	{
		internal IntPtr _pMultipleTrustee;

		internal int _MultipleTrusteeOperation;

		internal int _TrusteeForm;

		internal int _TrusteeType;

		[MarshalAs(UnmanagedType.LPTStr)]
		internal string _name;

		internal Trustee(string name)
		{
			_pMultipleTrustee = IntPtr.Zero;
			_MultipleTrusteeOperation = 0;
			_TrusteeForm = 1;
			_TrusteeType = 1;
			_name = name;
		}
	}

	[DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
	internal static extern uint GetEffectiveRightsFromAclW(byte[] pAcl, ref Trustee pTrustee, out uint pAccessMask);

	[DllImport("advapi32.dll", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	internal static extern bool CheckTokenMembership(IntPtr tokenHandle, byte[] sidToCheck, out bool isMember);

	[DllImport("advapi32.dll", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	internal static extern bool ConvertSidToStringSidW(IntPtr sid, out IntPtr stringSid);

	[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	internal static extern int CreateWellKnownSid(int sidType, byte[] domainSid, [Out] byte[] resultSid, ref uint resultSidLength);

	[DllImport("advapi32.dll", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	internal static extern bool GetTokenInformation(IntPtr tokenHandle, uint token_class, IntPtr tokenStruct, uint tokenInformationLength, ref uint tokenString);

	[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
	internal static extern int lstrlenW(IntPtr ptr);
}
