namespace Stimulsoft.Editor;

public class XmlPrintingInfo
{
	private Printing owner;

	private PrintOptions options;

	private PrintOptions allowedOptions;

	private XmlPageHeaderInfo footer;

	private XmlPageHeaderInfo header;

	public XmlPageHeaderInfo Footer
	{
		get
		{
			if (owner == null)
			{
				return footer;
			}
			return (XmlPageHeaderInfo)((PageHeader)owner.Footer).XmlInfo;
		}
		set
		{
			footer = value;
			if (owner != null && value != null)
			{
				((PageHeader)owner.Footer).XmlInfo = value;
			}
		}
	}

	public XmlPageHeaderInfo Header
	{
		get
		{
			if (owner == null)
			{
				return header;
			}
			return (XmlPageHeaderInfo)((PageHeader)owner.Header).XmlInfo;
		}
		set
		{
			header = value;
			if (owner != null && value != null)
			{
				((PageHeader)owner.Header).XmlInfo = value;
			}
		}
	}

	public PrintOptions Options
	{
		get
		{
			if (owner == null)
			{
				return options;
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

	public PrintOptions AllowedOptions
	{
		get
		{
			if (owner == null)
			{
				return allowedOptions;
			}
			return owner.AllowedOptions;
		}
		set
		{
			allowedOptions = value;
			if (owner != null)
			{
				owner.AllowedOptions = value;
			}
		}
	}

	public XmlPrintingInfo()
	{
		options = EditConsts.DefaultPrintOptions;
		allowedOptions = EditConsts.DefaultPrintOptions;
	}

	public XmlPrintingInfo(Printing owner)
		: this()
	{
		this.owner = owner;
	}

	public virtual void Load()
	{
		if (owner != null)
		{
			options = Options;
			allowedOptions = AllowedOptions;
			header = Header;
			footer = Footer;
			if (header != null)
			{
				header.Load();
			}
			if (footer != null)
			{
				footer.Load();
			}
		}
	}

	public virtual void FixupReferences(Printing owner)
	{
		this.owner = owner;
		Options = options;
		AllowedOptions = allowedOptions;
		Header = header;
		Footer = footer;
	}
}
