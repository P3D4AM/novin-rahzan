using System;

namespace Stimulsoft.Controls;

public class StiValueChangedEventArgs : EventArgs
{
	private StiTreeView treeView;

	private string text;

	public StiTreeView TreeView
	{
		get
		{
			return treeView;
		}
		set
		{
			treeView = value;
		}
	}

	public string Text
	{
		get
		{
			return text;
		}
		set
		{
			text = value;
		}
	}

	public StiValueChangedEventArgs(StiTreeView treeView, string text)
	{
		TreeView = treeView;
		Text = text;
	}
}
