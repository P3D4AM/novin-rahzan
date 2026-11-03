using System.IO;

namespace Stimulsoft.Report.Export;

public class StiHtmlTextWriter
{
	public enum WriterMode
	{
		None,
		BeginTag,
		Attribute,
		Data
	}

	private TextWriter stream;

	private WriterMode mode;

	private int indent;

	public int Indent
	{
		get
		{
			return indent;
		}
		set
		{
			indent = value;
		}
	}

	public void Write(string st)
	{
		CheckIndent();
		stream.Write(st);
		mode = WriterMode.Data;
	}

	public void WriteLine()
	{
		stream.WriteLine();
		mode = WriterMode.None;
	}

	public void WriteLine(string st)
	{
		if (string.IsNullOrEmpty(st))
		{
			stream.WriteLine();
		}
		else
		{
			CheckIndent();
			stream.WriteLine(st);
		}
		mode = WriterMode.None;
	}

	public void WriteBeginTag(string st)
	{
		CloseTag();
		CheckIndent();
		stream.Write("<" + st);
		mode = WriterMode.BeginTag;
	}

	public void WriteFullBeginTag(string st)
	{
		CloseTag();
		CheckIndent();
		stream.Write("<" + st + ">");
		mode = WriterMode.Data;
	}

	public void WriteEndTag(string st)
	{
		if (mode == WriterMode.BeginTag)
		{
			stream.Write("/>");
		}
		else
		{
			CloseTag();
			CheckIndent();
			stream.Write("</" + st + ">");
		}
		mode = WriterMode.Data;
	}

	public void WriteFullEndTag(string st)
	{
		CloseTag();
		CheckIndent();
		stream.Write("</" + st + ">");
		mode = WriterMode.Data;
	}

	public void WriteAttribute(string attr, string value)
	{
		stream.Write(" " + attr);
		if (value != null)
		{
			stream.Write("=\"" + value + "\"");
		}
		mode = WriterMode.Attribute;
	}

	public void WriteStyleAttribute(string attr, string value)
	{
		stream.Write(attr + ":" + value + ";");
		mode = WriterMode.Attribute;
	}

	public void Flush()
	{
		stream.Flush();
	}

	private void CloseTag()
	{
		if (mode == WriterMode.Attribute || mode == WriterMode.BeginTag)
		{
			stream.Write(">");
		}
	}

	private void CheckIndent()
	{
		if (mode == WriterMode.None)
		{
			for (int i = 0; i < indent; i++)
			{
				stream.Write("\t");
			}
		}
	}

	public StiHtmlTextWriter(TextWriter baseStream)
	{
		stream = baseStream;
		mode = WriterMode.None;
		indent = 0;
	}
}
