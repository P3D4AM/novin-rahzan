using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using Microsoft.Data.Common;

namespace Microsoft.Data.SqlClient;

[ComVisible(true)]
[ClassInterface(ClassInterfaceType.None)]
[Guid("afef65ad-4577-447a-a148-83acadd3d4b9")]
[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
public sealed class SQLDebugging : ISQLDebug
{
	private const int STANDARD_RIGHTS_REQUIRED = 983040;

	private const int DELETE = 65536;

	private const int READ_CONTROL = 131072;

	private const int WRITE_DAC = 262144;

	private const int WRITE_OWNER = 524288;

	private const int SYNCHRONIZE = 1048576;

	private const int FILE_ALL_ACCESS = 2032127;

	private const uint GENERIC_READ = 2147483648u;

	private const uint GENERIC_WRITE = 1073741824u;

	private const uint GENERIC_EXECUTE = 536870912u;

	private const uint GENERIC_ALL = 268435456u;

	private const int SECURITY_DESCRIPTOR_REVISION = 1;

	private const int ACL_REVISION = 2;

	private const int SECURITY_AUTHENTICATED_USER_RID = 11;

	private const int SECURITY_LOCAL_SYSTEM_RID = 18;

	private const int SECURITY_BUILTIN_DOMAIN_RID = 32;

	private const int SECURITY_WORLD_RID = 0;

	private const byte SECURITY_NT_AUTHORITY = 5;

	private const int DOMAIN_GROUP_RID_ADMINS = 512;

	private const int DOMAIN_ALIAS_RID_ADMINS = 544;

	private const int sizeofSECURITY_ATTRIBUTES = 12;

	private const int sizeofSECURITY_DESCRIPTOR = 20;

	private const int sizeofACCESS_ALLOWED_ACE = 12;

	private const int sizeofACCESS_DENIED_ACE = 12;

	private const int sizeofSID_IDENTIFIER_AUTHORITY = 6;

	private const int sizeofACL = 8;

	private IntPtr CreateSD(ref IntPtr pDacl)
	{
		IntPtr intPtr = IntPtr.Zero;
		IntPtr pSid = IntPtr.Zero;
		IntPtr pSid2 = IntPtr.Zero;
		IntPtr zero = IntPtr.Zero;
		int num = 0;
		bool flag = false;
		zero = Marshal.AllocHGlobal(6);
		if (!(zero == IntPtr.Zero))
		{
			Marshal.WriteInt32(zero, 0, 0);
			Marshal.WriteByte(zero, 4, 0);
			Marshal.WriteByte(zero, 5, 5);
			flag = NativeMethods.AllocateAndInitializeSid(zero, 1, 11, 0, 0, 0, 0, 0, 0, 0, ref pSid);
			if (flag && !(pSid == IntPtr.Zero))
			{
				flag = NativeMethods.AllocateAndInitializeSid(zero, 2, 32, 544, 0, 0, 0, 0, 0, 0, ref pSid2);
				if (flag && !(pSid2 == IntPtr.Zero))
				{
					flag = false;
					intPtr = Marshal.AllocHGlobal(20);
					if (!(intPtr == IntPtr.Zero))
					{
						for (int i = 0; i < 20; i++)
						{
							Marshal.WriteByte(intPtr, i, 0);
						}
						num = 44 + NativeMethods.GetLengthSid(pSid) + NativeMethods.GetLengthSid(pSid2);
						pDacl = Marshal.AllocHGlobal(num);
						if (!(pDacl == IntPtr.Zero) && NativeMethods.InitializeAcl(pDacl, num, 2) && NativeMethods.AddAccessDeniedAce(pDacl, 2, 262144, pSid) && NativeMethods.AddAccessAllowedAce(pDacl, 2, 2147483648u, pSid) && NativeMethods.AddAccessAllowedAce(pDacl, 2, 268435456u, pSid2) && NativeMethods.InitializeSecurityDescriptor(intPtr, 1) && NativeMethods.SetSecurityDescriptorDacl(intPtr, bDaclPresent: true, pDacl, bDaclDefaulted: false))
						{
							flag = true;
						}
					}
				}
			}
		}
		if (zero != IntPtr.Zero)
		{
			Marshal.FreeHGlobal(zero);
		}
		if (pSid2 != IntPtr.Zero)
		{
			NativeMethods.FreeSid(pSid2);
		}
		if (pSid != IntPtr.Zero)
		{
			NativeMethods.FreeSid(pSid);
		}
		if (flag)
		{
			return intPtr;
		}
		if (intPtr != IntPtr.Zero)
		{
			Marshal.FreeHGlobal(intPtr);
		}
		return IntPtr.Zero;
	}

	bool ISQLDebug.SQLDebug(int dwpidDebugger, int dwpidDebuggee, [MarshalAs(UnmanagedType.LPStr)] string pszMachineName, [MarshalAs(UnmanagedType.LPStr)] string pszSDIDLLName, int dwOption, int cbData, byte[] rgbData)
	{
		bool flag = false;
		IntPtr zero = IntPtr.Zero;
		IntPtr zero2 = IntPtr.Zero;
		IntPtr zero3 = IntPtr.Zero;
		IntPtr zero4 = IntPtr.Zero;
		IntPtr pDacl = IntPtr.Zero;
		if (pszMachineName == null || pszSDIDLLName == null)
		{
			return false;
		}
		if (pszMachineName.Length > 32 || pszSDIDLLName.Length > 16)
		{
			return false;
		}
		Encoding encoding = Encoding.GetEncoding(1252);
		byte[] bytes = encoding.GetBytes(pszMachineName);
		byte[] bytes2 = encoding.GetBytes(pszSDIDLLName);
		if (rgbData != null && cbData > 255)
		{
			return false;
		}
		string text = ((!ADP.IsPlatformNT5) ? "SqlClientSSDebug" : "Global\\SqlClientSSDebug");
		text += dwpidDebuggee.ToString(CultureInfo.InvariantCulture);
		zero3 = CreateSD(ref pDacl);
		zero4 = Marshal.AllocHGlobal(12);
		if (zero3 == IntPtr.Zero || zero4 == IntPtr.Zero)
		{
			return false;
		}
		Marshal.WriteInt32(zero4, 0, 12);
		Marshal.WriteIntPtr(zero4, 4, zero3);
		Marshal.WriteInt32(zero4, 8, 0);
		zero = NativeMethods.CreateFileMappingA(ADP.InvalidPtr, zero4, 4, 0, Marshal.SizeOf(typeof(MEMMAP)), text);
		if (!(IntPtr.Zero == zero))
		{
			zero2 = NativeMethods.MapViewOfFile(zero, 6, 0, 0, IntPtr.Zero);
			if (!(IntPtr.Zero == zero2))
			{
				int num = 0;
				Marshal.WriteInt32(zero2, num, dwpidDebugger);
				num += 4;
				Marshal.WriteInt32(zero2, num, dwOption);
				num += 4;
				Marshal.Copy(bytes, 0, ADP.IntPtrOffset(zero2, num), bytes.Length);
				num += 32;
				Marshal.Copy(bytes2, 0, ADP.IntPtrOffset(zero2, num), bytes2.Length);
				num += 16;
				Marshal.WriteInt32(zero2, num, cbData);
				num += 4;
				if (rgbData != null)
				{
					Marshal.Copy(rgbData, 0, ADP.IntPtrOffset(zero2, num), cbData);
				}
				NativeMethods.UnmapViewOfFile(zero2);
				flag = true;
			}
		}
		if (!flag && zero != IntPtr.Zero)
		{
			NativeMethods.CloseHandle(zero);
		}
		if (zero4 != IntPtr.Zero)
		{
			Marshal.FreeHGlobal(zero4);
		}
		if (zero3 != IntPtr.Zero)
		{
			Marshal.FreeHGlobal(zero3);
		}
		if (pDacl != IntPtr.Zero)
		{
			Marshal.FreeHGlobal(pDacl);
		}
		return flag;
	}
}
