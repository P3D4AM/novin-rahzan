using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using System.Xml;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Exceptions;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Server;

namespace Stimulsoft.Base.Helpers;

public static class StiSvgHelper
{
	public const string AssemblyName = "Stimulsoft.Svg";

	public const string AssemblyVersion = "2022.1.1";

	private static object lockObject = new object();

	public static string SvgAssemblyPrefix = "Stimulsoft.";

	private static Assembly svgAssembly;

	public static Assembly SvgAssembly
	{
		get
		{
			lock (lockObject)
			{
				if (svgAssembly == null && !TryToLoadAssembly)
				{
					TryToLoadAssembly = true;
					svgAssembly = StiAssemblyFinder.GetAssembly("Stimulsoft.Svg.dll");
					if (svgAssembly == null)
					{
						SvgAssemblyPrefix = string.Empty;
						svgAssembly = StiAssemblyFinder.GetAssembly("Svg.dll");
					}
					if (svgAssembly == null)
					{
						if (StiAccountSettings.CurrentAccount != null)
						{
							if (!StiBaseOptions.IsDashboardViewerWPF)
							{
								try
								{
									IStiNuGetDownloader nuGetDownloader = StiAccountSettings.CurrentAccount.AccountCreater.GetNuGetDownloader();
									nuGetDownloader.RunDownloadSvg();
									svgAssembly = StiAssemblyFinder.GetAssembly("Stimulsoft.Svg.dll");
								}
								catch
								{
								}
							}
						}
						else
						{
							IStiNotFoundProvider notFoundProvider = StiNotFoundProviderCreater.GetNotFoundProvider();
							notFoundProvider.Show("Stimulsoft.Svg");
						}
					}
				}
				return svgAssembly;
			}
		}
	}

	public static bool IsSvgAssemblyLoaded => svgAssembly != null;

	public static bool TryToLoadAssembly { get; set; }

	internal static Image ConvertSvgToImageScaled(byte[] svgObject, float svgScale = 1f)
	{
		Image val = ConvertSvgToImage(svgObject, 100, 100, stretch: false, aspectRatio: true);
		if (svgScale == 1f)
		{
			return val;
		}
		Image val2 = null;
		try
		{
			return ConvertSvgToImage(svgObject, (int)((float)val.Width * svgScale), (int)((float)val.Height * svgScale), stretch: true, aspectRatio: true);
		}
		catch (OutOfMemoryException)
		{
			if (svgScale < 1f)
			{
				return val;
			}
			svgScale = (svgScale + 1f) / 2f;
			try
			{
				return ConvertSvgToImage(svgObject, (int)((float)val.Width * svgScale), (int)((float)val.Height * svgScale), stretch: true, aspectRatio: true);
			}
			catch (OutOfMemoryException)
			{
				return val;
			}
		}
	}

