using System;
using System.IO;
using System.Xml.Serialization;

namespace Stimulsoft.Editor;

public class PersistentSettings : IPersistentSettings
{
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

	public virtual void Assign(IPersistentSettings source)
	{
	}

	public void SaveFile(string fileName)
	{
		try
		{
			Stream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
			try
			{
				StreamWriter streamWriter = new StreamWriter(stream);
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
		}
	}

	public void SaveStream(TextWriter writer)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(GetXmlType());
		try
		{
			xmlSerializer.Serialize(writer, XmlInfo);
		}
		catch (Exception exception)
		{
			writer.Flush();
			ErrorHandler.Error(exception);
		}
	}

	public void LoadFile(string fileName)
	{
		try
		{
			Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			try
			{
				StreamReader streamReader = new StreamReader(stream);
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
		}
	}

	public void LoadStream(TextReader reader)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(GetXmlType());
		try
		{
			XmlInfo = xmlSerializer.Deserialize(reader);
		}
		catch (Exception exception)
		{
			ErrorHandler.Error(exception);
		}
	}

	public virtual Type GetXmlType()
	{
		return null;
	}
}
