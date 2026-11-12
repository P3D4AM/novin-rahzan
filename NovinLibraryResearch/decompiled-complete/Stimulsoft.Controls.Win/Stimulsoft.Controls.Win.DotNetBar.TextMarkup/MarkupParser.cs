using System.Collections;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Stimulsoft.Controls.Win.DotNetBar.TextMarkup;

internal class MarkupParser
{
	private static string TextElementName = "text";

	private static string BodyTag = "body";

	public static BodyElement Parse(string text)
	{
		StringBuilder stringBuilder = new StringBuilder(text.Length);
		BodyElement bodyElement = new BodyElement();
		bodyElement.HasExpandElement = false;
		MarkupElement markupElement = bodyElement;
		Stack stack = new Stack();
		stack.Push(bodyElement);
		text = text.Replace("&nbsp;", "{ent_nbsp}");
		text = text.Replace("&zwsp;", "{ent_zwsp}");
		text = text.Replace("&lt;", "{ent_lt}");
		text = text.Replace("&gt;", "{ent_gt}");
		text = text.Replace("&amp;", "{ent_amp}");
		text = text.Replace("|", "{ent_l}");
		text = text.Replace("&", "|");
		text = text.Replace("{ent_nbsp}", "&nbsp;");
		text = text.Replace("{ent_zwsp}", "&zwsp;");
		text = text.Replace("{ent_lt}", "&lt;");
		text = text.Replace("{ent_gt}", "&gt;");
		string s = text;
		if (text.TrimStart(new char[0]).IndexOf("<?xml") != 0)
		{
			s = "<" + BodyTag + ">" + text + "</" + BodyTag + ">";
		}
		StringReader input = new StringReader(s);
		try
		{
			XmlTextReader xmlTextReader = new XmlTextReader(input);
			xmlTextReader.DtdProcessing = DtdProcessing.Ignore;
			while (xmlTextReader.Read())
			{
				if (xmlTextReader.NodeType == XmlNodeType.Element)
				{
					if (xmlTextReader.Name == BodyTag)
					{
						continue;
					}
					MarkupElement markupElement2 = CreateMarkupElement(xmlTextReader.Name);
					if (markupElement2 == null)
					{
						xmlTextReader.Skip();
						continue;
					}
					if (markupElement2 is ExpandElement)
					{
						bodyElement.HasExpandElement = true;
					}
					if (markupElement2 is IActiveMarkupElement)
					{
						bodyElement.ActiveElements.Add(markupElement2);
					}
					if (xmlTextReader.AttributeCount > 0)
					{
						markupElement2.ReadAttributes(xmlTextReader);
						xmlTextReader.MoveToElement();
					}
					markupElement.Elements.Add(markupElement2);
					if (markupElement2 is ContainerElement)
					{
						markupElement = markupElement2;
					}
					if (!xmlTextReader.IsEmptyElement)
					{
						stack.Push(markupElement2);
					}
				}
				else if (xmlTextReader.NodeType == XmlNodeType.Text)
				{
					if (xmlTextReader.Value.Length == 1)
					{
						TextElement textElement = CreateMarkupElement(TextElementName) as TextElement;
						if (xmlTextReader.Value == " ")
						{
							textElement.TrailingSpace = true;
							stringBuilder.Append(' ');
						}
						else
						{
							textElement.Text = xmlTextReader.Value;
							textElement.Text = textElement.Text.Replace("|", "&");
							textElement.Text = textElement.Text.Replace("{ent_l}", "|");
							textElement.Text = textElement.Text.Replace("{ent_amp}", "&&");
							stringBuilder.Append(textElement.Text + " ");
						}
						markupElement.Elements.Add(textElement);
						continue;
					}
					string text2 = xmlTextReader.Value;
					if (text2.StartsWith("\r\n"))
					{
						text2 = text2.TrimStart('\r', '\n');
					}
					text2 = text2.Replace("\r\n", " ");
					string[] array = text2.Split(new char[1] { ' ' });
					bool flag = false;
					if (markupElement.Elements.Count > 0 && markupElement.Elements[markupElement.Elements.Count - 1] is NewLine)
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
						TextElement textElement2 = CreateMarkupElement(TextElementName) as TextElement;
						textElement2.Text = array[i].Replace("|", "&");
						textElement2.Text = textElement2.Text.Replace("{ent_l}", "|");
						textElement2.Text = textElement2.Text.Replace("{ent_amp}", "&&");
						stringBuilder.Append(textElement2.Text + " ");
						if (i < array.Length - 1)
						{
							textElement2.TrailingSpace = true;
							flag = true;
						}
						markupElement.Elements.Add(textElement2);
					}
				}
				else if (xmlTextReader.NodeType == XmlNodeType.Whitespace)
				{
					if (xmlTextReader.Value.IndexOf(' ') >= 0)
					{
						TextElement textElement3 = CreateMarkupElement(TextElementName) as TextElement;
						textElement3.TrailingSpace = true;
						markupElement.Elements.Add(textElement3);
					}
				}
				else if (xmlTextReader.NodeType == XmlNodeType.EntityReference)
				{
					TextElement textElement4 = CreateMarkupElement(TextElementName) as TextElement;
					if (xmlTextReader.Name == "nbsp")
					{
						textElement4.TrailingSpace = true;
					}
					else if (xmlTextReader.Name == "zwsp")
					{
						textElement4.TrailingSpace = false;
					}
					else
					{
						textElement4.Text = xmlTextReader.Name;
					}
					textElement4.EnablePrefixHandling = false;
					markupElement.Elements.Add(textElement4);
				}
				else if (xmlTextReader.NodeType == XmlNodeType.EndElement)
				{
					MarkupElement markupElement3 = stack.Pop() as MarkupElement;
					if (markupElement3 != markupElement)
					{
						markupElement.Elements.Add(new EndMarkupElement(markupElement3));
					}
					else if (markupElement != bodyElement)
					{
						markupElement = markupElement.Parent;
					}
				}
			}
		}
		catch
		{
			return null;
		}
		bodyElement.PlainText = stringBuilder.ToString();
		return bodyElement;
	}

	public static MarkupElement CreateMarkupElement(string elementName)
	{
		switch (elementName)
		{
		case "b":
		case "strong":
			return new Strong();
		case "i":
		case "em":
			return new Italic();
		case "u":
			return new Underline();
		case "br":
			return new NewLine();
		case "expand":
			return new ExpandElement();
		case "a":
			return new HyperLink();
		case "p":
			return new Paragraph();
		case "div":
			return new Div();
		case "span":
			return new Span();
		case "img":
			return new ImageElement();
		case "h1":
			return new Heading();
		case "h2":
			return new Heading(2);
		case "h3":
			return new Heading(3);
		case "h4":
			return new Heading(4);
		case "h5":
			return new Heading(5);
		case "h6":
			return new Heading(6);
		case "font":
			return new FontElement();
		case "s":
		case "strike":
			return new Strike();
		default:
			if (elementName == TextElementName)
			{
				return new TextElement();
			}
			if (elementName == "symbol")
			{
				return new SymbolElement();
			}
			return null;
		}
	}

	public static bool IsMarkup(ref string text)
	{
		if (text.IndexOf("</") < 0 && text.IndexOf("/>") < 0)
		{
			return false;
		}
		return true;
	}

	internal static string RemoveExpand(string text)
	{
		return Regex.Replace(text, "<expand.*?>", "", RegexOptions.IgnoreCase);
	}
}
