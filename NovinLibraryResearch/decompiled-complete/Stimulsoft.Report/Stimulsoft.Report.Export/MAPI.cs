using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using Microsoft.Win32;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Export;

[SuppressUnmanagedCodeSecurity]
public static class MAPI
{
	private const int MAPI_DIALOG = 8;

	private const int MapiLogonUI = 1;

	public static void SendEMail(string subject, string body, string filePath)
	{
		SendEMail(string.Empty, subject, body, filePath);
	}

	public static void SendEMail(string recipient, string subject, string body, string filePath)
	{
		try
		{
			IntPtr zero = IntPtr.Zero;
			IntPtr session = IntPtr.Zero;
			if (Logon(zero, ref session))
			{
				MapiMessage mapiMessage = CreateMessage(recipient, subject, body, filePath);
				MAPISendMail(session, zero, mapiMessage, 8, 0);
				Logoff(zero, ref session);
				DisposeMessage(mapiMessage);
				return;
			}
			if (Environment.Is64BitOperatingSystem)
			{
				try
				{
					Process.Start(new ProcessStartInfo("Fixmapi.exe"));
				}
				catch
				{
				}
			}
			int num;
			try
			{
				MapiMessage message = CreateMessage(recipient, subject, body, filePath);
				num = MAPISendMail(IntPtr.Zero, IntPtr.Zero, message, 9, 0);
			}
			catch
			{
				num = 1;
			}
			if (num <= 1)
			{
				return;
			}
			try
			{
				object value = Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\Clients\\Mail", "", "");
				if (value == null)
				{
					value = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Clients\\Mail", "", "");
				}
				if (value != null)
				{
					string text = value.ToString().ToLowerInvariant();
					if (!string.IsNullOrEmpty(text) && text.IndexOf("outlook", StringComparison.Ordinal) != -1)
					{
						filePath = null;
					}
				}
			}
			catch
			{
			}
			StringBuilder stringBuilder = new StringBuilder("mailto:");
			if (!string.IsNullOrEmpty(recipient))
			{
				stringBuilder.Append(recipient);
			}
			if (!string.IsNullOrEmpty(subject) || !string.IsNullOrEmpty(body) || !string.IsNullOrEmpty(filePath))
			{
				stringBuilder.Append("?");
			}
			if (!string.IsNullOrEmpty(subject))
			{
				stringBuilder.Append("subject=" + StiExportUtils.StringToUrl(subject));
			}
			if (!string.IsNullOrEmpty(body))
			{
				stringBuilder.Append("&body=" + StiExportUtils.StringToUrl(body));
			}
			if (!string.IsNullOrEmpty(filePath))
			{
				stringBuilder.Append("&attachment=\"" + StiExportUtils.StringToUrl(filePath) + "\"");
			}
			try
			{
				StiProcess.Start($"\"{stringBuilder}\"");
			}
			catch
			{
			}
		}
		finally
		{
			_controlfp(524319, 16);
		}
	}

	private static MapiMessage CreateMessage(string recipient, string subject, string body, string filePath)
	{
		MapiMessage mapiMessage = new MapiMessage
		{
			subject = subject,
			noteText = body,
			fileCount = 1,
			files = GetFileDesc(filePath)
		};
		if (!string.IsNullOrEmpty(recipient))
		{
			mapiMessage.recipCount = 1;
			mapiMessage.recips = GetRecipients(recipient);
		}
		return mapiMessage;
	}

	private static IntPtr GetRecipients(string recipient)
	{
		if (string.IsNullOrEmpty(recipient))
		{
			return IntPtr.Zero;
		}
		int cb = Marshal.SizeOf(typeof(MapiRecipDesc));
		IntPtr intPtr = Marshal.AllocHGlobal(cb);
		MapiRecipDesc structure = new MapiRecipDesc
		{
			recipClass = 1,
			name = recipient
		};
		int num = (int)intPtr;
		Marshal.StructureToPtr(structure, (IntPtr)num, fDeleteOld: false);
		return intPtr;
	}

	private static void DisposeMessage(MapiMessage msg)
	{
		if (!(msg.files == IntPtr.Zero))
		{
			Marshal.DestroyStructure(msg.files, typeof(MapiFileDesc));
			Marshal.FreeHGlobal(msg.files);
		}
	}

	private static IntPtr GetFileDesc(string filePath)
	{
		if (string.IsNullOrEmpty(filePath))
		{
			return IntPtr.Zero;
		}
		Type typeFromHandle = typeof(MapiFileDesc);
		int cb = Marshal.SizeOf(typeFromHandle);
		IntPtr intPtr = Marshal.AllocHGlobal(cb);
		MapiFileDesc structure = new MapiFileDesc
		{
			position = -1,
			fileName = Path.GetFileName(filePath),
			pathName = filePath
		};
		Marshal.StructureToPtr(structure, intPtr, fDeleteOld: false);
		return intPtr;
	}

	private static void Logoff(IntPtr hwnd, ref IntPtr session)
	{
		if (!(session == IntPtr.Zero))
		{
			MAPILogoff(session, hwnd, 0, 0);
			session = IntPtr.Zero;
		}
	}

	private static bool Logon(IntPtr hwnd, ref IntPtr session)
	{
		if (Environment.Is64BitOperatingSystem)
		{
			return false;
		}
		int num = MAPILogon(hwnd, null, null, 0, 0, ref session);
		if (num != 0)
		{
			num = MAPILogon(hwnd, null, null, 1, 0, ref session);
		}
		return num == 0;
	}

	[DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
	private static extern int _controlfp(int n, int mask);

	[DllImport("MAPI32.DLL", CharSet = CharSet.Ansi)]
	private static extern int MAPILogoff(IntPtr session, IntPtr hwnd, int flags, int reserved);

	[DllImport("MAPI32.DLL", CharSet = CharSet.Ansi)]
	private static extern int MAPILogon(IntPtr hwnd, string profileName, string password, int flags, int reserved, ref IntPtr session);

	[DllImport("MAPI32.DLL", CharSet = CharSet.Ansi)]
	private static extern int MAPISendMail(IntPtr session, IntPtr uiParam, MapiMessage message, int flags, int reserved);
}
