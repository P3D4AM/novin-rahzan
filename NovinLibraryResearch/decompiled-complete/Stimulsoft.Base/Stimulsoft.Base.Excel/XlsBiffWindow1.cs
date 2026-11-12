using System;

namespace Stimulsoft.Base.Excel;

internal class XlsBiffWindow1 : XlsBiffRecord
{
	[Flags]
	public enum Window1Flags : ushort
	{
		Hidden = 1,
		Minimized = 2,
		HScrollVisible = 8,
		VScrollVisible = 0x10,
		WorkbookTabs = 0x20
	}

	public ushort Left => ReadUInt16(0);

	public ushort Top => ReadUInt16(2);

	public ushort Width => ReadUInt16(4);

	public ushort Height => ReadUInt16(6);

	public Window1Flags Flags => (Window1Flags)ReadUInt16(8);

	public ushort ActiveTab => ReadUInt16(10);

	public ushort FirstVisibleTab => ReadUInt16(12);

	public ushort SelectedTabCount => ReadUInt16(14);

	public ushort TabRatio => ReadUInt16(16);

	internal XlsBiffWindow1(byte[] bytes, uint offset)
		: base(bytes, offset)
	{
	}

	internal XlsBiffWindow1(byte[] bytes)
		: this(bytes, 0u)
	{
	}
}