	public static Image ConvertSvgToImage(byte[] svgObject, int width, int height, bool stretch = true, bool aspectRatio = false, bool rethrowOutOfMemory = false)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		if (svgObject == null || svgObject.Length == 0)
		{
			return null;
		}
		Image result = null;
		if (ConvertSvgToImageInternal(svgObject, width, height, stretch, aspectRatio, out result, rethrowOutOfMemory))
		{
			return result;
		}
		if (width < 250 && height < 30)
		{
			return null;
		}
		Bitmap val = new Bitmap(width, height);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			DrawSvgIsNotFound(val2, width, height);
			return (Image)(object)val;
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	private static bool ConvertSvgToImageInternal(byte[] svgObject, int width, int height, bool stretch, bool aspectRatio, out Image result, bool rethrowOutOfMemory)
	{
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Expected O, but got Unknown
		result = null;
		try
		{
			if (SvgAssembly == null)
			{
				return false;
			}
			if (svgObject == null || svgObject.Length == 0)
			{
				return true;
			}
			object obj = OpenSvg(svgObject);
			if (obj == null)
			{
				return false;
			}
			Type type = SvgAssembly.GetType(SvgAssemblyPrefix + "Svg.SvgUnit");
			PropertyInfo propertyInfo = null;
			if (type != null)
			{
				propertyInfo = type.GetProperty("Value");
			}
			bool flag = false;
			if ((!stretch || aspectRatio) && propertyInfo != null)
			{
				SizeF svgSize = GetSvgSize(obj);
				if (svgSize.Width != 0f && svgSize.Height != 0f)
				{
					if (svgSize.Width > (float)width || svgSize.Height > (float)height)
					{
						flag = true;
					}
					if (aspectRatio)
					{
						double num = svgSize.Width / svgSize.Height;
						double num2 = (float)width / (float)height;
						if (num > num2)
						{
							height = (int)Math.Round((double)width / num);
						}
						else
						{
							width = (int)Math.Round((double)height * num);
						}
					}
				}
			}
			if (stretch)
			{
				MethodInfo method = obj.GetType().GetMethod("Draw", new Type[2]
				{
					typeof(int),
					typeof(int)
				});
				if (method == null)
				{
					return false;
				}
				object? obj2 = method.Invoke(obj, new object[2] { width, height });
				result = (Image)((obj2 is Image) ? obj2 : null);
			}
			else if (flag)
			{
				Bitmap val = new Bitmap(width, height);
				MethodInfo method2 = obj.GetType().GetMethod("Draw", new Type[1] { typeof(Bitmap) });
				if (method2 == null)
				{
					return false;
				}
				object? obj3 = method2.Invoke(obj, new object[1] { val });
				object obj4 = ((obj3 is Image) ? obj3 : null);
				result = (Image)(object)val;
			}
			else
			{
				MethodInfo method3 = obj.GetType().GetMethod("Draw", new Type[0]);
				if (method3 == null)
				{
					return false;
				}
				object? obj5 = method3.Invoke(obj, new object[0]);
				result = (Image)((obj5 is Image) ? obj5 : null);
			}
			return true;
		}
		catch (Exception ex)
		{
			if (ex.InnerException is OutOfMemoryException && rethrowOutOfMemory)
			{
				throw new OutOfMemoryException();
			}
			return false;
		}
	}

	public static bool DrawSvg(byte[] svgObject, RectangleF rect, bool stretch, bool aspectRatio, double zoom, Graphics gr)
	{
		if (svgObject == null || svgObject.Length == 0)
		{
			return true;
		}
		if (DrawSvgInternal(svgObject, rect, stretch, aspectRatio, zoom, gr))
		{
			return true;
		}
		return false;
	}

	private static bool DrawSvgInternal(byte[] svgObject, RectangleF rect, bool stretch, bool aspectRatio, double zoom, Graphics gr)
	{
		try
		{
			if (SvgAssembly == null)
			{
				return false;
			}
			if (svgObject == null || svgObject.Length == 0)
			{
				return true;
			}
			object obj = OpenSvg(svgObject);
			if (obj == null)
			{
				return false;
			}
			SizeF svgSize = GetSvgSize(obj);
			float num = rect.Width;
			float num2 = rect.Height;
			if ((!stretch || aspectRatio) && svgSize.Width != 0f && svgSize.Height != 0f)
			{
				if (aspectRatio)
				{
					double num3 = svgSize.Width / svgSize.Height;
					double num4 = num / num2;
					if (num3 > num4)
					{
						num2 = (float)Math.Round((double)num / num3);
					}
					else
					{
						num = (float)Math.Round((double)num2 * num3);
					}
				}
				if (!stretch)
				{
					num = svgSize.Width * (float)zoom;
					num2 = svgSize.Height * (float)zoom;
				}
			}
			GraphicsState val = gr.Save();
			gr.TranslateTransform(rect.X, rect.Y);
			gr.ScaleTransform(num / svgSize.Width, num2 / svgSize.Height);
			MethodInfo method = obj.GetType().GetMethod("Draw", new Type[1] { typeof(Graphics) });
			if (method == null)
			{
				return false;
			}
			method.Invoke(obj, new object[1] { gr });
			gr.Restore(val);
			return true;
		}
		catch
		{
			return false;
		}
	}

