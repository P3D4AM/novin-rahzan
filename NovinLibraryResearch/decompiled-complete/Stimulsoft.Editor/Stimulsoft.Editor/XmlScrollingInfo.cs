using System.Windows.Forms;

namespace Stimulsoft.Editor;

public class XmlScrollingInfo
{
	private Scrolling owner;

	private int windowOriginX;

	private int windowOriginY;

	private RichTextBoxScrollBars scrollBars = (RichTextBoxScrollBars)3;

	private int defaultHorzScrollSize = EditConsts.DefaultHorzScrollSize;

	private ScrollingOptions options;

	private ScrollingButtons horzButtons = new ScrollingButtons(null);

	private ScrollingButtons vertButtons = new ScrollingButtons(null);

	public int WindowOriginX
	{
		get
		{
			if (owner == null)
			{
				return windowOriginX;
			}
			return owner.WindowOriginX;
		}
		set
		{
			windowOriginX = value;
			if (owner != null)
			{
				owner.WindowOriginX = value;
			}
		}
	}

	public int WindowOriginY
	{
		get
		{
			if (owner == null)
			{
				return windowOriginY;
			}
			return owner.WindowOriginY;
		}
		set
		{
			windowOriginY = value;
			if (owner != null)
			{
				owner.WindowOriginY = value;
			}
		}
	}

	public RichTextBoxScrollBars ScrollBars
	{
		get
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			if (owner == null)
			{
				return scrollBars;
			}
			return owner.ScrollBars;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			scrollBars = value;
			if (owner != null)
			{
				owner.ScrollBars = value;
			}
		}
	}

	public int DefaultHorzScrollSize
	{
		get
		{
			if (owner == null)
			{
				return defaultHorzScrollSize;
			}
			return owner.DefaultHorzScrollSize;
		}
		set
		{
			defaultHorzScrollSize = value;
			if (owner != null)
			{
				owner.DefaultHorzScrollSize = value;
			}
		}
	}

	public ScrollingOptions Options
	{
		get
		{
			if (owner == null)
			{
				return EditConsts.DefaultScrollingOptions;
			}
			return owner.Options;
		}
		set
		{
			options = value;
			if (owner != null)
			{
				owner.Options = value;
			}
		}
	}

	public ScrollingButtons HorzButtons
	{
		get
		{
			if (owner == null)
			{
				return horzButtons;
			}
			return owner.HorizontalButtons;
		}
		set
		{
			if (horzButtons != value)
			{
				horzButtons.Assign(value);
			}
			if (owner != null)
			{
				owner.HorizontalButtons.Assign(value);
			}
		}
	}

	public ScrollingButtons VertButtons
	{
		get
		{
			if (owner == null)
			{
				return vertButtons;
			}
			return owner.VerticalButtons;
		}
		set
		{
			if (vertButtons != value)
			{
				vertButtons.Assign(value);
			}
			if (owner != null)
			{
				owner.VerticalButtons.Assign(value);
			}
		}
	}

	public XmlScrollingInfo()
	{
	}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


	public XmlScrollingInfo(Scrolling owner)
		: this()
	{
		this.owner = owner;
	}

	public virtual void Load()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		if (owner != null)
		{
			windowOriginX = WindowOriginX;
			windowOriginY = WindowOriginY;
			scrollBars = ScrollBars;
			defaultHorzScrollSize = DefaultHorzScrollSize;
			options = Options;
			horzButtons.Assign(HorzButtons);
			vertButtons.Assign(VertButtons);
		}
	}

	public virtual void FixupReferences(Scrolling owner)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		this.owner = owner;
		WindowOriginX = windowOriginX;
		WindowOriginY = windowOriginY;
		ScrollBars = scrollBars;
		DefaultHorzScrollSize = defaultHorzScrollSize;
		Options = options;
		HorzButtons = horzButtons;
		VertButtons = vertButtons;
	}
}
