using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;

namespace DevComponents.DotNetBar;

[ToolboxItem(false)]
[TypeConverter(typeof(ExpandableObjectConverter))]
public class TabColors
{
	private EventHandler ᗆ;

	private Color ފ = Color.Empty;

	private Color ތ = Color.Empty;

	private int ގ = 90;

	private Color ᕲ = Color.Empty;

	private Color ᕳ = Color.Empty;

	private Color _07A8 = Color.Empty;

	private Color ޗ = Color.Empty;

	private BackgroundColorBlendCollection _283E = new BackgroundColorBlendCollection();

	[Description("Indicates the inactive tab background color.")]
	[Browsable(true)]
	[Category("Style")]
	public Color BackColor
	{
		get
		{
			return ފ;
		}
		set
		{
			ފ = value;
			ᇌ();
		}
	}

	[Description("Indicates the inactive tab target gradient background color.")]
	[Browsable(true)]
	[Category("Style")]
	public Color BackColor2
	{
		get
		{
			return ތ;
		}
		set
		{
			ތ = value;
			ᇌ();
		}
	}

	[Browsable(true)]
	[DefaultValue(90)]
	[Category("Style")]
	[Description("Indicates the gradient angle.")]
	public int BackColorGradientAngle
	{
		get
		{
			return ގ;
		}
		set
		{
			ގ = value;
			ᇌ();
		}
	}

	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Collection that defines the multicolor gradient background.")]
	public BackgroundColorBlendCollection BackgroundColorBlend => _283E;

	[Category("Style")]
	[Description("Indicates the inactive tab light border color.")]
	[Browsable(true)]
	public Color LightBorderColor
	{
		get
		{
			return ᕲ;
		}
		set
		{
			ᕲ = value;
			ᇌ();
		}
	}

	[Category("Style")]
	[Description("Indicates the inactive tab dark border color.")]
	[Browsable(true)]
	public Color DarkBorderColor
	{
		get
		{
			return ᕳ;
		}
		set
		{
			ᕳ = value;
			ᇌ();
		}
	}

	[Category("Style")]
	[Description("Indicates the inactive tab border color.")]
	[Browsable(true)]
	public Color BorderColor
	{
		get
		{
			return _07A8;
		}
		set
		{
			_07A8 = value;
			ᇌ();
		}
	}

	[Description("Indicates the inactive tab text color.")]
	[Category("Style")]
	[Browsable(true)]
	public Color TextColor
	{
		get
		{
			return ޗ;
		}
		set
		{
			ޗ = value;
			ᇌ();
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

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBackColor()
	{
		return !ފ.IsEmpty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackColor()
	{
		BackColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public bool ShouldSerializeBackColor2()
	{
		return !ތ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public void ResetBackColor2()
	{
		BackColor2 = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public bool ShouldSerializeLightBorderColor()
	{
		return !ᕲ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public void ResetLightBorderColor()
	{
		LightBorderColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public bool ShouldSerializeDarkBorderColor()
	{
		return !ᕳ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public void ResetDarkBorderColor()
	{
		DarkBorderColor = Color.Empty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBorderColor()
	{
		return !_07A8.IsEmpty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBorderColor()
	{
		BorderColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public bool ShouldSerializeTextColor()
	{
		return !ޗ.IsEmpty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTextColor()
	{
		TextColor = Color.Empty;
	}

	private void ᇌ()
	{
		if (ᗆ != null)
		{
			ᗆ(this, new EventArgs());
		}
	}
}