	public static SizeF GetSvgSize(object document)
	{
		SizeF result = default(Size);
		Type type = SvgAssembly.GetType(SvgAssemblyPrefix + "Svg.SvgDocument");
		Type type2 = SvgAssembly.GetType(SvgAssemblyPrefix + "Svg.SvgUnit");
		PropertyInfo propertyInfo = null;
		if (type2 != null)
		{
			propertyInfo = type2.GetProperty("Value");
		}
		Type type3 = SvgAssembly.GetType(SvgAssemblyPrefix + "Svg.SvgUnitType");
		PropertyInfo propertyInfo2 = null;
		if (type3 != null)
		{
			propertyInfo2 = type2.GetProperty("Type");
		}
		if (propertyInfo != null)
		{
			float width = 0f;
			float height = 0f;
			PropertyInfo property = type.GetProperty("Width");
			if (property != null)
			{
				object value = property.GetValue(document, new object[0]);
				if (value != null)
				{
					object value2 = propertyInfo2.GetValue(value, new object[0]);
					if (value2 != null && value2.ToString().ToLowerInvariant() == "percentage")
					{
						return result;
					}
					object value3 = propertyInfo.GetValue(value, new object[0]);
					width = (float)value3;
				}
			}
			PropertyInfo property2 = type.GetProperty("Height");
			if (property2 != null)
			{
				object value4 = property2.GetValue(document, new object[0]);
				if (value4 != null)
				{
					object value5 = propertyInfo2.GetValue(value4, new object[0]);
					if (value5 != null && value5.ToString().ToLowerInvariant() == "percentage")
					{
						return result;
					}
					object value6 = propertyInfo.GetValue(value4, new object[0]);
					height = (float)value6;
				}
			}
			result.Width = width;
			result.Height = height;
		}
		return result;
	}

