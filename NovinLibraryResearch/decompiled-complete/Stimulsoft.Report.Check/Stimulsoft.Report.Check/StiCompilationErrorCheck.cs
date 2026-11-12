using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using Stimulsoft.Base;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiCompilationErrorCheck : StiReportCheck
{
	public override bool PreviewVisible => true;

	public CompilerError Error { get; set; }

	public string ComponentName { get; set; }

	public string PropertyName { get; set; }

	public override string ShortMessage
	{
		get
		{
			if (Error != null)
			{
				return string.Format(StiLocalizationExt.Get("CheckReport", "StiCompilationErrorCheckShort"), ElementName, Error.ErrorText);
			}
			return StiLocalizationExt.Get("CheckReport", "StiCompilationErrorCheckShort");
		}
	}

	public override string LongMessage
	{
		get
		{
			if (Error == null)
			{
				return StiLocalizationExt.Get("CheckReport", "StiCompilationErrorCheckLong");
			}
			string text = ((ComponentName == null) ? string.Format(StiLocalizationExt.Get("CheckReport", "StiCompilationErrorCheckLong"), ElementName) : ((!PropertyName.EndsWith("Event")) ? string.Format(StiLocalizationExt.Get("CheckReport", "StiCompilationErrorCheck2Long"), ElementName, PropertyName, ComponentName) : string.Format(StiLocalizationExt.Get("CheckReport", "StiCompilationErrorCheck3Long"), ElementName, PropertyName.Substring(0, PropertyName.Length - "Event".Length), ComponentName)));
			return text + "\n" + Error.ErrorText;
		}
	}

	public override StiCheckStatus Status
	{
		get
		{
			if (Error == null || !Error.IsWarning)
			{
				return StiCheckStatus.Error;
			}
			return StiCheckStatus.Warning;
		}
	}

	public override void CreatePreviewImage(out Image elementImage, out Image highlightedElementImage, bool useScale = false)
	{
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Expected O, but got Unknown
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Expected O, but got Unknown
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Expected O, but got Unknown
		StiReport stiReport = base.Element as StiReport;
		if (ComponentName != null)
		{
			StiComponent componentByName = stiReport.GetComponentByName(ComponentName);
			if (componentByName != null)
			{
				base.Element = componentByName;
				base.CreatePreviewImage(out elementImage, out highlightedElementImage, useScale);
				base.Element = stiReport;
				return;
			}
		}
		string script = stiReport.Script;
		stiReport.ScriptUnpack();
		string script2 = stiReport.Script;
		stiReport.Script = script;
		int num = 0;
		int i = 0;
		if (num != Error.Line)
		{
			for (; i < script2.Length; i++)
			{
				char c = script2[i];
				if (c == '\r')
				{
					num++;
					if (num == Error.Line)
					{
						break;
					}
				}
			}
		}
		int num2 = i;
		if (num2 != 0)
		{
			int num3 = 12;
			while (true)
			{
				num2--;
				if (num2 == 0)
				{
					break;
				}
				char c2 = script2[num2];
				if (c2 == '\r')
				{
					num3--;
					if (num3 == 0)
					{
						break;
					}
				}
			}
		}
		int num4 = i;
		if (num4 != script2.Length - 1)
		{
			int num5 = 11;
			while (true)
			{
				num4++;
				if (num4 == script2.Length - 1)
				{
					break;
				}
				char c3 = script2[num4];
				if (c3 == '\r')
				{
					num5--;
					if (num5 == 0)
					{
						break;
					}
				}
			}
		}
		float num6 = (useScale ? 16 : 8);
		Font val = new Font("Arial", num6);
		try
		{
			float height = val.GetHeight();
			int num7 = 500;
			int num8 = (int)(24f * height);
			if (useScale)
			{
				num7 *= 2;
				num8 *= 2;
			}
			string text = script2.Substring(num2, num4 - num2);
			Bitmap val2 = new Bitmap(num7, num8);
			Graphics val3 = Graphics.FromImage((Image)(object)val2);
			try
			{
				float y = ((float)num8 - val3.MeasureString(text, val).Height) / 2f - height / 2f;
				val3.Clear(Color.White);
				val3.DrawString(text, val, Brushes.Black, new PointF(0f, y));
				val3.DrawRectangle(Pens.Gray, new Rectangle(0, 0, ((Image)val2).Width - 1, ((Image)val2).Height - 1));
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
			elementImage = (Image)(object)val2;
			object obj = ((Image)val2).Clone();
			Image val4 = (Image)((obj is Image) ? obj : null);
			Graphics val5 = Graphics.FromImage(val4);
			try
			{
				Pen val6 = new Pen(Color.FromArgb(2013200384));
				try
				{
					val6.Width = 3f;
					val5.DrawRectangle(val6, 2f, (float)(val4.Height / 2) - height / 2f, (float)(val4.Width - 4), height);
				}
				finally
				{
					((IDisposable)val6)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val5)?.Dispose();
			}
			highlightedElementImage = val4;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private List<int> SplitScriptToLines(string script)
	{
		List<int> list = new List<int>();
		int i = 0;
		while (i < script.Length)
		{
			list.Add(i);
			for (; i < script.Length; i++)
			{
				char c = script[i];
				if (c != '\r' && c != '\n')
				{
					continue;
				}
				if (i + 1 < script.Length)
				{
					char c2 = script[i + 1];
					if ((c2 == '\r' || c2 == '\n') && c2 != c)
					{
						i++;
					}
				}
				break;
			}
			if (i < script.Length)
			{
				i++;
			}
		}
		list.Add(i);
		return list;
	}

	private string[] FindCheckerInfo(StiReport report, string script, List<int> lines, CompilerError error)
	{
		string text = ((report.ScriptLanguage == StiReportLanguageType.CSharp || report.ScriptLanguage == StiReportLanguageType.JS) ? "// " : "'");
		string text2 = text + "CheckerInfo:";
		int num = error.Line - 1;
		while (num > 0 && num < lines.Count)
		{
			int num2 = lines[num];
			int num3 = ((num + 1 < lines.Count) ? (lines[num + 1] - lines[num]) : (-1));
			if (num3 != -1 && num2 >= 0 && num2 < script.Length && num2 + num3 < script.Length)
			{
				string text3 = script.Substring(num2, num3).Trim();
				if (text3.StartsWith(text))
				{
					if (!text3.StartsWith(text2))
					{
						break;
					}
					text3 = text3.Substring(text2.Length).Trim();
					int num4 = text3.IndexOf(' ');
					if (num4 != -1)
					{
						return new string[2]
						{
							text3.Substring(0, num4),
							text3.Substring(num4).Trim()
						};
					}
				}
			}
			num--;
		}
		return null;
	}

	public override object ProcessCheck(StiReport report, object obj)
	{
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Expected O, but got Unknown
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Expected O, but got Unknown
		if (report.CalculationMode == StiCalculationMode.Interpretation)
		{
			return null;
		}
		base.Element = obj;
		if (report.CompilerResults != null)
		{
			string script = report.Script;
			report.ScriptUnpack();
			string script2 = report.Script.Replace("+\r\n", "+ ");
			report.Script = script;
			List<int> lines = SplitScriptToLines(script2);
			List<StiCheck> list = new List<StiCheck>();
			{
				foreach (CompilerError item in (CollectionBase)(object)report.CompilerResults.Errors)
				{
					CompilerError error = item;
					StiCompilationErrorCheck stiCompilationErrorCheck = new StiCompilationErrorCheck
					{
						Element = obj,
						Error = error
					};
					string[] array = FindCheckerInfo(report, script2, lines, error);
					if (array != null)
					{
						stiCompilationErrorCheck.PropertyName = array[0];
						stiCompilationErrorCheck.ComponentName = array[1];
						stiCompilationErrorCheck.Actions.Add(new StiEditPropertyAction());
					}
					list.Add(stiCompilationErrorCheck);
					if (report.Designer != null)
					{
						stiCompilationErrorCheck.Actions.Add(new StiGoToCodeAction());
					}
				}
				return list;
			}
		}
		if (report.CompilerResults == null && report.CompiledReport == null && report.ReferencedAssemblies != null)
		{
			List<StiCheck> list2 = new List<StiCheck>();
			string[] referencedAssemblies = report.ReferencedAssemblies;
			foreach (string text in referencedAssemblies)
			{
				if (text == null || text.Trim().Length == 0)
				{
					continue;
				}
				switch (text.Trim().ToLowerInvariant())
				{
				case "stimulsoft.controls.dll":
				case "stimulsoft.base.dll":
				case "stimulsoft.report.dll":
				case "stimulsoft.report.design.dll":
				case "stimulsoft.report.odbcdatabase.dll":
				case "stimulsoft.report.oracledatabase.dll":
					continue;
				}
				try
				{
					Assembly assembly = StiAssemblyFinder.GetAssembly(text);
					if (assembly == null)
					{
						Assembly.Load(text);
					}
				}
				catch (Exception ex)
				{
					if (ex is FileNotFoundException ex2)
					{
						string format = StiLocalizationExt.Get("CheckReport", "StiCompilationErrorAssemblyCheckLong");
						list2.Add(new StiCompilationErrorCheck
						{
							Element = report,
							Error = new CompilerError("", 0, 0, "", string.Format(format, ex2.FileName))
						});
					}
				}
			}
			if (list2.Count > 0)
			{
				return list2;
			}
		}
		return null;
	}
}
