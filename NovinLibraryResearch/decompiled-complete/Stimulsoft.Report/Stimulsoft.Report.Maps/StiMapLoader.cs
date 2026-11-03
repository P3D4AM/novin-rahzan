using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using Stimulsoft.Base;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Map;
using Stimulsoft.Base.Maps.Geoms;
using Stimulsoft.Report.Maps.Helpers;

namespace Stimulsoft.Report.Maps;

public static class StiMapLoader
{
	private enum StiGeomIdent
	{
		None = 1,
		MoveToM,
		MoveTom,
		Line_L,
		Line_l,
		Bezier_C,
		Bezier_c,
		Beziers_S,
		Beziers_s,
		QuadraticCurve_T,
		QuadraticCurve_t,
		VerticalLineto_V,
		VerticalLineto_v,
		HorizontalLineto_H,
		HorizontalLineto_h,
		Close
	}

	private static Hashtable hashMaps;

	private static IStiMapResourceFinder resourceFinder;

	private static bool isLoading;

	public static bool IsMapAssemblyLoaded { get; private set; }

	public static void DeleteAllCustomMaps()
	{
		if (hashMaps == null || hashMaps.Count <= 0)
		{
			return;
		}
		List<string> list = new List<string>();
		foreach (string key in hashMaps.Keys)
		{
			StiMapSvgContainer stiMapSvgContainer = (StiMapSvgContainer)hashMaps[key];
			if (stiMapSvgContainer.IsCustom)
			{
				list.Add(key);
			}
		}
		foreach (string item in list)
		{
			hashMaps.Remove(item);
		}
	}

	private static string FinalResourceNmae(string resourceName, string lang)
	{
		if (resourceName.ToLowerInvariant() == "Germany".ToLowerInvariant() && lang == "DE")
		{
			return "Germany_DE";
		}
		if (resourceName.ToLowerInvariant() == "France".ToLowerInvariant() && lang == "FR")
		{
			return "France_FR";
		}
		if (resourceName.ToLowerInvariant() == "Italy".ToLowerInvariant() && lang == "IT")
		{
			return "Italy_IT";
		}
		if (resourceName.ToLowerInvariant() == "Russia".ToLowerInvariant() && lang == "RU")
		{
			return "Russia_RU";
		}
		return resourceName;
	}

	public static StiMapSvgContainer LoadResource(StiReport report, string resourceName, string lang)
	{
		if (hashMaps == null)
		{
			hashMaps = new Hashtable();
		}
		string text = FinalResourceNmae(resourceName, lang);
		if (hashMaps.ContainsKey(text))
		{
			return (StiMapSvgContainer)hashMaps[text];
		}
		if (resourceFinder == null)
		{
			if (isLoading)
			{
				return null;
			}
			try
			{
				isLoading = true;
				Assembly assembly = StiAssemblyFinder.GetAssembly("Stimulsoft.Map, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", true);
				if (assembly == null)
				{
					return null;
				}
				IsMapAssemblyLoaded = true;
				Type type = StiTypeFinder.GetType("Stimulsoft.Map.StiMapResourceFinder, Stimulsoft.Map, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a");
				object? obj = Activator.CreateInstance(type);
				resourceFinder = (IStiMapResourceFinder)((obj is IStiMapResourceFinder) ? obj : null);
				if (resourceFinder == null)
				{
					return null;
				}
			}
			catch
			{
			}
		}
		StiMapSvgContainer stiMapSvgContainer;
		if (StiCustomMapFinder.IsCustom(text))
		{
			stiMapSvgContainer = StiCustomMapFinder.GetContainer(report, text);
		}
		else
		{
			string text2 = resourceFinder.Get(text);
			stiMapSvgContainer = new StiMapSvgContainer();
			JsonConvert.PopulateObject(text2, (object)stiMapSvgContainer, StiJsonHelper.DefaultSerializerSettings);
			stiMapSvgContainer.Prepare();
		}
		if (!hashMaps.ContainsKey(text))
		{
			hashMaps.Add(text, stiMapSvgContainer);
		}
		return stiMapSvgContainer;
	}

