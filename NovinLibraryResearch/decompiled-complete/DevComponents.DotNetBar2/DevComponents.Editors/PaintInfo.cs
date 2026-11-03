using System.Drawing;

namespace DevComponents.Editors;

public class PaintInfo
{
	private Graphics __187B;

	private Point __187C;

	private Font __187D;

	private Color __187E = SystemColors.ControlText;

	private Color _ᛋ = SystemColors.ControlDark;

	private bool _ᚿ;

	private Font _ᠣ;

	private Color _ᠤ = Color.Empty;

	private Size __187F;

	private bool _ᢀ = true;

	private bool _ኗ;

	private InputControlColors _ቿ;

	private bool _ᢁ;

	public Graphics Graphics
	{
		get
		{
			return __187B;
		}
		set
		{
			__187B = value;
		}
	}

	public Point RenderOffset
	{
		get
		{
			return __187C;
		}
		set
		{
			__187C = value;
		}
	}

	public Font DefaultFont
	{
		get
		{
			return __187D;
		}
		set
		{
			__187D = value;
		}
	}

	public Color ForeColor
	{
		get
		{
			return __187E;
		}
		set
		{
			__187E = value;
		}
	}

	public Color DisabledForeColor
	{
		get
		{
			return _ᛋ;
		}
		set
		{
			_ᛋ = value;
		}
	}

	public bool WatermarkEnabled
	{
		get
		{
			return _ᚿ;
		}
		set
		{
			_ᚿ = value;
		}
	}

	public Font WatermarkFont
	{
		get
		{
			return _ᠣ;
		}
		set
		{
			_ᠣ = value;
		}
	}

	public Color WatermarkColor
	{
		get
		{
			return _ᠤ;
		}
		set
		{
			_ᠤ = value;
		}
	}

	public Size AvailableSize
	{
		get
		{
			return __187F;
		}
		set
		{
			__187F = value;
		}
	}

	public bool ParentEnabled
	{
		get
		{
			return _ᢀ;
		}
		set
		{
			_ᢀ = value;
		}
	}

	public bool MouseOver
	{
		get
		{
			return _ኗ;
		}
		set
		{
			_ኗ = value;
		}
	}

	public InputControlColors Colors
	{
		get
		{
			return _ቿ;
		}
		set
		{
			_ቿ = value;
		}
	}

	public bool RenderSystemItemsOnly
	{
		get
		{
			return _ᢁ;
		}
		set
		{
			_ᢁ = value;
		}
	}
}
