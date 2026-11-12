using System;
using System.Drawing.Drawing2D;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class SuperTabGetTabItemPathEventArgs : EventArgs
{
	private SuperTabItem _Tab;

	private GraphicsPath _Path;

	public SuperTabItem Tab => _Tab;

	public GraphicsPath Path
	{
		get
		{
			return _Path;
		}
		set
		{
			_Path = value;
		}
	}

	public SuperTabGetTabItemPathEventArgs(SuperTabItem tab)
	{
		_Tab = tab;
	}
}
