namespace TimeAttendanceDAL.Utility.Extension;

public static class String
{
	public static string Fit(this string s, int length, char fitter, char direction)
	{
		if (s == null)
		{
			return "";
		}
		if ((uint)direction <= 82u)
		{
			if (direction == 'C')
			{
				goto IL_008b;
			}
			if (direction == 'L')
			{
				goto IL_0059;
			}
			if (direction == 'R')
			{
				goto IL_003f;
			}
		}
		else
		{
			if (direction == 'c')
			{
				goto IL_008b;
			}
			if (direction == 'l')
			{
				goto IL_0059;
			}
			if (direction == 'r')
			{
				goto IL_003f;
			}
		}
		goto IL_0094;
		IL_008b:
		while (s.Length < length)
		{
			s += fitter;
			if (s.Length < length)
			{
				s = fitter + s;
			}
		}
		goto IL_0094;
		IL_0094:
		return s;
		IL_003f:
		while (s.Length < length)
		{
			s = fitter + s;
		}
		goto IL_0094;
		IL_0059:
		while (s.Length < length)
		{
			s += fitter;
		}
		goto IL_0094;
	}
}
