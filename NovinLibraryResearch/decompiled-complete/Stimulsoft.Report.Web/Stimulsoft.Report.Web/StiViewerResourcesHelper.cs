using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Text;
using Stimulsoft.Base;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Report.Web;

internal class StiViewerResourcesHelper
{
	public static StiWebActionResult Get(StiRequestParams requestParams)
	{
		if (requestParams.Resource == "scripts")
		{
			byte[] scripts = GetScripts(requestParams);
			return new StiWebActionResult(scripts, "text/javascript");
		}
		if (requestParams.Resource == "styles")
		{
			byte[] styles = GetStyles(requestParams);
			return new StiWebActionResult(styles, "text/css");
		}
		if (requestParams.Resource == "images")
		{
			Hashtable hashtable = new Hashtable();
			hashtable["images"] = GetImagesArray(requestParams);
			string @string = requestParams.GetString("sharingLocalization");
			if (!string.IsNullOrEmpty(@string))
			{
				requestParams.Localization = @string;
				hashtable["localizationItems"] = StiCollectionsHelper.GetLocalizationItems(requestParams);
			}
			return StiWebActionResult.JsonResult(requestParams, hashtable);
		}
		return new StiWebActionResult();
	}

	public static Bitmap GetBitmap(StiRequestParams requestParams, string imageName)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		Assembly assembly = typeof(StiViewerResourcesHelper).Assembly;
		string name = $"{typeof(StiViewerResourcesHelper).Namespace}.{requestParams.ComponentType}.Images.{imageName}";
		Stream manifestResourceStream = assembly.GetManifestResourceStream(name);
		if (manifestResourceStream == null)
		{
			return null;
		}
		return new Bitmap(manifestResourceStream);
	}

	public static Hashtable GetImagesArray(StiRequestParams requestParams, double? scaleFactor_ = null)
	{
		double num = (scaleFactor_.HasValue ? StiObjectConverter.ConvertToDouble(scaleFactor_) : Math.Max(requestParams.GetDouble("imagesScalingFactor"), 1.0));
		Hashtable hashtable = new Hashtable();
		Assembly assembly = typeof(StiViewerResourcesHelper).Assembly;
		List<string> list = new List<string>(assembly.GetManifestResourceNames());
		string text = $"{typeof(StiViewerResourcesHelper).Namespace}.{requestParams.ComponentType}.Images.";
		string scaleSuffix = StiScalingImagesHelper.GetScaleSuffix(num);
		string text2 = text + "Themes." + requestParams.Theme + ".";
		foreach (string item2 in list)
		{
			if ((!item2.EndsWith(".png") && !item2.EndsWith(".gif")) || (!requestParams.Scripts.IncludeDashboards && item2.IndexOf(".Dashboards") >= 0))
			{
				continue;
			}
			if (item2.StartsWith(text2))
			{
				string key = item2.Replace(text2, string.Empty);
				hashtable[key] = GetImageData(assembly, item2);
			}
			else
			{
				if (!item2.StartsWith(text))
				{
					continue;
				}
				string text3 = item2.Replace(text, string.Empty);
				string scaleSuffixFromName = StiScalingImagesHelper.GetScaleSuffixFromName(text3);
				string text4 = text3.Replace(scaleSuffixFromName + ".", ".");
				string text5 = text4.Substring(0, text4.LastIndexOf("."));
				string text6 = text4.Substring(text4.LastIndexOf(".") + 1);
				string item = text + text5 + scaleSuffix + "." + text6;
				if (scaleSuffixFromName != scaleSuffix && list.IndexOf(item) >= 0)
				{
					continue;
				}
				if (num > 1.0 && string.IsNullOrEmpty(scaleSuffixFromName))
				{
					try
					{
						Image scalingImage = StiScalingImagesHelper.GetScalingImage(assembly, text, text5, text6, num);
						using MemoryStream memoryStream = new MemoryStream();
						scalingImage.Save((Stream)memoryStream, ImageFormat.Png);
						hashtable[text4] = "data:image/png;base64," + Convert.ToBase64String(memoryStream.ToArray());
					}
					catch (Exception ex)
					{
						Console.WriteLine(ex.Message);
						hashtable[text4] = GetImageData(assembly, item2);
					}
				}
				else if (scaleSuffixFromName == scaleSuffix && !hashtable.Contains(text4))
				{
					hashtable[text4] = GetImageData(assembly, item2);
				}
			}
		}
		return hashtable;
	}

	internal static string GetImageData(Assembly assembly, string name)
	{
		Stream manifestResourceStream = assembly.GetManifestResourceStream(name);
		if (manifestResourceStream == null)
		{
			return null;
		}
		MemoryStream memoryStream = new MemoryStream();
		manifestResourceStream.CopyTo(memoryStream);
		byte[] inArray = memoryStream.ToArray();
		memoryStream.Dispose();
		manifestResourceStream.Dispose();
		string text = name.Substring(name.Length - 3);
		string text2 = Convert.ToBase64String(inArray);
		return "data:image/" + text + ";base64," + text2;
	}

	public static string GetCollections(StiRequestParams requestParams, bool includeImages = false)
	{
		Hashtable hashtable = new Hashtable();
		hashtable["loc"] = StiCollectionsHelper.GetLocalizationItems(requestParams);
		hashtable["encodingData"] = StiCollectionsHelper.GetEncodingDataItems();
		hashtable["dateRanges"] = StiCollectionsHelper.GetDateRangesItems();
		hashtable["months"] = new string[12]
		{
			"January", "February", "March", "April", "May", "June", "July", "August", "September", "October",
			"November", "December"
		};
		hashtable["dayOfWeek"] = new string[7] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
		hashtable["paperSizes"] = StiCollectionsHelper.GetPaperSizes();
		hashtable["pdfSecurityCertificates"] = StiCollectionsHelper.GetPdfSecurityCertificatesItems();
		if (includeImages)
		{
			hashtable["images"] = GetImagesArray(requestParams);
		}
		return JsonConvert.SerializeObject(hashtable, Formatting.None, new StringEnumConverter());
	}

	internal static byte[] GetScripts(StiRequestParams requestParams, bool forJSProject = false)
	{
		string guid = "ViewerScripts_" + StiMD5Helper.ComputeHash(requestParams.Theme + requestParams.Localization + requestParams.Version);
		if (requestParams.ComponentType == StiComponentType.Viewer)
		{
			string resourceInternal = requestParams.Cache.Helper.GetResourceInternal(requestParams, guid);
			if (!string.IsNullOrEmpty(resourceInternal))
			{
				return Encoding.UTF8.GetBytes(resourceInternal);
			}
		}
		Assembly assembly = typeof(StiViewerResourcesHelper).Assembly;
		string[] manifestResourceNames = assembly.GetManifestResourceNames();
		string value = $"{typeof(StiViewerResourcesHelper).Namespace}.{requestParams.ComponentType}.Scripts.";
		string text = string.Empty;
		string[] array = manifestResourceNames;
		foreach (string text2 in array)
		{
			if (text2.IndexOf(value) != 0 || !text2.EndsWith(".js") || (!requestParams.Scripts.IncludeDashboards && text2.IndexOf(".Dashboards.") >= 0) || (!requestParams.Scripts.IncludeClient && text2.EndsWith(".Scripts.Client.js")) || text2.IndexOf(".LoginControls.") >= 0 || text2.EndsWith("InitializeFolderReportsPanel.js"))
			{
				continue;
			}
			Stream manifestResourceStream = assembly.GetManifestResourceStream(text2);
			using (StreamReader streamReader = new StreamReader(manifestResourceStream))
			{
				string text3 = streamReader.ReadToEnd();
				if (text2.EndsWith(".Main.js") && requestParams.ComponentType == StiComponentType.Viewer)
				{
					string newValue = $"this.collections = {GetCollections(requestParams, forJSProject)}";
					text3 = text3.Replace("this.collections = {};", newValue);
				}
				text = text + text3 + "\r\n";
			}
			manifestResourceStream.Dispose();
		}
		if (requestParams.ComponentType == StiComponentType.Viewer)
		{
			requestParams.Cache.Helper.SaveResourceInternal(requestParams, text, guid);
		}
		return Encoding.UTF8.GetBytes(text);
	}

	private static Hashtable GetStylesConstants(string css)
	{
		Hashtable hashtable = new Hashtable();
		string[] array = css.Split(new char[1] { ';' });
		for (int i = 0; i < array.Length; i++)
		{
			int num = array[i].IndexOf('@');
			string[] array2 = array[i].Substring((num >= 0) ? num : 0).Split(new char[1] { '=' });
			if (array2.Length == 2)
			{
				hashtable[array2[0].Trim()] = array2[1].Trim();
			}
		}
		return hashtable;
	}

	public static byte[] GetStyles(StiRequestParams requestParams)
	{
		string guid = "ViewerStyles_" + StiMD5Helper.ComputeHash(requestParams.Theme.ToString() + requestParams.Version);
		string resourceInternal = requestParams.Cache.Helper.GetResourceInternal(requestParams, guid);
		if (!string.IsNullOrEmpty(resourceInternal))
		{
			return Encoding.UTF8.GetBytes(resourceInternal);
		}
		Assembly assembly = typeof(StiViewerResourcesHelper).Assembly;
		string[] manifestResourceNames = assembly.GetManifestResourceNames();
		string text = string.Empty;
		string arg = requestParams.Theme;
		if (requestParams.Theme.StartsWith("Simple"))
		{
			arg = "Simple";
		}
		else if (requestParams.Theme.StartsWith("Office2007"))
		{
			arg = "Office2007";
		}
		else if (requestParams.Theme.StartsWith("Office2010"))
		{
			arg = "Office2010";
		}
		else if (requestParams.Theme.StartsWith("Office2013"))
		{
			arg = "Office2013";
		}
		string value = $"{typeof(StiViewerResourcesHelper).Namespace}.{requestParams.ComponentType}.Styles.{arg}.";
		Hashtable hashtable = null;
		string[] array = manifestResourceNames;
		foreach (string text2 in array)
		{
			if (text2.IndexOf(value) != 0 || !text2.EndsWith(".css") || text2.EndsWith("FolderReportsPanelStyles.css"))
			{
				continue;
			}
			Stream manifestResourceStream = assembly.GetManifestResourceStream(text2);
			using StreamReader streamReader = new StreamReader(manifestResourceStream);
			string text3 = streamReader.ReadToEnd();
			if (text2.EndsWith(requestParams.Theme + ".Constants.css"))
			{
				hashtable = GetStylesConstants(text3);
			}
			else if (!text2.EndsWith(".Constants.css"))
			{
				text = text + text3 + "\r\n";
			}
		}
		if (hashtable != null)
		{
			foreach (DictionaryEntry item in hashtable)
			{
				text = text.Replace((string)item.Key, (string)item.Value);
			}
		}
		requestParams.Cache.Helper.SaveResourceInternal(requestParams, text, guid);
		return Encoding.UTF8.GetBytes(text);
	}
}
