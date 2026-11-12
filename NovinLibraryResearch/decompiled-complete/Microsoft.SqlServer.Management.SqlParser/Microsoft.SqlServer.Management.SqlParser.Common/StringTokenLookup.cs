namespace Microsoft.SqlServer.Management.SqlParser.Common;

internal static class StringTokenLookup
{
	public const int MinTokenValue = 0;

	public const int MaxTokenValue = 881;

	public static StringToken Find(string buffer)
	{
		return Find(buffer, 0, buffer.Length);
	}

	public static StringToken Find(string buffer, int startIndex, int length)
	{
		return length switch
		{
			1 => Find_Len1(buffer, startIndex), 
			2 => Find_Len2(buffer, startIndex), 
			3 => Find_Len3(buffer, startIndex), 
			4 => Find_Len4(buffer, startIndex), 
			5 => Find_Len5(buffer, startIndex), 
			6 => Find_Len6(buffer, startIndex), 
			7 => Find_Len7(buffer, startIndex), 
			8 => Find_Len8(buffer, startIndex), 
			9 => Find_Len9(buffer, startIndex), 
			10 => Find_Len10(buffer, startIndex), 
			11 => Find_Len11(buffer, startIndex), 
			12 => Find_Len12(buffer, startIndex), 
			13 => Find_Len13(buffer, startIndex), 
			14 => Find_Len14(buffer, startIndex), 
			15 => Find_Len15(buffer, startIndex), 
			16 => Find_Len16(buffer, startIndex), 
			17 => Find_Len17(buffer, startIndex), 
			18 => Find_Len18(buffer, startIndex), 
			19 => Find_Len19(buffer, startIndex), 
			20 => Find_Len20(buffer, startIndex), 
			21 => Find_Len21(buffer, startIndex), 
			22 => Find_Len22(buffer, startIndex), 
			23 => Find_Len23(buffer, startIndex), 
			24 => Find_Len24(buffer, startIndex), 
			25 => Find_Len25(buffer, startIndex), 
			26 => Find_Len26(buffer, startIndex), 
			27 => Find_Len27(buffer, startIndex), 
			28 => Find_Len28(buffer, startIndex), 
			29 => Find_Len29(buffer, startIndex), 
			30 => Find_Len30(buffer, startIndex), 
			31 => Find_Len31(buffer, startIndex), 
			32 => Find_Len32(buffer, startIndex), 
			33 => Find_Len33(buffer, startIndex), 
			34 => Find_Len34(buffer, startIndex), 
			35 => Find_Len35(buffer, startIndex), 
			37 => Find_Len37(buffer, startIndex), 
			41 => Find_Len41(buffer, startIndex), 
			_ => StringToken.None, 
		};
	}

	private static StringToken Find_Len1(string buffer, int startIndex)
	{
		StringToken result = StringToken.None;
		switch (buffer[startIndex++])
		{
		case 'D':
		case 'd':
			result = StringToken.D;
			break;
		case 'M':
		case 'm':
			result = StringToken.M;
			break;
		case 'N':
		case 'n':
			result = StringToken.N;
			break;
		case 'Q':
		case 'q':
			result = StringToken.Q;
			break;
		case 'S':
		case 's':
			result = StringToken.S;
			break;
		case 'W':
		case 'w':
			result = StringToken.W;
			break;
		case 'Y':
		case 'y':
			result = StringToken.Y;
			break;
		}
		return result;
	}

	private static StringToken Find_Len2(string buffer, int startIndex)
	{
		StringToken result = StringToken.None;
		switch (buffer[startIndex++])
		{
		case 'A':
		case 'a':
			switch (buffer[startIndex++])
			{
			case 'S':
			case 's':
				result = StringToken.As;
				break;
			case 'T':
			case 't':
				result = StringToken.At;
				break;
			}
			break;
		case 'B':
		case 'b':
		{
			char c = buffer[startIndex++];
			if (c == 'Y' || c == 'y')
			{
				result = StringToken.By;
			}
			break;
		}
		case 'D':
		case 'd':
			switch (buffer[startIndex++])
			{
			case 'D':
			case 'd':
				result = StringToken.DD;
				break;
			case 'W':
			case 'w':
				result = StringToken.DW;
				break;
			case 'Y':
			case 'y':
				result = StringToken.DY;
				break;
			}
			break;
		case 'H':
		case 'h':
		{
			char c = buffer[startIndex++];
			if (c == 'H' || c == 'h')
			{
				result = StringToken.HH;
			}
			break;
		}
		case 'I':
		case 'i':
			switch (buffer[startIndex++])
			{
			case 'F':
			case 'f':
				result = StringToken.If;
				break;
			case 'N':
			case 'n':
				result = StringToken.In;
				break;
			case 'S':
			case 's':
				result = StringToken.Is;
				break;
			}
			break;
		case 'M':
		case 'm':
			switch (buffer[startIndex++])
			{
			case 'I':
			case 'i':
				result = StringToken.MI;
				break;
			case 'M':
			case 'm':
				result = StringToken.MM;
				break;
			case 'S':
			case 's':
				result = StringToken.MS;
				break;
			}
			break;
		case 'N':
		case 'n':
			switch (buffer[startIndex++])
			{
			case 'O':
			case 'o':
				result = StringToken.No;
				break;
			case 'S':
			case 's':
				result = StringToken.NS;
				break;
			}
			break;
		case 'O':
		case 'o':
			switch (buffer[startIndex++])
			{
			case 'F':
			case 'f':
				result = StringToken.Of;
				break;
			case 'N':
			case 'n':
				result = StringToken.On;
				break;
			case 'R':
			case 'r':
				result = StringToken.Or;
				break;
			}
			break;
		case 'Q':
		case 'q':
		{
			char c = buffer[startIndex++];
			if (c == 'Q' || c == 'q')
			{
				result = StringToken.QQ;
			}
			break;
		}
		case 'S':
		case 's':
		{
			char c = buffer[startIndex++];
			if (c == 'S' || c == 's')
			{
				result = StringToken.SS;
			}
			break;
		}
		case 'T':
		case 't':
			switch (buffer[startIndex++])
			{
			case 'O':
			case 'o':
				result = StringToken.To;
				break;
			case 'Z':
			case 'z':
				result = StringToken.TZ;
				break;
			}
			break;
		case 'W':
		case 'w':
			switch (buffer[startIndex++])
			{
			case 'K':
			case 'k':
				result = StringToken.WK;
				break;
			case 'W':
			case 'w':
				result = StringToken.WW;
				break;
			}
			break;
		case 'Y':
		case 'y':
		{
			char c = buffer[startIndex++];
			if (c == 'Y' || c == 'y')
			{
				result = StringToken.YY;
			}
			break;
		}
		}
		return result;
	}

	private static StringToken Find_Len3(string buffer, int startIndex)
	{
		StringToken result = StringToken.None;
		switch (buffer[startIndex++])
		{
		case 'A':
		case 'a':
			switch (buffer[startIndex++])
			{
			case 'D':
			case 'd':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'D' || c2 == 'd')
				{
					result = StringToken.Add;
				}
				break;
			}
			case 'L':
			case 'l':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'L' || c2 == 'l')
				{
					result = StringToken.All;
				}
				break;
			}
			case 'N':
			case 'n':
				switch (buffer[startIndex++])
				{
				case 'D':
				case 'd':
					result = StringToken.And;
					break;
				case 'Y':
				case 'y':
					result = StringToken.Any;
					break;
				}
				break;
			case 'S':
			case 's':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'C' || c2 == 'c')
				{
					result = StringToken.Asc;
				}
				break;
			}
			}
			break;
		case 'B':
		case 'b':
		{
			char c = buffer[startIndex++];
			if (c == 'I' || c == 'i')
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'T' || c2 == 't')
				{
					result = StringToken.Bit;
				}
			}
			break;
		}
		case 'C':
		case 'c':
		{
			char c = buffer[startIndex++];
			if (c == 'P' || c == 'p')
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'U' || c2 == 'u')
				{
					result = StringToken.Cpu;
				}
			}
			break;
		}
		case 'D':
		case 'd':
			switch (buffer[startIndex++])
			{
			case 'A':
			case 'a':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'Y' || c2 == 'y')
				{
					result = StringToken.Day;
				}
				break;
			}
			case 'E':
			case 'e':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'C' || c2 == 'c')
				{
					result = StringToken.Dec;
				}
				break;
			}
			}
			break;
		case 'E':
		case 'e':
		{
			char c = buffer[startIndex++];
			if (c == 'N' || c == 'n')
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'D' || c2 == 'd')
				{
					result = StringToken.End;
				}
			}
			break;
		}
		case 'F':
		case 'f':
		{
			char c = buffer[startIndex++];
			if (c == 'O' || c == 'o')
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'R' || c2 == 'r')
				{
					result = StringToken.For;
				}
			}
			break;
		}
		case 'G':
		case 'g':
		{
			char c = buffer[startIndex++];
			if (c == 'E' || c == 'e')
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'T' || c2 == 't')
				{
					result = StringToken.Get;
				}
			}
			break;
		}
		case 'I':
		case 'i':
		{
			char c = buffer[startIndex++];
			if (c == 'N' || c == 'n')
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'T' || c2 == 't')
				{
					result = StringToken.Int;
				}
			}
			break;
		}
		case 'K':
		case 'k':
		{
			char c = buffer[startIndex++];
			if (c == 'E' || c == 'e')
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'Y' || c2 == 'y')
				{
					result = StringToken.Key;
				}
			}
			break;
		}
		case 'L':
		case 'l':
		{
			char c = buffer[startIndex++];
			if (c == 'O' || c == 'o')
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'G' || c2 == 'g')
				{
					result = StringToken.Log;
				}
			}
			break;
		}
		case 'M':
		case 'm':
		{
			char c = buffer[startIndex++];
			if (c == 'C' || c == 'c')
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'S' || c2 == 's')
				{
					result = StringToken.MCS;
				}
			}
			break;
		}
		case 'N':
		case 'n':
		{
			char c = buffer[startIndex++];
			if (c == 'O' || c == 'o')
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'T' || c2 == 't')
				{
					result = StringToken.Not;
				}
			}
			break;
		}
		case 'O':
		case 'o':
			switch (buffer[startIndex++])
			{
			case 'F':
			case 'f':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'F' || c2 == 'f')
				{
					result = StringToken.Off;
				}
				break;
			}
			case 'R':
			case 'r':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'C' || c2 == 'c')
				{
					result = StringToken.Orc;
				}
				break;
			}
			case 'U':
			case 'u':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'T' || c2 == 't')
				{
					result = StringToken.Out;
				}
				break;
			}
			}
			break;
		case 'R':
		case 'r':
			switch (buffer[startIndex++])
			{
			case 'A':
			case 'a':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'W' || c2 == 'w')
				{
					result = StringToken.Raw;
				}
				break;
			}
			case 'O':
			case 'o':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'W' || c2 == 'w')
				{
					result = StringToken.Row;
				}
				break;
			}
			}
			break;
		case 'S':
		case 's':
			switch (buffer[startIndex++])
			{
			case 'E':
			case 'e':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'T' || c2 == 't')
				{
					result = StringToken.Set;
				}
				break;
			}
			case 'I':
			case 'i':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'D' || c2 == 'd')
				{
					result = StringToken.Sid;
				}
				break;
			}
			case 'Q':
			case 'q':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'L' || c2 == 'l')
				{
					result = StringToken.Sql;
				}
				break;
			}
			}
			break;
		case 'T':
		case 't':
			switch (buffer[startIndex++])
			{
			case 'O':
			case 'o':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'P' || c2 == 'p')
				{
					result = StringToken.Top;
				}
				break;
			}
			case 'R':
			case 'r':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'Y' || c2 == 'y')
				{
					result = StringToken.Try;
				}
				break;
			}
			}
			break;
		case 'U':
		case 'u':
			switch (buffer[startIndex++])
			{
			case 'R':
			case 'r':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'L' || c2 == 'l')
				{
					result = StringToken.Url;
				}
				break;
			}
			case 'S':
			case 's':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'E' || c2 == 'e')
				{
					result = StringToken.Use;
				}
				break;
			}
			}
			break;
		case 'X':
		case 'x':
		{
			char c = buffer[startIndex++];
			if (c == 'M' || c == 'm')
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'L' || c2 == 'l')
				{
					result = StringToken.Xml;
				}
			}
			break;
		}
		}
		return result;
	}

	private static StringToken Find_Len4(string buffer, int startIndex)
	{
		StringToken result = StringToken.None;
		switch (buffer[startIndex++])
		{
		case 'A':
		case 'a':
		{
			char c = buffer[startIndex++];
			if (c != 'U' && c != 'u')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 == 'T' || c2 == 't')
			{
				char c3 = buffer[startIndex++];
				if (c3 == 'O' || c3 == 'o')
				{
					result = StringToken.Auto;
				}
			}
			break;
		}
		case 'B':
		case 'b':
		{
			char c = buffer[startIndex++];
			if (c != 'U' && c != 'u')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 == 'L' || c2 == 'l')
			{
				char c3 = buffer[startIndex++];
				if (c3 == 'K' || c3 == 'k')
				{
					result = StringToken.Bulk;
				}
			}
			break;
		}
		case 'C':
		case 'c':
			switch (buffer[startIndex++])
			{
			case 'A':
			case 'a':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'S' || c2 == 's')
				{
					switch (buffer[startIndex++])
					{
					case 'E':
					case 'e':
						result = StringToken.Case;
						break;
					case 'T':
					case 't':
						result = StringToken.Cast;
						break;
					}
				}
				break;
			}
			case 'H':
			case 'h':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'A' || c2 == 'a')
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'R' || c3 == 'r')
					{
						result = StringToken.Char;
					}
				}
				break;
			}
			case 'O':
			case 'o':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'P' || c2 == 'p')
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'Y' || c3 == 'y')
					{
						result = StringToken.Copy;
					}
				}
				break;
			}
			case 'U':
			case 'u':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'B' || c2 == 'b')
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'E' || c3 == 'e')
					{
						result = StringToken.Cube;
					}
				}
				break;
			}
			}
			break;
		case 'D':
		case 'd':
			switch (buffer[startIndex++])
			{
			case 'A':
			case 'a':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'T' || c2 == 't')
				{
					switch (buffer[startIndex++])
					{
					case 'A':
					case 'a':
						result = StringToken.Data;
						break;
					case 'E':
					case 'e':
						result = StringToken.Date;
						break;
					}
				}
				break;
			}
			case 'B':
			case 'b':
				switch (buffer[startIndex++])
				{
				case 'C':
				case 'c':
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'C' || c3 == 'c')
					{
						result = StringToken.Dbcc;
					}
					break;
				}
				case 'T':
				case 't':
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'S' || c3 == 's')
					{
						result = StringToken.Dbts;
					}
					break;
				}
				}
				break;
			case 'E':
			case 'e':
				switch (buffer[startIndex++])
				{
				case 'N':
				case 'n':
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'Y' || c3 == 'y')
					{
						result = StringToken.Deny;
					}
					break;
				}
				case 'S':
				case 's':
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'C' || c3 == 'c')
					{
						result = StringToken.Desc;
					}
					break;
				}
				}
				break;
			case 'R':
			case 'r':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'O' || c2 == 'o')
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'P' || c3 == 'p')
					{
						result = StringToken.Drop;
					}
				}
				break;
			}
			case 'U':
			case 'u':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'M' || c2 == 'm')
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'P' || c3 == 'p')
					{
						result = StringToken.Dump;
					}
				}
				break;
			}
			}
			break;
		case 'E':
		case 'e':
			switch (buffer[startIndex++])
			{
			case 'D':
			case 'd':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'G' || c2 == 'g')
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'E' || c3 == 'e')
					{
						result = StringToken.Edge;
					}
				}
				break;
			}
			case 'L':
			case 'l':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'S' || c2 == 's')
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'E' || c3 == 'e')
					{
						result = StringToken.Else;
					}
				}
				break;
			}
			case 'X':
			case 'x':
				switch (buffer[startIndex++])
				{
				case 'E':
				case 'e':
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'C' || c3 == 'c')
					{
						result = StringToken.Exec;
					}
					break;
				}
				case 'I':
				case 'i':
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'T' || c3 == 't')
					{
						result = StringToken.Exit;
					}
					break;
				}
				}
				break;
			}
			break;
		case 'F':
		case 'f':
			switch (buffer[startIndex++])
			{
			case 'I':
			case 'i':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'L' || c2 == 'l')
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'E' || c3 == 'e')
					{
						result = StringToken.File;
					}
				}
				break;
			}
			case 'R':
			case 'r':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'O' || c2 == 'o')
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'M' || c3 == 'm')
					{
						result = StringToken.From;
					}
				}
				break;
			}
			case 'U':
			case 'u':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'L' || c2 == 'l')
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'L' || c3 == 'l')
					{
						result = StringToken.Full;
					}
				}
				break;
			}
			}
			break;
		case 'G':
		case 'g':
		{
			char c = buffer[startIndex++];
			if (c != 'O' && c != 'o')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 == 'T' || c2 == 't')
			{
				char c3 = buffer[startIndex++];
				if (c3 == 'O' || c3 == 'o')
				{
					result = StringToken.Goto;
				}
			}
			break;
		}
		case 'H':
		case 'h':
			switch (buffer[startIndex++])
			{
			case 'A':
			case 'a':
				switch (buffer[startIndex++])
				{
				case 'D':
				case 'd':
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'R' || c3 == 'r')
					{
						result = StringToken.Hadr;
					}
					break;
				}
				case 'S':
				case 's':
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'H' || c3 == 'h')
					{
						result = StringToken.Hash;
					}
					break;
				}
				}
				break;
			case 'E':
			case 'e':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'A' || c2 == 'a')
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'P' || c3 == 'p')
					{
						result = StringToken.Heap;
					}
				}
				break;
			}
			case 'O':
			case 'o':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'U' || c2 == 'u')
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'R' || c3 == 'r')
					{
						result = StringToken.Hour;
					}
				}
				break;
			}
			}
			break;
		case 'I':
		case 'i':
			switch (buffer[startIndex++])
			{
			case 'D':
			case 'd':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'L' || c2 == 'l')
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'E' || c3 == 'e')
					{
						result = StringToken.Idle;
					}
				}
				break;
			}
			case 'N':
			case 'n':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'T' || c2 == 't')
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'O' || c3 == 'o')
					{
						result = StringToken.Into;
					}
				}
				break;
			}
			}
			break;
		case 'J':
		case 'j':
			switch (buffer[startIndex++])
			{
			case 'O':
			case 'o':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'I' || c2 == 'i')
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'N' || c3 == 'n')
					{
						result = StringToken.Join;
					}
				}
				break;
			}
			case 'S':
			case 's':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'O' || c2 == 'o')
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'N' || c3 == 'n')
					{
						result = StringToken.Json;
					}
				}
				break;
			}
			}
			break;
		case 'K':
		case 'k':
		{
			char c = buffer[startIndex++];
			if (c != 'I' && c != 'i')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 == 'L' || c2 == 'l')
			{
				char c3 = buffer[startIndex++];
				if (c3 == 'L' || c3 == 'l')
				{
					result = StringToken.Kill;
				}
			}
			break;
		}
		case 'L':
		case 'l':
			switch (buffer[startIndex++])
			{
			case 'E':
			case 'e':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'F' || c2 == 'f')
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'T' || c3 == 't')
					{
						result = StringToken.Left;
					}
				}
				break;
			}
			case 'I':
			case 'i':
				switch (buffer[startIndex++])
				{
				case 'K':
				case 'k':
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'E' || c3 == 'e')
					{
						result = StringToken.Like;
					}
					break;
				}
				case 'S':
				case 's':
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'T' || c3 == 't')
					{
						result = StringToken.List;
					}
					break;
				}
				}
				break;
			case 'O':
			case 'o':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'A' || c2 == 'a')
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'D' || c3 == 'd')
					{
						result = StringToken.Load;
					}
				}
				break;
			}
			}
			break;
		case 'M':
		case 'm':
		{
			char c = buffer[startIndex++];
			if (c != 'O' && c != 'o')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 == 'V' || c2 == 'v')
			{
				char c3 = buffer[startIndex++];
				if (c3 == 'E' || c3 == 'e')
				{
					result = StringToken.Move;
				}
			}
			break;
		}
		case 'N':
		case 'n':
			switch (buffer[startIndex++])
			{
			case 'A':
			case 'a':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'M' || c2 == 'm')
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'E' || c3 == 'e')
					{
						result = StringToken.Name;
					}
				}
				break;
			}
			case 'E':
			case 'e':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'X' || c2 == 'x')
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'T' || c3 == 't')
					{
						result = StringToken.Next;
					}
				}
				break;
			}
			case 'O':
			case 'o':
				switch (buffer[startIndex++])
				{
				case 'D':
				case 'd':
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'E' || c3 == 'e')
					{
						result = StringToken.Node;
					}
					break;
				}
				case 'N':
				case 'n':
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'E' || c3 == 'e')
					{
						result = StringToken.NONE;
					}
					break;
				}
				}
				break;
			case 'U':
			case 'u':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'L' || c2 == 'l')
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'L' || c3 == 'l')
					{
						result = StringToken.Null;
					}
				}
				break;
			}
			}
			break;
		case 'O':
		case 'o':
			switch (buffer[startIndex++])
			{
			case 'N':
			case 'n':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'L' || c2 == 'l')
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'Y' || c3 == 'y')
					{
						result = StringToken.Only;
					}
				}
				break;
			}
			case 'P':
			case 'p':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'E' || c2 == 'e')
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'N' || c3 == 'n')
					{
						result = StringToken.Open;
					}
				}
				break;
			}
			case 'V':
			case 'v':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'E' || c2 == 'e')
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'R' || c3 == 'r')
					{
						result = StringToken.Over;
					}
				}
				break;
			}
			}
			break;
		case 'P':
		case 'p':
			switch (buffer[startIndex++])
			{
			case 'A':
			case 'a':
				switch (buffer[startIndex++])
				{
				case 'G':
				case 'g':
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'E' || c3 == 'e')
					{
						result = StringToken.Page;
					}
					break;
				}
				case 'T':
				case 't':
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'H' || c3 == 'h')
					{
						result = StringToken.Path;
					}
					break;
				}
				}
				break;
			case 'L':
			case 'l':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'A' || c2 == 'a')
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'N' || c3 == 'n')
					{
						result = StringToken.Plan;
					}
				}
				break;
			}
			case 'O':
			case 'o':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'O' || c2 == 'o')
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'L' || c3 == 'l')
					{
						result = StringToken.Pool;
					}
				}
				break;
			}
			case 'R':
			case 'r':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'O' || c2 == 'o')
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'C' || c3 == 'c')
					{
						result = StringToken.Proc;
					}
				}
				break;
			}
			}
			break;
		case 'R':
		case 'r':
			switch (buffer[startIndex++])
			{
			case 'E':
			case 'e':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'A' || c2 == 'a')
				{
					switch (buffer[startIndex++])
					{
					case 'D':
					case 'd':
						result = StringToken.Read;
						break;
					case 'L':
					case 'l':
						result = StringToken.Real;
						break;
					}
				}
				break;
			}
			case 'O':
			case 'o':
				switch (buffer[startIndex++])
				{
				case 'L':
				case 'l':
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'E' || c3 == 'e')
					{
						result = StringToken.Role;
					}
					break;
				}
				case 'W':
				case 'w':
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'S' || c3 == 's')
					{
						result = StringToken.Rows;
					}
					break;
				}
				}
				break;
			case 'U':
			case 'u':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'L' || c2 == 'l')
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'E' || c3 == 'e')
					{
						result = StringToken.Rule;
					}
				}
				break;
			}
			}
			break;
		case 'S':
		case 's':
			switch (buffer[startIndex++])
			{
			case 'A':
			case 'a':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'V' || c2 == 'v')
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'E' || c3 == 'e')
					{
						result = StringToken.Save;
					}
				}
				break;
			}
			case 'E':
			case 'e':
				switch (buffer[startIndex++])
				{
				case 'L':
				case 'l':
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'F' || c3 == 'f')
					{
						result = StringToken.Self;
					}
					break;
				}
				case 'N':
				case 'n':
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'D' || c3 == 'd')
					{
						result = StringToken.Send;
					}
					break;
				}
				case 'T':
				case 't':
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'S' || c3 == 's')
					{
						result = StringToken.Sets;
					}
					break;
				}
				}
				break;
			case 'O':
			case 'o':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'M' || c2 == 'm')
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'E' || c3 == 'e')
					{
						result = StringToken.Some;
					}
				}
				break;
			}
			case 'P':
			case 'p':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'I' || c2 == 'i')
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'D' || c3 == 'd')
					{
						result = StringToken.Spid;
					}
				}
				break;
			}
			}
			break;
		case 'T':
		case 't':
			switch (buffer[startIndex++])
			{
			case 'E':
			case 'e':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'X' || c2 == 'x')
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'T' || c3 == 't')
					{
						result = StringToken.Text;
					}
				}
				break;
			}
			case 'H':
			case 'h':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'E' || c2 == 'e')
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'N' || c3 == 'n')
					{
						result = StringToken.Then;
					}
				}
				break;
			}
			case 'I':
			case 'i':
				switch (buffer[startIndex++])
				{
				case 'E':
				case 'e':
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'S' || c3 == 's')
					{
						result = StringToken.Ties;
					}
					break;
				}
				case 'M':
				case 'm':
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'E' || c3 == 'e')
					{
						result = StringToken.Time;
					}
					break;
				}
				}
				break;
			case 'R':
			case 'r':
				switch (buffer[startIndex++])
				{
				case 'A':
				case 'a':
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'N' || c3 == 'n')
					{
						result = StringToken.Tran;
					}
					break;
				}
				case 'U':
				case 'u':
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'E' || c3 == 'e')
					{
						result = StringToken.True;
					}
					break;
				}
				}
				break;
			case 'Y':
			case 'y':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'P' || c2 == 'p')
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'E' || c3 == 'e')
					{
						result = StringToken.Type;
					}
				}
				break;
			}
			}
			break;
		case 'U':
		case 'u':
		{
			char c = buffer[startIndex++];
			if (c != 'S' && c != 's')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 == 'E' || c2 == 'e')
			{
				char c3 = buffer[startIndex++];
				if (c3 == 'R' || c3 == 'r')
				{
					result = StringToken.User;
				}
			}
			break;
		}
		case 'V':
		case 'v':
		{
			char c = buffer[startIndex++];
			if (c != 'I' && c != 'i')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 == 'E' || c2 == 'e')
			{
				char c3 = buffer[startIndex++];
				if (c3 == 'W' || c3 == 'w')
				{
					result = StringToken.View;
				}
			}
			break;
		}
		case 'W':
		case 'w':
			switch (buffer[startIndex++])
			{
			case 'E':
			case 'e':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'E' || c2 == 'e')
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'K' || c3 == 'k')
					{
						result = StringToken.Week;
					}
				}
				break;
			}
			case 'H':
			case 'h':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'E' || c2 == 'e')
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'N' || c3 == 'n')
					{
						result = StringToken.When;
					}
				}
				break;
			}
			case 'I':
			case 'i':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'T' || c2 == 't')
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'H' || c3 == 'h')
					{
						result = StringToken.With;
					}
				}
				break;
			}
			}
			break;
		case 'Y':
		case 'y':
			switch (buffer[startIndex++])
			{
			case 'E':
			case 'e':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'A' || c2 == 'a')
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'R' || c3 == 'r')
					{
						result = StringToken.Year;
					}
				}
				break;
			}
			case 'Y':
			case 'y':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'Y' || c2 == 'y')
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'Y' || c3 == 'y')
					{
						result = StringToken.YYYY;
					}
				}
				break;
			}
			}
			break;
		case 'Z':
		case 'z':
		{
			char c = buffer[startIndex++];
			if (c != 'O' && c != 'o')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 == 'N' || c2 == 'n')
			{
				char c3 = buffer[startIndex++];
				if (c3 == 'E' || c3 == 'e')
				{
					result = StringToken.Zone;
				}
			}
			break;
		}
		}
		return result;
	}

	private static StringToken Find_Len5(string buffer, int startIndex)
	{
		StringToken result = StringToken.None;
		switch (buffer[startIndex++])
		{
		case 'A':
		case 'a':
			switch (buffer[startIndex++])
			{
			case 'B':
			case 'b':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'O' && c2 != 'o')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'R' || c3 == 'r')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'T' || c4 == 't')
					{
						result = StringToken.Abort;
					}
				}
				break;
			}
			case 'F':
			case 'f':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'T' && c2 != 't')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'E' || c3 == 'e')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'R' || c4 == 'r')
					{
						result = StringToken.After;
					}
				}
				break;
			}
			case 'L':
			case 'l':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'T' && c2 != 't')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'E' || c3 == 'e')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'R' || c4 == 'r')
					{
						result = StringToken.Alter;
					}
				}
				break;
			}
			case 'P':
			case 'p':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'P' && c2 != 'p')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'L' || c3 == 'l')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'Y' || c4 == 'y')
					{
						result = StringToken.Apply;
					}
				}
				break;
			}
			case 'S':
			case 's':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'Y' && c2 != 'y')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'N' || c3 == 'n')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'C' || c4 == 'c')
					{
						result = StringToken.Async;
					}
				}
				break;
			}
			case 'U':
			case 'u':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'D' && c2 != 'd')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'I' || c3 == 'i')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'T' || c4 == 't')
					{
						result = StringToken.Audit;
					}
				}
				break;
			}
			}
			break;
		case 'B':
		case 'b':
			switch (buffer[startIndex++])
			{
			case 'E':
			case 'e':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'G' && c2 != 'g')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'I' || c3 == 'i')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'N' || c4 == 'n')
					{
						result = StringToken.Begin;
					}
				}
				break;
			}
			case 'L':
			case 'l':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'O' && c2 != 'o')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'C' || c3 == 'c')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'K' || c4 == 'k')
					{
						result = StringToken.Block;
					}
				}
				break;
			}
			case 'R':
			case 'r':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'E' && c2 != 'e')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'A' || c3 == 'a')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'K' || c4 == 'k')
					{
						result = StringToken.Break;
					}
				}
				break;
			}
			case 'U':
			case 'u':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'I' && c2 != 'i')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'L' || c3 == 'l')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'D' || c4 == 'd')
					{
						result = StringToken.Build;
					}
				}
				break;
			}
			}
			break;
		case 'C':
		case 'c':
			switch (buffer[startIndex++])
			{
			case 'A':
			case 'a':
				switch (buffer[startIndex++])
				{
				case 'C':
				case 'c':
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'H' || c3 == 'h')
					{
						char c4 = buffer[startIndex++];
						if (c4 == 'E' || c4 == 'e')
						{
							result = StringToken.Cache;
						}
					}
					break;
				}
				case 'T':
				case 't':
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'C' || c3 == 'c')
					{
						char c4 = buffer[startIndex++];
						if (c4 == 'H' || c4 == 'h')
						{
							result = StringToken.Catch;
						}
					}
					break;
				}
				}
				break;
			case 'H':
			case 'h':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'E' && c2 != 'e')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'C' || c3 == 'c')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'K' || c4 == 'k')
					{
						result = StringToken.Check;
					}
				}
				break;
			}
			case 'L':
			case 'l':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'O' && c2 != 'o')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'S' || c3 == 's')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'E' || c4 == 'e')
					{
						result = StringToken.Close;
					}
				}
				break;
			}
			case 'R':
			case 'r':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'O' && c2 != 'o')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'S' || c3 == 's')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'S' || c4 == 's')
					{
						result = StringToken.Cross;
					}
				}
				break;
			}
			case 'Y':
			case 'y':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'C' && c2 != 'c')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'L' || c3 == 'l')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'E' || c4 == 'e')
					{
						result = StringToken.Cycle;
					}
				}
				break;
			}
			}
			break;
		case 'E':
		case 'e':
			switch (buffer[startIndex++])
			{
			case 'R':
			case 'r':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'R' && c2 != 'r')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'O' || c3 == 'o')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'R' || c4 == 'r')
					{
						result = StringToken.Error;
					}
				}
				break;
			}
			case 'V':
			case 'v':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'E' && c2 != 'e')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'N' || c3 == 'n')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'T' || c4 == 't')
					{
						result = StringToken.Event;
					}
				}
				break;
			}
			}
			break;
		case 'F':
		case 'f':
			switch (buffer[startIndex++])
			{
			case 'A':
			case 'a':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'L' && c2 != 'l')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'S' || c3 == 's')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'E' || c4 == 'e')
					{
						result = StringToken.False;
					}
				}
				break;
			}
			case 'E':
			case 'e':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'T' && c2 != 't')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'C' || c3 == 'c')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'H' || c4 == 'h')
					{
						result = StringToken.Fetch;
					}
				}
				break;
			}
			case 'I':
			case 'i':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'R' && c2 != 'r')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'S' || c3 == 's')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'T' || c4 == 't')
					{
						result = StringToken.First;
					}
				}
				break;
			}
			case 'L':
			case 'l':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'O' && c2 != 'o')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'A' || c3 == 'a')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'T' || c4 == 't')
					{
						result = StringToken.Float;
					}
				}
				break;
			}
			}
			break;
		case 'G':
		case 'g':
		{
			char c = buffer[startIndex++];
			if (c != 'R' && c != 'r')
			{
				break;
			}
			switch (buffer[startIndex++])
			{
			case 'A':
			case 'a':
			{
				char c3 = buffer[startIndex++];
				if (c3 == 'N' || c3 == 'n')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'T' || c4 == 't')
					{
						result = StringToken.Grant;
					}
				}
				break;
			}
			case 'O':
			case 'o':
			{
				char c3 = buffer[startIndex++];
				if (c3 == 'U' || c3 == 'u')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'P' || c4 == 'p')
					{
						result = StringToken.Group;
					}
				}
				break;
			}
			}
			break;
		}
		case 'I':
		case 'i':
			switch (buffer[startIndex++])
			{
			case 'M':
			case 'm':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'A' && c2 != 'a')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'G' || c3 == 'g')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'E' || c4 == 'e')
					{
						result = StringToken.Image;
					}
				}
				break;
			}
			case 'N':
			case 'n':
				switch (buffer[startIndex++])
				{
				case 'D':
				case 'd':
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'E' || c3 == 'e')
					{
						char c4 = buffer[startIndex++];
						if (c4 == 'X' || c4 == 'x')
						{
							result = StringToken.Index;
						}
					}
					break;
				}
				case 'N':
				case 'n':
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'E' || c3 == 'e')
					{
						char c4 = buffer[startIndex++];
						if (c4 == 'R' || c4 == 'r')
						{
							result = StringToken.Inner;
						}
					}
					break;
				}
				case 'P':
				case 'p':
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'U' || c3 == 'u')
					{
						char c4 = buffer[startIndex++];
						if (c4 == 'T' || c4 == 't')
						{
							result = StringToken.Input;
						}
					}
					break;
				}
				}
				break;
			case 'S':
			case 's':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'O' && c2 != 'o')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'W' || c3 == 'w')
				{
					switch (buffer[startIndex++])
					{
					case 'K':
					case 'k':
						result = StringToken.IsoWk;
						break;
					case 'W':
					case 'w':
						result = StringToken.IsoWw;
						break;
					}
				}
				break;
			}
			}
			break;
		case 'L':
		case 'l':
		{
			char c = buffer[startIndex++];
			if (c != 'O' && c != 'o')
			{
				break;
			}
			switch (buffer[startIndex++])
			{
			case 'C':
			case 'c':
			{
				char c3 = buffer[startIndex++];
				if (c3 == 'A' || c3 == 'a')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'L' || c4 == 'l')
					{
						result = StringToken.Local;
					}
				}
				break;
			}
			case 'G':
			case 'g':
				switch (buffer[startIndex++])
				{
				case 'I':
				case 'i':
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'N' || c4 == 'n')
					{
						result = StringToken.Login;
					}
					break;
				}
				case 'O':
				case 'o':
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'N' || c4 == 'n')
					{
						result = StringToken.Logon;
					}
					break;
				}
				}
				break;
			}
			break;
		}
		case 'M':
		case 'm':
			switch (buffer[startIndex++])
			{
			case 'A':
			case 'a':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'T' && c2 != 't')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'C' || c3 == 'c')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'H' || c4 == 'h')
					{
						result = StringToken.Match;
					}
				}
				break;
			}
			case 'E':
			case 'e':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'R' && c2 != 'r')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'G' || c3 == 'g')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'E' || c4 == 'e')
					{
						result = StringToken.Merge;
					}
				}
				break;
			}
			case 'O':
			case 'o':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'N' && c2 != 'n')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case 'E':
				case 'e':
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'Y' || c4 == 'y')
					{
						result = StringToken.Money;
					}
					break;
				}
				case 'T':
				case 't':
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'H' || c4 == 'h')
					{
						result = StringToken.Month;
					}
					break;
				}
				}
				break;
			}
			}
			break;
		case 'N':
		case 'n':
			switch (buffer[startIndex++])
			{
			case 'C':
			case 'c':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'H' && c2 != 'h')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'A' || c3 == 'a')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'R' || c4 == 'r')
					{
						result = StringToken.NChar;
					}
				}
				break;
			}
			case 'T':
			case 't':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'E' && c2 != 'e')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'X' || c3 == 'x')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'T' || c4 == 't')
					{
						result = StringToken.NText;
					}
				}
				break;
			}
			}
			break;
		case 'O':
		case 'o':
			switch (buffer[startIndex++])
			{
			case 'R':
			case 'r':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'D' && c2 != 'd')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'E' || c3 == 'e')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'R' || c4 == 'r')
					{
						result = StringToken.Order;
					}
				}
				break;
			}
			case 'U':
			case 'u':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'T' && c2 != 't')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'E' || c3 == 'e')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'R' || c4 == 'r')
					{
						result = StringToken.Outer;
					}
				}
				break;
			}
			case 'W':
			case 'w':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'N' && c2 != 'n')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'E' || c3 == 'e')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'R' || c4 == 'r')
					{
						result = StringToken.Owner;
					}
				}
				break;
			}
			}
			break;
		case 'P':
		case 'p':
			switch (buffer[startIndex++])
			{
			case 'A':
			case 'a':
				switch (buffer[startIndex++])
				{
				case 'R':
				case 'r':
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'S' || c3 == 's')
					{
						char c4 = buffer[startIndex++];
						if (c4 == 'E' || c4 == 'e')
						{
							result = StringToken.Parse;
						}
					}
					break;
				}
				case 'U':
				case 'u':
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'S' || c3 == 's')
					{
						char c4 = buffer[startIndex++];
						if (c4 == 'E' || c4 == 'e')
						{
							result = StringToken.Pause;
						}
					}
					break;
				}
				}
				break;
			case 'I':
			case 'i':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'V' && c2 != 'v')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'O' || c3 == 'o')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'T' || c4 == 't')
					{
						result = StringToken.Pivot;
					}
				}
				break;
			}
			case 'R':
			case 'r':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'I' && c2 != 'i')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'N' || c3 == 'n')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'T' || c4 == 't')
					{
						result = StringToken.Print;
					}
				}
				break;
			}
			}
			break;
		case 'Q':
		case 'q':
		{
			char c = buffer[startIndex++];
			if (c != 'U' && c != 'u')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'E' && c2 != 'e')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 == 'U' || c3 == 'u')
			{
				char c4 = buffer[startIndex++];
				if (c4 == 'E' || c4 == 'e')
				{
					result = StringToken.Queue;
				}
			}
			break;
		}
		case 'R':
		case 'r':
			switch (buffer[startIndex++])
			{
			case 'D':
			case 'd':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'B' && c2 != 'b')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'M' || c3 == 'm')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'S' || c4 == 's')
					{
						result = StringToken.Rdbms;
					}
				}
				break;
			}
			case 'I':
			case 'i':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'G' && c2 != 'g')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'H' || c3 == 'h')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'T' || c4 == 't')
					{
						result = StringToken.Right;
					}
				}
				break;
			}
			case 'O':
			case 'o':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'U' && c2 != 'u')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'T' || c3 == 't')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'E' || c4 == 'e')
					{
						result = StringToken.Route;
					}
				}
				break;
			}
			}
			break;
		case 'S':
		case 's':
		{
			char c = buffer[startIndex++];
			if (c != 'T' && c != 't')
			{
				break;
			}
			switch (buffer[startIndex++])
			{
			case 'A':
			case 'a':
			{
				char c3 = buffer[startIndex++];
				if (c3 == 'R' || c3 == 'r')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'T' || c4 == 't')
					{
						result = StringToken.Start;
					}
				}
				break;
			}
			case 'O':
			case 'o':
			{
				char c3 = buffer[startIndex++];
				if (c3 == 'R' || c3 == 'r')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'E' || c4 == 'e')
					{
						result = StringToken.Store;
					}
				}
				break;
			}
			}
			break;
		}
		case 'T':
		case 't':
			switch (buffer[startIndex++])
			{
			case 'A':
			case 'a':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'B' && c2 != 'b')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'L' || c3 == 'l')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'E' || c4 == 'e')
					{
						result = StringToken.Table;
					}
				}
				break;
			}
			case 'H':
			case 'h':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'R' && c2 != 'r')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'O' || c3 == 'o')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'W' || c4 == 'w')
					{
						result = StringToken.Throw;
					}
				}
				break;
			}
			}
			break;
		case 'U':
		case 'u':
			switch (buffer[startIndex++])
			{
			case 'N':
			case 'n':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'I' && c2 != 'i')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'O' || c3 == 'o')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'N' || c4 == 'n')
					{
						result = StringToken.Union;
					}
				}
				break;
			}
			case 'S':
			case 's':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'I' && c2 != 'i')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'N' || c3 == 'n')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'G' || c4 == 'g')
					{
						result = StringToken.Using;
					}
				}
				break;
			}
			}
			break;
		case 'V':
		case 'v':
		{
			char c = buffer[startIndex++];
			if (c != 'A' && c != 'a')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'L' && c2 != 'l')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 == 'U' || c3 == 'u')
			{
				char c4 = buffer[startIndex++];
				if (c4 == 'E' || c4 == 'e')
				{
					result = StringToken.Value;
				}
			}
			break;
		}
		case 'W':
		case 'w':
		{
			char c = buffer[startIndex++];
			if (c != 'H' && c != 'h')
			{
				break;
			}
			switch (buffer[startIndex++])
			{
			case 'E':
			case 'e':
			{
				char c3 = buffer[startIndex++];
				if (c3 == 'R' || c3 == 'r')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'E' || c4 == 'e')
					{
						result = StringToken.Where;
					}
				}
				break;
			}
			case 'I':
			case 'i':
			{
				char c3 = buffer[startIndex++];
				if (c3 == 'L' || c3 == 'l')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'E' || c4 == 'e')
					{
						result = StringToken.While;
					}
				}
				break;
			}
			}
			break;
		}
		case 'X':
		case 'x':
		{
			char c = buffer[startIndex++];
			if (c != 'L' && c != 'l')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'O' && c2 != 'o')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 == 'C' || c3 == 'c')
			{
				char c4 = buffer[startIndex++];
				if (c4 == 'K' || c4 == 'k')
				{
					result = StringToken.XLock;
				}
			}
			break;
		}
		}
		return result;
	}

	private static StringToken Find_Len6(string buffer, int startIndex)
	{
		StringToken result = StringToken.None;
		switch (buffer[startIndex++])
		{
		case 'A':
		case 'a':
			switch (buffer[startIndex++])
			{
			case 'C':
			case 'c':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'T' && c2 != 't')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'I' && c3 != 'i')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 == 'O' || c4 == 'o')
				{
					char c5 = buffer[startIndex++];
					if (c5 == 'N' || c5 == 'n')
					{
						result = StringToken.Action;
					}
				}
				break;
			}
			case 'L':
			case 'l':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'W' && c2 != 'w')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'A' && c3 != 'a')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 == 'Y' || c4 == 'y')
				{
					char c5 = buffer[startIndex++];
					if (c5 == 'S' || c5 == 's')
					{
						result = StringToken.Always;
					}
				}
				break;
			}
			case 'T':
			case 't':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'O' && c2 != 'o')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'M' && c3 != 'm')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 == 'I' || c4 == 'i')
				{
					char c5 = buffer[startIndex++];
					if (c5 == 'C' || c5 == 'c')
					{
						result = StringToken.Atomic;
					}
				}
				break;
			}
			}
			break;
		case 'B':
		case 'b':
			switch (buffer[startIndex++])
			{
			case 'A':
			case 'a':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'C' && c2 != 'c')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'K' && c3 != 'k')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 == 'U' || c4 == 'u')
				{
					char c5 = buffer[startIndex++];
					if (c5 == 'P' || c5 == 'p')
					{
						result = StringToken.Backup;
					}
				}
				break;
			}
			case 'E':
			case 'e':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'F' && c2 != 'f')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'O' && c3 != 'o')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 == 'R' || c4 == 'r')
				{
					char c5 = buffer[startIndex++];
					if (c5 == 'E' || c5 == 'e')
					{
						result = StringToken.Before;
					}
				}
				break;
			}
			case 'I':
			case 'i':
				switch (buffer[startIndex++])
				{
				case 'G':
				case 'g':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'I' && c3 != 'i')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 == 'N' || c4 == 'n')
					{
						char c5 = buffer[startIndex++];
						if (c5 == 'T' || c5 == 't')
						{
							result = StringToken.BigInt;
						}
					}
					break;
				}
				case 'N':
				case 'n':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'A' && c3 != 'a')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 == 'R' || c4 == 'r')
					{
						char c5 = buffer[startIndex++];
						if (c5 == 'Y' || c5 == 'y')
						{
							result = StringToken.Binary;
						}
					}
					break;
				}
				}
				break;
			case 'R':
			case 'r':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'O' && c2 != 'o')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case 'K':
				case 'k':
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'E' || c4 == 'e')
					{
						char c5 = buffer[startIndex++];
						if (c5 == 'R' || c5 == 'r')
						{
							result = StringToken.Broker;
						}
					}
					break;
				}
				case 'W':
				case 'w':
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'S' || c4 == 's')
					{
						char c5 = buffer[startIndex++];
						if (c5 == 'E' || c5 == 'e')
						{
							result = StringToken.Browse;
						}
					}
					break;
				}
				}
				break;
			}
			case 'U':
			case 'u':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'F' && c2 != 'f')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'F' && c3 != 'f')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 == 'E' || c4 == 'e')
				{
					char c5 = buffer[startIndex++];
					if (c5 == 'R' || c5 == 'r')
					{
						result = StringToken.Buffer;
					}
				}
				break;
			}
			}
			break;
		case 'C':
		case 'c':
			switch (buffer[startIndex++])
			{
			case 'A':
			case 'a':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'L' && c2 != 'l')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'L' && c3 != 'l')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 == 'E' || c4 == 'e')
				{
					switch (buffer[startIndex++])
					{
					case 'D':
					case 'd':
						result = StringToken.Called;
						break;
					case 'R':
					case 'r':
						result = StringToken.Caller;
						break;
					}
				}
				break;
			}
			case 'O':
			case 'o':
				switch (buffer[startIndex++])
				{
				case 'L':
				case 'l':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'U' && c3 != 'u')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 == 'M' || c4 == 'm')
					{
						char c5 = buffer[startIndex++];
						if (c5 == 'N' || c5 == 'n')
						{
							result = StringToken.Column;
						}
					}
					break;
				}
				case 'M':
				case 'm':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'M' && c3 != 'm')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 == 'I' || c4 == 'i')
					{
						char c5 = buffer[startIndex++];
						if (c5 == 'T' || c5 == 't')
						{
							result = StringToken.Commit;
						}
					}
					break;
				}
				}
				break;
			case 'R':
			case 'r':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'E' && c2 != 'e')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'A' && c3 != 'a')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 == 'T' || c4 == 't')
				{
					char c5 = buffer[startIndex++];
					if (c5 == 'E' || c5 == 'e')
					{
						result = StringToken.Create;
					}
				}
				break;
			}
			case 'U':
			case 'u':
				switch (buffer[startIndex++])
				{
				case 'R':
				case 'r':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'S' && c3 != 's')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 == 'O' || c4 == 'o')
					{
						char c5 = buffer[startIndex++];
						if (c5 == 'R' || c5 == 'r')
						{
							result = StringToken.Cursor;
						}
					}
					break;
				}
				case 'S':
				case 's':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'T' && c3 != 't')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 == 'O' || c4 == 'o')
					{
						char c5 = buffer[startIndex++];
						if (c5 == 'M' || c5 == 'm')
						{
							result = StringToken.Custom;
						}
					}
					break;
				}
				}
				break;
			}
			break;
		case 'D':
		case 'd':
			switch (buffer[startIndex++])
			{
			case 'E':
			case 'e':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'L' && c2 != 'l')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'E' && c3 != 'e')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 == 'T' || c4 == 't')
				{
					char c5 = buffer[startIndex++];
					if (c5 == 'E' || c5 == 'e')
					{
						result = StringToken.Delete;
					}
				}
				break;
			}
			case 'I':
			case 'i':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'A' && c2 != 'a')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'L' && c3 != 'l')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 == 'O' || c4 == 'o')
				{
					char c5 = buffer[startIndex++];
					if (c5 == 'G' || c5 == 'g')
					{
						result = StringToken.Dialog;
					}
				}
				break;
			}
			case 'O':
			case 'o':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'U' && c2 != 'u')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'B' && c3 != 'b')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 == 'L' || c4 == 'l')
				{
					char c5 = buffer[startIndex++];
					if (c5 == 'E' || c5 == 'e')
					{
						result = StringToken.Double;
					}
				}
				break;
			}
			}
			break;
		case 'E':
		case 'e':
			switch (buffer[startIndex++])
			{
			case 'N':
			case 'n':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'A' && c2 != 'a')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'B' && c3 != 'b')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 == 'L' || c4 == 'l')
				{
					char c5 = buffer[startIndex++];
					if (c5 == 'E' || c5 == 'e')
					{
						result = StringToken.Enable;
					}
				}
				break;
			}
			case 'R':
			case 'r':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'R' && c2 != 'r')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'L' && c3 != 'l')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 == 'V' || c4 == 'v')
				{
					char c5 = buffer[startIndex++];
					if (c5 == 'L' || c5 == 'l')
					{
						result = StringToken.Errlvl;
					}
				}
				break;
			}
			case 'S':
			case 's':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'C' && c2 != 'c')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'A' && c3 != 'a')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 == 'P' || c4 == 'p')
				{
					char c5 = buffer[startIndex++];
					if (c5 == 'E' || c5 == 'e')
					{
						result = StringToken.Escape;
					}
				}
				break;
			}
			case 'X':
			case 'x':
				switch (buffer[startIndex++])
				{
				case 'C':
				case 'c':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'E' && c3 != 'e')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 == 'P' || c4 == 'p')
					{
						char c5 = buffer[startIndex++];
						if (c5 == 'T' || c5 == 't')
						{
							result = StringToken.Except;
						}
					}
					break;
				}
				case 'I':
				case 'i':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'S' && c3 != 's')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 == 'T' || c4 == 't')
					{
						char c5 = buffer[startIndex++];
						if (c5 == 'S' || c5 == 's')
						{
							result = StringToken.Exists;
						}
					}
					break;
				}
				}
				break;
			}
			break;
		case 'F':
		case 'f':
			switch (buffer[startIndex++])
			{
			case 'I':
			case 'i':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'L' && c2 != 'l')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'T' && c3 != 't')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 == 'E' || c4 == 'e')
				{
					char c5 = buffer[startIndex++];
					if (c5 == 'R' || c5 == 'r')
					{
						result = StringToken.Filter;
					}
				}
				break;
			}
			case 'O':
			case 'o':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'R' && c2 != 'r')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'M' && c3 != 'm')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 == 'A' || c4 == 'a')
				{
					char c5 = buffer[startIndex++];
					if (c5 == 'T' || c5 == 't')
					{
						result = StringToken.Format;
					}
				}
				break;
			}
			}
			break;
		case 'G':
		case 'g':
		{
			char c = buffer[startIndex++];
			if (c != 'L' && c != 'l')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'O' && c2 != 'o')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'B' && c3 != 'b')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 == 'A' || c4 == 'a')
			{
				char c5 = buffer[startIndex++];
				if (c5 == 'L' || c5 == 'l')
				{
					result = StringToken.Global;
				}
			}
			break;
		}
		case 'H':
		case 'h':
			switch (buffer[startIndex++])
			{
			case 'A':
			case 'a':
				switch (buffer[startIndex++])
				{
				case 'D':
				case 'd':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'O' && c3 != 'o')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 == 'O' || c4 == 'o')
					{
						char c5 = buffer[startIndex++];
						if (c5 == 'P' || c5 == 'p')
						{
							result = StringToken.Hadoop;
						}
					}
					break;
				}
				case 'S':
				case 's':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'H' && c3 != 'h')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 == 'E' || c4 == 'e')
					{
						char c5 = buffer[startIndex++];
						if (c5 == 'D' || c5 == 'd')
						{
							result = StringToken.Hashed;
						}
					}
					break;
				}
				case 'V':
				case 'v':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'I' && c3 != 'i')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 == 'N' || c4 == 'n')
					{
						char c5 = buffer[startIndex++];
						if (c5 == 'G' || c5 == 'g')
						{
							result = StringToken.Having;
						}
					}
					break;
				}
				}
				break;
			case 'I':
			case 'i':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'D' && c2 != 'd')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'D' && c3 != 'd')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 == 'E' || c4 == 'e')
				{
					char c5 = buffer[startIndex++];
					if (c5 == 'N' || c5 == 'n')
					{
						result = StringToken.Hidden;
					}
				}
				break;
			}
			}
			break;
		case 'I':
		case 'i':
		{
			char c = buffer[startIndex++];
			if (c != 'N' && c != 'n')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'S' && c2 != 's')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'E' && c3 != 'e')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 == 'R' || c4 == 'r')
			{
				char c5 = buffer[startIndex++];
				if (c5 == 'T' || c5 == 't')
				{
					result = StringToken.Insert;
				}
			}
			break;
		}
		case 'K':
		case 'k':
		{
			char c = buffer[startIndex++];
			if (c != 'E' && c != 'e')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'Y' && c2 != 'y')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'S' && c3 != 's')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 == 'E' || c4 == 'e')
			{
				char c5 = buffer[startIndex++];
				if (c5 == 'T' || c5 == 't')
				{
					result = StringToken.Keyset;
				}
			}
			break;
		}
		case 'L':
		case 'l':
			switch (buffer[startIndex++])
			{
			case 'A':
			case 'a':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'N' && c2 != 'n')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'G' && c3 != 'g')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 == 'I' || c4 == 'i')
				{
					char c5 = buffer[startIndex++];
					if (c5 == 'D' || c5 == 'd')
					{
						result = StringToken.Langid;
					}
				}
				break;
			}
			case 'E':
			case 'e':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'D' && c2 != 'd')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'G' && c3 != 'g')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 == 'E' || c4 == 'e')
				{
					char c5 = buffer[startIndex++];
					if (c5 == 'R' || c5 == 'r')
					{
						result = StringToken.Ledger;
					}
				}
				break;
			}
			case 'I':
			case 'i':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'N' && c2 != 'n')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'E' && c3 != 'e')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 == 'N' || c4 == 'n')
				{
					char c5 = buffer[startIndex++];
					if (c5 == 'O' || c5 == 'o')
					{
						result = StringToken.LineNo;
					}
				}
				break;
			}
			}
			break;
		case 'M':
		case 'm':
			switch (buffer[startIndex++])
			{
			case 'A':
			case 'a':
				switch (buffer[startIndex++])
				{
				case 'S':
				case 's':
					switch (buffer[startIndex++])
					{
					case 'K':
					case 'k':
					{
						char c4 = buffer[startIndex++];
						if (c4 == 'E' || c4 == 'e')
						{
							char c5 = buffer[startIndex++];
							if (c5 == 'D' || c5 == 'd')
							{
								result = StringToken.Masked;
							}
						}
						break;
					}
					case 'T':
					case 't':
					{
						char c4 = buffer[startIndex++];
						if (c4 == 'E' || c4 == 'e')
						{
							char c5 = buffer[startIndex++];
							if (c5 == 'R' || c5 == 'r')
							{
								result = StringToken.Master;
							}
						}
						break;
					}
					}
					break;
				case 'X':
				case 'x':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'D' && c3 != 'd')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 == 'O' || c4 == 'o')
					{
						char c5 = buffer[startIndex++];
						if (c5 == 'P' || c5 == 'p')
						{
							result = StringToken.MaxDop;
						}
					}
					break;
				}
				}
				break;
			case 'I':
			case 'i':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'N' && c2 != 'n')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'U' && c3 != 'u')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 == 'T' || c4 == 't')
				{
					char c5 = buffer[startIndex++];
					if (c5 == 'E' || c5 == 'e')
					{
						result = StringToken.Minute;
					}
				}
				break;
			}
			case 'O':
			case 'o':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'D' && c2 != 'd')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'I' && c3 != 'i')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 == 'F' || c4 == 'f')
				{
					char c5 = buffer[startIndex++];
					if (c5 == 'Y' || c5 == 'y')
					{
						result = StringToken.Modify;
					}
				}
				break;
			}
			}
			break;
		case 'N':
		case 'n':
			switch (buffer[startIndex++])
			{
			case 'O':
			case 'o':
				switch (buffer[startIndex++])
				{
				case 'L':
				case 'l':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'O' && c3 != 'o')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 == 'C' || c4 == 'c')
					{
						char c5 = buffer[startIndex++];
						if (c5 == 'K' || c5 == 'k')
						{
							result = StringToken.NoLock;
						}
					}
					break;
				}
				case 'W':
				case 'w':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'A' && c3 != 'a')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 == 'I' || c4 == 'i')
					{
						char c5 = buffer[startIndex++];
						if (c5 == 'T' || c5 == 't')
						{
							result = StringToken.NoWait;
						}
					}
					break;
				}
				}
				break;
			case 'U':
			case 'u':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'L' && c2 != 'l')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'L' && c3 != 'l')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 == 'I' || c4 == 'i')
				{
					char c5 = buffer[startIndex++];
					if (c5 == 'F' || c5 == 'f')
					{
						result = StringToken.NullIf;
					}
				}
				break;
			}
			}
			break;
		case 'O':
		case 'o':
			switch (buffer[startIndex++])
			{
			case 'F':
			case 'f':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'F' && c2 != 'f')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'S' && c3 != 's')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 == 'E' || c4 == 'e')
				{
					char c5 = buffer[startIndex++];
					if (c5 == 'T' || c5 == 't')
					{
						result = StringToken.Offset;
					}
				}
				break;
			}
			case 'N':
			case 'n':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'L' && c2 != 'l')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'I' && c3 != 'i')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 == 'N' || c4 == 'n')
				{
					char c5 = buffer[startIndex++];
					if (c5 == 'E' || c5 == 'e')
					{
						result = StringToken.Online;
					}
				}
				break;
			}
			case 'P':
			case 'p':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'T' && c2 != 't')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'I' && c3 != 'i')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 == 'O' || c4 == 'o')
				{
					char c5 = buffer[startIndex++];
					if (c5 == 'N' || c5 == 'n')
					{
						result = StringToken.Option;
					}
				}
				break;
			}
			case 'U':
			case 'u':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'T' && c2 != 't')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'P' && c3 != 'p')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 == 'U' || c4 == 'u')
				{
					char c5 = buffer[startIndex++];
					if (c5 == 'T' || c5 == 't')
					{
						result = StringToken.Output;
					}
				}
				break;
			}
			}
			break;
		case 'P':
		case 'p':
			switch (buffer[startIndex++])
			{
			case 'E':
			case 'e':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'R' && c2 != 'r')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'I' && c3 != 'i')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 == 'O' || c4 == 'o')
				{
					char c5 = buffer[startIndex++];
					if (c5 == 'D' || c5 == 'd')
					{
						result = StringToken.Period;
					}
				}
				break;
			}
			case 'O':
			case 'o':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'L' && c2 != 'l')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'I' && c3 != 'i')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 == 'C' || c4 == 'c')
				{
					char c5 = buffer[startIndex++];
					if (c5 == 'Y' || c5 == 'y')
					{
						result = StringToken.Policy;
					}
				}
				break;
			}
			case 'R':
			case 'r':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'O' && c2 != 'o')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'C' && c3 != 'c')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 == 'I' || c4 == 'i')
				{
					char c5 = buffer[startIndex++];
					if (c5 == 'D' || c5 == 'd')
					{
						result = StringToken.Procid;
					}
				}
				break;
			}
			case 'U':
			case 'u':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'B' && c2 != 'b')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'L' && c3 != 'l')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 == 'I' || c4 == 'i')
				{
					char c5 = buffer[startIndex++];
					if (c5 == 'C' || c5 == 'c')
					{
						result = StringToken.Public;
					}
				}
				break;
			}
			}
			break;
		case 'R':
		case 'r':
			switch (buffer[startIndex++])
			{
			case 'C':
			case 'c':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'F' && c2 != 'f')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'I' && c3 != 'i')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 == 'L' || c4 == 'l')
				{
					char c5 = buffer[startIndex++];
					if (c5 == 'E' || c5 == 'e')
					{
						result = StringToken.Rcfile;
					}
				}
				break;
			}
			case 'E':
			case 'e':
				switch (buffer[startIndex++])
				{
				case 'M':
				case 'm':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'O' && c3 != 'o')
					{
						break;
					}
					switch (buffer[startIndex++])
					{
					case 'T':
					case 't':
					{
						char c5 = buffer[startIndex++];
						if (c5 == 'E' || c5 == 'e')
						{
							result = StringToken.Remote;
						}
						break;
					}
					case 'V':
					case 'v':
					{
						char c5 = buffer[startIndex++];
						if (c5 == 'E' || c5 == 'e')
						{
							result = StringToken.Remove;
						}
						break;
					}
					}
					break;
				}
				case 'S':
				case 's':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'U' && c3 != 'u')
					{
						break;
					}
					switch (buffer[startIndex++])
					{
					case 'L':
					case 'l':
					{
						char c5 = buffer[startIndex++];
						if (c5 == 'T' || c5 == 't')
						{
							result = StringToken.Result;
						}
						break;
					}
					case 'M':
					case 'm':
					{
						char c5 = buffer[startIndex++];
						if (c5 == 'E' || c5 == 'e')
						{
							result = StringToken.Resume;
						}
						break;
					}
					}
					break;
				}
				case 'T':
				case 't':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'U' && c3 != 'u')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 == 'R' || c4 == 'r')
					{
						char c5 = buffer[startIndex++];
						if (c5 == 'N' || c5 == 'n')
						{
							result = StringToken.Return;
						}
					}
					break;
				}
				case 'V':
				case 'v':
					switch (buffer[startIndex++])
					{
					case 'E':
					case 'e':
					{
						char c4 = buffer[startIndex++];
						if (c4 == 'R' || c4 == 'r')
						{
							char c5 = buffer[startIndex++];
							if (c5 == 'T' || c5 == 't')
							{
								result = StringToken.Revert;
							}
						}
						break;
					}
					case 'O':
					case 'o':
					{
						char c4 = buffer[startIndex++];
						if (c4 == 'K' || c4 == 'k')
						{
							char c5 = buffer[startIndex++];
							if (c5 == 'E' || c5 == 'e')
							{
								result = StringToken.Revoke;
							}
						}
						break;
					}
					}
					break;
				}
				break;
			case 'O':
			case 'o':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'L' && c2 != 'l')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'L' && c3 != 'l')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 == 'U' || c4 == 'u')
				{
					char c5 = buffer[startIndex++];
					if (c5 == 'P' || c5 == 'p')
					{
						result = StringToken.Rollup;
					}
				}
				break;
			}
			}
			break;
		case 'S':
		case 's':
			switch (buffer[startIndex++])
			{
			case 'C':
			case 'c':
				switch (buffer[startIndex++])
				{
				case 'H':
				case 'h':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'E' && c3 != 'e')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 == 'M' || c4 == 'm')
					{
						char c5 = buffer[startIndex++];
						if (c5 == 'A' || c5 == 'a')
						{
							result = StringToken.Schema;
						}
					}
					break;
				}
				case 'O':
				case 'o':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'P' && c3 != 'p')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 == 'E' || c4 == 'e')
					{
						char c5 = buffer[startIndex++];
						if (c5 == 'D' || c5 == 'd')
						{
							result = StringToken.Scoped;
						}
					}
					break;
				}
				case 'R':
				case 'r':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'O' && c3 != 'o')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 == 'L' || c4 == 'l')
					{
						char c5 = buffer[startIndex++];
						if (c5 == 'L' || c5 == 'l')
						{
							result = StringToken.Scroll;
						}
					}
					break;
				}
				}
				break;
			case 'E':
			case 'e':
				switch (buffer[startIndex++])
				{
				case 'A':
				case 'a':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'R' && c3 != 'r')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 == 'C' || c4 == 'c')
					{
						char c5 = buffer[startIndex++];
						if (c5 == 'H' || c5 == 'h')
						{
							result = StringToken.Search;
						}
					}
					break;
				}
				case 'C':
				case 'c':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'O' && c3 != 'o')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 == 'N' || c4 == 'n')
					{
						char c5 = buffer[startIndex++];
						if (c5 == 'D' || c5 == 'd')
						{
							result = StringToken.Second;
						}
					}
					break;
				}
				case 'L':
				case 'l':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'E' && c3 != 'e')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 == 'C' || c4 == 'c')
					{
						char c5 = buffer[startIndex++];
						if (c5 == 'T' || c5 == 't')
						{
							result = StringToken.Select;
						}
					}
					break;
				}
				case 'R':
				case 'r':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'V' && c3 != 'v')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 == 'E' || c4 == 'e')
					{
						char c5 = buffer[startIndex++];
						if (c5 == 'R' || c5 == 'r')
						{
							result = StringToken.Server;
						}
					}
					break;
				}
				}
				break;
			case 'O':
			case 'o':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'U' && c2 != 'u')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'R' && c3 != 'r')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 == 'C' || c4 == 'c')
				{
					char c5 = buffer[startIndex++];
					if (c5 == 'E' || c5 == 'e')
					{
						result = StringToken.Source;
					}
				}
				break;
			}
			case 'P':
			case 'p':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'A' && c2 != 'a')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'R' && c3 != 'r')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 == 'S' || c4 == 's')
				{
					char c5 = buffer[startIndex++];
					if (c5 == 'E' || c5 == 'e')
					{
						result = StringToken.Sparse;
					}
				}
				break;
			}
			case 'T':
			case 't':
				switch (buffer[startIndex++])
				{
				case 'A':
				case 'a':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'T' && c3 != 't')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 == 'I' || c4 == 'i')
					{
						char c5 = buffer[startIndex++];
						if (c5 == 'C' || c5 == 'c')
						{
							result = StringToken.Static;
						}
					}
					break;
				}
				case 'R':
				case 'r':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'E' && c3 != 'e')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 == 'A' || c4 == 'a')
					{
						char c5 = buffer[startIndex++];
						if (c5 == 'M' || c5 == 'm')
						{
							result = StringToken.Stream;
						}
					}
					break;
				}
				}
				break;
			case 'W':
			case 'w':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'I' && c2 != 'i')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'T' && c3 != 't')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 == 'C' || c4 == 'c')
				{
					char c5 = buffer[startIndex++];
					if (c5 == 'H' || c5 == 'h')
					{
						result = StringToken.Switch;
					}
				}
				break;
			}
			}
			break;
		case 'T':
		case 't':
		{
			char c = buffer[startIndex++];
			if (c != 'A' && c != 'a')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'R' && c2 != 'r')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'G' && c3 != 'g')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 == 'E' || c4 == 'e')
			{
				char c5 = buffer[startIndex++];
				if (c5 == 'T' || c5 == 't')
				{
					result = StringToken.Target;
				}
			}
			break;
		}
		case 'U':
		case 'u':
			switch (buffer[startIndex++])
			{
			case 'N':
			case 'n':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'I' && c2 != 'i')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'Q' && c3 != 'q')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 == 'U' || c4 == 'u')
				{
					char c5 = buffer[startIndex++];
					if (c5 == 'E' || c5 == 'e')
					{
						result = StringToken.Unique;
					}
				}
				break;
			}
			case 'P':
			case 'p':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'D' && c2 != 'd')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'A' && c3 != 'a')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 == 'T' || c4 == 't')
				{
					char c5 = buffer[startIndex++];
					if (c5 == 'E' || c5 == 'e')
					{
						result = StringToken.Update;
					}
				}
				break;
			}
			}
			break;
		case 'V':
		case 'v':
		{
			char c = buffer[startIndex++];
			if (c != 'A' && c != 'a')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'L' && c2 != 'l')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'U' && c3 != 'u')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 == 'E' || c4 == 'e')
			{
				char c5 = buffer[startIndex++];
				if (c5 == 'S' || c5 == 's')
				{
					result = StringToken.Values;
				}
			}
			break;
		}
		case 'W':
		case 'w':
		{
			char c = buffer[startIndex++];
			if (c != 'I' && c != 'i')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'T' && c2 != 't')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'H' && c3 != 'h')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 == 'I' || c4 == 'i')
			{
				char c5 = buffer[startIndex++];
				if (c5 == 'N' || c5 == 'n')
				{
					result = StringToken.Within;
				}
			}
			break;
		}
		case 'X':
		case 'x':
		{
			char c = buffer[startIndex++];
			if (c != 'Q' && c != 'q')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'U' && c2 != 'u')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'E' && c3 != 'e')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 == 'R' || c4 == 'r')
			{
				char c5 = buffer[startIndex++];
				if (c5 == 'Y' || c5 == 'y')
				{
					result = StringToken.XQuery;
				}
			}
			break;
		}
		}
		return result;
	}

	private static StringToken Find_Len7(string buffer, int startIndex)
	{
		StringToken result = StringToken.None;
		switch (buffer[startIndex++])
		{
		case 'B':
		case 'b':
		{
			char c6 = buffer[startIndex++];
			if (c6 != 'E' && c6 != 'e')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'T' && c5 != 't')
			{
				break;
			}
			char c = buffer[startIndex++];
			if (c != 'W' && c != 'w')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'E' && c2 != 'e')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 == 'E' || c3 == 'e')
			{
				char c4 = buffer[startIndex++];
				if (c4 == 'N' || c4 == 'n')
				{
					result = StringToken.Between;
				}
			}
			break;
		}
		case 'C':
		case 'c':
			switch (buffer[startIndex++])
			{
			case 'A':
			case 'a':
				switch (buffer[startIndex++])
				{
				case 'S':
				case 's':
				{
					char c = buffer[startIndex++];
					if (c != 'C' && c != 'c')
					{
						break;
					}
					char c2 = buffer[startIndex++];
					if (c2 != 'A' && c2 != 'a')
					{
						break;
					}
					char c3 = buffer[startIndex++];
					if (c3 == 'D' || c3 == 'd')
					{
						char c4 = buffer[startIndex++];
						if (c4 == 'E' || c4 == 'e')
						{
							result = StringToken.Cascade;
						}
					}
					break;
				}
				case 'T':
				case 't':
				{
					char c = buffer[startIndex++];
					if (c != 'A' && c != 'a')
					{
						break;
					}
					char c2 = buffer[startIndex++];
					if (c2 != 'L' && c2 != 'l')
					{
						break;
					}
					char c3 = buffer[startIndex++];
					if (c3 == 'O' || c3 == 'o')
					{
						char c4 = buffer[startIndex++];
						if (c4 == 'G' || c4 == 'g')
						{
							result = StringToken.Catalog;
						}
					}
					break;
				}
				}
				break;
			case 'O':
			case 'o':
				switch (buffer[startIndex++])
				{
				case 'L':
				case 'l':
				{
					char c = buffer[startIndex++];
					if (c != 'L' && c != 'l')
					{
						break;
					}
					char c2 = buffer[startIndex++];
					if (c2 != 'A' && c2 != 'a')
					{
						break;
					}
					char c3 = buffer[startIndex++];
					if (c3 == 'T' || c3 == 't')
					{
						char c4 = buffer[startIndex++];
						if (c4 == 'E' || c4 == 'e')
						{
							result = StringToken.Collate;
						}
					}
					break;
				}
				case 'M':
				case 'm':
				{
					char c = buffer[startIndex++];
					if (c != 'P' && c != 'p')
					{
						break;
					}
					char c2 = buffer[startIndex++];
					if (c2 != 'U' && c2 != 'u')
					{
						break;
					}
					char c3 = buffer[startIndex++];
					if (c3 == 'T' || c3 == 't')
					{
						char c4 = buffer[startIndex++];
						if (c4 == 'E' || c4 == 'e')
						{
							result = StringToken.Compute;
						}
					}
					break;
				}
				case 'N':
				case 'n':
					switch (buffer[startIndex++])
					{
					case 'T':
					case 't':
					{
						char c2 = buffer[startIndex++];
						if (c2 != 'E' && c2 != 'e')
						{
							break;
						}
						switch (buffer[startIndex++])
						{
						case 'N':
						case 'n':
						{
							char c4 = buffer[startIndex++];
							if (c4 == 'T' || c4 == 't')
							{
								result = StringToken.Content;
							}
							break;
						}
						case 'X':
						case 'x':
						{
							char c4 = buffer[startIndex++];
							if (c4 == 'T' || c4 == 't')
							{
								result = StringToken.Context;
							}
							break;
						}
						}
						break;
					}
					case 'V':
					case 'v':
					{
						char c2 = buffer[startIndex++];
						if (c2 != 'E' && c2 != 'e')
						{
							break;
						}
						char c3 = buffer[startIndex++];
						if (c3 == 'R' || c3 == 'r')
						{
							char c4 = buffer[startIndex++];
							if (c4 == 'T' || c4 == 't')
							{
								result = StringToken.Convert;
							}
						}
						break;
					}
					}
					break;
				case 'U':
				case 'u':
				{
					char c = buffer[startIndex++];
					if (c != 'N' && c != 'n')
					{
						break;
					}
					char c2 = buffer[startIndex++];
					if (c2 != 'T' && c2 != 't')
					{
						break;
					}
					char c3 = buffer[startIndex++];
					if (c3 == 'E' || c3 == 'e')
					{
						char c4 = buffer[startIndex++];
						if (c4 == 'R' || c4 == 'r')
						{
							result = StringToken.Counter;
						}
					}
					break;
				}
				}
				break;
			case 'U':
			case 'u':
			{
				char c5 = buffer[startIndex++];
				if (c5 != 'R' && c5 != 'r')
				{
					break;
				}
				char c = buffer[startIndex++];
				if (c != 'R' && c != 'r')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'E' && c2 != 'e')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'N' || c3 == 'n')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'T' || c4 == 't')
					{
						result = StringToken.Current;
					}
				}
				break;
			}
			}
			break;
		case 'D':
		case 'd':
			switch (buffer[startIndex++])
			{
			case 'E':
			case 'e':
				switch (buffer[startIndex++])
				{
				case 'C':
				case 'c':
					switch (buffer[startIndex++])
					{
					case 'I':
					case 'i':
					{
						char c2 = buffer[startIndex++];
						if (c2 != 'M' && c2 != 'm')
						{
							break;
						}
						char c3 = buffer[startIndex++];
						if (c3 == 'A' || c3 == 'a')
						{
							char c4 = buffer[startIndex++];
							if (c4 == 'L' || c4 == 'l')
							{
								result = StringToken.Decimal;
							}
						}
						break;
					}
					case 'L':
					case 'l':
					{
						char c2 = buffer[startIndex++];
						if (c2 != 'A' && c2 != 'a')
						{
							break;
						}
						char c3 = buffer[startIndex++];
						if (c3 == 'R' || c3 == 'r')
						{
							char c4 = buffer[startIndex++];
							if (c4 == 'E' || c4 == 'e')
							{
								result = StringToken.Declare;
							}
						}
						break;
					}
					}
					break;
				case 'F':
				case 'f':
				{
					char c = buffer[startIndex++];
					if (c != 'A' && c != 'a')
					{
						break;
					}
					char c2 = buffer[startIndex++];
					if (c2 != 'U' && c2 != 'u')
					{
						break;
					}
					char c3 = buffer[startIndex++];
					if (c3 == 'L' || c3 == 'l')
					{
						char c4 = buffer[startIndex++];
						if (c4 == 'T' || c4 == 't')
						{
							result = StringToken.Default;
						}
					}
					break;
				}
				}
				break;
			case 'I':
			case 'i':
			{
				char c5 = buffer[startIndex++];
				if (c5 != 'S' && c5 != 's')
				{
					break;
				}
				char c = buffer[startIndex++];
				if (c != 'A' && c != 'a')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'B' && c2 != 'b')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'L' || c3 == 'l')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'E' || c4 == 'e')
					{
						result = StringToken.Disable;
					}
				}
				break;
			}
			case 'Y':
			case 'y':
			{
				char c5 = buffer[startIndex++];
				if (c5 != 'N' && c5 != 'n')
				{
					break;
				}
				char c = buffer[startIndex++];
				if (c != 'A' && c != 'a')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'M' && c2 != 'm')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'I' || c3 == 'i')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'C' || c4 == 'c')
					{
						result = StringToken.Dynamic;
					}
				}
				break;
			}
			}
			break;
		case 'E':
		case 'e':
		{
			char c6 = buffer[startIndex++];
			if (c6 != 'X' && c6 != 'x')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'E' && c5 != 'e')
			{
				break;
			}
			char c = buffer[startIndex++];
			if (c != 'C' && c != 'c')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'U' && c2 != 'u')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 == 'T' || c3 == 't')
			{
				char c4 = buffer[startIndex++];
				if (c4 == 'E' || c4 == 'e')
				{
					result = StringToken.Execute;
				}
			}
			break;
		}
		case 'F':
		case 'f':
		{
			char c6 = buffer[startIndex++];
			if (c6 != 'O' && c6 != 'o')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'R' && c5 != 'r')
			{
				break;
			}
			char c = buffer[startIndex++];
			if (c != 'E' && c != 'e')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'I' && c2 != 'i')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 == 'G' || c3 == 'g')
			{
				char c4 = buffer[startIndex++];
				if (c4 == 'N' || c4 == 'n')
				{
					result = StringToken.Foreign;
				}
			}
			break;
		}
		case 'I':
		case 'i':
			switch (buffer[startIndex++])
			{
			case 'N':
			case 'n':
				switch (buffer[startIndex++])
				{
				case 'C':
				case 'c':
				{
					char c = buffer[startIndex++];
					if (c != 'L' && c != 'l')
					{
						break;
					}
					char c2 = buffer[startIndex++];
					if (c2 != 'U' && c2 != 'u')
					{
						break;
					}
					char c3 = buffer[startIndex++];
					if (c3 == 'D' || c3 == 'd')
					{
						char c4 = buffer[startIndex++];
						if (c4 == 'E' || c4 == 'e')
						{
							result = StringToken.Include;
						}
					}
					break;
				}
				case 'H':
				case 'h':
				{
					char c = buffer[startIndex++];
					if (c != 'E' && c != 'e')
					{
						break;
					}
					char c2 = buffer[startIndex++];
					if (c2 != 'R' && c2 != 'r')
					{
						break;
					}
					char c3 = buffer[startIndex++];
					if (c3 == 'I' || c3 == 'i')
					{
						char c4 = buffer[startIndex++];
						if (c4 == 'T' || c4 == 't')
						{
							result = StringToken.Inherit;
						}
					}
					break;
				}
				case 'S':
				case 's':
				{
					char c = buffer[startIndex++];
					if (c != 'T' && c != 't')
					{
						break;
					}
					char c2 = buffer[startIndex++];
					if (c2 != 'E' && c2 != 'e')
					{
						break;
					}
					char c3 = buffer[startIndex++];
					if (c3 == 'A' || c3 == 'a')
					{
						char c4 = buffer[startIndex++];
						if (c4 == 'D' || c4 == 'd')
						{
							result = StringToken.Instead;
						}
					}
					break;
				}
				case 'T':
				case 't':
				{
					char c = buffer[startIndex++];
					if (c != 'E' && c != 'e')
					{
						break;
					}
					char c2 = buffer[startIndex++];
					if (c2 != 'G' && c2 != 'g')
					{
						break;
					}
					char c3 = buffer[startIndex++];
					if (c3 == 'E' || c3 == 'e')
					{
						char c4 = buffer[startIndex++];
						if (c4 == 'R' || c4 == 'r')
						{
							result = StringToken.Integer;
						}
					}
					break;
				}
				}
				break;
			case 'O':
			case 'o':
			{
				if (buffer[startIndex++] != '_')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'B' && c5 != 'b')
				{
					break;
				}
				char c = buffer[startIndex++];
				if (c != 'U' && c != 'u')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 == 'S' || c2 == 's')
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'Y' || c3 == 'y')
					{
						result = StringToken.Io_busy;
					}
				}
				break;
			}
			}
			break;
		case 'L':
		case 'l':
		{
			char c6 = buffer[startIndex++];
			if (c6 != 'I' && c6 != 'i')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'B' && c5 != 'b')
			{
				break;
			}
			char c = buffer[startIndex++];
			if (c != 'R' && c != 'r')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'A' && c2 != 'a')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 == 'R' || c3 == 'r')
			{
				char c4 = buffer[startIndex++];
				if (c4 == 'Y' || c4 == 'y')
				{
					result = StringToken.Library;
				}
			}
			break;
		}
		case 'M':
		case 'm':
			switch (buffer[startIndex++])
			{
			case 'A':
			case 'a':
			{
				char c5 = buffer[startIndex++];
				if (c5 != 'T' && c5 != 't')
				{
					break;
				}
				char c = buffer[startIndex++];
				if (c != 'C' && c != 'c')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'H' && c2 != 'h')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'E' || c3 == 'e')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'D' || c4 == 'd')
					{
						result = StringToken.Matched;
					}
				}
				break;
			}
			case 'D':
			case 'd':
			{
				char c5 = buffer[startIndex++];
				if ((c5 != 'S' && c5 != 's') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c = buffer[startIndex++];
				if (c != 'L' && c != 'l')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 == 'O' || c2 == 'o')
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'G' || c3 == 'g')
					{
						result = StringToken.MdsLog;
					}
				}
				break;
			}
			case 'E':
			case 'e':
			{
				char c5 = buffer[startIndex++];
				if (c5 != 'S' && c5 != 's')
				{
					break;
				}
				char c = buffer[startIndex++];
				if (c != 'S' && c != 's')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'A' && c2 != 'a')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'G' || c3 == 'g')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'E' || c4 == 'e')
					{
						result = StringToken.Message;
					}
				}
				break;
			}
			case 'I':
			case 'i':
			{
				char c5 = buffer[startIndex++];
				if (c5 != 'N' && c5 != 'n')
				{
					break;
				}
				char c = buffer[startIndex++];
				if (c != 'U' && c != 'u')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'T' && c2 != 't')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'E' || c3 == 'e')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'S' || c4 == 's')
					{
						result = StringToken.Minutes;
					}
				}
				break;
			}
			}
			break;
		case 'N':
		case 'n':
			switch (buffer[startIndex++])
			{
			case 'O':
			case 'o':
			{
				char c5 = buffer[startIndex++];
				if (c5 != 'C' && c5 != 'c')
				{
					break;
				}
				char c = buffer[startIndex++];
				if (c != 'H' && c != 'h')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'E' && c2 != 'e')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'C' || c3 == 'c')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'K' || c4 == 'k')
					{
						result = StringToken.NoCheck;
					}
				}
				break;
			}
			case 'U':
			case 'u':
			{
				char c5 = buffer[startIndex++];
				if (c5 != 'M' && c5 != 'm')
				{
					break;
				}
				char c = buffer[startIndex++];
				if (c != 'E' && c != 'e')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'R' && c2 != 'r')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'I' || c3 == 'i')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'C' || c4 == 'c')
					{
						result = StringToken.Numeric;
					}
				}
				break;
			}
			}
			break;
		case 'O':
		case 'o':
			switch (buffer[startIndex++])
			{
			case 'F':
			case 'f':
			{
				char c5 = buffer[startIndex++];
				if (c5 != 'F' && c5 != 'f')
				{
					break;
				}
				char c = buffer[startIndex++];
				if (c != 'S' && c != 's')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'E' && c2 != 'e')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'T' || c3 == 't')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'S' || c4 == 's')
					{
						result = StringToken.Offsets;
					}
				}
				break;
			}
			case 'P':
			case 'p':
				switch (buffer[startIndex++])
				{
				case 'E':
				case 'e':
				{
					char c = buffer[startIndex++];
					if (c != 'N' && c != 'n')
					{
						break;
					}
					char c2 = buffer[startIndex++];
					if (c2 != 'X' && c2 != 'x')
					{
						break;
					}
					char c3 = buffer[startIndex++];
					if (c3 == 'M' || c3 == 'm')
					{
						char c4 = buffer[startIndex++];
						if (c4 == 'L' || c4 == 'l')
						{
							result = StringToken.OpenXml;
						}
					}
					break;
				}
				case 'T':
				case 't':
				{
					char c = buffer[startIndex++];
					if (c != 'I' && c != 'i')
					{
						break;
					}
					char c2 = buffer[startIndex++];
					if (c2 != 'O' && c2 != 'o')
					{
						break;
					}
					char c3 = buffer[startIndex++];
					if (c3 == 'N' || c3 == 'n')
					{
						char c4 = buffer[startIndex++];
						if (c4 == 'S' || c4 == 's')
						{
							result = StringToken.Options;
						}
					}
					break;
				}
				}
				break;
			}
			break;
		case 'P':
		case 'p':
			switch (buffer[startIndex++])
			{
			case 'A':
			case 'a':
				switch (buffer[startIndex++])
				{
				case 'G':
				case 'g':
				{
					char c = buffer[startIndex++];
					if (c != 'L' && c != 'l')
					{
						break;
					}
					char c2 = buffer[startIndex++];
					if (c2 != 'O' && c2 != 'o')
					{
						break;
					}
					char c3 = buffer[startIndex++];
					if (c3 == 'C' || c3 == 'c')
					{
						char c4 = buffer[startIndex++];
						if (c4 == 'K' || c4 == 'k')
						{
							result = StringToken.PagLock;
						}
					}
					break;
				}
				case 'R':
				case 'r':
				{
					char c = buffer[startIndex++];
					if (c != 'Q' && c != 'q')
					{
						break;
					}
					char c2 = buffer[startIndex++];
					if (c2 != 'U' && c2 != 'u')
					{
						break;
					}
					char c3 = buffer[startIndex++];
					if (c3 == 'E' || c3 == 'e')
					{
						char c4 = buffer[startIndex++];
						if (c4 == 'T' || c4 == 't')
						{
							result = StringToken.Parquet;
						}
					}
					break;
				}
				}
				break;
			case 'E':
			case 'e':
			{
				char c5 = buffer[startIndex++];
				if (c5 != 'R' && c5 != 'r')
				{
					break;
				}
				char c = buffer[startIndex++];
				if (c != 'C' && c != 'c')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'E' && c2 != 'e')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'N' || c3 == 'n')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'T' || c4 == 't')
					{
						result = StringToken.Percent;
					}
				}
				break;
			}
			case 'R':
			case 'r':
				switch (buffer[startIndex++])
				{
				case 'I':
				case 'i':
				{
					char c = buffer[startIndex++];
					if (c != 'M' && c != 'm')
					{
						break;
					}
					char c2 = buffer[startIndex++];
					if (c2 != 'A' && c2 != 'a')
					{
						break;
					}
					char c3 = buffer[startIndex++];
					if (c3 == 'R' || c3 == 'r')
					{
						char c4 = buffer[startIndex++];
						if (c4 == 'Y' || c4 == 'y')
						{
							result = StringToken.Primary;
						}
					}
					break;
				}
				case 'O':
				case 'o':
				{
					char c = buffer[startIndex++];
					if (c != 'C' && c != 'c')
					{
						break;
					}
					char c2 = buffer[startIndex++];
					if (c2 != 'E' && c2 != 'e')
					{
						break;
					}
					char c3 = buffer[startIndex++];
					if (c3 == 'S' || c3 == 's')
					{
						char c4 = buffer[startIndex++];
						if (c4 == 'S' || c4 == 's')
						{
							result = StringToken.Process;
						}
					}
					break;
				}
				}
				break;
			}
			break;
		case 'Q':
		case 'q':
		{
			char c6 = buffer[startIndex++];
			if (c6 != 'U' && c6 != 'u')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'A' && c5 != 'a')
			{
				break;
			}
			char c = buffer[startIndex++];
			if (c != 'R' && c != 'r')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'T' && c2 != 't')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 == 'E' || c3 == 'e')
			{
				char c4 = buffer[startIndex++];
				if (c4 == 'R' || c4 == 'r')
				{
					result = StringToken.Quarter;
				}
			}
			break;
		}
		case 'R':
		case 'r':
			switch (buffer[startIndex++])
			{
			case 'E':
			case 'e':
				switch (buffer[startIndex++])
				{
				case 'B':
				case 'b':
				{
					char c = buffer[startIndex++];
					if (c != 'U' && c != 'u')
					{
						break;
					}
					char c2 = buffer[startIndex++];
					if (c2 != 'I' && c2 != 'i')
					{
						break;
					}
					char c3 = buffer[startIndex++];
					if (c3 == 'L' || c3 == 'l')
					{
						char c4 = buffer[startIndex++];
						if (c4 == 'D' || c4 == 'd')
						{
							result = StringToken.Rebuild;
						}
					}
					break;
				}
				case 'C':
				case 'c':
				{
					char c = buffer[startIndex++];
					if (c != 'E' && c != 'e')
					{
						break;
					}
					char c2 = buffer[startIndex++];
					if (c2 != 'I' && c2 != 'i')
					{
						break;
					}
					char c3 = buffer[startIndex++];
					if (c3 == 'V' || c3 == 'v')
					{
						char c4 = buffer[startIndex++];
						if (c4 == 'E' || c4 == 'e')
						{
							result = StringToken.Receive;
						}
					}
					break;
				}
				case 'P':
				case 'p':
				{
					char c = buffer[startIndex++];
					if (c != 'L' && c != 'l')
					{
						break;
					}
					char c2 = buffer[startIndex++];
					if (c2 != 'A' && c2 != 'a')
					{
						break;
					}
					char c3 = buffer[startIndex++];
					if (c3 == 'C' || c3 == 'c')
					{
						char c4 = buffer[startIndex++];
						if (c4 == 'E' || c4 == 'e')
						{
							result = StringToken.Replace;
						}
					}
					break;
				}
				case 'S':
				case 's':
				{
					char c = buffer[startIndex++];
					if (c != 'T' && c != 't')
					{
						break;
					}
					switch (buffer[startIndex++])
					{
					case 'A':
					case 'a':
					{
						char c3 = buffer[startIndex++];
						if (c3 == 'R' || c3 == 'r')
						{
							char c4 = buffer[startIndex++];
							if (c4 == 'T' || c4 == 't')
							{
								result = StringToken.Restart;
							}
						}
						break;
					}
					case 'O':
					case 'o':
					{
						char c3 = buffer[startIndex++];
						if (c3 == 'R' || c3 == 'r')
						{
							char c4 = buffer[startIndex++];
							if (c4 == 'E' || c4 == 'e')
							{
								result = StringToken.Restore;
							}
						}
						break;
					}
					}
					break;
				}
				case 'T':
				case 't':
				{
					char c = buffer[startIndex++];
					if (c != 'U' && c != 'u')
					{
						break;
					}
					char c2 = buffer[startIndex++];
					if (c2 != 'R' && c2 != 'r')
					{
						break;
					}
					char c3 = buffer[startIndex++];
					if (c3 == 'N' || c3 == 'n')
					{
						char c4 = buffer[startIndex++];
						if (c4 == 'S' || c4 == 's')
						{
							result = StringToken.Returns;
						}
					}
					break;
				}
				}
				break;
			case 'O':
			case 'o':
			{
				char c5 = buffer[startIndex++];
				if (c5 != 'W' && c5 != 'w')
				{
					break;
				}
				char c = buffer[startIndex++];
				if (c != 'L' && c != 'l')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'O' && c2 != 'o')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'C' || c3 == 'c')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'K' || c4 == 'k')
					{
						result = StringToken.RowLock;
					}
				}
				break;
			}
			}
			break;
		case 'S':
		case 's':
			switch (buffer[startIndex++])
			{
			case 'E':
			case 'e':
				switch (buffer[startIndex++])
				{
				case 'R':
				case 'r':
				{
					char c = buffer[startIndex++];
					if (c != 'V' && c != 'v')
					{
						break;
					}
					char c2 = buffer[startIndex++];
					if (c2 != 'I' && c2 != 'i')
					{
						break;
					}
					char c3 = buffer[startIndex++];
					if (c3 == 'C' || c3 == 'c')
					{
						char c4 = buffer[startIndex++];
						if (c4 == 'E' || c4 == 'e')
						{
							result = StringToken.Service;
						}
					}
					break;
				}
				case 'S':
				case 's':
				{
					char c = buffer[startIndex++];
					if (c != 'S' && c != 's')
					{
						break;
					}
					char c2 = buffer[startIndex++];
					if (c2 != 'I' && c2 != 'i')
					{
						break;
					}
					char c3 = buffer[startIndex++];
					if (c3 == 'O' || c3 == 'o')
					{
						char c4 = buffer[startIndex++];
						if (c4 == 'N' || c4 == 'n')
						{
							result = StringToken.Session;
						}
					}
					break;
				}
				case 'T':
				case 't':
				{
					char c = buffer[startIndex++];
					if (c != 'U' && c != 'u')
					{
						break;
					}
					char c2 = buffer[startIndex++];
					if (c2 != 'S' && c2 != 's')
					{
						break;
					}
					char c3 = buffer[startIndex++];
					if (c3 == 'E' || c3 == 'e')
					{
						char c4 = buffer[startIndex++];
						if (c4 == 'R' || c4 == 'r')
						{
							result = StringToken.SetUser;
						}
					}
					break;
				}
				}
				break;
			case 'H':
			case 'h':
			{
				char c5 = buffer[startIndex++];
				if (c5 != 'A' && c5 != 'a')
				{
					break;
				}
				char c = buffer[startIndex++];
				if (c != 'R' && c != 'r')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'D' && c2 != 'd')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'E' || c3 == 'e')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'D' || c4 == 'd')
					{
						result = StringToken.Sharded;
					}
				}
				break;
			}
			case 'P':
			case 'p':
			{
				char c5 = buffer[startIndex++];
				if (c5 != 'A' && c5 != 'a')
				{
					break;
				}
				char c = buffer[startIndex++];
				if (c != 'T' && c != 't')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'I' && c2 != 'i')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'A' || c3 == 'a')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'L' || c4 == 'l')
					{
						result = StringToken.Spatial;
					}
				}
				break;
			}
			case 'Y':
			case 'y':
				switch (buffer[startIndex++])
				{
				case 'N':
				case 'n':
				{
					char c = buffer[startIndex++];
					if (c != 'O' && c != 'o')
					{
						break;
					}
					char c2 = buffer[startIndex++];
					if (c2 != 'N' && c2 != 'n')
					{
						break;
					}
					char c3 = buffer[startIndex++];
					if (c3 == 'Y' || c3 == 'y')
					{
						char c4 = buffer[startIndex++];
						if (c4 == 'M' || c4 == 'm')
						{
							result = StringToken.Synonym;
						}
					}
					break;
				}
				case 'S':
				case 's':
				{
					char c = buffer[startIndex++];
					if (c != 'N' && c != 'n')
					{
						break;
					}
					char c2 = buffer[startIndex++];
					if (c2 != 'A' && c2 != 'a')
					{
						break;
					}
					char c3 = buffer[startIndex++];
					if (c3 == 'M' || c3 == 'm')
					{
						char c4 = buffer[startIndex++];
						if (c4 == 'E' || c4 == 'e')
						{
							result = StringToken.SysName;
						}
					}
					break;
				}
				}
				break;
			}
			break;
		case 'T':
		case 't':
			switch (buffer[startIndex++])
			{
			case 'A':
			case 'a':
			{
				char c5 = buffer[startIndex++];
				if (c5 != 'B' && c5 != 'b')
				{
					break;
				}
				char c = buffer[startIndex++];
				if (c != 'L' && c != 'l')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'O' && c2 != 'o')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'C' || c3 == 'c')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'K' || c4 == 'k')
					{
						result = StringToken.TabLock;
					}
				}
				break;
			}
			case 'I':
			case 'i':
			{
				char c5 = buffer[startIndex++];
				if (c5 != 'N' && c5 != 'n')
				{
					break;
				}
				char c = buffer[startIndex++];
				if (c != 'Y' && c != 'y')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'I' && c2 != 'i')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'N' || c3 == 'n')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'T' || c4 == 't')
					{
						result = StringToken.TinyInt;
					}
				}
				break;
			}
			case 'R':
			case 'r':
			{
				char c5 = buffer[startIndex++];
				if (c5 != 'I' && c5 != 'i')
				{
					break;
				}
				char c = buffer[startIndex++];
				if (c != 'G' && c != 'g')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'G' && c2 != 'g')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'E' || c3 == 'e')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'R' || c4 == 'r')
					{
						result = StringToken.Trigger;
					}
				}
				break;
			}
			case 'S':
			case 's':
			{
				char c5 = buffer[startIndex++];
				if (c5 != 'E' && c5 != 'e')
				{
					break;
				}
				char c = buffer[startIndex++];
				if (c != 'Q' && c != 'q')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'U' && c2 != 'u')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'A' || c3 == 'a')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'L' || c4 == 'l')
					{
						result = StringToken.Tsequal;
					}
				}
				break;
			}
			}
			break;
		case 'U':
		case 'u':
			switch (buffer[startIndex++])
			{
			case 'N':
			case 'n':
			{
				char c5 = buffer[startIndex++];
				if (c5 != 'P' && c5 != 'p')
				{
					break;
				}
				char c = buffer[startIndex++];
				if (c != 'I' && c != 'i')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'V' && c2 != 'v')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'O' || c3 == 'o')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'T' || c4 == 't')
					{
						result = StringToken.Unpivot;
					}
				}
				break;
			}
			case 'P':
			case 'p':
			{
				char c5 = buffer[startIndex++];
				if (c5 != 'D' && c5 != 'd')
				{
					break;
				}
				char c = buffer[startIndex++];
				if (c != 'L' && c != 'l')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'O' && c2 != 'o')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'C' || c3 == 'c')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'K' || c4 == 'k')
					{
						result = StringToken.UpdLock;
					}
				}
				break;
			}
			case 'S':
			case 's':
			{
				char c5 = buffer[startIndex++];
				if (c5 != 'E' && c5 != 'e')
				{
					break;
				}
				char c = buffer[startIndex++];
				if ((c != 'R' && c != 'r') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 == 'D' || c2 == 'd')
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'B' || c3 == 'b')
					{
						result = StringToken.UserDb;
					}
				}
				break;
			}
			}
			break;
		case 'V':
		case 'v':
			switch (buffer[startIndex++])
			{
			case 'A':
			case 'a':
			{
				char c5 = buffer[startIndex++];
				if (c5 != 'R' && c5 != 'r')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case 'C':
				case 'c':
				{
					char c2 = buffer[startIndex++];
					if (c2 != 'H' && c2 != 'h')
					{
						break;
					}
					char c3 = buffer[startIndex++];
					if (c3 == 'A' || c3 == 'a')
					{
						char c4 = buffer[startIndex++];
						if (c4 == 'R' || c4 == 'r')
						{
							result = StringToken.VarChar;
						}
					}
					break;
				}
				case 'I':
				case 'i':
				{
					char c2 = buffer[startIndex++];
					if (c2 != 'A' && c2 != 'a')
					{
						break;
					}
					char c3 = buffer[startIndex++];
					if (c3 == 'N' || c3 == 'n')
					{
						char c4 = buffer[startIndex++];
						if (c4 == 'T' || c4 == 't')
						{
							result = StringToken.Variant;
						}
					}
					break;
				}
				case 'Y':
				case 'y':
				{
					char c2 = buffer[startIndex++];
					if (c2 != 'I' && c2 != 'i')
					{
						break;
					}
					char c3 = buffer[startIndex++];
					if (c3 == 'N' || c3 == 'n')
					{
						char c4 = buffer[startIndex++];
						if (c4 == 'G' || c4 == 'g')
						{
							result = StringToken.Varying;
						}
					}
					break;
				}
				}
				break;
			}
			case 'E':
			case 'e':
			{
				char c5 = buffer[startIndex++];
				if (c5 != 'R' && c5 != 'r')
				{
					break;
				}
				char c = buffer[startIndex++];
				if (c != 'S' && c != 's')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'I' && c2 != 'i')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'O' || c3 == 'o')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'N' || c4 == 'n')
					{
						result = StringToken.Version;
					}
				}
				break;
			}
			}
			break;
		case 'W':
		case 'w':
			switch (buffer[startIndex++])
			{
			case 'A':
			case 'a':
			{
				char c5 = buffer[startIndex++];
				if (c5 != 'I' && c5 != 'i')
				{
					break;
				}
				char c = buffer[startIndex++];
				if (c != 'T' && c != 't')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'F' && c2 != 'f')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'O' || c3 == 'o')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'R' || c4 == 'r')
					{
						result = StringToken.WaitFor;
					}
				}
				break;
			}
			case 'E':
			case 'e':
			{
				char c5 = buffer[startIndex++];
				if (c5 != 'E' && c5 != 'e')
				{
					break;
				}
				char c = buffer[startIndex++];
				if (c != 'K' && c != 'k')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'D' && c2 != 'd')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 == 'A' || c3 == 'a')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'Y' || c4 == 'y')
					{
						result = StringToken.Weekday;
					}
				}
				break;
			}
			case 'I':
			case 'i':
				switch (buffer[startIndex++])
				{
				case 'N':
				case 'n':
				{
					char c = buffer[startIndex++];
					if (c != 'D' && c != 'd')
					{
						break;
					}
					char c2 = buffer[startIndex++];
					if (c2 != 'O' && c2 != 'o')
					{
						break;
					}
					char c3 = buffer[startIndex++];
					if (c3 == 'W' || c3 == 'w')
					{
						char c4 = buffer[startIndex++];
						if (c4 == 'S' || c4 == 's')
						{
							result = StringToken.Windows;
						}
					}
					break;
				}
				case 'T':
				case 't':
				{
					char c = buffer[startIndex++];
					if (c != 'H' && c != 'h')
					{
						break;
					}
					char c2 = buffer[startIndex++];
					if (c2 != 'O' && c2 != 'o')
					{
						break;
					}
					char c3 = buffer[startIndex++];
					if (c3 == 'U' || c3 == 'u')
					{
						char c4 = buffer[startIndex++];
						if (c4 == 'T' || c4 == 't')
						{
							result = StringToken.Without;
						}
					}
					break;
				}
				}
				break;
			}
			break;
		}
		return result;
	}

	private static StringToken Find_Len8(string buffer, int startIndex)
	{
		StringToken result = StringToken.None;
		switch (buffer[startIndex++])
		{
		case 'A':
		case 'a':
		{
			char c = buffer[startIndex++];
			if (c != 'S' && c != 's')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'S' && c2 != 's')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'E' && c3 != 'e')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'M' && c4 != 'm')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'B' && c5 != 'b')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 == 'L' || c6 == 'l')
			{
				char c7 = buffer[startIndex++];
				if (c7 == 'Y' || c7 == 'y')
				{
					result = StringToken.Assembly;
				}
			}
			break;
		}
		case 'C':
		case 'c':
			switch (buffer[startIndex++])
			{
			case 'O':
			case 'o':
				switch (buffer[startIndex++])
				{
				case 'A':
				case 'a':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'L' && c3 != 'l')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'E' && c4 != 'e')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'S' && c5 != 's')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 == 'C' || c6 == 'c')
					{
						char c7 = buffer[startIndex++];
						if (c7 == 'E' || c7 == 'e')
						{
							result = StringToken.Coalesce;
						}
					}
					break;
				}
				case 'N':
				case 'n':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'T' && c3 != 't')
					{
						break;
					}
					switch (buffer[startIndex++])
					{
					case 'A':
					case 'a':
					{
						char c5 = buffer[startIndex++];
						if (c5 != 'I' && c5 != 'i')
						{
							break;
						}
						char c6 = buffer[startIndex++];
						if (c6 == 'N' || c6 == 'n')
						{
							char c7 = buffer[startIndex++];
							if (c7 == 'S' || c7 == 's')
							{
								result = StringToken.Contains;
							}
						}
						break;
					}
					case 'I':
					case 'i':
					{
						char c5 = buffer[startIndex++];
						if (c5 != 'N' && c5 != 'n')
						{
							break;
						}
						char c6 = buffer[startIndex++];
						if (c6 == 'U' || c6 == 'u')
						{
							char c7 = buffer[startIndex++];
							if (c7 == 'E' || c7 == 'e')
							{
								result = StringToken.Continue;
							}
						}
						break;
					}
					case 'R':
					case 'r':
					{
						char c5 = buffer[startIndex++];
						if (c5 != 'A' && c5 != 'a')
						{
							break;
						}
						char c6 = buffer[startIndex++];
						if (c6 == 'C' || c6 == 'c')
						{
							char c7 = buffer[startIndex++];
							if (c7 == 'T' || c7 == 't')
							{
								result = StringToken.Contract;
							}
						}
						break;
					}
					}
					break;
				}
				}
				break;
			case 'P':
			case 'p':
			{
				char c2 = buffer[startIndex++];
				if ((c2 != 'U' && c2 != 'u') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'B' && c3 != 'b')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'U' && c4 != 'u')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 == 'S' || c5 == 's')
				{
					char c6 = buffer[startIndex++];
					if (c6 == 'Y' || c6 == 'y')
					{
						result = StringToken.Cpu_busy;
					}
				}
				break;
			}
			}
			break;
		case 'D':
		case 'd':
			switch (buffer[startIndex++])
			{
			case 'A':
			case 'a':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'T' && c2 != 't')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case 'A':
				case 'a':
				{
					char c4 = buffer[startIndex++];
					if (c4 != 'B' && c4 != 'b')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'A' && c5 != 'a')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 == 'S' || c6 == 's')
					{
						char c7 = buffer[startIndex++];
						if (c7 == 'E' || c7 == 'e')
						{
							result = StringToken.Database;
						}
					}
					break;
				}
				case 'E':
				case 'e':
				{
					char c4 = buffer[startIndex++];
					if (c4 != 'T' && c4 != 't')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'I' && c5 != 'i')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 == 'M' || c6 == 'm')
					{
						char c7 = buffer[startIndex++];
						if (c7 == 'E' || c7 == 'e')
						{
							result = StringToken.DateTime;
						}
					}
					break;
				}
				}
				break;
			}
			case 'I':
			case 'i':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'S' && c2 != 's')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'T' && c3 != 't')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'I' && c4 != 'i')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'N' && c5 != 'n')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 == 'C' || c6 == 'c')
				{
					char c7 = buffer[startIndex++];
					if (c7 == 'T' || c7 == 't')
					{
						result = StringToken.Distinct;
					}
				}
				break;
			}
			}
			break;
		case 'E':
		case 'e':
			switch (buffer[startIndex++])
			{
			case 'N':
			case 'n':
				switch (buffer[startIndex++])
				{
				case 'C':
				case 'c':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'O' && c3 != 'o')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'D' && c4 != 'd')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'I' && c5 != 'i')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 == 'N' || c6 == 'n')
					{
						char c7 = buffer[startIndex++];
						if (c7 == 'G' || c7 == 'g')
						{
							result = StringToken.Encoding;
						}
					}
					break;
				}
				case 'D':
				case 'd':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'P' && c3 != 'p')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'O' && c4 != 'o')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'I' && c5 != 'i')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 == 'N' || c6 == 'n')
					{
						char c7 = buffer[startIndex++];
						if (c7 == 'T' || c7 == 't')
						{
							result = StringToken.EndPoint;
						}
					}
					break;
				}
				}
				break;
			case 'X':
			case 'x':
				switch (buffer[startIndex++])
				{
				case 'P':
				case 'p':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'L' && c3 != 'l')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'I' && c4 != 'i')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'C' && c5 != 'c')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 == 'I' || c6 == 'i')
					{
						char c7 = buffer[startIndex++];
						if (c7 == 'T' || c7 == 't')
						{
							result = StringToken.Explicit;
						}
					}
					break;
				}
				case 'T':
				case 't':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'E' && c3 != 'e')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'R' && c4 != 'r')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'N' && c5 != 'n')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 == 'A' || c6 == 'a')
					{
						char c7 = buffer[startIndex++];
						if (c7 == 'L' || c7 == 'l')
						{
							result = StringToken.External;
						}
					}
					break;
				}
				}
				break;
			}
			break;
		case 'F':
		case 'f':
			switch (buffer[startIndex++])
			{
			case 'A':
			case 'a':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'I' && c2 != 'i')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'L' && c3 != 'l')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'O' && c4 != 'o')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'V' && c5 != 'v')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 == 'E' || c6 == 'e')
				{
					char c7 = buffer[startIndex++];
					if (c7 == 'R' || c7 == 'r')
					{
						result = StringToken.FailOver;
					}
				}
				break;
			}
			case 'R':
			case 'r':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'E' && c2 != 'e')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'E' && c3 != 'e')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'T' && c4 != 't')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'E' && c5 != 'e')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 == 'X' || c6 == 'x')
				{
					char c7 = buffer[startIndex++];
					if (c7 == 'T' || c7 == 't')
					{
						result = StringToken.FreeText;
					}
				}
				break;
			}
			case 'U':
			case 'u':
				switch (buffer[startIndex++])
				{
				case 'L':
				case 'l':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'L' && c3 != 'l')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'T' && c4 != 't')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'E' && c5 != 'e')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 == 'X' || c6 == 'x')
					{
						char c7 = buffer[startIndex++];
						if (c7 == 'T' || c7 == 't')
						{
							result = StringToken.FullText;
						}
					}
					break;
				}
				case 'N':
				case 'n':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'C' && c3 != 'c')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'T' && c4 != 't')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'I' && c5 != 'i')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 == 'O' || c6 == 'o')
					{
						char c7 = buffer[startIndex++];
						if (c7 == 'N' || c7 == 'n')
						{
							result = StringToken.Function;
						}
					}
					break;
				}
				}
				break;
			}
			break;
		case 'G':
		case 'g':
			switch (buffer[startIndex++])
			{
			case 'E':
			case 'e':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'O' && c2 != 'o')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'M' && c3 != 'm')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'E' && c4 != 'e')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'T' && c5 != 't')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 == 'R' || c6 == 'r')
				{
					char c7 = buffer[startIndex++];
					if (c7 == 'Y' || c7 == 'y')
					{
						result = StringToken.Geometry;
					}
				}
				break;
			}
			case 'O':
			case 'o':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'V' && c2 != 'v')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'E' && c3 != 'e')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'R' && c4 != 'r')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'N' && c5 != 'n')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 == 'O' || c6 == 'o')
				{
					char c7 = buffer[startIndex++];
					if (c7 == 'R' || c7 == 'r')
					{
						result = StringToken.Governor;
					}
				}
				break;
			}
			case 'R':
			case 'r':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'O' && c2 != 'o')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'U' && c3 != 'u')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'P' && c4 != 'p')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'I' && c5 != 'i')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 == 'N' || c6 == 'n')
				{
					char c7 = buffer[startIndex++];
					if (c7 == 'G' || c7 == 'g')
					{
						result = StringToken.Grouping;
					}
				}
				break;
			}
			}
			break;
		case 'H':
		case 'h':
		{
			char c = buffer[startIndex++];
			if (c != 'O' && c != 'o')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'L' && c2 != 'l')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'D' && c3 != 'd')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'L' && c4 != 'l')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'O' && c5 != 'o')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 == 'C' || c6 == 'c')
			{
				char c7 = buffer[startIndex++];
				if (c7 == 'K' || c7 == 'k')
				{
					result = StringToken.HoldLock;
				}
			}
			break;
		}
		case 'I':
		case 'i':
			switch (buffer[startIndex++])
			{
			case 'D':
			case 'd':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'E' && c2 != 'e')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'N' && c3 != 'n')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'T' && c4 != 't')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'I' && c5 != 'i')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 == 'T' || c6 == 't')
				{
					char c7 = buffer[startIndex++];
					if (c7 == 'Y' || c7 == 'y')
					{
						result = StringToken.Identity;
					}
				}
				break;
			}
			case 'N':
			case 'n':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'F' && c2 != 'f')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'I' && c3 != 'i')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'N' && c4 != 'n')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'I' && c5 != 'i')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 == 'T' || c6 == 't')
				{
					char c7 = buffer[startIndex++];
					if (c7 == 'E' || c7 == 'e')
					{
						result = StringToken.Infinite;
					}
				}
				break;
			}
			case 'S':
			case 's':
			{
				char c2 = buffer[startIndex++];
				if ((c2 != 'O' && c2 != 'o') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'W' && c3 != 'w')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'E' && c4 != 'e')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 == 'E' || c5 == 'e')
				{
					char c6 = buffer[startIndex++];
					if (c6 == 'K' || c6 == 'k')
					{
						result = StringToken.IsoWeek;
					}
				}
				break;
			}
			}
			break;
		case 'K':
		case 'k':
		{
			char c = buffer[startIndex++];
			if (c != 'E' && c != 'e')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if ((c2 != 'Y' && c2 != 'y') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'P' && c3 != 'p')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'A' && c4 != 'a')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 == 'T' || c5 == 't')
			{
				char c6 = buffer[startIndex++];
				if (c6 == 'H' || c6 == 'h')
				{
					result = StringToken.KeyPath;
				}
			}
			break;
		}
		case 'L':
		case 'l':
			switch (buffer[startIndex++])
			{
			case 'A':
			case 'a':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'N' && c2 != 'n')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'G' && c3 != 'g')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'U' && c4 != 'u')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'A' && c5 != 'a')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 == 'G' || c6 == 'g')
				{
					char c7 = buffer[startIndex++];
					if (c7 == 'E' || c7 == 'e')
					{
						result = StringToken.Language;
					}
				}
				break;
			}
			case 'O':
			case 'o':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'C' && c2 != 'c')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'A' && c3 != 'a')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'T' && c4 != 't')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'I' && c5 != 'i')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 == 'O' || c6 == 'o')
				{
					char c7 = buffer[startIndex++];
					if (c7 == 'N' || c7 == 'n')
					{
						result = StringToken.Location;
					}
				}
				break;
			}
			}
			break;
		case 'M':
		case 'm':
			switch (buffer[startIndex++])
			{
			case 'A':
			case 'a':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'X' && c2 != 'x')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case '_':
				{
					char c4 = buffer[startIndex++];
					if (c4 != 'S' && c4 != 's')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'I' && c5 != 'i')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 == 'Z' || c6 == 'z')
					{
						char c7 = buffer[startIndex++];
						if (c7 == 'E' || c7 == 'e')
						{
							result = StringToken.MaxSize;
						}
					}
					break;
				}
				case 'V':
				case 'v':
				{
					char c4 = buffer[startIndex++];
					if (c4 != 'A' && c4 != 'a')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'L' && c5 != 'l')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 == 'U' || c6 == 'u')
					{
						char c7 = buffer[startIndex++];
						if (c7 == 'E' || c7 == 'e')
						{
							result = StringToken.MaxValue;
						}
					}
					break;
				}
				}
				break;
			}
			case 'I':
			case 'i':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'N' && c2 != 'n')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'V' && c3 != 'v')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'A' && c4 != 'a')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'L' && c5 != 'l')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 == 'U' || c6 == 'u')
				{
					char c7 = buffer[startIndex++];
					if (c7 == 'E' || c7 == 'e')
					{
						result = StringToken.MinValue;
					}
				}
				break;
			}
			}
			break;
		case 'N':
		case 'n':
			switch (buffer[startIndex++])
			{
			case 'A':
			case 'a':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'T' && c2 != 't')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'I' && c3 != 'i')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'O' && c4 != 'o')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'N' && c5 != 'n')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 == 'A' || c6 == 'a')
				{
					char c7 = buffer[startIndex++];
					if (c7 == 'L' || c7 == 'l')
					{
						result = StringToken.National;
					}
				}
				break;
			}
			case 'O':
			case 'o':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'E' && c2 != 'e')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'X' && c3 != 'x')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'P' && c4 != 'p')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'A' && c5 != 'a')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 == 'N' || c6 == 'n')
				{
					char c7 = buffer[startIndex++];
					if (c7 == 'D' || c7 == 'd')
					{
						result = StringToken.NoExpand;
					}
				}
				break;
			}
			case 'U':
			case 'u':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'M' && c2 != 'm')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'A' && c3 != 'a')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'N' && c4 != 'n')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'O' && c5 != 'o')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 == 'D' || c6 == 'd')
				{
					char c7 = buffer[startIndex++];
					if (c7 == 'E' || c7 == 'e')
					{
						result = StringToken.NumaNode;
					}
				}
				break;
			}
			case 'V':
			case 'v':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'A' && c2 != 'a')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'R' && c3 != 'r')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'C' && c4 != 'c')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'H' && c5 != 'h')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 == 'A' || c6 == 'a')
				{
					char c7 = buffer[startIndex++];
					if (c7 == 'R' || c7 == 'r')
					{
						result = StringToken.NVarChar;
					}
				}
				break;
			}
			}
			break;
		case 'O':
		case 'o':
		{
			char c = buffer[startIndex++];
			if (c != 'P' && c != 'p')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'E' && c2 != 'e')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'N' && c3 != 'n')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'J' && c4 != 'j')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'S' && c5 != 's')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 == 'O' || c6 == 'o')
			{
				char c7 = buffer[startIndex++];
				if (c7 == 'N' || c7 == 'n')
				{
					result = StringToken.Openjson;
				}
			}
			break;
		}
		case 'P':
		case 'p':
			switch (buffer[startIndex++])
			{
			case 'A':
			case 'a':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'S' && c2 != 's')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'S' && c3 != 's')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'W' && c4 != 'w')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'O' && c5 != 'o')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 == 'R' || c6 == 'r')
				{
					char c7 = buffer[startIndex++];
					if (c7 == 'D' || c7 == 'd')
					{
						result = StringToken.Password;
					}
				}
				break;
			}
			case 'L':
			case 'l':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'A' && c2 != 'a')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'T' && c3 != 't')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'F' && c4 != 'f')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'O' && c5 != 'o')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 == 'R' || c6 == 'r')
				{
					char c7 = buffer[startIndex++];
					if (c7 == 'M' || c7 == 'm')
					{
						result = StringToken.Platform;
					}
				}
				break;
			}
			case 'R':
			case 'r':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'O' && c2 != 'o')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case 'P':
				case 'p':
				{
					char c4 = buffer[startIndex++];
					if (c4 != 'E' && c4 != 'e')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'R' && c5 != 'r')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 == 'T' || c6 == 't')
					{
						char c7 = buffer[startIndex++];
						if (c7 == 'Y' || c7 == 'y')
						{
							result = StringToken.Property;
						}
					}
					break;
				}
				case 'V':
				case 'v':
				{
					char c4 = buffer[startIndex++];
					if (c4 != 'I' && c4 != 'i')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'D' && c5 != 'd')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 == 'E' || c6 == 'e')
					{
						char c7 = buffer[startIndex++];
						if (c7 == 'R' || c7 == 'r')
						{
							result = StringToken.Provider;
						}
					}
					break;
				}
				}
				break;
			}
			case 'U':
			case 'u':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'S' && c2 != 's')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'H' && c3 != 'h')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'D' && c4 != 'd')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'O' && c5 != 'o')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 == 'W' || c6 == 'w')
				{
					char c7 = buffer[startIndex++];
					if (c7 == 'N' || c7 == 'n')
					{
						result = StringToken.PushDown;
					}
				}
				break;
			}
			}
			break;
		case 'R':
		case 'r':
			switch (buffer[startIndex++])
			{
			case 'E':
			case 'e':
				switch (buffer[startIndex++])
				{
				case 'A':
				case 'a':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'D' && c3 != 'd')
					{
						break;
					}
					switch (buffer[startIndex++])
					{
					case 'O':
					case 'o':
					{
						char c5 = buffer[startIndex++];
						if (c5 != 'N' && c5 != 'n')
						{
							break;
						}
						char c6 = buffer[startIndex++];
						if (c6 == 'L' || c6 == 'l')
						{
							char c7 = buffer[startIndex++];
							if (c7 == 'Y' || c7 == 'y')
							{
								result = StringToken.ReadOnly;
							}
						}
						break;
					}
					case 'P':
					case 'p':
					{
						char c5 = buffer[startIndex++];
						if (c5 != 'A' && c5 != 'a')
						{
							break;
						}
						char c6 = buffer[startIndex++];
						if (c6 == 'S' || c6 == 's')
						{
							char c7 = buffer[startIndex++];
							if (c7 == 'T' || c7 == 't')
							{
								result = StringToken.ReadPast;
							}
						}
						break;
					}
					case 'T':
					case 't':
					{
						char c5 = buffer[startIndex++];
						if (c5 != 'E' && c5 != 'e')
						{
							break;
						}
						char c6 = buffer[startIndex++];
						if (c6 == 'X' || c6 == 'x')
						{
							char c7 = buffer[startIndex++];
							if (c7 == 'T' || c7 == 't')
							{
								result = StringToken.ReadText;
							}
						}
						break;
					}
					}
					break;
				}
				case 'S':
				case 's':
					switch (buffer[startIndex++])
					{
					case 'O':
					case 'o':
					{
						char c4 = buffer[startIndex++];
						if (c4 != 'U' && c4 != 'u')
						{
							break;
						}
						char c5 = buffer[startIndex++];
						if (c5 != 'R' && c5 != 'r')
						{
							break;
						}
						char c6 = buffer[startIndex++];
						if (c6 == 'C' || c6 == 'c')
						{
							char c7 = buffer[startIndex++];
							if (c7 == 'E' || c7 == 'e')
							{
								result = StringToken.Resource;
							}
						}
						break;
					}
					case 'T':
					case 't':
					{
						char c4 = buffer[startIndex++];
						if (c4 != 'R' && c4 != 'r')
						{
							break;
						}
						char c5 = buffer[startIndex++];
						if (c5 != 'I' && c5 != 'i')
						{
							break;
						}
						char c6 = buffer[startIndex++];
						if (c6 == 'C' || c6 == 'c')
						{
							char c7 = buffer[startIndex++];
							if (c7 == 'T' || c7 == 't')
							{
								result = StringToken.Restrict;
							}
						}
						break;
					}
					}
					break;
				}
				break;
			case 'O':
			case 'o':
				switch (buffer[startIndex++])
				{
				case 'L':
				case 'l':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'L' && c3 != 'l')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'B' && c4 != 'b')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'A' && c5 != 'a')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 == 'C' || c6 == 'c')
					{
						char c7 = buffer[startIndex++];
						if (c7 == 'K' || c7 == 'k')
						{
							result = StringToken.Rollback;
						}
					}
					break;
				}
				case 'W':
				case 'w':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'C' && c3 != 'c')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'O' && c4 != 'o')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'U' && c5 != 'u')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 == 'N' || c6 == 'n')
					{
						char c7 = buffer[startIndex++];
						if (c7 == 'T' || c7 == 't')
						{
							result = StringToken.RowCount;
						}
					}
					break;
				}
				}
				break;
			}
			break;
		case 'S':
		case 's':
			switch (buffer[startIndex++])
			{
			case 'E':
			case 'e':
				switch (buffer[startIndex++])
				{
				case 'C':
				case 'c':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'U' && c3 != 'u')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'R' && c4 != 'r')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'I' && c5 != 'i')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 == 'T' || c6 == 't')
					{
						char c7 = buffer[startIndex++];
						if (c7 == 'Y' || c7 == 'y')
						{
							result = StringToken.Security;
						}
					}
					break;
				}
				case 'Q':
				case 'q':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'U' && c3 != 'u')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'E' && c4 != 'e')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'N' && c5 != 'n')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 == 'C' || c6 == 'c')
					{
						char c7 = buffer[startIndex++];
						if (c7 == 'E' || c7 == 'e')
						{
							result = StringToken.Sequence;
						}
					}
					break;
				}
				}
				break;
			case 'H':
			case 'h':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'U' && c2 != 'u')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'T' && c3 != 't')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'D' && c4 != 'd')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'O' && c5 != 'o')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 == 'W' || c6 == 'w')
				{
					char c7 = buffer[startIndex++];
					if (c7 == 'N' || c7 == 'n')
					{
						result = StringToken.Shutdown;
					}
				}
				break;
			}
			case 'M':
			case 'm':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'A' && c2 != 'a')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'L' && c3 != 'l')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'L' && c4 != 'l')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'I' && c5 != 'i')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 == 'N' || c6 == 'n')
				{
					char c7 = buffer[startIndex++];
					if (c7 == 'T' || c7 == 't')
					{
						result = StringToken.SmallInt;
					}
				}
				break;
			}
			case 'N':
			case 'n':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'A' && c2 != 'a')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'P' && c3 != 'p')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'S' && c4 != 's')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'H' && c5 != 'h')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 == 'O' || c6 == 'o')
				{
					char c7 = buffer[startIndex++];
					if (c7 == 'T' || c7 == 't')
					{
						result = StringToken.Snapshot;
					}
				}
				break;
			}
			case 'O':
			case 'o':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'F' && c2 != 'f')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'T' && c3 != 't')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'N' && c4 != 'n')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'U' && c5 != 'u')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 == 'M' || c6 == 'm')
				{
					char c7 = buffer[startIndex++];
					if (c7 == 'A' || c7 == 'a')
					{
						result = StringToken.SoftNuma;
					}
				}
				break;
			}
			case 'T':
			case 't':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'O' && c2 != 'o')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'P' && c3 != 'p')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'L' && c4 != 'l')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'I' && c5 != 'i')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 == 'S' || c6 == 's')
				{
					char c7 = buffer[startIndex++];
					if (c7 == 'T' || c7 == 't')
					{
						result = StringToken.StopList;
					}
				}
				break;
			}
			}
			break;
		case 'T':
		case 't':
			switch (buffer[startIndex++])
			{
			case 'A':
			case 'a':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'B' && c2 != 'b')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'L' && c3 != 'l')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'O' && c4 != 'o')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'C' && c5 != 'c')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 == 'K' || c6 == 'k')
				{
					char c7 = buffer[startIndex++];
					if (c7 == 'X' || c7 == 'x')
					{
						result = StringToken.TabLockX;
					}
				}
				break;
			}
			case 'E':
			case 'e':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'X' && c2 != 'x')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'T' && c3 != 't')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'S' && c4 != 's')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'I' && c5 != 'i')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 == 'Z' || c6 == 'z')
				{
					char c7 = buffer[startIndex++];
					if (c7 == 'E' || c7 == 'e')
					{
						result = StringToken.TextSize;
					}
				}
				break;
			}
			case 'R':
			case 'r':
				switch (buffer[startIndex++])
				{
				case 'U':
				case 'u':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'N' && c3 != 'n')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'C' && c4 != 'c')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'A' && c5 != 'a')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 == 'T' || c6 == 't')
					{
						char c7 = buffer[startIndex++];
						if (c7 == 'E' || c7 == 'e')
						{
							result = StringToken.Truncate;
						}
					}
					break;
				}
				case 'Y':
				case 'y':
				{
					if (buffer[startIndex++] != '_')
					{
						break;
					}
					char c3 = buffer[startIndex++];
					if (c3 != 'C' && c3 != 'c')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'A' && c4 != 'a')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 == 'S' || c5 == 's')
					{
						char c6 = buffer[startIndex++];
						if (c6 == 'T' || c6 == 't')
						{
							result = StringToken.TryCast;
						}
					}
					break;
				}
				}
				break;
			case 'Z':
			case 'z':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'O' && c2 != 'o')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'F' && c3 != 'f')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'F' && c4 != 'f')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'S' && c5 != 's')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 == 'E' || c6 == 'e')
				{
					char c7 = buffer[startIndex++];
					if (c7 == 'T' || c7 == 't')
					{
						result = StringToken.TzOffset;
					}
				}
				break;
			}
			}
			break;
		case 'W':
		case 'w':
		{
			char c = buffer[startIndex++];
			if (c != 'O' && c != 'o')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'R' && c2 != 'r')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'K' && c3 != 'k')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'L' && c4 != 'l')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'O' && c5 != 'o')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 == 'A' || c6 == 'a')
			{
				char c7 = buffer[startIndex++];
				if (c7 == 'D' || c7 == 'd')
				{
					result = StringToken.Workload;
				}
			}
			break;
		}
		}
		return result;
	}

	private static StringToken Find_Len9(string buffer, int startIndex)
	{
		StringToken result = StringToken.None;
		switch (buffer[startIndex++])
		{
		case 'A':
		case 'a':
			switch (buffer[startIndex++])
			{
			case 'G':
			case 'g':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'G' && c2 != 'g')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'R' && c3 != 'r')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'E' && c4 != 'e')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'G' && c5 != 'g')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'A' && c6 != 'a')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 == 'T' || c7 == 't')
				{
					char c8 = buffer[startIndex++];
					if (c8 == 'E' || c8 == 'e')
					{
						result = StringToken.Aggregate;
					}
				}
				break;
			}
			case 'L':
			case 'l':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'G' && c2 != 'g')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'O' && c3 != 'o')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'R' && c4 != 'r')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'I' && c5 != 'i')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'T' && c6 != 't')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 == 'H' || c7 == 'h')
				{
					char c8 = buffer[startIndex++];
					if (c8 == 'M' || c8 == 'm')
					{
						result = StringToken.Algorithm;
					}
				}
				break;
			}
			}
			break;
		case 'B':
		case 'b':
		{
			char c = buffer[startIndex++];
			if (c != 'I' && c != 'i')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'N' && c2 != 'n')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if ((c3 != 'D' && c3 != 'd') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'R' && c4 != 'r')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'U' && c5 != 'u')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 == 'L' || c6 == 'l')
			{
				char c7 = buffer[startIndex++];
				if (c7 == 'E' || c7 == 'e')
				{
					result = StringToken.BindRule;
				}
			}
			break;
		}
		case 'C':
		case 'c':
			switch (buffer[startIndex++])
			{
			case 'H':
			case 'h':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'A' && c2 != 'a')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'R' && c3 != 'r')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'A' && c4 != 'a')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'C' && c5 != 'c')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'T' && c6 != 't')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 == 'E' || c7 == 'e')
				{
					char c8 = buffer[startIndex++];
					if (c8 == 'R' || c8 == 'r')
					{
						result = StringToken.Character;
					}
				}
				break;
			}
			case 'L':
			case 'l':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'U' && c2 != 'u')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'S' && c3 != 's')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'T' && c4 != 't')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'E' && c5 != 'e')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'R' && c6 != 'r')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 == 'E' || c7 == 'e')
				{
					char c8 = buffer[startIndex++];
					if (c8 == 'D' || c8 == 'd')
					{
						result = StringToken.Clustered;
					}
				}
				break;
			}
			}
			break;
		case 'D':
		case 'd':
			switch (buffer[startIndex++])
			{
			case 'A':
			case 'a':
				switch (buffer[startIndex++])
				{
				case 'T':
				case 't':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'E' && c3 != 'e')
					{
						break;
					}
					switch (buffer[startIndex++])
					{
					case 'F':
					case 'f':
					{
						char c5 = buffer[startIndex++];
						if (c5 != 'I' && c5 != 'i')
						{
							break;
						}
						char c6 = buffer[startIndex++];
						if (c6 != 'R' && c6 != 'r')
						{
							break;
						}
						char c7 = buffer[startIndex++];
						if (c7 == 'S' || c7 == 's')
						{
							char c8 = buffer[startIndex++];
							if (c8 == 'T' || c8 == 't')
							{
								result = StringToken.Datefirst;
							}
						}
						break;
					}
					case 'T':
					case 't':
					{
						char c5 = buffer[startIndex++];
						if (c5 != 'I' && c5 != 'i')
						{
							break;
						}
						char c6 = buffer[startIndex++];
						if (c6 == 'M' || c6 == 'm')
						{
							char c7 = buffer[startIndex++];
							if ((c7 == 'E' || c7 == 'e') && buffer[startIndex++] == '2')
							{
								result = StringToken.DateTime2;
							}
						}
						break;
					}
					}
					break;
				}
				case 'Y':
				case 'y':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'O' && c3 != 'o')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'F' && c4 != 'f')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'Y' && c5 != 'y')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'E' && c6 != 'e')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 == 'A' || c7 == 'a')
					{
						char c8 = buffer[startIndex++];
						if (c8 == 'R' || c8 == 'r')
						{
							result = StringToken.DayOfYear;
						}
					}
					break;
				}
				}
				break;
			case 'R':
			case 'r':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'O' && c2 != 'o')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if ((c3 != 'P' && c3 != 'p') || buffer[startIndex++] != '_')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case 'R':
				case 'r':
					switch (buffer[startIndex++])
					{
					case 'O':
					case 'o':
					{
						char c6 = buffer[startIndex++];
						if (c6 == 'L' || c6 == 'l')
						{
							char c7 = buffer[startIndex++];
							if (c7 == 'E' || c7 == 'e')
							{
								result = StringToken.DropRole;
							}
						}
						break;
					}
					case 'U':
					case 'u':
					{
						char c6 = buffer[startIndex++];
						if (c6 == 'L' || c6 == 'l')
						{
							char c7 = buffer[startIndex++];
							if (c7 == 'E' || c7 == 'e')
							{
								result = StringToken.DropRule;
							}
						}
						break;
					}
					}
					break;
				case 'T':
				case 't':
				{
					char c5 = buffer[startIndex++];
					if (c5 != 'Y' && c5 != 'y')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 == 'P' || c6 == 'p')
					{
						char c7 = buffer[startIndex++];
						if (c7 == 'E' || c7 == 'e')
						{
							result = StringToken.DropType;
						}
					}
					break;
				}
				case 'U':
				case 'u':
				{
					char c5 = buffer[startIndex++];
					if (c5 != 'S' && c5 != 's')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 == 'E' || c6 == 'e')
					{
						char c7 = buffer[startIndex++];
						if (c7 == 'R' || c7 == 'r')
						{
							result = StringToken.DropUser;
						}
					}
					break;
				}
				case 'V':
				case 'v':
				{
					char c5 = buffer[startIndex++];
					if (c5 != 'I' && c5 != 'i')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 == 'E' || c6 == 'e')
					{
						char c7 = buffer[startIndex++];
						if (c7 == 'W' || c7 == 'w')
						{
							result = StringToken.DropView;
						}
					}
					break;
				}
				}
				break;
			}
			}
			break;
		case 'E':
		case 'e':
		{
			char c = buffer[startIndex++];
			if (c != 'N' && c != 'n')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'C' && c2 != 'c')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'R' && c3 != 'r')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'Y' && c4 != 'y')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'P' && c5 != 'p')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'T' && c6 != 't')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 == 'E' || c7 == 'e')
			{
				char c8 = buffer[startIndex++];
				if (c8 == 'D' || c8 == 'd')
				{
					result = StringToken.Encrypted;
				}
			}
			break;
		}
		case 'F':
		case 'f':
			switch (buffer[startIndex++])
			{
			case 'E':
			case 'e':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'D' && c2 != 'd')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'E' && c3 != 'e')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'R' && c4 != 'r')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'A' && c5 != 'a')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'T' && c6 != 't')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 == 'E' || c7 == 'e')
				{
					char c8 = buffer[startIndex++];
					if (c8 == 'D' || c8 == 'd')
					{
						result = StringToken.Federated;
					}
				}
				break;
			}
			case 'I':
			case 'i':
				switch (buffer[startIndex++])
				{
				case 'L':
				case 'l':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'E' && c3 != 'e')
					{
						break;
					}
					switch (buffer[startIndex++])
					{
					case '_':
					{
						char c5 = buffer[startIndex++];
						if (c5 != 'N' && c5 != 'n')
						{
							break;
						}
						char c6 = buffer[startIndex++];
						if (c6 != 'A' && c6 != 'a')
						{
							break;
						}
						char c7 = buffer[startIndex++];
						if (c7 == 'M' || c7 == 'm')
						{
							char c8 = buffer[startIndex++];
							if (c8 == 'E' || c8 == 'e')
							{
								result = StringToken.FileName;
							}
						}
						break;
					}
					case 'G':
					case 'g':
					{
						char c5 = buffer[startIndex++];
						if (c5 != 'R' && c5 != 'r')
						{
							break;
						}
						char c6 = buffer[startIndex++];
						if (c6 != 'O' && c6 != 'o')
						{
							break;
						}
						char c7 = buffer[startIndex++];
						if (c7 == 'U' || c7 == 'u')
						{
							char c8 = buffer[startIndex++];
							if (c8 == 'P' || c8 == 'p')
							{
								result = StringToken.FileGroup;
							}
						}
						break;
					}
					case 'T':
					case 't':
					{
						char c5 = buffer[startIndex++];
						if (c5 != 'A' && c5 != 'a')
						{
							break;
						}
						char c6 = buffer[startIndex++];
						if (c6 != 'B' && c6 != 'b')
						{
							break;
						}
						char c7 = buffer[startIndex++];
						if (c7 == 'L' || c7 == 'l')
						{
							char c8 = buffer[startIndex++];
							if (c8 == 'E' || c8 == 'e')
							{
								result = StringToken.Filetable;
							}
						}
						break;
					}
					}
					break;
				}
				case 'R':
				case 'r':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'S' && c3 != 's')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if ((c4 != 'T' && c4 != 't') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'R' && c5 != 'r')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 == 'O' || c6 == 'o')
					{
						char c7 = buffer[startIndex++];
						if (c7 == 'W' || c7 == 'w')
						{
							result = StringToken.FirstRow;
						}
					}
					break;
				}
				}
				break;
			case 'O':
			case 'o':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'R' && c2 != 'r')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'C' && c3 != 'c')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'E' && c4 != 'e')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'S' && c5 != 's')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'E' && c6 != 'e')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 == 'E' || c7 == 'e')
				{
					char c8 = buffer[startIndex++];
					if (c8 == 'K' || c8 == 'k')
					{
						result = StringToken.ForceSeek;
					}
				}
				break;
			}
			}
			break;
		case 'G':
		case 'g':
		{
			char c = buffer[startIndex++];
			if (c != 'E' && c != 'e')
			{
				break;
			}
			switch (buffer[startIndex++])
			{
			case 'N':
			case 'n':
			{
				char c3 = buffer[startIndex++];
				if (c3 != 'E' && c3 != 'e')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'R' && c4 != 'r')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'A' && c5 != 'a')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'T' && c6 != 't')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 == 'E' || c7 == 'e')
				{
					char c8 = buffer[startIndex++];
					if (c8 == 'D' || c8 == 'd')
					{
						result = StringToken.Generated;
					}
				}
				break;
			}
			case 'O':
			case 'o':
			{
				char c3 = buffer[startIndex++];
				if (c3 != 'G' && c3 != 'g')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'R' && c4 != 'r')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'A' && c5 != 'a')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'P' && c6 != 'p')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 == 'H' || c7 == 'h')
				{
					char c8 = buffer[startIndex++];
					if (c8 == 'Y' || c8 == 'y')
					{
						result = StringToken.Geography;
					}
				}
				break;
			}
			}
			break;
		}
		case 'H':
		case 'h':
		{
			char c = buffer[startIndex++];
			if (c != 'A' && c != 'a')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'R' && c2 != 'r')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'D' && c3 != 'd')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'E' && c4 != 'e')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'N' && c5 != 'n')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'I' && c6 != 'i')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 == 'N' || c7 == 'n')
			{
				char c8 = buffer[startIndex++];
				if (c8 == 'G' || c8 == 'g')
				{
					result = StringToken.Hardening;
				}
			}
			break;
		}
		case 'I':
		case 'i':
		{
			char c = buffer[startIndex++];
			if (c != 'N' && c != 'n')
			{
				break;
			}
			switch (buffer[startIndex++])
			{
			case 'C':
			case 'c':
			{
				char c3 = buffer[startIndex++];
				if (c3 != 'R' && c3 != 'r')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'E' && c4 != 'e')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'M' && c5 != 'm')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'E' && c6 != 'e')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 == 'N' || c7 == 'n')
				{
					char c8 = buffer[startIndex++];
					if (c8 == 'T' || c8 == 't')
					{
						result = StringToken.Increment;
					}
				}
				break;
			}
			case 'T':
			case 't':
			{
				char c3 = buffer[startIndex++];
				if (c3 != 'E' && c3 != 'e')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'R' && c4 != 'r')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'S' && c5 != 's')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'E' && c6 != 'e')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 == 'C' || c7 == 'c')
				{
					char c8 = buffer[startIndex++];
					if (c8 == 'T' || c8 == 't')
					{
						result = StringToken.Intersect;
					}
				}
				break;
			}
			}
			break;
		}
		case 'M':
		case 'm':
		{
			char c = buffer[startIndex++];
			if (c != 'A' && c != 'a')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'X' && c2 != 'x')
			{
				break;
			}
			switch (buffer[startIndex++])
			{
			case '_':
			{
				char c4 = buffer[startIndex++];
				if (c4 != 'F' && c4 != 'f')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'I' && c5 != 'i')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'L' && c6 != 'l')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 == 'E' || c7 == 'e')
				{
					char c8 = buffer[startIndex++];
					if (c8 == 'S' || c8 == 's')
					{
						result = StringToken.MaxFiles;
					}
				}
				break;
			}
			case 'L':
			case 'l':
			{
				char c4 = buffer[startIndex++];
				if (c4 != 'E' && c4 != 'e')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'N' && c5 != 'n')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'G' && c6 != 'g')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 == 'T' || c7 == 't')
				{
					char c8 = buffer[startIndex++];
					if (c8 == 'H' || c8 == 'h')
					{
						result = StringToken.MaxLength;
					}
				}
				break;
			}
			}
			break;
		}
		case 'N':
		case 'n':
		{
			char c = buffer[startIndex++];
			if (c != 'E' && c != 'e')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'S' && c2 != 's')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'T' && c3 != 't')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'L' && c4 != 'l')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'E' && c5 != 'e')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'V' && c6 != 'v')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 == 'E' || c7 == 'e')
			{
				char c8 = buffer[startIndex++];
				if (c8 == 'L' || c8 == 'l')
				{
					result = StringToken.Nestlevel;
				}
			}
			break;
		}
		case 'O':
		case 'o':
			switch (buffer[startIndex++])
			{
			case 'B':
			case 'b':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'J' && c2 != 'j')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'E' && c3 != 'e')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'C' && c4 != 'c')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if ((c5 != 'T' && c5 != 't') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 == 'I' || c6 == 'i')
				{
					char c7 = buffer[startIndex++];
					if (c7 == 'D' || c7 == 'd')
					{
						result = StringToken.ObjectId;
					}
				}
				break;
			}
			case 'P':
			case 'p':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'E' && c2 != 'e')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'N' && c3 != 'n')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'Q' && c4 != 'q')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'U' && c5 != 'u')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'E' && c6 != 'e')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 == 'R' || c7 == 'r')
				{
					char c8 = buffer[startIndex++];
					if (c8 == 'Y' || c8 == 'y')
					{
						result = StringToken.OpenQuery;
					}
				}
				break;
			}
			}
			break;
		case 'P':
		case 'p':
			switch (buffer[startIndex++])
			{
			case 'A':
			case 'a':
				switch (buffer[startIndex++])
				{
				case 'C':
				case 'c':
				{
					char c3 = buffer[startIndex++];
					if ((c3 != 'K' && c3 != 'k') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'S' && c4 != 's')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'E' && c5 != 'e')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 == 'N' || c6 == 'n')
					{
						char c7 = buffer[startIndex++];
						if (c7 == 'T' || c7 == 't')
						{
							result = StringToken.Pack_sent;
						}
					}
					break;
				}
				case 'D':
				case 'd':
				{
					if (buffer[startIndex++] != '_')
					{
						break;
					}
					char c3 = buffer[startIndex++];
					if (c3 != 'I' && c3 != 'i')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'N' && c4 != 'n')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'D' && c5 != 'd')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 == 'E' || c6 == 'e')
					{
						char c7 = buffer[startIndex++];
						if (c7 == 'X' || c7 == 'x')
						{
							result = StringToken.PadIndex;
						}
					}
					break;
				}
				case 'R':
				case 'r':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'T' && c3 != 't')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'I' && c4 != 'i')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'T' && c5 != 't')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'I' && c6 != 'i')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 == 'O' || c7 == 'o')
					{
						char c8 = buffer[startIndex++];
						if (c8 == 'N' || c8 == 'n')
						{
							result = StringToken.Partition;
						}
					}
					break;
				}
				}
				break;
			case 'E':
			case 'e':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'R' && c2 != 'r')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'S' && c3 != 's')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'I' && c4 != 'i')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'S' && c5 != 's')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'T' && c6 != 't')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 == 'E' || c7 == 'e')
				{
					char c8 = buffer[startIndex++];
					if (c8 == 'D' || c8 == 'd')
					{
						result = StringToken.Persisted;
					}
				}
				break;
			}
			case 'R':
			case 'r':
				switch (buffer[startIndex++])
				{
				case 'E':
				case 'e':
					switch (buffer[startIndex++])
					{
					case 'C':
					case 'c':
					{
						char c4 = buffer[startIndex++];
						if (c4 != 'I' && c4 != 'i')
						{
							break;
						}
						char c5 = buffer[startIndex++];
						if (c5 != 'S' && c5 != 's')
						{
							break;
						}
						char c6 = buffer[startIndex++];
						if (c6 != 'I' && c6 != 'i')
						{
							break;
						}
						char c7 = buffer[startIndex++];
						if (c7 == 'O' || c7 == 'o')
						{
							char c8 = buffer[startIndex++];
							if (c8 == 'N' || c8 == 'n')
							{
								result = StringToken.Precision;
							}
						}
						break;
					}
					case 'D':
					case 'd':
					{
						char c4 = buffer[startIndex++];
						if (c4 != 'I' && c4 != 'i')
						{
							break;
						}
						char c5 = buffer[startIndex++];
						if (c5 != 'C' && c5 != 'c')
						{
							break;
						}
						char c6 = buffer[startIndex++];
						if (c6 != 'A' && c6 != 'a')
						{
							break;
						}
						char c7 = buffer[startIndex++];
						if (c7 == 'T' || c7 == 't')
						{
							char c8 = buffer[startIndex++];
							if (c8 == 'E' || c8 == 'e')
							{
								result = StringToken.Predicate;
							}
						}
						break;
					}
					}
					break;
				case 'O':
				case 'o':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'C' && c3 != 'c')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'E' && c4 != 'e')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'D' && c5 != 'd')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'U' && c6 != 'u')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 == 'R' || c7 == 'r')
					{
						char c8 = buffer[startIndex++];
						if (c8 == 'E' || c8 == 'e')
						{
							result = StringToken.Procedure;
						}
					}
					break;
				}
				}
				break;
			}
			break;
		case 'R':
		case 'r':
			switch (buffer[startIndex++])
			{
			case 'A':
			case 'a':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'I' && c2 != 'i')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'S' && c3 != 's')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'E' && c4 != 'e')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'R' && c5 != 'r')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'R' && c6 != 'r')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 == 'O' || c7 == 'o')
				{
					char c8 = buffer[startIndex++];
					if (c8 == 'R' || c8 == 'r')
					{
						result = StringToken.Raiserror;
					}
				}
				break;
			}
			case 'E':
			case 'e':
				switch (buffer[startIndex++])
				{
				case 'A':
				case 'a':
				{
					char c3 = buffer[startIndex++];
					if ((c3 != 'D' && c3 != 'd') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'O' && c4 != 'o')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'N' && c5 != 'n')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 == 'L' || c6 == 'l')
					{
						char c7 = buffer[startIndex++];
						if (c7 == 'Y' || c7 == 'y')
						{
							result = StringToken.Read_Only;
						}
					}
					break;
				}
				case 'C':
				case 'c':
					switch (buffer[startIndex++])
					{
					case 'O':
					case 'o':
					{
						char c4 = buffer[startIndex++];
						if (c4 != 'M' && c4 != 'm')
						{
							break;
						}
						char c5 = buffer[startIndex++];
						if (c5 != 'P' && c5 != 'p')
						{
							break;
						}
						char c6 = buffer[startIndex++];
						if (c6 != 'I' && c6 != 'i')
						{
							break;
						}
						char c7 = buffer[startIndex++];
						if (c7 == 'L' || c7 == 'l')
						{
							char c8 = buffer[startIndex++];
							if (c8 == 'E' || c8 == 'e')
							{
								result = StringToken.Recompile;
							}
						}
						break;
					}
					case 'U':
					case 'u':
					{
						char c4 = buffer[startIndex++];
						if (c4 != 'R' && c4 != 'r')
						{
							break;
						}
						char c5 = buffer[startIndex++];
						if (c5 != 'S' && c5 != 's')
						{
							break;
						}
						char c6 = buffer[startIndex++];
						if (c6 != 'I' && c6 != 'i')
						{
							break;
						}
						char c7 = buffer[startIndex++];
						if (c7 == 'V' || c7 == 'v')
						{
							char c8 = buffer[startIndex++];
							if (c8 == 'E' || c8 == 'e')
							{
								result = StringToken.Recursive;
							}
						}
						break;
					}
					}
					break;
				case 'M':
				case 'm':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'S' && c3 != 's')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'E' && c4 != 'e')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'R' && c5 != 'r')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'V' && c6 != 'v')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 == 'E' || c7 == 'e')
					{
						char c8 = buffer[startIndex++];
						if (c8 == 'R' || c8 == 'r')
						{
							result = StringToken.Remserver;
						}
					}
					break;
				}
				case 'S':
				case 's':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'U' && c3 != 'u')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'M' && c4 != 'm')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'A' && c5 != 'a')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'B' && c6 != 'b')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 == 'L' || c7 == 'l')
					{
						char c8 = buffer[startIndex++];
						if (c8 == 'E' || c8 == 'e')
						{
							result = StringToken.Resumable;
						}
					}
					break;
				}
				}
				break;
			}
			break;
		case 'S':
		case 's':
			switch (buffer[startIndex++])
			{
			case 'C':
			case 'c':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'H' && c2 != 'h')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'E' && c3 != 'e')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'D' && c4 != 'd')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'U' && c5 != 'u')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'L' && c6 != 'l')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 == 'E' || c7 == 'e')
				{
					char c8 = buffer[startIndex++];
					if (c8 == 'R' || c8 == 'r')
					{
						result = StringToken.Scheduler;
					}
				}
				break;
			}
			case 'E':
			case 'e':
				switch (buffer[startIndex++])
				{
				case 'C':
				case 'c':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'O' && c3 != 'o')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'N' && c4 != 'n')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'D' && c5 != 'd')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'A' && c6 != 'a')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 == 'R' || c7 == 'r')
					{
						char c8 = buffer[startIndex++];
						if (c8 == 'Y' || c8 == 'y')
						{
							result = StringToken.Secondary;
						}
					}
					break;
				}
				case 'L':
				case 'l':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'E' && c3 != 'e')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'C' && c4 != 'c')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'T' && c5 != 't')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'I' && c6 != 'i')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 == 'V' || c7 == 'v')
					{
						char c8 = buffer[startIndex++];
						if (c8 == 'E' || c8 == 'e')
						{
							result = StringToken.Selective;
						}
					}
					break;
				}
				}
				break;
			case 'I':
			case 'i':
				switch (buffer[startIndex++])
				{
				case 'G':
				case 'g':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'N' && c3 != 'n')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'A' && c4 != 'a')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'T' && c5 != 't')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'U' && c6 != 'u')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 == 'R' || c7 == 'r')
					{
						char c8 = buffer[startIndex++];
						if (c8 == 'E' || c8 == 'e')
						{
							result = StringToken.Signature;
						}
					}
					break;
				}
				case 'N':
				case 'n':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'G' && c3 != 'g')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'L' && c4 != 'l')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'E' && c5 != 'e')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'T' && c6 != 't')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 == 'O' || c7 == 'o')
					{
						char c8 = buffer[startIndex++];
						if (c8 == 'N' || c8 == 'n')
						{
							result = StringToken.Singleton;
						}
					}
					break;
				}
				}
				break;
			case 'Y':
			case 'y':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'M' && c2 != 'm')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'M' && c3 != 'm')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'E' && c4 != 'e')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'T' && c5 != 't')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'R' && c6 != 'r')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 == 'I' || c7 == 'i')
				{
					char c8 = buffer[startIndex++];
					if (c8 == 'C' || c8 == 'c')
					{
						result = StringToken.Symmetric;
					}
				}
				break;
			}
			}
			break;
		case 'T':
		case 't':
			switch (buffer[startIndex++])
			{
			case 'I':
			case 'i':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'M' && c2 != 'm')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'E' && c3 != 'e')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case 'S':
				case 's':
				{
					char c5 = buffer[startIndex++];
					if (c5 != 'T' && c5 != 't')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'A' && c6 != 'a')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 == 'M' || c7 == 'm')
					{
						char c8 = buffer[startIndex++];
						if (c8 == 'P' || c8 == 'p')
						{
							result = StringToken.Timestamp;
						}
					}
					break;
				}
				case 'T':
				case 't':
				{
					char c5 = buffer[startIndex++];
					if (c5 != 'I' && c5 != 'i')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'C' && c6 != 'c')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 == 'K' || c7 == 'k')
					{
						char c8 = buffer[startIndex++];
						if (c8 == 'S' || c8 == 's')
						{
							result = StringToken.Timeticks;
						}
					}
					break;
				}
				}
				break;
			}
			case 'R':
			case 'r':
				switch (buffer[startIndex++])
				{
				case 'A':
				case 'a':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'N' && c3 != 'n')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'C' && c4 != 'c')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'O' && c5 != 'o')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'U' && c6 != 'u')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 == 'N' || c7 == 'n')
					{
						char c8 = buffer[startIndex++];
						if (c8 == 'T' || c8 == 't')
						{
							result = StringToken.Trancount;
						}
					}
					break;
				}
				case 'Y':
				case 'y':
				{
					if (buffer[startIndex++] != '_')
					{
						break;
					}
					char c3 = buffer[startIndex++];
					if (c3 != 'P' && c3 != 'p')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'A' && c4 != 'a')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'R' && c5 != 'r')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 == 'S' || c6 == 's')
					{
						char c7 = buffer[startIndex++];
						if (c7 == 'E' || c7 == 'e')
						{
							result = StringToken.TryParse;
						}
					}
					break;
				}
				}
				break;
			case 'Y':
			case 'y':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'P' && c2 != 'p')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if ((c3 != 'E' && c3 != 'e') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'D' && c4 != 'd')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'E' && c5 != 'e')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 == 'S' || c6 == 's')
				{
					char c7 = buffer[startIndex++];
					if (c7 == 'C' || c7 == 'c')
					{
						result = StringToken.TypeDesc;
					}
				}
				break;
			}
			}
			break;
		case 'U':
		case 'u':
		{
			char c = buffer[startIndex++];
			if (c != 'N' && c != 'n')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'B' && c2 != 'b')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'O' && c3 != 'o')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'U' && c4 != 'u')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'N' && c5 != 'n')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'D' && c6 != 'd')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 == 'E' || c7 == 'e')
			{
				char c8 = buffer[startIndex++];
				if (c8 == 'D' || c8 == 'd')
				{
					result = StringToken.Unbounded;
				}
			}
			break;
		}
		case 'V':
		case 'v':
		{
			char c = buffer[startIndex++];
			if (c != 'A' && c != 'a')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'R' && c2 != 'r')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'B' && c3 != 'b')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'I' && c4 != 'i')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'N' && c5 != 'n')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'A' && c6 != 'a')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 == 'R' || c7 == 'r')
			{
				char c8 = buffer[startIndex++];
				if (c8 == 'Y' || c8 == 'y')
				{
					result = StringToken.VarBinary;
				}
			}
			break;
		}
		case 'W':
		case 'w':
		{
			char c = buffer[startIndex++];
			if (c != 'R' && c != 'r')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'I' && c2 != 'i')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'T' && c3 != 't')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'E' && c4 != 'e')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'T' && c5 != 't')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'E' && c6 != 'e')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 == 'X' || c7 == 'x')
			{
				char c8 = buffer[startIndex++];
				if (c8 == 'T' || c8 == 't')
				{
					result = StringToken.WriteText;
				}
			}
			break;
		}
		case 'X':
		case 'x':
		{
			char c = buffer[startIndex++];
			if (c != 'M' && c != 'm')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'L' && c2 != 'l')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'B' && c3 != 'b')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'I' && c4 != 'i')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'N' && c5 != 'n')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'A' && c6 != 'a')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 == 'R' || c7 == 'r')
			{
				char c8 = buffer[startIndex++];
				if (c8 == 'Y' || c8 == 'y')
				{
					result = StringToken.XmlBinary;
				}
			}
			break;
		}
		}
		return result;
	}

	private static StringToken Find_Len10(string buffer, int startIndex)
	{
		StringToken result = StringToken.None;
		switch (buffer[startIndex++])
		{
		case 'A':
		case 'a':
			switch (buffer[startIndex++])
			{
			case 'L':
			case 'l':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'T' && c2 != 't')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'E' && c3 != 'e')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if ((c4 != 'R' && c4 != 'r') || buffer[startIndex++] != '_')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case 'R':
				case 'r':
				{
					char c6 = buffer[startIndex++];
					if (c6 != 'O' && c6 != 'o')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 == 'L' || c7 == 'l')
					{
						char c8 = buffer[startIndex++];
						if (c8 == 'E' || c8 == 'e')
						{
							result = StringToken.AlterRole;
						}
					}
					break;
				}
				case 'U':
				case 'u':
				{
					char c6 = buffer[startIndex++];
					if (c6 != 'S' && c6 != 's')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 == 'E' || c7 == 'e')
					{
						char c8 = buffer[startIndex++];
						if (c8 == 'R' || c8 == 'r')
						{
							result = StringToken.AlterUser;
						}
					}
					break;
				}
				case 'V':
				case 'v':
				{
					char c6 = buffer[startIndex++];
					if (c6 != 'I' && c6 != 'i')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 == 'E' || c7 == 'e')
					{
						char c8 = buffer[startIndex++];
						if (c8 == 'W' || c8 == 'w')
						{
							result = StringToken.AlterView;
						}
					}
					break;
				}
				}
				break;
			}
			case 'S':
			case 's':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'Y' && c2 != 'y')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'M' && c3 != 'm')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'M' && c4 != 'm')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'E' && c5 != 'e')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'T' && c6 != 't')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'R' && c7 != 'r')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 == 'I' || c8 == 'i')
				{
					char c9 = buffer[startIndex++];
					if (c9 == 'C' || c9 == 'c')
					{
						result = StringToken.Asymmetric;
					}
				}
				break;
			}
			}
			break;
		case 'C':
		case 'c':
			switch (buffer[startIndex++])
			{
			case 'H':
			case 'h':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'E' && c2 != 'e')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'C' && c3 != 'c')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'K' && c4 != 'k')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'P' && c5 != 'p')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'O' && c6 != 'o')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'I' && c7 != 'i')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 == 'N' || c8 == 'n')
				{
					char c9 = buffer[startIndex++];
					if (c9 == 'T' || c9 == 't')
					{
						result = StringToken.Checkpoint;
					}
				}
				break;
			}
			case 'O':
			case 'o':
				switch (buffer[startIndex++])
				{
				case 'L':
				case 'l':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'U' && c3 != 'u')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'M' && c4 != 'm')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if ((c5 != 'N' && c5 != 'n') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'S' && c6 != 's')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 == 'E' || c7 == 'e')
					{
						char c8 = buffer[startIndex++];
						if (c8 == 'T' || c8 == 't')
						{
							result = StringToken.ColumnSet;
						}
					}
					break;
				}
				case 'N':
				case 'n':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'S' && c3 != 's')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'T' && c4 != 't')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'R' && c5 != 'r')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'A' && c6 != 'a')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'I' && c7 != 'i')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 == 'N' || c8 == 'n')
					{
						char c9 = buffer[startIndex++];
						if (c9 == 'T' || c9 == 't')
						{
							result = StringToken.Constraint;
						}
					}
					break;
				}
				}
				break;
			case 'R':
			case 'r':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'E' && c2 != 'e')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'D' && c3 != 'd')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'E' && c4 != 'e')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'N' && c5 != 'n')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'T' && c6 != 't')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'I' && c7 != 'i')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 == 'A' || c8 == 'a')
				{
					char c9 = buffer[startIndex++];
					if (c9 == 'L' || c9 == 'l')
					{
						result = StringToken.Credential;
					}
				}
				break;
			}
			}
			break;
		case 'D':
		case 'd':
			switch (buffer[startIndex++])
			{
			case 'E':
			case 'e':
				switch (buffer[startIndex++])
				{
				case 'A':
				case 'a':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'L' && c3 != 'l')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'L' && c4 != 'l')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'O' && c5 != 'o')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'C' && c6 != 'c')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'A' && c7 != 'a')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 == 'T' || c8 == 't')
					{
						char c9 = buffer[startIndex++];
						if (c9 == 'E' || c9 == 'e')
						{
							result = StringToken.Deallocate;
						}
					}
					break;
				}
				case 'C':
				case 'c':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'R' && c3 != 'r')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'Y' && c4 != 'y')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'P' && c5 != 'p')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'T' && c6 != 't')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'I' && c7 != 'i')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 == 'O' || c8 == 'o')
					{
						char c9 = buffer[startIndex++];
						if (c9 == 'N' || c9 == 'n')
						{
							result = StringToken.Decryption;
						}
					}
					break;
				}
				}
				break;
			case 'R':
			case 'r':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'O' && c2 != 'o')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if ((c3 != 'P' && c3 != 'p') || buffer[startIndex++] != '_')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case 'I':
				case 'i':
				{
					char c5 = buffer[startIndex++];
					if (c5 != 'N' && c5 != 'n')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'D' && c6 != 'd')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 == 'E' || c7 == 'e')
					{
						char c8 = buffer[startIndex++];
						if (c8 == 'X' || c8 == 'x')
						{
							result = StringToken.DropIndex;
						}
					}
					break;
				}
				case 'L':
				case 'l':
				{
					char c5 = buffer[startIndex++];
					if (c5 != 'O' && c5 != 'o')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'G' && c6 != 'g')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 == 'I' || c7 == 'i')
					{
						char c8 = buffer[startIndex++];
						if (c8 == 'N' || c8 == 'n')
						{
							result = StringToken.DropLogin;
						}
					}
					break;
				}
				case 'Q':
				case 'q':
				{
					char c5 = buffer[startIndex++];
					if (c5 != 'U' && c5 != 'u')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'E' && c6 != 'e')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 == 'U' || c7 == 'u')
					{
						char c8 = buffer[startIndex++];
						if (c8 == 'E' || c8 == 'e')
						{
							result = StringToken.DropQueue;
						}
					}
					break;
				}
				case 'R':
				case 'r':
				{
					char c5 = buffer[startIndex++];
					if (c5 != 'O' && c5 != 'o')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'U' && c6 != 'u')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 == 'T' || c7 == 't')
					{
						char c8 = buffer[startIndex++];
						if (c8 == 'E' || c8 == 'e')
						{
							result = StringToken.DropRoute;
						}
					}
					break;
				}
				case 'T':
				case 't':
				{
					char c5 = buffer[startIndex++];
					if (c5 != 'A' && c5 != 'a')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'B' && c6 != 'b')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 == 'L' || c7 == 'l')
					{
						char c8 = buffer[startIndex++];
						if (c8 == 'E' || c8 == 'e')
						{
							result = StringToken.DropTable;
						}
					}
					break;
				}
				}
				break;
			}
			case 'U':
			case 'u':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'R' && c2 != 'r')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'A' && c3 != 'a')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'B' && c4 != 'b')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'I' && c5 != 'i')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'L' && c6 != 'l')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'I' && c7 != 'i')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 == 'T' || c8 == 't')
				{
					char c9 = buffer[startIndex++];
					if (c9 == 'Y' || c9 == 'y')
					{
						result = StringToken.Durability;
					}
				}
				break;
			}
			}
			break;
		case 'E':
		case 'e':
		{
			char c = buffer[startIndex++];
			if (c != 'N' && c != 'n')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'C' && c2 != 'c')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'R' && c3 != 'r')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'Y' && c4 != 'y')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'P' && c5 != 'p')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'T' && c6 != 't')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'I' && c7 != 'i')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 == 'O' || c8 == 'o')
			{
				char c9 = buffer[startIndex++];
				if (c9 == 'N' || c9 == 'n')
				{
					result = StringToken.Encryption;
				}
			}
			break;
		}
		case 'F':
		case 'f':
		{
			char c = buffer[startIndex++];
			if (c != 'I' && c != 'i')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'L' && c2 != 'l')
			{
				break;
			}
			switch (buffer[startIndex++])
			{
			case 'E':
			case 'e':
			{
				char c4 = buffer[startIndex++];
				if (c4 != 'S' && c4 != 's')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'T' && c5 != 't')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'R' && c6 != 'r')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'E' && c7 != 'e')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 == 'A' || c8 == 'a')
				{
					char c9 = buffer[startIndex++];
					if (c9 == 'M' || c9 == 'm')
					{
						result = StringToken.FileStream;
					}
				}
				break;
			}
			case 'L':
			case 'l':
			{
				char c4 = buffer[startIndex++];
				if (c4 != 'F' && c4 != 'f')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'A' && c5 != 'a')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'C' && c6 != 'c')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'T' && c7 != 't')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 == 'O' || c8 == 'o')
				{
					char c9 = buffer[startIndex++];
					if (c9 == 'R' || c9 == 'r')
					{
						result = StringToken.FillFactor;
					}
				}
				break;
			}
			}
			break;
		}
		case 'N':
		case 'n':
			switch (buffer[startIndex++])
			{
			case 'A':
			case 'a':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'N' && c2 != 'n')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'O' && c3 != 'o')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'S' && c4 != 's')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'E' && c5 != 'e')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'C' && c6 != 'c')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'O' && c7 != 'o')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 == 'N' || c8 == 'n')
				{
					char c9 = buffer[startIndex++];
					if (c9 == 'D' || c9 == 'd')
					{
						result = StringToken.Nanosecond;
					}
				}
				break;
			}
			case 'C':
			case 'c':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'H' && c2 != 'h')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'A' && c3 != 'a')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'R' && c4 != 'r')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'A' && c5 != 'a')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'C' && c6 != 'c')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'T' && c7 != 't')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 == 'E' || c8 == 'e')
				{
					char c9 = buffer[startIndex++];
					if (c9 == 'R' || c9 == 'r')
					{
						result = StringToken.NCharacter;
					}
				}
				break;
			}
			}
			break;
		case 'O':
		case 'o':
		{
			char c = buffer[startIndex++];
			if (c != 'P' && c != 'p')
			{
				break;
			}
			switch (buffer[startIndex++])
			{
			case 'E':
			case 'e':
			{
				char c3 = buffer[startIndex++];
				if (c3 != 'N' && c3 != 'n')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'R' && c4 != 'r')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'O' && c5 != 'o')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'W' && c6 != 'w')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'S' && c7 != 's')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 == 'E' || c8 == 'e')
				{
					char c9 = buffer[startIndex++];
					if (c9 == 'T' || c9 == 't')
					{
						result = StringToken.OpenRowSet;
					}
				}
				break;
			}
			case 'T':
			case 't':
			{
				char c3 = buffer[startIndex++];
				if (c3 != 'I' && c3 != 'i')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'M' && c4 != 'm')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'I' && c5 != 'i')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'S' && c6 != 's')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'T' && c7 != 't')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 == 'I' || c8 == 'i')
				{
					char c9 = buffer[startIndex++];
					if (c9 == 'C' || c9 == 'c')
					{
						result = StringToken.Optimistic;
					}
				}
				break;
			}
			}
			break;
		}
		case 'P':
		case 'p':
			switch (buffer[startIndex++])
			{
			case 'A':
			case 'a':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'R' && c2 != 'r')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case 'A':
				case 'a':
				{
					char c4 = buffer[startIndex++];
					if (c4 != 'M' && c4 != 'm')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'E' && c5 != 'e')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'T' && c6 != 't')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'E' && c7 != 'e')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 == 'R' || c8 == 'r')
					{
						char c9 = buffer[startIndex++];
						if (c9 == 'S' || c9 == 's')
						{
							result = StringToken.Parameters;
						}
					}
					break;
				}
				case 'T':
				case 't':
				{
					char c4 = buffer[startIndex++];
					if (c4 != 'I' && c4 != 'i')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'T' && c5 != 't')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'I' && c6 != 'i')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'O' && c7 != 'o')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 == 'N' || c8 == 'n')
					{
						char c9 = buffer[startIndex++];
						if (c9 == 'S' || c9 == 's')
						{
							result = StringToken.Partitions;
						}
					}
					break;
				}
				}
				break;
			}
			case 'E':
			case 'e':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'R' && c2 != 'r')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'C' && c3 != 'c')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'E' && c4 != 'e')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'N' && c5 != 'n')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'T' && c6 != 't')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'A' && c7 != 'a')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 == 'G' || c8 == 'g')
				{
					char c9 = buffer[startIndex++];
					if (c9 == 'E' || c9 == 'e')
					{
						result = StringToken.Percentage;
					}
				}
				break;
			}
			case 'O':
			case 'o':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'P' && c2 != 'p')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'U' && c3 != 'u')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'L' && c4 != 'l')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'A' && c5 != 'a')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'T' && c6 != 't')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'I' && c7 != 'i')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 == 'O' || c8 == 'o')
				{
					char c9 = buffer[startIndex++];
					if (c9 == 'N' || c9 == 'n')
					{
						result = StringToken.Population;
					}
				}
				break;
			}
			}
			break;
		case 'R':
		case 'r':
			switch (buffer[startIndex++])
			{
			case 'E':
			case 'e':
				switch (buffer[startIndex++])
				{
				case 'F':
				case 'f':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'E' && c3 != 'e')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'R' && c4 != 'r')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'E' && c5 != 'e')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'N' && c6 != 'n')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'C' && c7 != 'c')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 == 'E' || c8 == 'e')
					{
						char c9 = buffer[startIndex++];
						if (c9 == 'S' || c9 == 's')
						{
							result = StringToken.References;
						}
					}
					break;
				}
				case 'O':
				case 'o':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'R' && c3 != 'r')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'G' && c4 != 'g')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'A' && c5 != 'a')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'N' && c6 != 'n')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'I' && c7 != 'i')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 == 'Z' || c8 == 'z')
					{
						char c9 = buffer[startIndex++];
						if (c9 == 'E' || c9 == 'e')
						{
							result = StringToken.Reorganize;
						}
					}
					break;
				}
				case 'P':
				case 'p':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'L' && c3 != 'l')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'I' && c4 != 'i')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'C' && c5 != 'c')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'A' && c6 != 'a')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'T' && c7 != 't')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 == 'E' || c8 == 'e')
					{
						char c9 = buffer[startIndex++];
						if (c9 == 'D' || c9 == 'd')
						{
							result = StringToken.Replicated;
						}
					}
					break;
				}
				}
				break;
			case 'O':
			case 'o':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'W' && c2 != 'w')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case 'G':
				case 'g':
				{
					char c4 = buffer[startIndex++];
					if (c4 != 'U' && c4 != 'u')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'I' && c5 != 'i')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'D' && c6 != 'd')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'C' && c7 != 'c')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 == 'O' || c8 == 'o')
					{
						char c9 = buffer[startIndex++];
						if (c9 == 'L' || c9 == 'l')
						{
							result = StringToken.RowGuidCol;
						}
					}
					break;
				}
				case 'V':
				case 'v':
				{
					char c4 = buffer[startIndex++];
					if (c4 != 'E' && c4 != 'e')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'R' && c5 != 'r')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'S' && c6 != 's')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'I' && c7 != 'i')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 == 'O' || c8 == 'o')
					{
						char c9 = buffer[startIndex++];
						if (c9 == 'N' || c9 == 'n')
						{
							result = StringToken.RowVersion;
						}
					}
					break;
				}
				}
				break;
			}
			}
			break;
		case 'S':
		case 's':
			switch (buffer[startIndex++])
			{
			case 'E':
			case 'e':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'R' && c2 != 'r')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'V' && c3 != 'v')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'E' && c4 != 'e')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'R' && c5 != 'r')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'N' && c6 != 'n')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'A' && c7 != 'a')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 == 'M' || c8 == 'm')
				{
					char c9 = buffer[startIndex++];
					if (c9 == 'E' || c9 == 'e')
					{
						result = StringToken.Servername;
					}
				}
				break;
			}
			case 'M':
			case 'm':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'A' && c2 != 'a')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'L' && c3 != 'l')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'L' && c4 != 'l')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'M' && c5 != 'm')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'O' && c6 != 'o')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'N' && c7 != 'n')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 == 'E' || c8 == 'e')
				{
					char c9 = buffer[startIndex++];
					if (c9 == 'Y' || c9 == 'y')
					{
						result = StringToken.SmallMoney;
					}
				}
				break;
			}
			case 'T':
			case 't':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'A' && c2 != 'a')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'T' && c3 != 't')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'I' && c4 != 'i')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'S' && c5 != 's')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'T' && c6 != 't')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'I' && c7 != 'i')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 == 'C' || c8 == 'c')
				{
					char c9 = buffer[startIndex++];
					if (c9 == 'S' || c9 == 's')
					{
						result = StringToken.Statistics;
					}
				}
				break;
			}
			}
			break;
		case 'T':
		case 't':
		{
			char c = buffer[startIndex++];
			if (c != 'O' && c != 'o')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'T' && c2 != 't')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'A' && c3 != 'a')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if ((c4 != 'L' && c4 != 'l') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'R' && c5 != 'r')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'E' && c6 != 'e')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 == 'A' || c7 == 'a')
			{
				char c8 = buffer[startIndex++];
				if (c8 == 'D' || c8 == 'd')
				{
					result = StringToken.Total_read;
				}
			}
			break;
		}
		case 'U':
		case 'u':
		{
			char c = buffer[startIndex++];
			if (c != 'P' && c != 'p')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'D' && c2 != 'd')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'A' && c3 != 'a')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'T' && c4 != 't')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'E' && c5 != 'e')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'T' && c6 != 't')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'E' && c7 != 'e')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 == 'X' || c8 == 'x')
			{
				char c9 = buffer[startIndex++];
				if (c9 == 'T' || c9 == 't')
				{
					result = StringToken.UpdateText;
				}
			}
			break;
		}
		case 'V':
		case 'v':
		{
			char c = buffer[startIndex++];
			if (c != 'A' && c != 'a')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'R' && c2 != 'r')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'C' && c3 != 'c')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'H' && c4 != 'h')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'A' && c5 != 'a')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'R' && c6 != 'r')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'M' && c7 != 'm')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 == 'A' || c8 == 'a')
			{
				char c9 = buffer[startIndex++];
				if (c9 == 'X' || c9 == 'x')
				{
					result = StringToken.VarCharMax;
				}
			}
			break;
		}
		}
		return result;
	}

	private static StringToken Find_Len11(string buffer, int startIndex)
	{
		StringToken result = StringToken.None;
		switch (buffer[startIndex++])
		{
		case 'A':
		case 'a':
			switch (buffer[startIndex++])
			{
			case 'L':
			case 'l':
			{
				char c9 = buffer[startIndex++];
				if (c9 != 'T' && c9 != 't')
				{
					break;
				}
				char c = buffer[startIndex++];
				if (c != 'E' && c != 'e')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if ((c2 != 'R' && c2 != 'r') || buffer[startIndex++] != '_')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case 'I':
				case 'i':
				{
					char c4 = buffer[startIndex++];
					if (c4 != 'N' && c4 != 'n')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'D' && c5 != 'd')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 == 'E' || c6 == 'e')
					{
						char c7 = buffer[startIndex++];
						if (c7 == 'X' || c7 == 'x')
						{
							result = StringToken.AlterIndex;
						}
					}
					break;
				}
				case 'L':
				case 'l':
				{
					char c4 = buffer[startIndex++];
					if (c4 != 'O' && c4 != 'o')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'G' && c5 != 'g')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 == 'I' || c6 == 'i')
					{
						char c7 = buffer[startIndex++];
						if (c7 == 'N' || c7 == 'n')
						{
							result = StringToken.AlterLogin;
						}
					}
					break;
				}
				case 'Q':
				case 'q':
				{
					char c4 = buffer[startIndex++];
					if (c4 != 'U' && c4 != 'u')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'E' && c5 != 'e')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 == 'U' || c6 == 'u')
					{
						char c7 = buffer[startIndex++];
						if (c7 == 'E' || c7 == 'e')
						{
							result = StringToken.AlterQueue;
						}
					}
					break;
				}
				case 'R':
				case 'r':
				{
					char c4 = buffer[startIndex++];
					if (c4 != 'O' && c4 != 'o')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'U' && c5 != 'u')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 == 'T' || c6 == 't')
					{
						char c7 = buffer[startIndex++];
						if (c7 == 'E' || c7 == 'e')
						{
							result = StringToken.AlterRoute;
						}
					}
					break;
				}
				case 'T':
				case 't':
				{
					char c4 = buffer[startIndex++];
					if (c4 != 'A' && c4 != 'a')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'B' && c5 != 'b')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 == 'L' || c6 == 'l')
					{
						char c7 = buffer[startIndex++];
						if (c7 == 'E' || c7 == 'e')
						{
							result = StringToken.AlterTable;
						}
					}
					break;
				}
				}
				break;
			}
			case 'P':
			case 'p':
			{
				char c9 = buffer[startIndex++];
				if (c9 != 'P' && c9 != 'p')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case 'E':
				case 'e':
				{
					char c2 = buffer[startIndex++];
					if (c2 != 'N' && c2 != 'n')
					{
						break;
					}
					char c3 = buffer[startIndex++];
					if ((c3 != 'D' && c3 != 'd') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'O' && c4 != 'o')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'N' && c5 != 'n')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 == 'L' || c6 == 'l')
					{
						char c7 = buffer[startIndex++];
						if (c7 == 'Y' || c7 == 'y')
						{
							result = StringToken.AppendOnly;
						}
					}
					break;
				}
				case 'L':
				case 'l':
				{
					char c2 = buffer[startIndex++];
					if (c2 != 'I' && c2 != 'i')
					{
						break;
					}
					char c3 = buffer[startIndex++];
					if (c3 != 'C' && c3 != 'c')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'A' && c4 != 'a')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'T' && c5 != 't')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'I' && c6 != 'i')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 == 'O' || c7 == 'o')
					{
						char c8 = buffer[startIndex++];
						if (c8 == 'N' || c8 == 'n')
						{
							result = StringToken.Application;
						}
					}
					break;
				}
				}
				break;
			}
			}
			break;
		case 'C':
		case 'c':
			switch (buffer[startIndex++])
			{
			case 'E':
			case 'e':
			{
				char c9 = buffer[startIndex++];
				if (c9 != 'R' && c9 != 'r')
				{
					break;
				}
				char c = buffer[startIndex++];
				if (c != 'T' && c != 't')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'I' && c2 != 'i')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'F' && c3 != 'f')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'I' && c4 != 'i')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'C' && c5 != 'c')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'A' && c6 != 'a')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 == 'T' || c7 == 't')
				{
					char c8 = buffer[startIndex++];
					if (c8 == 'E' || c8 == 'e')
					{
						result = StringToken.Certificate;
					}
				}
				break;
			}
			case 'O':
			case 'o':
				switch (buffer[startIndex++])
				{
				case 'L':
				case 'l':
				{
					char c = buffer[startIndex++];
					if (c != 'U' && c != 'u')
					{
						break;
					}
					char c2 = buffer[startIndex++];
					if (c2 != 'M' && c2 != 'm')
					{
						break;
					}
					char c3 = buffer[startIndex++];
					if (c3 != 'N' && c3 != 'n')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'S' && c4 != 's')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'T' && c5 != 't')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'O' && c6 != 'o')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 == 'R' || c7 == 'r')
					{
						char c8 = buffer[startIndex++];
						if (c8 == 'E' || c8 == 'e')
						{
							result = StringToken.ColumnStore;
						}
					}
					break;
				}
				case 'N':
				case 'n':
					switch (buffer[startIndex++])
					{
					case 'N':
					case 'n':
					{
						char c2 = buffer[startIndex++];
						if (c2 != 'E' && c2 != 'e')
						{
							break;
						}
						char c3 = buffer[startIndex++];
						if (c3 != 'C' && c3 != 'c')
						{
							break;
						}
						char c4 = buffer[startIndex++];
						if (c4 != 'T' && c4 != 't')
						{
							break;
						}
						char c5 = buffer[startIndex++];
						if (c5 != 'I' && c5 != 'i')
						{
							break;
						}
						char c6 = buffer[startIndex++];
						if (c6 != 'O' && c6 != 'o')
						{
							break;
						}
						char c7 = buffer[startIndex++];
						if (c7 == 'N' || c7 == 'n')
						{
							char c8 = buffer[startIndex++];
							if (c8 == 'S' || c8 == 's')
							{
								result = StringToken.Connections;
							}
						}
						break;
					}
					case 'T':
					case 't':
					{
						char c2 = buffer[startIndex++];
						if (c2 != 'A' && c2 != 'a')
						{
							break;
						}
						char c3 = buffer[startIndex++];
						if (c3 != 'I' && c3 != 'i')
						{
							break;
						}
						char c4 = buffer[startIndex++];
						if (c4 != 'N' && c4 != 'n')
						{
							break;
						}
						char c5 = buffer[startIndex++];
						if (c5 != 'M' && c5 != 'm')
						{
							break;
						}
						char c6 = buffer[startIndex++];
						if (c6 != 'E' && c6 != 'e')
						{
							break;
						}
						char c7 = buffer[startIndex++];
						if (c7 == 'N' || c7 == 'n')
						{
							char c8 = buffer[startIndex++];
							if (c8 == 'T' || c8 == 't')
							{
								result = StringToken.Containment;
							}
						}
						break;
					}
					}
					break;
				}
				break;
			case 'R':
			case 'r':
			{
				char c9 = buffer[startIndex++];
				if (c9 != 'E' && c9 != 'e')
				{
					break;
				}
				char c = buffer[startIndex++];
				if (c != 'A' && c != 'a')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'T' && c2 != 't')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if ((c3 != 'E' && c3 != 'e') || buffer[startIndex++] != '_')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case 'R':
				case 'r':
					switch (buffer[startIndex++])
					{
					case 'O':
					case 'o':
					{
						char c6 = buffer[startIndex++];
						if (c6 == 'L' || c6 == 'l')
						{
							char c7 = buffer[startIndex++];
							if (c7 == 'E' || c7 == 'e')
							{
								result = StringToken.CreateRole;
							}
						}
						break;
					}
					case 'U':
					case 'u':
					{
						char c6 = buffer[startIndex++];
						if (c6 == 'L' || c6 == 'l')
						{
							char c7 = buffer[startIndex++];
							if (c7 == 'E' || c7 == 'e')
							{
								result = StringToken.CreateRule;
							}
						}
						break;
					}
					}
					break;
				case 'T':
				case 't':
				{
					char c5 = buffer[startIndex++];
					if (c5 != 'Y' && c5 != 'y')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 == 'P' || c6 == 'p')
					{
						char c7 = buffer[startIndex++];
						if (c7 == 'E' || c7 == 'e')
						{
							result = StringToken.CreateType;
						}
					}
					break;
				}
				case 'U':
				case 'u':
				{
					char c5 = buffer[startIndex++];
					if (c5 != 'S' && c5 != 's')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 == 'E' || c6 == 'e')
					{
						char c7 = buffer[startIndex++];
						if (c7 == 'R' || c7 == 'r')
						{
							result = StringToken.CreateUser;
						}
					}
					break;
				}
				case 'V':
				case 'v':
				{
					char c5 = buffer[startIndex++];
					if (c5 != 'I' && c5 != 'i')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 == 'E' || c6 == 'e')
					{
						char c7 = buffer[startIndex++];
						if (c7 == 'W' || c7 == 'w')
						{
							result = StringToken.CreateView;
						}
					}
					break;
				}
				}
				break;
			}
			case 'U':
			case 'u':
			{
				char c9 = buffer[startIndex++];
				if (c9 != 'R' && c9 != 'r')
				{
					break;
				}
				char c = buffer[startIndex++];
				if (c != 'S' && c != 's')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'O' && c2 != 'o')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if ((c3 != 'R' && c3 != 'r') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'R' && c4 != 'r')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'O' && c5 != 'o')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 == 'W' || c6 == 'w')
				{
					char c7 = buffer[startIndex++];
					if (c7 == 'S' || c7 == 's')
					{
						result = StringToken.Cursor_rows;
					}
				}
				break;
			}
			}
			break;
		case 'D':
		case 'd':
			switch (buffer[startIndex++])
			{
			case 'A':
			case 'a':
			{
				char c9 = buffer[startIndex++];
				if (c9 != 'T' && c9 != 't')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case 'A':
				case 'a':
				{
					if (buffer[startIndex++] != '_')
					{
						break;
					}
					char c2 = buffer[startIndex++];
					if (c2 != 'S' && c2 != 's')
					{
						break;
					}
					char c3 = buffer[startIndex++];
					if (c3 != 'O' && c3 != 'o')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'U' && c4 != 'u')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'R' && c5 != 'r')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 == 'C' || c6 == 'c')
					{
						char c7 = buffer[startIndex++];
						if (c7 == 'E' || c7 == 'e')
						{
							result = StringToken.DataSource;
						}
					}
					break;
				}
				case 'E':
				case 'e':
				{
					if (buffer[startIndex++] != '_')
					{
						break;
					}
					char c2 = buffer[startIndex++];
					if (c2 != 'F' && c2 != 'f')
					{
						break;
					}
					char c3 = buffer[startIndex++];
					if (c3 != 'O' && c3 != 'o')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'R' && c4 != 'r')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'M' && c5 != 'm')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 == 'A' || c6 == 'a')
					{
						char c7 = buffer[startIndex++];
						if (c7 == 'T' || c7 == 't')
						{
							result = StringToken.DateFormat;
						}
					}
					break;
				}
				}
				break;
			}
			case 'E':
			case 'e':
			{
				char c9 = buffer[startIndex++];
				if (c9 != 'N' && c9 != 'n')
				{
					break;
				}
				char c = buffer[startIndex++];
				if ((c != 'Y' && c != 'y') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'S' && c2 != 's')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'E' && c3 != 'e')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'R' && c4 != 'r')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'V' && c5 != 'v')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 == 'E' || c6 == 'e')
				{
					char c7 = buffer[startIndex++];
					if (c7 == 'R' || c7 == 'r')
					{
						result = StringToken.DenyServer;
					}
				}
				break;
			}
			case 'I':
			case 'i':
				switch (buffer[startIndex++])
				{
				case 'A':
				case 'a':
				{
					char c = buffer[startIndex++];
					if (c != 'G' && c != 'g')
					{
						break;
					}
					char c2 = buffer[startIndex++];
					if (c2 != 'N' && c2 != 'n')
					{
						break;
					}
					char c3 = buffer[startIndex++];
					if (c3 != 'O' && c3 != 'o')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'S' && c4 != 's')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'T' && c5 != 't')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'I' && c6 != 'i')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 == 'C' || c7 == 'c')
					{
						char c8 = buffer[startIndex++];
						if (c8 == 'S' || c8 == 's')
						{
							result = StringToken.Diagnostics;
						}
					}
					break;
				}
				case 'S':
				case 's':
				{
					char c = buffer[startIndex++];
					if (c != 'T' && c != 't')
					{
						break;
					}
					char c2 = buffer[startIndex++];
					if (c2 != 'R' && c2 != 'r')
					{
						break;
					}
					char c3 = buffer[startIndex++];
					if (c3 != 'I' && c3 != 'i')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'B' && c4 != 'b')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'U' && c5 != 'u')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'T' && c6 != 't')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 == 'E' || c7 == 'e')
					{
						char c8 = buffer[startIndex++];
						if (c8 == 'D' || c8 == 'd')
						{
							result = StringToken.Distributed;
						}
					}
					break;
				}
				}
				break;
			case 'R':
			case 'r':
			{
				char c9 = buffer[startIndex++];
				if (c9 != 'O' && c9 != 'o')
				{
					break;
				}
				char c = buffer[startIndex++];
				if ((c != 'P' && c != 'p') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'S' && c2 != 's')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'C' && c3 != 'c')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'H' && c4 != 'h')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'E' && c5 != 'e')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 == 'M' || c6 == 'm')
				{
					char c7 = buffer[startIndex++];
					if (c7 == 'A' || c7 == 'a')
					{
						result = StringToken.DropSchema;
					}
				}
				break;
			}
			}
			break;
		case 'F':
		case 'f':
			switch (buffer[startIndex++])
			{
			case 'I':
			case 'i':
			{
				char c9 = buffer[startIndex++];
				if (c9 != 'L' && c9 != 'l')
				{
					break;
				}
				char c = buffer[startIndex++];
				if ((c != 'E' && c != 'e') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'F' && c2 != 'f')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'O' && c3 != 'o')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'R' && c4 != 'r')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'M' && c5 != 'm')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 == 'A' || c6 == 'a')
				{
					char c7 = buffer[startIndex++];
					if (c7 == 'T' || c7 == 't')
					{
						result = StringToken.FileFormat;
					}
				}
				break;
			}
			case 'O':
			case 'o':
			{
				char c9 = buffer[startIndex++];
				if (c9 != 'R' && c9 != 'r')
				{
					break;
				}
				char c = buffer[startIndex++];
				if (c != 'M' && c != 'm')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'A' && c2 != 'a')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if ((c3 != 'T' && c3 != 't') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'T' && c4 != 't')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'Y' && c5 != 'y')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 == 'P' || c6 == 'p')
				{
					char c7 = buffer[startIndex++];
					if (c7 == 'E' || c7 == 'e')
					{
						result = StringToken.FormatType;
					}
				}
				break;
			}
			}
			break;
		case 'H':
		case 'h':
		{
			char c10 = buffer[startIndex++];
			if (c10 != 'I' && c10 != 'i')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'E' && c9 != 'e')
			{
				break;
			}
			char c = buffer[startIndex++];
			if (c != 'R' && c != 'r')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'A' && c2 != 'a')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'R' && c3 != 'r')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'C' && c4 != 'c')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'H' && c5 != 'h')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'Y' && c6 != 'y')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 == 'I' || c7 == 'i')
			{
				char c8 = buffer[startIndex++];
				if (c8 == 'D' || c8 == 'd')
				{
					result = StringToken.HierarchyId;
				}
			}
			break;
		}
		case 'I':
		case 'i':
			switch (buffer[startIndex++])
			{
			case 'D':
			case 'd':
			{
				char c9 = buffer[startIndex++];
				if (c9 != 'E' && c9 != 'e')
				{
					break;
				}
				char c = buffer[startIndex++];
				if (c != 'N' && c != 'n')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'T' && c2 != 't')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'I' && c3 != 'i')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'T' && c4 != 't')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'Y' && c5 != 'y')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'C' && c6 != 'c')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 == 'O' || c7 == 'o')
				{
					char c8 = buffer[startIndex++];
					if (c8 == 'L' || c8 == 'l')
					{
						result = StringToken.IdentityCol;
					}
				}
				break;
			}
			case 'N':
			case 'n':
			{
				char c9 = buffer[startIndex++];
				if (c9 != 'S' && c9 != 's')
				{
					break;
				}
				char c = buffer[startIndex++];
				if (c != 'E' && c != 'e')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'N' && c2 != 'n')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'S' && c3 != 's')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'I' && c4 != 'i')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'T' && c5 != 't')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'I' && c6 != 'i')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 == 'V' || c7 == 'v')
				{
					char c8 = buffer[startIndex++];
					if (c8 == 'E' || c8 == 'e')
					{
						result = StringToken.Insensitive;
					}
				}
				break;
			}
			}
			break;
		case 'L':
		case 'l':
		{
			char c10 = buffer[startIndex++];
			if (c10 != 'E' && c10 != 'e')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'D' && c9 != 'd')
			{
				break;
			}
			char c = buffer[startIndex++];
			if (c != 'G' && c != 'g')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'E' && c2 != 'e')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if ((c3 != 'R' && c3 != 'r') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'V' && c4 != 'v')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'I' && c5 != 'i')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 == 'E' || c6 == 'e')
			{
				char c7 = buffer[startIndex++];
				if (c7 == 'W' || c7 == 'w')
				{
					result = StringToken.LedgerView;
				}
			}
			break;
		}
		case 'M':
		case 'm':
			switch (buffer[startIndex++])
			{
			case 'I':
			case 'i':
				switch (buffer[startIndex++])
				{
				case 'C':
				case 'c':
				{
					char c = buffer[startIndex++];
					if (c != 'R' && c != 'r')
					{
						break;
					}
					char c2 = buffer[startIndex++];
					if (c2 != 'O' && c2 != 'o')
					{
						break;
					}
					char c3 = buffer[startIndex++];
					if (c3 != 'S' && c3 != 's')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'E' && c4 != 'e')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'C' && c5 != 'c')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'O' && c6 != 'o')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 == 'N' || c7 == 'n')
					{
						char c8 = buffer[startIndex++];
						if (c8 == 'D' || c8 == 'd')
						{
							result = StringToken.Microsecond;
						}
					}
					break;
				}
				case 'L':
				case 'l':
				{
					char c = buffer[startIndex++];
					if (c != 'L' && c != 'l')
					{
						break;
					}
					char c2 = buffer[startIndex++];
					if (c2 != 'I' && c2 != 'i')
					{
						break;
					}
					char c3 = buffer[startIndex++];
					if (c3 != 'S' && c3 != 's')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'E' && c4 != 'e')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'C' && c5 != 'c')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'O' && c6 != 'o')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 == 'N' || c7 == 'n')
					{
						char c8 = buffer[startIndex++];
						if (c8 == 'D' || c8 == 'd')
						{
							result = StringToken.Millisecond;
						}
					}
					break;
				}
				}
				break;
			case 'U':
			case 'u':
			{
				char c9 = buffer[startIndex++];
				if (c9 != 'S' && c9 != 's')
				{
					break;
				}
				char c = buffer[startIndex++];
				if ((c != 'T' && c != 't') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'C' && c2 != 'c')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'H' && c3 != 'h')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'A' && c4 != 'a')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'N' && c5 != 'n')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 == 'G' || c6 == 'g')
				{
					char c7 = buffer[startIndex++];
					if (c7 == 'E' || c7 == 'e')
					{
						result = StringToken.MustChange;
					}
				}
				break;
			}
			}
			break;
		case 'N':
		case 'n':
		{
			char c10 = buffer[startIndex++];
			if (c10 != 'V' && c10 != 'v')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'A' && c9 != 'a')
			{
				break;
			}
			char c = buffer[startIndex++];
			if (c != 'R' && c != 'r')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'C' && c2 != 'c')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'H' && c3 != 'h')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'A' && c4 != 'a')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'R' && c5 != 'r')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'M' && c6 != 'm')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 == 'A' || c7 == 'a')
			{
				char c8 = buffer[startIndex++];
				if (c8 == 'X' || c8 == 'x')
				{
					result = StringToken.NVarCharMax;
				}
			}
			break;
		}
		case 'O':
		case 'o':
		{
			char c10 = buffer[startIndex++];
			if (c10 != 'B' && c10 != 'b')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'J' && c9 != 'j')
			{
				break;
			}
			char c = buffer[startIndex++];
			if (c != 'E' && c != 'e')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'C' && c2 != 'c')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if ((c3 != 'T' && c3 != 't') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'N' && c4 != 'n')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'A' && c5 != 'a')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 == 'M' || c6 == 'm')
			{
				char c7 = buffer[startIndex++];
				if (c7 == 'E' || c7 == 'e')
				{
					result = StringToken.ObjectName;
				}
			}
			break;
		}
		case 'Q':
		case 'q':
		{
			char c10 = buffer[startIndex++];
			if (c10 != 'U' && c10 != 'u')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'E' && c9 != 'e')
			{
				break;
			}
			char c = buffer[startIndex++];
			if (c != 'R' && c != 'r')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if ((c2 != 'Y' && c2 != 'y') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'S' && c3 != 's')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'T' && c4 != 't')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'O' && c5 != 'o')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 == 'R' || c6 == 'r')
			{
				char c7 = buffer[startIndex++];
				if (c7 == 'E' || c7 == 'e')
				{
					result = StringToken.QueryStore;
				}
			}
			break;
		}
		case 'R':
		case 'r':
			switch (buffer[startIndex++])
			{
			case 'E':
			case 'e':
				switch (buffer[startIndex++])
				{
				case 'C':
				case 'c':
				{
					char c = buffer[startIndex++];
					if (c != 'O' && c != 'o')
					{
						break;
					}
					char c2 = buffer[startIndex++];
					if (c2 != 'N' && c2 != 'n')
					{
						break;
					}
					char c3 = buffer[startIndex++];
					if (c3 != 'F' && c3 != 'f')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'I' && c4 != 'i')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'G' && c5 != 'g')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'U' && c6 != 'u')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 == 'R' || c7 == 'r')
					{
						char c8 = buffer[startIndex++];
						if (c8 == 'E' || c8 == 'e')
						{
							result = StringToken.Reconfigure;
						}
					}
					break;
				}
				case 'J':
				case 'j':
				{
					char c = buffer[startIndex++];
					if (c != 'E' && c != 'e')
					{
						break;
					}
					char c2 = buffer[startIndex++];
					if (c2 != 'C' && c2 != 'c')
					{
						break;
					}
					char c3 = buffer[startIndex++];
					if ((c3 != 'T' && c3 != 't') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'T' && c4 != 't')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'Y' && c5 != 'y')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 == 'P' || c6 == 'p')
					{
						char c7 = buffer[startIndex++];
						if (c7 == 'E' || c7 == 'e')
						{
							result = StringToken.RejectType;
						}
					}
					break;
				}
				case 'P':
				case 'p':
				{
					char c = buffer[startIndex++];
					if (c != 'L' && c != 'l')
					{
						break;
					}
					char c2 = buffer[startIndex++];
					if (c2 != 'I' && c2 != 'i')
					{
						break;
					}
					char c3 = buffer[startIndex++];
					if (c3 != 'C' && c3 != 'c')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'A' && c4 != 'a')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'T' && c5 != 't')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'I' && c6 != 'i')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 == 'O' || c7 == 'o')
					{
						char c8 = buffer[startIndex++];
						if (c8 == 'N' || c8 == 'n')
						{
							result = StringToken.Replication;
						}
					}
					break;
				}
				}
				break;
			case 'O':
			case 'o':
			{
				char c9 = buffer[startIndex++];
				if (c9 != 'U' && c9 != 'u')
				{
					break;
				}
				char c = buffer[startIndex++];
				if (c != 'N' && c != 'n')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if ((c2 != 'D' && c2 != 'd') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'R' && c3 != 'r')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'O' && c4 != 'o')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'B' && c5 != 'b')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 == 'I' || c6 == 'i')
				{
					char c7 = buffer[startIndex++];
					if (c7 == 'N' || c7 == 'n')
					{
						result = StringToken.RoundRobin;
					}
				}
				break;
			}
			}
			break;
		case 'S':
		case 's':
			switch (buffer[startIndex++])
			{
			case 'C':
			case 'c':
			{
				char c9 = buffer[startIndex++];
				if (c9 != 'H' && c9 != 'h')
				{
					break;
				}
				char c = buffer[startIndex++];
				if (c != 'E' && c != 'e')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'M' && c2 != 'm')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if ((c3 != 'A' && c3 != 'a') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'N' && c4 != 'n')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'A' && c5 != 'a')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 == 'M' || c6 == 'm')
				{
					char c7 = buffer[startIndex++];
					if (c7 == 'E' || c7 == 'e')
					{
						result = StringToken.SchemaName;
					}
				}
				break;
			}
			case 'E':
			case 'e':
				switch (buffer[startIndex++])
				{
				case 'N':
				case 'n':
				{
					char c = buffer[startIndex++];
					if (c != 'S' && c != 's')
					{
						break;
					}
					char c2 = buffer[startIndex++];
					if (c2 != 'I' && c2 != 'i')
					{
						break;
					}
					char c3 = buffer[startIndex++];
					if (c3 != 'T' && c3 != 't')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'I' && c4 != 'i')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'V' && c5 != 'v')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'I' && c6 != 'i')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 == 'T' || c7 == 't')
					{
						char c8 = buffer[startIndex++];
						if (c8 == 'Y' || c8 == 'y')
						{
							result = StringToken.Sensitivity;
						}
					}
					break;
				}
				case 'R':
				case 'r':
				{
					char c = buffer[startIndex++];
					if (c != 'V' && c != 'v')
					{
						break;
					}
					char c2 = buffer[startIndex++];
					if (c2 != 'I' && c2 != 'i')
					{
						break;
					}
					char c3 = buffer[startIndex++];
					if (c3 != 'C' && c3 != 'c')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'E' && c4 != 'e')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'N' && c5 != 'n')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'A' && c6 != 'a')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 == 'M' || c7 == 'm')
					{
						char c8 = buffer[startIndex++];
						if (c8 == 'E' || c8 == 'e')
						{
							result = StringToken.Servicename;
						}
					}
					break;
				}
				}
				break;
			case 'O':
			case 'o':
			{
				char c9 = buffer[startIndex++];
				if (c9 != 'R' && c9 != 'r')
				{
					break;
				}
				char c = buffer[startIndex++];
				if (c != 'T' && c != 't')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'E' && c2 != 'e')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if ((c3 != 'D' && c3 != 'd') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'D' && c4 != 'd')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'A' && c5 != 'a')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 == 'T' || c6 == 't')
				{
					char c7 = buffer[startIndex++];
					if (c7 == 'A' || c7 == 'a')
					{
						result = StringToken.SortedData;
					}
				}
				break;
			}
			case 'Q':
			case 'q':
			{
				char c9 = buffer[startIndex++];
				if ((c9 != 'L' && c9 != 'l') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c = buffer[startIndex++];
				if (c != 'V' && c != 'v')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'A' && c2 != 'a')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'R' && c3 != 'r')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'I' && c4 != 'i')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'A' && c5 != 'a')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 == 'N' || c6 == 'n')
				{
					char c7 = buffer[startIndex++];
					if (c7 == 'T' || c7 == 't')
					{
						result = StringToken.SqlVariant;
					}
				}
				break;
			}
			case 'Y':
			case 'y':
			{
				char c9 = buffer[startIndex++];
				if (c9 != 'S' && c9 != 's')
				{
					break;
				}
				char c = buffer[startIndex++];
				if (c != 'T' && c != 't')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'E' && c2 != 'e')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if ((c3 != 'M' && c3 != 'm') || buffer[startIndex++] != '_')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case 'T':
				case 't':
				{
					char c5 = buffer[startIndex++];
					if (c5 != 'I' && c5 != 'i')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 == 'M' || c6 == 'm')
					{
						char c7 = buffer[startIndex++];
						if (c7 == 'E' || c7 == 'e')
						{
							result = StringToken.SystemTime;
						}
					}
					break;
				}
				case 'U':
				case 'u':
				{
					char c5 = buffer[startIndex++];
					if (c5 != 'S' && c5 != 's')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 == 'E' || c6 == 'e')
					{
						char c7 = buffer[startIndex++];
						if (c7 == 'R' || c7 == 'r')
						{
							result = StringToken.SystemUser;
						}
					}
					break;
				}
				}
				break;
			}
			}
			break;
		case 'T':
		case 't':
			switch (buffer[startIndex++])
			{
			case 'A':
			case 'a':
			{
				char c9 = buffer[startIndex++];
				if (c9 != 'B' && c9 != 'b')
				{
					break;
				}
				char c = buffer[startIndex++];
				if (c != 'L' && c != 'l')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'E' && c2 != 'e')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'S' && c3 != 's')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'A' && c4 != 'a')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'M' && c5 != 'm')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'P' && c6 != 'p')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 == 'L' || c7 == 'l')
				{
					char c8 = buffer[startIndex++];
					if (c8 == 'E' || c8 == 'e')
					{
						result = StringToken.TableSample;
					}
				}
				break;
			}
			case 'O':
			case 'o':
			{
				char c9 = buffer[startIndex++];
				if (c9 != 'T' && c9 != 't')
				{
					break;
				}
				char c = buffer[startIndex++];
				if (c != 'A' && c != 'a')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if ((c2 != 'L' && c2 != 'l') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'W' && c3 != 'w')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'R' && c4 != 'r')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'I' && c5 != 'i')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 == 'T' || c6 == 't')
				{
					char c7 = buffer[startIndex++];
					if (c7 == 'E' || c7 == 'e')
					{
						result = StringToken.Total_write;
					}
				}
				break;
			}
			case 'R':
			case 'r':
				switch (buffer[startIndex++])
				{
				case 'A':
				case 'a':
				{
					char c = buffer[startIndex++];
					if (c != 'N' && c != 'n')
					{
						break;
					}
					char c2 = buffer[startIndex++];
					if (c2 != 'S' && c2 != 's')
					{
						break;
					}
					char c3 = buffer[startIndex++];
					if (c3 != 'A' && c3 != 'a')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'C' && c4 != 'c')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'T' && c5 != 't')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'I' && c6 != 'i')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 == 'O' || c7 == 'o')
					{
						char c8 = buffer[startIndex++];
						if (c8 == 'N' || c8 == 'n')
						{
							result = StringToken.Transaction;
						}
					}
					break;
				}
				case 'Y':
				case 'y':
				{
					if (buffer[startIndex++] != '_')
					{
						break;
					}
					char c = buffer[startIndex++];
					if (c != 'C' && c != 'c')
					{
						break;
					}
					char c2 = buffer[startIndex++];
					if (c2 != 'O' && c2 != 'o')
					{
						break;
					}
					char c3 = buffer[startIndex++];
					if (c3 != 'N' && c3 != 'n')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'V' && c4 != 'v')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'E' && c5 != 'e')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 == 'R' || c6 == 'r')
					{
						char c7 = buffer[startIndex++];
						if (c7 == 'T' || c7 == 't')
						{
							result = StringToken.TryConvert;
						}
					}
					break;
				}
				}
				break;
			}
			break;
		}
		return result;
	}

	private static StringToken Find_Len12(string buffer, int startIndex)
	{
		StringToken result = StringToken.None;
		switch (buffer[startIndex++])
		{
		case 'A':
		case 'a':
			switch (buffer[startIndex++])
			{
			case 'L':
			case 'l':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'T' && c2 != 't')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'E' && c3 != 'e')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if ((c4 != 'R' && c4 != 'r') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'S' && c5 != 's')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'C' && c6 != 'c')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'H' && c7 != 'h')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'E' && c8 != 'e')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 == 'M' || c9 == 'm')
				{
					char c10 = buffer[startIndex++];
					if (c10 == 'A' || c10 == 'a')
					{
						result = StringToken.AlterSchema;
					}
				}
				break;
			}
			case 'V':
			case 'v':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'A' && c2 != 'a')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'I' && c3 != 'i')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'L' && c4 != 'l')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'A' && c5 != 'a')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'B' && c6 != 'b')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'I' && c7 != 'i')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'L' && c8 != 'l')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'I' && c9 != 'i')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 == 'T' || c10 == 't')
				{
					char c11 = buffer[startIndex++];
					if (c11 == 'Y' || c11 == 'y')
					{
						result = StringToken.Availability;
					}
				}
				break;
			}
			}
			break;
		case 'B':
		case 'b':
			switch (buffer[startIndex++])
			{
			case 'I':
			case 'i':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'N' && c2 != 'n')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if ((c3 != 'D' && c3 != 'd') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'D' && c4 != 'd')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'E' && c5 != 'e')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'F' && c6 != 'f')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'A' && c7 != 'a')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'U' && c8 != 'u')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 == 'L' || c9 == 'l')
				{
					char c10 = buffer[startIndex++];
					if (c10 == 'T' || c10 == 't')
					{
						result = StringToken.BindDefault;
					}
				}
				break;
			}
			case 'L':
			case 'l':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'O' && c2 != 'o')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if ((c3 != 'B' && c3 != 'b') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'S' && c4 != 's')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'T' && c5 != 't')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'O' && c6 != 'o')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'R' && c7 != 'r')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'A' && c8 != 'a')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 == 'G' || c9 == 'g')
				{
					char c10 = buffer[startIndex++];
					if (c10 == 'E' || c10 == 'e')
					{
						result = StringToken.BlobStorage;
					}
				}
				break;
			}
			case 'U':
			case 'u':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'C' && c2 != 'c')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'K' && c3 != 'k')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'E' && c4 != 'e')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if ((c5 != 'T' && c5 != 't') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'C' && c6 != 'c')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'O' && c7 != 'o')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'U' && c8 != 'u')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 == 'N' || c9 == 'n')
				{
					char c10 = buffer[startIndex++];
					if (c10 == 'T' || c10 == 't')
					{
						result = StringToken.BucketCount;
					}
				}
				break;
			}
			}
			break;
		case 'C':
		case 'c':
			switch (buffer[startIndex++])
			{
			case 'H':
			case 'h':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'E' && c2 != 'e')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'C' && c3 != 'c')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if ((c4 != 'K' && c4 != 'k') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'P' && c5 != 'p')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'O' && c6 != 'o')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'L' && c7 != 'l')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'I' && c8 != 'i')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 == 'C' || c9 == 'c')
				{
					char c10 = buffer[startIndex++];
					if (c10 == 'Y' || c10 == 'y')
					{
						result = StringToken.CheckPolicy;
					}
				}
				break;
			}
			case 'O':
			case 'o':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'N' && c2 != 'n')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'V' && c3 != 'v')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'E' && c4 != 'e')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'R' && c5 != 'r')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'S' && c6 != 's')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'A' && c7 != 'a')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'T' && c8 != 't')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'I' && c9 != 'i')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 == 'O' || c10 == 'o')
				{
					char c11 = buffer[startIndex++];
					if (c11 == 'N' || c11 == 'n')
					{
						result = StringToken.Conversation;
					}
				}
				break;
			}
			case 'R':
			case 'r':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'E' && c2 != 'e')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'A' && c3 != 'a')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'T' && c4 != 't')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if ((c5 != 'E' && c5 != 'e') || buffer[startIndex++] != '_')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case 'I':
				case 'i':
				{
					char c7 = buffer[startIndex++];
					if (c7 != 'N' && c7 != 'n')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'D' && c8 != 'd')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 == 'E' || c9 == 'e')
					{
						char c10 = buffer[startIndex++];
						if (c10 == 'X' || c10 == 'x')
						{
							result = StringToken.CreateIndex;
						}
					}
					break;
				}
				case 'L':
				case 'l':
				{
					char c7 = buffer[startIndex++];
					if (c7 != 'O' && c7 != 'o')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'G' && c8 != 'g')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 == 'I' || c9 == 'i')
					{
						char c10 = buffer[startIndex++];
						if (c10 == 'N' || c10 == 'n')
						{
							result = StringToken.CreateLogin;
						}
					}
					break;
				}
				case 'Q':
				case 'q':
				{
					char c7 = buffer[startIndex++];
					if (c7 != 'U' && c7 != 'u')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'E' && c8 != 'e')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 == 'U' || c9 == 'u')
					{
						char c10 = buffer[startIndex++];
						if (c10 == 'E' || c10 == 'e')
						{
							result = StringToken.CreateQueue;
						}
					}
					break;
				}
				case 'R':
				case 'r':
				{
					char c7 = buffer[startIndex++];
					if (c7 != 'O' && c7 != 'o')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'U' && c8 != 'u')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 == 'T' || c9 == 't')
					{
						char c10 = buffer[startIndex++];
						if (c10 == 'E' || c10 == 'e')
						{
							result = StringToken.CreateRoute;
						}
					}
					break;
				}
				case 'T':
				case 't':
				{
					char c7 = buffer[startIndex++];
					if (c7 != 'A' && c7 != 'a')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'B' && c8 != 'b')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 == 'L' || c9 == 'l')
					{
						char c10 = buffer[startIndex++];
						if (c10 == 'E' || c10 == 'e')
						{
							result = StringToken.CreateTable;
						}
					}
					break;
				}
				}
				break;
			}
			case 'U':
			case 'u':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'R' && c2 != 'r')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'R' && c3 != 'r')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'E' && c4 != 'e')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'N' && c5 != 'n')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if ((c6 != 'T' && c6 != 't') || buffer[startIndex++] != '_')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case 'D':
				case 'd':
				{
					char c8 = buffer[startIndex++];
					if (c8 != 'A' && c8 != 'a')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 == 'T' || c9 == 't')
					{
						char c10 = buffer[startIndex++];
						if (c10 == 'E' || c10 == 'e')
						{
							result = StringToken.CurrentDate;
						}
					}
					break;
				}
				case 'T':
				case 't':
				{
					char c8 = buffer[startIndex++];
					if (c8 != 'I' && c8 != 'i')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 == 'M' || c9 == 'm')
					{
						char c10 = buffer[startIndex++];
						if (c10 == 'E' || c10 == 'e')
						{
							result = StringToken.CurrentTime;
						}
					}
					break;
				}
				case 'U':
				case 'u':
				{
					char c8 = buffer[startIndex++];
					if (c8 != 'S' && c8 != 's')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 == 'E' || c9 == 'e')
					{
						char c10 = buffer[startIndex++];
						if (c10 == 'R' || c10 == 'r')
						{
							result = StringToken.CurrentUser;
						}
					}
					break;
				}
				}
				break;
			}
			}
			break;
		case 'D':
		case 'd':
			switch (buffer[startIndex++])
			{
			case 'I':
			case 'i':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'S' && c2 != 's')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'T' && c3 != 't')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'R' && c4 != 'r')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'I' && c5 != 'i')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'B' && c6 != 'b')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'U' && c7 != 'u')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'T' && c8 != 't')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'I' && c9 != 'i')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 == 'O' || c10 == 'o')
				{
					char c11 = buffer[startIndex++];
					if (c11 == 'N' || c11 == 'n')
					{
						result = StringToken.Distribution;
					}
				}
				break;
			}
			case 'R':
			case 'r':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'O' && c2 != 'o')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if ((c3 != 'P' && c3 != 'p') || buffer[startIndex++] != '_')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case 'D':
				case 'd':
				{
					char c5 = buffer[startIndex++];
					if (c5 != 'E' && c5 != 'e')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'F' && c6 != 'f')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'A' && c7 != 'a')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'U' && c8 != 'u')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 == 'L' || c9 == 'l')
					{
						char c10 = buffer[startIndex++];
						if (c10 == 'T' || c10 == 't')
						{
							result = StringToken.DropDefault;
						}
					}
					break;
				}
				case 'M':
				case 'm':
				{
					char c5 = buffer[startIndex++];
					if (c5 != 'E' && c5 != 'e')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'S' && c6 != 's')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'S' && c7 != 's')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'A' && c8 != 'a')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 == 'G' || c9 == 'g')
					{
						char c10 = buffer[startIndex++];
						if (c10 == 'E' || c10 == 'e')
						{
							result = StringToken.DropMessage;
						}
					}
					break;
				}
				case 'S':
				case 's':
					switch (buffer[startIndex++])
					{
					case 'E':
					case 'e':
					{
						char c6 = buffer[startIndex++];
						if (c6 != 'R' && c6 != 'r')
						{
							break;
						}
						char c7 = buffer[startIndex++];
						if (c7 != 'V' && c7 != 'v')
						{
							break;
						}
						char c8 = buffer[startIndex++];
						if (c8 != 'I' && c8 != 'i')
						{
							break;
						}
						char c9 = buffer[startIndex++];
						if (c9 == 'C' || c9 == 'c')
						{
							char c10 = buffer[startIndex++];
							if (c10 == 'E' || c10 == 'e')
							{
								result = StringToken.DropService;
							}
						}
						break;
					}
					case 'Y':
					case 'y':
					{
						char c6 = buffer[startIndex++];
						if (c6 != 'N' && c6 != 'n')
						{
							break;
						}
						char c7 = buffer[startIndex++];
						if (c7 != 'O' && c7 != 'o')
						{
							break;
						}
						char c8 = buffer[startIndex++];
						if (c8 != 'N' && c8 != 'n')
						{
							break;
						}
						char c9 = buffer[startIndex++];
						if (c9 == 'Y' || c9 == 'y')
						{
							char c10 = buffer[startIndex++];
							if (c10 == 'M' || c10 == 'm')
							{
								result = StringToken.DropSynonym;
							}
						}
						break;
					}
					}
					break;
				case 'T':
				case 't':
				{
					char c5 = buffer[startIndex++];
					if (c5 != 'R' && c5 != 'r')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'I' && c6 != 'i')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'G' && c7 != 'g')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'G' && c8 != 'g')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 == 'E' || c9 == 'e')
					{
						char c10 = buffer[startIndex++];
						if (c10 == 'R' || c10 == 'r')
						{
							result = StringToken.DropTrigger;
						}
					}
					break;
				}
				}
				break;
			}
			}
			break;
		case 'F':
		case 'f':
			switch (buffer[startIndex++])
			{
			case 'A':
			case 'a':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'S' && c2 != 's')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'T' && c3 != 't')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case '_':
				{
					char c5 = buffer[startIndex++];
					if (c5 != 'F' && c5 != 'f')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'O' && c6 != 'o')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'R' && c7 != 'r')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'W' && c8 != 'w')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'A' && c9 != 'a')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 == 'R' || c10 == 'r')
					{
						char c11 = buffer[startIndex++];
						if (c11 == 'D' || c11 == 'd')
						{
							result = StringToken.FastForward;
						}
					}
					break;
				}
				case 'F':
				case 'f':
				{
					char c5 = buffer[startIndex++];
					if (c5 != 'I' && c5 != 'i')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'R' && c6 != 'r')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'S' && c7 != 's')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'T' && c8 != 't')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'R' && c9 != 'r')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 == 'O' || c10 == 'o')
					{
						char c11 = buffer[startIndex++];
						if (c11 == 'W' || c11 == 'w')
						{
							result = StringToken.FastFirstRow;
						}
					}
					break;
				}
				}
				break;
			}
			case 'E':
			case 'e':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'T' && c2 != 't')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'C' && c3 != 'c')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if ((c4 != 'H' && c4 != 'h') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'S' && c5 != 's')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'T' && c6 != 't')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'A' && c7 != 'a')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'T' && c8 != 't')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 == 'U' || c9 == 'u')
				{
					char c10 = buffer[startIndex++];
					if (c10 == 'S' || c10 == 's')
					{
						result = StringToken.Fetch_status;
					}
				}
				break;
			}
			case 'O':
			case 'o':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'R' && c2 != 'r')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'W' && c3 != 'w')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'A' && c4 != 'a')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'R' && c5 != 'r')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if ((c6 != 'D' && c6 != 'd') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'O' && c7 != 'o')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'N' && c8 != 'n')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 == 'L' || c9 == 'l')
				{
					char c10 = buffer[startIndex++];
					if (c10 == 'Y' || c10 == 'y')
					{
						result = StringToken.ForwardOnly;
					}
				}
				break;
			}
			}
			break;
		case 'G':
		case 'g':
		{
			char c = buffer[startIndex++];
			if (c != 'R' && c != 'r')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'A' && c2 != 'a')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'N' && c3 != 'n')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if ((c4 != 'T' && c4 != 't') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'S' && c5 != 's')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'E' && c6 != 'e')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'R' && c7 != 'r')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'V' && c8 != 'v')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 == 'E' || c9 == 'e')
			{
				char c10 = buffer[startIndex++];
				if (c10 == 'R' || c10 == 'r')
				{
					result = StringToken.GrantServer;
				}
			}
			break;
		}
		case 'K':
		case 'k':
		{
			char c = buffer[startIndex++];
			if (c != 'E' && c != 'e')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'E' && c2 != 'e')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'P' && c3 != 'p')
			{
				break;
			}
			switch (buffer[startIndex++])
			{
			case 'D':
			case 'd':
			{
				char c5 = buffer[startIndex++];
				if (c5 != 'E' && c5 != 'e')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'F' && c6 != 'f')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'A' && c7 != 'a')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'U' && c8 != 'u')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'L' && c9 != 'l')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 == 'T' || c10 == 't')
				{
					char c11 = buffer[startIndex++];
					if (c11 == 'S' || c11 == 's')
					{
						result = StringToken.KeepDefaults;
					}
				}
				break;
			}
			case 'I':
			case 'i':
			{
				char c5 = buffer[startIndex++];
				if (c5 != 'D' && c5 != 'd')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'E' && c6 != 'e')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'N' && c7 != 'n')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'T' && c8 != 't')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'I' && c9 != 'i')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 == 'T' || c10 == 't')
				{
					char c11 = buffer[startIndex++];
					if (c11 == 'Y' || c11 == 'y')
					{
						result = StringToken.KeepIdentity;
					}
				}
				break;
			}
			}
			break;
		}
		case 'L':
		case 'l':
		{
			char c = buffer[startIndex++];
			if (c != 'O' && c != 'o')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'C' && c2 != 'c')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if ((c3 != 'K' && c3 != 'k') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'T' && c4 != 't')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'I' && c5 != 'i')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'M' && c6 != 'm')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'E' && c7 != 'e')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'O' && c8 != 'o')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 == 'U' || c9 == 'u')
			{
				char c10 = buffer[startIndex++];
				if (c10 == 'T' || c10 == 't')
				{
					result = StringToken.Lock_timeout;
				}
			}
			break;
		}
		case 'M':
		case 'm':
		{
			char c = buffer[startIndex++];
			if (c != 'A' && c != 'a')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if ((c2 != 'X' && c2 != 'x') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'D' && c3 != 'd')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'U' && c4 != 'u')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'R' && c5 != 'r')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'A' && c6 != 'a')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'T' && c7 != 't')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'I' && c8 != 'i')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 == 'O' || c9 == 'o')
			{
				char c10 = buffer[startIndex++];
				if (c10 == 'N' || c10 == 'n')
				{
					result = StringToken.MaxDuration;
				}
			}
			break;
		}
		case 'N':
		case 'n':
		{
			char c = buffer[startIndex++];
			if (c != 'O' && c != 'o')
			{
				break;
			}
			switch (buffer[startIndex++])
			{
			case 'N':
			case 'n':
			{
				char c3 = buffer[startIndex++];
				if (c3 != 'C' && c3 != 'c')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'L' && c4 != 'l')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'U' && c5 != 'u')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'S' && c6 != 's')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'T' && c7 != 't')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'E' && c8 != 'e')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'R' && c9 != 'r')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 == 'E' || c10 == 'e')
				{
					char c11 = buffer[startIndex++];
					if (c11 == 'D' || c11 == 'd')
					{
						result = StringToken.NonClustered;
					}
				}
				break;
			}
			case 'T':
			case 't':
			{
				char c3 = buffer[startIndex++];
				if (c3 != 'I' && c3 != 'i')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'F' && c4 != 'f')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'I' && c5 != 'i')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'C' && c6 != 'c')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'A' && c7 != 'a')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'T' && c8 != 't')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'I' && c9 != 'i')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 == 'O' || c10 == 'o')
				{
					char c11 = buffer[startIndex++];
					if (c11 == 'N' || c11 == 'n')
					{
						result = StringToken.Notification;
					}
				}
				break;
			}
			}
			break;
		}
		case 'P':
		case 'p':
		{
			char c = buffer[startIndex++];
			if (c != 'R' && c != 'r')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'I' && c2 != 'i')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'M' && c3 != 'm')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'A' && c4 != 'a')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'R' && c5 != 'r')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if ((c6 != 'Y' && c6 != 'y') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'R' && c7 != 'r')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'O' && c8 != 'o')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 == 'L' || c9 == 'l')
			{
				char c10 = buffer[startIndex++];
				if (c10 == 'E' || c10 == 'e')
				{
					result = StringToken.PrimaryRole;
				}
			}
			break;
		}
		case 'R':
		case 'r':
		{
			char c = buffer[startIndex++];
			if (c != 'E' && c != 'e')
			{
				break;
			}
			switch (buffer[startIndex++])
			{
			case 'F':
			case 'f':
			{
				char c3 = buffer[startIndex++];
				if (c3 != 'R' && c3 != 'r')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'E' && c4 != 'e')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'S' && c5 != 's')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if ((c6 != 'H' && c6 != 'h') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'M' && c7 != 'm')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'O' && c8 != 'o')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 == 'D' || c9 == 'd')
				{
					char c10 = buffer[startIndex++];
					if (c10 == 'E' || c10 == 'e')
					{
						result = StringToken.RefreshMode;
					}
				}
				break;
			}
			case 'J':
			case 'j':
			{
				char c3 = buffer[startIndex++];
				if (c3 != 'E' && c3 != 'e')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'C' && c4 != 'c')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if ((c5 != 'T' && c5 != 't') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'V' && c6 != 'v')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'A' && c7 != 'a')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'L' && c8 != 'l')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 == 'U' || c9 == 'u')
				{
					char c10 = buffer[startIndex++];
					if (c10 == 'E' || c10 == 'e')
					{
						result = StringToken.RejectValue;
					}
				}
				break;
			}
			}
			break;
		}
		case 'S':
		case 's':
			switch (buffer[startIndex++])
			{
			case 'C':
			case 'c':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'R' && c2 != 'r')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'O' && c3 != 'o')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'L' && c4 != 'l')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if ((c5 != 'L' && c5 != 'l') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'L' && c6 != 'l')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'O' && c7 != 'o')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'C' && c8 != 'c')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 == 'K' || c9 == 'k')
				{
					char c10 = buffer[startIndex++];
					if (c10 == 'S' || c10 == 's')
					{
						result = StringToken.ScrollLocks;
					}
				}
				break;
			}
			case 'E':
			case 'e':
				switch (buffer[startIndex++])
				{
				case 'C':
				case 'c':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'U' && c3 != 'u')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'R' && c4 != 'r')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'I' && c5 != 'i')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'T' && c6 != 't')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if ((c7 != 'Y' && c7 != 'y') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'L' && c8 != 'l')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 == 'O' || c9 == 'o')
					{
						char c10 = buffer[startIndex++];
						if (c10 == 'G' || c10 == 'g')
						{
							result = StringToken.SecurityLog;
						}
					}
					break;
				}
				case 'R':
				case 'r':
					switch (buffer[startIndex++])
					{
					case 'D':
					case 'd':
					{
						char c4 = buffer[startIndex++];
						if ((c4 != 'E' && c4 != 'e') || buffer[startIndex++] != '_')
						{
							break;
						}
						char c5 = buffer[startIndex++];
						if (c5 != 'M' && c5 != 'm')
						{
							break;
						}
						char c6 = buffer[startIndex++];
						if (c6 != 'E' && c6 != 'e')
						{
							break;
						}
						char c7 = buffer[startIndex++];
						if (c7 != 'T' && c7 != 't')
						{
							break;
						}
						char c8 = buffer[startIndex++];
						if (c8 != 'H' && c8 != 'h')
						{
							break;
						}
						char c9 = buffer[startIndex++];
						if (c9 == 'O' || c9 == 'o')
						{
							char c10 = buffer[startIndex++];
							if (c10 == 'D' || c10 == 'd')
							{
								result = StringToken.SerdeMethod;
							}
						}
						break;
					}
					case 'I':
					case 'i':
					{
						char c4 = buffer[startIndex++];
						if (c4 != 'A' && c4 != 'a')
						{
							break;
						}
						char c5 = buffer[startIndex++];
						if (c5 != 'L' && c5 != 'l')
						{
							break;
						}
						char c6 = buffer[startIndex++];
						if (c6 != 'I' && c6 != 'i')
						{
							break;
						}
						char c7 = buffer[startIndex++];
						if (c7 != 'Z' && c7 != 'z')
						{
							break;
						}
						char c8 = buffer[startIndex++];
						if (c8 != 'A' && c8 != 'a')
						{
							break;
						}
						char c9 = buffer[startIndex++];
						if (c9 != 'B' && c9 != 'b')
						{
							break;
						}
						char c10 = buffer[startIndex++];
						if (c10 == 'L' || c10 == 'l')
						{
							char c11 = buffer[startIndex++];
							if (c11 == 'E' || c11 == 'e')
							{
								result = StringToken.Serializable;
							}
						}
						break;
					}
					}
					break;
				case 'S':
				case 's':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'S' && c3 != 's')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'I' && c4 != 'i')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'O' && c5 != 'o')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if ((c6 != 'N' && c6 != 'n') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'U' && c7 != 'u')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'S' && c8 != 's')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 == 'E' || c9 == 'e')
					{
						char c10 = buffer[startIndex++];
						if (c10 == 'R' || c10 == 'r')
						{
							result = StringToken.SessionUser;
						}
					}
					break;
				}
				}
				break;
			}
			break;
		case 'T':
		case 't':
			switch (buffer[startIndex++])
			{
			case 'E':
			case 'e':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'X' && c2 != 'x')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'T' && c3 != 't')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'I' && c4 != 'i')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'M' && c5 != 'm')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'A' && c6 != 'a')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'G' && c7 != 'g')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if ((c8 != 'E' && c8 != 'e') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 == 'O' || c9 == 'o')
				{
					char c10 = buffer[startIndex++];
					if (c10 == 'N' || c10 == 'n')
					{
						result = StringToken.TextImageOn;
					}
				}
				break;
			}
			case 'O':
			case 'o':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'T' && c2 != 't')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'A' && c3 != 'a')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if ((c4 != 'L' && c4 != 'l') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'E' && c5 != 'e')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'R' && c6 != 'r')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'R' && c7 != 'r')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'O' && c8 != 'o')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 == 'R' || c9 == 'r')
				{
					char c10 = buffer[startIndex++];
					if (c10 == 'S' || c10 == 's')
					{
						result = StringToken.Total_errors;
					}
				}
				break;
			}
			case 'Y':
			case 'y':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'P' && c2 != 'p')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if ((c3 != 'E' && c3 != 'e') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'W' && c4 != 'w')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'A' && c5 != 'a')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'R' && c6 != 'r')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'N' && c7 != 'n')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'I' && c8 != 'i')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 == 'N' || c9 == 'n')
				{
					char c10 = buffer[startIndex++];
					if (c10 == 'G' || c10 == 'g')
					{
						result = StringToken.TypeWarning;
					}
				}
				break;
			}
			}
			break;
		case 'V':
		case 'v':
		{
			char c = buffer[startIndex++];
			if (c != 'A' && c != 'a')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'R' && c2 != 'r')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'B' && c3 != 'b')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'I' && c4 != 'i')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'N' && c5 != 'n')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'A' && c6 != 'a')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'R' && c7 != 'r')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'Y' && c8 != 'y')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'M' && c9 != 'm')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 == 'A' || c10 == 'a')
			{
				char c11 = buffer[startIndex++];
				if (c11 == 'X' || c11 == 'x')
				{
					result = StringToken.VarBinaryMax;
				}
			}
			break;
		}
		case 'W':
		case 'w':
		{
			char c = buffer[startIndex++];
			if (c != 'H' && c != 'h')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'E' && c2 != 'e')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'R' && c3 != 'r')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'E' && c4 != 'e')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'C' && c5 != 'c')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'U' && c6 != 'u')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'R' && c7 != 'r')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'R' && c8 != 'r')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'E' && c9 != 'e')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 == 'N' || c10 == 'n')
			{
				char c11 = buffer[startIndex++];
				if (c11 == 'T' || c11 == 't')
				{
					result = StringToken.WhereCurrent;
				}
			}
			break;
		}
		}
		return result;
	}

	private static StringToken Find_Len13(string buffer, int startIndex)
	{
		StringToken result = StringToken.None;
		switch (buffer[startIndex++])
		{
		case 'A':
		case 'a':
			switch (buffer[startIndex++])
			{
			case 'L':
			case 'l':
				switch (buffer[startIndex++])
				{
				case 'L':
				case 'l':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'O' && c3 != 'o')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if ((c4 != 'W' && c4 != 'w') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'D' && c5 != 'd')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'U' && c6 != 'u')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if ((c7 != 'P' && c7 != 'p') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'R' && c8 != 'r')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 == 'O' || c9 == 'o')
					{
						char c10 = buffer[startIndex++];
						if (c10 == 'W' || c10 == 'w')
						{
							result = StringToken.AllowDupRow;
						}
					}
					break;
				}
				case 'T':
				case 't':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'E' && c3 != 'e')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if ((c4 != 'R' && c4 != 'r') || buffer[startIndex++] != '_')
					{
						break;
					}
					switch (buffer[startIndex++])
					{
					case 'M':
					case 'm':
					{
						char c6 = buffer[startIndex++];
						if (c6 != 'E' && c6 != 'e')
						{
							break;
						}
						char c7 = buffer[startIndex++];
						if (c7 != 'S' && c7 != 's')
						{
							break;
						}
						char c8 = buffer[startIndex++];
						if (c8 != 'S' && c8 != 's')
						{
							break;
						}
						char c9 = buffer[startIndex++];
						if (c9 != 'A' && c9 != 'a')
						{
							break;
						}
						char c10 = buffer[startIndex++];
						if (c10 == 'G' || c10 == 'g')
						{
							char c11 = buffer[startIndex++];
							if (c11 == 'E' || c11 == 'e')
							{
								result = StringToken.AlterMessage;
							}
						}
						break;
					}
					case 'S':
					case 's':
					{
						char c6 = buffer[startIndex++];
						if (c6 != 'E' && c6 != 'e')
						{
							break;
						}
						char c7 = buffer[startIndex++];
						if (c7 != 'R' && c7 != 'r')
						{
							break;
						}
						char c8 = buffer[startIndex++];
						if (c8 != 'V' && c8 != 'v')
						{
							break;
						}
						char c9 = buffer[startIndex++];
						if (c9 != 'I' && c9 != 'i')
						{
							break;
						}
						char c10 = buffer[startIndex++];
						if (c10 == 'C' || c10 == 'c')
						{
							char c11 = buffer[startIndex++];
							if (c11 == 'E' || c11 == 'e')
							{
								result = StringToken.AlterService;
							}
						}
						break;
					}
					case 'T':
					case 't':
					{
						char c6 = buffer[startIndex++];
						if (c6 != 'R' && c6 != 'r')
						{
							break;
						}
						char c7 = buffer[startIndex++];
						if (c7 != 'I' && c7 != 'i')
						{
							break;
						}
						char c8 = buffer[startIndex++];
						if (c8 != 'G' && c8 != 'g')
						{
							break;
						}
						char c9 = buffer[startIndex++];
						if (c9 != 'G' && c9 != 'g')
						{
							break;
						}
						char c10 = buffer[startIndex++];
						if (c10 == 'E' || c10 == 'e')
						{
							char c11 = buffer[startIndex++];
							if (c11 == 'R' || c11 == 'r')
							{
								result = StringToken.AlterTrigger;
							}
						}
						break;
					}
					}
					break;
				}
				}
				break;
			case 'U':
			case 'u':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'T' && c2 != 't')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'H' && c3 != 'h')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'O' && c4 != 'o')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'R' && c5 != 'r')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'I' && c6 != 'i')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'Z' && c7 != 'z')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'A' && c8 != 'a')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'T' && c9 != 't')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'I' && c10 != 'i')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 == 'O' || c11 == 'o')
				{
					char c12 = buffer[startIndex++];
					if (c12 == 'N' || c12 == 'n')
					{
						result = StringToken.Authorization;
					}
				}
				break;
			}
			}
			break;
		case 'B':
		case 'b':
		{
			char c = buffer[startIndex++];
			if (c != 'U' && c != 'u')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'I' && c2 != 'i')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'L' && c3 != 'l')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if ((c4 != 'D' && c4 != 'd') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'R' && c5 != 'r')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'E' && c6 != 'e')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'P' && c7 != 'p')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'L' && c8 != 'l')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'I' && c9 != 'i')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 == 'C' || c10 == 'c')
			{
				char c11 = buffer[startIndex++];
				if (c11 == 'A' || c11 == 'a')
				{
					result = StringToken.BuildReplica;
				}
			}
			break;
		}
		case 'C':
		case 'c':
			switch (buffer[startIndex++])
			{
			case 'O':
			case 'o':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'N' && c2 != 'n')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case 'F':
				case 'f':
				{
					char c4 = buffer[startIndex++];
					if (c4 != 'I' && c4 != 'i')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'G' && c5 != 'g')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'U' && c6 != 'u')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'R' && c7 != 'r')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'A' && c8 != 'a')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'T' && c9 != 't')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'I' && c10 != 'i')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 == 'O' || c11 == 'o')
					{
						char c12 = buffer[startIndex++];
						if (c12 == 'N' || c12 == 'n')
						{
							result = StringToken.Configuration;
						}
					}
					break;
				}
				case 'T':
				case 't':
				{
					char c4 = buffer[startIndex++];
					if (c4 != 'A' && c4 != 'a')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'I' && c5 != 'i')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'N' && c6 != 'n')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'S' && c7 != 's')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'T' && c8 != 't')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'A' && c9 != 'a')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'B' && c10 != 'b')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 == 'L' || c11 == 'l')
					{
						char c12 = buffer[startIndex++];
						if (c12 == 'E' || c12 == 'e')
						{
							result = StringToken.ContainsTable;
						}
					}
					break;
				}
				}
				break;
			}
			case 'R':
			case 'r':
				switch (buffer[startIndex++])
				{
				case 'E':
				case 'e':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'A' && c3 != 'a')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'T' && c4 != 't')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if ((c5 != 'E' && c5 != 'e') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'S' && c6 != 's')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'C' && c7 != 'c')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'H' && c8 != 'h')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'E' && c9 != 'e')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 == 'M' || c10 == 'm')
					{
						char c11 = buffer[startIndex++];
						if (c11 == 'A' || c11 == 'a')
						{
							result = StringToken.CreateSchema;
						}
					}
					break;
				}
				case 'Y':
				case 'y':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'P' && c3 != 'p')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'T' && c4 != 't')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'O' && c5 != 'o')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'G' && c6 != 'g')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'R' && c7 != 'r')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'A' && c8 != 'a')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'P' && c9 != 'p')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'H' && c10 != 'h')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 == 'I' || c11 == 'i')
					{
						char c12 = buffer[startIndex++];
						if (c12 == 'C' || c12 == 'c')
						{
							result = StringToken.Cryptographic;
						}
					}
					break;
				}
				}
				break;
			}
			break;
		case 'D':
		case 'd':
			switch (buffer[startIndex++])
			{
			case 'A':
			case 'a':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'T' && c2 != 't')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case 'A':
				case 'a':
					switch (buffer[startIndex++])
					{
					case '_':
					{
						char c5 = buffer[startIndex++];
						if (c5 != 'D' && c5 != 'd')
						{
							break;
						}
						char c6 = buffer[startIndex++];
						if (c6 != 'E' && c6 != 'e')
						{
							break;
						}
						char c7 = buffer[startIndex++];
						if (c7 != 'L' && c7 != 'l')
						{
							break;
						}
						char c8 = buffer[startIndex++];
						if (c8 != 'E' && c8 != 'e')
						{
							break;
						}
						char c9 = buffer[startIndex++];
						if (c9 != 'T' && c9 != 't')
						{
							break;
						}
						char c10 = buffer[startIndex++];
						if (c10 != 'I' && c10 != 'i')
						{
							break;
						}
						char c11 = buffer[startIndex++];
						if (c11 == 'O' || c11 == 'o')
						{
							char c12 = buffer[startIndex++];
							if (c12 == 'N' || c12 == 'n')
							{
								result = StringToken.DataDeletion;
							}
						}
						break;
					}
					case 'B':
					case 'b':
					{
						char c5 = buffer[startIndex++];
						if (c5 != 'A' && c5 != 'a')
						{
							break;
						}
						char c6 = buffer[startIndex++];
						if (c6 != 'S' && c6 != 's')
						{
							break;
						}
						char c7 = buffer[startIndex++];
						if ((c7 != 'E' && c7 != 'e') || buffer[startIndex++] != '_')
						{
							break;
						}
						char c8 = buffer[startIndex++];
						if (c8 != 'N' && c8 != 'n')
						{
							break;
						}
						char c9 = buffer[startIndex++];
						if (c9 != 'A' && c9 != 'a')
						{
							break;
						}
						char c10 = buffer[startIndex++];
						if (c10 == 'M' || c10 == 'm')
						{
							char c11 = buffer[startIndex++];
							if (c11 == 'E' || c11 == 'e')
							{
								result = StringToken.DatabaseName;
							}
						}
						break;
					}
					}
					break;
				case 'E':
				case 'e':
				{
					char c4 = buffer[startIndex++];
					if (c4 != 'F' && c4 != 'f')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'R' && c5 != 'r')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'O' && c6 != 'o')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'M' && c7 != 'm')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'P' && c8 != 'p')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'A' && c9 != 'a')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'R' && c10 != 'r')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 == 'T' || c11 == 't')
					{
						char c12 = buffer[startIndex++];
						if (c12 == 'S' || c12 == 's')
						{
							result = StringToken.DateFromParts;
						}
					}
					break;
				}
				}
				break;
			}
			case 'E':
			case 'e':
				switch (buffer[startIndex++])
				{
				case 'L':
				case 'l':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'I' && c3 != 'i')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'M' && c4 != 'm')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'I' && c5 != 'i')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'T' && c6 != 't')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'E' && c7 != 'e')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'D' && c8 != 'd')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'T' && c9 != 't')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'E' && c10 != 'e')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 == 'X' || c11 == 'x')
					{
						char c12 = buffer[startIndex++];
						if (c12 == 'T' || c12 == 't')
						{
							result = StringToken.DelimitedText;
						}
					}
					break;
				}
				case 'N':
				case 'n':
				{
					char c3 = buffer[startIndex++];
					if ((c3 != 'Y' && c3 != 'y') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'D' && c4 != 'd')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'A' && c5 != 'a')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'T' && c6 != 't')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'A' && c7 != 'a')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'B' && c8 != 'b')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'A' && c9 != 'a')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 == 'S' || c10 == 's')
					{
						char c11 = buffer[startIndex++];
						if (c11 == 'E' || c11 == 'e')
						{
							result = StringToken.DenyDatabase;
						}
					}
					break;
				}
				case 'S':
				case 's':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'I' && c3 != 'i')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'R' && c4 != 'r')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'E' && c5 != 'e')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if ((c6 != 'D' && c6 != 'd') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'S' && c7 != 's')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'T' && c8 != 't')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'A' && c9 != 'a')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 == 'T' || c10 == 't')
					{
						char c11 = buffer[startIndex++];
						if (c11 == 'E' || c11 == 'e')
						{
							result = StringToken.DesiredState;
						}
					}
					break;
				}
				}
				break;
			case 'R':
			case 'r':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'O' && c2 != 'o')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if ((c3 != 'P' && c3 != 'p') || buffer[startIndex++] != '_')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case 'A':
				case 'a':
				{
					char c5 = buffer[startIndex++];
					if (c5 != 'S' && c5 != 's')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'S' && c6 != 's')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'E' && c7 != 'e')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'M' && c8 != 'm')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'B' && c9 != 'b')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 == 'L' || c10 == 'l')
					{
						char c11 = buffer[startIndex++];
						if (c11 == 'Y' || c11 == 'y')
						{
							result = StringToken.DropAssembly;
						}
					}
					break;
				}
				case 'C':
				case 'c':
				{
					char c5 = buffer[startIndex++];
					if (c5 != 'O' && c5 != 'o')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'N' && c6 != 'n')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'T' && c7 != 't')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'R' && c8 != 'r')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'A' && c9 != 'a')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 == 'C' || c10 == 'c')
					{
						char c11 = buffer[startIndex++];
						if (c11 == 'T' || c11 == 't')
						{
							result = StringToken.DropContract;
						}
					}
					break;
				}
				case 'D':
				case 'd':
				{
					char c5 = buffer[startIndex++];
					if (c5 != 'A' && c5 != 'a')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'T' && c6 != 't')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'A' && c7 != 'a')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'B' && c8 != 'b')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'A' && c9 != 'a')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 == 'S' || c10 == 's')
					{
						char c11 = buffer[startIndex++];
						if (c11 == 'E' || c11 == 'e')
						{
							result = StringToken.DropDatabase;
						}
					}
					break;
				}
				case 'E':
				case 'e':
					switch (buffer[startIndex++])
					{
					case 'N':
					case 'n':
					{
						char c6 = buffer[startIndex++];
						if (c6 != 'D' && c6 != 'd')
						{
							break;
						}
						char c7 = buffer[startIndex++];
						if (c7 != 'P' && c7 != 'p')
						{
							break;
						}
						char c8 = buffer[startIndex++];
						if (c8 != 'O' && c8 != 'o')
						{
							break;
						}
						char c9 = buffer[startIndex++];
						if (c9 != 'I' && c9 != 'i')
						{
							break;
						}
						char c10 = buffer[startIndex++];
						if (c10 == 'N' || c10 == 'n')
						{
							char c11 = buffer[startIndex++];
							if (c11 == 'T' || c11 == 't')
							{
								result = StringToken.DropEndpoint;
							}
						}
						break;
					}
					case 'X':
					case 'x':
					{
						char c6 = buffer[startIndex++];
						if (c6 != 'I' && c6 != 'i')
						{
							break;
						}
						char c7 = buffer[startIndex++];
						if (c7 != 'S' && c7 != 's')
						{
							break;
						}
						char c8 = buffer[startIndex++];
						if (c8 != 'T' && c8 != 't')
						{
							break;
						}
						char c9 = buffer[startIndex++];
						if (c9 != 'I' && c9 != 'i')
						{
							break;
						}
						char c10 = buffer[startIndex++];
						if (c10 == 'N' || c10 == 'n')
						{
							char c11 = buffer[startIndex++];
							if (c11 == 'G' || c11 == 'g')
							{
								result = StringToken.DropExisting;
							}
						}
						break;
					}
					}
					break;
				case 'F':
				case 'f':
				{
					char c5 = buffer[startIndex++];
					if (c5 != 'U' && c5 != 'u')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'N' && c6 != 'n')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'C' && c7 != 'c')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'T' && c8 != 't')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'I' && c9 != 'i')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 == 'O' || c10 == 'o')
					{
						char c11 = buffer[startIndex++];
						if (c11 == 'N' || c11 == 'n')
						{
							result = StringToken.DropFunction;
						}
					}
					break;
				}
				}
				break;
			}
			}
			break;
		case 'F':
		case 'f':
			switch (buffer[startIndex++])
			{
			case 'I':
			case 'i':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'L' && c2 != 'l')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case 'E':
				case 'e':
				{
					char c4 = buffer[startIndex++];
					if (c4 != 'S' && c4 != 's')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'T' && c5 != 't')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'R' && c6 != 'r')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'E' && c7 != 'e')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'A' && c8 != 'a')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if ((c9 != 'M' && c9 != 'm') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 == 'O' || c10 == 'o')
					{
						char c11 = buffer[startIndex++];
						if (c11 == 'N' || c11 == 'n')
						{
							result = StringToken.FileStreamOn;
						}
					}
					break;
				}
				case 'T':
				case 't':
				{
					char c4 = buffer[startIndex++];
					if (c4 != 'E' && c4 != 'e')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if ((c5 != 'R' && c5 != 'r') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'C' && c6 != 'c')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'O' && c7 != 'o')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'L' && c8 != 'l')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'U' && c9 != 'u')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 == 'M' || c10 == 'm')
					{
						char c11 = buffer[startIndex++];
						if (c11 == 'N' || c11 == 'n')
						{
							result = StringToken.FilterColumn;
						}
					}
					break;
				}
				}
				break;
			}
			case 'R':
			case 'r':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'E' && c2 != 'e')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'E' && c3 != 'e')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'T' && c4 != 't')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'E' && c5 != 'e')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'X' && c6 != 'x')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'T' && c7 != 't')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'T' && c8 != 't')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'A' && c9 != 'a')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'B' && c10 != 'b')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 == 'L' || c11 == 'l')
				{
					char c12 = buffer[startIndex++];
					if (c12 == 'E' || c12 == 'e')
					{
						result = StringToken.FreeTextTable;
					}
				}
				break;
			}
			}
			break;
		case 'H':
		case 'h':
		{
			char c = buffer[startIndex++];
			if (c != 'I' && c != 'i')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'S' && c2 != 's')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'T' && c3 != 't')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'O' && c4 != 'o')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'R' && c5 != 'r')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if ((c6 != 'Y' && c6 != 'y') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'T' && c7 != 't')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'A' && c8 != 'a')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'B' && c9 != 'b')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 == 'L' || c10 == 'l')
			{
				char c11 = buffer[startIndex++];
				if (c11 == 'E' || c11 == 'e')
				{
					result = StringToken.HistoryTable;
				}
			}
			break;
		}
		case 'I':
		case 'i':
		{
			char c = buffer[startIndex++];
			if (c != 'N' && c != 'n')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'P' && c2 != 'p')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'U' && c3 != 'u')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if ((c4 != 'T' && c4 != 't') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'O' && c5 != 'o')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'P' && c6 != 'p')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'T' && c7 != 't')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'I' && c8 != 'i')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'O' && c9 != 'o')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 == 'N' || c10 == 'n')
			{
				char c11 = buffer[startIndex++];
				if (c11 == 'S' || c11 == 's')
				{
					result = StringToken.InputOptions;
				}
			}
			break;
		}
		case 'M':
		case 'm':
		{
			char c = buffer[startIndex++];
			if (c != 'A' && c != 'a')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if ((c2 != 'X' && c2 != 'x') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'P' && c3 != 'p')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'R' && c4 != 'r')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'E' && c5 != 'e')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'C' && c6 != 'c')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'I' && c7 != 'i')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'S' && c8 != 's')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'I' && c9 != 'i')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 == 'O' || c10 == 'o')
			{
				char c11 = buffer[startIndex++];
				if (c11 == 'N' || c11 == 'n')
				{
					result = StringToken.Max_precision;
				}
			}
			break;
		}
		case 'P':
		case 'p':
		{
			char c = buffer[startIndex++];
			if (c != 'A' && c != 'a')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'C' && c2 != 'c')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'K' && c3 != 'k')
			{
				break;
			}
			switch (buffer[startIndex++])
			{
			case '_':
			{
				char c5 = buffer[startIndex++];
				if (c5 != 'R' && c5 != 'r')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'E' && c6 != 'e')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'C' && c7 != 'c')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'E' && c8 != 'e')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'I' && c9 != 'i')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'V' && c10 != 'v')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 == 'E' || c11 == 'e')
				{
					char c12 = buffer[startIndex++];
					if (c12 == 'D' || c12 == 'd')
					{
						result = StringToken.Pack_received;
					}
				}
				break;
			}
			case 'E':
			case 'e':
			{
				char c5 = buffer[startIndex++];
				if ((c5 != 'T' && c5 != 't') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'E' && c6 != 'e')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'R' && c7 != 'r')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'R' && c8 != 'r')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'O' && c9 != 'o')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 == 'R' || c10 == 'r')
				{
					char c11 = buffer[startIndex++];
					if (c11 == 'S' || c11 == 's')
					{
						result = StringToken.Packet_errors;
					}
				}
				break;
			}
			}
			break;
		}
		case 'R':
		case 'r':
		{
			char c = buffer[startIndex++];
			if (c != 'E' && c != 'e')
			{
				break;
			}
			switch (buffer[startIndex++])
			{
			case 'A':
			case 'a':
			{
				char c3 = buffer[startIndex++];
				if (c3 != 'D' && c3 != 'd')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'C' && c4 != 'c')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'O' && c5 != 'o')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'M' && c6 != 'm')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'M' && c7 != 'm')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'I' && c8 != 'i')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'T' && c9 != 't')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'T' && c10 != 't')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 == 'E' || c11 == 'e')
				{
					char c12 = buffer[startIndex++];
					if (c12 == 'D' || c12 == 'd')
					{
						result = StringToken.ReadCommitted;
					}
				}
				break;
			}
			case 'V':
			case 'v':
			{
				char c3 = buffer[startIndex++];
				if (c3 != 'O' && c3 != 'o')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'K' && c4 != 'k')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if ((c5 != 'E' && c5 != 'e') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'S' && c6 != 's')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'E' && c7 != 'e')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'R' && c8 != 'r')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'V' && c9 != 'v')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 == 'E' || c10 == 'e')
				{
					char c11 = buffer[startIndex++];
					if (c11 == 'R' || c11 == 'r')
					{
						result = StringToken.RevokeServer;
					}
				}
				break;
			}
			}
			break;
		}
		case 'S':
		case 's':
			switch (buffer[startIndex++])
			{
			case 'C':
			case 'c':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'H' && c2 != 'h')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'E' && c3 != 'e')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'M' && c4 != 'm')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'A' && c5 != 'a')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'B' && c6 != 'b')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'I' && c7 != 'i')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'N' && c8 != 'n')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'D' && c9 != 'd')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'I' && c10 != 'i')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 == 'N' || c11 == 'n')
				{
					char c12 = buffer[startIndex++];
					if (c12 == 'G' || c12 == 'g')
					{
						result = StringToken.SchemaBinding;
					}
				}
				break;
			}
			case 'H':
			case 'h':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'A' && c2 != 'a')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'R' && c3 != 'r')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'E' && c4 != 'e')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if ((c5 != 'D' && c5 != 'd') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'M' && c6 != 'm')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'E' && c7 != 'e')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'M' && c8 != 'm')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'O' && c9 != 'o')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 == 'R' || c10 == 'r')
				{
					char c11 = buffer[startIndex++];
					if (c11 == 'Y' || c11 == 'y')
					{
						result = StringToken.SharedMemory;
					}
				}
				break;
			}
			case 'M':
			case 'm':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'A' && c2 != 'a')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'L' && c3 != 'l')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'L' && c4 != 'l')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'D' && c5 != 'd')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'A' && c6 != 'a')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'T' && c7 != 't')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'E' && c8 != 'e')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'T' && c9 != 't')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'I' && c10 != 'i')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 == 'M' || c11 == 'm')
				{
					char c12 = buffer[startIndex++];
					if (c12 == 'E' || c12 == 'e')
					{
						result = StringToken.SmallDateTime;
					}
				}
				break;
			}
			case 'P':
			case 'p':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'E' && c2 != 'e')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'C' && c3 != 'c')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'I' && c4 != 'i')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'F' && c5 != 'f')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'I' && c6 != 'i')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'C' && c7 != 'c')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'A' && c8 != 'a')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'T' && c9 != 't')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'I' && c10 != 'i')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 == 'O' || c11 == 'o')
				{
					char c12 = buffer[startIndex++];
					if (c12 == 'N' || c12 == 'n')
					{
						result = StringToken.Specification;
					}
				}
				break;
			}
			}
			break;
		case 'V':
		case 'v':
		{
			char c = buffer[startIndex++];
			if (c != 'I' && c != 'i')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'E' && c2 != 'e')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if ((c3 != 'W' && c3 != 'w') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'M' && c4 != 'm')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'E' && c5 != 'e')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'T' && c6 != 't')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'A' && c7 != 'a')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'D' && c8 != 'd')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'A' && c9 != 'a')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 == 'T' || c10 == 't')
			{
				char c11 = buffer[startIndex++];
				if (c11 == 'A' || c11 == 'a')
				{
					result = StringToken.ViewMetadata;
				}
			}
			break;
		}
		case 'X':
		case 'x':
		{
			char c = buffer[startIndex++];
			if (c != 'M' && c != 'm')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'L' && c2 != 'l')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'N' && c3 != 'n')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'A' && c4 != 'a')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'M' && c5 != 'm')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'E' && c6 != 'e')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'S' && c7 != 's')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'P' && c8 != 'p')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'A' && c9 != 'a')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'C' && c10 != 'c')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 == 'E' || c11 == 'e')
			{
				char c12 = buffer[startIndex++];
				if (c12 == 'S' || c12 == 's')
				{
					result = StringToken.XmlNamespaces;
				}
			}
			break;
		}
		}
		return result;
	}

	private static StringToken Find_Len14(string buffer, int startIndex)
	{
		StringToken result = StringToken.None;
		switch (buffer[startIndex++])
		{
		case 'A':
		case 'a':
		{
			char c = buffer[startIndex++];
			if (c != 'L' && c != 'l')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'T' && c2 != 't')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'E' && c3 != 'e')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if ((c4 != 'R' && c4 != 'r') || buffer[startIndex++] != '_')
			{
				break;
			}
			switch (buffer[startIndex++])
			{
			case 'A':
			case 'a':
			{
				char c6 = buffer[startIndex++];
				if (c6 != 'S' && c6 != 's')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'S' && c7 != 's')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'E' && c8 != 'e')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'M' && c9 != 'm')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'B' && c10 != 'b')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 == 'L' || c11 == 'l')
				{
					char c12 = buffer[startIndex++];
					if (c12 == 'Y' || c12 == 'y')
					{
						result = StringToken.AlterAssembly;
					}
				}
				break;
			}
			case 'D':
			case 'd':
			{
				char c6 = buffer[startIndex++];
				if (c6 != 'A' && c6 != 'a')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'T' && c7 != 't')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'A' && c8 != 'a')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'B' && c9 != 'b')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'A' && c10 != 'a')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 == 'S' || c11 == 's')
				{
					char c12 = buffer[startIndex++];
					if (c12 == 'E' || c12 == 'e')
					{
						result = StringToken.AlterDatabase;
					}
				}
				break;
			}
			case 'E':
			case 'e':
			{
				char c6 = buffer[startIndex++];
				if (c6 != 'N' && c6 != 'n')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'D' && c7 != 'd')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'P' && c8 != 'p')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'O' && c9 != 'o')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'I' && c10 != 'i')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 == 'N' || c11 == 'n')
				{
					char c12 = buffer[startIndex++];
					if (c12 == 'T' || c12 == 't')
					{
						result = StringToken.AlterEndpoint;
					}
				}
				break;
			}
			case 'F':
			case 'f':
			{
				char c6 = buffer[startIndex++];
				if (c6 != 'U' && c6 != 'u')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'N' && c7 != 'n')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'C' && c8 != 'c')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'T' && c9 != 't')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'I' && c10 != 'i')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 == 'O' || c11 == 'o')
				{
					char c12 = buffer[startIndex++];
					if (c12 == 'N' || c12 == 'n')
					{
						result = StringToken.AlterFunction;
					}
				}
				break;
			}
			case 'I':
			case 'i':
			{
				char c6 = buffer[startIndex++];
				if (c6 != 'N' && c6 != 'n')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'S' && c7 != 's')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'T' && c8 != 't')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'A' && c9 != 'a')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'N' && c10 != 'n')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 == 'C' || c11 == 'c')
				{
					char c12 = buffer[startIndex++];
					if (c12 == 'E' || c12 == 'e')
					{
						result = StringToken.AlterInstance;
					}
				}
				break;
			}
			}
			break;
		}
		case 'C':
		case 'c':
			switch (buffer[startIndex++])
			{
			case 'L':
			case 'l':
				switch (buffer[startIndex++])
				{
				case 'A':
				case 'a':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'S' && c3 != 's')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'S' && c4 != 's')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'I' && c5 != 'i')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'F' && c6 != 'f')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'I' && c7 != 'i')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'C' && c8 != 'c')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'A' && c9 != 'a')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'T' && c10 != 't')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 != 'I' && c11 != 'i')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if (c12 == 'O' || c12 == 'o')
					{
						char c13 = buffer[startIndex++];
						if (c13 == 'N' || c13 == 'n')
						{
							result = StringToken.Classification;
						}
					}
					break;
				}
				case 'E':
				case 'e':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'A' && c3 != 'a')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'N' && c4 != 'n')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'U' && c5 != 'u')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if ((c6 != 'P' && c6 != 'p') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'P' && c7 != 'p')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'O' && c8 != 'o')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'L' && c9 != 'l')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'I' && c10 != 'i')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 == 'C' || c11 == 'c')
					{
						char c12 = buffer[startIndex++];
						if (c12 == 'Y' || c12 == 'y')
						{
							result = StringToken.CleanupPolicy;
						}
					}
					break;
				}
				}
				break;
			case 'R':
			case 'r':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'E' && c2 != 'e')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'A' && c3 != 'a')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'T' && c4 != 't')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if ((c5 != 'E' && c5 != 'e') || buffer[startIndex++] != '_')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case 'D':
				case 'd':
				{
					char c7 = buffer[startIndex++];
					if (c7 != 'E' && c7 != 'e')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'F' && c8 != 'f')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'A' && c9 != 'a')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'U' && c10 != 'u')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 == 'L' || c11 == 'l')
					{
						char c12 = buffer[startIndex++];
						if (c12 == 'T' || c12 == 't')
						{
							result = StringToken.CreateDefault;
						}
					}
					break;
				}
				case 'M':
				case 'm':
				{
					char c7 = buffer[startIndex++];
					if (c7 != 'E' && c7 != 'e')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'S' && c8 != 's')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'S' && c9 != 's')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'A' && c10 != 'a')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 == 'G' || c11 == 'g')
					{
						char c12 = buffer[startIndex++];
						if (c12 == 'E' || c12 == 'e')
						{
							result = StringToken.CreateMessage;
						}
					}
					break;
				}
				case 'S':
				case 's':
					switch (buffer[startIndex++])
					{
					case 'E':
					case 'e':
					{
						char c8 = buffer[startIndex++];
						if (c8 != 'R' && c8 != 'r')
						{
							break;
						}
						char c9 = buffer[startIndex++];
						if (c9 != 'V' && c9 != 'v')
						{
							break;
						}
						char c10 = buffer[startIndex++];
						if (c10 != 'I' && c10 != 'i')
						{
							break;
						}
						char c11 = buffer[startIndex++];
						if (c11 == 'C' || c11 == 'c')
						{
							char c12 = buffer[startIndex++];
							if (c12 == 'E' || c12 == 'e')
							{
								result = StringToken.CreateService;
							}
						}
						break;
					}
					case 'Y':
					case 'y':
					{
						char c8 = buffer[startIndex++];
						if (c8 != 'N' && c8 != 'n')
						{
							break;
						}
						char c9 = buffer[startIndex++];
						if (c9 != 'O' && c9 != 'o')
						{
							break;
						}
						char c10 = buffer[startIndex++];
						if (c10 != 'N' && c10 != 'n')
						{
							break;
						}
						char c11 = buffer[startIndex++];
						if (c11 == 'Y' || c11 == 'y')
						{
							char c12 = buffer[startIndex++];
							if (c12 == 'M' || c12 == 'm')
							{
								result = StringToken.CreateSynonym;
							}
						}
						break;
					}
					}
					break;
				case 'T':
				case 't':
				{
					char c7 = buffer[startIndex++];
					if (c7 != 'R' && c7 != 'r')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'I' && c8 != 'i')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'G' && c9 != 'g')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'G' && c10 != 'g')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 == 'E' || c11 == 'e')
					{
						char c12 = buffer[startIndex++];
						if (c12 == 'R' || c12 == 'r')
						{
							result = StringToken.CreateTrigger;
						}
					}
					break;
				}
				}
				break;
			}
			}
			break;
		case 'D':
		case 'd':
			switch (buffer[startIndex++])
			{
			case 'A':
			case 'a':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'T' && c2 != 't')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case 'A':
				case 'a':
				{
					if (buffer[startIndex++] != '_')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'R' && c4 != 'r')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'E' && c5 != 'e')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'T' && c6 != 't')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'E' && c7 != 'e')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'N' && c8 != 'n')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'T' && c9 != 't')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'I' && c10 != 'i')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 == 'O' || c11 == 'o')
					{
						char c12 = buffer[startIndex++];
						if (c12 == 'N' || c12 == 'n')
						{
							result = StringToken.DataRetention;
						}
					}
					break;
				}
				case 'E':
				case 'e':
				{
					char c4 = buffer[startIndex++];
					if (c4 != 'T' && c4 != 't')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'I' && c5 != 'i')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'M' && c6 != 'm')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'E' && c7 != 'e')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'O' && c8 != 'o')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'F' && c9 != 'f')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'F' && c10 != 'f')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 != 'S' && c11 != 's')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if (c12 == 'E' || c12 == 'e')
					{
						char c13 = buffer[startIndex++];
						if (c13 == 'T' || c13 == 't')
						{
							result = StringToken.DateTimeOffset;
						}
					}
					break;
				}
				}
				break;
			}
			case 'D':
			case 'd':
			{
				char c2 = buffer[startIndex++];
				if ((c2 != 'L' && c2 != 'l') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'S' && c3 != 's')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'S' && c4 != 's')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if ((c5 != 'B' && c5 != 'b') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'E' && c6 != 'e')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'V' && c7 != 'v')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'E' && c8 != 'e')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'N' && c9 != 'n')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 == 'T' || c10 == 't')
				{
					char c11 = buffer[startIndex++];
					if (c11 == 'S' || c11 == 's')
					{
						result = StringToken.DdlSsbEvents;
					}
				}
				break;
			}
			case 'E':
			case 'e':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'F' && c2 != 'f')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'A' && c3 != 'a')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'U' && c4 != 'u')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'L' && c5 != 'l')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if ((c6 != 'T' && c6 != 't') || buffer[startIndex++] != '_')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case 'L':
				case 'l':
				{
					char c8 = buffer[startIndex++];
					if (c8 != 'A' && c8 != 'a')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'N' && c9 != 'n')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'G' && c10 != 'g')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 == 'I' || c11 == 'i')
					{
						char c12 = buffer[startIndex++];
						if (c12 == 'D' || c12 == 'd')
						{
							result = StringToken.Default_langid;
						}
					}
					break;
				}
				case 'S':
				case 's':
				{
					char c8 = buffer[startIndex++];
					if (c8 != 'C' && c8 != 'c')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'H' && c9 != 'h')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'E' && c10 != 'e')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 == 'M' || c11 == 'm')
					{
						char c12 = buffer[startIndex++];
						if (c12 == 'A' || c12 == 'a')
						{
							result = StringToken.DefaultSchema;
						}
					}
					break;
				}
				}
				break;
			}
			case 'I':
			case 'i':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'R' && c2 != 'r')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'E' && c3 != 'e')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'C' && c4 != 'c')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'T' && c5 != 't')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'O' && c6 != 'o')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'R' && c7 != 'r')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if ((c8 != 'Y' && c8 != 'y') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'N' && c9 != 'n')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'A' && c10 != 'a')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 == 'M' || c11 == 'm')
				{
					char c12 = buffer[startIndex++];
					if (c12 == 'E' || c12 == 'e')
					{
						result = StringToken.DirectoryName;
					}
				}
				break;
			}
			case 'R':
			case 'r':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'O' && c2 != 'o')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if ((c3 != 'P' && c3 != 'p') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'P' && c4 != 'p')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'R' && c5 != 'r')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'O' && c6 != 'o')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'C' && c7 != 'c')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'E' && c8 != 'e')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'D' && c9 != 'd')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'U' && c10 != 'u')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 == 'R' || c11 == 'r')
				{
					char c12 = buffer[startIndex++];
					if (c12 == 'E' || c12 == 'e')
					{
						result = StringToken.DropProcedure;
					}
				}
				break;
			}
			}
			break;
		case 'F':
		case 'f':
		{
			char c = buffer[startIndex++];
			if (c != 'O' && c != 'o')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'R' && c2 != 'r')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'M' && c3 != 'm')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'A' && c4 != 'a')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if ((c5 != 'T' && c5 != 't') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'O' && c6 != 'o')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'P' && c7 != 'p')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'T' && c8 != 't')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'I' && c9 != 'i')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'O' && c10 != 'o')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 == 'N' || c11 == 'n')
			{
				char c12 = buffer[startIndex++];
				if (c12 == 'S' || c12 == 's')
				{
					result = StringToken.FormatOptions;
				}
			}
			break;
		}
		case 'G':
		case 'g':
		{
			char c = buffer[startIndex++];
			if (c != 'R' && c != 'r')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'A' && c2 != 'a')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'N' && c3 != 'n')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if ((c4 != 'T' && c4 != 't') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'D' && c5 != 'd')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'A' && c6 != 'a')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'T' && c7 != 't')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'A' && c8 != 'a')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'B' && c9 != 'b')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'A' && c10 != 'a')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 == 'S' || c11 == 's')
			{
				char c12 = buffer[startIndex++];
				if (c12 == 'E' || c12 == 'e')
				{
					result = StringToken.GrantDatabase;
				}
			}
			break;
		}
		case 'I':
		case 'i':
		{
			char c = buffer[startIndex++];
			if (c != 'G' && c != 'g')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'N' && c2 != 'n')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'O' && c3 != 'o')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'R' && c4 != 'r')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if ((c5 != 'E' && c5 != 'e') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'D' && c6 != 'd')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'U' && c7 != 'u')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if ((c8 != 'P' && c8 != 'p') || buffer[startIndex++] != '_')
			{
				break;
			}
			switch (buffer[startIndex++])
			{
			case 'K':
			case 'k':
			{
				char c10 = buffer[startIndex++];
				if (c10 == 'E' || c10 == 'e')
				{
					char c11 = buffer[startIndex++];
					if (c11 == 'Y' || c11 == 'y')
					{
						result = StringToken.IgnoreDupKey;
					}
				}
				break;
			}
			case 'R':
			case 'r':
			{
				char c10 = buffer[startIndex++];
				if (c10 == 'O' || c10 == 'o')
				{
					char c11 = buffer[startIndex++];
					if (c11 == 'W' || c11 == 'w')
					{
						result = StringToken.IgnoreDupRow;
					}
				}
				break;
			}
			}
			break;
		}
		case 'K':
		case 'k':
		{
			char c = buffer[startIndex++];
			if (c != 'E' && c != 'e')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if ((c2 != 'Y' && c2 != 'y') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'S' && c3 != 's')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'T' && c4 != 't')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'O' && c5 != 'o')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'R' && c6 != 'r')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if ((c7 != 'E' && c7 != 'e') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'T' && c8 != 't')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'Y' && c9 != 'y')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 == 'P' || c10 == 'p')
			{
				char c11 = buffer[startIndex++];
				if (c11 == 'E' || c11 == 'e')
				{
					result = StringToken.KeyStoreType;
				}
			}
			break;
		}
		case 'L':
		case 'l':
		{
			char c = buffer[startIndex++];
			if (c != 'O' && c != 'o')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if ((c2 != 'B' && c2 != 'b') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'C' && c3 != 'c')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'O' && c4 != 'o')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'M' && c5 != 'm')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'P' && c6 != 'p')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'A' && c7 != 'a')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'C' && c8 != 'c')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'T' && c9 != 't')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'I' && c10 != 'i')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 == 'O' || c11 == 'o')
			{
				char c12 = buffer[startIndex++];
				if (c12 == 'N' || c12 == 'n')
				{
					result = StringToken.LobCompaction;
				}
			}
			break;
		}
		case 'O':
		case 'o':
			switch (buffer[startIndex++])
			{
			case 'P':
			case 'p':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'E' && c2 != 'e')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case 'N':
				case 'n':
				{
					char c4 = buffer[startIndex++];
					if (c4 != 'D' && c4 != 'd')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'A' && c5 != 'a')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'T' && c6 != 't')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'A' && c7 != 'a')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'S' && c8 != 's')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'O' && c9 != 'o')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'U' && c10 != 'u')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 != 'R' && c11 != 'r')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if (c12 == 'C' || c12 == 'c')
					{
						char c13 = buffer[startIndex++];
						if (c13 == 'E' || c13 == 'e')
						{
							result = StringToken.OpenDatasource;
						}
					}
					break;
				}
				case 'R':
				case 'r':
				{
					char c4 = buffer[startIndex++];
					if (c4 != 'A' && c4 != 'a')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'T' && c5 != 't')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'I' && c6 != 'i')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'O' && c7 != 'o')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if ((c8 != 'N' && c8 != 'n') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'M' && c9 != 'm')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'O' && c10 != 'o')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 == 'D' || c11 == 'd')
					{
						char c12 = buffer[startIndex++];
						if (c12 == 'E' || c12 == 'e')
						{
							result = StringToken.OperationMode;
						}
					}
					break;
				}
				}
				break;
			}
			case 'U':
			case 'u':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'T' && c2 != 't')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'P' && c3 != 'p')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'U' && c4 != 'u')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if ((c5 != 'T' && c5 != 't') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'O' && c6 != 'o')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'P' && c7 != 'p')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'T' && c8 != 't')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'I' && c9 != 'i')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'O' && c10 != 'o')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 == 'N' || c11 == 'n')
				{
					char c12 = buffer[startIndex++];
					if (c12 == 'S' || c12 == 's')
					{
						result = StringToken.OutputOptions;
					}
				}
				break;
			}
			}
			break;
		case 'R':
		case 'r':
			switch (buffer[startIndex++])
			{
			case 'E':
			case 'e':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'P' && c2 != 'p')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'E' && c3 != 'e')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'A' && c4 != 'a')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'T' && c5 != 't')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'A' && c6 != 'a')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'B' && c7 != 'b')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'L' && c8 != 'l')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'E' && c9 != 'e')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'R' && c10 != 'r')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'E' && c11 != 'e')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 == 'A' || c12 == 'a')
				{
					char c13 = buffer[startIndex++];
					if (c13 == 'D' || c13 == 'd')
					{
						result = StringToken.RepeatableRead;
					}
				}
				break;
			}
			case 'O':
			case 'o':
			{
				char c2 = buffer[startIndex++];
				if ((c2 != 'W' && c2 != 'w') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'T' && c3 != 't')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'E' && c4 != 'e')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'R' && c5 != 'r')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'M' && c6 != 'm')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'I' && c7 != 'i')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'N' && c8 != 'n')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'A' && c9 != 'a')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'T' && c10 != 't')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 == 'O' || c11 == 'o')
				{
					char c12 = buffer[startIndex++];
					if (c12 == 'R' || c12 == 'r')
					{
						result = StringToken.RowTerminator;
					}
				}
				break;
			}
			}
			break;
		case 'S':
		case 's':
			switch (buffer[startIndex++])
			{
			case 'E':
			case 'e':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'C' && c2 != 'c')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'O' && c3 != 'o')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'N' && c4 != 'n')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'D' && c5 != 'd')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'A' && c6 != 'a')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'R' && c7 != 'r')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if ((c8 != 'Y' && c8 != 'y') || buffer[startIndex++] != '_')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case 'R':
				case 'r':
				{
					char c10 = buffer[startIndex++];
					if (c10 != 'O' && c10 != 'o')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 == 'L' || c11 == 'l')
					{
						char c12 = buffer[startIndex++];
						if (c12 == 'E' || c12 == 'e')
						{
							result = StringToken.SecondaryRole;
						}
					}
					break;
				}
				case 'T':
				case 't':
				{
					char c10 = buffer[startIndex++];
					if (c10 != 'Y' && c10 != 'y')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 == 'P' || c11 == 'p')
					{
						char c12 = buffer[startIndex++];
						if (c12 == 'E' || c12 == 'e')
						{
							result = StringToken.SecondaryType;
						}
					}
					break;
				}
				}
				break;
			}
			case 'H':
			case 'h':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'A' && c2 != 'a')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'R' && c3 != 'r')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if ((c4 != 'D' && c4 != 'd') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'M' && c5 != 'm')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'A' && c6 != 'a')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if ((c7 != 'P' && c7 != 'p') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'N' && c8 != 'n')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'A' && c9 != 'a')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 == 'M' || c10 == 'm')
				{
					char c11 = buffer[startIndex++];
					if (c11 == 'E' || c11 == 'e')
					{
						result = StringToken.ShardMapName;
					}
				}
				break;
			}
			case 'O':
			case 'o':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'R' && c2 != 'r')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if ((c3 != 'T' && c3 != 't') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'I' && c4 != 'i')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if ((c5 != 'N' && c5 != 'n') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'T' && c6 != 't')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'E' && c7 != 'e')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'M' && c8 != 'm')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'P' && c9 != 'p')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 == 'D' || c10 == 'd')
				{
					char c11 = buffer[startIndex++];
					if (c11 == 'B' || c11 == 'b')
					{
						result = StringToken.SortInTempDb;
					}
				}
				break;
			}
			}
			break;
		case 'T':
		case 't':
		{
			char c = buffer[startIndex++];
			if (c != 'R' && c != 'r')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'A' && c2 != 'a')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'N' && c3 != 'n')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'S' && c4 != 's')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'A' && c5 != 'a')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'C' && c6 != 'c')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'T' && c7 != 't')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'I' && c8 != 'i')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'O' && c9 != 'o')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if ((c10 != 'N' && c10 != 'n') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 == 'I' || c11 == 'i')
			{
				char c12 = buffer[startIndex++];
				if (c12 == 'D' || c12 == 'd')
				{
					result = StringToken.TransactionId;
				}
			}
			break;
		}
		case 'W':
		case 'w':
		{
			char c = buffer[startIndex++];
			if (c != 'O' && c != 'o')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'R' && c2 != 'r')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'K' && c3 != 'k')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'L' && c4 != 'l')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'O' && c5 != 'o')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'A' && c6 != 'a')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if ((c7 != 'D' && c7 != 'd') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'G' && c8 != 'g')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'R' && c9 != 'r')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'O' && c10 != 'o')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 == 'U' || c11 == 'u')
			{
				char c12 = buffer[startIndex++];
				if (c12 == 'P' || c12 == 'p')
				{
					result = StringToken.WorkloadGroup;
				}
			}
			break;
		}
		}
		return result;
	}

	private static StringToken Find_Len15(string buffer, int startIndex)
	{
		StringToken result = StringToken.None;
		switch (buffer[startIndex++])
		{
		case 'A':
		case 'a':
			switch (buffer[startIndex++])
			{
			case 'D':
			case 'd':
			{
				char c = buffer[startIndex++];
				if ((c != 'D' && c != 'd') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'R' && c2 != 'r')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'O' && c3 != 'o')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'L' && c4 != 'l')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if ((c5 != 'E' && c5 != 'e') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'M' && c6 != 'm')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'E' && c7 != 'e')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'M' && c8 != 'm')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'B' && c9 != 'b')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 == 'E' || c10 == 'e')
				{
					char c11 = buffer[startIndex++];
					if (c11 == 'R' || c11 == 'r')
					{
						result = StringToken.AddRoleMember;
					}
				}
				break;
			}
			case 'L':
			case 'l':
				switch (buffer[startIndex++])
				{
				case 'L':
				case 'l':
				{
					char c2 = buffer[startIndex++];
					if (c2 != 'O' && c2 != 'o')
					{
						break;
					}
					char c3 = buffer[startIndex++];
					if ((c3 != 'W' && c3 != 'w') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'R' && c4 != 'r')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'O' && c5 != 'o')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if ((c6 != 'W' && c6 != 'w') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'L' && c7 != 'l')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'O' && c8 != 'o')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'C' && c9 != 'c')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 == 'K' || c10 == 'k')
					{
						char c11 = buffer[startIndex++];
						if (c11 == 'S' || c11 == 's')
						{
							result = StringToken.AllowRowLocks;
						}
					}
					break;
				}
				case 'T':
				case 't':
				{
					char c2 = buffer[startIndex++];
					if (c2 != 'E' && c2 != 'e')
					{
						break;
					}
					char c3 = buffer[startIndex++];
					if ((c3 != 'R' && c3 != 'r') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'P' && c4 != 'p')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'R' && c5 != 'r')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'O' && c6 != 'o')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'C' && c7 != 'c')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'E' && c8 != 'e')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'D' && c9 != 'd')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'U' && c10 != 'u')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 == 'R' || c11 == 'r')
					{
						char c12 = buffer[startIndex++];
						if (c12 == 'E' || c12 == 'e')
						{
							result = StringToken.AlterProcedure;
						}
					}
					break;
				}
				}
				break;
			case 'P':
			case 'p':
			{
				char c = buffer[startIndex++];
				if (c != 'P' && c != 'p')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'L' && c2 != 'l')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'I' && c3 != 'i')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'C' && c4 != 'c')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'A' && c5 != 'a')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'T' && c6 != 't')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'I' && c7 != 'i')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'O' && c8 != 'o')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if ((c9 != 'N' && c9 != 'n') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'L' && c10 != 'l')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 == 'O' || c11 == 'o')
				{
					char c12 = buffer[startIndex++];
					if (c12 == 'G' || c12 == 'g')
					{
						result = StringToken.ApplicationLog;
					}
				}
				break;
			}
			}
			break;
		case 'C':
		case 'c':
			switch (buffer[startIndex++])
			{
			case 'H':
			case 'h':
			{
				char c = buffer[startIndex++];
				if (c != 'A' && c != 'a')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'N' && c2 != 'n')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'G' && c3 != 'g')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if ((c4 != 'E' && c4 != 'e') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'T' && c5 != 't')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'R' && c6 != 'r')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'A' && c7 != 'a')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'C' && c8 != 'c')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'K' && c9 != 'k')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'I' && c10 != 'i')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 == 'N' || c11 == 'n')
				{
					char c12 = buffer[startIndex++];
					if (c12 == 'G' || c12 == 'g')
					{
						result = StringToken.ChangeTracking;
					}
				}
				break;
			}
			case 'R':
			case 'r':
			{
				char c = buffer[startIndex++];
				if (c != 'E' && c != 'e')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'A' && c2 != 'a')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'T' && c3 != 't')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if ((c4 != 'E' && c4 != 'e') || buffer[startIndex++] != '_')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case 'A':
				case 'a':
				{
					char c6 = buffer[startIndex++];
					if (c6 != 'S' && c6 != 's')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'S' && c7 != 's')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'E' && c8 != 'e')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'M' && c9 != 'm')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'B' && c10 != 'b')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 == 'L' || c11 == 'l')
					{
						char c12 = buffer[startIndex++];
						if (c12 == 'Y' || c12 == 'y')
						{
							result = StringToken.CreateAssembly;
						}
					}
					break;
				}
				case 'C':
				case 'c':
				{
					char c6 = buffer[startIndex++];
					if (c6 != 'O' && c6 != 'o')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'N' && c7 != 'n')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'T' && c8 != 't')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'R' && c9 != 'r')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'A' && c10 != 'a')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 == 'C' || c11 == 'c')
					{
						char c12 = buffer[startIndex++];
						if (c12 == 'T' || c12 == 't')
						{
							result = StringToken.CreateContract;
						}
					}
					break;
				}
				case 'D':
				case 'd':
				{
					char c6 = buffer[startIndex++];
					if (c6 != 'A' && c6 != 'a')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'T' && c7 != 't')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'A' && c8 != 'a')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'B' && c9 != 'b')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'A' && c10 != 'a')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 == 'S' || c11 == 's')
					{
						char c12 = buffer[startIndex++];
						if (c12 == 'E' || c12 == 'e')
						{
							result = StringToken.CreateDatabase;
						}
					}
					break;
				}
				case 'E':
				case 'e':
				{
					char c6 = buffer[startIndex++];
					if (c6 != 'N' && c6 != 'n')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'D' && c7 != 'd')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'P' && c8 != 'p')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'O' && c9 != 'o')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'I' && c10 != 'i')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 == 'N' || c11 == 'n')
					{
						char c12 = buffer[startIndex++];
						if (c12 == 'T' || c12 == 't')
						{
							result = StringToken.CreateEndpoint;
						}
					}
					break;
				}
				case 'F':
				case 'f':
				{
					char c6 = buffer[startIndex++];
					if (c6 != 'U' && c6 != 'u')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'N' && c7 != 'n')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'C' && c8 != 'c')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'T' && c9 != 't')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'I' && c10 != 'i')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 == 'O' || c11 == 'o')
					{
						char c12 = buffer[startIndex++];
						if (c12 == 'N' || c12 == 'n')
						{
							result = StringToken.CreateFunction;
						}
					}
					break;
				}
				}
				break;
			}
			}
			break;
		case 'D':
		case 'd':
			switch (buffer[startIndex++])
			{
			case 'D':
			case 'd':
			{
				char c = buffer[startIndex++];
				if ((c != 'L' && c != 'l') || buffer[startIndex++] != '_')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case 'R':
				case 'r':
					switch (buffer[startIndex++])
					{
					case 'O':
					case 'o':
					{
						char c4 = buffer[startIndex++];
						if (c4 != 'L' && c4 != 'l')
						{
							break;
						}
						char c5 = buffer[startIndex++];
						if ((c5 != 'E' && c5 != 'e') || buffer[startIndex++] != '_')
						{
							break;
						}
						char c6 = buffer[startIndex++];
						if (c6 != 'E' && c6 != 'e')
						{
							break;
						}
						char c7 = buffer[startIndex++];
						if (c7 != 'V' && c7 != 'v')
						{
							break;
						}
						char c8 = buffer[startIndex++];
						if (c8 != 'E' && c8 != 'e')
						{
							break;
						}
						char c9 = buffer[startIndex++];
						if (c9 != 'N' && c9 != 'n')
						{
							break;
						}
						char c10 = buffer[startIndex++];
						if (c10 == 'T' || c10 == 't')
						{
							char c11 = buffer[startIndex++];
							if (c11 == 'S' || c11 == 's')
							{
								result = StringToken.DdlRoleEvents;
							}
						}
						break;
					}
					case 'U':
					case 'u':
					{
						char c4 = buffer[startIndex++];
						if (c4 != 'L' && c4 != 'l')
						{
							break;
						}
						char c5 = buffer[startIndex++];
						if ((c5 != 'E' && c5 != 'e') || buffer[startIndex++] != '_')
						{
							break;
						}
						char c6 = buffer[startIndex++];
						if (c6 != 'E' && c6 != 'e')
						{
							break;
						}
						char c7 = buffer[startIndex++];
						if (c7 != 'V' && c7 != 'v')
						{
							break;
						}
						char c8 = buffer[startIndex++];
						if (c8 != 'E' && c8 != 'e')
						{
							break;
						}
						char c9 = buffer[startIndex++];
						if (c9 != 'N' && c9 != 'n')
						{
							break;
						}
						char c10 = buffer[startIndex++];
						if (c10 == 'T' || c10 == 't')
						{
							char c11 = buffer[startIndex++];
							if (c11 == 'S' || c11 == 's')
							{
								result = StringToken.DdlRuleEvents;
							}
						}
						break;
					}
					}
					break;
				case 'T':
				case 't':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'Y' && c3 != 'y')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'P' && c4 != 'p')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if ((c5 != 'E' && c5 != 'e') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'E' && c6 != 'e')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'V' && c7 != 'v')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'E' && c8 != 'e')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'N' && c9 != 'n')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 == 'T' || c10 == 't')
					{
						char c11 = buffer[startIndex++];
						if (c11 == 'S' || c11 == 's')
						{
							result = StringToken.DdlTypeEvents;
						}
					}
					break;
				}
				case 'U':
				case 'u':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'S' && c3 != 's')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'E' && c4 != 'e')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if ((c5 != 'R' && c5 != 'r') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'E' && c6 != 'e')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'V' && c7 != 'v')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'E' && c8 != 'e')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'N' && c9 != 'n')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 == 'T' || c10 == 't')
					{
						char c11 = buffer[startIndex++];
						if (c11 == 'S' || c11 == 's')
						{
							result = StringToken.DdlUserEvents;
						}
					}
					break;
				}
				case 'V':
				case 'v':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'I' && c3 != 'i')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'E' && c4 != 'e')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if ((c5 != 'W' && c5 != 'w') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'E' && c6 != 'e')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'V' && c7 != 'v')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'E' && c8 != 'e')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'N' && c9 != 'n')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 == 'T' || c10 == 't')
					{
						char c11 = buffer[startIndex++];
						if (c11 == 'S' || c11 == 's')
						{
							result = StringToken.DdlViewEvents;
						}
					}
					break;
				}
				}
				break;
			}
			case 'R':
			case 'r':
			{
				char c = buffer[startIndex++];
				if (c != 'O' && c != 'o')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if ((c2 != 'P' && c2 != 'p') || buffer[startIndex++] != '_')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case 'P':
				case 'p':
				{
					char c4 = buffer[startIndex++];
					if (c4 != 'L' && c4 != 'l')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'A' && c5 != 'a')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if ((c6 != 'N' && c6 != 'n') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'G' && c7 != 'g')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'U' && c8 != 'u')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'I' && c9 != 'i')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 == 'D' || c10 == 'd')
					{
						char c11 = buffer[startIndex++];
						if (c11 == 'E' || c11 == 'e')
						{
							result = StringToken.DropPlanGuide;
						}
					}
					break;
				}
				case 'S':
				case 's':
				{
					char c4 = buffer[startIndex++];
					if (c4 != 'T' && c4 != 't')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'A' && c5 != 'a')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'T' && c6 != 't')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'I' && c7 != 'i')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'S' && c8 != 's')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'T' && c9 != 't')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'I' && c10 != 'i')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 == 'C' || c11 == 'c')
					{
						char c12 = buffer[startIndex++];
						if (c12 == 'S' || c12 == 's')
						{
							result = StringToken.DropStatistics;
						}
					}
					break;
				}
				}
				break;
			}
			}
			break;
		case 'E':
		case 'e':
		{
			char c13 = buffer[startIndex++];
			if (c13 != 'N' && c13 != 'n')
			{
				break;
			}
			char c = buffer[startIndex++];
			if (c != 'C' && c != 'c')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'R' && c2 != 'r')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'Y' && c3 != 'y')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'P' && c4 != 'p')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'T' && c5 != 't')
			{
				break;
			}
			switch (buffer[startIndex++])
			{
			case 'E':
			case 'e':
			{
				char c7 = buffer[startIndex++];
				if ((c7 != 'D' && c7 != 'd') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'V' && c8 != 'v')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'A' && c9 != 'a')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'L' && c10 != 'l')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 == 'U' || c11 == 'u')
				{
					char c12 = buffer[startIndex++];
					if (c12 == 'E' || c12 == 'e')
					{
						result = StringToken.EncryptedValue;
					}
				}
				break;
			}
			case 'I':
			case 'i':
			{
				char c7 = buffer[startIndex++];
				if (c7 != 'O' && c7 != 'o')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if ((c8 != 'N' && c8 != 'n') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'T' && c9 != 't')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'Y' && c10 != 'y')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 == 'P' || c11 == 'p')
				{
					char c12 = buffer[startIndex++];
					if (c12 == 'E' || c12 == 'e')
					{
						result = StringToken.EncryptionType;
					}
				}
				break;
			}
			}
			break;
		}
		case 'I':
		case 'i':
		{
			char c13 = buffer[startIndex++];
			if (c13 != 'D' && c13 != 'd')
			{
				break;
			}
			char c = buffer[startIndex++];
			if (c != 'E' && c != 'e')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'N' && c2 != 'n')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'T' && c3 != 't')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'I' && c4 != 'i')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'T' && c5 != 't')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if ((c6 != 'Y' && c6 != 'y') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'I' && c7 != 'i')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'N' && c8 != 'n')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'S' && c9 != 's')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'E' && c10 != 'e')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 == 'R' || c11 == 'r')
			{
				char c12 = buffer[startIndex++];
				if (c12 == 'T' || c12 == 't')
				{
					result = StringToken.IdentityInsert;
				}
			}
			break;
		}
		case 'L':
		case 'l':
		{
			char c13 = buffer[startIndex++];
			if (c13 != 'O' && c13 != 'o')
			{
				break;
			}
			char c = buffer[startIndex++];
			if (c != 'C' && c != 'c')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if ((c2 != 'K' && c2 != 'k') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'E' && c3 != 'e')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'S' && c4 != 's')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'C' && c5 != 'c')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'A' && c6 != 'a')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'L' && c7 != 'l')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'A' && c8 != 'a')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'T' && c9 != 't')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'I' && c10 != 'i')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 == 'O' || c11 == 'o')
			{
				char c12 = buffer[startIndex++];
				if (c12 == 'N' || c12 == 'n')
				{
					result = StringToken.LockEscalation;
				}
			}
			break;
		}
		case 'M':
		case 'm':
		{
			char c13 = buffer[startIndex++];
			if (c13 != 'A' && c13 != 'a')
			{
				break;
			}
			char c = buffer[startIndex++];
			if ((c != 'X' && c != 'x') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'C' && c2 != 'c')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'O' && c3 != 'o')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'N' && c4 != 'n')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'N' && c5 != 'n')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'E' && c6 != 'e')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'C' && c7 != 'c')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'T' && c8 != 't')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'I' && c9 != 'i')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'O' && c10 != 'o')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 == 'N' || c11 == 'n')
			{
				char c12 = buffer[startIndex++];
				if (c12 == 'S' || c12 == 's')
				{
					result = StringToken.Max_connections;
				}
			}
			break;
		}
		case 'P':
		case 'p':
		{
			char c13 = buffer[startIndex++];
			if (c13 != 'R' && c13 != 'r')
			{
				break;
			}
			char c = buffer[startIndex++];
			if (c != 'O' && c != 'o')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'P' && c2 != 'p')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'E' && c3 != 'e')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'R' && c4 != 'r')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'T' && c5 != 't')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if ((c6 != 'Y' && c6 != 'y') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'I' && c7 != 'i')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'N' && c8 != 'n')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if ((c9 != 'T' && c9 != 't') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 == 'I' || c10 == 'i')
			{
				char c11 = buffer[startIndex++];
				if (c11 == 'D' || c11 == 'd')
				{
					result = StringToken.PropertyIntId;
				}
			}
			break;
		}
		case 'R':
		case 'r':
		{
			char c13 = buffer[startIndex++];
			if (c13 != 'E' && c13 != 'e')
			{
				break;
			}
			switch (buffer[startIndex++])
			{
			case 'A':
			case 'a':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'D' && c2 != 'd')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'U' && c3 != 'u')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'N' && c4 != 'n')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'C' && c5 != 'c')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'O' && c6 != 'o')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'M' && c7 != 'm')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'M' && c8 != 'm')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'I' && c9 != 'i')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'T' && c10 != 't')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'T' && c11 != 't')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 == 'E' || c12 == 'e')
				{
					char c14 = buffer[startIndex++];
					if (c14 == 'D' || c14 == 'd')
					{
						result = StringToken.ReadUncommitted;
					}
				}
				break;
			}
			case 'V':
			case 'v':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'O' && c2 != 'o')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'K' && c3 != 'k')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if ((c4 != 'E' && c4 != 'e') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'D' && c5 != 'd')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'A' && c6 != 'a')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'T' && c7 != 't')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'A' && c8 != 'a')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'B' && c9 != 'b')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'A' && c10 != 'a')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 == 'S' || c11 == 's')
				{
					char c12 = buffer[startIndex++];
					if (c12 == 'E' || c12 == 'e')
					{
						result = StringToken.RevokeDatabase;
					}
				}
				break;
			}
			}
			break;
		}
		case 'S':
		case 's':
			switch (buffer[startIndex++])
			{
			case 'E':
			case 'e':
			{
				char c = buffer[startIndex++];
				if (c != 'Q' && c != 'q')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'U' && c2 != 'u')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'E' && c3 != 'e')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'N' && c4 != 'n')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'C' && c5 != 'c')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if ((c6 != 'E' && c6 != 'e') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'N' && c7 != 'n')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'U' && c8 != 'u')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'M' && c9 != 'm')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'B' && c10 != 'b')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 == 'E' || c11 == 'e')
				{
					char c12 = buffer[startIndex++];
					if (c12 == 'R' || c12 == 'r')
					{
						result = StringToken.SequenceNumber;
					}
				}
				break;
			}
			case 'T':
			case 't':
			{
				char c = buffer[startIndex++];
				if (c != 'A' && c != 'a')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'T' && c2 != 't')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'I' && c3 != 'i')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'S' && c4 != 's')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'T' && c5 != 't')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'I' && c6 != 'i')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'C' && c7 != 'c')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if ((c8 != 'S' && c8 != 's') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'O' && c9 != 'o')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'N' && c10 != 'n')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 == 'L' || c11 == 'l')
				{
					char c12 = buffer[startIndex++];
					if (c12 == 'Y' || c12 == 'y')
					{
						result = StringToken.StatisticsOnly;
					}
				}
				break;
			}
			}
			break;
		}
		return result;
	}

	private static StringToken Find_Len16(string buffer, int startIndex)
	{
		StringToken result = StringToken.None;
		switch (buffer[startIndex++])
		{
		case 'A':
		case 'a':
			switch (buffer[startIndex++])
			{
			case 'B':
			case 'b':
			{
				char c = buffer[startIndex++];
				if (c != 'O' && c != 'o')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'R' && c2 != 'r')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if ((c3 != 'T' && c3 != 't') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'A' && c4 != 'a')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'F' && c5 != 'f')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'T' && c6 != 't')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'E' && c7 != 'e')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if ((c8 != 'R' && c8 != 'r') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'W' && c9 != 'w')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'A' && c10 != 'a')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 == 'I' || c11 == 'i')
				{
					char c12 = buffer[startIndex++];
					if (c12 == 'T' || c12 == 't')
					{
						result = StringToken.AbortAfterWait;
					}
				}
				break;
			}
			case 'L':
			case 'l':
				switch (buffer[startIndex++])
				{
				case 'L':
				case 'l':
				{
					char c2 = buffer[startIndex++];
					if (c2 != 'O' && c2 != 'o')
					{
						break;
					}
					char c3 = buffer[startIndex++];
					if ((c3 != 'W' && c3 != 'w') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'P' && c4 != 'p')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'A' && c5 != 'a')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'G' && c6 != 'g')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if ((c7 != 'E' && c7 != 'e') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'L' && c8 != 'l')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'O' && c9 != 'o')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'C' && c10 != 'c')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 == 'K' || c11 == 'k')
					{
						char c12 = buffer[startIndex++];
						if (c12 == 'S' || c12 == 's')
						{
							result = StringToken.AllowPageLocks;
						}
					}
					break;
				}
				case 'T':
				case 't':
				{
					char c2 = buffer[startIndex++];
					if (c2 != 'E' && c2 != 'e')
					{
						break;
					}
					char c3 = buffer[startIndex++];
					if ((c3 != 'R' && c3 != 'r') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'P' && c4 != 'p')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'L' && c5 != 'l')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'A' && c6 != 'a')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if ((c7 != 'N' && c7 != 'n') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'G' && c8 != 'g')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'U' && c9 != 'u')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'I' && c10 != 'i')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 == 'D' || c11 == 'd')
					{
						char c12 = buffer[startIndex++];
						if (c12 == 'E' || c12 == 'e')
						{
							result = StringToken.AlterPlanGuide;
						}
					}
					break;
				}
				}
				break;
			case 'U':
			case 'u':
			{
				char c = buffer[startIndex++];
				if (c != 'T' && c != 't')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'O' && c2 != 'o')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'M' && c3 != 'm')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'A' && c4 != 'a')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'T' && c5 != 't')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'I' && c6 != 'i')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if ((c7 != 'C' && c7 != 'c') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'T' && c8 != 't')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'U' && c9 != 'u')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'N' && c10 != 'n')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'I' && c11 != 'i')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 == 'N' || c12 == 'n')
				{
					char c13 = buffer[startIndex++];
					if (c13 == 'G' || c13 == 'g')
					{
						result = StringToken.AutomaticTuning;
					}
				}
				break;
			}
			}
			break;
		case 'C':
		case 'c':
			switch (buffer[startIndex++])
			{
			case 'H':
			case 'h':
			{
				char c = buffer[startIndex++];
				if (c != 'E' && c != 'e')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'C' && c2 != 'c')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if ((c3 != 'K' && c3 != 'k') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'E' && c4 != 'e')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'X' && c5 != 'x')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'P' && c6 != 'p')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'I' && c7 != 'i')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'R' && c8 != 'r')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'A' && c9 != 'a')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'T' && c10 != 't')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'I' && c11 != 'i')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 == 'O' || c12 == 'o')
				{
					char c13 = buffer[startIndex++];
					if (c13 == 'N' || c13 == 'n')
					{
						result = StringToken.CheckExpiration;
					}
				}
				break;
			}
			case 'R':
			case 'r':
			{
				char c = buffer[startIndex++];
				if (c != 'E' && c != 'e')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'A' && c2 != 'a')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'T' && c3 != 't')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if ((c4 != 'E' && c4 != 'e') || buffer[startIndex++] != '_')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case 'P':
				case 'p':
				{
					char c6 = buffer[startIndex++];
					if (c6 != 'R' && c6 != 'r')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'O' && c7 != 'o')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'C' && c8 != 'c')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'E' && c9 != 'e')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'D' && c10 != 'd')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 != 'U' && c11 != 'u')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if (c12 == 'R' || c12 == 'r')
					{
						char c13 = buffer[startIndex++];
						if (c13 == 'E' || c13 == 'e')
						{
							result = StringToken.CreateProcedure;
						}
					}
					break;
				}
				case 'X':
				case 'x':
				{
					char c6 = buffer[startIndex++];
					if (c6 != 'M' && c6 != 'm')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if ((c7 != 'L' && c7 != 'l') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'I' && c8 != 'i')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'N' && c9 != 'n')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'D' && c10 != 'd')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 == 'E' || c11 == 'e')
					{
						char c12 = buffer[startIndex++];
						if (c12 == 'X' || c12 == 'x')
						{
							result = StringToken.CreateXmlIndex;
						}
					}
					break;
				}
				}
				break;
			}
			}
			break;
		case 'D':
		case 'd':
			switch (buffer[startIndex++])
			{
			case 'A':
			case 'a':
			{
				char c = buffer[startIndex++];
				if (c != 'T' && c != 't')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'A' && c2 != 'a')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case '_':
				{
					char c4 = buffer[startIndex++];
					if (c4 != 'C' && c4 != 'c')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'O' && c5 != 'o')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'M' && c6 != 'm')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'P' && c7 != 'p')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'R' && c8 != 'r')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'E' && c9 != 'e')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'S' && c10 != 's')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 != 'S' && c11 != 's')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if (c12 != 'I' && c12 != 'i')
					{
						break;
					}
					char c13 = buffer[startIndex++];
					if (c13 == 'O' || c13 == 'o')
					{
						char c14 = buffer[startIndex++];
						if (c14 == 'N' || c14 == 'n')
						{
							result = StringToken.DataCompression;
						}
					}
					break;
				}
				case 'B':
				case 'b':
				{
					char c4 = buffer[startIndex++];
					if (c4 != 'A' && c4 != 'a')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'S' && c5 != 's')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if ((c6 != 'E' && c6 != 'e') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'D' && c7 != 'd')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'E' && c8 != 'e')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'F' && c9 != 'f')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'A' && c10 != 'a')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 != 'U' && c11 != 'u')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if (c12 == 'L' || c12 == 'l')
					{
						char c13 = buffer[startIndex++];
						if (c13 == 'T' || c13 == 't')
						{
							result = StringToken.DatabaseDefault;
						}
					}
					break;
				}
				}
				break;
			}
			case 'D':
			case 'd':
			{
				char c = buffer[startIndex++];
				if ((c != 'L' && c != 'l') || buffer[startIndex++] != '_')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case 'I':
				case 'i':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'N' && c3 != 'n')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'D' && c4 != 'd')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'E' && c5 != 'e')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if ((c6 != 'X' && c6 != 'x') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'E' && c7 != 'e')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'V' && c8 != 'v')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'E' && c9 != 'e')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'N' && c10 != 'n')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 == 'T' || c11 == 't')
					{
						char c12 = buffer[startIndex++];
						if (c12 == 'S' || c12 == 's')
						{
							result = StringToken.DdlIndexEvents;
						}
					}
					break;
				}
				case 'L':
				case 'l':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'O' && c3 != 'o')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'G' && c4 != 'g')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'I' && c5 != 'i')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if ((c6 != 'N' && c6 != 'n') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'E' && c7 != 'e')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'V' && c8 != 'v')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'E' && c9 != 'e')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'N' && c10 != 'n')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 == 'T' || c11 == 't')
					{
						char c12 = buffer[startIndex++];
						if (c12 == 'S' || c12 == 's')
						{
							result = StringToken.DdlLoginEvents;
						}
					}
					break;
				}
				case 'Q':
				case 'q':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'U' && c3 != 'u')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'E' && c4 != 'e')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'U' && c5 != 'u')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if ((c6 != 'E' && c6 != 'e') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'E' && c7 != 'e')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'V' && c8 != 'v')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'E' && c9 != 'e')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'N' && c10 != 'n')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 == 'T' || c11 == 't')
					{
						char c12 = buffer[startIndex++];
						if (c12 == 'S' || c12 == 's')
						{
							result = StringToken.DdlQueueEvents;
						}
					}
					break;
				}
				case 'R':
				case 'r':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'O' && c3 != 'o')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'U' && c4 != 'u')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'T' && c5 != 't')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if ((c6 != 'E' && c6 != 'e') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'E' && c7 != 'e')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'V' && c8 != 'v')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'E' && c9 != 'e')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'N' && c10 != 'n')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 == 'T' || c11 == 't')
					{
						char c12 = buffer[startIndex++];
						if (c12 == 'S' || c12 == 's')
						{
							result = StringToken.DdlRouteEvents;
						}
					}
					break;
				}
				case 'T':
				case 't':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'A' && c3 != 'a')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'B' && c4 != 'b')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'L' && c5 != 'l')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if ((c6 != 'E' && c6 != 'e') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'E' && c7 != 'e')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'V' && c8 != 'v')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'E' && c9 != 'e')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'N' && c10 != 'n')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 == 'T' || c11 == 't')
					{
						char c12 = buffer[startIndex++];
						if (c12 == 'S' || c12 == 's')
						{
							result = StringToken.DdlTableEvents;
						}
					}
					break;
				}
				}
				break;
			}
			case 'E':
			case 'e':
			{
				char c = buffer[startIndex++];
				if (c != 'F' && c != 'f')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case '_':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'S' && c3 != 's')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'O' && c4 != 'o')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'R' && c5 != 'r')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'T' && c6 != 't')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'O' && c7 != 'o')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'R' && c8 != 'r')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'D' && c9 != 'd')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'E' && c10 != 'e')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if ((c11 != 'R' && c11 != 'r') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if (c12 == 'I' || c12 == 'i')
					{
						char c13 = buffer[startIndex++];
						if (c13 == 'D' || c13 == 'd')
						{
							result = StringToken.Def_sortorder_id;
						}
					}
					break;
				}
				case 'A':
				case 'a':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'U' && c3 != 'u')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'L' && c4 != 'l')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if ((c5 != 'T' && c5 != 't') || buffer[startIndex++] != '_')
					{
						break;
					}
					switch (buffer[startIndex++])
					{
					case 'D':
					case 'd':
					{
						char c7 = buffer[startIndex++];
						if (c7 != 'A' && c7 != 'a')
						{
							break;
						}
						char c8 = buffer[startIndex++];
						if (c8 != 'T' && c8 != 't')
						{
							break;
						}
						char c9 = buffer[startIndex++];
						if (c9 != 'A' && c9 != 'a')
						{
							break;
						}
						char c10 = buffer[startIndex++];
						if (c10 != 'B' && c10 != 'b')
						{
							break;
						}
						char c11 = buffer[startIndex++];
						if (c11 != 'A' && c11 != 'a')
						{
							break;
						}
						char c12 = buffer[startIndex++];
						if (c12 == 'S' || c12 == 's')
						{
							char c13 = buffer[startIndex++];
							if (c13 == 'E' || c13 == 'e')
							{
								result = StringToken.DefaultDatabase;
							}
						}
						break;
					}
					case 'L':
					case 'l':
					{
						char c7 = buffer[startIndex++];
						if (c7 != 'A' && c7 != 'a')
						{
							break;
						}
						char c8 = buffer[startIndex++];
						if (c8 != 'N' && c8 != 'n')
						{
							break;
						}
						char c9 = buffer[startIndex++];
						if (c9 != 'G' && c9 != 'g')
						{
							break;
						}
						char c10 = buffer[startIndex++];
						if (c10 != 'U' && c10 != 'u')
						{
							break;
						}
						char c11 = buffer[startIndex++];
						if (c11 != 'A' && c11 != 'a')
						{
							break;
						}
						char c12 = buffer[startIndex++];
						if (c12 == 'G' || c12 == 'g')
						{
							char c13 = buffer[startIndex++];
							if (c13 == 'E' || c13 == 'e')
							{
								result = StringToken.DefaultLanguage;
							}
						}
						break;
					}
					}
					break;
				}
				}
				break;
			}
			case 'R':
			case 'r':
			{
				char c = buffer[startIndex++];
				if (c != 'O' && c != 'o')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if ((c2 != 'P' && c2 != 'p') || buffer[startIndex++] != '_')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case 'C':
				case 'c':
				{
					char c4 = buffer[startIndex++];
					if (c4 != 'E' && c4 != 'e')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'R' && c5 != 'r')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'T' && c6 != 't')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'I' && c7 != 'i')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'F' && c8 != 'f')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'I' && c9 != 'i')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'C' && c10 != 'c')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 != 'A' && c11 != 'a')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if (c12 == 'T' || c12 == 't')
					{
						char c13 = buffer[startIndex++];
						if (c13 == 'E' || c13 == 'e')
						{
							result = StringToken.DropCertificate;
						}
					}
					break;
				}
				case 'R':
				case 'r':
				{
					char c4 = buffer[startIndex++];
					if (c4 != 'O' && c4 != 'o')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'L' && c5 != 'l')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if ((c6 != 'E' && c6 != 'e') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'M' && c7 != 'm')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'E' && c8 != 'e')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'M' && c9 != 'm')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'B' && c10 != 'b')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 == 'E' || c11 == 'e')
					{
						char c12 = buffer[startIndex++];
						if (c12 == 'R' || c12 == 'r')
						{
							result = StringToken.DropRoleMember;
						}
					}
					break;
				}
				}
				break;
			}
			}
			break;
		case 'E':
		case 'e':
		{
			char c15 = buffer[startIndex++];
			if (c15 != 'X' && c15 != 'x')
			{
				break;
			}
			char c = buffer[startIndex++];
			if (c != 'T' && c != 't')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'E' && c2 != 'e')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'R' && c3 != 'r')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'N' && c4 != 'n')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'A' && c5 != 'a')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if ((c6 != 'L' && c6 != 'l') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'M' && c7 != 'm')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'O' && c8 != 'o')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'N' && c9 != 'n')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'I' && c10 != 'i')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'T' && c11 != 't')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 == 'O' || c12 == 'o')
			{
				char c13 = buffer[startIndex++];
				if (c13 == 'R' || c13 == 'r')
				{
					result = StringToken.ExternalMonitor;
				}
			}
			break;
		}
		case 'F':
		case 'f':
		{
			char c15 = buffer[startIndex++];
			if (c15 != 'I' && c15 != 'i')
			{
				break;
			}
			char c = buffer[startIndex++];
			if (c != 'E' && c != 'e')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'L' && c2 != 'l')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if ((c3 != 'D' && c3 != 'd') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'T' && c4 != 't')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'E' && c5 != 'e')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'R' && c6 != 'r')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'M' && c7 != 'm')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'I' && c8 != 'i')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'N' && c9 != 'n')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'A' && c10 != 'a')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'T' && c11 != 't')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 == 'O' || c12 == 'o')
			{
				char c13 = buffer[startIndex++];
				if (c13 == 'R' || c13 == 'r')
				{
					result = StringToken.FieldTerminator;
				}
			}
			break;
		}
		case 'M':
		case 'm':
			switch (buffer[startIndex++])
			{
			case 'E':
			case 'e':
			{
				char c = buffer[startIndex++];
				if (c != 'M' && c != 'm')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'O' && c2 != 'o')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'R' && c3 != 'r')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if ((c4 != 'Y' && c4 != 'y') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'O' && c5 != 'o')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'P' && c6 != 'p')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'T' && c7 != 't')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'I' && c8 != 'i')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'M' && c9 != 'm')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'I' && c10 != 'i')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'Z' && c11 != 'z')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 == 'E' || c12 == 'e')
				{
					char c13 = buffer[startIndex++];
					if (c13 == 'D' || c13 == 'd')
					{
						result = StringToken.MemoryOptimized;
					}
				}
				break;
			}
			case 'I':
			case 'i':
			{
				char c = buffer[startIndex++];
				if (c != 'C' && c != 'c')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'R' && c2 != 'r')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'O' && c3 != 'o')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'S' && c4 != 's')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'O' && c5 != 'o')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'F' && c6 != 'f')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'T' && c7 != 't')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'V' && c8 != 'v')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'E' && c9 != 'e')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'R' && c10 != 'r')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'S' && c11 != 's')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'I' && c12 != 'i')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 == 'O' || c13 == 'o')
				{
					char c14 = buffer[startIndex++];
					if (c14 == 'N' || c14 == 'n')
					{
						result = StringToken.Microsoftversion;
					}
				}
				break;
			}
			}
			break;
		case 'R':
		case 'r':
		{
			char c15 = buffer[startIndex++];
			if (c15 != 'E' && c15 != 'e')
			{
				break;
			}
			char c = buffer[startIndex++];
			if (c != 'T' && c != 't')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'E' && c2 != 'e')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'N' && c3 != 'n')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'T' && c4 != 't')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'I' && c5 != 'i')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'O' && c6 != 'o')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if ((c7 != 'N' && c7 != 'n') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'P' && c8 != 'p')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'E' && c9 != 'e')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'R' && c10 != 'r')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'I' && c11 != 'i')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 == 'O' || c12 == 'o')
			{
				char c13 = buffer[startIndex++];
				if (c13 == 'D' || c13 == 'd')
				{
					result = StringToken.RetentionPeriod;
				}
			}
			break;
		}
		case 'S':
		case 's':
		{
			char c15 = buffer[startIndex++];
			if (c15 != 'T' && c15 != 't')
			{
				break;
			}
			char c = buffer[startIndex++];
			if (c != 'R' && c != 'r')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'I' && c2 != 'i')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'N' && c3 != 'n')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if ((c4 != 'G' && c4 != 'g') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'D' && c5 != 'd')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'E' && c6 != 'e')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'L' && c7 != 'l')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'I' && c8 != 'i')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'M' && c9 != 'm')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'I' && c10 != 'i')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'T' && c11 != 't')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 == 'E' || c12 == 'e')
			{
				char c13 = buffer[startIndex++];
				if (c13 == 'R' || c13 == 'r')
				{
					result = StringToken.StringDelimiter;
				}
			}
			break;
		}
		case 'U':
		case 'u':
			switch (buffer[startIndex++])
			{
			case 'N':
			case 'n':
			{
				char c = buffer[startIndex++];
				if (c != 'I' && c != 'i')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'Q' && c2 != 'q')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'U' && c3 != 'u')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'E' && c4 != 'e')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'I' && c5 != 'i')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'D' && c6 != 'd')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'E' && c7 != 'e')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'N' && c8 != 'n')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'T' && c9 != 't')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'I' && c10 != 'i')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'F' && c11 != 'f')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'I' && c12 != 'i')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 == 'E' || c13 == 'e')
				{
					char c14 = buffer[startIndex++];
					if (c14 == 'R' || c14 == 'r')
					{
						result = StringToken.UniqueIdentifier;
					}
				}
				break;
			}
			case 'S':
			case 's':
			{
				char c = buffer[startIndex++];
				if ((c != 'E' && c != 'e') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'T' && c2 != 't')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'Y' && c3 != 'y')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'P' && c4 != 'p')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if ((c5 != 'E' && c5 != 'e') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'D' && c6 != 'd')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'E' && c7 != 'e')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'F' && c8 != 'f')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'A' && c9 != 'a')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'U' && c10 != 'u')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 == 'L' || c11 == 'l')
				{
					char c12 = buffer[startIndex++];
					if (c12 == 'T' || c12 == 't')
					{
						result = StringToken.UseTypeDefault;
					}
				}
				break;
			}
			}
			break;
		}
		return result;
	}

	private static StringToken Find_Len17(string buffer, int startIndex)
	{
		StringToken result = StringToken.None;
		switch (buffer[startIndex++])
		{
		case 'A':
		case 'a':
		{
			char c = buffer[startIndex++];
			if (c != 'L' && c != 'l')
			{
				break;
			}
			switch (buffer[startIndex++])
			{
			case 'L':
			case 'l':
			{
				char c3 = buffer[startIndex++];
				if (c3 != 'O' && c3 != 'o')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if ((c4 != 'W' && c4 != 'w') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'C' && c5 != 'c')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'O' && c6 != 'o')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'N' && c7 != 'n')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'N' && c8 != 'n')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'E' && c9 != 'e')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'C' && c10 != 'c')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'T' && c11 != 't')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'I' && c12 != 'i')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 != 'O' && c13 != 'o')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if (c14 == 'N' || c14 == 'n')
				{
					char c15 = buffer[startIndex++];
					if (c15 == 'S' || c15 == 's')
					{
						result = StringToken.AllowConnections;
					}
				}
				break;
			}
			case 'T':
			case 't':
			{
				char c3 = buffer[startIndex++];
				if (c3 != 'E' && c3 != 'e')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if ((c4 != 'R' && c4 != 'r') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'C' && c5 != 'c')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'E' && c6 != 'e')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'R' && c7 != 'r')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'T' && c8 != 't')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'I' && c9 != 'i')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'F' && c10 != 'f')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'I' && c11 != 'i')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'C' && c12 != 'c')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 != 'A' && c13 != 'a')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if (c14 == 'T' || c14 == 't')
				{
					char c15 = buffer[startIndex++];
					if (c15 == 'E' || c15 == 'e')
					{
						result = StringToken.AlterCertificate;
					}
				}
				break;
			}
			}
			break;
		}
		case 'C':
		case 'c':
			switch (buffer[startIndex++])
			{
			case 'O':
			case 'o':
				switch (buffer[startIndex++])
				{
				case 'L':
				case 'l':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'U' && c3 != 'u')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'M' && c4 != 'm')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if ((c5 != 'N' && c5 != 'n') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'M' && c6 != 'm')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'A' && c7 != 'a')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'S' && c8 != 's')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'T' && c9 != 't')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'E' && c10 != 'e')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if ((c11 != 'R' && c11 != 'r') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if (c12 != 'K' && c12 != 'k')
					{
						break;
					}
					char c13 = buffer[startIndex++];
					if (c13 == 'E' || c13 == 'e')
					{
						char c14 = buffer[startIndex++];
						if (c14 == 'Y' || c14 == 'y')
						{
							result = StringToken.ColumnMasterKey;
						}
					}
					break;
				}
				case 'M':
				case 'm':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'P' && c3 != 'p')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'R' && c4 != 'r')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'E' && c5 != 'e')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'S' && c6 != 's')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'S' && c7 != 's')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'I' && c8 != 'i')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'O' && c9 != 'o')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if ((c10 != 'N' && c10 != 'n') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 != 'D' && c11 != 'd')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if (c12 != 'E' && c12 != 'e')
					{
						break;
					}
					char c13 = buffer[startIndex++];
					if (c13 != 'L' && c13 != 'l')
					{
						break;
					}
					char c14 = buffer[startIndex++];
					if (c14 == 'A' || c14 == 'a')
					{
						char c15 = buffer[startIndex++];
						if (c15 == 'Y' || c15 == 'y')
						{
							result = StringToken.CompressionDelay;
						}
					}
					break;
				}
				}
				break;
			case 'R':
			case 'r':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'E' && c2 != 'e')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'A' && c3 != 'a')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'T' && c4 != 't')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if ((c5 != 'E' && c5 != 'e') || buffer[startIndex++] != '_')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case 'P':
				case 'p':
				{
					char c7 = buffer[startIndex++];
					if (c7 != 'L' && c7 != 'l')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'A' && c8 != 'a')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if ((c9 != 'N' && c9 != 'n') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'G' && c10 != 'g')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 != 'U' && c11 != 'u')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if (c12 != 'I' && c12 != 'i')
					{
						break;
					}
					char c13 = buffer[startIndex++];
					if (c13 == 'D' || c13 == 'd')
					{
						char c14 = buffer[startIndex++];
						if (c14 == 'E' || c14 == 'e')
						{
							result = StringToken.CreatePlanGuide;
						}
					}
					break;
				}
				case 'S':
				case 's':
				{
					char c7 = buffer[startIndex++];
					if (c7 != 'T' && c7 != 't')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'A' && c8 != 'a')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'T' && c9 != 't')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'I' && c10 != 'i')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 != 'S' && c11 != 's')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if (c12 != 'T' && c12 != 't')
					{
						break;
					}
					char c13 = buffer[startIndex++];
					if (c13 != 'I' && c13 != 'i')
					{
						break;
					}
					char c14 = buffer[startIndex++];
					if (c14 == 'C' || c14 == 'c')
					{
						char c15 = buffer[startIndex++];
						if (c15 == 'S' || c15 == 's')
						{
							result = StringToken.CreateStatistics;
						}
					}
					break;
				}
				}
				break;
			}
			case 'U':
			case 'u':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'R' && c2 != 'r')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'R' && c3 != 'r')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'E' && c4 != 'e')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'N' && c5 != 'n')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if ((c6 != 'T' && c6 != 't') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'T' && c7 != 't')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'I' && c8 != 'i')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'M' && c9 != 'm')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'E' && c10 != 'e')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'S' && c11 != 's')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'T' && c12 != 't')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 != 'A' && c13 != 'a')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if (c14 == 'M' || c14 == 'm')
				{
					char c15 = buffer[startIndex++];
					if (c15 == 'P' || c15 == 'p')
					{
						result = StringToken.CurrentTimestamp;
					}
				}
				break;
			}
			}
			break;
		case 'D':
		case 'd':
			switch (buffer[startIndex++])
			{
			case 'D':
			case 'd':
			{
				char c2 = buffer[startIndex++];
				if ((c2 != 'L' && c2 != 'l') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'S' && c3 != 's')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'C' && c4 != 'c')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'H' && c5 != 'h')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'E' && c6 != 'e')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'M' && c7 != 'm')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if ((c8 != 'A' && c8 != 'a') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'E' && c9 != 'e')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'V' && c10 != 'v')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'E' && c11 != 'e')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'N' && c12 != 'n')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 == 'T' || c13 == 't')
				{
					char c14 = buffer[startIndex++];
					if (c14 == 'S' || c14 == 's')
					{
						result = StringToken.DdlSchemaEvents;
					}
				}
				break;
			}
			case 'R':
			case 'r':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'O' && c2 != 'o')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if ((c3 != 'P' && c3 != 'p') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'M' && c4 != 'm')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'E' && c5 != 'e')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'S' && c6 != 's')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'S' && c7 != 's')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'A' && c8 != 'a')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'G' && c9 != 'g')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if ((c10 != 'E' && c10 != 'e') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'T' && c11 != 't')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'Y' && c12 != 'y')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 == 'P' || c13 == 'p')
				{
					char c14 = buffer[startIndex++];
					if (c14 == 'E' || c14 == 'e')
					{
						result = StringToken.DropMessageType;
					}
				}
				break;
			}
			}
			break;
		case 'P':
		case 'p':
			switch (buffer[startIndex++])
			{
			case 'L':
			case 'l':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'A' && c2 != 'a')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if ((c3 != 'N' && c3 != 'n') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'F' && c4 != 'f')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'O' && c5 != 'o')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'R' && c6 != 'r')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'C' && c7 != 'c')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'I' && c8 != 'i')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'N' && c9 != 'n')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if ((c10 != 'G' && c10 != 'g') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'M' && c11 != 'm')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'O' && c12 != 'o')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 == 'D' || c13 == 'd')
				{
					char c14 = buffer[startIndex++];
					if (c14 == 'E' || c14 == 'e')
					{
						result = StringToken.PlanForcingMode;
					}
				}
				break;
			}
			case 'R':
			case 'r':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'O' && c2 != 'o')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'P' && c3 != 'p')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'E' && c4 != 'e')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'R' && c5 != 'r')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'T' && c6 != 't')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if ((c7 != 'Y' && c7 != 'y') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'S' && c8 != 's')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'E' && c9 != 'e')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if ((c10 != 'T' && c10 != 't') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'G' && c11 != 'g')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'U' && c12 != 'u')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 == 'I' || c13 == 'i')
				{
					char c14 = buffer[startIndex++];
					if (c14 == 'D' || c14 == 'd')
					{
						result = StringToken.PropertySetGuid;
					}
				}
				break;
			}
			}
			break;
		case 'Q':
		case 'q':
		{
			char c = buffer[startIndex++];
			if (c != 'U' && c != 'u')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'O' && c2 != 'o')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'T' && c3 != 't')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'E' && c4 != 'e')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if ((c5 != 'D' && c5 != 'd') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'I' && c6 != 'i')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'D' && c7 != 'd')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'E' && c8 != 'e')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'N' && c9 != 'n')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'T' && c10 != 't')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'I' && c11 != 'i')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 != 'F' && c12 != 'f')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if (c13 != 'I' && c13 != 'i')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 == 'E' || c14 == 'e')
			{
				char c15 = buffer[startIndex++];
				if (c15 == 'R' || c15 == 'r')
				{
					result = StringToken.QuotedIdentifier;
				}
			}
			break;
		}
		case 'R':
		case 'r':
		{
			char c = buffer[startIndex++];
			if (c != 'E' && c != 'e')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'A' && c2 != 'a')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'D' && c3 != 'd')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'C' && c4 != 'c')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'O' && c5 != 'o')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'M' && c6 != 'm')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'M' && c7 != 'm')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'I' && c8 != 'i')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'T' && c9 != 't')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'T' && c10 != 't')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'E' && c11 != 'e')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 != 'D' && c12 != 'd')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if (c13 != 'L' && c13 != 'l')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 != 'O' && c14 != 'o')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if (c15 == 'C' || c15 == 'c')
			{
				char c16 = buffer[startIndex++];
				if (c16 == 'K' || c16 == 'k')
				{
					result = StringToken.ReadCommittedLock;
				}
			}
			break;
		}
		case 'S':
		case 's':
			switch (buffer[startIndex++])
			{
			case 'E':
			case 'e':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'R' && c2 != 'r')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'V' && c3 != 'v')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'I' && c4 != 'i')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'C' && c5 != 'c')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if ((c6 != 'E' && c6 != 'e') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'O' && c7 != 'o')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'B' && c8 != 'b')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'J' && c9 != 'j')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'E' && c10 != 'e')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'C' && c11 != 'c')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'T' && c12 != 't')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 != 'I' && c13 != 'i')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if (c14 == 'V' || c14 == 'v')
				{
					char c15 = buffer[startIndex++];
					if (c15 == 'E' || c15 == 'e')
					{
						result = StringToken.ServiceObjective;
					}
				}
				break;
			}
			case 'H':
			case 'h':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'A' && c2 != 'a')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'R' && c3 != 'r')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if ((c4 != 'D' && c4 != 'd') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'M' && c5 != 'm')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'A' && c6 != 'a')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if ((c7 != 'P' && c7 != 'p') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'M' && c8 != 'm')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'A' && c9 != 'a')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'N' && c10 != 'n')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'A' && c11 != 'a')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'G' && c12 != 'g')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 == 'E' || c13 == 'e')
				{
					char c14 = buffer[startIndex++];
					if (c14 == 'R' || c14 == 'r')
					{
						result = StringToken.ShardMapManager;
					}
				}
				break;
			}
			case 'O':
			case 'o':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'R' && c2 != 'r')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'T' && c3 != 't')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'E' && c4 != 'e')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if ((c5 != 'D' && c5 != 'd') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'D' && c6 != 'd')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'A' && c7 != 'a')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'T' && c8 != 't')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if ((c9 != 'A' && c9 != 'a') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'R' && c10 != 'r')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'E' && c11 != 'e')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'O' && c12 != 'o')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 == 'R' || c13 == 'r')
				{
					char c14 = buffer[startIndex++];
					if (c14 == 'G' || c14 == 'g')
					{
						result = StringToken.SortedDataReorg;
					}
				}
				break;
			}
			case 'Y':
			case 'y':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'S' && c2 != 's')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'T' && c3 != 't')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'E' && c4 != 'e')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if ((c5 != 'M' && c5 != 'm') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'V' && c6 != 'v')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'E' && c7 != 'e')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'R' && c8 != 'r')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'S' && c9 != 's')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'I' && c10 != 'i')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'O' && c11 != 'o')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'N' && c12 != 'n')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 != 'I' && c13 != 'i')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if (c14 == 'N' || c14 == 'n')
				{
					char c15 = buffer[startIndex++];
					if (c15 == 'G' || c15 == 'g')
					{
						result = StringToken.SystemVersioning;
					}
				}
				break;
			}
			}
			break;
		case 'U':
		case 'u':
		{
			char c = buffer[startIndex++];
			if (c != 'P' && c != 'p')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'D' && c2 != 'd')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'A' && c3 != 'a')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'T' && c4 != 't')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if ((c5 != 'E' && c5 != 'e') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'S' && c6 != 's')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'T' && c7 != 't')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'A' && c8 != 'a')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'T' && c9 != 't')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'I' && c10 != 'i')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'S' && c11 != 's')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 != 'T' && c12 != 't')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if (c13 != 'I' && c13 != 'i')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 == 'C' || c14 == 'c')
			{
				char c15 = buffer[startIndex++];
				if (c15 == 'S' || c15 == 's')
				{
					result = StringToken.UpdateStatistics;
				}
			}
			break;
		}
		}
		return result;
	}

	private static StringToken Find_Len18(string buffer, int startIndex)
	{
		StringToken result = StringToken.None;
		switch (buffer[startIndex++])
		{
		case 'A':
		case 'a':
		{
			char c = buffer[startIndex++];
			if (c != 'L' && c != 'l')
			{
				break;
			}
			switch (buffer[startIndex++])
			{
			case 'L':
			case 'l':
			{
				if (buffer[startIndex++] != '_')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'S' && c3 != 's')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'P' && c4 != 'p')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'A' && c5 != 'a')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'R' && c6 != 'r')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'S' && c7 != 's')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if ((c8 != 'E' && c8 != 'e') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'C' && c9 != 'c')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'O' && c10 != 'o')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'L' && c11 != 'l')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'U' && c12 != 'u')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 != 'M' && c13 != 'm')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if (c14 == 'N' || c14 == 'n')
				{
					char c15 = buffer[startIndex++];
					if (c15 == 'S' || c15 == 's')
					{
						result = StringToken.AllSparseColumns;
					}
				}
				break;
			}
			case 'T':
			case 't':
			{
				char c3 = buffer[startIndex++];
				if (c3 != 'E' && c3 != 'e')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if ((c4 != 'R' && c4 != 'r') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'M' && c5 != 'm')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'E' && c6 != 'e')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'S' && c7 != 's')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'S' && c8 != 's')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'A' && c9 != 'a')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'G' && c10 != 'g')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if ((c11 != 'E' && c11 != 'e') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'T' && c12 != 't')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 != 'Y' && c13 != 'y')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if (c14 == 'P' || c14 == 'p')
				{
					char c15 = buffer[startIndex++];
					if (c15 == 'E' || c15 == 'e')
					{
						result = StringToken.AlterMessageType;
					}
				}
				break;
			}
			}
			break;
		}
		case 'C':
		case 'c':
			switch (buffer[startIndex++])
			{
			case 'O':
			case 'o':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'N' && c2 != 'n')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'N' && c3 != 'n')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'E' && c4 != 'e')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'C' && c5 != 'c')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'T' && c6 != 't')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'I' && c7 != 'i')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'O' && c8 != 'o')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if ((c9 != 'N' && c9 != 'n') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'O' && c10 != 'o')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'P' && c11 != 'p')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'T' && c12 != 't')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 != 'I' && c13 != 'i')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if (c14 != 'O' && c14 != 'o')
				{
					break;
				}
				char c15 = buffer[startIndex++];
				if (c15 == 'N' || c15 == 'n')
				{
					char c16 = buffer[startIndex++];
					if (c16 == 'S' || c16 == 's')
					{
						result = StringToken.ConnectionOptions;
					}
				}
				break;
			}
			case 'R':
			case 'r':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'E' && c2 != 'e')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'A' && c3 != 'a')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'T' && c4 != 't')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if ((c5 != 'E' && c5 != 'e') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'C' && c6 != 'c')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'E' && c7 != 'e')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'R' && c8 != 'r')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'T' && c9 != 't')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'I' && c10 != 'i')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'F' && c11 != 'f')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'I' && c12 != 'i')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 != 'C' && c13 != 'c')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if (c14 != 'A' && c14 != 'a')
				{
					break;
				}
				char c15 = buffer[startIndex++];
				if (c15 == 'T' || c15 == 't')
				{
					char c16 = buffer[startIndex++];
					if (c16 == 'E' || c16 == 'e')
					{
						result = StringToken.CreateCertificate;
					}
				}
				break;
			}
			}
			break;
		case 'D':
		case 'd':
			switch (buffer[startIndex++])
			{
			case 'D':
			case 'd':
			{
				char c2 = buffer[startIndex++];
				if ((c2 != 'L' && c2 != 'l') || buffer[startIndex++] != '_')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case 'D':
				case 'd':
				{
					char c4 = buffer[startIndex++];
					if (c4 != 'E' && c4 != 'e')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'F' && c5 != 'f')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'A' && c6 != 'a')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'U' && c7 != 'u')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'L' && c8 != 'l')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if ((c9 != 'T' && c9 != 't') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'E' && c10 != 'e')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 != 'V' && c11 != 'v')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if (c12 != 'E' && c12 != 'e')
					{
						break;
					}
					char c13 = buffer[startIndex++];
					if (c13 != 'N' && c13 != 'n')
					{
						break;
					}
					char c14 = buffer[startIndex++];
					if (c14 == 'T' || c14 == 't')
					{
						char c15 = buffer[startIndex++];
						if (c15 == 'S' || c15 == 's')
						{
							result = StringToken.DdlDefaultEvents;
						}
					}
					break;
				}
				case 'M':
				case 'm':
				{
					char c4 = buffer[startIndex++];
					if (c4 != 'E' && c4 != 'e')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'S' && c5 != 's')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'S' && c6 != 's')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'A' && c7 != 'a')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'G' && c8 != 'g')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if ((c9 != 'E' && c9 != 'e') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'E' && c10 != 'e')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 != 'V' && c11 != 'v')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if (c12 != 'E' && c12 != 'e')
					{
						break;
					}
					char c13 = buffer[startIndex++];
					if (c13 != 'N' && c13 != 'n')
					{
						break;
					}
					char c14 = buffer[startIndex++];
					if (c14 == 'T' || c14 == 't')
					{
						char c15 = buffer[startIndex++];
						if (c15 == 'S' || c15 == 's')
						{
							result = StringToken.DdlMessageEvents;
						}
					}
					break;
				}
				case 'S':
				case 's':
					switch (buffer[startIndex++])
					{
					case 'E':
					case 'e':
					{
						char c5 = buffer[startIndex++];
						if (c5 != 'R' && c5 != 'r')
						{
							break;
						}
						char c6 = buffer[startIndex++];
						if (c6 != 'V' && c6 != 'v')
						{
							break;
						}
						char c7 = buffer[startIndex++];
						if (c7 != 'I' && c7 != 'i')
						{
							break;
						}
						char c8 = buffer[startIndex++];
						if (c8 != 'C' && c8 != 'c')
						{
							break;
						}
						char c9 = buffer[startIndex++];
						if ((c9 != 'E' && c9 != 'e') || buffer[startIndex++] != '_')
						{
							break;
						}
						char c10 = buffer[startIndex++];
						if (c10 != 'E' && c10 != 'e')
						{
							break;
						}
						char c11 = buffer[startIndex++];
						if (c11 != 'V' && c11 != 'v')
						{
							break;
						}
						char c12 = buffer[startIndex++];
						if (c12 != 'E' && c12 != 'e')
						{
							break;
						}
						char c13 = buffer[startIndex++];
						if (c13 != 'N' && c13 != 'n')
						{
							break;
						}
						char c14 = buffer[startIndex++];
						if (c14 == 'T' || c14 == 't')
						{
							char c15 = buffer[startIndex++];
							if (c15 == 'S' || c15 == 's')
							{
								result = StringToken.DdlServiceEvents;
							}
						}
						break;
					}
					case 'Y':
					case 'y':
					{
						char c5 = buffer[startIndex++];
						if (c5 != 'N' && c5 != 'n')
						{
							break;
						}
						char c6 = buffer[startIndex++];
						if (c6 != 'O' && c6 != 'o')
						{
							break;
						}
						char c7 = buffer[startIndex++];
						if (c7 != 'N' && c7 != 'n')
						{
							break;
						}
						char c8 = buffer[startIndex++];
						if (c8 != 'Y' && c8 != 'y')
						{
							break;
						}
						char c9 = buffer[startIndex++];
						if ((c9 != 'M' && c9 != 'm') || buffer[startIndex++] != '_')
						{
							break;
						}
						char c10 = buffer[startIndex++];
						if (c10 != 'E' && c10 != 'e')
						{
							break;
						}
						char c11 = buffer[startIndex++];
						if (c11 != 'V' && c11 != 'v')
						{
							break;
						}
						char c12 = buffer[startIndex++];
						if (c12 != 'E' && c12 != 'e')
						{
							break;
						}
						char c13 = buffer[startIndex++];
						if (c13 != 'N' && c13 != 'n')
						{
							break;
						}
						char c14 = buffer[startIndex++];
						if (c14 == 'T' || c14 == 't')
						{
							char c15 = buffer[startIndex++];
							if (c15 == 'S' || c15 == 's')
							{
								result = StringToken.DdlSynonymEvents;
							}
						}
						break;
					}
					}
					break;
				case 'T':
				case 't':
				{
					char c4 = buffer[startIndex++];
					if (c4 != 'R' && c4 != 'r')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'I' && c5 != 'i')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'G' && c6 != 'g')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'G' && c7 != 'g')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'E' && c8 != 'e')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if ((c9 != 'R' && c9 != 'r') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'E' && c10 != 'e')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 != 'V' && c11 != 'v')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if (c12 != 'E' && c12 != 'e')
					{
						break;
					}
					char c13 = buffer[startIndex++];
					if (c13 != 'N' && c13 != 'n')
					{
						break;
					}
					char c14 = buffer[startIndex++];
					if (c14 == 'T' || c14 == 't')
					{
						char c15 = buffer[startIndex++];
						if (c15 == 'S' || c15 == 's')
						{
							result = StringToken.DdlTriggerEvents;
						}
					}
					break;
				}
				}
				break;
			}
			case 'E':
			case 'e':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'L' && c2 != 'l')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'A' && c3 != 'a')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'Y' && c4 != 'y')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'E' && c5 != 'e')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if ((c6 != 'D' && c6 != 'd') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'D' && c7 != 'd')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'U' && c8 != 'u')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'R' && c9 != 'r')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'A' && c10 != 'a')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'B' && c11 != 'b')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'I' && c12 != 'i')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 != 'L' && c13 != 'l')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if (c14 != 'I' && c14 != 'i')
				{
					break;
				}
				char c15 = buffer[startIndex++];
				if (c15 == 'T' || c15 == 't')
				{
					char c16 = buffer[startIndex++];
					if (c16 == 'Y' || c16 == 'y')
					{
						result = StringToken.DelayedDurability;
					}
				}
				break;
			}
			case 'R':
			case 'r':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'O' && c2 != 'o')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if ((c3 != 'P' && c3 != 'p') || buffer[startIndex++] != '_')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case 'L':
				case 'l':
				{
					char c5 = buffer[startIndex++];
					if (c5 != 'I' && c5 != 'i')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'N' && c6 != 'n')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'K' && c7 != 'k')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'E' && c8 != 'e')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if ((c9 != 'D' && c9 != 'd') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'S' && c10 != 's')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 != 'E' && c11 != 'e')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if (c12 != 'R' && c12 != 'r')
					{
						break;
					}
					char c13 = buffer[startIndex++];
					if (c13 != 'V' && c13 != 'v')
					{
						break;
					}
					char c14 = buffer[startIndex++];
					if (c14 == 'E' || c14 == 'e')
					{
						char c15 = buffer[startIndex++];
						if (c15 == 'R' || c15 == 'r')
						{
							result = StringToken.DropLinkedServer;
						}
					}
					break;
				}
				case 'R':
				case 'r':
				{
					char c5 = buffer[startIndex++];
					if (c5 != 'E' && c5 != 'e')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'M' && c6 != 'm')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'O' && c7 != 'o')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'T' && c8 != 't')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if ((c9 != 'E' && c9 != 'e') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'S' && c10 != 's')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 != 'E' && c11 != 'e')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if (c12 != 'R' && c12 != 'r')
					{
						break;
					}
					char c13 = buffer[startIndex++];
					if (c13 != 'V' && c13 != 'v')
					{
						break;
					}
					char c14 = buffer[startIndex++];
					if (c14 == 'E' || c14 == 'e')
					{
						char c15 = buffer[startIndex++];
						if (c15 == 'R' || c15 == 'r')
						{
							result = StringToken.DropRemoteServer;
						}
					}
					break;
				}
				}
				break;
			}
			}
			break;
		case 'N':
		case 'n':
		{
			char c = buffer[startIndex++];
			if (c != 'A' && c != 'a')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'T' && c2 != 't')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'I' && c3 != 'i')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'V' && c4 != 'v')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if ((c5 != 'E' && c5 != 'e') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'C' && c6 != 'c')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'O' && c7 != 'o')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'M' && c8 != 'm')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'P' && c9 != 'p')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'I' && c10 != 'i')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'L' && c11 != 'l')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 != 'A' && c12 != 'a')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if (c13 != 'T' && c13 != 't')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 != 'I' && c14 != 'i')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if (c15 == 'O' || c15 == 'o')
			{
				char c16 = buffer[startIndex++];
				if (c16 == 'N' || c16 == 'n')
				{
					result = StringToken.NativeCompilation;
				}
			}
			break;
		}
		case 'P':
		case 'p':
		{
			char c = buffer[startIndex++];
			if (c != 'A' && c != 'a')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'R' && c2 != 'r')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'A' && c3 != 'a')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'M' && c4 != 'm')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'E' && c5 != 'e')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'T' && c6 != 't')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'E' && c7 != 'e')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if ((c8 != 'R' && c8 != 'r') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'S' && c9 != 's')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'N' && c10 != 'n')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'I' && c11 != 'i')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 != 'F' && c12 != 'f')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if (c13 != 'F' && c13 != 'f')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 != 'I' && c14 != 'i')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if (c15 == 'N' || c15 == 'n')
			{
				char c16 = buffer[startIndex++];
				if (c16 == 'G' || c16 == 'g')
				{
					result = StringToken.ParameterSniffing;
				}
			}
			break;
		}
		case 'Q':
		case 'q':
		{
			char c = buffer[startIndex++];
			if (c != 'U' && c != 'u')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'E' && c2 != 'e')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'R' && c3 != 'r')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if ((c4 != 'Y' && c4 != 'y') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'C' && c5 != 'c')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'A' && c6 != 'a')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'P' && c7 != 'p')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'T' && c8 != 't')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'U' && c9 != 'u')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'R' && c10 != 'r')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if ((c11 != 'E' && c11 != 'e') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 != 'M' && c12 != 'm')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if (c13 != 'O' && c13 != 'o')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 == 'D' || c14 == 'd')
			{
				char c15 = buffer[startIndex++];
				if (c15 == 'E' || c15 == 'e')
				{
					result = StringToken.QueryCaptureMode;
				}
			}
			break;
		}
		}
		return result;
	}

	private static StringToken Find_Len19(string buffer, int startIndex)
	{
		StringToken result = StringToken.None;
		switch (buffer[startIndex++])
		{
		case 'A':
		case 'a':
		{
			char c = buffer[startIndex++];
			if (c != 'L' && c != 'l')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if (c16 != 'T' && c16 != 't')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'E' && c2 != 'e')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if ((c3 != 'R' && c3 != 'r') || buffer[startIndex++] != '_')
			{
				break;
			}
			switch (buffer[startIndex++])
			{
			case 'L':
			case 'l':
			{
				char c5 = buffer[startIndex++];
				if (c5 != 'I' && c5 != 'i')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'N' && c6 != 'n')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'K' && c7 != 'k')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'E' && c8 != 'e')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if ((c9 != 'D' && c9 != 'd') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'S' && c10 != 's')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'E' && c11 != 'e')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'R' && c12 != 'r')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 != 'V' && c13 != 'v')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if (c14 == 'E' || c14 == 'e')
				{
					char c15 = buffer[startIndex++];
					if (c15 == 'R' || c15 == 'r')
					{
						result = StringToken.AlterLinkedServer;
					}
				}
				break;
			}
			case 'R':
			case 'r':
			{
				char c5 = buffer[startIndex++];
				if (c5 != 'E' && c5 != 'e')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'M' && c6 != 'm')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'O' && c7 != 'o')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'T' && c8 != 't')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if ((c9 != 'E' && c9 != 'e') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'S' && c10 != 's')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'E' && c11 != 'e')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'R' && c12 != 'r')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 != 'V' && c13 != 'v')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if (c14 == 'E' || c14 == 'e')
				{
					char c15 = buffer[startIndex++];
					if (c15 == 'R' || c15 == 'r')
					{
						result = StringToken.AlterRemoteServer;
					}
				}
				break;
			}
			}
			break;
		}
		case 'C':
		case 'c':
			switch (buffer[startIndex++])
			{
			case 'O':
			case 'o':
			{
				char c16 = buffer[startIndex++];
				if (c16 != 'L' && c16 != 'l')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'U' && c2 != 'u')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'M' && c3 != 'm')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'N' && c4 != 'n')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'S' && c5 != 's')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'T' && c6 != 't')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'O' && c7 != 'o')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'R' && c8 != 'r')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if ((c9 != 'E' && c9 != 'e') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'A' && c10 != 'a')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'R' && c11 != 'r')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'C' && c12 != 'c')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 != 'H' && c13 != 'h')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if (c14 != 'I' && c14 != 'i')
				{
					break;
				}
				char c15 = buffer[startIndex++];
				if (c15 == 'V' || c15 == 'v')
				{
					char c17 = buffer[startIndex++];
					if (c17 == 'E' || c17 == 'e')
					{
						result = StringToken.ColumnStoreArchive;
					}
				}
				break;
			}
			case 'R':
			case 'r':
			{
				char c16 = buffer[startIndex++];
				if (c16 != 'E' && c16 != 'e')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'A' && c2 != 'a')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'T' && c3 != 't')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if ((c4 != 'E' && c4 != 'e') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'M' && c5 != 'm')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'E' && c6 != 'e')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'S' && c7 != 's')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'S' && c8 != 's')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'A' && c9 != 'a')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'G' && c10 != 'g')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if ((c11 != 'E' && c11 != 'e') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'T' && c12 != 't')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 != 'Y' && c13 != 'y')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if (c14 == 'P' || c14 == 'p')
				{
					char c15 = buffer[startIndex++];
					if (c15 == 'E' || c15 == 'e')
					{
						result = StringToken.CreateMessageType;
					}
				}
				break;
			}
			}
			break;
		case 'D':
		case 'd':
			switch (buffer[startIndex++])
			{
			case 'D':
			case 'd':
			{
				char c16 = buffer[startIndex++];
				if ((c16 != 'L' && c16 != 'l') || buffer[startIndex++] != '_')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case 'A':
				case 'a':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'S' && c3 != 's')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'S' && c4 != 's')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'E' && c5 != 'e')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'M' && c6 != 'm')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'B' && c7 != 'b')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'L' && c8 != 'l')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if ((c9 != 'Y' && c9 != 'y') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'E' && c10 != 'e')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 != 'V' && c11 != 'v')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if (c12 != 'E' && c12 != 'e')
					{
						break;
					}
					char c13 = buffer[startIndex++];
					if (c13 != 'N' && c13 != 'n')
					{
						break;
					}
					char c14 = buffer[startIndex++];
					if (c14 == 'T' || c14 == 't')
					{
						char c15 = buffer[startIndex++];
						if (c15 == 'S' || c15 == 's')
						{
							result = StringToken.DdlAssemblyEvents;
						}
					}
					break;
				}
				case 'C':
				case 'c':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'O' && c3 != 'o')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'N' && c4 != 'n')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'T' && c5 != 't')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'R' && c6 != 'r')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'A' && c7 != 'a')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'C' && c8 != 'c')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if ((c9 != 'T' && c9 != 't') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'E' && c10 != 'e')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 != 'V' && c11 != 'v')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if (c12 != 'E' && c12 != 'e')
					{
						break;
					}
					char c13 = buffer[startIndex++];
					if (c13 != 'N' && c13 != 'n')
					{
						break;
					}
					char c14 = buffer[startIndex++];
					if (c14 == 'T' || c14 == 't')
					{
						char c15 = buffer[startIndex++];
						if (c15 == 'S' || c15 == 's')
						{
							result = StringToken.DdlContractEvents;
						}
					}
					break;
				}
				case 'E':
				case 'e':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'N' && c3 != 'n')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'D' && c4 != 'd')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'P' && c5 != 'p')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'O' && c6 != 'o')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'I' && c7 != 'i')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'N' && c8 != 'n')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if ((c9 != 'T' && c9 != 't') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'E' && c10 != 'e')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 != 'V' && c11 != 'v')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if (c12 != 'E' && c12 != 'e')
					{
						break;
					}
					char c13 = buffer[startIndex++];
					if (c13 != 'N' && c13 != 'n')
					{
						break;
					}
					char c14 = buffer[startIndex++];
					if (c14 == 'T' || c14 == 't')
					{
						char c15 = buffer[startIndex++];
						if (c15 == 'S' || c15 == 's')
						{
							result = StringToken.DdlEndpointEvents;
						}
					}
					break;
				}
				case 'F':
				case 'f':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'U' && c3 != 'u')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'N' && c4 != 'n')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'C' && c5 != 'c')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'T' && c6 != 't')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'I' && c7 != 'i')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'O' && c8 != 'o')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if ((c9 != 'N' && c9 != 'n') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'E' && c10 != 'e')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 != 'V' && c11 != 'v')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if (c12 != 'E' && c12 != 'e')
					{
						break;
					}
					char c13 = buffer[startIndex++];
					if (c13 != 'N' && c13 != 'n')
					{
						break;
					}
					char c14 = buffer[startIndex++];
					if (c14 == 'T' || c14 == 't')
					{
						char c15 = buffer[startIndex++];
						if (c15 == 'S' || c15 == 's')
						{
							result = StringToken.DdlFunctionEvents;
						}
					}
					break;
				}
				}
				break;
			}
			case 'R':
			case 'r':
			{
				char c16 = buffer[startIndex++];
				if (c16 != 'O' && c16 != 'o')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if ((c2 != 'P' && c2 != 'p') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'F' && c3 != 'f')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'U' && c4 != 'u')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'L' && c5 != 'l')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'L' && c6 != 'l')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'T' && c7 != 't')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'E' && c8 != 'e')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'X' && c9 != 'x')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if ((c10 != 'T' && c10 != 't') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'I' && c11 != 'i')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'N' && c12 != 'n')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 != 'D' && c13 != 'd')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if (c14 == 'E' || c14 == 'e')
				{
					char c15 = buffer[startIndex++];
					if (c15 == 'X' || c15 == 'x')
					{
						result = StringToken.DropFulltextIndex;
					}
				}
				break;
			}
			}
			break;
		case 'F':
		case 'f':
		{
			char c = buffer[startIndex++];
			if (c != 'I' && c != 'i')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if (c16 != 'L' && c16 != 'l')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'E' && c2 != 'e')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'T' && c3 != 't')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'A' && c4 != 'a')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'B' && c5 != 'b')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'L' && c6 != 'l')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if ((c7 != 'E' && c7 != 'e') || buffer[startIndex++] != '_')
			{
				break;
			}
			switch (buffer[startIndex++])
			{
			case 'D':
			case 'd':
			{
				char c9 = buffer[startIndex++];
				if (c9 != 'I' && c9 != 'i')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'R' && c10 != 'r')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'E' && c11 != 'e')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'C' && c12 != 'c')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 != 'T' && c13 != 't')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if (c14 != 'O' && c14 != 'o')
				{
					break;
				}
				char c15 = buffer[startIndex++];
				if (c15 == 'R' || c15 == 'r')
				{
					char c17 = buffer[startIndex++];
					if (c17 == 'Y' || c17 == 'y')
					{
						result = StringToken.FileTableDirectory;
					}
				}
				break;
			}
			case 'N':
			case 'n':
			{
				char c9 = buffer[startIndex++];
				if (c9 != 'A' && c9 != 'a')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'M' && c10 != 'm')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'E' && c11 != 'e')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'S' && c12 != 's')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 != 'P' && c13 != 'p')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if (c14 != 'A' && c14 != 'a')
				{
					break;
				}
				char c15 = buffer[startIndex++];
				if (c15 == 'C' || c15 == 'c')
				{
					char c17 = buffer[startIndex++];
					if (c17 == 'E' || c17 == 'e')
					{
						result = StringToken.FileTableNamespace;
					}
				}
				break;
			}
			}
			break;
		}
		case 'I':
		case 'i':
		{
			char c = buffer[startIndex++];
			if (c != 'N' && c != 'n')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if (c16 != 'C' && c16 != 'c')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'L' && c2 != 'l')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'U' && c3 != 'u')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'D' && c4 != 'd')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if ((c5 != 'E' && c5 != 'e') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'N' && c6 != 'n')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'U' && c7 != 'u')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'L' && c8 != 'l')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if ((c9 != 'L' && c9 != 'l') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'V' && c10 != 'v')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'A' && c11 != 'a')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 != 'L' && c12 != 'l')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if (c13 != 'U' && c13 != 'u')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 == 'E' || c14 == 'e')
			{
				char c15 = buffer[startIndex++];
				if (c15 == 'S' || c15 == 's')
				{
					result = StringToken.IncludeNullValues;
				}
			}
			break;
		}
		case 'M':
		case 'm':
		{
			char c = buffer[startIndex++];
			if (c != 'A' && c != 'a')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if ((c16 != 'X' && c16 != 'x') || buffer[startIndex++] != '_')
			{
				break;
			}
			switch (buffer[startIndex++])
			{
			case 'P':
			case 'p':
			{
				char c3 = buffer[startIndex++];
				if (c3 != 'L' && c3 != 'l')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'A' && c4 != 'a')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'N' && c5 != 'n')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if ((c6 != 'S' && c6 != 's') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'P' && c7 != 'p')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'E' && c8 != 'e')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if ((c9 != 'R' && c9 != 'r') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'Q' && c10 != 'q')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'U' && c11 != 'u')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'E' && c12 != 'e')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 == 'R' || c13 == 'r')
				{
					char c14 = buffer[startIndex++];
					if (c14 == 'Y' || c14 == 'y')
					{
						result = StringToken.MaxPlansPerQuery;
					}
				}
				break;
			}
			case 'S':
			case 's':
			{
				char c3 = buffer[startIndex++];
				if (c3 != 'T' && c3 != 't')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'O' && c4 != 'o')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'R' && c5 != 'r')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'A' && c6 != 'a')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'G' && c7 != 'g')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if ((c8 != 'E' && c8 != 'e') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'S' && c9 != 's')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'I' && c10 != 'i')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'Z' && c11 != 'z')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if ((c12 != 'E' && c12 != 'e') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 == 'M' || c13 == 'm')
				{
					char c14 = buffer[startIndex++];
					if (c14 == 'B' || c14 == 'b')
					{
						result = StringToken.MaxStorageSizeMb;
					}
				}
				break;
			}
			}
			break;
		}
		case 'R':
		case 'r':
		{
			char c = buffer[startIndex++];
			if (c != 'E' && c != 'e')
			{
				break;
			}
			switch (buffer[startIndex++])
			{
			case 'J':
			case 'j':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'E' && c2 != 'e')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'C' && c3 != 'c')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if ((c4 != 'T' && c4 != 't') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'S' && c5 != 's')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'A' && c6 != 'a')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'M' && c7 != 'm')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'P' && c8 != 'p')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'L' && c9 != 'l')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if ((c10 != 'E' && c10 != 'e') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'V' && c11 != 'v')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'A' && c12 != 'a')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 != 'L' && c13 != 'l')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if (c14 == 'U' || c14 == 'u')
				{
					char c15 = buffer[startIndex++];
					if (c15 == 'E' || c15 == 'e')
					{
						result = StringToken.RejectSampleValue;
					}
				}
				break;
			}
			case 'M':
			case 'm':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'O' && c2 != 'o')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'T' && c3 != 't')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if ((c4 != 'E' && c4 != 'e') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'D' && c5 != 'd')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'A' && c6 != 'a')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'T' && c7 != 't')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if ((c8 != 'A' && c8 != 'a') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'A' && c9 != 'a')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'R' && c10 != 'r')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'C' && c11 != 'c')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'H' && c12 != 'h')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 != 'I' && c13 != 'i')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if (c14 == 'V' || c14 == 'v')
				{
					char c15 = buffer[startIndex++];
					if (c15 == 'E' || c15 == 'e')
					{
						result = StringToken.RemoteDataArchive;
					}
				}
				break;
			}
			}
			break;
		}
		}
		return result;
	}

	private static StringToken Find_Len20(string buffer, int startIndex)
	{
		StringToken result = StringToken.None;
		switch (buffer[startIndex++])
		{
		case 'A':
		case 'a':
		{
			char c = buffer[startIndex++];
			if (c != 'L' && c != 'l')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'T' && c2 != 't')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'E' && c3 != 'e')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if ((c4 != 'R' && c4 != 'r') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'F' && c5 != 'f')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'U' && c6 != 'u')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'L' && c7 != 'l')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'L' && c8 != 'l')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'T' && c9 != 't')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'E' && c10 != 'e')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'X' && c11 != 'x')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if ((c12 != 'T' && c12 != 't') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if (c13 != 'I' && c13 != 'i')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 != 'N' && c14 != 'n')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if (c15 != 'D' && c15 != 'd')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if (c16 == 'E' || c16 == 'e')
			{
				char c17 = buffer[startIndex++];
				if (c17 == 'X' || c17 == 'x')
				{
					result = StringToken.AlterFulltextIndex;
				}
			}
			break;
		}
		case 'C':
		case 'c':
		{
			char c = buffer[startIndex++];
			if (c != 'R' && c != 'r')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'E' && c2 != 'e')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'A' && c3 != 'a')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'T' && c4 != 't')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if ((c5 != 'E' && c5 != 'e') || buffer[startIndex++] != '_')
			{
				break;
			}
			switch (buffer[startIndex++])
			{
			case 'L':
			case 'l':
			{
				char c7 = buffer[startIndex++];
				if (c7 != 'I' && c7 != 'i')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'N' && c8 != 'n')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'K' && c9 != 'k')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'E' && c10 != 'e')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if ((c11 != 'D' && c11 != 'd') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'S' && c12 != 's')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 != 'E' && c13 != 'e')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if (c14 != 'R' && c14 != 'r')
				{
					break;
				}
				char c15 = buffer[startIndex++];
				if (c15 != 'V' && c15 != 'v')
				{
					break;
				}
				char c16 = buffer[startIndex++];
				if (c16 == 'E' || c16 == 'e')
				{
					char c17 = buffer[startIndex++];
					if (c17 == 'R' || c17 == 'r')
					{
						result = StringToken.CreateLinkedServer;
					}
				}
				break;
			}
			case 'R':
			case 'r':
			{
				char c7 = buffer[startIndex++];
				if (c7 != 'E' && c7 != 'e')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'M' && c8 != 'm')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'O' && c9 != 'o')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'T' && c10 != 't')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if ((c11 != 'E' && c11 != 'e') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'S' && c12 != 's')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 != 'E' && c13 != 'e')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if (c14 != 'R' && c14 != 'r')
				{
					break;
				}
				char c15 = buffer[startIndex++];
				if (c15 != 'V' && c15 != 'v')
				{
					break;
				}
				char c16 = buffer[startIndex++];
				if (c16 == 'E' || c16 == 'e')
				{
					char c17 = buffer[startIndex++];
					if (c17 == 'R' || c17 == 'r')
					{
						result = StringToken.CreateRemoteServer;
					}
				}
				break;
			}
			case 'S':
			case 's':
			{
				char c7 = buffer[startIndex++];
				if (c7 != 'P' && c7 != 'p')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'A' && c8 != 'a')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'T' && c9 != 't')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'I' && c10 != 'i')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'A' && c11 != 'a')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if ((c12 != 'L' && c12 != 'l') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 != 'I' && c13 != 'i')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if (c14 != 'N' && c14 != 'n')
				{
					break;
				}
				char c15 = buffer[startIndex++];
				if (c15 != 'D' && c15 != 'd')
				{
					break;
				}
				char c16 = buffer[startIndex++];
				if (c16 == 'E' || c16 == 'e')
				{
					char c17 = buffer[startIndex++];
					if (c17 == 'X' || c17 == 'x')
					{
						result = StringToken.CreateSpatialIndex;
					}
				}
				break;
			}
			}
			break;
		}
		case 'D':
		case 'd':
		{
			char c = buffer[startIndex++];
			if (c != 'D' && c != 'd')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if ((c2 != 'L' && c2 != 'l') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'P' && c3 != 'p')
			{
				break;
			}
			switch (buffer[startIndex++])
			{
			case 'A':
			case 'a':
			{
				char c5 = buffer[startIndex++];
				if (c5 != 'R' && c5 != 'r')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'T' && c6 != 't')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'I' && c7 != 'i')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'T' && c8 != 't')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'I' && c9 != 'i')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'O' && c10 != 'o')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if ((c11 != 'N' && c11 != 'n') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'E' && c12 != 'e')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 != 'V' && c13 != 'v')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if (c14 != 'E' && c14 != 'e')
				{
					break;
				}
				char c15 = buffer[startIndex++];
				if (c15 != 'N' && c15 != 'n')
				{
					break;
				}
				char c16 = buffer[startIndex++];
				if (c16 == 'T' || c16 == 't')
				{
					char c17 = buffer[startIndex++];
					if (c17 == 'S' || c17 == 's')
					{
						result = StringToken.DdlPartitionEvents;
					}
				}
				break;
			}
			case 'R':
			case 'r':
			{
				char c5 = buffer[startIndex++];
				if (c5 != 'O' && c5 != 'o')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'C' && c6 != 'c')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'E' && c7 != 'e')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'D' && c8 != 'd')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'U' && c9 != 'u')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'R' && c10 != 'r')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if ((c11 != 'E' && c11 != 'e') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'E' && c12 != 'e')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 != 'V' && c13 != 'v')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if (c14 != 'E' && c14 != 'e')
				{
					break;
				}
				char c15 = buffer[startIndex++];
				if (c15 != 'N' && c15 != 'n')
				{
					break;
				}
				char c16 = buffer[startIndex++];
				if (c16 == 'T' || c16 == 't')
				{
					char c17 = buffer[startIndex++];
					if (c17 == 'S' || c17 == 's')
					{
						result = StringToken.DdlProcedureEvents;
					}
				}
				break;
			}
			}
			break;
		}
		case 'E':
		case 'e':
		{
			char c = buffer[startIndex++];
			if (c != 'N' && c != 'n')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'C' && c2 != 'c')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'L' && c3 != 'l')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'A' && c4 != 'a')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'V' && c5 != 'v')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if ((c6 != 'E' && c6 != 'e') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'C' && c7 != 'c')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'O' && c8 != 'o')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'M' && c9 != 'm')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'P' && c10 != 'p')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'U' && c11 != 'u')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 != 'T' && c12 != 't')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if (c13 != 'A' && c13 != 'a')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 != 'T' && c14 != 't')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if (c15 != 'I' && c15 != 'i')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if (c16 != 'O' && c16 != 'o')
			{
				break;
			}
			char c17 = buffer[startIndex++];
			if (c17 == 'N' || c17 == 'n')
			{
				char c18 = buffer[startIndex++];
				if (c18 == 'S' || c18 == 's')
				{
					result = StringToken.EnclaveComputations;
				}
			}
			break;
		}
		case 'J':
		case 'j':
		{
			char c = buffer[startIndex++];
			if (c != 'O' && c != 'o')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if ((c2 != 'B' && c2 != 'b') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'T' && c3 != 't')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'R' && c4 != 'r')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'A' && c5 != 'a')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'C' && c6 != 'c')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'K' && c7 != 'k')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'E' && c8 != 'e')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if ((c9 != 'R' && c9 != 'r') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'L' && c10 != 'l')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'O' && c11 != 'o')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 != 'C' && c12 != 'c')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if (c13 != 'A' && c13 != 'a')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 != 'T' && c14 != 't')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if (c15 != 'I' && c15 != 'i')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if (c16 == 'O' || c16 == 'o')
			{
				char c17 = buffer[startIndex++];
				if (c17 == 'N' || c17 == 'n')
				{
					result = StringToken.JobTrackerLocation;
				}
			}
			break;
		}
		case 'P':
		case 'p':
		{
			char c = buffer[startIndex++];
			if (c != 'R' && c != 'r')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'O' && c2 != 'o')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'P' && c3 != 'p')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'E' && c4 != 'e')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'R' && c5 != 'r')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'T' && c6 != 't')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if ((c7 != 'Y' && c7 != 'y') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'D' && c8 != 'd')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'E' && c9 != 'e')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'S' && c10 != 's')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'C' && c11 != 'c')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 != 'R' && c12 != 'r')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if (c13 != 'I' && c13 != 'i')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 != 'P' && c14 != 'p')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if (c15 != 'T' && c15 != 't')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if (c16 != 'I' && c16 != 'i')
			{
				break;
			}
			char c17 = buffer[startIndex++];
			if (c17 == 'O' || c17 == 'o')
			{
				char c18 = buffer[startIndex++];
				if (c18 == 'N' || c18 == 'n')
				{
					result = StringToken.PropertyDescription;
				}
			}
			break;
		}
		case 'S':
		case 's':
		{
			char c = buffer[startIndex++];
			if (c != 'T' && c != 't')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'A' && c2 != 'a')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'L' && c3 != 'l')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if ((c4 != 'E' && c4 != 'e') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'P' && c5 != 'p')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'A' && c6 != 'a')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'G' && c7 != 'g')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if ((c8 != 'E' && c8 != 'e') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'D' && c9 != 'd')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'E' && c10 != 'e')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'T' && c11 != 't')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 != 'E' && c12 != 'e')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if (c13 != 'C' && c13 != 'c')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 != 'T' && c14 != 't')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if (c15 != 'I' && c15 != 'i')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if (c16 == 'O' || c16 == 'o')
			{
				char c17 = buffer[startIndex++];
				if (c17 == 'N' || c17 == 'n')
				{
					result = StringToken.StalePageDetection;
				}
			}
			break;
		}
		case 'T':
		case 't':
		{
			char c = buffer[startIndex++];
			if (c != 'A' && c != 'a')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'R' && c2 != 'r')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'G' && c3 != 'g')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'E' && c4 != 'e')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if ((c5 != 'T' && c5 != 't') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'R' && c6 != 'r')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'E' && c7 != 'e')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'C' && c8 != 'c')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'O' && c9 != 'o')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'V' && c10 != 'v')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'E' && c11 != 'e')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 != 'R' && c12 != 'r')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if ((c13 != 'Y' && c13 != 'y') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 != 'T' && c14 != 't')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if (c15 != 'I' && c15 != 'i')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if (c16 == 'M' || c16 == 'm')
			{
				char c17 = buffer[startIndex++];
				if (c17 == 'E' || c17 == 'e')
				{
					result = StringToken.TargetRecoveryTime;
				}
			}
			break;
		}
		case 'W':
		case 'w':
		{
			char c = buffer[startIndex++];
			if (c != 'A' && c != 'a')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'I' && c2 != 'i')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if ((c3 != 'T' && c3 != 't') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'A' && c4 != 'a')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if ((c5 != 'T' && c5 != 't') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'L' && c6 != 'l')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'O' && c7 != 'o')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if ((c8 != 'W' && c8 != 'w') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'P' && c9 != 'p')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'R' && c10 != 'r')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'I' && c11 != 'i')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 != 'O' && c12 != 'o')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if (c13 != 'R' && c13 != 'r')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 != 'I' && c14 != 'i')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if (c15 == 'T' || c15 == 't')
			{
				char c16 = buffer[startIndex++];
				if (c16 == 'Y' || c16 == 'y')
				{
					result = StringToken.WaitAtLowPriority;
				}
			}
			break;
		}
		}
		return result;
	}

	private static StringToken Find_Len21(string buffer, int startIndex)
	{
		StringToken result = StringToken.None;
		switch (buffer[startIndex++])
		{
		case 'C':
		case 'c':
			switch (buffer[startIndex++])
			{
			case 'O':
			case 'o':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'L' && c2 != 'l')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'U' && c3 != 'u')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'M' && c4 != 'm')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if ((c5 != 'N' && c5 != 'n') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'E' && c6 != 'e')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'N' && c7 != 'n')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'C' && c8 != 'c')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'R' && c9 != 'r')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'Y' && c10 != 'y')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'P' && c11 != 'p')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'T' && c12 != 't')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 != 'I' && c13 != 'i')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if (c14 != 'O' && c14 != 'o')
				{
					break;
				}
				char c15 = buffer[startIndex++];
				if ((c15 != 'N' && c15 != 'n') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c16 = buffer[startIndex++];
				if (c16 != 'K' && c16 != 'k')
				{
					break;
				}
				char c17 = buffer[startIndex++];
				if (c17 == 'E' || c17 == 'e')
				{
					char c18 = buffer[startIndex++];
					if (c18 == 'Y' || c18 == 'y')
					{
						result = StringToken.ColumnEncryptionKey;
					}
				}
				break;
			}
			case 'R':
			case 'r':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'E' && c2 != 'e')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'A' && c3 != 'a')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'T' && c4 != 't')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if ((c5 != 'E' && c5 != 'e') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'F' && c6 != 'f')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'U' && c7 != 'u')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'L' && c8 != 'l')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'L' && c9 != 'l')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'T' && c10 != 't')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'E' && c11 != 'e')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'X' && c12 != 'x')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if ((c13 != 'T' && c13 != 't') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if (c14 != 'I' && c14 != 'i')
				{
					break;
				}
				char c15 = buffer[startIndex++];
				if (c15 != 'N' && c15 != 'n')
				{
					break;
				}
				char c16 = buffer[startIndex++];
				if (c16 != 'D' && c16 != 'd')
				{
					break;
				}
				char c17 = buffer[startIndex++];
				if (c17 == 'E' || c17 == 'e')
				{
					char c18 = buffer[startIndex++];
					if (c18 == 'X' || c18 == 'x')
					{
						result = StringToken.CreateFulltextIndex;
					}
				}
				break;
			}
			}
			break;
		case 'D':
		case 'd':
			switch (buffer[startIndex++])
			{
			case 'D':
			case 'd':
			{
				char c2 = buffer[startIndex++];
				if ((c2 != 'L' && c2 != 'l') || buffer[startIndex++] != '_')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case 'G':
				case 'g':
				{
					char c4 = buffer[startIndex++];
					if (c4 != 'D' && c4 != 'd')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if ((c5 != 'R' && c5 != 'r') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'S' && c6 != 's')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'E' && c7 != 'e')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'R' && c8 != 'r')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'V' && c9 != 'v')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'E' && c10 != 'e')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if ((c11 != 'R' && c11 != 'r') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if (c12 != 'E' && c12 != 'e')
					{
						break;
					}
					char c13 = buffer[startIndex++];
					if (c13 != 'V' && c13 != 'v')
					{
						break;
					}
					char c14 = buffer[startIndex++];
					if (c14 != 'E' && c14 != 'e')
					{
						break;
					}
					char c15 = buffer[startIndex++];
					if (c15 != 'N' && c15 != 'n')
					{
						break;
					}
					char c16 = buffer[startIndex++];
					if (c16 == 'T' || c16 == 't')
					{
						char c17 = buffer[startIndex++];
						if (c17 == 'S' || c17 == 's')
						{
							result = StringToken.DdlGdrServerEvents;
						}
					}
					break;
				}
				case 'P':
				case 'p':
				{
					char c4 = buffer[startIndex++];
					if (c4 != 'L' && c4 != 'l')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'A' && c5 != 'a')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if ((c6 != 'N' && c6 != 'n') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'G' && c7 != 'g')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'U' && c8 != 'u')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'I' && c9 != 'i')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'D' && c10 != 'd')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if ((c11 != 'E' && c11 != 'e') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if (c12 != 'E' && c12 != 'e')
					{
						break;
					}
					char c13 = buffer[startIndex++];
					if (c13 != 'V' && c13 != 'v')
					{
						break;
					}
					char c14 = buffer[startIndex++];
					if (c14 != 'E' && c14 != 'e')
					{
						break;
					}
					char c15 = buffer[startIndex++];
					if (c15 != 'N' && c15 != 'n')
					{
						break;
					}
					char c16 = buffer[startIndex++];
					if (c16 == 'T' || c16 == 't')
					{
						char c17 = buffer[startIndex++];
						if (c17 == 'S' || c17 == 's')
						{
							result = StringToken.DdlPlanGuideEvents;
						}
					}
					break;
				}
				case 'S':
				case 's':
				{
					char c4 = buffer[startIndex++];
					if (c4 != 'T' && c4 != 't')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'A' && c5 != 'a')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'T' && c6 != 't')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'I' && c7 != 'i')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'S' && c8 != 's')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'T' && c9 != 't')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'I' && c10 != 'i')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 != 'C' && c11 != 'c')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if ((c12 != 'S' && c12 != 's') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c13 = buffer[startIndex++];
					if (c13 != 'E' && c13 != 'e')
					{
						break;
					}
					char c14 = buffer[startIndex++];
					if (c14 != 'V' && c14 != 'v')
					{
						break;
					}
					char c15 = buffer[startIndex++];
					if (c15 != 'E' && c15 != 'e')
					{
						break;
					}
					char c16 = buffer[startIndex++];
					if (c16 != 'N' && c16 != 'n')
					{
						break;
					}
					char c17 = buffer[startIndex++];
					if (c17 == 'T' || c17 == 't')
					{
						char c18 = buffer[startIndex++];
						if (c18 == 'S' || c18 == 's')
						{
							result = StringToken.DdlStatisticsEvents;
						}
					}
					break;
				}
				case 'T':
				case 't':
				{
					char c4 = buffer[startIndex++];
					if (c4 != 'A' && c4 != 'a')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'B' && c5 != 'b')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'L' && c6 != 'l')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if ((c7 != 'E' && c7 != 'e') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'V' && c8 != 'v')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'I' && c9 != 'i')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'E' && c10 != 'e')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if ((c11 != 'W' && c11 != 'w') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if (c12 != 'E' && c12 != 'e')
					{
						break;
					}
					char c13 = buffer[startIndex++];
					if (c13 != 'V' && c13 != 'v')
					{
						break;
					}
					char c14 = buffer[startIndex++];
					if (c14 != 'E' && c14 != 'e')
					{
						break;
					}
					char c15 = buffer[startIndex++];
					if (c15 != 'N' && c15 != 'n')
					{
						break;
					}
					char c16 = buffer[startIndex++];
					if (c16 == 'T' || c16 == 't')
					{
						char c17 = buffer[startIndex++];
						if (c17 == 'S' || c17 == 's')
						{
							result = StringToken.DdlTableViewEvents;
						}
					}
					break;
				}
				}
				break;
			}
			case 'R':
			case 'r':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'O' && c2 != 'o')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if ((c3 != 'P' && c3 != 'p') || buffer[startIndex++] != '_')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case 'A':
				case 'a':
				{
					char c5 = buffer[startIndex++];
					if (c5 != 'P' && c5 != 'p')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'P' && c6 != 'p')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'L' && c7 != 'l')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'I' && c8 != 'i')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'C' && c9 != 'c')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'A' && c10 != 'a')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 != 'T' && c11 != 't')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if (c12 != 'I' && c12 != 'i')
					{
						break;
					}
					char c13 = buffer[startIndex++];
					if (c13 != 'O' && c13 != 'o')
					{
						break;
					}
					char c14 = buffer[startIndex++];
					if ((c14 != 'N' && c14 != 'n') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c15 = buffer[startIndex++];
					if (c15 != 'R' && c15 != 'r')
					{
						break;
					}
					char c16 = buffer[startIndex++];
					if (c16 != 'O' && c16 != 'o')
					{
						break;
					}
					char c17 = buffer[startIndex++];
					if (c17 == 'L' || c17 == 'l')
					{
						char c18 = buffer[startIndex++];
						if (c18 == 'E' || c18 == 'e')
						{
							result = StringToken.DropApplicationRole;
						}
					}
					break;
				}
				case 'F':
				case 'f':
				{
					char c5 = buffer[startIndex++];
					if (c5 != 'U' && c5 != 'u')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'L' && c6 != 'l')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'L' && c7 != 'l')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'T' && c8 != 't')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'E' && c9 != 'e')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'X' && c10 != 'x')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if ((c11 != 'T' && c11 != 't') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if (c12 != 'C' && c12 != 'c')
					{
						break;
					}
					char c13 = buffer[startIndex++];
					if (c13 != 'A' && c13 != 'a')
					{
						break;
					}
					char c14 = buffer[startIndex++];
					if (c14 != 'T' && c14 != 't')
					{
						break;
					}
					char c15 = buffer[startIndex++];
					if (c15 != 'A' && c15 != 'a')
					{
						break;
					}
					char c16 = buffer[startIndex++];
					if (c16 != 'L' && c16 != 'l')
					{
						break;
					}
					char c17 = buffer[startIndex++];
					if (c17 == 'O' || c17 == 'o')
					{
						char c18 = buffer[startIndex++];
						if (c18 == 'G' || c18 == 'g')
						{
							result = StringToken.DropFulltextCatalog;
						}
					}
					break;
				}
				case 'P':
				case 'p':
				{
					char c5 = buffer[startIndex++];
					if (c5 != 'A' && c5 != 'a')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'R' && c6 != 'r')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'T' && c7 != 't')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'I' && c8 != 'i')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'T' && c9 != 't')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'I' && c10 != 'i')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 != 'O' && c11 != 'o')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if ((c12 != 'N' && c12 != 'n') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c13 = buffer[startIndex++];
					if (c13 != 'S' && c13 != 's')
					{
						break;
					}
					char c14 = buffer[startIndex++];
					if (c14 != 'C' && c14 != 'c')
					{
						break;
					}
					char c15 = buffer[startIndex++];
					if (c15 != 'H' && c15 != 'h')
					{
						break;
					}
					char c16 = buffer[startIndex++];
					if (c16 != 'E' && c16 != 'e')
					{
						break;
					}
					char c17 = buffer[startIndex++];
					if (c17 == 'M' || c17 == 'm')
					{
						char c18 = buffer[startIndex++];
						if (c18 == 'E' || c18 == 'e')
						{
							result = StringToken.DropPartitionScheme;
						}
					}
					break;
				}
				}
				break;
			}
			}
			break;
		case 'E':
		case 'e':
		{
			char c = buffer[startIndex++];
			if (c != 'N' && c != 'n')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'V' && c2 != 'v')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'I' && c3 != 'i')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'R' && c4 != 'r')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'O' && c5 != 'o')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'N' && c6 != 'n')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'M' && c7 != 'm')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'E' && c8 != 'e')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'N' && c9 != 'n')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if ((c10 != 'T' && c10 != 't') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'V' && c11 != 'v')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 != 'A' && c12 != 'a')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if (c13 != 'R' && c13 != 'r')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 != 'I' && c14 != 'i')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if (c15 != 'A' && c15 != 'a')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if (c16 != 'B' && c16 != 'b')
			{
				break;
			}
			char c17 = buffer[startIndex++];
			if (c17 != 'L' && c17 != 'l')
			{
				break;
			}
			char c18 = buffer[startIndex++];
			if (c18 == 'E' || c18 == 'e')
			{
				char c19 = buffer[startIndex++];
				if (c19 == 'S' || c19 == 's')
				{
					result = StringToken.EnvironmentVariables;
				}
			}
			break;
		}
		case 'N':
		case 'n':
		{
			char c = buffer[startIndex++];
			if (c != 'O' && c != 'o')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if ((c2 != 'N' && c2 != 'n') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'T' && c3 != 't')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'R' && c4 != 'r')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'A' && c5 != 'a')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'N' && c6 != 'n')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'S' && c7 != 's')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'A' && c8 != 'a')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'C' && c9 != 'c')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'T' && c10 != 't')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'E' && c11 != 'e')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if ((c12 != 'D' && c12 != 'd') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if (c13 != 'A' && c13 != 'a')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 != 'C' && c14 != 'c')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if (c15 != 'C' && c15 != 'c')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if (c16 != 'E' && c16 != 'e')
			{
				break;
			}
			char c17 = buffer[startIndex++];
			if (c17 == 'S' || c17 == 's')
			{
				char c18 = buffer[startIndex++];
				if (c18 == 'S' || c18 == 's')
				{
					result = StringToken.NonTransactedAccess;
				}
			}
			break;
		}
		case 'P':
		case 'p':
		{
			char c = buffer[startIndex++];
			if (c != 'E' && c != 'e')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'R' && c2 != 'r')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'S' && c3 != 's')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'I' && c4 != 'i')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'S' && c5 != 's')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'T' && c6 != 't')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'E' && c7 != 'e')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'N' && c8 != 'n')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if ((c9 != 'T' && c9 != 't') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'L' && c10 != 'l')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'O' && c11 != 'o')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if ((c12 != 'G' && c12 != 'g') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if (c13 != 'B' && c13 != 'b')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 != 'U' && c14 != 'u')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if (c15 != 'F' && c15 != 'f')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if (c16 != 'F' && c16 != 'f')
			{
				break;
			}
			char c17 = buffer[startIndex++];
			if (c17 == 'E' || c17 == 'e')
			{
				char c18 = buffer[startIndex++];
				if (c18 == 'R' || c18 == 'r')
				{
					result = StringToken.PersistentLogBuffer;
				}
			}
			break;
		}
		case 'R':
		case 'r':
		{
			char c = buffer[startIndex++];
			if (c != 'E' && c != 'e')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'J' && c2 != 'j')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'E' && c3 != 'e')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'C' && c4 != 'c')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'T' && c5 != 't')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'E' && c6 != 'e')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if ((c7 != 'D' && c7 != 'd') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'R' && c8 != 'r')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'O' && c9 != 'o')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if ((c10 != 'W' && c10 != 'w') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'L' && c11 != 'l')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 != 'O' && c12 != 'o')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if (c13 != 'C' && c13 != 'c')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 != 'A' && c14 != 'a')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if (c15 != 'T' && c15 != 't')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if (c16 != 'I' && c16 != 'i')
			{
				break;
			}
			char c17 = buffer[startIndex++];
			if (c17 == 'O' || c17 == 'o')
			{
				char c18 = buffer[startIndex++];
				if (c18 == 'N' || c18 == 'n')
				{
					result = StringToken.RejectedRowLocation;
				}
			}
			break;
		}
		}
		return result;
	}

	private static StringToken Find_Len22(string buffer, int startIndex)
	{
		StringToken result = StringToken.None;
		switch (buffer[startIndex++])
		{
		case 'A':
		case 'a':
			switch (buffer[startIndex++])
			{
			case 'D':
			case 'd':
			{
				char c = buffer[startIndex++];
				if ((c != 'D' && c != 'd') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'S' && c2 != 's')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'E' && c3 != 'e')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'R' && c4 != 'r')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'V' && c5 != 'v')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'E' && c6 != 'e')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if ((c7 != 'R' && c7 != 'r') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'R' && c8 != 'r')
				{
					break;
				}
				char c19 = buffer[startIndex++];
				if (c19 != 'O' && c19 != 'o')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'L' && c9 != 'l')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if ((c10 != 'E' && c10 != 'e') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'M' && c11 != 'm')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'E' && c12 != 'e')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 != 'M' && c13 != 'm')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if (c14 != 'B' && c14 != 'b')
				{
					break;
				}
				char c15 = buffer[startIndex++];
				if (c15 == 'E' || c15 == 'e')
				{
					char c16 = buffer[startIndex++];
					if (c16 == 'R' || c16 == 'r')
					{
						result = StringToken.AddServerRoleMember;
					}
				}
				break;
			}
			case 'L':
			case 'l':
			{
				char c = buffer[startIndex++];
				if (c != 'T' && c != 't')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'E' && c2 != 'e')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if ((c3 != 'R' && c3 != 'r') || buffer[startIndex++] != '_')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case 'A':
				case 'a':
				{
					char c5 = buffer[startIndex++];
					if (c5 != 'P' && c5 != 'p')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'P' && c6 != 'p')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'L' && c7 != 'l')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'I' && c8 != 'i')
					{
						break;
					}
					char c19 = buffer[startIndex++];
					if (c19 != 'C' && c19 != 'c')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'A' && c9 != 'a')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'T' && c10 != 't')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 != 'I' && c11 != 'i')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if (c12 != 'O' && c12 != 'o')
					{
						break;
					}
					char c13 = buffer[startIndex++];
					if ((c13 != 'N' && c13 != 'n') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c14 = buffer[startIndex++];
					if (c14 != 'R' && c14 != 'r')
					{
						break;
					}
					char c15 = buffer[startIndex++];
					if (c15 != 'O' && c15 != 'o')
					{
						break;
					}
					char c16 = buffer[startIndex++];
					if (c16 == 'L' || c16 == 'l')
					{
						char c17 = buffer[startIndex++];
						if (c17 == 'E' || c17 == 'e')
						{
							result = StringToken.AlterApplicationRole;
						}
					}
					break;
				}
				case 'F':
				case 'f':
				{
					char c5 = buffer[startIndex++];
					if (c5 != 'U' && c5 != 'u')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'L' && c6 != 'l')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'L' && c7 != 'l')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'T' && c8 != 't')
					{
						break;
					}
					char c19 = buffer[startIndex++];
					if (c19 != 'E' && c19 != 'e')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'X' && c9 != 'x')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if ((c10 != 'T' && c10 != 't') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 != 'C' && c11 != 'c')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if (c12 != 'A' && c12 != 'a')
					{
						break;
					}
					char c13 = buffer[startIndex++];
					if (c13 != 'T' && c13 != 't')
					{
						break;
					}
					char c14 = buffer[startIndex++];
					if (c14 != 'A' && c14 != 'a')
					{
						break;
					}
					char c15 = buffer[startIndex++];
					if (c15 != 'L' && c15 != 'l')
					{
						break;
					}
					char c16 = buffer[startIndex++];
					if (c16 == 'O' || c16 == 'o')
					{
						char c17 = buffer[startIndex++];
						if (c17 == 'G' || c17 == 'g')
						{
							result = StringToken.AlterFulltextCatalog;
						}
					}
					break;
				}
				case 'P':
				case 'p':
				{
					char c5 = buffer[startIndex++];
					if (c5 != 'A' && c5 != 'a')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'R' && c6 != 'r')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'T' && c7 != 't')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'I' && c8 != 'i')
					{
						break;
					}
					char c19 = buffer[startIndex++];
					if (c19 != 'T' && c19 != 't')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'I' && c9 != 'i')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'O' && c10 != 'o')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if ((c11 != 'N' && c11 != 'n') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if (c12 != 'S' && c12 != 's')
					{
						break;
					}
					char c13 = buffer[startIndex++];
					if (c13 != 'C' && c13 != 'c')
					{
						break;
					}
					char c14 = buffer[startIndex++];
					if (c14 != 'H' && c14 != 'h')
					{
						break;
					}
					char c15 = buffer[startIndex++];
					if (c15 != 'E' && c15 != 'e')
					{
						break;
					}
					char c16 = buffer[startIndex++];
					if (c16 == 'M' || c16 == 'm')
					{
						char c17 = buffer[startIndex++];
						if (c17 == 'E' || c17 == 'e')
						{
							result = StringToken.AlterPartitionScheme;
						}
					}
					break;
				}
				}
				break;
			}
			case 'U':
			case 'u':
			{
				char c = buffer[startIndex++];
				if (c != 'T' && c != 't')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if ((c2 != 'O' && c2 != 'o') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'C' && c3 != 'c')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'R' && c4 != 'r')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'E' && c5 != 'e')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'A' && c6 != 'a')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'T' && c7 != 't')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if ((c8 != 'E' && c8 != 'e') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c19 = buffer[startIndex++];
				if (c19 != 'S' && c19 != 's')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'T' && c9 != 't')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'A' && c10 != 'a')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'T' && c11 != 't')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'I' && c12 != 'i')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 != 'S' && c13 != 's')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if (c14 != 'T' && c14 != 't')
				{
					break;
				}
				char c15 = buffer[startIndex++];
				if (c15 != 'I' && c15 != 'i')
				{
					break;
				}
				char c16 = buffer[startIndex++];
				if (c16 == 'C' || c16 == 'c')
				{
					char c17 = buffer[startIndex++];
					if (c17 == 'S' || c17 == 's')
					{
						result = StringToken.AutoCreateStatistics;
					}
				}
				break;
			}
			}
			break;
		case 'D':
		case 'd':
			switch (buffer[startIndex++])
			{
			case 'A':
			case 'a':
			{
				char c = buffer[startIndex++];
				if (c != 'T' && c != 't')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if ((c2 != 'A' && c2 != 'a') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'C' && c3 != 'c')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'O' && c4 != 'o')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'N' && c5 != 'n')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'S' && c6 != 's')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'I' && c7 != 'i')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'S' && c8 != 's')
				{
					break;
				}
				char c19 = buffer[startIndex++];
				if (c19 != 'T' && c19 != 't')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'E' && c9 != 'e')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'N' && c10 != 'n')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'C' && c11 != 'c')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if ((c12 != 'Y' && c12 != 'y') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 != 'C' && c13 != 'c')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if (c14 != 'H' && c14 != 'h')
				{
					break;
				}
				char c15 = buffer[startIndex++];
				if (c15 != 'E' && c15 != 'e')
				{
					break;
				}
				char c16 = buffer[startIndex++];
				if (c16 == 'C' || c16 == 'c')
				{
					char c17 = buffer[startIndex++];
					if (c17 == 'K' || c17 == 'k')
					{
						result = StringToken.DataConsistencyCheck;
					}
				}
				break;
			}
			case 'D':
			case 'd':
			{
				char c = buffer[startIndex++];
				if ((c != 'L' && c != 'l') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'C' && c2 != 'c')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'E' && c3 != 'e')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'R' && c4 != 'r')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'T' && c5 != 't')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'I' && c6 != 'i')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'F' && c7 != 'f')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'I' && c8 != 'i')
				{
					break;
				}
				char c19 = buffer[startIndex++];
				if (c19 != 'C' && c19 != 'c')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'A' && c9 != 'a')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'T' && c10 != 't')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if ((c11 != 'E' && c11 != 'e') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'E' && c12 != 'e')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 != 'V' && c13 != 'v')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if (c14 != 'E' && c14 != 'e')
				{
					break;
				}
				char c15 = buffer[startIndex++];
				if (c15 != 'N' && c15 != 'n')
				{
					break;
				}
				char c16 = buffer[startIndex++];
				if (c16 == 'T' || c16 == 't')
				{
					char c17 = buffer[startIndex++];
					if (c17 == 'S' || c17 == 's')
					{
						result = StringToken.DdlCertificateEvents;
					}
				}
				break;
			}
			case 'R':
			case 'r':
			{
				char c = buffer[startIndex++];
				if (c != 'O' && c != 'o')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if ((c2 != 'P' && c2 != 'p') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'E' && c3 != 'e')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'X' && c4 != 'x')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'T' && c5 != 't')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'E' && c6 != 'e')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'N' && c7 != 'n')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'D' && c8 != 'd')
				{
					break;
				}
				char c19 = buffer[startIndex++];
				if (c19 != 'E' && c19 != 'e')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if ((c9 != 'D' && c9 != 'd') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'P' && c10 != 'p')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'R' && c11 != 'r')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'O' && c12 != 'o')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 != 'P' && c13 != 'p')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if (c14 != 'E' && c14 != 'e')
				{
					break;
				}
				char c15 = buffer[startIndex++];
				if (c15 != 'R' && c15 != 'r')
				{
					break;
				}
				char c16 = buffer[startIndex++];
				if (c16 == 'T' || c16 == 't')
				{
					char c17 = buffer[startIndex++];
					if (c17 == 'Y' || c17 == 'y')
					{
						result = StringToken.DropExtendedProperty;
					}
				}
				break;
			}
			}
			break;
		case 'F':
		case 'f':
		{
			char c21 = buffer[startIndex++];
			if (c21 != 'L' && c21 != 'l')
			{
				break;
			}
			char c = buffer[startIndex++];
			if (c != 'U' && c != 'u')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'S' && c2 != 's')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if ((c3 != 'H' && c3 != 'h') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'I' && c4 != 'i')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'N' && c5 != 'n')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'T' && c6 != 't')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'E' && c7 != 'e')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'R' && c8 != 'r')
			{
				break;
			}
			char c19 = buffer[startIndex++];
			if (c19 != 'V' && c19 != 'v')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'A' && c9 != 'a')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if ((c10 != 'L' && c10 != 'l') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'S' && c11 != 's')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 != 'E' && c12 != 'e')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if (c13 != 'C' && c13 != 'c')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 != 'O' && c14 != 'o')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if (c15 != 'N' && c15 != 'n')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if (c16 == 'D' || c16 == 'd')
			{
				char c17 = buffer[startIndex++];
				if (c17 == 'S' || c17 == 's')
				{
					result = StringToken.FlushIntervalSeconds;
				}
			}
			break;
		}
		case 'S':
		case 's':
			switch (buffer[startIndex++])
			{
			case 'E':
			case 'e':
			{
				char c = buffer[startIndex++];
				if (c != 'M' && c != 'm')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'A' && c2 != 'a')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'N' && c3 != 'n')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'T' && c4 != 't')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'I' && c5 != 'i')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'C' && c6 != 'c')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'K' && c7 != 'k')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'E' && c8 != 'e')
				{
					break;
				}
				char c19 = buffer[startIndex++];
				if (c19 != 'Y' && c19 != 'y')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'P' && c9 != 'p')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'H' && c10 != 'h')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'R' && c11 != 'r')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'A' && c12 != 'a')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 != 'S' && c13 != 's')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if (c14 != 'E' && c14 != 'e')
				{
					break;
				}
				char c15 = buffer[startIndex++];
				if (c15 != 'T' && c15 != 't')
				{
					break;
				}
				char c16 = buffer[startIndex++];
				if (c16 != 'A' && c16 != 'a')
				{
					break;
				}
				char c17 = buffer[startIndex++];
				if (c17 != 'B' && c17 != 'b')
				{
					break;
				}
				char c18 = buffer[startIndex++];
				if (c18 == 'L' || c18 == 'l')
				{
					char c20 = buffer[startIndex++];
					if (c20 == 'E' || c20 == 'e')
					{
						result = StringToken.SemanticKeyPhraseTable;
					}
				}
				break;
			}
			case 'T':
			case 't':
			{
				char c = buffer[startIndex++];
				if (c != 'A' && c != 'a')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'T' && c2 != 't')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'I' && c3 != 'i')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'S' && c4 != 's')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'T' && c5 != 't')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'I' && c6 != 'i')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'C' && c7 != 'c')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if ((c8 != 'S' && c8 != 's') || buffer[startIndex++] != '_')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case 'I':
				case 'i':
				{
					char c9 = buffer[startIndex++];
					if (c9 != 'N' && c9 != 'n')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'C' && c10 != 'c')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 != 'R' && c11 != 'r')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if (c12 != 'E' && c12 != 'e')
					{
						break;
					}
					char c13 = buffer[startIndex++];
					if (c13 != 'M' && c13 != 'm')
					{
						break;
					}
					char c14 = buffer[startIndex++];
					if (c14 != 'E' && c14 != 'e')
					{
						break;
					}
					char c15 = buffer[startIndex++];
					if (c15 != 'N' && c15 != 'n')
					{
						break;
					}
					char c16 = buffer[startIndex++];
					if (c16 != 'T' && c16 != 't')
					{
						break;
					}
					char c17 = buffer[startIndex++];
					if (c17 == 'A' || c17 == 'a')
					{
						char c18 = buffer[startIndex++];
						if (c18 == 'L' || c18 == 'l')
						{
							result = StringToken.StatisticsIncremental;
						}
					}
					break;
				}
				case 'N':
				case 'n':
				{
					char c9 = buffer[startIndex++];
					if (c9 != 'O' && c9 != 'o')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'R' && c10 != 'r')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 != 'E' && c11 != 'e')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if (c12 != 'C' && c12 != 'c')
					{
						break;
					}
					char c13 = buffer[startIndex++];
					if (c13 != 'O' && c13 != 'o')
					{
						break;
					}
					char c14 = buffer[startIndex++];
					if (c14 != 'M' && c14 != 'm')
					{
						break;
					}
					char c15 = buffer[startIndex++];
					if (c15 != 'P' && c15 != 'p')
					{
						break;
					}
					char c16 = buffer[startIndex++];
					if (c16 != 'U' && c16 != 'u')
					{
						break;
					}
					char c17 = buffer[startIndex++];
					if (c17 == 'T' || c17 == 't')
					{
						char c18 = buffer[startIndex++];
						if (c18 == 'E' || c18 == 'e')
						{
							result = StringToken.StatisticsNoRecompute;
						}
					}
					break;
				}
				}
				break;
			}
			}
			break;
		}
		return result;
	}

	private static StringToken Find_Len23(string buffer, int startIndex)
	{
		StringToken result = StringToken.None;
		switch (buffer[startIndex++])
		{
		case 'C':
		case 'c':
			switch (buffer[startIndex++])
			{
			case 'H':
			case 'h':
			{
				char c = buffer[startIndex++];
				if (c != 'A' && c != 'a')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'N' && c2 != 'n')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'G' && c3 != 'g')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if ((c4 != 'E' && c4 != 'e') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'T' && c5 != 't')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'R' && c6 != 'r')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'A' && c7 != 'a')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'C' && c8 != 'c')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'K' && c9 != 'k')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'I' && c10 != 'i')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'N' && c11 != 'n')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if ((c12 != 'G' && c12 != 'g') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 != 'C' && c13 != 'c')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if (c14 != 'O' && c14 != 'o')
				{
					break;
				}
				char c15 = buffer[startIndex++];
				if (c15 != 'N' && c15 != 'n')
				{
					break;
				}
				char c16 = buffer[startIndex++];
				if (c16 != 'T' && c16 != 't')
				{
					break;
				}
				char c17 = buffer[startIndex++];
				if (c17 != 'E' && c17 != 'e')
				{
					break;
				}
				char c18 = buffer[startIndex++];
				if (c18 == 'X' || c18 == 'x')
				{
					char c19 = buffer[startIndex++];
					if (c19 == 'T' || c19 == 't')
					{
						result = StringToken.ChangeTrackingContext;
					}
				}
				break;
			}
			case 'O':
			case 'o':
			{
				char c = buffer[startIndex++];
				if (c != 'M' && c != 'm')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'P' && c2 != 'p')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'R' && c3 != 'r')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'E' && c4 != 'e')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'S' && c5 != 's')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if ((c6 != 'S' && c6 != 's') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'A' && c7 != 'a')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'L' && c8 != 'l')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if ((c9 != 'L' && c9 != 'l') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'R' && c10 != 'r')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'O' && c11 != 'o')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if ((c12 != 'W' && c12 != 'w') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 != 'G' && c13 != 'g')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if (c14 != 'R' && c14 != 'r')
				{
					break;
				}
				char c15 = buffer[startIndex++];
				if (c15 != 'O' && c15 != 'o')
				{
					break;
				}
				char c16 = buffer[startIndex++];
				if (c16 != 'U' && c16 != 'u')
				{
					break;
				}
				char c17 = buffer[startIndex++];
				if (c17 == 'P' || c17 == 'p')
				{
					char c18 = buffer[startIndex++];
					if (c18 == 'S' || c18 == 's')
					{
						result = StringToken.CompressAllRowGroups;
					}
				}
				break;
			}
			case 'R':
			case 'r':
			{
				char c = buffer[startIndex++];
				if (c != 'E' && c != 'e')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'A' && c2 != 'a')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'T' && c3 != 't')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if ((c4 != 'E' && c4 != 'e') || buffer[startIndex++] != '_')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case 'A':
				case 'a':
				{
					char c6 = buffer[startIndex++];
					if (c6 != 'P' && c6 != 'p')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'P' && c7 != 'p')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'L' && c8 != 'l')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'I' && c9 != 'i')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'C' && c10 != 'c')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 != 'A' && c11 != 'a')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if (c12 != 'T' && c12 != 't')
					{
						break;
					}
					char c13 = buffer[startIndex++];
					if (c13 != 'I' && c13 != 'i')
					{
						break;
					}
					char c14 = buffer[startIndex++];
					if (c14 != 'O' && c14 != 'o')
					{
						break;
					}
					char c15 = buffer[startIndex++];
					if ((c15 != 'N' && c15 != 'n') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c16 = buffer[startIndex++];
					if (c16 != 'R' && c16 != 'r')
					{
						break;
					}
					char c17 = buffer[startIndex++];
					if (c17 != 'O' && c17 != 'o')
					{
						break;
					}
					char c18 = buffer[startIndex++];
					if (c18 == 'L' || c18 == 'l')
					{
						char c19 = buffer[startIndex++];
						if (c19 == 'E' || c19 == 'e')
						{
							result = StringToken.CreateApplicationRole;
						}
					}
					break;
				}
				case 'F':
				case 'f':
				{
					char c6 = buffer[startIndex++];
					if (c6 != 'U' && c6 != 'u')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'L' && c7 != 'l')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'L' && c8 != 'l')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'T' && c9 != 't')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'E' && c10 != 'e')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 != 'X' && c11 != 'x')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if ((c12 != 'T' && c12 != 't') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c13 = buffer[startIndex++];
					if (c13 != 'C' && c13 != 'c')
					{
						break;
					}
					char c14 = buffer[startIndex++];
					if (c14 != 'A' && c14 != 'a')
					{
						break;
					}
					char c15 = buffer[startIndex++];
					if (c15 != 'T' && c15 != 't')
					{
						break;
					}
					char c16 = buffer[startIndex++];
					if (c16 != 'A' && c16 != 'a')
					{
						break;
					}
					char c17 = buffer[startIndex++];
					if (c17 != 'L' && c17 != 'l')
					{
						break;
					}
					char c18 = buffer[startIndex++];
					if (c18 == 'O' || c18 == 'o')
					{
						char c19 = buffer[startIndex++];
						if (c19 == 'G' || c19 == 'g')
						{
							result = StringToken.CreateFulltextCatalog;
						}
					}
					break;
				}
				case 'P':
				case 'p':
				{
					char c6 = buffer[startIndex++];
					if (c6 != 'A' && c6 != 'a')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'R' && c7 != 'r')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'T' && c8 != 't')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'I' && c9 != 'i')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'T' && c10 != 't')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 != 'I' && c11 != 'i')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if (c12 != 'O' && c12 != 'o')
					{
						break;
					}
					char c13 = buffer[startIndex++];
					if ((c13 != 'N' && c13 != 'n') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c14 = buffer[startIndex++];
					if (c14 != 'S' && c14 != 's')
					{
						break;
					}
					char c15 = buffer[startIndex++];
					if (c15 != 'C' && c15 != 'c')
					{
						break;
					}
					char c16 = buffer[startIndex++];
					if (c16 != 'H' && c16 != 'h')
					{
						break;
					}
					char c17 = buffer[startIndex++];
					if (c17 != 'E' && c17 != 'e')
					{
						break;
					}
					char c18 = buffer[startIndex++];
					if (c18 == 'M' || c18 == 'm')
					{
						char c19 = buffer[startIndex++];
						if (c19 == 'E' || c19 == 'e')
						{
							result = StringToken.CreatePartitionScheme;
						}
					}
					break;
				}
				}
				break;
			}
			}
			break;
		case 'D':
		case 'd':
			switch (buffer[startIndex++])
			{
			case 'D':
			case 'd':
			{
				char c = buffer[startIndex++];
				if ((c != 'L' && c != 'l') || buffer[startIndex++] != '_')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case 'G':
				case 'g':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'D' && c3 != 'd')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if ((c4 != 'R' && c4 != 'r') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'D' && c5 != 'd')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'A' && c6 != 'a')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'T' && c7 != 't')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'A' && c8 != 'a')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'B' && c9 != 'b')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'A' && c10 != 'a')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 != 'S' && c11 != 's')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if ((c12 != 'E' && c12 != 'e') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c13 = buffer[startIndex++];
					if (c13 != 'E' && c13 != 'e')
					{
						break;
					}
					char c14 = buffer[startIndex++];
					if (c14 != 'V' && c14 != 'v')
					{
						break;
					}
					char c15 = buffer[startIndex++];
					if (c15 != 'E' && c15 != 'e')
					{
						break;
					}
					char c16 = buffer[startIndex++];
					if (c16 != 'N' && c16 != 'n')
					{
						break;
					}
					char c17 = buffer[startIndex++];
					if (c17 == 'T' || c17 == 't')
					{
						char c18 = buffer[startIndex++];
						if (c18 == 'S' || c18 == 's')
						{
							result = StringToken.DdlGdrDatabaseEvents;
						}
					}
					break;
				}
				case 'M':
				case 'm':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'E' && c3 != 'e')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'S' && c4 != 's')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'S' && c5 != 's')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'A' && c6 != 'a')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'G' && c7 != 'g')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if ((c8 != 'E' && c8 != 'e') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'T' && c9 != 't')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'Y' && c10 != 'y')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 != 'P' && c11 != 'p')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if ((c12 != 'E' && c12 != 'e') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c13 = buffer[startIndex++];
					if (c13 != 'E' && c13 != 'e')
					{
						break;
					}
					char c14 = buffer[startIndex++];
					if (c14 != 'V' && c14 != 'v')
					{
						break;
					}
					char c15 = buffer[startIndex++];
					if (c15 != 'E' && c15 != 'e')
					{
						break;
					}
					char c16 = buffer[startIndex++];
					if (c16 != 'N' && c16 != 'n')
					{
						break;
					}
					char c17 = buffer[startIndex++];
					if (c17 == 'T' || c17 == 't')
					{
						char c18 = buffer[startIndex++];
						if (c18 == 'S' || c18 == 's')
						{
							result = StringToken.DdlMessageTypeEvents;
						}
					}
					break;
				}
				case 'S':
				case 's':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'E' && c3 != 'e')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'R' && c4 != 'r')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'V' && c5 != 'v')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'E' && c6 != 'e')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if ((c7 != 'R' && c7 != 'r') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'L' && c8 != 'l')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'E' && c9 != 'e')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'V' && c10 != 'v')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 != 'E' && c11 != 'e')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if ((c12 != 'L' && c12 != 'l') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c13 = buffer[startIndex++];
					if (c13 != 'E' && c13 != 'e')
					{
						break;
					}
					char c14 = buffer[startIndex++];
					if (c14 != 'V' && c14 != 'v')
					{
						break;
					}
					char c15 = buffer[startIndex++];
					if (c15 != 'E' && c15 != 'e')
					{
						break;
					}
					char c16 = buffer[startIndex++];
					if (c16 != 'N' && c16 != 'n')
					{
						break;
					}
					char c17 = buffer[startIndex++];
					if (c17 == 'T' || c17 == 't')
					{
						char c18 = buffer[startIndex++];
						if (c18 == 'S' || c18 == 's')
						{
							result = StringToken.DdlServerLevelEvents;
						}
					}
					break;
				}
				}
				break;
			}
			case 'R':
			case 'r':
			{
				char c = buffer[startIndex++];
				if (c != 'O' && c != 'o')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if ((c2 != 'P' && c2 != 'p') || buffer[startIndex++] != '_')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case 'E':
				case 'e':
					switch (buffer[startIndex++])
					{
					case 'V':
					case 'v':
					{
						char c5 = buffer[startIndex++];
						if (c5 != 'E' && c5 != 'e')
						{
							break;
						}
						char c6 = buffer[startIndex++];
						if (c6 != 'N' && c6 != 'n')
						{
							break;
						}
						char c7 = buffer[startIndex++];
						if ((c7 != 'T' && c7 != 't') || buffer[startIndex++] != '_')
						{
							break;
						}
						char c8 = buffer[startIndex++];
						if (c8 != 'N' && c8 != 'n')
						{
							break;
						}
						char c9 = buffer[startIndex++];
						if (c9 != 'O' && c9 != 'o')
						{
							break;
						}
						char c10 = buffer[startIndex++];
						if (c10 != 'T' && c10 != 't')
						{
							break;
						}
						char c11 = buffer[startIndex++];
						if (c11 != 'I' && c11 != 'i')
						{
							break;
						}
						char c12 = buffer[startIndex++];
						if (c12 != 'F' && c12 != 'f')
						{
							break;
						}
						char c13 = buffer[startIndex++];
						if (c13 != 'I' && c13 != 'i')
						{
							break;
						}
						char c14 = buffer[startIndex++];
						if (c14 != 'C' && c14 != 'c')
						{
							break;
						}
						char c15 = buffer[startIndex++];
						if (c15 != 'A' && c15 != 'a')
						{
							break;
						}
						char c16 = buffer[startIndex++];
						if (c16 != 'T' && c16 != 't')
						{
							break;
						}
						char c17 = buffer[startIndex++];
						if (c17 != 'I' && c17 != 'i')
						{
							break;
						}
						char c18 = buffer[startIndex++];
						if (c18 == 'O' || c18 == 'o')
						{
							char c19 = buffer[startIndex++];
							if (c19 == 'N' || c19 == 'n')
							{
								result = StringToken.DropEventNotification;
							}
						}
						break;
					}
					case 'X':
					case 'x':
					{
						char c5 = buffer[startIndex++];
						if (c5 != 'T' && c5 != 't')
						{
							break;
						}
						char c6 = buffer[startIndex++];
						if (c6 != 'E' && c6 != 'e')
						{
							break;
						}
						char c7 = buffer[startIndex++];
						if (c7 != 'N' && c7 != 'n')
						{
							break;
						}
						char c8 = buffer[startIndex++];
						if (c8 != 'D' && c8 != 'd')
						{
							break;
						}
						char c9 = buffer[startIndex++];
						if (c9 != 'E' && c9 != 'e')
						{
							break;
						}
						char c10 = buffer[startIndex++];
						if ((c10 != 'D' && c10 != 'd') || buffer[startIndex++] != '_')
						{
							break;
						}
						char c11 = buffer[startIndex++];
						if (c11 != 'P' && c11 != 'p')
						{
							break;
						}
						char c12 = buffer[startIndex++];
						if (c12 != 'R' && c12 != 'r')
						{
							break;
						}
						char c13 = buffer[startIndex++];
						if (c13 != 'O' && c13 != 'o')
						{
							break;
						}
						char c14 = buffer[startIndex++];
						if (c14 != 'C' && c14 != 'c')
						{
							break;
						}
						char c15 = buffer[startIndex++];
						if (c15 != 'E' && c15 != 'e')
						{
							break;
						}
						char c16 = buffer[startIndex++];
						if (c16 != 'D' && c16 != 'd')
						{
							break;
						}
						char c17 = buffer[startIndex++];
						if (c17 != 'U' && c17 != 'u')
						{
							break;
						}
						char c18 = buffer[startIndex++];
						if (c18 == 'R' || c18 == 'r')
						{
							char c19 = buffer[startIndex++];
							if (c19 == 'E' || c19 == 'e')
							{
								result = StringToken.DropExtendedProcedure;
							}
						}
						break;
					}
					}
					break;
				case 'P':
				case 'p':
				{
					char c4 = buffer[startIndex++];
					if (c4 != 'A' && c4 != 'a')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'R' && c5 != 'r')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'T' && c6 != 't')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'I' && c7 != 'i')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'T' && c8 != 't')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'I' && c9 != 'i')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'O' && c10 != 'o')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if ((c11 != 'N' && c11 != 'n') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if (c12 != 'F' && c12 != 'f')
					{
						break;
					}
					char c13 = buffer[startIndex++];
					if (c13 != 'U' && c13 != 'u')
					{
						break;
					}
					char c14 = buffer[startIndex++];
					if (c14 != 'N' && c14 != 'n')
					{
						break;
					}
					char c15 = buffer[startIndex++];
					if (c15 != 'C' && c15 != 'c')
					{
						break;
					}
					char c16 = buffer[startIndex++];
					if (c16 != 'T' && c16 != 't')
					{
						break;
					}
					char c17 = buffer[startIndex++];
					if (c17 != 'I' && c17 != 'i')
					{
						break;
					}
					char c18 = buffer[startIndex++];
					if (c18 == 'O' || c18 == 'o')
					{
						char c19 = buffer[startIndex++];
						if (c19 == 'N' || c19 == 'n')
						{
							result = StringToken.DropPartitionFunction;
						}
					}
					break;
				}
				case 'S':
				case 's':
				{
					char c4 = buffer[startIndex++];
					if (c4 != 'E' && c4 != 'e')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'R' && c5 != 'r')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'V' && c6 != 'v')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'E' && c7 != 'e')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if ((c8 != 'R' && c8 != 'r') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'R' && c9 != 'r')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'O' && c10 != 'o')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 != 'L' && c11 != 'l')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if ((c12 != 'E' && c12 != 'e') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c13 = buffer[startIndex++];
					if (c13 != 'M' && c13 != 'm')
					{
						break;
					}
					char c14 = buffer[startIndex++];
					if (c14 != 'E' && c14 != 'e')
					{
						break;
					}
					char c15 = buffer[startIndex++];
					if (c15 != 'M' && c15 != 'm')
					{
						break;
					}
					char c16 = buffer[startIndex++];
					if (c16 != 'B' && c16 != 'b')
					{
						break;
					}
					char c17 = buffer[startIndex++];
					if (c17 == 'E' || c17 == 'e')
					{
						char c18 = buffer[startIndex++];
						if (c18 == 'R' || c18 == 'r')
						{
							result = StringToken.DropServerRoleMember;
						}
					}
					break;
				}
				}
				break;
			}
			}
			break;
		case 'I':
		case 'i':
		{
			char c22 = buffer[startIndex++];
			if (c22 != 'N' && c22 != 'n')
			{
				break;
			}
			char c = buffer[startIndex++];
			if (c != 'T' && c != 't')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'E' && c2 != 'e')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'R' && c3 != 'r')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'V' && c4 != 'v')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'A' && c5 != 'a')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if ((c6 != 'L' && c6 != 'l') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'L' && c7 != 'l')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'E' && c8 != 'e')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'N' && c9 != 'n')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'G' && c10 != 'g')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'T' && c11 != 't')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if ((c12 != 'H' && c12 != 'h') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if (c13 != 'M' && c13 != 'm')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 != 'I' && c14 != 'i')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if (c15 != 'N' && c15 != 'n')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if (c16 != 'U' && c16 != 'u')
			{
				break;
			}
			char c17 = buffer[startIndex++];
			if (c17 != 'T' && c17 != 't')
			{
				break;
			}
			char c18 = buffer[startIndex++];
			if (c18 == 'E' || c18 == 'e')
			{
				char c19 = buffer[startIndex++];
				if (c19 == 'S' || c19 == 's')
				{
					result = StringToken.IntervalLengthMinutes;
				}
			}
			break;
		}
		case 'K':
		case 'k':
		{
			char c22 = buffer[startIndex++];
			if (c22 != 'E' && c22 != 'e')
			{
				break;
			}
			char c = buffer[startIndex++];
			if ((c != 'Y' && c != 'y') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'S' && c2 != 's')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'T' && c3 != 't')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'O' && c4 != 'o')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'R' && c5 != 'r')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if ((c6 != 'E' && c6 != 'e') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'P' && c7 != 'p')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'R' && c8 != 'r')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'O' && c9 != 'o')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'V' && c10 != 'v')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'I' && c11 != 'i')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 != 'D' && c12 != 'd')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if (c13 != 'E' && c13 != 'e')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if ((c14 != 'R' && c14 != 'r') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if (c15 != 'N' && c15 != 'n')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if (c16 != 'A' && c16 != 'a')
			{
				break;
			}
			char c17 = buffer[startIndex++];
			if (c17 == 'M' || c17 == 'm')
			{
				char c18 = buffer[startIndex++];
				if (c18 == 'E' || c18 == 'e')
				{
					result = StringToken.KeyStoreProviderName;
				}
			}
			break;
		}
		case 'S':
		case 's':
			switch (buffer[startIndex++])
			{
			case 'E':
			case 'e':
			{
				char c = buffer[startIndex++];
				if (c != 'M' && c != 'm')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'A' && c2 != 'a')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'N' && c3 != 'n')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'T' && c4 != 't')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'I' && c5 != 'i')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'C' && c6 != 'c')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'S' && c7 != 's')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'I' && c8 != 'i')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'M' && c9 != 'm')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'I' && c10 != 'i')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'L' && c11 != 'l')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'A' && c12 != 'a')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 != 'R' && c13 != 'r')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if (c14 != 'I' && c14 != 'i')
				{
					break;
				}
				char c15 = buffer[startIndex++];
				if (c15 != 'T' && c15 != 't')
				{
					break;
				}
				char c16 = buffer[startIndex++];
				if (c16 != 'Y' && c16 != 'y')
				{
					break;
				}
				char c17 = buffer[startIndex++];
				if (c17 != 'T' && c17 != 't')
				{
					break;
				}
				char c18 = buffer[startIndex++];
				if (c18 != 'A' && c18 != 'a')
				{
					break;
				}
				char c19 = buffer[startIndex++];
				if (c19 != 'B' && c19 != 'b')
				{
					break;
				}
				char c20 = buffer[startIndex++];
				if (c20 == 'L' || c20 == 'l')
				{
					char c21 = buffer[startIndex++];
					if (c21 == 'E' || c21 == 'e')
					{
						result = StringToken.SemanticSimilarityTable;
					}
				}
				break;
			}
			case 'I':
			case 'i':
			{
				char c = buffer[startIndex++];
				if (c != 'Z' && c != 'z')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if ((c2 != 'E' && c2 != 'e') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'B' && c3 != 'b')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'A' && c4 != 'a')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'S' && c5 != 's')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'E' && c6 != 'e')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if ((c7 != 'D' && c7 != 'd') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'C' && c8 != 'c')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'L' && c9 != 'l')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'E' && c10 != 'e')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'A' && c11 != 'a')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'N' && c12 != 'n')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 != 'U' && c13 != 'u')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if ((c14 != 'P' && c14 != 'p') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c15 = buffer[startIndex++];
				if (c15 != 'M' && c15 != 'm')
				{
					break;
				}
				char c16 = buffer[startIndex++];
				if (c16 != 'O' && c16 != 'o')
				{
					break;
				}
				char c17 = buffer[startIndex++];
				if (c17 == 'D' || c17 == 'd')
				{
					char c18 = buffer[startIndex++];
					if (c18 == 'E' || c18 == 'e')
					{
						result = StringToken.SizeBasedCleanupMode;
					}
				}
				break;
			}
			}
			break;
		}
		return result;
	}

	private static StringToken Find_Len24(string buffer, int startIndex)
	{
		StringToken result = StringToken.None;
		switch (buffer[startIndex++])
		{
		case 'A':
		case 'a':
		{
			char c = buffer[startIndex++];
			if (c != 'L' && c != 'l')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'T' && c2 != 't')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'E' && c3 != 'e')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if ((c4 != 'R' && c4 != 'r') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'P' && c5 != 'p')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'A' && c6 != 'a')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'R' && c7 != 'r')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'T' && c8 != 't')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'I' && c9 != 'i')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'T' && c10 != 't')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'I' && c11 != 'i')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 != 'O' && c12 != 'o')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if ((c13 != 'N' && c13 != 'n') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 != 'F' && c14 != 'f')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if (c15 != 'U' && c15 != 'u')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if (c16 != 'N' && c16 != 'n')
			{
				break;
			}
			char c17 = buffer[startIndex++];
			if (c17 != 'C' && c17 != 'c')
			{
				break;
			}
			char c18 = buffer[startIndex++];
			if (c18 != 'T' && c18 != 't')
			{
				break;
			}
			char c19 = buffer[startIndex++];
			if (c19 != 'I' && c19 != 'i')
			{
				break;
			}
			char c20 = buffer[startIndex++];
			if (c20 == 'O' || c20 == 'o')
			{
				char c21 = buffer[startIndex++];
				if (c21 == 'N' || c21 == 'n')
				{
					result = StringToken.AlterPartitionFunction;
				}
			}
			break;
		}
		case 'C':
		case 'c':
		{
			char c = buffer[startIndex++];
			if (c != 'R' && c != 'r')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'E' && c2 != 'e')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'A' && c3 != 'a')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'T' && c4 != 't')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if ((c5 != 'E' && c5 != 'e') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'E' && c6 != 'e')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'X' && c7 != 'x')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'T' && c8 != 't')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'E' && c9 != 'e')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'N' && c10 != 'n')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'D' && c11 != 'd')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 != 'E' && c12 != 'e')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if ((c13 != 'D' && c13 != 'd') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 != 'P' && c14 != 'p')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if (c15 != 'R' && c15 != 'r')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if (c16 != 'O' && c16 != 'o')
			{
				break;
			}
			char c17 = buffer[startIndex++];
			if (c17 != 'P' && c17 != 'p')
			{
				break;
			}
			char c18 = buffer[startIndex++];
			if (c18 != 'E' && c18 != 'e')
			{
				break;
			}
			char c19 = buffer[startIndex++];
			if (c19 != 'R' && c19 != 'r')
			{
				break;
			}
			char c20 = buffer[startIndex++];
			if (c20 == 'T' || c20 == 't')
			{
				char c21 = buffer[startIndex++];
				if (c21 == 'Y' || c21 == 'y')
				{
					result = StringToken.CreateExtendedProperty;
				}
			}
			break;
		}
		case 'D':
		case 'd':
			switch (buffer[startIndex++])
			{
			case 'D':
			case 'd':
			{
				char c2 = buffer[startIndex++];
				if ((c2 != 'L' && c2 != 'l') || buffer[startIndex++] != '_')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case 'L':
				case 'l':
				{
					char c4 = buffer[startIndex++];
					if (c4 != 'I' && c4 != 'i')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'N' && c5 != 'n')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'K' && c6 != 'k')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'E' && c7 != 'e')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if ((c8 != 'D' && c8 != 'd') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'S' && c9 != 's')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'E' && c10 != 'e')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 != 'R' && c11 != 'r')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if (c12 != 'V' && c12 != 'v')
					{
						break;
					}
					char c13 = buffer[startIndex++];
					if (c13 != 'E' && c13 != 'e')
					{
						break;
					}
					char c14 = buffer[startIndex++];
					if ((c14 != 'R' && c14 != 'r') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c15 = buffer[startIndex++];
					if (c15 != 'E' && c15 != 'e')
					{
						break;
					}
					char c16 = buffer[startIndex++];
					if (c16 != 'V' && c16 != 'v')
					{
						break;
					}
					char c17 = buffer[startIndex++];
					if (c17 != 'E' && c17 != 'e')
					{
						break;
					}
					char c18 = buffer[startIndex++];
					if (c18 != 'N' && c18 != 'n')
					{
						break;
					}
					char c19 = buffer[startIndex++];
					if (c19 == 'T' || c19 == 't')
					{
						char c20 = buffer[startIndex++];
						if (c20 == 'S' || c20 == 's')
						{
							result = StringToken.DdlLinkedServerEvents;
						}
					}
					break;
				}
				case 'R':
				case 'r':
				{
					char c4 = buffer[startIndex++];
					if (c4 != 'E' && c4 != 'e')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'M' && c5 != 'm')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'O' && c6 != 'o')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'T' && c7 != 't')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if ((c8 != 'E' && c8 != 'e') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'S' && c9 != 's')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'E' && c10 != 'e')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 != 'R' && c11 != 'r')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if (c12 != 'V' && c12 != 'v')
					{
						break;
					}
					char c13 = buffer[startIndex++];
					if (c13 != 'E' && c13 != 'e')
					{
						break;
					}
					char c14 = buffer[startIndex++];
					if ((c14 != 'R' && c14 != 'r') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c15 = buffer[startIndex++];
					if (c15 != 'E' && c15 != 'e')
					{
						break;
					}
					char c16 = buffer[startIndex++];
					if (c16 != 'V' && c16 != 'v')
					{
						break;
					}
					char c17 = buffer[startIndex++];
					if (c17 != 'E' && c17 != 'e')
					{
						break;
					}
					char c18 = buffer[startIndex++];
					if (c18 != 'N' && c18 != 'n')
					{
						break;
					}
					char c19 = buffer[startIndex++];
					if (c19 == 'T' || c19 == 't')
					{
						char c20 = buffer[startIndex++];
						if (c20 == 'S' || c20 == 's')
						{
							result = StringToken.DdlRemoteServerEvents;
						}
					}
					break;
				}
				}
				break;
			}
			case 'R':
			case 'r':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'O' && c2 != 'o')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if ((c3 != 'P' && c3 != 'p') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'L' && c4 != 'l')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'I' && c5 != 'i')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'N' && c6 != 'n')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'K' && c7 != 'k')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'E' && c8 != 'e')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if ((c9 != 'D' && c9 != 'd') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'S' && c10 != 's')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'E' && c11 != 'e')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'R' && c12 != 'r')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 != 'V' && c13 != 'v')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if (c14 != 'E' && c14 != 'e')
				{
					break;
				}
				char c15 = buffer[startIndex++];
				if ((c15 != 'R' && c15 != 'r') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c16 = buffer[startIndex++];
				if (c16 != 'L' && c16 != 'l')
				{
					break;
				}
				char c17 = buffer[startIndex++];
				if (c17 != 'O' && c17 != 'o')
				{
					break;
				}
				char c18 = buffer[startIndex++];
				if (c18 != 'G' && c18 != 'g')
				{
					break;
				}
				char c19 = buffer[startIndex++];
				if (c19 == 'I' || c19 == 'i')
				{
					char c20 = buffer[startIndex++];
					if (c20 == 'N' || c20 == 'n')
					{
						result = StringToken.DropLinkedServerLogin;
					}
				}
				break;
			}
			}
			break;
		case 'H':
		case 'h':
		{
			char c = buffer[startIndex++];
			if (c != 'I' && c != 'i')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'S' && c2 != 's')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'T' && c3 != 't')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'O' && c4 != 'o')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'R' && c5 != 'r')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if ((c6 != 'Y' && c6 != 'y') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'R' && c7 != 'r')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'E' && c8 != 'e')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'T' && c9 != 't')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'E' && c10 != 'e')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'N' && c11 != 'n')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 != 'T' && c12 != 't')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if (c13 != 'I' && c13 != 'i')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 != 'O' && c14 != 'o')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if ((c15 != 'N' && c15 != 'n') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if (c16 != 'P' && c16 != 'p')
			{
				break;
			}
			char c17 = buffer[startIndex++];
			if (c17 != 'E' && c17 != 'e')
			{
				break;
			}
			char c18 = buffer[startIndex++];
			if (c18 != 'R' && c18 != 'r')
			{
				break;
			}
			char c19 = buffer[startIndex++];
			if (c19 != 'I' && c19 != 'i')
			{
				break;
			}
			char c20 = buffer[startIndex++];
			if (c20 == 'O' || c20 == 'o')
			{
				char c21 = buffer[startIndex++];
				if (c21 == 'D' || c21 == 'd')
				{
					result = StringToken.HistoryRetentionPeriod;
				}
			}
			break;
		}
		case 'Q':
		case 'q':
		{
			char c = buffer[startIndex++];
			if (c != 'U' && c != 'u')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'E' && c2 != 'e')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'R' && c3 != 'r')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if ((c4 != 'Y' && c4 != 'y') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'O' && c5 != 'o')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'P' && c6 != 'p')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'T' && c7 != 't')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'I' && c8 != 'i')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'M' && c9 != 'm')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'I' && c10 != 'i')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'Z' && c11 != 'z')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 != 'E' && c12 != 'e')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if ((c13 != 'R' && c13 != 'r') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 != 'H' && c14 != 'h')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if (c15 != 'O' && c15 != 'o')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if (c16 != 'T' && c16 != 't')
			{
				break;
			}
			char c17 = buffer[startIndex++];
			if (c17 != 'F' && c17 != 'f')
			{
				break;
			}
			char c18 = buffer[startIndex++];
			if (c18 != 'I' && c18 != 'i')
			{
				break;
			}
			char c19 = buffer[startIndex++];
			if (c19 != 'X' && c19 != 'x')
			{
				break;
			}
			char c20 = buffer[startIndex++];
			if (c20 == 'E' || c20 == 'e')
			{
				char c21 = buffer[startIndex++];
				if (c21 == 'S' || c21 == 's')
				{
					result = StringToken.QueryOptimizerHotfixes;
				}
			}
			break;
		}
		case 'S':
		case 's':
		{
			char c = buffer[startIndex++];
			if (c != 'P' && c != 'p')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'A' && c2 != 'a')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'T' && c3 != 't')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'I' && c4 != 'i')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'A' && c5 != 'a')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if ((c6 != 'L' && c6 != 'l') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'W' && c7 != 'w')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'I' && c8 != 'i')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'N' && c9 != 'n')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'D' && c10 != 'd')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'O' && c11 != 'o')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if ((c12 != 'W' && c12 != 'w') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if (c13 != 'M' && c13 != 'm')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 != 'A' && c14 != 'a')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if ((c15 != 'X' && c15 != 'x') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if (c16 != 'C' && c16 != 'c')
			{
				break;
			}
			char c17 = buffer[startIndex++];
			if (c17 != 'E' && c17 != 'e')
			{
				break;
			}
			char c18 = buffer[startIndex++];
			if (c18 != 'L' && c18 != 'l')
			{
				break;
			}
			char c19 = buffer[startIndex++];
			if (c19 == 'L' || c19 == 'l')
			{
				char c20 = buffer[startIndex++];
				if (c20 == 'S' || c20 == 's')
				{
					result = StringToken.SpatialWindowMaxCells;
				}
			}
			break;
		}
		}
		return result;
	}

	private static StringToken Find_Len25(string buffer, int startIndex)
	{
		StringToken result = StringToken.None;
		switch (buffer[startIndex++])
		{
		case 'B':
		case 'b':
		{
			char c = buffer[startIndex++];
			if (c != 'A' && c != 'a')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'C' && c2 != 'c')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'K' && c3 != 'k')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'U' && c4 != 'u')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if ((c5 != 'P' && c5 != 'p') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'S' && c6 != 's')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'T' && c7 != 't')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'O' && c8 != 'o')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'R' && c9 != 'r')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'A' && c10 != 'a')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'G' && c11 != 'g')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if ((c12 != 'E' && c12 != 'e') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if (c13 != 'R' && c13 != 'r')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 != 'E' && c14 != 'e')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if (c15 != 'D' && c15 != 'd')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if (c16 != 'U' && c16 != 'u')
			{
				break;
			}
			char c17 = buffer[startIndex++];
			if (c17 != 'N' && c17 != 'n')
			{
				break;
			}
			char c18 = buffer[startIndex++];
			if (c18 != 'D' && c18 != 'd')
			{
				break;
			}
			char c19 = buffer[startIndex++];
			if (c19 != 'A' && c19 != 'a')
			{
				break;
			}
			char c20 = buffer[startIndex++];
			if (c20 != 'N' && c20 != 'n')
			{
				break;
			}
			char c21 = buffer[startIndex++];
			if (c21 == 'C' || c21 == 'c')
			{
				char c22 = buffer[startIndex++];
				if (c22 == 'Y' || c22 == 'y')
				{
					result = StringToken.BackupStorageRedundancy;
				}
			}
			break;
		}
		case 'C':
		case 'c':
		{
			char c = buffer[startIndex++];
			if (c != 'R' && c != 'r')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'E' && c2 != 'e')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'A' && c3 != 'a')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'T' && c4 != 't')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if ((c5 != 'E' && c5 != 'e') || buffer[startIndex++] != '_')
			{
				break;
			}
			switch (buffer[startIndex++])
			{
			case 'E':
			case 'e':
				switch (buffer[startIndex++])
				{
				case 'V':
				case 'v':
				{
					char c8 = buffer[startIndex++];
					if (c8 != 'E' && c8 != 'e')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'N' && c9 != 'n')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if ((c10 != 'T' && c10 != 't') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 != 'N' && c11 != 'n')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if (c12 != 'O' && c12 != 'o')
					{
						break;
					}
					char c13 = buffer[startIndex++];
					if (c13 != 'T' && c13 != 't')
					{
						break;
					}
					char c14 = buffer[startIndex++];
					if (c14 != 'I' && c14 != 'i')
					{
						break;
					}
					char c15 = buffer[startIndex++];
					if (c15 != 'F' && c15 != 'f')
					{
						break;
					}
					char c16 = buffer[startIndex++];
					if (c16 != 'I' && c16 != 'i')
					{
						break;
					}
					char c17 = buffer[startIndex++];
					if (c17 != 'C' && c17 != 'c')
					{
						break;
					}
					char c18 = buffer[startIndex++];
					if (c18 != 'A' && c18 != 'a')
					{
						break;
					}
					char c19 = buffer[startIndex++];
					if (c19 != 'T' && c19 != 't')
					{
						break;
					}
					char c20 = buffer[startIndex++];
					if (c20 != 'I' && c20 != 'i')
					{
						break;
					}
					char c21 = buffer[startIndex++];
					if (c21 == 'O' || c21 == 'o')
					{
						char c22 = buffer[startIndex++];
						if (c22 == 'N' || c22 == 'n')
						{
							result = StringToken.CreateEventNotification;
						}
					}
					break;
				}
				case 'X':
				case 'x':
				{
					char c8 = buffer[startIndex++];
					if (c8 != 'T' && c8 != 't')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'E' && c9 != 'e')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'N' && c10 != 'n')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 != 'D' && c11 != 'd')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if (c12 != 'E' && c12 != 'e')
					{
						break;
					}
					char c13 = buffer[startIndex++];
					if ((c13 != 'D' && c13 != 'd') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c14 = buffer[startIndex++];
					if (c14 != 'P' && c14 != 'p')
					{
						break;
					}
					char c15 = buffer[startIndex++];
					if (c15 != 'R' && c15 != 'r')
					{
						break;
					}
					char c16 = buffer[startIndex++];
					if (c16 != 'O' && c16 != 'o')
					{
						break;
					}
					char c17 = buffer[startIndex++];
					if (c17 != 'C' && c17 != 'c')
					{
						break;
					}
					char c18 = buffer[startIndex++];
					if (c18 != 'E' && c18 != 'e')
					{
						break;
					}
					char c19 = buffer[startIndex++];
					if (c19 != 'D' && c19 != 'd')
					{
						break;
					}
					char c20 = buffer[startIndex++];
					if (c20 != 'U' && c20 != 'u')
					{
						break;
					}
					char c21 = buffer[startIndex++];
					if (c21 == 'R' || c21 == 'r')
					{
						char c22 = buffer[startIndex++];
						if (c22 == 'E' || c22 == 'e')
						{
							result = StringToken.CreateExtendedProcedure;
						}
					}
					break;
				}
				}
				break;
			case 'P':
			case 'p':
			{
				char c7 = buffer[startIndex++];
				if (c7 != 'A' && c7 != 'a')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'R' && c8 != 'r')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'T' && c9 != 't')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'I' && c10 != 'i')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'T' && c11 != 't')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'I' && c12 != 'i')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 != 'O' && c13 != 'o')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if ((c14 != 'N' && c14 != 'n') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c15 = buffer[startIndex++];
				if (c15 != 'F' && c15 != 'f')
				{
					break;
				}
				char c16 = buffer[startIndex++];
				if (c16 != 'U' && c16 != 'u')
				{
					break;
				}
				char c17 = buffer[startIndex++];
				if (c17 != 'N' && c17 != 'n')
				{
					break;
				}
				char c18 = buffer[startIndex++];
				if (c18 != 'C' && c18 != 'c')
				{
					break;
				}
				char c19 = buffer[startIndex++];
				if (c19 != 'T' && c19 != 't')
				{
					break;
				}
				char c20 = buffer[startIndex++];
				if (c20 != 'I' && c20 != 'i')
				{
					break;
				}
				char c21 = buffer[startIndex++];
				if (c21 == 'O' || c21 == 'o')
				{
					char c22 = buffer[startIndex++];
					if (c22 == 'N' || c22 == 'n')
					{
						result = StringToken.CreatePartitionFunction;
					}
				}
				break;
			}
			}
			break;
		}
		case 'D':
		case 'd':
		{
			char c = buffer[startIndex++];
			if (c != 'D' && c != 'd')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if ((c2 != 'L' && c2 != 'l') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'D' && c3 != 'd')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'A' && c4 != 'a')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'T' && c5 != 't')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'A' && c6 != 'a')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'B' && c7 != 'b')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'A' && c8 != 'a')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'S' && c9 != 's')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if ((c10 != 'E' && c10 != 'e') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'L' && c11 != 'l')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 != 'E' && c12 != 'e')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if (c13 != 'V' && c13 != 'v')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 != 'E' && c14 != 'e')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if ((c15 != 'L' && c15 != 'l') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if (c16 != 'E' && c16 != 'e')
			{
				break;
			}
			char c17 = buffer[startIndex++];
			if (c17 != 'V' && c17 != 'v')
			{
				break;
			}
			char c18 = buffer[startIndex++];
			if (c18 != 'E' && c18 != 'e')
			{
				break;
			}
			char c19 = buffer[startIndex++];
			if (c19 != 'N' && c19 != 'n')
			{
				break;
			}
			char c20 = buffer[startIndex++];
			if (c20 == 'T' || c20 == 't')
			{
				char c21 = buffer[startIndex++];
				if (c21 == 'S' || c21 == 's')
				{
					result = StringToken.DdlDatabaseLevelEvents;
				}
			}
			break;
		}
		case 'F':
		case 'f':
		{
			char c = buffer[startIndex++];
			if (c != 'E' && c != 'e')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'D' && c2 != 'd')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'E' && c3 != 'e')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'R' && c4 != 'r')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'A' && c5 != 'a')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'T' && c6 != 't')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'E' && c7 != 'e')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if ((c8 != 'D' && c8 != 'd') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'S' && c9 != 's')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'E' && c10 != 'e')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'R' && c11 != 'r')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 != 'V' && c12 != 'v')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if (c13 != 'I' && c13 != 'i')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 != 'C' && c14 != 'c')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if ((c15 != 'E' && c15 != 'e') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if (c16 != 'A' && c16 != 'a')
			{
				break;
			}
			char c17 = buffer[startIndex++];
			if (c17 != 'C' && c17 != 'c')
			{
				break;
			}
			char c18 = buffer[startIndex++];
			if (c18 != 'C' && c18 != 'c')
			{
				break;
			}
			char c19 = buffer[startIndex++];
			if (c19 != 'O' && c19 != 'o')
			{
				break;
			}
			char c20 = buffer[startIndex++];
			if (c20 != 'U' && c20 != 'u')
			{
				break;
			}
			char c21 = buffer[startIndex++];
			if (c21 == 'N' || c21 == 'n')
			{
				char c22 = buffer[startIndex++];
				if (c22 == 'T' || c22 == 't')
				{
					result = StringToken.FederatedServiceAccount;
				}
			}
			break;
		}
		case 'R':
		case 'r':
		{
			char c = buffer[startIndex++];
			if (c != 'E' && c != 'e')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'S' && c2 != 's')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'O' && c3 != 'o')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'U' && c4 != 'u')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'R' && c5 != 'r')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'C' && c6 != 'c')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if ((c7 != 'E' && c7 != 'e') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'M' && c8 != 'm')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'A' && c9 != 'a')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'N' && c10 != 'n')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'A' && c11 != 'a')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 != 'G' && c12 != 'g')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if (c13 != 'E' && c13 != 'e')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if ((c14 != 'R' && c14 != 'r') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if (c15 != 'L' && c15 != 'l')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if (c16 != 'O' && c16 != 'o')
			{
				break;
			}
			char c17 = buffer[startIndex++];
			if (c17 != 'C' && c17 != 'c')
			{
				break;
			}
			char c18 = buffer[startIndex++];
			if (c18 != 'A' && c18 != 'a')
			{
				break;
			}
			char c19 = buffer[startIndex++];
			if (c19 != 'T' && c19 != 't')
			{
				break;
			}
			char c20 = buffer[startIndex++];
			if (c20 != 'I' && c20 != 'i')
			{
				break;
			}
			char c21 = buffer[startIndex++];
			if (c21 == 'O' || c21 == 'o')
			{
				char c22 = buffer[startIndex++];
				if (c22 == 'N' || c22 == 'n')
				{
					result = StringToken.ResourceManagerLocation;
				}
			}
			break;
		}
		}
		return result;
	}

	private static StringToken Find_Len26(string buffer, int startIndex)
	{
		StringToken result = StringToken.None;
		switch (buffer[startIndex++])
		{
		case 'A':
		case 'a':
		{
			char c23 = buffer[startIndex++];
			if (c23 != 'L' && c23 != 'l')
			{
				break;
			}
			char c = buffer[startIndex++];
			if (c != 'T' && c != 't')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'E' && c2 != 'e')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if ((c3 != 'R' && c3 != 'r') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'A' && c4 != 'a')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'U' && c5 != 'u')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'T' && c6 != 't')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'H' && c7 != 'h')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'O' && c8 != 'o')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'R' && c9 != 'r')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'I' && c10 != 'i')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'Z' && c11 != 'z')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 != 'A' && c12 != 'a')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if (c13 != 'T' && c13 != 't')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 != 'I' && c14 != 'i')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if (c15 != 'O' && c15 != 'o')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if ((c16 != 'N' && c16 != 'n') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c17 = buffer[startIndex++];
			if (c17 != 'S' && c17 != 's')
			{
				break;
			}
			char c18 = buffer[startIndex++];
			if (c18 != 'E' && c18 != 'e')
			{
				break;
			}
			char c19 = buffer[startIndex++];
			if (c19 != 'R' && c19 != 'r')
			{
				break;
			}
			char c20 = buffer[startIndex++];
			if (c20 != 'V' && c20 != 'v')
			{
				break;
			}
			char c21 = buffer[startIndex++];
			if (c21 == 'E' || c21 == 'e')
			{
				char c22 = buffer[startIndex++];
				if (c22 == 'R' || c22 == 'r')
				{
					result = StringToken.AlterAuthorizationServer;
				}
			}
			break;
		}
		case 'C':
		case 'c':
		{
			char c23 = buffer[startIndex++];
			if (c23 != 'R' && c23 != 'r')
			{
				break;
			}
			char c = buffer[startIndex++];
			if (c != 'E' && c != 'e')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'A' && c2 != 'a')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'T' && c3 != 't')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if ((c4 != 'E' && c4 != 'e') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'L' && c5 != 'l')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'I' && c6 != 'i')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'N' && c7 != 'n')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'K' && c8 != 'k')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'E' && c9 != 'e')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if ((c10 != 'D' && c10 != 'd') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'S' && c11 != 's')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 != 'E' && c12 != 'e')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if (c13 != 'R' && c13 != 'r')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 != 'V' && c14 != 'v')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if (c15 != 'E' && c15 != 'e')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if ((c16 != 'R' && c16 != 'r') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c17 = buffer[startIndex++];
			if (c17 != 'L' && c17 != 'l')
			{
				break;
			}
			char c18 = buffer[startIndex++];
			if (c18 != 'O' && c18 != 'o')
			{
				break;
			}
			char c19 = buffer[startIndex++];
			if (c19 != 'G' && c19 != 'g')
			{
				break;
			}
			char c20 = buffer[startIndex++];
			if (c20 == 'I' || c20 == 'i')
			{
				char c21 = buffer[startIndex++];
				if (c21 == 'N' || c21 == 'n')
				{
					result = StringToken.CreateLinkedServerLogin;
				}
			}
			break;
		}
		case 'D':
		case 'd':
			switch (buffer[startIndex++])
			{
			case 'D':
			case 'd':
			{
				char c = buffer[startIndex++];
				if ((c != 'L' && c != 'l') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'S' && c2 != 's')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'E' && c3 != 'e')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'R' && c4 != 'r')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'V' && c5 != 'v')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'E' && c6 != 'e')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if ((c7 != 'R' && c7 != 'r') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'S' && c8 != 's')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'E' && c9 != 'e')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'C' && c10 != 'c')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'U' && c11 != 'u')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'R' && c12 != 'r')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 != 'I' && c13 != 'i')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if (c14 != 'T' && c14 != 't')
				{
					break;
				}
				char c15 = buffer[startIndex++];
				if ((c15 != 'Y' && c15 != 'y') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c16 = buffer[startIndex++];
				if (c16 != 'E' && c16 != 'e')
				{
					break;
				}
				char c17 = buffer[startIndex++];
				if (c17 != 'V' && c17 != 'v')
				{
					break;
				}
				char c18 = buffer[startIndex++];
				if (c18 != 'E' && c18 != 'e')
				{
					break;
				}
				char c19 = buffer[startIndex++];
				if (c19 != 'N' && c19 != 'n')
				{
					break;
				}
				char c20 = buffer[startIndex++];
				if (c20 == 'T' || c20 == 't')
				{
					char c21 = buffer[startIndex++];
					if (c21 == 'S' || c21 == 's')
					{
						result = StringToken.DdlServerSecurityEvents;
					}
				}
				break;
			}
			case 'R':
			case 'r':
			{
				char c = buffer[startIndex++];
				if (c != 'O' && c != 'o')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if ((c2 != 'P' && c2 != 'p') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'X' && c3 != 'x')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'M' && c4 != 'm')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if ((c5 != 'L' && c5 != 'l') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'S' && c6 != 's')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'C' && c7 != 'c')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'H' && c8 != 'h')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'E' && c9 != 'e')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'M' && c10 != 'm')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if ((c11 != 'A' && c11 != 'a') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'C' && c12 != 'c')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 != 'O' && c13 != 'o')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if (c14 != 'L' && c14 != 'l')
				{
					break;
				}
				char c15 = buffer[startIndex++];
				if (c15 != 'L' && c15 != 'l')
				{
					break;
				}
				char c16 = buffer[startIndex++];
				if (c16 != 'E' && c16 != 'e')
				{
					break;
				}
				char c17 = buffer[startIndex++];
				if (c17 != 'C' && c17 != 'c')
				{
					break;
				}
				char c18 = buffer[startIndex++];
				if (c18 != 'T' && c18 != 't')
				{
					break;
				}
				char c19 = buffer[startIndex++];
				if (c19 != 'I' && c19 != 'i')
				{
					break;
				}
				char c20 = buffer[startIndex++];
				if (c20 == 'O' || c20 == 'o')
				{
					char c21 = buffer[startIndex++];
					if (c21 == 'N' || c21 == 'n')
					{
						result = StringToken.DropXmlSchemaCollection;
					}
				}
				break;
			}
			}
			break;
		case 'F':
		case 'f':
		{
			char c23 = buffer[startIndex++];
			if (c23 != 'I' && c23 != 'i')
			{
				break;
			}
			char c = buffer[startIndex++];
			if (c != 'L' && c != 'l')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'E' && c2 != 'e')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'T' && c3 != 't')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'A' && c4 != 'a')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'B' && c5 != 'b')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'L' && c6 != 'l')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if ((c7 != 'E' && c7 != 'e') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'C' && c8 != 'c')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'O' && c9 != 'o')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'L' && c10 != 'l')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'L' && c11 != 'l')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 != 'A' && c12 != 'a')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if (c13 != 'T' && c13 != 't')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if ((c14 != 'E' && c14 != 'e') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if (c15 != 'F' && c15 != 'f')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if (c16 != 'I' && c16 != 'i')
			{
				break;
			}
			char c17 = buffer[startIndex++];
			if (c17 != 'L' && c17 != 'l')
			{
				break;
			}
			char c18 = buffer[startIndex++];
			if (c18 != 'E' && c18 != 'e')
			{
				break;
			}
			char c19 = buffer[startIndex++];
			if (c19 != 'N' && c19 != 'n')
			{
				break;
			}
			char c20 = buffer[startIndex++];
			if (c20 != 'A' && c20 != 'a')
			{
				break;
			}
			char c21 = buffer[startIndex++];
			if (c21 == 'M' || c21 == 'm')
			{
				char c22 = buffer[startIndex++];
				if (c22 == 'E' || c22 == 'e')
				{
					result = StringToken.FileTableCollateFilename;
				}
			}
			break;
		}
		case 'O':
		case 'o':
		{
			char c23 = buffer[startIndex++];
			if (c23 != 'P' && c23 != 'p')
			{
				break;
			}
			char c = buffer[startIndex++];
			if (c != 'E' && c != 'e')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'R' && c2 != 'r')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'A' && c3 != 'a')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'T' && c4 != 't')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'I' && c5 != 'i')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'O' && c6 != 'o')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if ((c7 != 'N' && c7 != 'n') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'T' && c8 != 't')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'Y' && c9 != 'y')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'P' && c10 != 'p')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if ((c11 != 'E' && c11 != 'e') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 != 'C' && c12 != 'c')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if (c13 != 'O' && c13 != 'o')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 != 'L' && c14 != 'l')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if (c15 != 'U' && c15 != 'u')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if (c16 != 'M' && c16 != 'm')
			{
				break;
			}
			char c17 = buffer[startIndex++];
			if ((c17 != 'N' && c17 != 'n') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c18 = buffer[startIndex++];
			if (c18 != 'N' && c18 != 'n')
			{
				break;
			}
			char c19 = buffer[startIndex++];
			if (c19 != 'A' && c19 != 'a')
			{
				break;
			}
			char c20 = buffer[startIndex++];
			if (c20 == 'M' || c20 == 'm')
			{
				char c21 = buffer[startIndex++];
				if (c21 == 'E' || c21 == 'e')
				{
					result = StringToken.OperationTypeColumnName;
				}
			}
			break;
		}
		case 'T':
		case 't':
			switch (buffer[startIndex++])
			{
			case 'E':
			case 'e':
			{
				char c = buffer[startIndex++];
				if (c != 'M' && c != 'm')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'P' && c2 != 'p')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'O' && c3 != 'o')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'R' && c4 != 'r')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'A' && c5 != 'a')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if ((c6 != 'L' && c6 != 'l') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'H' && c7 != 'h')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'I' && c8 != 'i')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'S' && c9 != 's')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'T' && c10 != 't')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'O' && c11 != 'o')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'R' && c12 != 'r')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if ((c13 != 'Y' && c13 != 'y') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if (c14 != 'R' && c14 != 'r')
				{
					break;
				}
				char c15 = buffer[startIndex++];
				if (c15 != 'E' && c15 != 'e')
				{
					break;
				}
				char c16 = buffer[startIndex++];
				if (c16 != 'T' && c16 != 't')
				{
					break;
				}
				char c17 = buffer[startIndex++];
				if (c17 != 'E' && c17 != 'e')
				{
					break;
				}
				char c18 = buffer[startIndex++];
				if (c18 != 'N' && c18 != 'n')
				{
					break;
				}
				char c19 = buffer[startIndex++];
				if (c19 != 'T' && c19 != 't')
				{
					break;
				}
				char c20 = buffer[startIndex++];
				if (c20 != 'I' && c20 != 'i')
				{
					break;
				}
				char c21 = buffer[startIndex++];
				if (c21 == 'O' || c21 == 'o')
				{
					char c22 = buffer[startIndex++];
					if (c22 == 'N' || c22 == 'n')
					{
						result = StringToken.TemporalHistoryRetention;
					}
				}
				break;
			}
			case 'R':
			case 'r':
			{
				char c = buffer[startIndex++];
				if (c != 'A' && c != 'a')
				{
					break;
				}
				char c2 = buffer[startIndex++];
				if (c2 != 'N' && c2 != 'n')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'S' && c3 != 's')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'A' && c4 != 'a')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'C' && c5 != 'c')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'T' && c6 != 't')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'I' && c7 != 'i')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'O' && c8 != 'o')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if ((c9 != 'N' && c9 != 'n') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'I' && c10 != 'i')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if ((c11 != 'D' && c11 != 'd') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'C' && c12 != 'c')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 != 'O' && c13 != 'o')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if (c14 != 'L' && c14 != 'l')
				{
					break;
				}
				char c15 = buffer[startIndex++];
				if (c15 != 'U' && c15 != 'u')
				{
					break;
				}
				char c16 = buffer[startIndex++];
				if (c16 != 'M' && c16 != 'm')
				{
					break;
				}
				char c17 = buffer[startIndex++];
				if ((c17 != 'N' && c17 != 'n') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c18 = buffer[startIndex++];
				if (c18 != 'N' && c18 != 'n')
				{
					break;
				}
				char c19 = buffer[startIndex++];
				if (c19 != 'A' && c19 != 'a')
				{
					break;
				}
				char c20 = buffer[startIndex++];
				if (c20 == 'M' || c20 == 'm')
				{
					char c21 = buffer[startIndex++];
					if (c21 == 'E' || c21 == 'e')
					{
						result = StringToken.TransactionIdColumnName;
					}
				}
				break;
			}
			}
			break;
		}
		return result;
	}

	private static StringToken Find_Len27(string buffer, int startIndex)
	{
		StringToken result = StringToken.None;
		switch (buffer[startIndex++])
		{
		case 'A':
		case 'a':
		{
			char c = buffer[startIndex++];
			if (c != 'L' && c != 'l')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'T' && c2 != 't')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'E' && c3 != 'e')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if ((c4 != 'R' && c4 != 'r') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'X' && c5 != 'x')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'M' && c6 != 'm')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if ((c7 != 'L' && c7 != 'l') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'S' && c8 != 's')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'C' && c9 != 'c')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'H' && c10 != 'h')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'E' && c11 != 'e')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 != 'M' && c12 != 'm')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if ((c13 != 'A' && c13 != 'a') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 != 'C' && c14 != 'c')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if (c15 != 'O' && c15 != 'o')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if (c16 != 'L' && c16 != 'l')
			{
				break;
			}
			char c17 = buffer[startIndex++];
			if (c17 != 'L' && c17 != 'l')
			{
				break;
			}
			char c18 = buffer[startIndex++];
			if (c18 != 'E' && c18 != 'e')
			{
				break;
			}
			char c19 = buffer[startIndex++];
			if (c19 != 'C' && c19 != 'c')
			{
				break;
			}
			char c20 = buffer[startIndex++];
			if (c20 != 'T' && c20 != 't')
			{
				break;
			}
			char c21 = buffer[startIndex++];
			if (c21 != 'I' && c21 != 'i')
			{
				break;
			}
			char c22 = buffer[startIndex++];
			if (c22 == 'O' || c22 == 'o')
			{
				char c23 = buffer[startIndex++];
				if (c23 == 'N' || c23 == 'n')
				{
					result = StringToken.AlterXmlSchemaCollection;
				}
			}
			break;
		}
		case 'D':
		case 'd':
			switch (buffer[startIndex++])
			{
			case 'A':
			case 'a':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'T' && c2 != 't')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if ((c3 != 'A' && c3 != 'a') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'F' && c4 != 'f')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'L' && c5 != 'l')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'U' && c6 != 'u')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'S' && c7 != 's')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if ((c8 != 'H' && c8 != 'h') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'I' && c9 != 'i')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'N' && c10 != 'n')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'T' && c11 != 't')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'E' && c12 != 'e')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 != 'R' && c13 != 'r')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if (c14 != 'V' && c14 != 'v')
				{
					break;
				}
				char c15 = buffer[startIndex++];
				if (c15 != 'A' && c15 != 'a')
				{
					break;
				}
				char c16 = buffer[startIndex++];
				if ((c16 != 'L' && c16 != 'l') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c17 = buffer[startIndex++];
				if (c17 != 'S' && c17 != 's')
				{
					break;
				}
				char c18 = buffer[startIndex++];
				if (c18 != 'E' && c18 != 'e')
				{
					break;
				}
				char c19 = buffer[startIndex++];
				if (c19 != 'C' && c19 != 'c')
				{
					break;
				}
				char c20 = buffer[startIndex++];
				if (c20 != 'O' && c20 != 'o')
				{
					break;
				}
				char c21 = buffer[startIndex++];
				if (c21 != 'N' && c21 != 'n')
				{
					break;
				}
				char c22 = buffer[startIndex++];
				if (c22 == 'D' || c22 == 'd')
				{
					char c23 = buffer[startIndex++];
					if (c23 == 'S' || c23 == 's')
					{
						result = StringToken.DataFlushIntervalSeconds;
					}
				}
				break;
			}
			case 'D':
			case 'd':
			{
				char c2 = buffer[startIndex++];
				if ((c2 != 'L' && c2 != 'l') || buffer[startIndex++] != '_')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case 'A':
				case 'a':
				{
					char c4 = buffer[startIndex++];
					if (c4 != 'P' && c4 != 'p')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'P' && c5 != 'p')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'L' && c6 != 'l')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'I' && c7 != 'i')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'C' && c8 != 'c')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'A' && c9 != 'a')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'T' && c10 != 't')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 != 'I' && c11 != 'i')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if (c12 != 'O' && c12 != 'o')
					{
						break;
					}
					char c13 = buffer[startIndex++];
					if ((c13 != 'N' && c13 != 'n') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c14 = buffer[startIndex++];
					if (c14 != 'R' && c14 != 'r')
					{
						break;
					}
					char c15 = buffer[startIndex++];
					if (c15 != 'O' && c15 != 'o')
					{
						break;
					}
					char c16 = buffer[startIndex++];
					if (c16 != 'L' && c16 != 'l')
					{
						break;
					}
					char c17 = buffer[startIndex++];
					if ((c17 != 'E' && c17 != 'e') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c18 = buffer[startIndex++];
					if (c18 != 'E' && c18 != 'e')
					{
						break;
					}
					char c19 = buffer[startIndex++];
					if (c19 != 'V' && c19 != 'v')
					{
						break;
					}
					char c20 = buffer[startIndex++];
					if (c20 != 'E' && c20 != 'e')
					{
						break;
					}
					char c21 = buffer[startIndex++];
					if (c21 != 'N' && c21 != 'n')
					{
						break;
					}
					char c22 = buffer[startIndex++];
					if (c22 == 'T' || c22 == 't')
					{
						char c23 = buffer[startIndex++];
						if (c23 == 'S' || c23 == 's')
						{
							result = StringToken.DdlApplicationRoleEvents;
						}
					}
					break;
				}
				case 'F':
				case 'f':
				{
					char c4 = buffer[startIndex++];
					if (c4 != 'U' && c4 != 'u')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'L' && c5 != 'l')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'L' && c6 != 'l')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'T' && c7 != 't')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'E' && c8 != 'e')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'X' && c9 != 'x')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if ((c10 != 'T' && c10 != 't') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 != 'C' && c11 != 'c')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if (c12 != 'A' && c12 != 'a')
					{
						break;
					}
					char c13 = buffer[startIndex++];
					if (c13 != 'T' && c13 != 't')
					{
						break;
					}
					char c14 = buffer[startIndex++];
					if (c14 != 'A' && c14 != 'a')
					{
						break;
					}
					char c15 = buffer[startIndex++];
					if (c15 != 'L' && c15 != 'l')
					{
						break;
					}
					char c16 = buffer[startIndex++];
					if (c16 != 'O' && c16 != 'o')
					{
						break;
					}
					char c17 = buffer[startIndex++];
					if ((c17 != 'G' && c17 != 'g') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c18 = buffer[startIndex++];
					if (c18 != 'E' && c18 != 'e')
					{
						break;
					}
					char c19 = buffer[startIndex++];
					if (c19 != 'V' && c19 != 'v')
					{
						break;
					}
					char c20 = buffer[startIndex++];
					if (c20 != 'E' && c20 != 'e')
					{
						break;
					}
					char c21 = buffer[startIndex++];
					if (c21 != 'N' && c21 != 'n')
					{
						break;
					}
					char c22 = buffer[startIndex++];
					if (c22 == 'T' || c22 == 't')
					{
						char c23 = buffer[startIndex++];
						if (c23 == 'S' || c23 == 's')
						{
							result = StringToken.DdlFulltextCatalogEvents;
						}
					}
					break;
				}
				case 'P':
				case 'p':
				{
					char c4 = buffer[startIndex++];
					if (c4 != 'A' && c4 != 'a')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'R' && c5 != 'r')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'T' && c6 != 't')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'I' && c7 != 'i')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'T' && c8 != 't')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'I' && c9 != 'i')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'O' && c10 != 'o')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if ((c11 != 'N' && c11 != 'n') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if (c12 != 'S' && c12 != 's')
					{
						break;
					}
					char c13 = buffer[startIndex++];
					if (c13 != 'C' && c13 != 'c')
					{
						break;
					}
					char c14 = buffer[startIndex++];
					if (c14 != 'H' && c14 != 'h')
					{
						break;
					}
					char c15 = buffer[startIndex++];
					if (c15 != 'E' && c15 != 'e')
					{
						break;
					}
					char c16 = buffer[startIndex++];
					if (c16 != 'M' && c16 != 'm')
					{
						break;
					}
					char c17 = buffer[startIndex++];
					if ((c17 != 'E' && c17 != 'e') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c18 = buffer[startIndex++];
					if (c18 != 'E' && c18 != 'e')
					{
						break;
					}
					char c19 = buffer[startIndex++];
					if (c19 != 'V' && c19 != 'v')
					{
						break;
					}
					char c20 = buffer[startIndex++];
					if (c20 != 'E' && c20 != 'e')
					{
						break;
					}
					char c21 = buffer[startIndex++];
					if (c21 != 'N' && c21 != 'n')
					{
						break;
					}
					char c22 = buffer[startIndex++];
					if (c22 == 'T' || c22 == 't')
					{
						char c23 = buffer[startIndex++];
						if (c23 == 'S' || c23 == 's')
						{
							result = StringToken.DdlPartitionSchemeEvents;
						}
					}
					break;
				}
				}
				break;
			}
			case 'R':
			case 'r':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'O' && c2 != 'o')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if ((c3 != 'P' && c3 != 'p') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'R' && c4 != 'r')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'E' && c5 != 'e')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'M' && c6 != 'm')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'O' && c7 != 'o')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'T' && c8 != 't')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if ((c9 != 'E' && c9 != 'e') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'S' && c10 != 's')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'E' && c11 != 'e')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'R' && c12 != 'r')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 != 'V' && c13 != 'v')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if (c14 != 'I' && c14 != 'i')
				{
					break;
				}
				char c15 = buffer[startIndex++];
				if (c15 != 'C' && c15 != 'c')
				{
					break;
				}
				char c16 = buffer[startIndex++];
				if ((c16 != 'E' && c16 != 'e') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c17 = buffer[startIndex++];
				if (c17 != 'B' && c17 != 'b')
				{
					break;
				}
				char c18 = buffer[startIndex++];
				if (c18 != 'I' && c18 != 'i')
				{
					break;
				}
				char c19 = buffer[startIndex++];
				if (c19 != 'N' && c19 != 'n')
				{
					break;
				}
				char c20 = buffer[startIndex++];
				if (c20 != 'D' && c20 != 'd')
				{
					break;
				}
				char c21 = buffer[startIndex++];
				if (c21 != 'I' && c21 != 'i')
				{
					break;
				}
				char c22 = buffer[startIndex++];
				if (c22 == 'N' || c22 == 'n')
				{
					char c23 = buffer[startIndex++];
					if (c23 == 'G' || c23 == 'g')
					{
						result = StringToken.DropRemoteServiceBinding;
					}
				}
				break;
			}
			}
			break;
		case 'O':
		case 'o':
		{
			char c = buffer[startIndex++];
			if (c != 'P' && c != 'p')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'T' && c2 != 't')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'I' && c3 != 'i')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'M' && c4 != 'm')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'I' && c5 != 'i')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'Z' && c6 != 'z')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if ((c7 != 'E' && c7 != 'e') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'F' && c8 != 'f')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'O' && c9 != 'o')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if ((c10 != 'R' && c10 != 'r') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'S' && c11 != 's')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 != 'E' && c12 != 'e')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if (c13 != 'Q' && c13 != 'q')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 != 'U' && c14 != 'u')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if (c15 != 'E' && c15 != 'e')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if (c16 != 'N' && c16 != 'n')
			{
				break;
			}
			char c17 = buffer[startIndex++];
			if (c17 != 'T' && c17 != 't')
			{
				break;
			}
			char c18 = buffer[startIndex++];
			if (c18 != 'I' && c18 != 'i')
			{
				break;
			}
			char c19 = buffer[startIndex++];
			if (c19 != 'A' && c19 != 'a')
			{
				break;
			}
			char c20 = buffer[startIndex++];
			if ((c20 != 'L' && c20 != 'l') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c21 = buffer[startIndex++];
			if (c21 != 'K' && c21 != 'k')
			{
				break;
			}
			char c22 = buffer[startIndex++];
			if (c22 == 'E' || c22 == 'e')
			{
				char c23 = buffer[startIndex++];
				if (c23 == 'Y' || c23 == 'y')
				{
					result = StringToken.OptimizeForSequentialKey;
				}
			}
			break;
		}
		case 'S':
		case 's':
		{
			char c = buffer[startIndex++];
			if (c != 'E' && c != 'e')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'Q' && c2 != 'q')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'U' && c3 != 'u')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'E' && c4 != 'e')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'N' && c5 != 'n')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'C' && c6 != 'c')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if ((c7 != 'E' && c7 != 'e') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'N' && c8 != 'n')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'U' && c9 != 'u')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'M' && c10 != 'm')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'B' && c11 != 'b')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 != 'E' && c12 != 'e')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if ((c13 != 'R' && c13 != 'r') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 != 'C' && c14 != 'c')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if (c15 != 'O' && c15 != 'o')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if (c16 != 'L' && c16 != 'l')
			{
				break;
			}
			char c17 = buffer[startIndex++];
			if (c17 != 'U' && c17 != 'u')
			{
				break;
			}
			char c18 = buffer[startIndex++];
			if (c18 != 'M' && c18 != 'm')
			{
				break;
			}
			char c19 = buffer[startIndex++];
			if ((c19 != 'N' && c19 != 'n') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c20 = buffer[startIndex++];
			if (c20 != 'N' && c20 != 'n')
			{
				break;
			}
			char c21 = buffer[startIndex++];
			if (c21 != 'A' && c21 != 'a')
			{
				break;
			}
			char c22 = buffer[startIndex++];
			if (c22 == 'M' || c22 == 'm')
			{
				char c23 = buffer[startIndex++];
				if (c23 == 'E' || c23 == 'e')
				{
					result = StringToken.SequenceNumberColumnName;
				}
			}
			break;
		}
		}
		return result;
	}

	private static StringToken Find_Len28(string buffer, int startIndex)
	{
		StringToken result = StringToken.None;
		switch (buffer[startIndex++])
		{
		case 'A':
		case 'a':
		{
			char c = buffer[startIndex++];
			if (c != 'L' && c != 'l')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'T' && c2 != 't')
			{
				break;
			}
			char c24 = buffer[startIndex++];
			if (c24 != 'E' && c24 != 'e')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if ((c3 != 'R' && c3 != 'r') || buffer[startIndex++] != '_')
			{
				break;
			}
			switch (buffer[startIndex++])
			{
			case 'A':
			case 'a':
			{
				char c5 = buffer[startIndex++];
				if (c5 != 'U' && c5 != 'u')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'T' && c6 != 't')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'H' && c7 != 'h')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'O' && c8 != 'o')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'R' && c9 != 'r')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'I' && c10 != 'i')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'Z' && c11 != 'z')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'A' && c12 != 'a')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 != 'T' && c13 != 't')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if (c14 != 'I' && c14 != 'i')
				{
					break;
				}
				char c15 = buffer[startIndex++];
				if (c15 != 'O' && c15 != 'o')
				{
					break;
				}
				char c16 = buffer[startIndex++];
				if ((c16 != 'N' && c16 != 'n') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c17 = buffer[startIndex++];
				if (c17 != 'D' && c17 != 'd')
				{
					break;
				}
				char c18 = buffer[startIndex++];
				if (c18 != 'A' && c18 != 'a')
				{
					break;
				}
				char c19 = buffer[startIndex++];
				if (c19 != 'T' && c19 != 't')
				{
					break;
				}
				char c20 = buffer[startIndex++];
				if (c20 != 'A' && c20 != 'a')
				{
					break;
				}
				char c21 = buffer[startIndex++];
				if (c21 != 'B' && c21 != 'b')
				{
					break;
				}
				char c22 = buffer[startIndex++];
				if (c22 != 'A' && c22 != 'a')
				{
					break;
				}
				char c23 = buffer[startIndex++];
				if (c23 == 'S' || c23 == 's')
				{
					char c25 = buffer[startIndex++];
					if (c25 == 'E' || c25 == 'e')
					{
						result = StringToken.AlterAuthorizationDatabase;
					}
				}
				break;
			}
			case 'R':
			case 'r':
			{
				char c5 = buffer[startIndex++];
				if (c5 != 'E' && c5 != 'e')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'M' && c6 != 'm')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'O' && c7 != 'o')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'T' && c8 != 't')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if ((c9 != 'E' && c9 != 'e') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'S' && c10 != 's')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'E' && c11 != 'e')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'R' && c12 != 'r')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 != 'V' && c13 != 'v')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if (c14 != 'I' && c14 != 'i')
				{
					break;
				}
				char c15 = buffer[startIndex++];
				if (c15 != 'C' && c15 != 'c')
				{
					break;
				}
				char c16 = buffer[startIndex++];
				if ((c16 != 'E' && c16 != 'e') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c17 = buffer[startIndex++];
				if (c17 != 'B' && c17 != 'b')
				{
					break;
				}
				char c18 = buffer[startIndex++];
				if (c18 != 'I' && c18 != 'i')
				{
					break;
				}
				char c19 = buffer[startIndex++];
				if (c19 != 'N' && c19 != 'n')
				{
					break;
				}
				char c20 = buffer[startIndex++];
				if (c20 != 'D' && c20 != 'd')
				{
					break;
				}
				char c21 = buffer[startIndex++];
				if (c21 != 'I' && c21 != 'i')
				{
					break;
				}
				char c22 = buffer[startIndex++];
				if (c22 == 'N' || c22 == 'n')
				{
					char c23 = buffer[startIndex++];
					if (c23 == 'G' || c23 == 'g')
					{
						result = StringToken.AlterRemoteServiceBinding;
					}
				}
				break;
			}
			}
			break;
		}
		case 'C':
		case 'c':
		{
			char c = buffer[startIndex++];
			if (c != 'R' && c != 'r')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'E' && c2 != 'e')
			{
				break;
			}
			char c24 = buffer[startIndex++];
			if (c24 != 'A' && c24 != 'a')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'T' && c3 != 't')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if ((c4 != 'E' && c4 != 'e') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'X' && c5 != 'x')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'M' && c6 != 'm')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if ((c7 != 'L' && c7 != 'l') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'S' && c8 != 's')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'C' && c9 != 'c')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'H' && c10 != 'h')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'E' && c11 != 'e')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 != 'M' && c12 != 'm')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if ((c13 != 'A' && c13 != 'a') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 != 'C' && c14 != 'c')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if (c15 != 'O' && c15 != 'o')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if (c16 != 'L' && c16 != 'l')
			{
				break;
			}
			char c17 = buffer[startIndex++];
			if (c17 != 'L' && c17 != 'l')
			{
				break;
			}
			char c18 = buffer[startIndex++];
			if (c18 != 'E' && c18 != 'e')
			{
				break;
			}
			char c19 = buffer[startIndex++];
			if (c19 != 'C' && c19 != 'c')
			{
				break;
			}
			char c20 = buffer[startIndex++];
			if (c20 != 'T' && c20 != 't')
			{
				break;
			}
			char c21 = buffer[startIndex++];
			if (c21 != 'I' && c21 != 'i')
			{
				break;
			}
			char c22 = buffer[startIndex++];
			if (c22 == 'O' || c22 == 'o')
			{
				char c23 = buffer[startIndex++];
				if (c23 == 'N' || c23 == 'n')
				{
					result = StringToken.CreateXmlSchemaCollection;
				}
			}
			break;
		}
		case 'D':
		case 'd':
		{
			char c = buffer[startIndex++];
			if (c != 'D' && c != 'd')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if ((c2 != 'L' && c2 != 'l') || buffer[startIndex++] != '_')
			{
				break;
			}
			switch (buffer[startIndex++])
			{
			case 'D':
			case 'd':
			{
				char c3 = buffer[startIndex++];
				if (c3 != 'A' && c3 != 'a')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'T' && c4 != 't')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'A' && c5 != 'a')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'B' && c6 != 'b')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'A' && c7 != 'a')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'S' && c8 != 's')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if ((c9 != 'E' && c9 != 'e') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'S' && c10 != 's')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'E' && c11 != 'e')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'C' && c12 != 'c')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 != 'U' && c13 != 'u')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if (c14 != 'R' && c14 != 'r')
				{
					break;
				}
				char c15 = buffer[startIndex++];
				if (c15 != 'I' && c15 != 'i')
				{
					break;
				}
				char c16 = buffer[startIndex++];
				if (c16 != 'T' && c16 != 't')
				{
					break;
				}
				char c17 = buffer[startIndex++];
				if ((c17 != 'Y' && c17 != 'y') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c18 = buffer[startIndex++];
				if (c18 != 'E' && c18 != 'e')
				{
					break;
				}
				char c19 = buffer[startIndex++];
				if (c19 != 'V' && c19 != 'v')
				{
					break;
				}
				char c20 = buffer[startIndex++];
				if (c20 != 'E' && c20 != 'e')
				{
					break;
				}
				char c21 = buffer[startIndex++];
				if (c21 != 'N' && c21 != 'n')
				{
					break;
				}
				char c22 = buffer[startIndex++];
				if (c22 == 'T' || c22 == 't')
				{
					char c23 = buffer[startIndex++];
					if (c23 == 'S' || c23 == 's')
					{
						result = StringToken.DdlDatabaseSecurityEvents;
					}
				}
				break;
			}
			case 'E':
			case 'e':
			{
				char c3 = buffer[startIndex++];
				if (c3 != 'X' && c3 != 'x')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'T' && c4 != 't')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'E' && c5 != 'e')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'N' && c6 != 'n')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'D' && c7 != 'd')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'E' && c8 != 'e')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if ((c9 != 'D' && c9 != 'd') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'P' && c10 != 'p')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'R' && c11 != 'r')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'O' && c12 != 'o')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 != 'P' && c13 != 'p')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if (c14 != 'E' && c14 != 'e')
				{
					break;
				}
				char c15 = buffer[startIndex++];
				if (c15 != 'R' && c15 != 'r')
				{
					break;
				}
				char c16 = buffer[startIndex++];
				if (c16 != 'T' && c16 != 't')
				{
					break;
				}
				char c17 = buffer[startIndex++];
				if ((c17 != 'Y' && c17 != 'y') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c18 = buffer[startIndex++];
				if (c18 != 'E' && c18 != 'e')
				{
					break;
				}
				char c19 = buffer[startIndex++];
				if (c19 != 'V' && c19 != 'v')
				{
					break;
				}
				char c20 = buffer[startIndex++];
				if (c20 != 'E' && c20 != 'e')
				{
					break;
				}
				char c21 = buffer[startIndex++];
				if (c21 != 'N' && c21 != 'n')
				{
					break;
				}
				char c22 = buffer[startIndex++];
				if (c22 == 'T' || c22 == 't')
				{
					char c23 = buffer[startIndex++];
					if (c23 == 'S' || c23 == 's')
					{
						result = StringToken.DdlExtendedPropertyEvents;
					}
				}
				break;
			}
			}
			break;
		}
		}
		return result;
	}

	private static StringToken Find_Len29(string buffer, int startIndex)
	{
		StringToken result = StringToken.None;
		switch (buffer[startIndex++])
		{
		case 'A':
		case 'a':
		{
			char c = buffer[startIndex++];
			if (c != 'C' && c != 'c')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'C' && c2 != 'c')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'E' && c3 != 'e')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'L' && c4 != 'l')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'E' && c5 != 'e')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'R' && c6 != 'r')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'A' && c7 != 'a')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'T' && c8 != 't')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'E' && c9 != 'e')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if ((c10 != 'D' && c10 != 'd') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'D' && c11 != 'd')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 != 'A' && c12 != 'a')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if (c13 != 'T' && c13 != 't')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 != 'A' && c14 != 'a')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if (c15 != 'B' && c15 != 'b')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if (c16 != 'A' && c16 != 'a')
			{
				break;
			}
			char c17 = buffer[startIndex++];
			if (c17 != 'S' && c17 != 's')
			{
				break;
			}
			char c18 = buffer[startIndex++];
			if ((c18 != 'E' && c18 != 'e') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c19 = buffer[startIndex++];
			if (c19 != 'R' && c19 != 'r')
			{
				break;
			}
			char c20 = buffer[startIndex++];
			if (c20 != 'E' && c20 != 'e')
			{
				break;
			}
			char c21 = buffer[startIndex++];
			if (c21 != 'C' && c21 != 'c')
			{
				break;
			}
			char c22 = buffer[startIndex++];
			if (c22 != 'O' && c22 != 'o')
			{
				break;
			}
			char c23 = buffer[startIndex++];
			if (c23 != 'V' && c23 != 'v')
			{
				break;
			}
			char c24 = buffer[startIndex++];
			if (c24 != 'E' && c24 != 'e')
			{
				break;
			}
			char c25 = buffer[startIndex++];
			if (c25 == 'R' || c25 == 'r')
			{
				char c26 = buffer[startIndex++];
				if (c26 == 'Y' || c26 == 'y')
				{
					result = StringToken.AcceleratedDatabaseRecovery;
				}
			}
			break;
		}
		case 'C':
		case 'c':
		{
			char c = buffer[startIndex++];
			if (c != 'R' && c != 'r')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'E' && c2 != 'e')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'A' && c3 != 'a')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'T' && c4 != 't')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if ((c5 != 'E' && c5 != 'e') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'R' && c6 != 'r')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'E' && c7 != 'e')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'M' && c8 != 'm')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'O' && c9 != 'o')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'T' && c10 != 't')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if ((c11 != 'E' && c11 != 'e') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 != 'S' && c12 != 's')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if (c13 != 'E' && c13 != 'e')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 != 'R' && c14 != 'r')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if (c15 != 'V' && c15 != 'v')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if (c16 != 'I' && c16 != 'i')
			{
				break;
			}
			char c17 = buffer[startIndex++];
			if (c17 != 'C' && c17 != 'c')
			{
				break;
			}
			char c18 = buffer[startIndex++];
			if ((c18 != 'E' && c18 != 'e') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c19 = buffer[startIndex++];
			if (c19 != 'B' && c19 != 'b')
			{
				break;
			}
			char c20 = buffer[startIndex++];
			if (c20 != 'I' && c20 != 'i')
			{
				break;
			}
			char c21 = buffer[startIndex++];
			if (c21 != 'N' && c21 != 'n')
			{
				break;
			}
			char c22 = buffer[startIndex++];
			if (c22 != 'D' && c22 != 'd')
			{
				break;
			}
			char c23 = buffer[startIndex++];
			if (c23 != 'I' && c23 != 'i')
			{
				break;
			}
			char c24 = buffer[startIndex++];
			if (c24 == 'N' || c24 == 'n')
			{
				char c25 = buffer[startIndex++];
				if (c25 == 'G' || c25 == 'g')
				{
					result = StringToken.CreateRemoteServiceBinding;
				}
			}
			break;
		}
		case 'D':
		case 'd':
		{
			char c = buffer[startIndex++];
			if (c != 'D' && c != 'd')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if ((c2 != 'L' && c2 != 'l') || buffer[startIndex++] != '_')
			{
				break;
			}
			switch (buffer[startIndex++])
			{
			case 'E':
			case 'e':
				switch (buffer[startIndex++])
				{
				case 'V':
				case 'v':
				{
					char c5 = buffer[startIndex++];
					if (c5 != 'E' && c5 != 'e')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'N' && c6 != 'n')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if ((c7 != 'T' && c7 != 't') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'N' && c8 != 'n')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'O' && c9 != 'o')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'T' && c10 != 't')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 != 'I' && c11 != 'i')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if (c12 != 'F' && c12 != 'f')
					{
						break;
					}
					char c13 = buffer[startIndex++];
					if (c13 != 'I' && c13 != 'i')
					{
						break;
					}
					char c14 = buffer[startIndex++];
					if (c14 != 'C' && c14 != 'c')
					{
						break;
					}
					char c15 = buffer[startIndex++];
					if (c15 != 'A' && c15 != 'a')
					{
						break;
					}
					char c16 = buffer[startIndex++];
					if (c16 != 'T' && c16 != 't')
					{
						break;
					}
					char c17 = buffer[startIndex++];
					if (c17 != 'I' && c17 != 'i')
					{
						break;
					}
					char c18 = buffer[startIndex++];
					if (c18 != 'O' && c18 != 'o')
					{
						break;
					}
					char c19 = buffer[startIndex++];
					if ((c19 != 'N' && c19 != 'n') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c20 = buffer[startIndex++];
					if (c20 != 'E' && c20 != 'e')
					{
						break;
					}
					char c21 = buffer[startIndex++];
					if (c21 != 'V' && c21 != 'v')
					{
						break;
					}
					char c22 = buffer[startIndex++];
					if (c22 != 'E' && c22 != 'e')
					{
						break;
					}
					char c23 = buffer[startIndex++];
					if (c23 != 'N' && c23 != 'n')
					{
						break;
					}
					char c24 = buffer[startIndex++];
					if (c24 == 'T' || c24 == 't')
					{
						char c25 = buffer[startIndex++];
						if (c25 == 'S' || c25 == 's')
						{
							result = StringToken.DdlEventNotificationEvents;
						}
					}
					break;
				}
				case 'X':
				case 'x':
				{
					char c5 = buffer[startIndex++];
					if (c5 != 'T' && c5 != 't')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'E' && c6 != 'e')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'N' && c7 != 'n')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'D' && c8 != 'd')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'E' && c9 != 'e')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if ((c10 != 'D' && c10 != 'd') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 != 'P' && c11 != 'p')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if (c12 != 'R' && c12 != 'r')
					{
						break;
					}
					char c13 = buffer[startIndex++];
					if (c13 != 'O' && c13 != 'o')
					{
						break;
					}
					char c14 = buffer[startIndex++];
					if (c14 != 'C' && c14 != 'c')
					{
						break;
					}
					char c15 = buffer[startIndex++];
					if (c15 != 'E' && c15 != 'e')
					{
						break;
					}
					char c16 = buffer[startIndex++];
					if (c16 != 'D' && c16 != 'd')
					{
						break;
					}
					char c17 = buffer[startIndex++];
					if (c17 != 'U' && c17 != 'u')
					{
						break;
					}
					char c18 = buffer[startIndex++];
					if (c18 != 'R' && c18 != 'r')
					{
						break;
					}
					char c19 = buffer[startIndex++];
					if ((c19 != 'E' && c19 != 'e') || buffer[startIndex++] != '_')
					{
						break;
					}
					char c20 = buffer[startIndex++];
					if (c20 != 'E' && c20 != 'e')
					{
						break;
					}
					char c21 = buffer[startIndex++];
					if (c21 != 'V' && c21 != 'v')
					{
						break;
					}
					char c22 = buffer[startIndex++];
					if (c22 != 'E' && c22 != 'e')
					{
						break;
					}
					char c23 = buffer[startIndex++];
					if (c23 != 'N' && c23 != 'n')
					{
						break;
					}
					char c24 = buffer[startIndex++];
					if (c24 == 'T' || c24 == 't')
					{
						char c25 = buffer[startIndex++];
						if (c25 == 'S' || c25 == 's')
						{
							result = StringToken.DdlExtendedProcedureEvents;
						}
					}
					break;
				}
				}
				break;
			case 'P':
			case 'p':
			{
				char c4 = buffer[startIndex++];
				if (c4 != 'A' && c4 != 'a')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'R' && c5 != 'r')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'T' && c6 != 't')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'I' && c7 != 'i')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'T' && c8 != 't')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'I' && c9 != 'i')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'O' && c10 != 'o')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if ((c11 != 'N' && c11 != 'n') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'F' && c12 != 'f')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 != 'U' && c13 != 'u')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if (c14 != 'N' && c14 != 'n')
				{
					break;
				}
				char c15 = buffer[startIndex++];
				if (c15 != 'C' && c15 != 'c')
				{
					break;
				}
				char c16 = buffer[startIndex++];
				if (c16 != 'T' && c16 != 't')
				{
					break;
				}
				char c17 = buffer[startIndex++];
				if (c17 != 'I' && c17 != 'i')
				{
					break;
				}
				char c18 = buffer[startIndex++];
				if (c18 != 'O' && c18 != 'o')
				{
					break;
				}
				char c19 = buffer[startIndex++];
				if ((c19 != 'N' && c19 != 'n') || buffer[startIndex++] != '_')
				{
					break;
				}
				char c20 = buffer[startIndex++];
				if (c20 != 'E' && c20 != 'e')
				{
					break;
				}
				char c21 = buffer[startIndex++];
				if (c21 != 'V' && c21 != 'v')
				{
					break;
				}
				char c22 = buffer[startIndex++];
				if (c22 != 'E' && c22 != 'e')
				{
					break;
				}
				char c23 = buffer[startIndex++];
				if (c23 != 'N' && c23 != 'n')
				{
					break;
				}
				char c24 = buffer[startIndex++];
				if (c24 == 'T' || c24 == 't')
				{
					char c25 = buffer[startIndex++];
					if (c25 == 'S' || c25 == 's')
					{
						result = StringToken.DdlPartitionFunctionEvents;
					}
				}
				break;
			}
			}
			break;
		}
		case 'L':
		case 'l':
		{
			char c = buffer[startIndex++];
			if (c != 'E' && c != 'e')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'G' && c2 != 'g')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'A' && c3 != 'a')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'C' && c4 != 'c')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if ((c5 != 'Y' && c5 != 'y') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'C' && c6 != 'c')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'A' && c7 != 'a')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'R' && c8 != 'r')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'D' && c9 != 'd')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'I' && c10 != 'i')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'N' && c11 != 'n')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 != 'A' && c12 != 'a')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if (c13 != 'L' && c13 != 'l')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 != 'I' && c14 != 'i')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if (c15 != 'T' && c15 != 't')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if ((c16 != 'Y' && c16 != 'y') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c17 = buffer[startIndex++];
			if (c17 != 'E' && c17 != 'e')
			{
				break;
			}
			char c18 = buffer[startIndex++];
			if (c18 != 'S' && c18 != 's')
			{
				break;
			}
			char c19 = buffer[startIndex++];
			if (c19 != 'T' && c19 != 't')
			{
				break;
			}
			char c20 = buffer[startIndex++];
			if (c20 != 'I' && c20 != 'i')
			{
				break;
			}
			char c21 = buffer[startIndex++];
			if (c21 != 'M' && c21 != 'm')
			{
				break;
			}
			char c22 = buffer[startIndex++];
			if (c22 != 'A' && c22 != 'a')
			{
				break;
			}
			char c23 = buffer[startIndex++];
			if (c23 != 'T' && c23 != 't')
			{
				break;
			}
			char c24 = buffer[startIndex++];
			if (c24 != 'I' && c24 != 'i')
			{
				break;
			}
			char c25 = buffer[startIndex++];
			if (c25 == 'O' || c25 == 'o')
			{
				char c26 = buffer[startIndex++];
				if (c26 == 'N' || c26 == 'n')
				{
					result = StringToken.LegacyCardinalityEstimation;
				}
			}
			break;
		}
		}
		return result;
	}

	private static StringToken Find_Len30(string buffer, int startIndex)
	{
		StringToken result = StringToken.None;
		switch (buffer[startIndex++])
		{
		case 'D':
		case 'd':
		{
			char c = buffer[startIndex++];
			if (c != 'D' && c != 'd')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if ((c2 != 'L' && c2 != 'l') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'L' && c3 != 'l')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'I' && c4 != 'i')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'N' && c5 != 'n')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'K' && c6 != 'k')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'E' && c7 != 'e')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if ((c8 != 'D' && c8 != 'd') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'S' && c9 != 's')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'E' && c10 != 'e')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'R' && c11 != 'r')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 != 'V' && c12 != 'v')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if (c13 != 'E' && c13 != 'e')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if ((c14 != 'R' && c14 != 'r') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if (c15 != 'L' && c15 != 'l')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if (c16 != 'O' && c16 != 'o')
			{
				break;
			}
			char c17 = buffer[startIndex++];
			if (c17 != 'G' && c17 != 'g')
			{
				break;
			}
			char c18 = buffer[startIndex++];
			if (c18 != 'I' && c18 != 'i')
			{
				break;
			}
			char c19 = buffer[startIndex++];
			if ((c19 != 'N' && c19 != 'n') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c20 = buffer[startIndex++];
			if (c20 != 'E' && c20 != 'e')
			{
				break;
			}
			char c21 = buffer[startIndex++];
			if (c21 != 'V' && c21 != 'v')
			{
				break;
			}
			char c22 = buffer[startIndex++];
			if (c22 != 'E' && c22 != 'e')
			{
				break;
			}
			char c23 = buffer[startIndex++];
			if (c23 != 'N' && c23 != 'n')
			{
				break;
			}
			char c24 = buffer[startIndex++];
			if (c24 == 'T' || c24 == 't')
			{
				char c25 = buffer[startIndex++];
				if (c25 == 'S' || c25 == 's')
				{
					result = StringToken.DdlLinkedServerLoginEvents;
				}
			}
			break;
		}
		case 'F':
		case 'f':
		{
			char c = buffer[startIndex++];
			if (c != 'O' && c != 'o')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'R' && c2 != 'r')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'C' && c3 != 'c')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if ((c4 != 'E' && c4 != 'e') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'F' && c5 != 'f')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'A' && c6 != 'a')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'I' && c7 != 'i')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'L' && c8 != 'l')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'O' && c9 != 'o')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'V' && c10 != 'v')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'E' && c11 != 'e')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if ((c12 != 'R' && c12 != 'r') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if (c13 != 'A' && c13 != 'a')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 != 'L' && c14 != 'l')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if (c15 != 'L' && c15 != 'l')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if (c16 != 'O' && c16 != 'o')
			{
				break;
			}
			char c17 = buffer[startIndex++];
			if ((c17 != 'W' && c17 != 'w') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c18 = buffer[startIndex++];
			if (c18 != 'D' && c18 != 'd')
			{
				break;
			}
			char c19 = buffer[startIndex++];
			if (c19 != 'A' && c19 != 'a')
			{
				break;
			}
			char c20 = buffer[startIndex++];
			if (c20 != 'T' && c20 != 't')
			{
				break;
			}
			char c21 = buffer[startIndex++];
			if ((c21 != 'A' && c21 != 'a') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c22 = buffer[startIndex++];
			if (c22 != 'L' && c22 != 'l')
			{
				break;
			}
			char c23 = buffer[startIndex++];
			if (c23 != 'O' && c23 != 'o')
			{
				break;
			}
			char c24 = buffer[startIndex++];
			if (c24 == 'S' || c24 == 's')
			{
				char c25 = buffer[startIndex++];
				if (c25 == 'S' || c25 == 's')
				{
					result = StringToken.ForceFailoverAllowDataLoss;
				}
			}
			break;
		}
		case 'S':
		case 's':
		{
			char c = buffer[startIndex++];
			if (c != 'E' && c != 'e')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'M' && c2 != 'm')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'A' && c3 != 'a')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'N' && c4 != 'n')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'T' && c5 != 't')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'I' && c6 != 'i')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'C' && c7 != 'c')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'S' && c8 != 's')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'I' && c9 != 'i')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'M' && c10 != 'm')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'I' && c11 != 'i')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 != 'L' && c12 != 'l')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if (c13 != 'A' && c13 != 'a')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 != 'R' && c14 != 'r')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if (c15 != 'I' && c15 != 'i')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if (c16 != 'T' && c16 != 't')
			{
				break;
			}
			char c17 = buffer[startIndex++];
			if (c17 != 'Y' && c17 != 'y')
			{
				break;
			}
			char c18 = buffer[startIndex++];
			if (c18 != 'D' && c18 != 'd')
			{
				break;
			}
			char c19 = buffer[startIndex++];
			if (c19 != 'E' && c19 != 'e')
			{
				break;
			}
			char c20 = buffer[startIndex++];
			if (c20 != 'T' && c20 != 't')
			{
				break;
			}
			char c21 = buffer[startIndex++];
			if (c21 != 'A' && c21 != 'a')
			{
				break;
			}
			char c22 = buffer[startIndex++];
			if (c22 != 'I' && c22 != 'i')
			{
				break;
			}
			char c23 = buffer[startIndex++];
			if (c23 != 'L' && c23 != 'l')
			{
				break;
			}
			char c24 = buffer[startIndex++];
			if (c24 != 'S' && c24 != 's')
			{
				break;
			}
			char c25 = buffer[startIndex++];
			if (c25 != 'T' && c25 != 't')
			{
				break;
			}
			char c26 = buffer[startIndex++];
			if (c26 != 'A' && c26 != 'a')
			{
				break;
			}
			char c27 = buffer[startIndex++];
			if (c27 != 'B' && c27 != 'b')
			{
				break;
			}
			char c28 = buffer[startIndex++];
			if (c28 == 'L' || c28 == 'l')
			{
				char c29 = buffer[startIndex++];
				if (c29 == 'E' || c29 == 'e')
				{
					result = StringToken.SemanticSimilarityDetailsTable;
				}
			}
			break;
		}
		}
		return result;
	}

	private static StringToken Find_Len31(string buffer, int startIndex)
	{
		StringToken result = StringToken.None;
		switch (buffer[startIndex++])
		{
		case 'D':
		case 'd':
		{
			char c = buffer[startIndex++];
			if (c != 'D' && c != 'd')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if ((c2 != 'L' && c2 != 'l') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'A' && c3 != 'a')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'U' && c4 != 'u')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'T' && c5 != 't')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'H' && c6 != 'h')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'O' && c7 != 'o')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'R' && c8 != 'r')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'I' && c9 != 'i')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'Z' && c10 != 'z')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'A' && c11 != 'a')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 != 'T' && c12 != 't')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if (c13 != 'I' && c13 != 'i')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 != 'O' && c14 != 'o')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if ((c15 != 'N' && c15 != 'n') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if (c16 != 'S' && c16 != 's')
			{
				break;
			}
			char c17 = buffer[startIndex++];
			if (c17 != 'E' && c17 != 'e')
			{
				break;
			}
			char c18 = buffer[startIndex++];
			if (c18 != 'R' && c18 != 'r')
			{
				break;
			}
			char c19 = buffer[startIndex++];
			if (c19 != 'V' && c19 != 'v')
			{
				break;
			}
			char c20 = buffer[startIndex++];
			if (c20 != 'E' && c20 != 'e')
			{
				break;
			}
			char c21 = buffer[startIndex++];
			if ((c21 != 'R' && c21 != 'r') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c22 = buffer[startIndex++];
			if (c22 != 'E' && c22 != 'e')
			{
				break;
			}
			char c23 = buffer[startIndex++];
			if (c23 != 'V' && c23 != 'v')
			{
				break;
			}
			char c24 = buffer[startIndex++];
			if (c24 != 'E' && c24 != 'e')
			{
				break;
			}
			char c25 = buffer[startIndex++];
			if (c25 != 'N' && c25 != 'n')
			{
				break;
			}
			char c26 = buffer[startIndex++];
			if (c26 == 'T' || c26 == 't')
			{
				char c27 = buffer[startIndex++];
				if (c27 == 'S' || c27 == 's')
				{
					result = StringToken.DdlAuthorizationServerEvents;
				}
			}
			break;
		}
		case 'O':
		case 'o':
		{
			char c = buffer[startIndex++];
			if (c != 'P' && c != 'p')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'E' && c2 != 'e')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'R' && c3 != 'r')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'A' && c4 != 'a')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'T' && c5 != 't')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'I' && c6 != 'i')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'O' && c7 != 'o')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if ((c8 != 'N' && c8 != 'n') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'T' && c9 != 't')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'Y' && c10 != 'y')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'P' && c11 != 'p')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if ((c12 != 'E' && c12 != 'e') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if (c13 != 'D' && c13 != 'd')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 != 'E' && c14 != 'e')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if (c15 != 'S' && c15 != 's')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if ((c16 != 'C' && c16 != 'c') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c17 = buffer[startIndex++];
			if (c17 != 'C' && c17 != 'c')
			{
				break;
			}
			char c18 = buffer[startIndex++];
			if (c18 != 'O' && c18 != 'o')
			{
				break;
			}
			char c19 = buffer[startIndex++];
			if (c19 != 'L' && c19 != 'l')
			{
				break;
			}
			char c20 = buffer[startIndex++];
			if (c20 != 'U' && c20 != 'u')
			{
				break;
			}
			char c21 = buffer[startIndex++];
			if (c21 != 'M' && c21 != 'm')
			{
				break;
			}
			char c22 = buffer[startIndex++];
			if ((c22 != 'N' && c22 != 'n') || buffer[startIndex++] != '_')
			{
				break;
			}
			char c23 = buffer[startIndex++];
			if (c23 != 'N' && c23 != 'n')
			{
				break;
			}
			char c24 = buffer[startIndex++];
			if (c24 != 'A' && c24 != 'a')
			{
				break;
			}
			char c25 = buffer[startIndex++];
			if (c25 == 'M' || c25 == 'm')
			{
				char c26 = buffer[startIndex++];
				if (c26 == 'E' || c26 == 'e')
				{
					result = StringToken.OperationTypeDescColumnName;
				}
			}
			break;
		}
		}
		return result;
	}

	private static StringToken Find_Len32(string buffer, int startIndex)
	{
		StringToken result = StringToken.None;
		char c = buffer[startIndex++];
		if (c == 'D' || c == 'd')
		{
			char c2 = buffer[startIndex++];
			if (c2 == 'D' || c2 == 'd')
			{
				char c3 = buffer[startIndex++];
				if ((c3 == 'L' || c3 == 'l') && buffer[startIndex++] == '_')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'X' || c4 == 'x')
					{
						char c5 = buffer[startIndex++];
						if (c5 == 'M' || c5 == 'm')
						{
							char c6 = buffer[startIndex++];
							if ((c6 == 'L' || c6 == 'l') && buffer[startIndex++] == '_')
							{
								char c7 = buffer[startIndex++];
								if (c7 == 'S' || c7 == 's')
								{
									char c8 = buffer[startIndex++];
									if (c8 == 'C' || c8 == 'c')
									{
										char c9 = buffer[startIndex++];
										if (c9 == 'H' || c9 == 'h')
										{
											char c10 = buffer[startIndex++];
											if (c10 == 'E' || c10 == 'e')
											{
												char c11 = buffer[startIndex++];
												if (c11 == 'M' || c11 == 'm')
												{
													char c12 = buffer[startIndex++];
													if ((c12 == 'A' || c12 == 'a') && buffer[startIndex++] == '_')
													{
														char c13 = buffer[startIndex++];
														if (c13 == 'C' || c13 == 'c')
														{
															char c14 = buffer[startIndex++];
															if (c14 == 'O' || c14 == 'o')
															{
																char c15 = buffer[startIndex++];
																if (c15 == 'L' || c15 == 'l')
																{
																	char c16 = buffer[startIndex++];
																	if (c16 == 'L' || c16 == 'l')
																	{
																		char c17 = buffer[startIndex++];
																		if (c17 == 'E' || c17 == 'e')
																		{
																			char c18 = buffer[startIndex++];
																			if (c18 == 'C' || c18 == 'c')
																			{
																				char c19 = buffer[startIndex++];
																				if (c19 == 'T' || c19 == 't')
																				{
																					char c20 = buffer[startIndex++];
																					if (c20 == 'I' || c20 == 'i')
																					{
																						char c21 = buffer[startIndex++];
																						if (c21 == 'O' || c21 == 'o')
																						{
																							char c22 = buffer[startIndex++];
																							if ((c22 == 'N' || c22 == 'n') && buffer[startIndex++] == '_')
																							{
																								char c23 = buffer[startIndex++];
																								if (c23 == 'E' || c23 == 'e')
																								{
																									char c24 = buffer[startIndex++];
																									if (c24 == 'V' || c24 == 'v')
																									{
																										char c25 = buffer[startIndex++];
																										if (c25 == 'E' || c25 == 'e')
																										{
																											char c26 = buffer[startIndex++];
																											if (c26 == 'N' || c26 == 'n')
																											{
																												char c27 = buffer[startIndex++];
																												if (c27 == 'T' || c27 == 't')
																												{
																													char c28 = buffer[startIndex++];
																													if (c28 == 'S' || c28 == 's')
																													{
																														result = StringToken.DdlXmlSchemaCollectionEvents;
																													}
																												}
																											}
																										}
																									}
																								}
																							}
																						}
																					}
																				}
																			}
																		}
																	}
																}
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
		return result;
	}

	private static StringToken Find_Len33(string buffer, int startIndex)
	{
		StringToken result = StringToken.None;
		char c = buffer[startIndex++];
		if (c == 'D' || c == 'd')
		{
			char c2 = buffer[startIndex++];
			if (c2 == 'D' || c2 == 'd')
			{
				char c3 = buffer[startIndex++];
				if ((c3 == 'L' || c3 == 'l') && buffer[startIndex++] == '_')
				{
					switch (buffer[startIndex++])
					{
					case 'A':
					case 'a':
					{
						char c4 = buffer[startIndex++];
						if (c4 != 'U' && c4 != 'u')
						{
							break;
						}
						char c5 = buffer[startIndex++];
						if (c5 != 'T' && c5 != 't')
						{
							break;
						}
						char c6 = buffer[startIndex++];
						if (c6 != 'H' && c6 != 'h')
						{
							break;
						}
						char c7 = buffer[startIndex++];
						if (c7 != 'O' && c7 != 'o')
						{
							break;
						}
						char c8 = buffer[startIndex++];
						if (c8 != 'R' && c8 != 'r')
						{
							break;
						}
						char c9 = buffer[startIndex++];
						if (c9 != 'I' && c9 != 'i')
						{
							break;
						}
						char c10 = buffer[startIndex++];
						if (c10 != 'Z' && c10 != 'z')
						{
							break;
						}
						char c11 = buffer[startIndex++];
						if (c11 != 'A' && c11 != 'a')
						{
							break;
						}
						char c12 = buffer[startIndex++];
						if (c12 != 'T' && c12 != 't')
						{
							break;
						}
						char c13 = buffer[startIndex++];
						if (c13 != 'I' && c13 != 'i')
						{
							break;
						}
						char c14 = buffer[startIndex++];
						if (c14 != 'O' && c14 != 'o')
						{
							break;
						}
						char c15 = buffer[startIndex++];
						if ((c15 != 'N' && c15 != 'n') || buffer[startIndex++] != '_')
						{
							break;
						}
						char c16 = buffer[startIndex++];
						if (c16 != 'D' && c16 != 'd')
						{
							break;
						}
						char c17 = buffer[startIndex++];
						if (c17 != 'A' && c17 != 'a')
						{
							break;
						}
						char c18 = buffer[startIndex++];
						if (c18 != 'T' && c18 != 't')
						{
							break;
						}
						char c19 = buffer[startIndex++];
						if (c19 != 'A' && c19 != 'a')
						{
							break;
						}
						char c20 = buffer[startIndex++];
						if (c20 != 'B' && c20 != 'b')
						{
							break;
						}
						char c21 = buffer[startIndex++];
						if (c21 != 'A' && c21 != 'a')
						{
							break;
						}
						char c22 = buffer[startIndex++];
						if (c22 != 'S' && c22 != 's')
						{
							break;
						}
						char c23 = buffer[startIndex++];
						if ((c23 != 'E' && c23 != 'e') || buffer[startIndex++] != '_')
						{
							break;
						}
						char c24 = buffer[startIndex++];
						if (c24 != 'E' && c24 != 'e')
						{
							break;
						}
						char c25 = buffer[startIndex++];
						if (c25 != 'V' && c25 != 'v')
						{
							break;
						}
						char c26 = buffer[startIndex++];
						if (c26 != 'E' && c26 != 'e')
						{
							break;
						}
						char c27 = buffer[startIndex++];
						if (c27 != 'N' && c27 != 'n')
						{
							break;
						}
						char c28 = buffer[startIndex++];
						if (c28 == 'T' || c28 == 't')
						{
							char c29 = buffer[startIndex++];
							if (c29 == 'S' || c29 == 's')
							{
								result = StringToken.DdlAuthorizationDatabaseEvents;
							}
						}
						break;
					}
					case 'R':
					case 'r':
					{
						char c4 = buffer[startIndex++];
						if (c4 != 'E' && c4 != 'e')
						{
							break;
						}
						char c5 = buffer[startIndex++];
						if (c5 != 'M' && c5 != 'm')
						{
							break;
						}
						char c6 = buffer[startIndex++];
						if (c6 != 'O' && c6 != 'o')
						{
							break;
						}
						char c7 = buffer[startIndex++];
						if (c7 != 'T' && c7 != 't')
						{
							break;
						}
						char c8 = buffer[startIndex++];
						if ((c8 != 'E' && c8 != 'e') || buffer[startIndex++] != '_')
						{
							break;
						}
						char c9 = buffer[startIndex++];
						if (c9 != 'S' && c9 != 's')
						{
							break;
						}
						char c10 = buffer[startIndex++];
						if (c10 != 'E' && c10 != 'e')
						{
							break;
						}
						char c11 = buffer[startIndex++];
						if (c11 != 'R' && c11 != 'r')
						{
							break;
						}
						char c12 = buffer[startIndex++];
						if (c12 != 'V' && c12 != 'v')
						{
							break;
						}
						char c13 = buffer[startIndex++];
						if (c13 != 'I' && c13 != 'i')
						{
							break;
						}
						char c14 = buffer[startIndex++];
						if (c14 != 'C' && c14 != 'c')
						{
							break;
						}
						char c15 = buffer[startIndex++];
						if ((c15 != 'E' && c15 != 'e') || buffer[startIndex++] != '_')
						{
							break;
						}
						char c16 = buffer[startIndex++];
						if (c16 != 'B' && c16 != 'b')
						{
							break;
						}
						char c17 = buffer[startIndex++];
						if (c17 != 'I' && c17 != 'i')
						{
							break;
						}
						char c18 = buffer[startIndex++];
						if (c18 != 'N' && c18 != 'n')
						{
							break;
						}
						char c19 = buffer[startIndex++];
						if (c19 != 'D' && c19 != 'd')
						{
							break;
						}
						char c20 = buffer[startIndex++];
						if (c20 != 'I' && c20 != 'i')
						{
							break;
						}
						char c21 = buffer[startIndex++];
						if (c21 != 'N' && c21 != 'n')
						{
							break;
						}
						char c22 = buffer[startIndex++];
						if ((c22 != 'G' && c22 != 'g') || buffer[startIndex++] != '_')
						{
							break;
						}
						char c23 = buffer[startIndex++];
						if (c23 != 'E' && c23 != 'e')
						{
							break;
						}
						char c24 = buffer[startIndex++];
						if (c24 != 'V' && c24 != 'v')
						{
							break;
						}
						char c25 = buffer[startIndex++];
						if (c25 != 'E' && c25 != 'e')
						{
							break;
						}
						char c26 = buffer[startIndex++];
						if (c26 != 'N' && c26 != 'n')
						{
							break;
						}
						char c27 = buffer[startIndex++];
						if (c27 == 'T' || c27 == 't')
						{
							char c28 = buffer[startIndex++];
							if (c28 == 'S' || c28 == 's')
							{
								result = StringToken.DdlRemoteServiceBindingEvents;
							}
						}
						break;
					}
					}
				}
			}
		}
		return result;
	}

	private static StringToken Find_Len34(string buffer, int startIndex)
	{
		StringToken result = StringToken.None;
		char c = buffer[startIndex++];
		if (c == 'P' || c == 'p')
		{
			char c2 = buffer[startIndex++];
			if (c2 == 'E' || c2 == 'e')
			{
				char c3 = buffer[startIndex++];
				if (c3 == 'R' || c3 == 'r')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'S' || c4 == 's')
					{
						char c5 = buffer[startIndex++];
						if (c5 == 'I' || c5 == 'i')
						{
							char c6 = buffer[startIndex++];
							if (c6 == 'S' || c6 == 's')
							{
								char c7 = buffer[startIndex++];
								if (c7 == 'T' || c7 == 't')
								{
									char c8 = buffer[startIndex++];
									if (c8 == 'E' || c8 == 'e')
									{
										char c9 = buffer[startIndex++];
										if (c9 == 'N' || c9 == 'n')
										{
											char c10 = buffer[startIndex++];
											if ((c10 == 'T' || c10 == 't') && buffer[startIndex++] == '_')
											{
												char c11 = buffer[startIndex++];
												if (c11 == 'V' || c11 == 'v')
												{
													char c12 = buffer[startIndex++];
													if (c12 == 'E' || c12 == 'e')
													{
														char c13 = buffer[startIndex++];
														if (c13 == 'R' || c13 == 'r')
														{
															char c14 = buffer[startIndex++];
															if (c14 == 'S' || c14 == 's')
															{
																char c15 = buffer[startIndex++];
																if (c15 == 'I' || c15 == 'i')
																{
																	char c16 = buffer[startIndex++];
																	if (c16 == 'O' || c16 == 'o')
																	{
																		char c17 = buffer[startIndex++];
																		if ((c17 == 'N' || c17 == 'n') && buffer[startIndex++] == '_')
																		{
																			char c18 = buffer[startIndex++];
																			if (c18 == 'S' || c18 == 's')
																			{
																				char c19 = buffer[startIndex++];
																				if (c19 == 'T' || c19 == 't')
																				{
																					char c20 = buffer[startIndex++];
																					if (c20 == 'O' || c20 == 'o')
																					{
																						char c21 = buffer[startIndex++];
																						if (c21 == 'R' || c21 == 'r')
																						{
																							char c22 = buffer[startIndex++];
																							if ((c22 == 'E' || c22 == 'e') && buffer[startIndex++] == '_')
																							{
																								char c23 = buffer[startIndex++];
																								if (c23 == 'F' || c23 == 'f')
																								{
																									char c24 = buffer[startIndex++];
																									if (c24 == 'I' || c24 == 'i')
																									{
																										char c25 = buffer[startIndex++];
																										if (c25 == 'L' || c25 == 'l')
																										{
																											char c26 = buffer[startIndex++];
																											if (c26 == 'E' || c26 == 'e')
																											{
																												char c27 = buffer[startIndex++];
																												if (c27 == 'G' || c27 == 'g')
																												{
																													char c28 = buffer[startIndex++];
																													if (c28 == 'R' || c28 == 'r')
																													{
																														char c29 = buffer[startIndex++];
																														if (c29 == 'O' || c29 == 'o')
																														{
																															char c30 = buffer[startIndex++];
																															if (c30 == 'U' || c30 == 'u')
																															{
																																char c31 = buffer[startIndex++];
																																if (c31 == 'P' || c31 == 'p')
																																{
																																	result = StringToken.PersistentVersionStoreFilegroup;
																																}
																															}
																														}
																													}
																												}
																											}
																										}
																									}
																								}
																							}
																						}
																					}
																				}
																			}
																		}
																	}
																}
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
		return result;
	}

	private static StringToken Find_Len35(string buffer, int startIndex)
	{
		StringToken result = StringToken.None;
		char c = buffer[startIndex++];
		if (c == 'A' || c == 'a')
		{
			char c2 = buffer[startIndex++];
			if (c2 == 'L' || c2 == 'l')
			{
				char c3 = buffer[startIndex++];
				if (c3 == 'L' || c3 == 'l')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'O' || c4 == 'o')
					{
						char c5 = buffer[startIndex++];
						if ((c5 == 'W' || c5 == 'w') && buffer[startIndex++] == '_')
						{
							char c6 = buffer[startIndex++];
							if (c6 == 'E' || c6 == 'e')
							{
								char c7 = buffer[startIndex++];
								if (c7 == 'N' || c7 == 'n')
								{
									char c8 = buffer[startIndex++];
									if (c8 == 'C' || c8 == 'c')
									{
										char c9 = buffer[startIndex++];
										if (c9 == 'R' || c9 == 'r')
										{
											char c10 = buffer[startIndex++];
											if (c10 == 'Y' || c10 == 'y')
											{
												char c11 = buffer[startIndex++];
												if (c11 == 'P' || c11 == 'p')
												{
													char c12 = buffer[startIndex++];
													if (c12 == 'T' || c12 == 't')
													{
														char c13 = buffer[startIndex++];
														if (c13 == 'E' || c13 == 'e')
														{
															char c14 = buffer[startIndex++];
															if ((c14 == 'D' || c14 == 'd') && buffer[startIndex++] == '_')
															{
																char c15 = buffer[startIndex++];
																if (c15 == 'V' || c15 == 'v')
																{
																	char c16 = buffer[startIndex++];
																	if (c16 == 'A' || c16 == 'a')
																	{
																		char c17 = buffer[startIndex++];
																		if (c17 == 'L' || c17 == 'l')
																		{
																			char c18 = buffer[startIndex++];
																			if (c18 == 'U' || c18 == 'u')
																			{
																				char c19 = buffer[startIndex++];
																				if ((c19 == 'E' || c19 == 'e') && buffer[startIndex++] == '_')
																				{
																					char c20 = buffer[startIndex++];
																					if (c20 == 'M' || c20 == 'm')
																					{
																						char c21 = buffer[startIndex++];
																						if (c21 == 'O' || c21 == 'o')
																						{
																							char c22 = buffer[startIndex++];
																							if (c22 == 'D' || c22 == 'd')
																							{
																								char c23 = buffer[startIndex++];
																								if (c23 == 'I' || c23 == 'i')
																								{
																									char c24 = buffer[startIndex++];
																									if (c24 == 'F' || c24 == 'f')
																									{
																										char c25 = buffer[startIndex++];
																										if (c25 == 'I' || c25 == 'i')
																										{
																											char c26 = buffer[startIndex++];
																											if (c26 == 'C' || c26 == 'c')
																											{
																												char c27 = buffer[startIndex++];
																												if (c27 == 'A' || c27 == 'a')
																												{
																													char c28 = buffer[startIndex++];
																													if (c28 == 'T' || c28 == 't')
																													{
																														char c29 = buffer[startIndex++];
																														if (c29 == 'I' || c29 == 'i')
																														{
																															char c30 = buffer[startIndex++];
																															if (c30 == 'O' || c30 == 'o')
																															{
																																char c31 = buffer[startIndex++];
																																if (c31 == 'N' || c31 == 'n')
																																{
																																	char c32 = buffer[startIndex++];
																																	if (c32 == 'S' || c32 == 's')
																																	{
																																		result = StringToken.AllowEncryptedValueModifications;
																																	}
																																}
																															}
																														}
																													}
																												}
																											}
																										}
																									}
																								}
																							}
																						}
																					}
																				}
																			}
																		}
																	}
																}
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
		return result;
	}

	private static StringToken Find_Len37(string buffer, int startIndex)
	{
		StringToken result = StringToken.None;
		char c = buffer[startIndex++];
		if (c == 'F' || c == 'f')
		{
			char c2 = buffer[startIndex++];
			if (c2 == 'I' || c2 == 'i')
			{
				char c3 = buffer[startIndex++];
				if (c3 == 'L' || c3 == 'l')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'E' || c4 == 'e')
					{
						char c5 = buffer[startIndex++];
						if (c5 == 'T' || c5 == 't')
						{
							char c6 = buffer[startIndex++];
							if (c6 == 'A' || c6 == 'a')
							{
								char c7 = buffer[startIndex++];
								if (c7 == 'B' || c7 == 'b')
								{
									char c8 = buffer[startIndex++];
									if (c8 == 'L' || c8 == 'l')
									{
										char c9 = buffer[startIndex++];
										if ((c9 == 'E' || c9 == 'e') && buffer[startIndex++] == '_')
										{
											char c10 = buffer[startIndex++];
											if (c10 == 'P' || c10 == 'p')
											{
												char c11 = buffer[startIndex++];
												if (c11 == 'R' || c11 == 'r')
												{
													char c12 = buffer[startIndex++];
													if (c12 == 'I' || c12 == 'i')
													{
														char c13 = buffer[startIndex++];
														if (c13 == 'M' || c13 == 'm')
														{
															char c14 = buffer[startIndex++];
															if (c14 == 'A' || c14 == 'a')
															{
																char c15 = buffer[startIndex++];
																if (c15 == 'R' || c15 == 'r')
																{
																	char c16 = buffer[startIndex++];
																	if ((c16 == 'Y' || c16 == 'y') && buffer[startIndex++] == '_')
																	{
																		char c17 = buffer[startIndex++];
																		if (c17 == 'K' || c17 == 'k')
																		{
																			char c18 = buffer[startIndex++];
																			if (c18 == 'E' || c18 == 'e')
																			{
																				char c19 = buffer[startIndex++];
																				if ((c19 == 'Y' || c19 == 'y') && buffer[startIndex++] == '_')
																				{
																					char c20 = buffer[startIndex++];
																					if (c20 == 'C' || c20 == 'c')
																					{
																						char c21 = buffer[startIndex++];
																						if (c21 == 'O' || c21 == 'o')
																						{
																							char c22 = buffer[startIndex++];
																							if (c22 == 'N' || c22 == 'n')
																							{
																								char c23 = buffer[startIndex++];
																								if (c23 == 'S' || c23 == 's')
																								{
																									char c24 = buffer[startIndex++];
																									if (c24 == 'T' || c24 == 't')
																									{
																										char c25 = buffer[startIndex++];
																										if (c25 == 'R' || c25 == 'r')
																										{
																											char c26 = buffer[startIndex++];
																											if (c26 == 'A' || c26 == 'a')
																											{
																												char c27 = buffer[startIndex++];
																												if (c27 == 'I' || c27 == 'i')
																												{
																													char c28 = buffer[startIndex++];
																													if (c28 == 'N' || c28 == 'n')
																													{
																														char c29 = buffer[startIndex++];
																														if ((c29 == 'T' || c29 == 't') && buffer[startIndex++] == '_')
																														{
																															char c30 = buffer[startIndex++];
																															if (c30 == 'N' || c30 == 'n')
																															{
																																char c31 = buffer[startIndex++];
																																if (c31 == 'A' || c31 == 'a')
																																{
																																	char c32 = buffer[startIndex++];
																																	if (c32 == 'M' || c32 == 'm')
																																	{
																																		char c33 = buffer[startIndex++];
																																		if (c33 == 'E' || c33 == 'e')
																																		{
																																			result = StringToken.FileTablePrimaryKeyConstraintName;
																																		}
																																	}
																																}
																															}
																														}
																													}
																												}
																											}
																										}
																									}
																								}
																							}
																						}
																					}
																				}
																			}
																		}
																	}
																}
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
		return result;
	}

	private static StringToken Find_Len41(string buffer, int startIndex)
	{
		StringToken result = StringToken.None;
		char c = buffer[startIndex++];
		if (c == 'F' || c == 'f')
		{
			char c2 = buffer[startIndex++];
			if (c2 == 'I' || c2 == 'i')
			{
				char c3 = buffer[startIndex++];
				if (c3 == 'L' || c3 == 'l')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'E' || c4 == 'e')
					{
						char c5 = buffer[startIndex++];
						if (c5 == 'T' || c5 == 't')
						{
							char c6 = buffer[startIndex++];
							if (c6 == 'A' || c6 == 'a')
							{
								char c7 = buffer[startIndex++];
								if (c7 == 'B' || c7 == 'b')
								{
									char c8 = buffer[startIndex++];
									if (c8 == 'L' || c8 == 'l')
									{
										char c9 = buffer[startIndex++];
										if ((c9 == 'E' || c9 == 'e') && buffer[startIndex++] == '_')
										{
											switch (buffer[startIndex++])
											{
											case 'F':
											case 'f':
											{
												char c10 = buffer[startIndex++];
												if (c10 != 'U' && c10 != 'u')
												{
													break;
												}
												char c11 = buffer[startIndex++];
												if (c11 != 'L' && c11 != 'l')
												{
													break;
												}
												char c12 = buffer[startIndex++];
												if (c12 != 'L' && c12 != 'l')
												{
													break;
												}
												char c13 = buffer[startIndex++];
												if (c13 != 'P' && c13 != 'p')
												{
													break;
												}
												char c14 = buffer[startIndex++];
												if (c14 != 'A' && c14 != 'a')
												{
													break;
												}
												char c15 = buffer[startIndex++];
												if (c15 != 'T' && c15 != 't')
												{
													break;
												}
												char c16 = buffer[startIndex++];
												if ((c16 != 'H' && c16 != 'h') || buffer[startIndex++] != '_')
												{
													break;
												}
												char c17 = buffer[startIndex++];
												if (c17 != 'U' && c17 != 'u')
												{
													break;
												}
												char c18 = buffer[startIndex++];
												if (c18 != 'N' && c18 != 'n')
												{
													break;
												}
												char c19 = buffer[startIndex++];
												if (c19 != 'I' && c19 != 'i')
												{
													break;
												}
												char c20 = buffer[startIndex++];
												if (c20 != 'Q' && c20 != 'q')
												{
													break;
												}
												char c21 = buffer[startIndex++];
												if (c21 != 'U' && c21 != 'u')
												{
													break;
												}
												char c22 = buffer[startIndex++];
												if ((c22 != 'E' && c22 != 'e') || buffer[startIndex++] != '_')
												{
													break;
												}
												char c23 = buffer[startIndex++];
												if (c23 != 'C' && c23 != 'c')
												{
													break;
												}
												char c24 = buffer[startIndex++];
												if (c24 != 'O' && c24 != 'o')
												{
													break;
												}
												char c25 = buffer[startIndex++];
												if (c25 != 'N' && c25 != 'n')
												{
													break;
												}
												char c26 = buffer[startIndex++];
												if (c26 != 'S' && c26 != 's')
												{
													break;
												}
												char c27 = buffer[startIndex++];
												if (c27 != 'T' && c27 != 't')
												{
													break;
												}
												char c28 = buffer[startIndex++];
												if (c28 != 'R' && c28 != 'r')
												{
													break;
												}
												char c29 = buffer[startIndex++];
												if (c29 != 'A' && c29 != 'a')
												{
													break;
												}
												char c30 = buffer[startIndex++];
												if (c30 != 'I' && c30 != 'i')
												{
													break;
												}
												char c31 = buffer[startIndex++];
												if (c31 != 'N' && c31 != 'n')
												{
													break;
												}
												char c32 = buffer[startIndex++];
												if ((c32 != 'T' && c32 != 't') || buffer[startIndex++] != '_')
												{
													break;
												}
												char c33 = buffer[startIndex++];
												if (c33 != 'N' && c33 != 'n')
												{
													break;
												}
												char c34 = buffer[startIndex++];
												if (c34 != 'A' && c34 != 'a')
												{
													break;
												}
												char c35 = buffer[startIndex++];
												if (c35 == 'M' || c35 == 'm')
												{
													char c36 = buffer[startIndex++];
													if (c36 == 'E' || c36 == 'e')
													{
														result = StringToken.FileTableFullpathUniqueConstraintName;
													}
												}
												break;
											}
											case 'S':
											case 's':
											{
												char c10 = buffer[startIndex++];
												if (c10 != 'T' && c10 != 't')
												{
													break;
												}
												char c11 = buffer[startIndex++];
												if (c11 != 'R' && c11 != 'r')
												{
													break;
												}
												char c12 = buffer[startIndex++];
												if (c12 != 'E' && c12 != 'e')
												{
													break;
												}
												char c13 = buffer[startIndex++];
												if (c13 != 'A' && c13 != 'a')
												{
													break;
												}
												char c14 = buffer[startIndex++];
												if (c14 != 'M' && c14 != 'm')
												{
													break;
												}
												char c15 = buffer[startIndex++];
												if (c15 != 'I' && c15 != 'i')
												{
													break;
												}
												char c16 = buffer[startIndex++];
												if ((c16 != 'D' && c16 != 'd') || buffer[startIndex++] != '_')
												{
													break;
												}
												char c17 = buffer[startIndex++];
												if (c17 != 'U' && c17 != 'u')
												{
													break;
												}
												char c18 = buffer[startIndex++];
												if (c18 != 'N' && c18 != 'n')
												{
													break;
												}
												char c19 = buffer[startIndex++];
												if (c19 != 'I' && c19 != 'i')
												{
													break;
												}
												char c20 = buffer[startIndex++];
												if (c20 != 'Q' && c20 != 'q')
												{
													break;
												}
												char c21 = buffer[startIndex++];
												if (c21 != 'U' && c21 != 'u')
												{
													break;
												}
												char c22 = buffer[startIndex++];
												if ((c22 != 'E' && c22 != 'e') || buffer[startIndex++] != '_')
												{
													break;
												}
												char c23 = buffer[startIndex++];
												if (c23 != 'C' && c23 != 'c')
												{
													break;
												}
												char c24 = buffer[startIndex++];
												if (c24 != 'O' && c24 != 'o')
												{
													break;
												}
												char c25 = buffer[startIndex++];
												if (c25 != 'N' && c25 != 'n')
												{
													break;
												}
												char c26 = buffer[startIndex++];
												if (c26 != 'S' && c26 != 's')
												{
													break;
												}
												char c27 = buffer[startIndex++];
												if (c27 != 'T' && c27 != 't')
												{
													break;
												}
												char c28 = buffer[startIndex++];
												if (c28 != 'R' && c28 != 'r')
												{
													break;
												}
												char c29 = buffer[startIndex++];
												if (c29 != 'A' && c29 != 'a')
												{
													break;
												}
												char c30 = buffer[startIndex++];
												if (c30 != 'I' && c30 != 'i')
												{
													break;
												}
												char c31 = buffer[startIndex++];
												if (c31 != 'N' && c31 != 'n')
												{
													break;
												}
												char c32 = buffer[startIndex++];
												if ((c32 != 'T' && c32 != 't') || buffer[startIndex++] != '_')
												{
													break;
												}
												char c33 = buffer[startIndex++];
												if (c33 != 'N' && c33 != 'n')
												{
													break;
												}
												char c34 = buffer[startIndex++];
												if (c34 != 'A' && c34 != 'a')
												{
													break;
												}
												char c35 = buffer[startIndex++];
												if (c35 == 'M' || c35 == 'm')
												{
													char c36 = buffer[startIndex++];
													if (c36 == 'E' || c36 == 'e')
													{
														result = StringToken.FileTableStreamIdUniqueConstraintName;
													}
												}
												break;
											}
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
		return result;
	}
}
