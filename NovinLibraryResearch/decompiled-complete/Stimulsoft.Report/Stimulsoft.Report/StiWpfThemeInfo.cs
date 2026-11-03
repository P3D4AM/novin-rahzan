using System;

namespace Stimulsoft.Report;

public class StiWpfThemeInfo
{
	public string Name { get; set; }

	public string Assembly { get; set; }

	public string Path { get; set; }

	public string CloudToolTipPath { get; set; }

	public string CheckPath { get; set; }

	public bool IsLoaded
	{
		get
		{
			if (CheckPath == null)
			{
				return true;
			}
			Type type = Type.GetType(CheckPath + "Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a");
			return type != null;
		}
	}

	public override string ToString()
	{
		return Name;
	}

	public StiWpfThemeInfo(string name, string assembly, string path, string cloudToolTipPath, string checkPath)
	{
		Name = name;
		Assembly = assembly;
		Path = path;
		CloudToolTipPath = cloudToolTipPath;
		CheckPath = checkPath;
	}
}
