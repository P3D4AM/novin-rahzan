using System.Drawing;

namespace Stimulsoft.Controls.Win.Editors;

public class PaintInfo
{
	private Graphics _Graphics;

	private Point _RenderOffset;

	private Font _DefaultFont;

	private Color _ForeColor = SystemColors.ControlText;

	private Color _DisabledForeColor = SystemColors.ControlDark;

	private bool _WatermarkEnabled;

	private Font _WatermarkFont;

	private Color _WatermarkColor = Color.Empty;

	private Size _AvailableSize;

	private bool _ParentEnabled = true;

	private bool _MouseOver;

	private InputControlColors _Colors;

	private bool _RenderSystemItemsOnly;

	public Graphics Graphics
	{
		get
		{
			return _Graphics;
		}
		set
		{
			_Graphics = value;
		}
	}

	public Point RenderOffset
	{
		get
		{
			return _RenderOffset;
		}
		set
		{
			_RenderOffset = value;
		}
	}

	public Font DefaultFont
	{
		get
		{
			return _DefaultFont;
		}
		set
		{
			_DefaultFont = value;
		}
	}

	public Color ForeColor
	{
		get
		{
			return _ForeColor;
		}
		set
		{
			_ForeColor = value;
		}
	}

	public Color DisabledForeColor
	{
		get
		{
			return _DisabledForeColor;
		}
		set
		{
			_DisabledForeColor = value;
		}
	}

	public bool WatermarkEnabled
	{
		get
		{
			return _WatermarkEnabled;
		}
		set
		{
			_WatermarkEnabled = value;
		}
	}

	public Font WatermarkFont
	{
		get
		{
			return _WatermarkFont;
		}
		set
		{
			_WatermarkFont = value;
		}
	}

	public Color WatermarkColor
	{
		get
		{
			return _WatermarkColor;
		}
		set
		{
			_WatermarkColor = value;
		}
	}

	public Size AvailableSize
	{
		get
		{
			return _AvailableSize;
		}
		set
		{
			_AvailableSize = value;
		}
	}

	public bool ParentEnabled
	{
		get
		{
			return _ParentEnabled;
		}
		set
		{
			_ParentEnabled = value;
		}
	}

	public bool MouseOver
	{
		get
		{
			return _MouseOver;
		}
		set
		{
			_MouseOver = value;
		}
	}

	public InputControlColors Colors
	{
		get
		{
			return _Colors;
		}
		set
		{
			_Colors = value;
		}
	}

	public bool RenderSystemItemsOnly
	{
		get
		{
			return _RenderSystemItemsOnly;
		}
		set
		{
			_RenderSystemItemsOnly = value;
		}
	}
}
