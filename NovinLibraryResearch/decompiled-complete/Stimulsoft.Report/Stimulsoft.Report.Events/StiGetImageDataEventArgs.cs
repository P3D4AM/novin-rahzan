using System;
using System.Drawing;
using Stimulsoft.Base.Blocks;

namespace Stimulsoft.Report.Events;

public class StiGetImageDataEventArgs : EventArgs, IStiBlocklyValueEventArgs
{
	public virtual Image Value { get; set; }

	public StiGetImageDataEventArgs()
		: this(null)
	{
	}

	public StiGetImageDataEventArgs(Image image)
	{
		Value = image;
	}
}
