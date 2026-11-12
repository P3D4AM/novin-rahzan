using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace Stimulsoft.Editor;

public class StringList : ArrayList, IStringList, ICollection, IEnumerable, IImport, IExport
{
	private string lineTerminator = "\r\n";

	public new string this[int index] => (string)base[index];

	[Description("Gets or sets the strings in the \"SyntaxStrings\" as a single string with the individual strings delimited by carriage returns.")]
	public virtual string Text
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			IEnumerator enumerator = GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					string text = (string)enumerator.Current;
					stringBuilder.Append(text + lineTerminator);
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
			if (stringBuilder.Length >= 2)
			{
				stringBuilder.Remove(stringBuilder.Length - 2, 2);
			}
			return stringBuilder.ToString();
		}
		set
		{
			Clear();
			if (value != null && value != string.Empty)
			{
				StringReader stringReader = new StringReader(value);
				string value2;
				while ((value2 = stringReader.ReadLine()) != null)
				{
					Add(value2);
				}
			}
		}
	}

	[DefaultValue("\r\n")]
	[Description("Gets or sets a string value that terminates line.")]
	public string LineTerminator
	{
		get
		{
			return lineTerminator;
		}
		set
		{
			lineTerminator = value;
		}
	}

	public StringList()
	{
	}

	public StringList(TextReader reader)
	{
		string value;
		while ((value = reader.ReadLine()) != null)
		{
			Add(value);
		}
	}

	public bool LoadFile(string fileName)
	{
		return LoadFile(fileName, null);
	}

	public bool LoadFile(string fileName, Encoding encoding)
	{
		bool result = true;
		try
		{
			Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			try
			{
				StreamReader streamReader = ((encoding != null) ? new StreamReader(stream, encoding) : new StreamReader(stream));
				try
				{
					LoadStream(streamReader);
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
			result = false;
		}
		return result;
	}

	public bool LoadStream(TextReader reader)
	{
		Clear();
		string value;
		while ((value = reader.ReadLine()) != null)
		{
			Add(value);
		}
		return true;
	}

	public bool LoadStream(Stream stream)
	{
		return LoadStream(stream, null);
	}

	public bool LoadStream(Stream stream, Encoding encoding)
	{
		TextReader textReader = ((encoding != null) ? new StreamReader(stream, encoding) : new StreamReader(stream));
		try
		{
			return LoadStream(textReader);
		}
		finally
		{
			textReader.Close();
		}
	}

	public bool SaveFile(string fileName)
	{
		return SaveFile(fileName, null);
	}

	public bool SaveFile(string fileName, Encoding encoding)
	{
		bool result = true;
		try
		{
			Stream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
			try
			{
				StreamWriter streamWriter = ((encoding != null) ? new StreamWriter(stream, encoding) : new StreamWriter(stream));
				try
				{
					SaveStream(streamWriter);
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
			result = false;
		}
		return result;
	}

	public bool SaveStream(TextWriter writer)
	{
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				string value = (string)enumerator.Current;
				writer.WriteLine(value);
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
		return true;
	}

	public bool SaveStream(Stream stream)
	{
		return SaveStream(stream, null);
	}

	public bool SaveStream(Stream stream, Encoding encoding)
	{
		TextWriter textWriter = ((encoding != null) ? new StreamWriter(stream, encoding) : new StreamWriter(stream));
		try
		{
			return SaveStream(textWriter);
		}
		finally
		{
			textWriter.Close();
		}
	}
}
