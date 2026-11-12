using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;

namespace Stimulsoft.Editor;

public class HyperTextEx : IHyperTextEx, IHyperText
{
	private Color urlColor = EditConsts.DefaultUrlForeColor;

	private FontStyle urlStyle = EditConsts.DefaultUrlFontStyle;

	private bool showHints = true;

	private UrlJumpEventArgs urlJumpArgs;

	private ISyntaxEdit owner;

	[DefaultValue(false)]
	[Description("Gets or sets a value indicating whether hypertext urls in the text should be highlighted.")]
	public bool HighlightUrls
	{
		get
		{
			return owner.Source.HighlightUrls;
		}
		set
		{
			owner.Source.HighlightUrls = value;
		}
	}

	[DefaultValue(true)]
	[Description("Gets or sets value indicating whether default hint for hypertext section needs displaying when user moves mouse over the hypertext.")]
	public bool ShowHints
	{
		get
		{
			return showHints;
		}
		set
		{
			showHints = value;
		}
	}

	[Description("Gets or sets a value that represents color of highlighted urls.")]
	public Color UrlColor
	{
		get
		{
			return urlColor;
		}
		set
		{
			if (urlColor != value)
			{
				urlColor = value;
				if (HighlightUrls)
				{
					owner.Invalidate();
				}
			}
		}
	}

	[Description("Gets or sets font style of highlighted urls.")]
	public FontStyle UrlStyle
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return urlStyle;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			if (urlStyle != value)
			{
				urlStyle = value;
				if (HighlightUrls)
				{
					owner.Invalidate();
				}
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public object XmlInfo
	{
		get
		{
			return new XmlHyperTextExInfo(this);
		}
		set
		{
			((XmlHyperTextExInfo)value).FixupReferences(this);
		}
	}

	[Browsable(false)]
	public event UrlJumpEvent JumpToUrl;

	[Browsable(false)]
	public event HyperTextEvent CheckHyperText
	{
		add
		{
			owner.Source.CheckHyperText += value;
		}
		remove
		{
			owner.Source.CheckHyperText -= value;
		}
	}

	public HyperTextEx()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		urlJumpArgs = new UrlJumpEventArgs("", handled: false);
	}

	public HyperTextEx(ISyntaxEdit owner)
		: this()
	{
		this.owner = owner;
	}

	protected internal bool IsUrlAtTextPoint(int x, int y, out string url, bool needUrl)
	{
		bool flag = false;
		url = string.Empty;
		if (HighlightUrls)
		{
			IStrItem item = owner.Lines.GetItem(y);
			if (item != null)
			{
				short[] colorData = item.ColorData;
				flag = x >= 0 && x < colorData.Length && ((colorData[x] >> 8) & 0x10) != 0;
				if (flag && needUrl)
				{
					int num = x;
					int i = x;
					while (num > 0 && ((uint)(colorData[num - 1] >> 8) & 0x10u) != 0)
					{
						num--;
					}
					for (; i < colorData.Length - 1 && ((uint)(colorData[i + 1] >> 8) & 0x10u) != 0; i++)
					{
					}
					url = item.String.Substring(num, i - num + 1);
				}
			}
		}
		return flag;
	}

	protected internal bool IsUrlAtPoint(int x, int y, out string url, bool needUrl)
	{
		url = string.Empty;
		if (HighlightUrls)
		{
			Point point = owner.ScreenToText(x, y);
			return IsUrlAtTextPoint(point.X, point.Y, out url, needUrl);
		}
		return false;
	}

	public bool IsHyperText(string text)
	{
		return owner.Source.IsHyperText(text);
	}

	public bool IsUrlAtPoint(int x, int y)
	{
		string url;
		return IsUrlAtPoint(x, y, out url, needUrl: false);
	}

	public void UrlJump(string text)
	{
		urlJumpArgs.Text = text;
		urlJumpArgs.Handled = false;
		this.JumpToUrl?.Invoke(this, urlJumpArgs);
		if (!urlJumpArgs.Handled)
		{
			try
			{
				StiProcess.Start(text);
			}
			catch
			{
			}
		}
	}

	public void Assign(IHyperTextEx source)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		HighlightUrls = source.HighlightUrls;
		UrlColor = source.UrlColor;
		UrlStyle = source.UrlStyle;
	}

	public virtual void ResetHighlightUrls()
	{
		HighlightUrls = false;
	}

	public virtual void ResetShowHints()
	{
		ShowHints = true;
	}

	public virtual void ResetUrlColor()
	{
		UrlColor = EditConsts.DefaultUrlForeColor;
	}

	public virtual void ResetUrlStyle()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		UrlStyle = EditConsts.DefaultUrlFontStyle;
	}

	public bool ShouldSerializeUrlColor()
	{
		return urlColor != EditConsts.DefaultUrlForeColor;
	}

	public bool ShouldSerializeUrlStyle()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return urlStyle != EditConsts.DefaultUrlFontStyle;
	}
}
