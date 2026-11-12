using System;
using System.Drawing;

namespace Stimulsoft.Editor;

public interface IEditRuler : IControl
{
	Color IndentBackColor { get; set; }

	bool Vertical { get; set; }

	int PageStart { get; set; }

	int PageWidth { get; set; }

	int RulerStart { get; set; }

	int RulerWidth { get; set; }

	int MarkWidth { get; set; }

	RulerUnits Units { get; set; }

	RulerOptions Options { get; set; }

	bool IsDragging { get; }

	event EventHandler Change;

	void Assign(IEditRuler source);

	void CancelDragging();

	void ResetOptions();

	void ResetUnits();

	void ResetIndentBackColor();
}
