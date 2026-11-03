using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DevComponents.AdvTree.Interop;

internal class _0EFE
{
	private struct _17BB
	{
		public int _17BC;

		public uint _17BD;

		public int _17BE;

		public IntPtr _17BF;
	}

	private const uint _08D2 = 1u;

	private const uint _08D3 = 2u;

	private const uint _08D4 = 16u;

	private const uint _08D5 = 1073741824u;

	private const uint _08D6 = 2147483648u;

	private const uint _08D7 = uint.MaxValue;

	[DllImport("user32")]
	private static extern bool TrackMouseEvent(ref _17BB tme);

	public static void ఘ(Control _0BDE)
	{
		if (_0BDE != null && _0BDE.IsHandleCreated)
		{
			_17BB tme = default(_17BB);
			tme._17BD = 1073741824u;
			tme._17BF = _0BDE.Handle;
			tme._17BC = Marshal.SizeOf((object)tme);
			TrackMouseEvent(ref tme);
			tme._17BD |= 1u;
			TrackMouseEvent(ref tme);
		}
	}
}
