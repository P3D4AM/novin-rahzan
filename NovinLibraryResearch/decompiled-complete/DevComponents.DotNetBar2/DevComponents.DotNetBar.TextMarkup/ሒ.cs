using System.Collections;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace DevComponents.DotNetBar.TextMarkup;

internal class ሒ
{
	private static string ሓ = "text";

	private static string ሔ = "body";

	public static _0CB4 ሕ(string _06EA)
	{
		StringBuilder stringBuilder = new StringBuilder(_06EA.Length);
		_0CB4 obj = new _0CB4();
		obj._0CBA = false;
		_0CBF obj2 = obj;
		Stack stack = new Stack();
		stack.Push(obj);
		_06EA = _06EA.Replace("&nbsp;", "{ent_nbsp}");
		_06EA = _06EA.Replace("&zwsp;", "{ent_zwsp}");
		_06EA = _06EA.Replace("&lt;", "{ent_lt}");
		_06EA = _06EA.Replace("&gt;", "{ent_gt}");
		_06EA = _06EA.Replace("&amp;", "{ent_amp}");
		_06EA = _06EA.Replace("|", "{ent_l}");
		_06EA = _06EA.Replace("&", "|");
		_06EA = _06EA.Replace("{ent_nbsp}", "&nbsp;");
		_06EA = _06EA.Replace("{ent_zwsp}", "&zwsp;");
		_06EA = _06EA.Replace("{ent_lt}", "&lt;");
		_06EA = _06EA.Replace("{ent_gt}", "&gt;");
		StringReader input = new StringReader("<" + ሔ + ">" + _06EA + "</" + ሔ + ">");
		try
		{
			XmlTextReader xmlTextReader = new XmlTextReader(input);
			while (xmlTextReader.Read())
			{
				if (xmlTextReader.NodeType == XmlNodeType.Element)
				{
					if (xmlTextReader.Name == ሔ)
					{
						continue;
					}
					_0CBF obj3 = ሖ(xmlTextReader.Name);
					if (obj3 == null)
					{
						xmlTextReader.Skip();
						continue;
					}
					if (obj3 is ሦ)
					{
						obj._0CBA = true;
					}
					if (obj3 is _0CFC)
					{
						obj.ಶ._0CCB(obj3);
					}
					if (xmlTextReader.AttributeCount > 0)
					{
						obj3.ReadAttributes(xmlTextReader);
						xmlTextReader.MoveToElement();
					}
					obj2.Elements._0CCB(obj3);
					if (obj3 is _0CBC)
					{
						obj2 = obj3;
					}
					if (!xmlTextReader.IsEmptyElement)
					{
						stack.Push(obj3);
					}
				}
				else if (xmlTextReader.NodeType == XmlNodeType.Text)
				{
					if (xmlTextReader.Value.Length == 1)
					{
						ሚ ሚ2 = ሖ(ሓ) as ሚ;
						if (xmlTextReader.Value == " ")
						{
							ሚ2.ማ = true;
							stringBuilder.Append(' ');
						}
						else
						{
							ሚ2.ᆺ = xmlTextReader.Value;
							ሚ2.ᆺ = ሚ2.ᆺ.Replace("|", "&");
							ሚ2.ᆺ = ሚ2.ᆺ.Replace("{ent_l}", "|");
							ሚ2.ᆺ = ሚ2.ᆺ.Replace("{ent_amp}", "&&");
							stringBuilder.Append(ሚ2.ᆺ + " ");
						}
						obj2.Elements._0CCB(ሚ2);
						continue;
					}
					string text = xmlTextReader.Value;
					if (text.StartsWith("\r\n"))
					{
						text = text.TrimStart('\r', '\n');
					}
					text = text.Replace("\r\n", " ");
					string[] array = text.Split(new char[1] { ' ' });
					bool flag = false;
					if (obj2.Elements.Count > 0 && obj2.Elements.get__0CCA(obj2.Elements.Count - 1) is ሥ)
					{
						flag = true;
					}
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i].Length == 0)
						{
							if (flag)
							{
								continue;
							}
							flag = true;
						}
						else
						{
							flag = false;
						}
						ሚ ሚ3 = ሖ(ሓ) as ሚ;
						ሚ3.ᆺ = array[i].Replace("|", "&");
						ሚ3.ᆺ = ሚ3.ᆺ.Replace("{ent_l}", "|");
						ሚ3.ᆺ = ሚ3.ᆺ.Replace("{ent_amp}", "&&");
						stringBuilder.Append(ሚ3.ᆺ + " ");
						if (i < array.Length - 1)
						{
							ሚ3.ማ = true;
							flag = true;
						}
						obj2.Elements._0CCB(ሚ3);
					}
				}
				else if (xmlTextReader.NodeType == XmlNodeType.Whitespace)
				{
					if (xmlTextReader.Value.IndexOf(' ') >= 0)
					{
						ሚ ሚ4 = ሖ(ሓ) as ሚ;
						ሚ4.ማ = true;
						obj2.Elements._0CCB(ሚ4);
					}
				}
				else if (xmlTextReader.NodeType == XmlNodeType.EntityReference)
				{
					ሚ ሚ5 = ሖ(ሓ) as ሚ;
					if (xmlTextReader.Name == "nbsp")
					{
						ሚ5.ማ = true;
					}
					else if (xmlTextReader.Name == "zwsp")
					{
						ሚ5.ማ = false;
					}
					else
					{
						ሚ5.ᆺ = xmlTextReader.Name;
					}
					ሚ5.ሜ = false;
					obj2.Elements._0CCB(ሚ5);
				}
				else if (xmlTextReader.NodeType == XmlNodeType.EndElement)
				{
					_0CBF obj4 = stack.Pop() as _0CBF;
					if (obj4 != obj2)
					{
						obj2.Elements._0CCB(new ഐ(obj4));
					}
					else if (obj2 != obj)
					{
						obj2 = obj2.Parent;
					}
				}
			}
		}
		catch
		{
			return null;
		}
		obj._0CBB = stringBuilder.ToString();
		return obj;
	}

	public static _0CBF ሖ(string ሗ)
	{
		switch (ሗ)
		{
		case "b":
		case "strong":
			return new ሠ();
		case "i":
		case "em":
			return new ሣ();
		case "u":
			return new ሤ();
		case "br":
			return new ሥ();
		case "expand":
			return new ሦ();
		case "a":
			return new _0CFE();
		case "p":
			return new ሮ();
		case "div":
			return new ሯ();
		case "span":
			return new ሸ();
		case "img":
			return new ሹ();
		case "h1":
			return new ሿ();
		case "h2":
			return new ሿ(2);
		case "h3":
			return new ሿ(3);
		case "h4":
			return new ሿ(4);
		case "h5":
			return new ሿ(5);
		case "h6":
			return new ሿ(6);
		case "font":
			return new ቂ();
		case "s":
		case "strike":
			return new _28E0();
		default:
			if (ሗ == ሓ)
			{
				return new ሚ();
			}
			if (ሗ == "symbol")
			{
				return new _28E1();
			}
			return null;
		}
	}

	public static bool መ(ref string _06EA)
	{
		if (_06EA == null || (_06EA.IndexOf("</") < 0 && _06EA.IndexOf("/>") < 0))
		{
			return false;
		}
		return true;
	}

	internal static string ሙ(string _06EA)
	{
		return Regex.Replace(_06EA, "<expand.*?>", "", RegexOptions.IgnoreCase);
	}
}
