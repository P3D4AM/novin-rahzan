using System;
using Stimulsoft.Base;
using Stimulsoft.Report.Viewer;

namespace Stimulsoft.Report;

public class StiGuiOptions
{
	public static Type ProgressGdiType { get; set; }

	public static Type ProgressWpfType { get; set; }

	public static bool AllowWinViewer
	{
		get
		{
			try
			{
				Type type = Type.GetType("Stimulsoft.Report.Win.StiProgressInformation, Stimulsoft.Report.Win, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a");
				return type != null;
			}
			catch
			{
				return false;
			}
		}
	}

	public static bool AllowWinDesigner
	{
		get
		{
			try
			{
				Type type = Type.GetType("Stimulsoft.Report.Design.StiDesigner, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a");
				return type != null;
			}
			catch
			{
				return false;
			}
		}
	}

	public static bool AllowWpfViewer
	{
		get
		{
			try
			{
				Type type = Type.GetType("Stimulsoft.Report.Wpf.StiWpfFormRunner, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a");
				return type != null;
			}
			catch
			{
				return false;
			}
		}
	}

	public static bool AllowWpfDesigner
	{
		get
		{
			try
			{
				Type type = Type.GetType("Stimulsoft.Report.WpfDesign.StiWpfDesigner, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a");
				return type != null;
			}
			catch
			{
				return false;
			}
		}
	}

	public static bool AllowWpfV2Designer
	{
		get
		{
			try
			{
				Type type = Type.GetType("Stimulsoft.Client.Designer.Native.StiCloudDesignerLauncherWindow, Stimulsoft.Client.Designer, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a");
				return type != null;
			}
			catch
			{
				return false;
			}
		}
	}

	private static IStiFormRunner GetFormRunner(StiGuiMode guiMode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Invalid comparison between Unknown and I4
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		if ((int)guiMode == 1)
		{
			Type type = Type.GetType("Stimulsoft.Report.Wpf.StiWpfFormRunner, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a");
			if (type == null)
			{
				throw new Exception("Assembly 'Stimulsoft.Report.Wpf' is not found");
			}
			object obj = StiActivator.CreateObject(type);
			return (IStiFormRunner)((obj is IStiFormRunner) ? obj : null);
		}
		return (IStiFormRunner)new StiWinFormRunner();
	}

