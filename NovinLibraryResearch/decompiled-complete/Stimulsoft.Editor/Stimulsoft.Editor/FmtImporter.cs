using System.Collections;
using System.Drawing;
using System.IO;

namespace Stimulsoft.Editor;

public class FmtImporter
{
	protected internal class FormattedItem
	{
		public string Tag;

		public FontStyle FontStyle;

		public FontStyle ExcludeStyle;

		public Color ForeColor;

		public Color BackColor;

		public bool StyleValid;
	}

	private ArrayList stack;

	private FontStyle defaultStyle;

	protected ISyntaxEdit owner;

	protected TextReader reader;

	protected ReadFormattedTextEventArgs readFormattedArgs;

	public FontStyle DefaultStyle
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return defaultStyle;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			defaultStyle = value;
		}
	}

	public event ReadFormattedTextEvent ReadFormattedText;

	public FmtImporter()
	{
		stack = new ArrayList();
		readFormattedArgs = new ReadFormattedTextEventArgs();
	}

	protected void Pop(string tag)
	{
		for (int num = stack.Count - 1; num >= 0; num--)
		{
			FormattedItem formattedItem = (FormattedItem)stack[num];
			if (formattedItem.Tag == tag)
			{
				stack.RemoveAt(num);
				break;
			}
		}
	}

	protected void Push(string tag)
	{
		FormattedItem formattedItem = new FormattedItem();
		formattedItem.Tag = tag;
		stack.Add(formattedItem);
	}

	protected FormattedItem GetCurrentItem()
	{
		if (stack.Count == 0)
		{
			stack.Add(new FormattedItem());
		}
		return (FormattedItem)stack[stack.Count - 1];
	}

	protected void GetCurrentAttributes(out Color foreColor, out Color backColor, out FontStyle fontStyle)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected I4, but got Unknown
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected I4, but got Unknown
		foreColor = Color.Empty;
		backColor = Color.Empty;
		fontStyle = (FontStyle)(int)DefaultStyle;
		foreach (FormattedItem item in stack)
		{
			if (item.ForeColor != Color.Empty)
			{
				foreColor = item.ForeColor;
			}
			if (item.BackColor != Color.Empty)
			{
				backColor = item.BackColor;
			}
			if (item.StyleValid)
			{
				fontStyle = (FontStyle)((fontStyle | item.FontStyle) & ~item.ExcludeStyle);
			}
		}
	}

	protected void WriteFontStyle(FontStyle style, bool value)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		FormattedItem currentItem = GetCurrentItem();
		currentItem.StyleValid = true;
		if (value)
		{
			currentItem.ExcludeStyle &= ~style;
			currentItem.FontStyle |= style;
		}
		else
		{
			currentItem.FontStyle &= ~style;
			currentItem.ExcludeStyle |= style;
		}
	}

	protected void WriteForeColor(Color foreColor)
	{
		FormattedItem currentItem = GetCurrentItem();
		currentItem.ForeColor = foreColor;
	}

	protected void WriteBackColor(Color backColor)
	{
		FormattedItem currentItem = GetCurrentItem();
		currentItem.BackColor = backColor;
	}

	protected void WriteString(string s)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		GetCurrentAttributes(out var foreColor, out var backColor, out var fontStyle);
		OnReadFormattedText(foreColor, backColor, fontStyle, s);
	}

	protected virtual void ReadHeader()
	{
	}

	protected virtual void ReadContent()
	{
		stack.Clear();
	}

	protected virtual void OnReadFormattedText(Color foreColor, Color backColor, FontStyle fontStyle, string text)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		if (this.ReadFormattedText != null)
		{
			readFormattedArgs.ForeColor = foreColor;
			readFormattedArgs.BackColor = backColor;
			readFormattedArgs.FontStyle = fontStyle;
			readFormattedArgs.Text = text;
			this.ReadFormattedText(this, readFormattedArgs);
		}
	}

	public virtual void Read(ISyntaxEdit owner, TextReader reader, object userData)
	{
		this.owner = owner;
		this.reader = reader;
		readFormattedArgs.UserData = userData;
		ReadHeader();
		ReadContent();
	}
}
