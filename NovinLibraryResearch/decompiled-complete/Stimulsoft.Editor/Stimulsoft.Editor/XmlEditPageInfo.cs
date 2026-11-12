using System.Drawing;
using System.Drawing.Printing;

namespace Stimulsoft.Editor;

public class XmlEditPageInfo
{
	private EditPage owner;

	private XmlPageHeaderInfo footer;

	private XmlPageHeaderInfo header;

	private int horzOffset = EditConsts.DefaultPageHorzOffset;

	private int vertOffset = EditConsts.DefaultPageVertOffset;

	private Margins margins;

	private Size pageSize;

	private PaperKind pageKind;

	private bool landscape;

	private bool paintNumber = true;

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

	public int HorzOffset
	{
		get
		{
			if (owner == null)
			{
				return horzOffset;
			}
			return owner.HorzOffset;
		}
		set
		{
			horzOffset = value;
			if (owner != null)
			{
				owner.HorzOffset = value;
			}
		}
	}

	public int VertOffset
	{
		get
		{
			if (owner == null)
			{
				return vertOffset;
			}
			return owner.VertOffset;
		}
		set
		{
			vertOffset = value;
			if (owner != null)
			{
				owner.VertOffset = value;
			}
		}
	}

	public Margins Margins
	{
		get
		{
			if (owner == null)
			{
				return margins;
			}
			return owner.Margins;
		}
		set
		{
			margins = value;
			if (owner != null)
			{
				owner.Margins = value;
			}
		}
	}

	public Size PageSize
	{
		get
		{
			if (owner == null)
			{
				return pageSize;
			}
			return owner.PageSize;
		}
		set
		{
			pageSize = value;
			if (owner != null)
			{
				owner.PageSize = value;
			}
		}
	}

	public PaperKind PageKind
	{
		get
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			if (owner == null)
			{
				return pageKind;
			}
			return owner.PageKind;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			pageKind = value;
			if (owner != null)
			{
				owner.PageKind = value;
			}
		}
	}

	public bool Landscape
	{
		get
		{
			if (owner == null)
			{
				return landscape;
			}
			return owner.Landscape;
		}
		set
		{
			landscape = value;
			if (owner != null)
			{
				owner.Landscape = value;
			}
		}
	}

	public bool PaintNumber
	{
		get
		{
			if (owner == null)
			{
				return paintNumber;
			}
			return owner.PaintNumber;
		}
		set
		{
			paintNumber = value;
			if (owner != null)
			{
				owner.PaintNumber = value;
			}
		}
	}

	public XmlEditPageInfo()
	{
	}

	public XmlEditPageInfo(EditPage owner)
		: this()
	{
		this.owner = owner;
	}

	public virtual void Load()
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		if (owner != null)
		{
			footer = Footer;
			header = Header;
			horzOffset = HorzOffset;
			landscape = Landscape;
			margins = Margins;
			pageKind = PageKind;
			pageSize = PageSize;
			paintNumber = PaintNumber;
			vertOffset = VertOffset;
			if (footer != null)
			{
				footer.Load();
			}
			if (header != null)
			{
				header.Load();
			}
		}
	}

	public virtual void FixupReferences(EditPage owner)
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		this.owner = owner;
		Footer = footer;
		Header = header;
		HorzOffset = horzOffset;
		Landscape = landscape;
		Margins = margins;
		PageKind = pageKind;
		PageSize = pageSize;
		PaintNumber = paintNumber;
		VertOffset = vertOffset;
	}
}