	public static IStiFormRunner GetExportFormRunner(string formName, StiGuiMode guiMode, object ownerWindow)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Invalid comparison between Unknown and I4
		IStiFormRunner formRunner = GetFormRunner(guiMode);
		formRunner.OwnerWindow = ownerWindow;
		if ((int)guiMode == 1)
		{
			formRunner.Create("Stimulsoft.Report.Export.Wpf." + formName, "Stimulsoft.Report.Wpf");
		}
		else
		{
			formRunner.Create("Stimulsoft.Report.Export.Win." + formName, "Stimulsoft.Report.Win");
		}
		return formRunner;
	}

	public static IStiProgressInformation GetProgressInformation(StiGuiMode guiMode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return GetProgressInformation(null, guiMode);
	}

	public static IStiProgressInformation GetProgressInformation(object ownerForm, StiGuiMode guiMode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Invalid comparison between Unknown and I4
		if ((int)guiMode == 1)
		{
			if (ProgressWpfType != null)
			{
				return StiActivator.CreateObject(ProgressWpfType, new object[1] { ownerForm }) as IStiProgressInformation;
			}
			Type type = Type.GetType("Stimulsoft.Report.Wpf.StiProgressInformation, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a");
			if (type == null)
			{
				throw new Exception("Assembly 'Stimulsoft.Report.Wpf' is not found");
			}
			return StiActivator.CreateObject(type, new object[1] { ownerForm }) as IStiProgressInformation;
		}
		if (ProgressGdiType != null)
		{
			return StiActivator.CreateObject(ProgressGdiType, new object[1] { ownerForm }) as IStiProgressInformation;
		}
		Type type2 = Type.GetType("Stimulsoft.Report.Win.StiProgressInformation, Stimulsoft.Report.Win, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a");
		if (type2 == null)
		{
			throw new Exception("Assembly 'Stimulsoft.Report.Win' is not found");
		}
		return StiActivator.CreateObject(type2, new object[1] { ownerForm }) as IStiProgressInformation;
	}

	public static IStiViewerForm GetViewerForm(StiReport report, StiGuiMode guiMode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Invalid comparison between Unknown and I4
		if ((int)guiMode == 1)
		{
			if (report != null && report.ContainsDashboard)
			{
				Type type = Type.GetType("Stimulsoft.Dashboard.Viewer.Wpf.StiWpfDashboardViewerWindow, Stimulsoft.Dashboard.Viewer.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a");
				if (type == null)
				{
					throw new Exception("Assembly 'Stimulsoft.Dashboard.Viewer.Wpf' is not found");
				}
				return StiActivator.CreateObject(type, new object[1] { report }) as IStiViewerForm;
			}
			Type type2 = Type.GetType("Stimulsoft.Report.Viewer.StiWpfViewerWindow, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a");
			if (type2 == null)
			{
				throw new Exception("Assembly 'Stimulsoft.Report.Wpf' is not found");
			}
			return StiActivator.CreateObject(type2, new object[1] { report }) as IStiViewerForm;
		}
		if (report != null && report.ContainsDashboard)
		{
			Type type3 = Type.GetType("Stimulsoft.Dashboard.Viewer.StiDashboardViewerForm, Stimulsoft.Dashboard.Viewer, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a");
			if (type3 == null)
			{
				throw new Exception("Assembly 'Stimulsoft.Dashboard.Viewer' is not found");
			}
			return StiActivator.CreateObject(type3, new object[1] { report }) as IStiViewerForm;
		}
		Type type4 = Type.GetType("Stimulsoft.Report.Viewer.StiViewerForm, Stimulsoft.Report.Win, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a");
		if (type4 == null)
		{
			throw new Exception("Assembly 'Stimulsoft.Report.Win' is not found");
		}
		return StiActivator.CreateObject(type4, new object[1] { report }) as IStiViewerForm;
	}

	public static IStiViewerForm GetViewerFormWithRibbonGUI(StiReport report, StiGuiMode guiMode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Invalid comparison between Unknown and I4
		if ((int)guiMode == 1)
		{
			Type type = Type.GetType("Stimulsoft.Report.Viewer.StiWpfRibbonViewerWindow, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a");
			if (type == null)
			{
				throw new Exception("Assembly 'Stimulsoft.Report.Wpf' is not found");
			}
			return StiActivator.CreateObject(type, new object[1] { report }) as IStiViewerForm;
		}
		Type type2 = Type.GetType("Stimulsoft.Report.Viewer.StiRibbonViewerForm, Stimulsoft.Report.Win, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a");
		if (type2 == null)
		{
			throw new Exception("Assembly 'Stimulsoft.Report.Win' is not found");
		}
		return StiActivator.CreateObject(type2, new object[1] { report }) as IStiViewerForm;
	}

	public static IStiViewerForm GetDotMatrixViewerForm(StiReport report, StiGuiMode guiMode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Invalid comparison between Unknown and I4
		if ((int)guiMode == 1)
		{
			Type type = Type.GetType("Stimulsoft.Report.Viewer.StiWpfDotMatrixViewerWindow, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a");
			if (type == null)
			{
				throw new Exception("Assembly 'Stimulsoft.Report.Wpf' is not found");
			}
			return StiActivator.CreateObject(type, new object[1] { report }) as IStiViewerForm;
		}
		Type type2 = Type.GetType("Stimulsoft.Report.Viewer.StiDotMatrixViewerForm, Stimulsoft.Report.Win, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a");
		if (type2 == null)
		{
			throw new Exception("Assembly 'Stimulsoft.Report.Win' is not found");
		}
		return StiActivator.CreateObject(type2, new object[1] { report }) as IStiViewerForm;
	}

	public static IStiViewerForm GetDotMatrixViewerFormWithRibbonGUI(StiReport report, StiGuiMode guiMode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Invalid comparison between Unknown and I4
		if ((int)guiMode == 1)
		{
			throw new Exception("Dot-Matrix viewer with Ribbon GUI is not available for WPF!");
		}
		Type type = Type.GetType("Stimulsoft.Report.Viewer.StiRibbonDotMatrixViewerForm, Stimulsoft.Report.Win, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a");
		if (type == null)
		{
			throw new Exception("Assembly 'Stimulsoft.Report.Win' is not found");
		}
		return StiActivator.CreateObject(type, new object[1] { report }) as IStiViewerForm;
	}
}
