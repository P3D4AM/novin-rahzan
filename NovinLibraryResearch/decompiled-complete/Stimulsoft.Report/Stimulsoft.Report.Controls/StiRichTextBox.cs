using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Controls;

[ToolboxItem(false)]
[SuppressUnmanagedCodeSecurity]
public class StiRichTextBox : RichTextBox
{
	private struct PARAFORMAT
	{
		public int cbSize;

		public uint dwMask;

		public short wNumbering;

		public short wReserved;

		public int dxStartIndent;

		public int dxRightIndent;

		public int dxOffset;

		public short wAlignment;

		public short cTabCount;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		public int[] rgxTabs;

		public int dySpaceBefore;

		public int dySpaceAfter;

		public int dyLineSpacing;

		public short sStyle;

		public byte bLineSpacingRule;

		public byte bOutlineLevel;

		public short wShadingWeight;

		public short wShadingStyle;

		public short wNumberingStart;

		public short wNumberingStyle;

		public short wNumberingTab;

		public short wBorderSpace;

		public short wBorderWidth;

		public short wBorders;
	}

	private struct CHARFORMATSTRUCT
	{
		public int cbSize;

		public uint dwMask;

		public uint dwEffects;

		public int yHeight;

		public int yOffset;

		public int crTextColor;

		public byte bCharSet;

		public byte bPitchAndFamily;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		public char[] szFaceName;
	}

	private static IntPtr moduleHandle;

	private static bool failLoadModule;

	private static string lastCheckedClassName;

	internal bool DrawTransparent;

	private const int EM_SETEVENTMASK = 1073;

	private const int EM_GETPARAFORMAT = 1085;

	private const int EM_SETPARAFORMAT = 1095;

	private const int EM_SETTYPOGRAPHYOPTIONS = 1226;

	private const int WM_SETREDRAW = 11;

	private const int TO_ADVANCEDTYPOGRAPHY = 1;

	private const int PFM_ALIGNMENT = 8;

	private const int SCF_SELECTION = 1;

	private const int SCF_WORD = 2;

	private const int SCF_ALL = 4;

	private const uint CFM_BOLD = 1u;

	private const uint CFM_ITALIC = 2u;

	private const uint CFM_UNDERLINE = 4u;

	private const uint CFM_STRIKEOUT = 8u;

	private const uint CFM_PROTECTED = 16u;

	private const uint CFM_LINK = 32u;

	private const uint CFM_SIZE = 2147483648u;

	private const uint CFM_COLOR = 1073741824u;

	private const uint CFM_FACE = 536870912u;

	private const uint CFM_OFFSET = 268435456u;

	private const uint CFM_CHARSET = 134217728u;

	private const uint CFE_BOLD = 1u;

	private const uint CFE_ITALIC = 2u;

	private const uint CFE_UNDERLINE = 4u;

	private const uint CFE_STRIKEOUT = 8u;

	private const uint CFE_PROTECTED = 16u;

	private const uint CFE_LINK = 32u;

	private const uint CFE_AUTOCOLOR = 1073741824u;

	protected override CreateParams CreateParams
	{
		get
		{
			CreateParams createParams = ((RichTextBox)this).CreateParams;
			if (StiOptions.Engine.ForceLoadExtendedRichTextLibrary.HasValue && !StiOptions.Engine.ForceLoadExtendedRichTextLibrary.Value)
			{
				return createParams;
			}
			if ((DrawTransparent || StiOptions.Engine.ForceLoadExtendedRichTextLibrary.GetValueOrDefault(false)) && TryLoadLibrary())
			{
				if (DrawTransparent)
				{
					createParams.ExStyle |= 0x20;
				}
				createParams.ClassName = StiOptions.Engine.ExtendedRichTextLibraryClassName;
			}
			return createParams;
		}
	}

	public StiRtfSelectionAlign SelectionAlignment
	{
		get
		{
			PARAFORMAT lp = default(PARAFORMAT);
			lp.cbSize = Marshal.SizeOf(lp);
			SendMessage(new HandleRef(this, ((Control)this).Handle), 1085, 1, ref lp);
			if ((lp.dwMask & 8) == 0)
			{
				return StiRtfSelectionAlign.Left;
			}
			return (StiRtfSelectionAlign)lp.wAlignment;
		}
		set
		{
			PARAFORMAT lp = default(PARAFORMAT);
			lp.cbSize = Marshal.SizeOf(lp);
			lp.dwMask = 8u;
			lp.wAlignment = (short)value;
			SendMessage(new HandleRef(this, ((Control)this).Handle), 1095, 1, ref lp);
		}
	}

