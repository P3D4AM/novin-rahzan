using System.Drawing;

namespace Stimulsoft.Editor;

public class XmlHyperTextExInfo
{
	private HyperTextEx owner;

	private FontStyle urlStyle = EditConsts.DefaultUrlFontStyle;

	private string urlColor = EditConsts.DefaultUrlForeColor.Name;

	public FontStyle UrlStyle
	{
		get
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			if (owner == null)
			{
				return urlStyle;
			}
			return owner.UrlStyle;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			urlStyle = value;
			if (owner != null)
			{
				owner.UrlStyle = value;
			}
		}
	}

	public string UrlColor
	{
		get
		{
			if (owner == null)
			{
				return urlColor;
			}
			return XmlHelper.SerializeColor(owner.UrlColor);
		}
		set
		{
			urlColor = value;
			if (owner != null)
			{
				owner.UrlColor = XmlHelper.DeserializeColor(value);
			}
		}
	}

	public XmlHyperTextExInfo()
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
	//IL_0006: Unknown result type (might be due to invalid IL or missing references)


	public XmlHyperTextExInfo(HyperTextEx owner)
		: this()
	{
		this.owner = owner;
	}

	public virtual void Load()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		if (owner != null)
		{
			urlStyle = UrlStyle;
			urlColor = UrlColor;
		}
	}

	public virtual void FixupReferences(HyperTextEx owner)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		this.owner = owner;
		UrlStyle = urlStyle;
		UrlColor = urlColor;
	}
}