	public static object OpenSvg(byte[] svgObject)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.XmlResolver = null;
		using (MemoryStream inStream = new MemoryStream(svgObject))
		{
			xmlDocument.Load(inStream);
		}
		XmlAttribute xmlAttribute = xmlDocument.DocumentElement.Attributes["width"];
		XmlAttribute xmlAttribute2 = xmlDocument.DocumentElement.Attributes["height"];
		XmlAttribute xmlAttribute3 = xmlDocument.DocumentElement.Attributes["viewBox"];
		if (xmlAttribute != null && xmlAttribute.Value == "100%" && xmlAttribute2 != null && xmlAttribute2.Value == "100%" && xmlAttribute3 != null && !string.IsNullOrWhiteSpace(xmlAttribute3.Value))
		{
			string[] array = xmlAttribute3.Value.Split(new char[1] { ' ' });
			if (array.Length == 4)
			{
				xmlAttribute.Value = array[2];
				xmlAttribute2.Value = array[3];
			}
		}
		CheckDescRecursive(xmlDocument.DocumentElement);
		Type type = SvgAssembly.GetType(SvgAssemblyPrefix + "Svg.SvgDocument");
		if (type == null)
		{
			return null;
		}
		MethodInfo method = type.GetMethod("Open", new Type[1] { typeof(XmlDocument) });
		if (method == null)
		{
			return null;
		}
		return method.Invoke(null, new object[1] { xmlDocument });
	}

	private static void CheckDescRecursive(XmlNode baseNode)
	{
		foreach (XmlNode childNode in baseNode.ChildNodes)
		{
			if (childNode.HasChildNodes)
			{
				if (childNode.NodeType == XmlNodeType.Element && childNode.Name == "text" && childNode.ChildNodes.Count == 2 && childNode.ChildNodes[0].Name == "desc" && childNode.ChildNodes[1].NodeType == XmlNodeType.Text)
				{
					childNode.RemoveChild(childNode.ChildNodes[0]);
				}
				else
				{
					CheckDescRecursive(childNode);
				}
			}
		}
	}

	public static bool DrawWithSvg(string svgLocator, RectangleF rectF, RectangleF rectSVG, double zoom, Graphics gr)
	{
		try
		{
			Rectangle rectangle = Rectangle.Ceiling(rectF);
			if (SvgAssembly == null)
			{
				DrawSvgIsNotFound(gr, rectangle.Width, rectangle.Height);
				return false;
			}
			Type type = SvgAssembly.GetType(SvgAssemblyPrefix + "Svg.SvgDocument");
			if (type == null)
			{
				return false;
			}
			Size size = new Size(rectangle.Width, rectangle.Height);
			GraphicsState val = gr.Save();
			gr.SetClip(rectangle, (CombineMode)1);
			gr.TranslateTransform((float)rectangle.X + rectSVG.X, (float)rectangle.Y + rectSVG.Y);
			DrawWithSvgInternal(gr, type, svgLocator);
			gr.Restore(val);
			return true;
		}
		catch
		{
			return false;
		}
	}

	public static void DrawWithSvgInternal(Graphics g, Type typeSvgDocument, string svgLocator)
	{
		try
		{
			MethodInfo method = typeSvgDocument.GetMethod("FromSvg", new Type[1] { typeof(string) });
			MethodInfo methodInfo = method.MakeGenericMethod(SvgAssembly.GetType(SvgAssemblyPrefix + "Svg.SvgDocument"));
			object obj = methodInfo.Invoke(null, new object[1] { svgLocator });
			MethodInfo method2 = obj.GetType().GetMethod("Draw", new Type[1] { typeof(Graphics) });
			method2.Invoke(obj, new object[1] { g });
		}
		catch
		{
		}
	}

	private static void DrawSvgIsNotFound(Graphics g, int width, int height)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		Font val = new Font("Arial", 8f, (FontStyle)1);
		try
		{
			object obj = StringFormat.GenericDefault.Clone();
			StringFormat val2 = (StringFormat)((obj is StringFormat) ? obj : null);
			try
			{
				Pen val3 = new Pen(StiUX.IsDark ? StiUX.ItemForeground : Color.Gray);
				try
				{
					if (!StiUX.IsDark)
					{
						val3.DashStyle = (DashStyle)1;
					}
					val2.Alignment = (StringAlignment)1;
					val2.LineAlignment = (StringAlignment)1;
					g.Clear(StiUX.Background);
					g.DrawString(string.Format(StiLocalization.Get("Notices", "IsNotFound"), "Stimulsoft.Svg.dll"), val, Brushes.Gray, (RectangleF)new Rectangle(0, 0, width, height), val2);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static Bitmap ConvertSvgToImage(string svgLocator, Rectangle rectF, RectangleF rectSVG, double zoom)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		Rectangle rectangle = Rectangle.Ceiling(rectF);
		Bitmap val = new Bitmap(rectangle.Width, rectangle.Height);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			DrawWithSvg(svgLocator, rectF, rectSVG, zoom, val2);
			return val;
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	public static bool IsSvgFile(string fileName)
	{
		if (string.IsNullOrWhiteSpace(fileName))
		{
			return false;
		}
		if (!File.Exists(fileName))
		{
			return false;
		}
		byte[] data = File.ReadAllBytes(fileName);
		return IsSvg(data);
	}

	public static bool IsSvg(byte[] data)
	{
		if (data == null || data.Length < 5)
		{
			return false;
		}
		int num = Math.Min(data.Length - 4, 1000);
		Stack<bool> stack = new Stack<bool>();
		bool flag = false;
		for (int i = 0; i < num; i++)
		{
			if (data[i] == 60)
			{
				if (stack.Count == 0 && data[i] == 60 && data[i + 1] == 115 && data[i + 2] == 118 && data[i + 3] == 103 && char.IsWhiteSpace((char)data[i + 4]))
				{
					return true;
				}
				if (data[i + 1] == 47)
				{
					if (stack.Count > 0)
					{
						flag = stack.Pop();
					}
					continue;
				}
				stack.Push(flag);
				if (data[i + 1] == 33 || data[i + 1] == 63)
				{
					flag = true;
				}
			}
			else if (data[i] == 62 && (flag || (i > 0 && data[i - 1] == 47)) && stack.Count > 0)
			{
				flag = stack.Pop();
			}
		}
		return false;
	}
}