	[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
	private static extern IntPtr LoadLibrary(string lpFileName);

	private static bool TryLoadLibrary()
	{
		if (!failLoadModule && moduleHandle == IntPtr.Zero)
		{
			try
			{
				moduleHandle = LoadLibrary("msftedit.dll");
			}
			catch
			{
				failLoadModule = true;
			}
		}
		return moduleHandle != IntPtr.Zero;
	}

	private void CheckClassName()
	{
		if (!(lastCheckedClassName != StiOptions.Engine.ExtendedRichTextLibraryClassName))
		{
			return;
		}
		try
		{
			lastCheckedClassName = StiOptions.Engine.ExtendedRichTextLibraryClassName;
			((RichTextBox)this).Rtf = string.Empty;
			((RichTextBox)this).Rtf = null;
		}
		catch (Win32Exception)
		{
			if (StiOptions.Engine.ExtendedRichTextLibraryClassName.ToLowerInvariant() == "richedit20w")
			{
				StiOptions.Engine.ForceLoadExtendedRichTextLibrary = false;
			}
			else if (StiOptions.Engine.ExtendedRichTextLibraryClassName.ToLowerInvariant() == "richedit50w")
			{
				StiOptions.Engine.ExtendedRichTextLibraryClassName = "RichEdit20W";
			}
			else
			{
				StiOptions.Engine.ExtendedRichTextLibraryClassName = "RichEdit50W";
			}
		}
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		((RichTextBox)this).OnHandleCreated(e);
		SendMessage(new HandleRef(this, ((Control)this).Handle), 1226, 1, 1);
	}

	[DllImport("user32", CharSet = CharSet.Auto)]
	private static extern int SendMessage(HandleRef hWnd, int msg, int wParam, int lParam);

	[DllImport("user32", CharSet = CharSet.Auto)]
	private static extern int SendMessage(HandleRef hWnd, int msg, int wParam, ref PARAFORMAT lp);

	[DllImport("user32", CharSet = CharSet.Auto)]
	private static extern int SendMessage(HandleRef hWnd, int msg, int wParam, ref CHARFORMATSTRUCT lp);

	public bool SetSelectionFont(string face)
	{
		CHARFORMATSTRUCT structure = default(CHARFORMATSTRUCT);
		structure.cbSize = Marshal.SizeOf(structure);
		structure.szFaceName = new char[32];
		structure.dwMask = 536870912u;
		face.CopyTo(0, structure.szFaceName, 0, Math.Min(31, face.Length));
		IntPtr intPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(structure));
		Marshal.StructureToPtr(structure, intPtr, fDeleteOld: false);
		int num = Win32.SendMessage(((Control)this).Handle, 1092, 1, intPtr);
		return num == 0;
	}

	public bool SetSelectionSize(int size)
	{
		CHARFORMATSTRUCT structure = default(CHARFORMATSTRUCT);
		structure.cbSize = Marshal.SizeOf(structure);
		structure.dwMask = 2147483648u;
		structure.yHeight = size * 20;
		IntPtr intPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(structure));
		Marshal.StructureToPtr(structure, intPtr, fDeleteOld: false);
		int num = Win32.SendMessage(((Control)this).Handle, 1092, 1, intPtr);
		return num == 0;
	}

	public bool SetSelectionBold(bool bold)
	{
		return SetSelectionStyle(1u, bold ? 1u : 0u);
	}

	public bool SetSelectionItalic(bool italic)
	{
		return SetSelectionStyle(2u, italic ? 2u : 0u);
	}

	public bool SetSelectionUnderlined(bool underlined)
	{
		return SetSelectionStyle(4u, underlined ? 4u : 0u);
	}

	private bool SetSelectionStyle(uint mask, uint effect)
	{
		CHARFORMATSTRUCT structure = default(CHARFORMATSTRUCT);
		structure.cbSize = Marshal.SizeOf(structure);
		structure.dwMask = mask;
		structure.dwEffects = effect;
		IntPtr intPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(structure));
		Marshal.StructureToPtr(structure, intPtr, fDeleteOld: false);
		int num = Win32.SendMessage(((Control)this).Handle, 1092, 1, intPtr);
		return num == 0;
	}

	public StiRichTextBox(bool drawTransparent)
	{
		DrawTransparent = drawTransparent;
		CheckClassName();
	}
}
