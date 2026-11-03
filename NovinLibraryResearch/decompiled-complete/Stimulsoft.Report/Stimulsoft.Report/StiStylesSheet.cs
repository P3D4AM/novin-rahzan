using System;
using System.Globalization;
using System.IO;
using System.Threading;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;

namespace Stimulsoft.Report;

public class StiStylesSheet
{
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public StiStylesCollection Styles { get; set; }

	public virtual void Save(Stream stream)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		StiSerializing val = new StiSerializing((StiObjectStringConverter)(object)new StiReportObjectStringConverter());
		val.Serialize((object)this, stream, "StiStylesSheet");
	}

	public virtual void Load(Stream stream)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		StiSerializing val = new StiSerializing((StiObjectStringConverter)(object)new StiReportObjectStringConverter());
		val.Deserialize((object)this, stream, "StiStylesSheet");
	}

	public virtual void Save(string file)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		StiFileUtils.ProcessReadOnly(file);
		FileStream fileStream = new FileStream(file, FileMode.Create, FileAccess.Write);
		StiSerializing val = new StiSerializing((StiObjectStringConverter)(object)new StiReportObjectStringConverter());
		val.Serialize((object)this, (Stream)fileStream, "StiStylesSheet");
		fileStream.Flush();
		fileStream.Close();
	}

	public virtual void Load(string file)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
		StiSerializing val = new StiSerializing((StiObjectStringConverter)(object)new StiReportObjectStringConverter());
		val.Deserialize((object)this, (Stream)fileStream, "StiStylesSheet");
		fileStream.Flush();
		fileStream.Close();
	}

	public virtual void LoadFromJsonFile(string file)
	{
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		try
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
			string text = File.ReadAllText(file);
			JObject jObject = JObject.Parse(text);
			Styles.Clear();
			Styles.LoadFromJsonObject(jObject);
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), "Loading styles...ERROR");
			StiLogService.Write(GetType(), e);
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
		finally
		{
			Thread.CurrentThread.CurrentCulture = currentCulture;
		}
	}

	public StiStylesSheet(StiStylesCollection styles)
	{
		Styles = styles;
	}
}
