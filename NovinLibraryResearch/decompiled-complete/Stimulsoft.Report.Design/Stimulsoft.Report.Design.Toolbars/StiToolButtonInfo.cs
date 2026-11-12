using System;
using System.Windows.Forms;

namespace Stimulsoft.Report.Design.Toolbars;

public class StiToolButtonInfo
{
	public ImageList ImageList { get; }

	public int ImageIndex { get; }

	public int ButtonIndex { get; }

	public EventHandler EventHandler { get; }

	public string Text { get; }

	public StiToolButtonInfo(ImageList imageList, int imageIndex, EventHandler eventHandler)
		: this(imageList, imageIndex, string.Empty, eventHandler)
	{
	}

	public StiToolButtonInfo(string text, EventHandler eventHandler)
		: this(null, -1, text, eventHandler)
	{
	}

	public StiToolButtonInfo(ImageList imageList, int imageIndex, string text, EventHandler eventHandler)
		: this(imageList, imageIndex, text, eventHandler, -1)
	{
	}

	public StiToolButtonInfo(ImageList imageList, int imageIndex, string text, EventHandler eventHandler, int buttonIndex)
	{
		ImageList = imageList;
		ImageIndex = imageIndex;
		Text = text;
		EventHandler = eventHandler;
		ButtonIndex = buttonIndex;
	}
}
