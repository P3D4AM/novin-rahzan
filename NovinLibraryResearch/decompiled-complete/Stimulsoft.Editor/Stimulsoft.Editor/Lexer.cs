using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Stimulsoft.Editor;

[ToolboxItem(false)]
public class Lexer : Component, ILexer, INotify
{
	private Container components;

	private int updateCount;

	private EventHandler notifyHandler;

	private LexScheme scheme;

	private int defaultState;

	private TextParsedEventArgs textParsedEventArgs;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int UpdateCount => updateCount;

	[Browsable(false)]
	public virtual string XmlScheme
	{
		get
		{
			try
			{
				StringWriter stringWriter = new StringWriter();
				SaveScheme(stringWriter);
				return stringWriter.ToString();
			}
			catch
			{
				return string.Empty;
			}
		}
		set
		{
			try
			{
				StringReader reader = new StringReader(value);
				LoadScheme(reader);
			}
			catch
			{
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual ILexScheme Scheme
	{
		get
		{
			return scheme;
		}
		set
		{
			StringWriter stringWriter = new StringWriter();
			SaveScheme(stringWriter, (LexScheme)value);
			StringReader reader = new StringReader(stringWriter.ToString());
			LoadScheme(reader);
		}
	}

	[Category("Parser")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Editor("QWhale.Design.SyntaxBuilderEditor, QWhale.Syntax", typeof(UITypeEditor))]
	[Description("Gets or sets list of rules for lexical analysis.")]
	public virtual LexScheme LexScheme
	{
		get
		{
			return (LexScheme)Scheme;
		}
		set
		{
			Scheme = value;
		}
	}

	[Category("Parser")]
	[Description("Gets or sets default state of the \"Lexer\".")]
	public int DefaultState
	{
		get
		{
			return defaultState;
		}
		set
		{
			if (defaultState != value)
			{
				defaultState = value;
				Update();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual object XmlInfo
	{
		get
		{
			return new XmlLexerInfo(this);
		}
		set
		{
			((XmlLexerInfo)value).FixupReferences(this);
		}
	}

	[Description("Occurs when text line is parsed, allowing to modify colors/styles information about the parsed line.")]
	public event TextParsedEvent TextParsed;

	public Lexer(IContainer container)
		: this()
	{
		container.Add(this);
	}

	public Lexer()
	{
		InitializeComponent();
		textParsedEventArgs = new TextParsedEventArgs();
		scheme = CreateLexScheme();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	protected virtual LexScheme CreateLexScheme()
	{
		return new LexScheme();
	}

	private void InitializeComponent()
	{
		components = new Container();
	}

	public int BeginUpdate()
	{
		updateCount++;
		return updateCount;
	}

	public int EndUpdate()
	{
		updateCount--;
		if (updateCount == 0)
		{
			Notify();
		}
		return updateCount;
	}

	public void AddNotifier(INotifier sender)
	{
		notifyHandler = (EventHandler)Delegate.Combine(notifyHandler, new EventHandler(sender.Notification));
	}

	public void RemoveNotifier(INotifier sender)
	{
		notifyHandler = (EventHandler)Delegate.Remove(notifyHandler, new EventHandler(sender.Notification));
	}

	public void Notify()
	{
		if (notifyHandler != null)
		{
			notifyHandler(this, EventArgs.Empty);
		}
	}

	private void StateFromMatch(Match match, string s, Hashtable blocks, ref int state, ref int style)
	{
		style = -1;
		LexSyntaxBlock lexSyntaxBlock = null;
		for (int i = 0; i < match.Groups.Count; i++)
		{
			if (match.Groups[i].Success)
			{
				lexSyntaxBlock = (LexSyntaxBlock)blocks[i];
				if (lexSyntaxBlock != null)
				{
					break;
				}
			}
		}
		if (lexSyntaxBlock == null)
		{
			return;
		}
		if (lexSyntaxBlock.LeaveState != null)
		{
			state = lexSyntaxBlock.LeaveState.Index;
		}
		if (lexSyntaxBlock.Style != null)
		{
			style = lexSyntaxBlock.Style.Index;
		}
		int num = lexSyntaxBlock.FindResword(s);
		if (num >= 0)
		{
			ILexStyle reswordStyle = lexSyntaxBlock.LexReswordSets[num].ReswordStyle;
			if (reswordStyle != null)
			{
				style = reswordStyle.Index;
			}
		}
	}

	protected void Update()
	{
		if (updateCount == 0)
		{
			Notify();
		}
	}

	protected void OnTextParsed(string str, ref short[] colorData)
	{
		if (this.TextParsed != null)
		{
			textParsedEventArgs.String = str;
			textParsedEventArgs.ColorData = colorData;
			this.TextParsed(this, textParsedEventArgs);
		}
	}

	public virtual int ParseText(int state, int line, string str, ref int pos, ref int len, ref int style)
	{
		len = 0;
		style = 0;
		if (str == null)
		{
			return state;
		}
		LexState lexState = (LexState)scheme.GetLexState(state);
		Match match = lexState.Regex.Match(str, pos);
		if (match.Success)
		{
			pos = match.Index;
			len = match.Length;
			int state2 = state;
			int style2 = 0;
			StateFromMatch(match, str.Substring(pos, len), lexState.BlockTable, ref state2, ref style);
			bool flag = pos + match.Length >= str.Length;
			int startat = (flag ? (pos + match.Length) : pos);
			while (match.Length == 0 || flag)
			{
				if (state != state2)
				{
					state = state2;
					lexState = (LexState)scheme.GetLexState(state);
					match = lexState.Regex.Match(str, startat);
				}
				else
				{
					match = match.NextMatch();
				}
				if (!match.Success)
				{
					break;
				}
				if (!flag)
				{
					pos = match.Index;
					len = match.Length;
				}
				StateFromMatch(match, string.Empty, lexState.BlockTable, ref state2, ref style2);
				if (!flag)
				{
					style = style2;
				}
			}
			state = state2;
		}
		if (len == 0)
		{
			len = 1;
		}
		return state;
	}

	public virtual int ParseText(int state, int line, string str, ref short[] colorData)
	{
		if (str == null || str == string.Empty)
		{
			return state;
		}
		int length = str.Length;
		if (scheme.LexStates.Count == 0)
		{
			for (int i = 0; i < length; i++)
			{
				colorData[i] = 0;
			}
			OnTextParsed(str, ref colorData);
			return state;
		}
		LexState lexState = (LexState)scheme.GetLexState(state);
		Match match = lexState.Regex.Match(str);
		int num = 0;
		int state2 = 0;
		int style = 0;
		while (match.Success)
		{
			if (match.Index > num)
			{
				for (int i = num; i < match.Index; i++)
				{
					colorData[i] = 0;
				}
			}
			num = match.Index;
			StateFromMatch(match, str.Substring(num, match.Length), lexState.BlockTable, ref state2, ref style);
			if (match.Length > 0)
			{
				for (int i = num; i < num + match.Length; i++)
				{
					colorData[i] = (byte)(style + 1);
				}
			}
			num += match.Length;
			if (state2 != state)
			{
				state = state2;
				lexState = (LexState)scheme.GetLexState(state2);
				match = lexState.Regex.Match(str, num);
			}
			else
			{
				match = match.NextMatch();
			}
		}
		for (int i = num; i < length; i++)
		{
			colorData[i] = 0;
		}
		OnTextParsed(str, ref colorData);
		return state;
	}

	public virtual bool ShouldSerializeToCode()
	{
		return true;
	}

	public void SaveScheme(string fileName)
	{
		try
		{
			Stream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
			try
			{
				StreamWriter streamWriter = new StreamWriter(stream);
				try
				{
					SaveScheme(streamWriter);
				}
				finally
				{
					streamWriter.Close();
				}
			}
			finally
			{
				stream.Close();
			}
		}
		catch (Exception exception)
		{
			ErrorHandler.Error(exception);
		}
	}

	public void LoadScheme(string fileName)
	{
		try
		{
			Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			try
			{
				StreamReader streamReader = new StreamReader(stream);
				try
				{
					LoadScheme(streamReader);
				}
				finally
				{
					streamReader.Close();
				}
			}
			finally
			{
				stream.Close();
			}
		}
		catch (Exception exception)
		{
			ErrorHandler.Error(exception);
		}
	}

	public void SaveScheme(TextWriter writer)
	{
		SaveScheme(writer, scheme);
	}

	public void SaveScheme(TextWriter writer, LexScheme scheme)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(LexScheme));
		try
		{
			xmlSerializer.Serialize(writer, scheme);
		}
		catch (Exception exception)
		{
			writer.Flush();
			ErrorHandler.Error(exception);
		}
	}

	public void LoadScheme(TextReader reader)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(LexScheme));
		try
		{
			scheme = (LexScheme)xmlSerializer.Deserialize(reader);
			Update();
		}
		catch (Exception exception)
		{
			ErrorHandler.Error(exception);
		}
	}

	public virtual void ResetDefaultState()
	{
		DefaultState = 0;
	}
}
