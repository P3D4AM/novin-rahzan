using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Components;

public class StiRichTextHelper
{
	internal struct STRUCT_CHARRANGE
	{
		public int cpMin;

		public int cpMax;
	}

	internal struct STRUCT_FORMATRANGE
	{
		public IntPtr hdc;

		public IntPtr hdcTarget;

		public RECT rc;

		public RECT rcPage;

		public STRUCT_CHARRANGE chrg;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal struct CHARFORMATSTRUCT
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

	public static int FormatRange(StiRtfFormatType formatType, RichTextBox richTextBox, Rectangle rect, Graphics g, int charFrom, int charTo, out int charEnd)
	{
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		int result = 0;
		STRUCT_CHARRANGE chrg = default(STRUCT_CHARRANGE);
		chrg.cpMin = charFrom;
		chrg.cpMax = ((charTo >= 0 && charTo > charFrom && charTo <= ((TextBoxBase)richTextBox).TextLength) ? charTo : ((TextBoxBase)richTextBox).TextLength);
		RECT val = default(RECT);
		switch (formatType)
		{
		case StiRtfFormatType.TotalRtfHeight:
			val.top = HundredthInchToTwips(0);
			val.left = HundredthInchToTwips(0);
			val.bottom = HundredthInchToTwips(1000000);
			val.right = HundredthInchToTwips(rect.Width);
			break;
		case StiRtfFormatType.MeasureRtf:
			val.top = HundredthInchToTwips(0);
			val.left = HundredthInchToTwips(0);
			val.bottom = HundredthInchToTwips(rect.Height);
			val.right = HundredthInchToTwips(rect.Width);
			break;
		default:
			val.top = HundredthInchToTwips(rect.Top);
			val.left = HundredthInchToTwips(rect.Left);
			val.bottom = HundredthInchToTwips(rect.Bottom);
			val.right = HundredthInchToTwips(rect.Right);
			break;
		}
		RECT rcPage = default(RECT);
		rcPage.top = 0;
		rcPage.left = 0;
		rcPage.bottom = val.bottom;
		rcPage.right = val.right;
		IntPtr hdc = g.GetHdc();
		try
		{
			STRUCT_FORMATRANGE structure = default(STRUCT_FORMATRANGE);
			structure.chrg = chrg;
			structure.hdc = hdc;
			structure.hdcTarget = hdc;
			structure.rc = val;
			structure.rcPage = rcPage;
			int num = ((formatType == StiRtfFormatType.DrawRtf) ? 1 : 0);
			IntPtr intPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(structure));
			Marshal.StructureToPtr(structure, intPtr, fDeleteOld: false);
			charEnd = Win32.SendMessage(((Control)richTextBox).Handle, 1081, num, intPtr);
			if (formatType != 0)
			{
				result = TwipsToHundredthInch(((STRUCT_FORMATRANGE)Marshal.PtrToStructure(intPtr, typeof(STRUCT_FORMATRANGE))).rc.bottom);
			}
			Marshal.FreeCoTaskMem(intPtr);
			IntPtr intPtr2 = new IntPtr(0);
			Win32.SendMessage(((Control)richTextBox).Handle, 1081, 0, intPtr2);
			return result;
		}
		finally
		{
			g.ReleaseHdc(hdc);
		}
	}

	private static int HundredthInchToTwips(int n)
	{
		return (int)((float)(n * 1440) / StiOptions.Engine.RichTextScale);
	}

	private static int TwipsToHundredthInch(int n)
	{
		return (int)((float)n * StiOptions.Engine.RichTextScale / 1440f);
	}
}
