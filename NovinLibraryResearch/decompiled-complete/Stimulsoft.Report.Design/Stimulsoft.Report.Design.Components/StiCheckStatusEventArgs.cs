using System;

namespace Stimulsoft.Report.Design.Components;

public class StiCheckStatusEventArgs : EventArgs
{
	public int Errors { get; }

	public int Warnings { get; }

	public int InformationMessages { get; }

	public int RenderingMessages { get; }

	public StiCheckStatusEventArgs(int errors, int warnings, int informationMessages, int renderingMessages)
	{
		Errors = errors;
		Warnings = warnings;
		InformationMessages = informationMessages;
		RenderingMessages = renderingMessages;
	}
}
