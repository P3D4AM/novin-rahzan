using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;

namespace DevComponents.Editors;

[ToolboxItem(false)]
[TypeConverter(typeof(ExpandableObjectConverter))]
[DesignTimeVisible(false)]
public class InputControlColors
{
	private EventHandler ᗆ;

	private Color _ᢂ = Color.Empty;

	private Color _ᢃ = Color.Empty;

	public Color Highlight
	{
		get
		{
			return _ᢂ;
		}
		set
		{
			if (_ᢂ != value)
			{
				_ᢂ = value;
				ᢄ();
			}
		}
	}

	public Color HighlightText
	{
		get
		{
			return _ᢃ;
		}
		set
		{
			if (_ᢃ != value)
			{
				_ᢃ = value;
				ᢄ();
			}
		}
	}

	public event EventHandler ColorChanged
	{
		add
		{
			EventHandler eventHandler = ᗆ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᗆ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ᗆ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᗆ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

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

	private void ᢄ()
	{
		if (ᗆ != null)
		{
			ᗆ(this, new EventArgs());
		}
	}
}
