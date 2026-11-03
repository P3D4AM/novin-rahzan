using System.Drawing;
using System.Xml;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.TextMarkup;

internal class ቂ : ሡ
{
	private Color _0D02 = Color.Empty;

	private Color _0D03 = Color.Empty;

	private int _߄;

	private bool ቄ;

	private string ቅ = "";

	private string ቆ = "";

	public Color _0CFF
	{
		get
		{
			return _0D02;
		}
		set
		{
			_0D02 = value;
		}
	}

	public int ᇐ
	{
		get
		{
			return _߄;
		}
		set
		{
			_߄ = value;
		}
	}

	public string ቃ
	{
		get
		{
			return ቅ;
		}
		set
		{
			ቅ = value;
		}
	}

	protected override void SetFont(MarkupDrawContext d)
	{
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Expected O, but got Unknown
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		Font val = d.CurrentFont;
		try
		{
			if (ቅ != "" || (_߄ != 0 && ቄ) || (_߄ > 4 && !ቄ))
			{
				if (ቅ != "")
				{
					d.CurrentFont = new Font(ቅ, (ቄ || _߄ == 0) ? (val.SizeInPoints + (float)_߄) : ((float)_߄), val.Style);
				}
				else
				{
					d.CurrentFont = new Font(val.FontFamily, (ቄ || _߄ == 0) ? (val.SizeInPoints + (float)_߄) : ((float)_߄), val.Style);
				}
			}
			else
			{
				val = null;
			}
		}
		catch
		{
			val = null;
		}
		if (val != null)
		{
			ሢ = val;
		}
		if (!d.IgnoreFormattingColors)
		{
			if (!_0D02.IsEmpty)
			{
				_0D03 = d.CurrentForeColor;
				d.CurrentForeColor = _0D02;
			}
			else if (ቆ != "" && GlobalManager.Renderer is Office2007Renderer)
			{
				_0D03 = d.CurrentForeColor;
				d.CurrentForeColor = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.Form.Active.CaptionTextExtra;
			}
		}
	}

	public override void RenderEnd(MarkupDrawContext d)
	{
		RestoreForeColor(d);
		base.RenderEnd(d);
	}

	public override void MeasureEnd(Size availableSize, MarkupDrawContext d)
	{
		RestoreForeColor(d);
		base.MeasureEnd(availableSize, d);
	}

	protected virtual void RestoreForeColor(MarkupDrawContext d)
	{
		if (d != null)
		{
			if (!_0D03.IsEmpty)
			{
				d.CurrentForeColor = _0D03;
			}
			_0D03 = Color.Empty;
		}
	}

	private Color ഉ(string _0956)
	{
		string text = _0956.ToLower();
		ቆ = "";
		if (text == "syscaptiontextextra")
		{
			ቆ = text;
			return Color.Empty;
		}
		return Color.FromName(_0956);
	}

	public override void ReadAttributes(XmlTextReader reader)
	{
		ቄ = false;
		for (int i = 0; i < reader.AttributeCount; i++)
		{
			reader.MoveToAttribute(i);
			if (reader.Name.ToLower() == "color")
			{
				try
				{
					string value = reader.Value;
					if (value.StartsWith("#"))
					{
						if (value.Length == 7)
						{
							_0D02 = ColorScheme.GetColor(value.Substring(1));
						}
					}
					else
					{
						_0D02 = ഉ(value);
					}
				}
				catch
				{
					_0D02 = Color.Empty;
				}
			}
			else if (reader.Name.ToLower() == "size")
			{
				string value2 = reader.Value;
				if (value2.StartsWith("+"))
				{
					try
					{
						_߄ = int.Parse(value2.Substring(1));
						ቄ = true;
					}
					catch
					{
						_߄ = 0;
					}
					continue;
				}
				if (value2.StartsWith("-"))
				{
					ቄ = true;
				}
				try
				{
					_߄ = int.Parse(value2);
				}
				catch
				{
					_߄ = 0;
				}
			}
			else if (reader.Name.ToLower() == "face")
			{
				ቅ = reader.Value;
			}
		}
	}
}
