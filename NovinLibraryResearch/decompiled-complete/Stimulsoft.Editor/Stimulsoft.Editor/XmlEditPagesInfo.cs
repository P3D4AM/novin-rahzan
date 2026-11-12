namespace Stimulsoft.Editor;

public class XmlEditPagesInfo
{
	private EditPages owner;

	private string backColor = EditConsts.DefaultPageBackColor.Name;

	private string borderColor = EditConsts.DefaultPageBorderColor.Name;

	private XmlEditPageInfo defaultPage;

	private bool displayWhiteSpace = true;

	private bool applyRulerToAllPages;

	private PageType pageType;

	private RulerOptions rulerOptions = EditConsts.DefaultRulerOptions;

	private EditRulers rulers;

	private RulerUnits rulerUnits = EditConsts.DefaultRulerUnits;

	public string BackColor
	{
		get
		{
			if (owner == null)
			{
				return backColor;
			}
			return XmlHelper.SerializeColor(owner.BackColor);
		}
		set
		{
			backColor = value;
			if (owner != null)
			{
				owner.BackColor = XmlHelper.DeserializeColor(value);
			}
		}
	}

	public string BorderColor
	{
		get
		{
			if (owner == null)
			{
				return borderColor;
			}
			return XmlHelper.SerializeColor(owner.BorderColor);
		}
		set
		{
			borderColor = value;
			if (owner != null)
			{
				owner.BorderColor = XmlHelper.DeserializeColor(value);
			}
		}
	}

	public bool DisplayWhiteSpace
	{
		get
		{
			if (owner == null)
			{
				return displayWhiteSpace;
			}
			return owner.DisplayWhiteSpace;
		}
		set
		{
			displayWhiteSpace = value;
			if (owner != null)
			{
				owner.DisplayWhiteSpace = value;
			}
		}
	}

	public PageType PageType
	{
		get
		{
			if (owner == null)
			{
				return pageType;
			}
			return owner.PageType;
		}
		set
		{
			pageType = value;
			if (owner != null)
			{
				owner.PageType = value;
			}
		}
	}

	public RulerOptions RulerOptions
	{
		get
		{
			if (owner == null)
			{
				return rulerOptions;
			}
			return owner.RulerOptions;
		}
		set
		{
			rulerOptions = value;
			if (owner != null)
			{
				owner.RulerOptions = value;
			}
		}
	}

	public EditRulers Rulers
	{
		get
		{
			if (owner == null)
			{
				return rulers;
			}
			return owner.Rulers;
		}
		set
		{
			rulers = value;
			if (owner != null)
			{
				owner.Rulers = value;
			}
		}
	}

	public RulerUnits RulerUnits
	{
		get
		{
			if (owner == null)
			{
				return rulerUnits;
			}
			return owner.RulerUnits;
		}
		set
		{
			rulerUnits = value;
			if (owner != null)
			{
				owner.RulerUnits = value;
			}
		}
	}

	public XmlEditPageInfo DefaultPage
	{
		get
		{
			if (owner == null)
			{
				return defaultPage;
			}
			return (XmlEditPageInfo)((EditPage)owner.DefaultPage).XmlInfo;
		}
		set
		{
			defaultPage = value;
			if (owner != null && value != null)
			{
				((EditPage)owner.DefaultPage).XmlInfo = value;
			}
		}
	}

	public bool ApplyRulerToAllPages
	{
		get
		{
			if (owner == null)
			{
				return applyRulerToAllPages;
			}
			return owner.ApplyRulerToAllPages;
		}
		set
		{
			applyRulerToAllPages = value;
			if (owner != null)
			{
				owner.ApplyRulerToAllPages = value;
			}
		}
	}

	public XmlEditPagesInfo()
	{
	}

	public XmlEditPagesInfo(EditPages owner)
		: this()
	{
		this.owner = owner;
	}

	public virtual void Load()
	{
		if (owner != null)
		{
			backColor = BackColor;
			borderColor = BorderColor;
			defaultPage = DefaultPage;
			displayWhiteSpace = DisplayWhiteSpace;
			pageType = PageType;
			rulerOptions = RulerOptions;
			rulers = Rulers;
			rulerUnits = RulerUnits;
			applyRulerToAllPages = ApplyRulerToAllPages;
			if (defaultPage != null)
			{
				defaultPage.Load();
			}
		}
	}

	public virtual void FixupReferences(EditPages owner)
	{
		this.owner = owner;
		BackColor = backColor;
		BorderColor = borderColor;
		DefaultPage = defaultPage;
		DisplayWhiteSpace = displayWhiteSpace;
		PageType = pageType;
		RulerOptions = rulerOptions;
		Rulers = rulers;
		RulerUnits = rulerUnits;
		ApplyRulerToAllPages = applyRulerToAllPages;
	}
}
