using System;

namespace Stimulsoft.Report.Controls;

internal struct TVITEMEX
{
	public int mask;

	public IntPtr hItem;

	public int state;

	public int stateMask;

	public string pszText;

	public int cchTextMax;

	public int iImage;

	public int iSelectedImage;

	public int cChildren;

	public IntPtr lParam;

	public int iIntegral;
}
