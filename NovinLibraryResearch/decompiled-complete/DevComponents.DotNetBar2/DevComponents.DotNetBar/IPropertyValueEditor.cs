using System;
using System.Drawing;

namespace DevComponents.DotNetBar;

public interface IPropertyValueEditor
{
	Font EditorFont { get; set; }

	bool IsEditorFocused { get; }

	object EditValue { get; set; }

	event EventHandler EditValueChanged;

	void FocusEditor();
}
