using System.Drawing;

namespace Stimulsoft.Editor;

public class XmlBracesInfo
{
	private char[] openBraces = EditConsts.DefaultOpenBraces;

	private char[] closingBraces = EditConsts.DefaultClosingBraces;

	private BracesOptions bracesOptions;

	private string foreColor = EditConsts.DefaultBracesForeColor.Name;

	private string backColor = string.Empty;

	private FontStyle style = EditConsts.DefaultBracesFontStyle;

	private bool useRoundRect;

	private Braces owner;

	public char[] OpenBraces
	{
		get
		{
			if (owner == null)
			{
				return openBraces;
			}
			return owner.OpenBraces;
		}
		set
		{
			openBraces = value;
			if (owner != null)
			{
				owner.OpenBraces = value;
			}
		}
	}

	public char[] ClosingBraces
	{
		get
		{
			if (owner == null)
			{
				return closingBraces;
			}
			return owner.ClosingBraces;
		}
		set
		{
			closingBraces = value;
			if (owner != null)
			{
				owner.ClosingBraces = value;
			}
		}
	}

	public BracesOptions BracesOptions
	{
		get
		{
			if (owner == null)
			{
				return bracesOptions;
			}
			return owner.BracesOptions;
		}
		set
		{
			bracesOptions = value;
			if (owner != null)
			{
				owner.BracesOptions = value;
			}
		}
	}

	public string ForeColor
	{
		get
		{
			if (owner == null)
			{
				return foreColor;
			}
			return XmlHelper.SerializeColor(owner.ForeColor);
		}
		set
		{
			foreColor = value;
			if (owner != null)
			{
				owner.ForeColor = XmlHelper.DeserializeColor(value);
			}
		}
	}

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

	public FontStyle Style
	{
		get
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			if (owner == null)
			{
				return style;
			}
			return owner.Style;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			style = value;
			if (owner != null)
			{
				owner.Style = value;
			}
		}
	}

	public bool UseRoundRect
	{
		get
		{
			if (owner == null)
			{
				return useRoundRect;
			}
			return owner.UseRoundRect;
		}
		set
		{
			useRoundRect = value;
			if (owner != null)
			{
				owner.UseRoundRect = value;
			}
		}
	}

	public XmlBracesInfo()
	{
	}//IL_0032: Unknown result type (might be due to invalid IL or missing references)
	//IL_0037: Unknown result type (might be due to invalid IL or missing references)


	public XmlBracesInfo(Braces owner)
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
			openBraces = OpenBraces;
			closingBraces = ClosingBraces;
			bracesOptions = BracesOptions;
			foreColor = ForeColor;
			backColor = BackColor;
			style = Style;
			useRoundRect = UseRoundRect;
		}
	}

	public virtual void FixupReferences(Braces owner)
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		this.owner = owner;
		OpenBraces = openBraces;
		ClosingBraces = closingBraces;
		BracesOptions = bracesOptions;
		ForeColor = foreColor;
		BackColor = backColor;
		Style = style;
		UseRoundRect = useRoundRect;
	}
}
