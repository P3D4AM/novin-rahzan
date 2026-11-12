using System;
using System.Collections;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Stimulsoft.Editor;

public class CodeCompletionProvider : ArrayList, ICodeCompletionProvider, IList, ICollection, IEnumerable
{
	private int selIndex = -1;

	private ImageList images;

	private bool useIndent = true;

	private bool useHtmlFormatting;

	private bool showDescriptions;

	private string editField = string.Empty;

	public string[] Descriptions
	{
		get
		{
			string[] array = new string[Count];
			for (int i = 0; i < Count; i++)
			{
				array[i] = GetDescription(i);
			}
			return array;
		}
	}

	public int[] ImageIndexes
	{
		get
		{
			int[] array = new int[Count];
			for (int i = 0; i < Count; i++)
			{
				array[i] = GetImageIndex(i);
			}
			return array;
		}
	}

	public string[] Strings
	{
		get
		{
			string[] array = new string[Count];
			for (int i = 0; i < Count; i++)
			{
				array[i] = GetText(i);
			}
			return array;
		}
	}

	public virtual int ColumnCount => 0;

	public int SelIndex
	{
		get
		{
			return selIndex;
		}
		set
		{
			selIndex = value;
		}
	}

	public virtual ImageList Images
	{
		get
		{
			return images;
		}
		set
		{
			images = value;
		}
	}

	public bool UseIndent
	{
		get
		{
			return useIndent;
		}
		set
		{
			useIndent = value;
		}
	}

	public bool UseHtmlFormatting
	{
		get
		{
			return useHtmlFormatting;
		}
		set
		{
			useHtmlFormatting = value;
		}
	}

	public bool ShowDescriptions
	{
		get
		{
			return showDescriptions;
		}
		set
		{
			showDescriptions = value;
		}
	}

	public string EditField
	{
		get
		{
			return editField;
		}
		set
		{
			editField = value;
		}
	}

	public virtual string EditPath => string.Empty;

	public virtual object XmlInfo
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public event ClosePopupEvent ClosePopup;

	public event ShowPopupEvent ShowPopup;

	protected virtual Type GetXmlType()
	{
		return null;
	}

	protected internal virtual bool ShouldFormatName()
	{
		return false;
	}

	public virtual void OnClosePopup(object sender, ClosingEventArgs e)
	{
		if (this.ClosePopup != null)
		{
			this.ClosePopup(sender, e);
		}
	}

	public virtual void OnShowPopup(object sender, ShowingEventArgs e)
	{
		if (this.ShowPopup != null)
		{
			this.ShowPopup(sender, e);
		}
	}

	public virtual string GetText(int index)
	{
		return string.Empty;
	}

	public virtual string GetName(int index)
	{
		return string.Empty;
	}

	public virtual int IndexOfName(string name, bool caseSensitive)
	{
		for (int i = 0; i < Count; i++)
		{
			if (string.Compare(name, GetName(i), !caseSensitive) == 0)
			{
				return i;
			}
		}
		return -1;
	}

	public virtual int GetImageIndex(int index)
	{
		return -1;
	}

	public virtual string GetDescription(int index)
	{
		return string.Empty;
	}

	public virtual string GetColumnText(int index, int column)
	{
		return string.Empty;
	}

	public virtual bool ColumnVisible(int column)
	{
		return false;
	}

	public virtual ICodeCompletionProvider GetParent()
	{
		return null;
	}

	public bool SaveFile(string fileName)
	{
		try
		{
			Stream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
			try
			{
				StreamWriter streamWriter = new StreamWriter(stream);
				try
				{
					return SaveStream(streamWriter);
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
			return false;
		}
	}

	public bool SaveStream(TextWriter writer)
	{
		if (GetXmlType() == null)
		{
			return false;
		}
		XmlSerializer xmlSerializer = new XmlSerializer(GetXmlType());
		try
		{
			xmlSerializer.Serialize(writer, XmlInfo);
			return true;
		}
		catch (Exception exception)
		{
			writer.Flush();
			ErrorHandler.Error(exception);
			return false;
		}
	}

	public bool SaveStream(Stream stream)
	{
		TextWriter textWriter = new StreamWriter(stream);
		try
		{
			return SaveStream(textWriter);
		}
		finally
		{
			textWriter.Close();
		}
	}

	public bool LoadFile(string fileName)
	{
		try
		{
			Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			try
			{
				StreamReader streamReader = new StreamReader(stream);
				try
				{
					return LoadStream(streamReader);
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
			return false;
		}
	}

	public bool LoadStream(TextReader reader)
	{
		if (GetXmlType() == null)
		{
			return false;
		}
		XmlSerializer xmlSerializer = new XmlSerializer(GetXmlType());
		try
		{
			XmlInfo = xmlSerializer.Deserialize(reader);
			return true;
		}
		catch (Exception exception)
		{
			ErrorHandler.Error(exception);
			return false;
		}
	}

	public bool LoadStream(Stream stream)
	{
		TextReader textReader = new StreamReader(stream);
		try
		{
			return LoadStream(textReader);
		}
		finally
		{
			textReader.Close();
		}
	}
}
