using System;
using System.ComponentModel;
using System.Drawing;

namespace Stimulsoft.Controls.Win.Editors;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
[TypeConverter(typeof(ExpandableObjectConverter))]
public class InputControlColors
{
	private Color _Highlight = Color.Empty;

	private Color _HighlightText = Color.Empty;

	public Color Highlight
	{
		get
		{
			return _Highlight;
		}
		set
		{
			if (_Highlight != value)
			{
				_Highlight = value;
				OnColorChanged();
			}
		}
	}

	public Color HighlightText
	{
		get
		{
			return _HighlightText;
		}
		set
		{
			if (_HighlightText != value)
			{
				_HighlightText = value;
				OnColorChanged();
			}
		}
	}

	public event EventHandler ColorChanged;

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetHighlight()
	{
		Highlight = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeHighlight()
	{
		return !Highlight.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetHighlightText()
	{
		HighlightText = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeHighlightText()
	{
		return !HighlightText.IsEmpty;
	}

	private void OnColorChanged()
	{
		if (this.ColorChanged != null)
		{
			this.ColorChanged(this, new EventArgs());
		}
	}
}
