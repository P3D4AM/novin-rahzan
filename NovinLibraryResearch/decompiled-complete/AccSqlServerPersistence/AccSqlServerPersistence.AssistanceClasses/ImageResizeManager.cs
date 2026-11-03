using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Entities;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace AccSqlServerPersistence.AssistanceClasses;

public class ImageResizeManager : IImageResizer
{
	public void LoadBitmapImage(string path)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		new Bitmap(path);
	}

	public async Task<ReturnedSaveFuncInfo> ReSizeAsync(FileInfoBusiness file, int MaxSize, int jpegQuality)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		if (file?.FileContent == null)
		{
			return ret;
		}
		int line = 0;
		try
		{
			line = 1;
			_ = file.FileName;
			try
			{
				line = 2;
				Bitmap val = new Bitmap((Stream)new MemoryStream(file.FileContent));
				try
				{
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			catch (Exception exception)
			{
				ret.AddReturnedValue(exception, "", "ReSizeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\AccSqlServerPersistence\\AssistanceClasses\\ImageResizeManager.cs", 37);
				try
				{
					line = 5;
					line = 6;
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await file.RemoveAsync(10), ReturnedState.None, "ReSizeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\AccSqlServerPersistence\\AssistanceClasses\\ImageResizeManager.cs", 43);
					line = 7;
					file = null;
					line = 8;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, $"AccSqlServerPersistence.AssistanceClasses//Line:{line}", "ReSizeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\AccSqlServerPersistence\\AssistanceClasses\\ImageResizeManager.cs", 50);
					ret.AddReturnedValue(ex, "", "ReSizeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\AccSqlServerPersistence\\AssistanceClasses\\ImageResizeManager.cs", 51);
				}
				line = 100;
				if (string.IsNullOrEmpty(file?.ContextGuid))
				{
					throw new NullReferenceException("if (string.IsNullOrEmpty(file?.ContextGuid))");
				}
				line = 101;
				return ret;
			}
			line = 14;
			Bitmap val2 = new Bitmap((Stream)new MemoryStream(file.FileContent));
			try
			{
				line = 15;
				if (((Image)val2).Width > MaxSize || ((Image)val2).Height > MaxSize || file.FileLen > 51200)
				{
					line = 16;
					int num = ((Image)val2).Height;
					line = 17;
					int num2 = ((Image)val2).Width;
					line = 18;
					if (num2 > MaxSize || num > MaxSize)
					{
						if (num2 > num)
						{
							line = 20;
							num = (int)Math.Round((float)(num * MaxSize) / (float)num2);
							num2 = MaxSize;
						}
						else
						{
							line = 25;
							num2 = (int)Math.Round((float)(num2 * MaxSize) / (float)num);
							num = MaxSize;
						}
					}
					line = 30;
					Image val3 = (Image)new Bitmap((Image)(object)val2, new Size(num2, num));
					line = 31;
					ImageConverter val4 = new ImageConverter();
					string text = new FileInfo(file.FileName).Extension.ToLower();
					if (text == ".jpg" || text == ".jpeg")
					{
						val3 = ChangeJpegImageQuality(val3, jpegQuality);
					}
					file.FileContent = (byte[])((TypeConverter)(object)val4).ConvertTo((object?)val3, typeof(byte[]));
					((Image)val2).Dispose();
					line = 33;
					val3.Dispose();
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		catch (Exception ex2)
		{
			string arg = "";
			if (file != null)
			{
				arg = file.GetLocalFilePath();
			}
			WebErrorLog.Instance.StartLog(ex2, $"AccSqlServerPersistence.AssistanceClasses//Line:{line}//FullPath:{arg}//fileName:{file?.FileName}", "ReSizeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\AccSqlServerPersistence\\AssistanceClasses\\ImageResizeManager.cs", 104);
			ret.AddReturnedValue(ex2, "", "ReSizeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\AccSqlServerPersistence\\AssistanceClasses\\ImageResizeManager.cs", 105);
		}
		return ret;
	}

	private static ImageCodecInfo GetEncoderInfo(string mimeType)
	{
		ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
		foreach (ImageCodecInfo val in imageEncoders)
		{
			if (val.MimeType == mimeType)
			{
				return val;
			}
		}
		return null;
	}

	private Image ChangeJpegImageQuality(Image img, int newQuality)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		try
		{
			Bitmap val = new Bitmap(img);
			try
			{
				ImageCodecInfo encoderInfo = GetEncoderInfo("image/jpeg");
				Encoder quality = Encoder.Quality;
				EncoderParameters val2 = new EncoderParameters(1);
				EncoderParameter val3 = new EncoderParameter(quality, (long)newQuality);
				val2.Param[0] = val3;
				MemoryStream memoryStream = new MemoryStream();
				((Image)val).Save((Stream)memoryStream, encoderInfo, val2);
				Image val4 = Image.FromStream((Stream)memoryStream);
				ImageAttributes val5 = new ImageAttributes();
				Graphics val6 = Graphics.FromImage(val4);
				try
				{
					val6.InterpolationMode = (InterpolationMode)7;
					val6.DrawImage(val4, new Rectangle(Point.Empty, val4.Size), 0, 0, val4.Width, val4.Height, (GraphicsUnit)2, val5);
				}
				finally
				{
					((IDisposable)val6)?.Dispose();
				}
				return val4;
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ChangeJpegImageQuality", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\AccSqlServerPersistence\\AssistanceClasses\\ImageResizeManager.cs", 151);
			return img;
		}
	}

	public async Task<bool> ReSizeAsync(string path, int MaxSize)
	{
		try
		{
			FileInfo fileInfo = new FileInfo(path);
			if (fileInfo.Extension.ToLower() == ".jpg" || fileInfo.Extension.ToLower() == ".jpeg")
			{
				Bitmap bm = new Bitmap(path);
				if (((Image)bm).Width > MaxSize || ((Image)bm).Height > MaxSize)
				{
					int num = ((Image)bm).Height;
					int num2 = ((Image)bm).Width;
					if (num2 > MaxSize || num > MaxSize)
					{
						if (num2 > num)
						{
							num = (int)Math.Round((float)(num * MaxSize) / (float)num2);
							num2 = MaxSize;
						}
						else
						{
							num2 = (int)Math.Round((float)(num2 * MaxSize) / (float)num);
							num = MaxSize;
						}
					}
					Bitmap val = new Bitmap((Image)(object)bm, new Size(num2, num));
					((Image)bm).Dispose();
					((Image)val).Save(path, ImageFormat.Jpeg);
					((Image)val).Dispose();
					await Task.Delay(500);
				}
				((Image)bm).Dispose();
				return true;
			}
			if (fileInfo.Extension.ToLower() == ".png" || fileInfo.Extension == ".gif")
			{
				return true;
			}
			return false;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ReSizeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\AccSqlServerPersistence\\AssistanceClasses\\ImageResizeManager.cs", 356);
			return false;
		}
	}
}