	private static PointD Reflect(double pointX, double pointY, PointD mirror)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		double num = Math.Abs(((PointD)(ref mirror)).X - pointX);
		double num2 = Math.Abs(((PointD)(ref mirror)).Y - pointY);
		double num3 = ((PointD)(ref mirror)).X + ((((PointD)(ref mirror)).X >= pointX) ? num : (0.0 - num));
		double num4 = ((PointD)(ref mirror)).Y + ((((PointD)(ref mirror)).Y >= pointY) ? num2 : (0.0 - num2));
		return new PointD(num3, num4);
	}

	public static StiMapGeomsContainer GetGeomsObject(StiReport report, string resourceName, string lang)
	{
		StiMapSvgContainer stiMapSvgContainer = LoadResource(report, resourceName, lang);
		if (stiMapSvgContainer == null)
		{
			return new StiMapGeomsContainer();
		}
		StiMapGeomsContainer stiMapGeomsContainer = new StiMapGeomsContainer
		{
			Width = stiMapSvgContainer.Width,
			Height = stiMapSvgContainer.Height,
			Name = stiMapSvgContainer.Name
		};
		if (stiMapSvgContainer.Geoms == null)
		{
			TimeSpan duration = TimeSpan.FromMilliseconds(100.0);
			TimeSpan beginTime = TimeSpan.Zero;
			foreach (string key in stiMapSvgContainer.HashPaths.Keys)
			{
				StiMapSvg stiMapSvg = stiMapSvgContainer.HashPaths[key];
				StiMapGeomsObject item = new StiMapGeomsObject
				{
					Key = key,
					Geoms = ParsePath(stiMapSvg.Data),
					Animation = new StiOpacityAnimation(duration, beginTime)
				};
				stiMapGeomsContainer.Geoms.Add(item);
				beginTime = TimeSpan.FromMilliseconds(200 / stiMapSvgContainer.HashPaths.Count * stiMapGeomsContainer.Geoms.Count);
			}
			stiMapSvgContainer.Geoms = stiMapGeomsContainer.Geoms;
		}
		else
		{
			stiMapGeomsContainer.Geoms = stiMapSvgContainer.Geoms;
		}
		return stiMapGeomsContainer;
	}

	private static void CreateGeom(StiGeomIdent ident, List<double> values, StiMapGeomCollection geoms, bool skipException = false)
	{
		//IL_0668: Unknown result type (might be due to invalid IL or missing references)
		//IL_066d: Unknown result type (might be due to invalid IL or missing references)
		//IL_06be: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_071c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0721: Unknown result type (might be due to invalid IL or missing references)
		//IL_0772: Unknown result type (might be due to invalid IL or missing references)
		//IL_0777: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_054e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0553: Unknown result type (might be due to invalid IL or missing references)
		//IL_0859: Unknown result type (might be due to invalid IL or missing references)
		//IL_085e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_03db: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		switch (ident)
		{
		case StiGeomIdent.MoveToM:
		{
			if (values.Count != 2)
			{
				if (skipException)
				{
					return;
				}
				throw new NotSupportedException();
			}
			StiMoveToMapGeom item6 = new StiMoveToMapGeom
			{
				X = Math.Round(values[0], 3),
				Y = Math.Round(values[1], 3)
			};
			geoms.Add(item6);
			break;
		}
		case StiGeomIdent.MoveTom:
		{
			if (values.Count != 2)
			{
				if (skipException)
				{
					return;
				}
				throw new NotSupportedException();
			}
			PointD lastPoint8 = geoms.GetLastPoint();
			StiMoveToMapGeom item11 = new StiMoveToMapGeom
			{
				X = Math.Round(((PointD)(ref lastPoint8)).X + values[0], 3),
				Y = Math.Round(((PointD)(ref lastPoint8)).Y + values[1], 3)
			};
			geoms.Add(item11);
			break;
		}
		case StiGeomIdent.Line_L:
			if (values.Count != 2)
			{
				if (skipException)
				{
					return;
				}
				if (values.Count % 2 != 0)
				{
					throw new NotSupportedException();
				}
				for (int k = 0; k < values.Count; k += 2)
				{
					StiLineMapGeom item8 = new StiLineMapGeom
					{
						X = Math.Round(values[k], 3),
						Y = Math.Round(values[k + 1], 3)
					};
					geoms.Add(item8);
				}
			}
			else
			{
				StiLineMapGeom item9 = new StiLineMapGeom
				{
					X = Math.Round(values[0], 3),
					Y = Math.Round(values[1], 3)
				};
				geoms.Add(item9);
			}
			break;
		case StiGeomIdent.Line_l:
		{
			if (values.Count % 2 != 0)
			{
				if (skipException)
				{
					return;
				}
				throw new NotSupportedException();
			}
			PointD lastPoint9 = geoms[geoms.Count - 1].GetLastPoint();
			if (values.Count == 2)
			{
				StiLineMapGeom item12 = new StiLineMapGeom
				{
					X = Math.Round(((PointD)(ref lastPoint9)).X + values[0], 3),
					Y = Math.Round(((PointD)(ref lastPoint9)).Y + values[1], 3)
				};
				geoms.Add(item12);
				break;
			}
			for (int l = 0; l < values.Count; l += 2)
			{
				StiLineMapGeom stiLineMapGeom = new StiLineMapGeom
				{
					X = Math.Round(((PointD)(ref lastPoint9)).X + values[l], 3),
					Y = Math.Round(((PointD)(ref lastPoint9)).Y + values[l + 1], 3)
				};
				geoms.Add(stiLineMapGeom);
				((PointD)(ref lastPoint9))._002Ector(stiLineMapGeom.X, stiLineMapGeom.Y);
			}
			break;
		}
		case StiGeomIdent.QuadraticCurve_T:
		case StiGeomIdent.QuadraticCurve_t:
		{
			StiQuadraticCurveMapGeom stiQuadraticCurveMapGeom = null;
			PointD lastPoint7;
			PointD val;
			if (values.Count != 2)
			{
				if (skipException)
				{
					return;
				}
				if (values.Count % 2 != 0)
				{
					throw new NotSupportedException();
				}
				for (int j = 0; j < values.Count; j += 2)
				{
					stiQuadraticCurveMapGeom = geoms.LastOrDefault() as StiQuadraticCurveMapGeom;
					lastPoint7 = geoms[geoms.Count - 1].GetLastPoint();
					val = ((stiQuadraticCurveMapGeom != null) ? Reflect(stiQuadraticCurveMapGeom.ControlPointX, stiQuadraticCurveMapGeom.ControlPointY, lastPoint7) : lastPoint7);
					StiQuadraticCurveMapGeom stiQuadraticCurveMapGeom2 = new StiQuadraticCurveMapGeom
					{
						StartX = ((PointD)(ref lastPoint7)).X,
						StartY = ((PointD)(ref lastPoint7)).Y,
						ControlPointX = ((PointD)(ref val)).X,
						ControlPointY = ((PointD)(ref val)).Y,
						EndX = Math.Round(values[j], 3),
						EndY = Math.Round(values[j + 1], 3)
					};
					if (ident == StiGeomIdent.QuadraticCurve_t)
					{
						stiQuadraticCurveMapGeom2.EndX += ((PointD)(ref lastPoint7)).X;
						stiQuadraticCurveMapGeom2.EndY += ((PointD)(ref lastPoint7)).Y;
					}
					geoms.Add(stiQuadraticCurveMapGeom2);
				}
			}
			else
			{
				stiQuadraticCurveMapGeom = geoms.LastOrDefault() as StiQuadraticCurveMapGeom;
				lastPoint7 = geoms[geoms.Count - 1].GetLastPoint();
				val = ((stiQuadraticCurveMapGeom != null) ? Reflect(stiQuadraticCurveMapGeom.ControlPointX, stiQuadraticCurveMapGeom.ControlPointY, lastPoint7) : lastPoint7);
				StiQuadraticCurveMapGeom stiQuadraticCurveMapGeom3 = new StiQuadraticCurveMapGeom
				{
					StartX = ((PointD)(ref lastPoint7)).X,
					StartY = ((PointD)(ref lastPoint7)).Y,
					ControlPointX = ((PointD)(ref val)).X,
					ControlPointY = ((PointD)(ref val)).Y,
					EndX = Math.Round(values[0], 3),
					EndY = Math.Round(values[1], 3)
				};
				if (ident == StiGeomIdent.QuadraticCurve_t)
				{
					stiQuadraticCurveMapGeom3.EndX += ((PointD)(ref lastPoint7)).X;
					stiQuadraticCurveMapGeom3.EndY += ((PointD)(ref lastPoint7)).Y;
				}
				geoms.Add(stiQuadraticCurveMapGeom3);
			}
			break;
		}
		case StiGeomIdent.Bezier_C:
		{
			if (values.Count != 6)
			{
				if (skipException)
				{
					return;
				}
				throw new NotSupportedException();
			}
			StiBezierMapGeom item10 = new StiBezierMapGeom
			{
				X1 = Math.Round(values[0], 3),
				Y1 = Math.Round(values[1], 3),
				X2 = Math.Round(values[2], 3),
				Y2 = Math.Round(values[3], 3),
				X3 = Math.Round(values[4], 3),
				Y3 = Math.Round(values[5], 3)
			};
			geoms.Add(item10);
			break;
		}
		case StiGeomIdent.Bezier_c:
		{
			if (values.Count % 6 != 0)
			{
				if (skipException)
				{
					return;
				}
				throw new NotSupportedException();
			}
			PointD lastPoint6 = geoms[geoms.Count - 1].GetLastPoint();
			int num = values.Count / 6;
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				StiBezierMapGeom stiBezierMapGeom = new StiBezierMapGeom
				{
					X1 = Math.Round(((PointD)(ref lastPoint6)).X + values[num2], 3),
					Y1 = Math.Round(((PointD)(ref lastPoint6)).Y + values[num2 + 1], 3),
					X2 = Math.Round(((PointD)(ref lastPoint6)).X + values[num2 + 2], 3),
					Y2 = Math.Round(((PointD)(ref lastPoint6)).Y + values[num2 + 3], 3),
					X3 = Math.Round(((PointD)(ref lastPoint6)).X + values[num2 + 4], 3),
					Y3 = Math.Round(((PointD)(ref lastPoint6)).Y + values[num2 + 5], 3)
				};
				geoms.Add(stiBezierMapGeom);
				((PointD)(ref lastPoint6))._002Ector(stiBezierMapGeom.X3, stiBezierMapGeom.Y3);
				num2 += 6;
			}
			break;
		}
		case StiGeomIdent.VerticalLineto_V:
		{
			PointD lastPoint5 = geoms[geoms.Count - 1].GetLastPoint();
			StiLineMapGeom item5 = new StiLineMapGeom
			{
				X = Math.Round(((PointD)(ref lastPoint5)).X, 3),
				Y = Math.Round(values[values.Count - 1], 3)
			};
			geoms.Add(item5);
			break;
		}
		case StiGeomIdent.VerticalLineto_v:
		{
			PointD lastPoint2 = geoms[geoms.Count - 1].GetLastPoint();
			StiLineMapGeom item2 = new StiLineMapGeom
			{
				X = Math.Round(((PointD)(ref lastPoint2)).X, 3),
				Y = Math.Round(((PointD)(ref lastPoint2)).Y + values[values.Count - 1], 3)
			};
			geoms.Add(item2);
			break;
		}
		case StiGeomIdent.HorizontalLineto_H:
		{
			PointD lastPoint3 = geoms[geoms.Count - 1].GetLastPoint();
			StiLineMapGeom item3 = new StiLineMapGeom
			{
				X = Math.Round(values[values.Count - 1], 3),
				Y = Math.Round(((PointD)(ref lastPoint3)).Y, 3)
			};
			geoms.Add(item3);
			break;
		}
		case StiGeomIdent.HorizontalLineto_h:
		{
			PointD lastPoint = geoms[geoms.Count - 1].GetLastPoint();
			StiLineMapGeom item = new StiLineMapGeom
			{
				X = Math.Round(((PointD)(ref lastPoint)).X + values[values.Count - 1], 3),
				Y = Math.Round(((PointD)(ref lastPoint)).Y, 3)
			};
			geoms.Add(item);
			break;
		}
		case StiGeomIdent.Beziers_S:
		{
			if (values.Count < 6)
			{
				if (skipException)
				{
					return;
				}
				throw new NotSupportedException();
			}
			List<double> list2 = new List<double>();
			foreach (double value2 in values)
			{
				list2.Add(Math.Round(value2, 3));
			}
			StiBeziersMapGeom item7 = new StiBeziersMapGeom
			{
				Array = list2.ToArray()
			};
			geoms.Add(item7);
			break;
		}
		case StiGeomIdent.Beziers_s:
		{
			if (values.Count < 6)
			{
				if (skipException)
				{
					return;
				}
				throw new NotSupportedException();
			}
			PointD lastPoint4 = geoms[geoms.Count - 1].GetLastPoint();
			((PointD)(ref lastPoint4)).X = ((PointD)(ref lastPoint4)).X + values[4];
			((PointD)(ref lastPoint4)).Y = ((PointD)(ref lastPoint4)).Y + values[5];
			bool flag = true;
			List<double> list = new List<double>();
			foreach (double value3 in values)
			{
				double value = (flag ? (value3 + ((PointD)(ref lastPoint4)).X) : (value3 + ((PointD)(ref lastPoint4)).Y));
				list.Add(Math.Round(value, 3));
				flag = !flag;
			}
			StiBeziersMapGeom item4 = new StiBeziersMapGeom
			{
				SvgValues = values.ToArray(),
				Array = list.ToArray()
			};
			geoms.Add(item4);
			list.Clear();
			list = null;
			break;
		}
		case StiGeomIdent.Close:
			if (values.Count != 0)
			{
				if (skipException)
				{
					return;
				}
				throw new NotSupportedException();
			}
			geoms.Add(new StiCloseMapGeom());
			break;
		}
		values.Clear();
	}

	public static List<StiMapGeom> ParsePath(string text)
	{
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		try
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
			StiMapGeomCollection geoms = new StiMapGeomCollection();
			int num = 0;
			int num2 = 0;
			StiGeomIdent stiGeomIdent = StiGeomIdent.None;
			int num3 = 0;
			List<double> list = new List<double>();
			double result = 0.0;
			int i = 0;
			for (int length = text.Length; i < length; i++)
			{
				char c = text[i];
				switch (c)
				{
				case 'M':
					if (double.TryParse(text.Substring(num3, i - num3), out result))
					{
						list.Add(result);
					}
					if (stiGeomIdent != StiGeomIdent.None)
					{
						CreateGeom(stiGeomIdent, list, geoms);
						num2 = 0;
					}
					stiGeomIdent = StiGeomIdent.MoveToM;
					num = i + 1;
					num3 = num;
					break;
				case 'm':
					if (double.TryParse(text.Substring(num3, i - num3), out result))
					{
						list.Add(result);
					}
					if (stiGeomIdent != StiGeomIdent.None)
					{
						CreateGeom(stiGeomIdent, list, geoms);
						num2 = 0;
					}
					stiGeomIdent = StiGeomIdent.MoveTom;
					num = i + 1;
					num3 = num;
					break;
				case 'C':
					if (double.TryParse(text.Substring(num3, i - num3), out result))
					{
						list.Add(result);
					}
					if (stiGeomIdent != StiGeomIdent.None)
					{
						CreateGeom(stiGeomIdent, list, geoms);
					}
					stiGeomIdent = StiGeomIdent.Bezier_C;
					num = i + 1;
					num3 = num;
					break;
				case 'c':
					if (double.TryParse(text.Substring(num3, i - num3), out result))
					{
						list.Add(result);
					}
					if (stiGeomIdent != StiGeomIdent.None)
					{
						CreateGeom(stiGeomIdent, list, geoms);
					}
					stiGeomIdent = StiGeomIdent.Bezier_c;
					num = i + 1;
					num3 = num;
					break;
				case 'S':
					if (double.TryParse(text.Substring(num3, i - num3), out result))
					{
						list.Add(result);
					}
					stiGeomIdent = StiGeomIdent.Beziers_S;
					num = i + 1;
					num3 = num;
					break;
				case 's':
					if (double.TryParse(text.Substring(num3, i - num3), out result))
					{
						list.Add(result);
					}
					stiGeomIdent = StiGeomIdent.Beziers_s;
					num = i + 1;
					num3 = num;
					break;
				case 'L':
					if (double.TryParse(text.Substring(num3, i - num3), out result))
					{
						list.Add(result);
					}
					if (stiGeomIdent != StiGeomIdent.None)
					{
						CreateGeom(stiGeomIdent, list, geoms);
					}
					stiGeomIdent = StiGeomIdent.Line_L;
					num = i + 1;
					num3 = num;
					break;
				case 'l':
					if (double.TryParse(text.Substring(num3, i - num3), out result))
					{
						list.Add(result);
					}
					if (stiGeomIdent != StiGeomIdent.None)
					{
						CreateGeom(stiGeomIdent, list, geoms);
					}
					stiGeomIdent = StiGeomIdent.Line_l;
					num = i + 1;
					num3 = num;
					break;
				case 'T':
				case 't':
					if (double.TryParse(text.Substring(num3, i - num3), out result))
					{
						list.Add(result);
					}
					if (stiGeomIdent != StiGeomIdent.None)
					{
						CreateGeom(stiGeomIdent, list, geoms);
					}
					stiGeomIdent = ((c == 't') ? StiGeomIdent.QuadraticCurve_t : StiGeomIdent.QuadraticCurve_T);
					num = i + 1;
					num3 = num;
					break;
				case 'Z':
				case 'z':
					if (double.TryParse(text.Substring(num3, i - num3), out result))
					{
						list.Add(result);
					}
					if (stiGeomIdent != StiGeomIdent.None)
					{
						CreateGeom(stiGeomIdent, list, geoms);
					}
					stiGeomIdent = StiGeomIdent.Close;
					num = i + 1;
					num3 = num;
					break;
				case 'V':
					if (double.TryParse(text.Substring(num3, i - num3), out result))
					{
						list.Add(result);
					}
					if (stiGeomIdent != StiGeomIdent.None)
					{
						CreateGeom(stiGeomIdent, list, geoms);
					}
					stiGeomIdent = StiGeomIdent.VerticalLineto_V;
					num = i + 1;
					num3 = num;
					break;
				case 'v':
					if (double.TryParse(text.Substring(num3, i - num3), out result))
					{
						list.Add(result);
					}
					if (stiGeomIdent != StiGeomIdent.None)
					{
						CreateGeom(stiGeomIdent, list, geoms);
					}
					stiGeomIdent = StiGeomIdent.VerticalLineto_v;
					num = i + 1;
					num3 = num;
					break;
				case 'H':
					if (double.TryParse(text.Substring(num3, i - num3), out result))
					{
						list.Add(result);
					}
					if (stiGeomIdent != StiGeomIdent.None)
					{
						CreateGeom(stiGeomIdent, list, geoms);
					}
					stiGeomIdent = StiGeomIdent.HorizontalLineto_H;
					num = i + 1;
					num3 = num;
					break;
				case 'h':
					if (double.TryParse(text.Substring(num3, i - num3), out result))
					{
						list.Add(result);
					}
					if (stiGeomIdent != StiGeomIdent.None)
					{
						CreateGeom(stiGeomIdent, list, geoms);
					}
					stiGeomIdent = StiGeomIdent.HorizontalLineto_h;
					num = i + 1;
					num3 = num;
					break;
				case 'A':
				case 'a':
					if (double.TryParse(text.Substring(num3, i - num3), out result))
					{
						list.Add(result);
					}
					if (stiGeomIdent != StiGeomIdent.None)
					{
						CreateGeom(stiGeomIdent, list, geoms);
					}
					stiGeomIdent = StiGeomIdent.None;
					ParseEllipticalArc(text, ref i, ref geoms, c == 'a');
					i--;
					num = i + 1;
					num3 = num;
					break;
				case '-':
					num2++;
					if (double.TryParse(text.Substring(num3, i - num3), out result))
					{
						list.Add(result);
					}
					num3 = i;
					break;
				case ' ':
				case ',':
					num2++;
					if (double.TryParse(text.Substring(num3, i - num3), out result))
					{
						list.Add(result);
					}
					num3 = i + 1;
					if (list.Count == 2)
					{
						switch (stiGeomIdent)
						{
						case StiGeomIdent.MoveToM:
						case StiGeomIdent.MoveTom:
							CreateGeom(stiGeomIdent, list, geoms);
							num2 = 0;
							stiGeomIdent = StiGeomIdent.None;
							break;
						case StiGeomIdent.None:
							CreateGeom(StiGeomIdent.Line_l, list, geoms);
							num2 = 0;
							break;
						}
					}
					break;
				case 'Q':
				case 'q':
					throw new Exception("");
				}
			}
			if (double.TryParse(text.Substring(num3, i - num3), out result))
			{
				list.Add(result);
			}
			if (stiGeomIdent != StiGeomIdent.None && list.Count > 1)
			{
				CreateGeom(stiGeomIdent, list, geoms);
			}
			return geoms;
		}
		finally
		{
			Thread.CurrentThread.CurrentCulture = currentCulture;
		}
	}

	private static void ParseEllipticalArc(string text, ref int index, ref StiMapGeomCollection geoms, bool isRelative)
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		index++;
		int num = index;
		int length = text.Length;
		while (index < length)
		{
			char ch = text[index];
			if (IsCloseEllipticalArcSymbol(ch))
			{
				break;
			}
			index++;
		}
		string text2 = text.Substring(num, index - num);
		PointD lastPoint = geoms[geoms.Count - 1].GetLastPoint();
		int index2 = 0;
		StiEllipticalArcMapGeom stiEllipticalArcMapGeom = new StiEllipticalArcMapGeom();
		stiEllipticalArcMapGeom.StartX = ((PointD)(ref lastPoint)).X;
		stiEllipticalArcMapGeom.StartY = ((PointD)(ref lastPoint)).Y;
		stiEllipticalArcMapGeom.RadiusX = ParseDouble(text2, ref index2);
		stiEllipticalArcMapGeom.RadiusY = ParseDouble(text2, ref index2);
		stiEllipticalArcMapGeom.Angle = ParseDouble(text2, ref index2);
		bool flag = ParseBool(text2, ref index2);
		stiEllipticalArcMapGeom.Size = (flag ? StiSvgArcSize.Large : StiSvgArcSize.Small);
		bool flag2 = ParseBool(text2, ref index2);
		stiEllipticalArcMapGeom.Sweep = (flag2 ? StiSvgArcSweep.Positive : StiSvgArcSweep.Negative);
		stiEllipticalArcMapGeom.EndX = ParseDouble(text2, ref index2);
		stiEllipticalArcMapGeom.EndY = ParseDouble(text2, ref index2);
		if (isRelative)
		{
			stiEllipticalArcMapGeom.EndX += ((PointD)(ref lastPoint)).X;
			stiEllipticalArcMapGeom.EndY += ((PointD)(ref lastPoint)).Y;
		}
		geoms.Add(stiEllipticalArcMapGeom);
	}

	private static double ParseDouble(string text, ref int index1)
	{
		int num = index1;
		for (int length = text.Length; index1 < length; index1++)
		{
			switch (text[index1])
			{
			case '-':
				if (index1 - num <= 0)
				{
					continue;
				}
				break;
			default:
				continue;
			case ' ':
			case ',':
				break;
			}
			break;
		}
		int num2 = index1 - num;
		if (num2 == 0)
		{
			throw new Exception("");
		}
		index1++;
		return double.Parse(text.Substring(num, num2).Trim());
	}

	private static bool ParseBool(string text, ref int index1)
	{
		int length = text.Length;
		while (index1 < length)
		{
			char c = text[index1];
			if (char.IsDigit(c))
			{
				index1++;
				return c == '1';
			}
			index1++;
		}
		throw new Exception("");
	}

	private static bool IsCloseEllipticalArcSymbol(char ch)
	{
		switch (ch)
		{
		case 'A':
		case 'C':
		case 'H':
		case 'L':
		case 'M':
		case 'S':
		case 'T':
		case 'V':
		case 'Z':
		case 'a':
		case 'c':
		case 'h':
		case 'l':
		case 'm':
		case 's':
		case 't':
		case 'v':
		case 'z':
			return true;
		case 'Q':
		case 'q':
			throw new Exception("");
		default:
			return false;
		}
	}
}
