using System;
using System.Drawing.Drawing2D;

namespace DevComponents.DotNetBar;

public class SuperTabGetTabItemPathEventArgs : EventArgs
{
	private SuperTabItem _Ὴ;

	private GraphicsPath _ῑ;

	public SuperTabItem Tab => _Ὴ;

	public GraphicsPath Path
	{
		get
		{
			return _ῑ;
		}
		set
		{
			_ῑ = value;
		}
	}

	public SuperTabGetTabItemPathEventArgs(SuperTabItem tab)
	{
		_Ὴ = tab;
	}
}
