using System;
using System.Drawing;

namespace Stimulsoft.Editor;

public class CodeCompletionArgs : EventArgs
{
	public ICodeCompletionProvider Provider;

	public char KeyChar;

	public CodeCompletionType CompletionType;

	public int Interval;

	public bool Handled;

	public bool NeedShow;

	public bool ToolTip;

	public int SelIndex = -1;

	public Point StartPosition;

	public Point EndPosition;

	public Point DisplayPosition;

	public bool NeedReparse;

	public void Init()
	{
		CompletionType = CodeCompletionType.None;
		Provider = null;
		KeyChar = '\0';
		ToolTip = false;
		Interval = 0;
		StartPosition = new Point(-1, -1);
		EndPosition = new Point(-1, -1);
		DisplayPosition = new Point(-1, -1);
		Handled = false;
		NeedShow = false;
		NeedReparse = true;
		SelIndex = -1;
	}

	public void Init(CodeCompletionType completionType, Point position, bool needReparse)
	{
		Init();
		CompletionType = completionType;
		StartPosition = position;
		DisplayPosition = position;
		NeedReparse = needReparse;
	}

	public void Init(CodeCompletionType completionType, Point position)
	{
		Init(completionType, position, needReparse: true);
	}
}
