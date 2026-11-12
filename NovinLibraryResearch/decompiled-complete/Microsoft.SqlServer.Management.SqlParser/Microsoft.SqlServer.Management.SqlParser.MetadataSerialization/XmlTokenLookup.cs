namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;

internal static class XmlTokenLookup
{
	public const int MinTokenValue = 0;

	public const int MaxTokenValue = 214;

	public static XmlToken Find(string buffer)
	{
		return Find(buffer, 0, buffer.Length);
	}

	public static XmlToken Find(string buffer, int startIndex, int length)
	{
		return length switch
		{
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
			27 => Find_Len27(buffer, startIndex), 
			35 => Find_Len35(buffer, startIndex), 
			36 => Find_Len36(buffer, startIndex), 
			37 => Find_Len37(buffer, startIndex), 
			38 => Find_Len38(buffer, startIndex), 
			_ => XmlToken.None, 
		};
	}

	private static XmlToken Find_Len2(string buffer, int startIndex)
	{
		XmlToken result = XmlToken.None;
		switch (buffer[startIndex++])
		{
		case 'M':
		case 'm':
		{
			char c = buffer[startIndex++];
			if (c == 'M' || c == 'm')
			{
				result = XmlToken.MM;
			}
			break;
		}
		case 'R':
		case 'r':
		{
			char c = buffer[startIndex++];
			if (c == 'E' || c == 'e')
			{
				result = XmlToken.RE;
			}
			break;
		}
		}
		return result;
	}

	private static XmlToken Find_Len3(string buffer, int startIndex)
	{
		XmlToken result = XmlToken.None;
		switch (buffer[startIndex++])
		{
		case 'K':
		case 'k':
		{
			char c = buffer[startIndex++];
			if (c == 'E' || c == 'e')
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'Y' || c2 == 'y')
				{
					result = XmlToken.Key;
				}
			}
			break;
		}
		case 'S':
		case 's':
		{
			char c = buffer[startIndex++];
			if (c == 'I' || c == 'i')
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'D' || c2 == 'd')
				{
					result = XmlToken.Sid;
				}
			}
			break;
		}
		}
		return result;
	}

	private static XmlToken Find_Len4(string buffer, int startIndex)
	{
		XmlToken result = XmlToken.None;
		switch (buffer[startIndex++])
		{
		case 'N':
		case 'n':
		{
			char c = buffer[startIndex++];
			if (c != 'A' && c != 'a')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 == 'M' || c2 == 'm')
			{
				char c3 = buffer[startIndex++];
				if (c3 == 'E' || c3 == 'e')
				{
					result = XmlToken.Name;
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
			if (c2 == 'E' || c2 == 'e')
			{
				char c3 = buffer[startIndex++];
				if (c3 == 'D' || c3 == 'd')
				{
					result = XmlToken.Seed;
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
				char c2 = buffer[startIndex++];
				if (c2 == 'X' || c2 == 'x')
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'T' || c3 == 't')
					{
						result = XmlToken.Text;
					}
				}
				break;
			}
			case 'Y':
			case 'y':
			{
				char c2 = buffer[startIndex++];
				if (c2 == 'P' || c2 == 'p')
				{
					char c3 = buffer[startIndex++];
					if (c3 == 'E' || c3 == 'e')
					{
						result = XmlToken.Type;
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
					result = XmlToken.User;
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
					result = XmlToken.View;
				}
			}
			break;
		}
		}
		return result;
	}

	private static XmlToken Find_Len5(string buffer, int startIndex)
	{
		XmlToken result = XmlToken.None;
		switch (buffer[startIndex++])
		{
		case 'L':
		case 'l':
		{
			char c = buffer[startIndex++];
			if (c != 'O' && c != 'o')
			{
				break;
			}
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
					result = XmlToken.Login;
				}
			}
			break;
		}
		case 'O':
		case 'o':
		{
			char c = buffer[startIndex++];
			if (c != 'W' && c != 'w')
			{
				break;
			}
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
					result = XmlToken.Owner;
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
			if (c2 != 'L' && c2 != 'l')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 == 'E' || c3 == 'e')
			{
				char c4 = buffer[startIndex++];
				if (c4 == 'S' || c4 == 's')
				{
					result = XmlToken.Roles;
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
					result = XmlToken.Table;
				}
			}
			break;
		}
		case 'U':
		case 'u':
		{
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
			if (c3 == 'R' || c3 == 'r')
			{
				char c4 = buffer[startIndex++];
				if (c4 == 'S' || c4 == 's')
				{
					result = XmlToken.Users;
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
			if (c2 != 'E' && c2 != 'e')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 == 'W' || c3 == 'w')
			{
				char c4 = buffer[startIndex++];
				if (c4 == 'S' || c4 == 's')
				{
					result = XmlToken.Views;
				}
			}
			break;
		}
		}
		return result;
	}

	private static XmlToken Find_Len6(string buffer, int startIndex)
	{
		XmlToken result = XmlToken.None;
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
			if (c4 == 'M' || c4 == 'm')
			{
				char c5 = buffer[startIndex++];
				if (c5 == 'N' || c5 == 'n')
				{
					result = XmlToken.Column;
				}
			}
			break;
		}
		case 'D':
		case 'd':
		{
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
					result = XmlToken.Delete;
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
					result = XmlToken.Insert;
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
				if (c4 == 'T' || c4 == 't')
				{
					char c5 = buffer[startIndex++];
					if (c5 == 'H' || c5 == 'h')
					{
						result = XmlToken.Length;
					}
				}
				break;
			}
			case 'O':
			case 'o':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'G' && c2 != 'g')
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
					if (c5 == 'S' || c5 == 's')
					{
						result = XmlToken.Logins;
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
			if (c3 != 'E' && c3 != 'e')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 == 'N' || c4 == 'n')
			{
				char c5 = buffer[startIndex++];
				if (c5 == 'T' || c5 == 't')
				{
					result = XmlToken.Parent;
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
				if (c4 == 'M' || c4 == 'm')
				{
					char c5 = buffer[startIndex++];
					if (c5 == 'A' || c5 == 'a')
					{
						result = XmlToken.Schema;
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
						result = XmlToken.Server;
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
			if (c4 == 'E' || c4 == 'e')
			{
				char c5 = buffer[startIndex++];
				if (c5 == 'S' || c5 == 's')
				{
					result = XmlToken.Tables;
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
			if (c4 == 'T' || c4 == 't')
			{
				char c5 = buffer[startIndex++];
				if (c5 == 'E' || c5 == 'e')
				{
					result = XmlToken.Update;
				}
			}
			break;
		}
		}
		return result;
	}

	private static XmlToken Find_Len7(string buffer, int startIndex)
	{
		XmlToken result = XmlToken.None;
		switch (buffer[startIndex++])
		{
		case 'C':
		case 'c':
		{
			char c6 = buffer[startIndex++];
			if (c6 != 'O' && c6 != 'o')
			{
				break;
			}
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
			if (c3 != 'M' && c3 != 'm')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 == 'N' || c4 == 'n')
			{
				char c5 = buffer[startIndex++];
				if (c5 == 'S' || c5 == 's')
				{
					result = XmlToken.Columns;
				}
			}
			break;
		}
		case 'G':
		case 'g':
		{
			char c6 = buffer[startIndex++];
			if (c6 != 'R' && c6 != 'r')
			{
				break;
			}
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
			if (c3 != 'T' && c3 != 't')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 == 'O' || c4 == 'o')
			{
				char c5 = buffer[startIndex++];
				if (c5 == 'R' || c5 == 'r')
				{
					result = XmlToken.Grantor;
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
			char c = buffer[startIndex++];
			if (c != 'D' && c != 'd')
			{
				break;
			}
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
			if (c4 == 'E' || c4 == 'e')
			{
				char c5 = buffer[startIndex++];
				if (c5 == 'S' || c5 == 's')
				{
					result = XmlToken.Indexes;
				}
			}
			break;
		}
		case 'M':
		case 'm':
		{
			char c6 = buffer[startIndex++];
			if (c6 != 'E' && c6 != 'e')
			{
				break;
			}
			char c = buffer[startIndex++];
			if (c != 'T' && c != 't')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'H' && c2 != 'h')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'O' && c3 != 'o')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 == 'D' || c4 == 'd')
			{
				char c5 = buffer[startIndex++];
				if (c5 == 'S' || c5 == 's')
				{
					result = XmlToken.Methods;
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
				if (c3 != 'M' && c3 != 'm')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 == 'A' || c4 == 'a')
				{
					char c5 = buffer[startIndex++];
					if (c5 == 'S' || c5 == 's')
					{
						result = XmlToken.Schemas;
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
				if (c3 != 'T' && c3 != 't')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 == 'U' || c4 == 'u')
				{
					char c5 = buffer[startIndex++];
					if (c5 == 'P' || c5 == 'p')
					{
						result = XmlToken.Startup;
					}
				}
				break;
			}
			case 'Y':
			case 'y':
			{
				char c = buffer[startIndex++];
				if (c != 'N' && c != 'n')
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
				if (c4 == 'Y' || c4 == 'y')
				{
					char c5 = buffer[startIndex++];
					if (c5 == 'M' || c5 == 'm')
					{
						result = XmlToken.Synonym;
					}
				}
				break;
			}
			}
			break;
		}
		return result;
	}

	private static XmlToken Find_Len8(string buffer, int startIndex)
	{
		XmlToken result = XmlToken.None;
		switch (buffer[startIndex++])
		{
		case 'B':
		case 'b':
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
						result = XmlToken.BaseType;
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
				if (c3 != 'Y' && c3 != 'y')
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
						result = XmlToken.BodyText;
					}
				}
				break;
			}
			}
			break;
		case 'D':
		case 'd':
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
			if (c3 != 'A' && c3 != 'a')
			{
				break;
			}
			switch (buffer[startIndex++])
			{
			case 'B':
			case 'b':
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
					if (c7 == 'E' || c7 == 'e')
					{
						result = XmlToken.Database;
					}
				}
				break;
			}
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
						result = XmlToken.DataType;
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
			case 'N':
			case 'n':
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
				if (c4 != 'X' && c4 != 'x')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'K' && c5 != 'k')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 == 'E' || c6 == 'e')
				{
					char c7 = buffer[startIndex++];
					if (c7 == 'Y' || c7 == 'y')
					{
						result = XmlToken.IndexKey;
					}
				}
				break;
			}
			case 'S':
			case 's':
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
						if (c7 == 'E' || c7 == 'e')
						{
							result = XmlToken.IsInline;
						}
					}
					break;
				}
				case 'O':
				case 'o':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'U' && c3 != 'u')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'T' && c4 != 't')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'P' && c5 != 'p')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 == 'U' || c6 == 'u')
					{
						char c7 = buffer[startIndex++];
						if (c7 == 'T' || c7 == 't')
						{
							result = XmlToken.IsOutput;
						}
					}
					break;
				}
				case 'S':
				case 's':
					switch (buffer[startIndex++])
					{
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
						if (c6 == 'S' || c6 == 's')
						{
							char c7 = buffer[startIndex++];
							if (c7 == 'E' || c7 == 'e')
							{
								result = XmlToken.IsSparse;
							}
						}
						break;
					}
					case 'Q':
					case 'q':
					{
						char c4 = buffer[startIndex++];
						if (c4 != 'L' && c4 != 'l')
						{
							break;
						}
						char c5 = buffer[startIndex++];
						if (c5 != 'C' && c5 != 'c')
						{
							break;
						}
						char c6 = buffer[startIndex++];
						if (c6 == 'L' || c6 == 'l')
						{
							char c7 = buffer[startIndex++];
							if (c7 == 'R' || c7 == 'r')
							{
								result = XmlToken.IsSqlClr;
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
						if (c5 != 'T' && c5 != 't')
						{
							break;
						}
						char c6 = buffer[startIndex++];
						if (c6 == 'I' || c6 == 'i')
						{
							char c7 = buffer[startIndex++];
							if (c7 == 'C' || c7 == 'c')
							{
								result = XmlToken.IsStatic;
							}
						}
						break;
					}
					}
					break;
				case 'U':
				case 'u':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'N' && c3 != 'n')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'I' && c4 != 'i')
					{
						break;
					}
					char c5 = buffer[startIndex++];
					if (c5 != 'Q' && c5 != 'q')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 == 'U' || c6 == 'u')
					{
						char c7 = buffer[startIndex++];
						if (c7 == 'E' || c7 == 'e')
						{
							result = XmlToken.IsUnique;
						}
					}
					break;
				}
				}
				break;
			}
			break;
		case 'L':
		case 'l':
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
					result = XmlToken.Language;
				}
			}
			break;
		}
		case 'N':
		case 'n':
		{
			char c = buffer[startIndex++];
			if (c != 'U' && c != 'u')
			{
				break;
			}
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
					result = XmlToken.Nullable;
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
			switch (buffer[startIndex++])
			{
			case 'D':
			case 'd':
			{
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
						result = XmlToken.PadIndex;
					}
				}
				break;
			}
			case 'S':
			case 's':
			{
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
						result = XmlToken.Password;
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
			char c = buffer[startIndex++];
			if (c != 'Y' && c != 'y')
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
			if (c4 != 'N' && c4 != 'n')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'Y' && c5 != 'y')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 == 'M' || c6 == 'm')
			{
				char c7 = buffer[startIndex++];
				if (c7 == 'S' || c7 == 's')
				{
					result = XmlToken.Synonyms;
				}
			}
			break;
		}
		case 'T':
		case 't':
			switch (buffer[startIndex++])
			{
			case 'R':
			case 'r':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'I' && c2 != 'i')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'G' && c3 != 'g')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'G' && c4 != 'g')
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
					if (c7 == 'S' || c7 == 's')
					{
						result = XmlToken.Triggers;
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
				if (c5 != 'P' && c5 != 'p')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 == 'E' || c6 == 'e')
				{
					char c7 = buffer[startIndex++];
					if (c7 == 'C' || c7 == 'c')
					{
						result = XmlToken.TypeSpec;
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
					result = XmlToken.UserType;
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
					result = XmlToken.XmlIndex;
				}
			}
			break;
		}
		}
		return result;
	}

	private static XmlToken Find_Len9(string buffer, int startIndex)
	{
		XmlToken result = XmlToken.None;
		switch (buffer[startIndex++])
		{
		case 'C':
		case 'c':
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
						result = XmlToken.ClassName;
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
						result = XmlToken.Collation;
					}
				}
				break;
			}
			}
			break;
		case 'D':
		case 'd':
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
			if (c7 == 'E' || c7 == 'e')
			{
				char c8 = buffer[startIndex++];
				if (c8 == 'S' || c8 == 's')
				{
					result = XmlToken.Databases;
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
			char c3 = buffer[startIndex++];
			if (c3 != 'E' && c3 != 'e')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'G' && c4 != 'g')
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
			if (c7 == 'U' || c7 == 'u')
			{
				char c8 = buffer[startIndex++];
				if (c8 == 'P' || c8 == 'p')
				{
					result = XmlToken.FileGroup;
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
							result = XmlToken.Increment;
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
					switch (buffer[startIndex++])
					{
					case 'A':
					case 'a':
					{
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
						if (c7 == 'E' || c7 == 'e')
						{
							char c8 = buffer[startIndex++];
							if (c8 == 'S' || c8 == 's')
							{
								result = XmlToken.Instances;
							}
						}
						break;
					}
					case 'E':
					case 'e':
					{
						char c5 = buffer[startIndex++];
						if (c5 != 'A' && c5 != 'a')
						{
							break;
						}
						char c6 = buffer[startIndex++];
						if (c6 != 'D' && c6 != 'd')
						{
							break;
						}
						char c7 = buffer[startIndex++];
						if (c7 == 'O' || c7 == 'o')
						{
							char c8 = buffer[startIndex++];
							if (c8 == 'F' || c8 == 'f')
							{
								result = XmlToken.InsteadOf;
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
				case 'C':
				case 'c':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'H' && c3 != 'h')
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
					if (c6 != 'K' && c6 != 'k')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 == 'E' || c7 == 'e')
					{
						char c8 = buffer[startIndex++];
						if (c8 == 'D' || c8 == 'd')
						{
							result = XmlToken.IsChecked;
						}
					}
					break;
				}
				case 'D':
				case 'd':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'E' && c3 != 'e')
					{
						break;
					}
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
					if (c6 != 'U' && c6 != 'u')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 == 'L' || c7 == 'l')
					{
						char c8 = buffer[startIndex++];
						if (c8 == 'T' || c8 == 't')
						{
							result = XmlToken.IsDefault;
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
					if (c7 == 'E' || c7 == 'e')
					{
						char c8 = buffer[startIndex++];
						if (c8 == 'D' || c8 == 'd')
						{
							result = XmlToken.IsEnabled;
						}
					}
					break;
				}
				case 'X':
				case 'x':
				{
					char c3 = buffer[startIndex++];
					if (c3 != 'M' && c3 != 'm')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'L' && c4 != 'l')
					{
						break;
					}
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
					if (c7 == 'D' || c7 == 'd')
					{
						char c8 = buffer[startIndex++];
						if (c8 == 'E' || c8 == 'e')
						{
							result = XmlToken.IsXmlNode;
						}
					}
					break;
				}
				}
				break;
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
			char c2 = buffer[startIndex++];
			if (c2 != 'G' && c2 != 'g')
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
			if (c5 != 'T' && c5 != 't')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'Y' && c6 != 'y')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 == 'P' || c7 == 'p')
			{
				char c8 = buffer[startIndex++];
				if (c8 == 'E' || c8 == 'e')
				{
					result = XmlToken.LoginType;
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
			if (c4 != 'Y' && c4 != 'y')
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
					result = XmlToken.QueryText;
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
			if (c2 != 'F' && c2 != 'f')
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
			if (c7 == 'C' || c7 == 'c')
			{
				char c8 = buffer[startIndex++];
				if (c8 == 'E' || c8 == 'e')
				{
					result = XmlToken.Reference;
				}
			}
			break;
		}
		case 'S':
		case 's':
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
			if (c6 != 'D' && c6 != 'd')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 == 'E' || c7 == 'e')
			{
				char c8 = buffer[startIndex++];
				if (c8 == 'R' || c8 == 'r')
				{
					result = XmlToken.SortOrder;
				}
			}
			break;
		}
		case 'U':
		case 'u':
		{
			char c = buffer[startIndex++];
			if (c != 'D' && c != 'd')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'T' && c2 != 't')
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
			if (c6 != 'H' && c6 != 'h')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 == 'O' || c7 == 'o')
			{
				char c8 = buffer[startIndex++];
				if (c8 == 'D' || c8 == 'd')
				{
					result = XmlToken.UdtMethod;
				}
			}
			break;
		}
		}
		return result;
	}

	private static XmlToken Find_Len10(string buffer, int startIndex)
	{
		XmlToken result = XmlToken.None;
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
					result = XmlToken.Credential;
				}
			}
			break;
		}
		case 'D':
		case 'd':
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
			if (c8 == 'E' || c8 == 'e')
			{
				char c9 = buffer[startIndex++];
				if (c9 == 'R' || c9 == 'r')
				{
					result = XmlToken.DmlTrigger;
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
				if (c4 != 'G' && c4 != 'g')
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
				if (c7 != 'U' && c7 != 'u')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 == 'P' || c8 == 'p')
				{
					char c9 = buffer[startIndex++];
					if (c9 == 'S' || c9 == 's')
					{
						result = XmlToken.FileGroups;
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
						result = XmlToken.FillFactor;
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
			if (c != 'S' && c != 's')
			{
				break;
			}
			switch (buffer[startIndex++])
			{
			case 'D':
			case 'd':
			{
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
				if (c7 != 'L' && c7 != 'l')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 == 'E' || c8 == 'e')
				{
					char c9 = buffer[startIndex++];
					if (c9 == 'D' || c9 == 'd')
					{
						result = XmlToken.IsDisabled;
					}
				}
				break;
			}
			case 'I':
			case 'i':
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
				if (c7 != 'D' && c7 != 'd')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 == 'E' || c8 == 'e')
				{
					char c9 = buffer[startIndex++];
					if (c9 == 'D' || c9 == 'd')
					{
						result = XmlToken.IsIncluded;
					}
				}
				break;
			}
			case 'N':
			case 'n':
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
				if (c7 != 'B' && c7 != 'b')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 == 'L' || c8 == 'l')
				{
					char c9 = buffer[startIndex++];
					if (c9 == 'E' || c9 == 'e')
					{
						result = XmlToken.IsNullable;
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
				if (c4 != 'A' && c4 != 'a')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'D' && c5 != 'd')
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
				if (c8 == 'L' || c8 == 'l')
				{
					char c9 = buffer[startIndex++];
					if (c9 == 'Y' || c9 == 'y')
					{
						result = XmlToken.IsReadOnly;
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
			if (c8 == 'R' || c8 == 'r')
			{
				char c9 = buffer[startIndex++];
				if (c9 == 'S' || c9 == 's')
				{
					result = XmlToken.Parameters;
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
				char c2 = buffer[startIndex++];
				if (c2 != 'T' && c2 != 't')
				{
					break;
				}
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
				if (c7 != 'Y' && c7 != 'y')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 == 'P' || c8 == 'p')
				{
					char c9 = buffer[startIndex++];
					if (c9 == 'E' || c9 == 'e')
					{
						result = XmlToken.ReturnType;
					}
				}
				break;
			}
			case 'O':
			case 'o':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'W' && c2 != 'w')
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
						result = XmlToken.RowGuidCol;
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
					result = XmlToken.Statistics;
				}
			}
			break;
		}
		}
		return result;
	}

	private static XmlToken Find_Len11(string buffer, int startIndex)
	{
		XmlToken result = XmlToken.None;
		switch (buffer[startIndex++])
		{
		case 'C':
		case 'c':
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
				if (c9 == 'T' || c9 == 't')
				{
					char c10 = buffer[startIndex++];
					if (c10 == 'E' || c10 == 'e')
					{
						result = XmlToken.Certificate;
					}
				}
				break;
			}
			case 'L':
			case 'l':
			{
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
				if (c7 != 'T' && c7 != 't')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'Y' && c8 != 'y')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 == 'P' || c9 == 'p')
				{
					char c10 = buffer[startIndex++];
					if (c10 == 'E' || c10 == 'e')
					{
						result = XmlToken.ClrDataType;
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
				case 'S':
				case 's':
				{
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
					if (c8 != 'N' && c8 != 'n')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 == 'T' || c9 == 't')
					{
						char c10 = buffer[startIndex++];
						if (c10 == 'S' || c10 == 's')
						{
							result = XmlToken.Constraints;
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
					if (c5 != 'X' && c5 != 'x')
					{
						break;
					}
					char c6 = buffer[startIndex++];
					if (c6 != 'T' && c6 != 't')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'T' && c7 != 't')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'Y' && c8 != 'y')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 == 'P' || c9 == 'p')
					{
						char c10 = buffer[startIndex++];
						if (c10 == 'E' || c10 == 'e')
						{
							result = XmlToken.ContextType;
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
				if (c8 != 'A' && c8 != 'a')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 == 'L' || c9 == 'l')
				{
					char c10 = buffer[startIndex++];
					if (c10 == 'S' || c10 == 's')
					{
						result = XmlToken.Credentials;
					}
				}
				break;
			}
			}
			break;
		case 'D':
		case 'd':
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
			if (c8 != 'E' && c8 != 'e')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 == 'R' || c9 == 'r')
			{
				char c10 = buffer[startIndex++];
				if (c10 == 'S' || c10 == 's')
				{
					result = XmlToken.DataMembers;
				}
			}
			break;
		}
		case 'I':
		case 'i':
		{
			char c = buffer[startIndex++];
			if (c != 'S' && c != 's')
			{
				break;
			}
			switch (buffer[startIndex++])
			{
			case 'C':
			case 'c':
				switch (buffer[startIndex++])
				{
				case 'L':
				case 'l':
				{
					char c4 = buffer[startIndex++];
					if (c4 != 'U' && c4 != 'u')
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
					if (c8 != 'R' && c8 != 'r')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 == 'E' || c9 == 'e')
					{
						char c10 = buffer[startIndex++];
						if (c10 == 'D' || c10 == 'd')
						{
							result = XmlToken.IsClustered;
						}
					}
					break;
				}
				case 'O':
				case 'o':
				{
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
					if (c6 != 'M' && c6 != 'm')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'N' && c7 != 'n')
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
						if (c10 == 'T' || c10 == 't')
						{
							result = XmlToken.IsColumnSet;
						}
					}
					break;
				}
				}
				break;
			case 'E':
			case 'e':
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
				if (c5 != 'R' && c5 != 'r')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'Y' && c6 != 'y')
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
				if (c9 == 'E' || c9 == 'e')
				{
					char c10 = buffer[startIndex++];
					if (c10 == 'D' || c10 == 'd')
					{
						result = XmlToken.IsEncrypted;
					}
				}
				break;
			}
			case 'F':
			case 'f':
			{
				char c3 = buffer[startIndex++];
				if (c3 != 'I' && c3 != 'i')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'X' && c4 != 'x')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'E' && c5 != 'e')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'D' && c6 != 'd')
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
						result = XmlToken.IsFixedRole;
					}
				}
				break;
			}
			case 'P':
			case 'p':
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
				if (c9 == 'E' || c9 == 'e')
				{
					char c10 = buffer[startIndex++];
					if (c10 == 'D' || c10 == 'd')
					{
						result = XmlToken.IsPersisted;
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
			if (c3 != 'M' && c3 != 'm')
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
			if (c8 != 'O' && c8 != 'o')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 == 'N' || c9 == 'n')
			{
				char c10 = buffer[startIndex++];
				if (c10 == 'S' || c10 == 's')
				{
					result = XmlToken.Permissions;
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
			if (c7 != 'T' && c7 != 't')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'Y' && c8 != 'y')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 == 'P' || c9 == 'p')
			{
				char c10 = buffer[startIndex++];
				if (c10 == 'E' || c10 == 'e')
				{
					result = XmlToken.XmlDataType;
				}
			}
			break;
		}
		}
		return result;
	}

	private static XmlToken Find_Len12(string buffer, int startIndex)
	{
		XmlToken result = XmlToken.None;
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
			if (c6 != 'L' && c6 != 'l')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'Y' && c7 != 'y')
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
					result = XmlToken.AssemblyName;
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
			if (c10 == 'E' || c10 == 'e')
			{
				char c11 = buffer[startIndex++];
				if (c11 == 'S' || c11 == 's')
				{
					result = XmlToken.Certificates;
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
				if (c9 != 'O' && c9 != 'o')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 == 'L' || c10 == 'l')
				{
					char c11 = buffer[startIndex++];
					if (c11 == 'E' || c11 == 'e')
					{
						result = XmlToken.DatabaseRole;
					}
				}
				break;
			}
			case 'E':
			case 'e':
				switch (buffer[startIndex++])
				{
				case 'F':
				case 'f':
				{
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
					if (c6 != 'T' && c6 != 't')
					{
						break;
					}
					char c7 = buffer[startIndex++];
					if (c7 != 'V' && c7 != 'v')
					{
						break;
					}
					char c8 = buffer[startIndex++];
					if (c8 != 'A' && c8 != 'a')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'L' && c9 != 'l')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 == 'U' || c10 == 'u')
					{
						char c11 = buffer[startIndex++];
						if (c11 == 'E' || c11 == 'e')
						{
							result = XmlToken.DefaultValue;
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
					if (c6 != 'A' && c6 != 'a')
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
							result = XmlToken.DeleteAction;
						}
					}
					break;
				}
				}
				break;
			}
			break;
		case 'I':
		case 'i':
			switch (buffer[startIndex++])
			{
			case 'N':
			case 'n':
			{
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
				if (c9 != 'K' && c9 != 'k')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 == 'E' || c10 == 'e')
				{
					char c11 = buffer[startIndex++];
					if (c11 == 'Y' || c11 == 'y')
					{
						result = XmlToken.InPrimaryKey;
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
					char c3 = buffer[startIndex++];
					if (c3 != 'O' && c3 != 'o')
					{
						break;
					}
					char c4 = buffer[startIndex++];
					if (c4 != 'M' && c4 != 'm')
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
							result = XmlToken.IsComVisible;
						}
					}
					break;
				}
				case 'F':
				case 'f':
				{
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
					if (c9 != 'E' && c9 != 'e')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 == 'A' || c10 == 'a')
					{
						char c11 = buffer[startIndex++];
						if (c11 == 'M' || c11 == 'm')
						{
							result = XmlToken.IsFileStream;
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
					if (c8 != 'I' && c8 != 'i')
					{
						break;
					}
					char c9 = buffer[startIndex++];
					if (c9 != 'L' && c9 != 'l')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 == 'E' || c10 == 'e')
					{
						char c11 = buffer[startIndex++];
						if (c11 == 'D' || c11 == 'd')
						{
							result = XmlToken.IsRecompiled;
						}
					}
					break;
				}
				}
				break;
			}
			break;
		case 'N':
		case 'n':
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
			if (c10 == 'L' || c10 == 'l')
			{
				char c11 = buffer[startIndex++];
				if (c11 == 'E' || c11 == 'e')
				{
					result = XmlToken.NumericScale;
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
			if (c2 != 'F' && c2 != 'f')
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
			if (c8 != 'E' && c8 != 'e')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'K' && c9 != 'k')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 == 'E' || c10 == 'e')
			{
				char c11 = buffer[startIndex++];
				if (c11 == 'Y' || c11 == 'y')
				{
					result = XmlToken.ReferenceKey;
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
			if (c10 == 'E' || c10 == 'e')
			{
				char c11 = buffer[startIndex++];
				if (c11 == 'X' || c11 == 'x')
				{
					result = XmlToken.SpatialIndex;
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
			if (c7 != 'B' && c7 != 'b')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'J' && c8 != 'j')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'E' && c9 != 'e')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 == 'C' || c10 == 'c')
			{
				char c11 = buffer[startIndex++];
				if (c11 == 'T' || c11 == 't')
				{
					result = XmlToken.TargetObject;
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
			if (c6 != 'A' && c6 != 'a')
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
					result = XmlToken.UpdateAction;
				}
			}
			break;
		}
		case 'V':
		case 'v':
		{
			char c = buffer[startIndex++];
			if (c != 'O' && c != 'o')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'I' && c2 != 'i')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'D' && c3 != 'd')
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
					result = XmlToken.VoidDataType;
				}
			}
			break;
		}
		}
		return result;
	}

	private static XmlToken Find_Len13(string buffer, int startIndex)
	{
		XmlToken result = XmlToken.None;
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
			if (c10 != 'K' && c10 != 'k')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 == 'E' || c11 == 'e')
			{
				char c12 = buffer[startIndex++];
				if (c12 == 'Y' || c12 == 'y')
				{
					result = XmlToken.AsymmetricKey;
				}
			}
			break;
		}
		case 'C':
		case 'c':
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
			if (c8 != 'N' && c8 != 'n')
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
			if (c11 == 'F' || c11 == 'f')
			{
				char c12 = buffer[startIndex++];
				if (c12 == 'O' || c12 == 'o')
				{
					result = XmlToken.CollationInfo;
				}
			}
			break;
		}
		case 'D':
		case 'd':
		{
			char c = buffer[startIndex++];
			if (c != 'E' && c != 'e')
			{
				break;
			}
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
			if (c6 != 'T' && c6 != 't')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'S' && c7 != 's')
			{
				break;
			}
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
					result = XmlToken.DefaultSchema;
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
				if (c4 != 'X' && c4 != 'x')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'E' && c5 != 'e')
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
				if (c10 != 'U' && c10 != 'u')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 == 'M' || c11 == 'm')
				{
					char c12 = buffer[startIndex++];
					if (c12 == 'N' || c12 == 'n')
					{
						result = XmlToken.IndexedColumn;
					}
				}
				break;
			}
			case 'S':
			case 's':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'S' && c2 != 's')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case 'C':
				case 'c':
				{
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
					if (c8 != 'B' && c8 != 'b')
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
					if (c11 == 'N' || c11 == 'n')
					{
						char c12 = buffer[startIndex++];
						if (c12 == 'D' || c12 == 'd')
						{
							result = XmlToken.IsSchemaBound;
						}
					}
					break;
				}
				case 'Y':
				case 'y':
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
					if (c10 != 'M' && c10 != 'm')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 == 'E' || c11 == 'e')
					{
						char c12 = buffer[startIndex++];
						if (c12 == 'D' || c12 == 'd')
						{
							result = XmlToken.IsSystemNamed;
						}
					}
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
			if (c != 'E' && c != 'e')
			{
				break;
			}
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
			if (c4 != 'L' && c4 != 'l')
			{
				break;
			}
			switch (buffer[startIndex++])
			{
			case '1':
			{
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
				if (c11 == 'T' || c11 == 't')
				{
					char c12 = buffer[startIndex++];
					if (c12 == 'Y' || c12 == 'y')
					{
						result = XmlToken.Level1Density;
					}
				}
				break;
			}
			case '2':
			{
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
				if (c11 == 'T' || c11 == 't')
				{
					char c12 = buffer[startIndex++];
					if (c12 == 'Y' || c12 == 'y')
					{
						result = XmlToken.Level2Density;
					}
				}
				break;
			}
			case '3':
			{
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
				if (c11 == 'T' || c11 == 't')
				{
					char c12 = buffer[startIndex++];
					if (c12 == 'Y' || c12 == 'y')
					{
						result = XmlToken.Level3Density;
					}
				}
				break;
			}
			case '4':
			{
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
				if (c11 == 'T' || c11 == 't')
				{
					char c12 = buffer[startIndex++];
					if (c12 == 'Y' || c12 == 'y')
					{
						result = XmlToken.Level4Density;
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
			if (c3 != 'B' && c3 != 'b')
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
			if (c6 != 'O' && c6 != 'o')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'F' && c7 != 'f')
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
			if (c10 != 'L' && c10 != 'l')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 == 'E' || c11 == 'e')
			{
				char c12 = buffer[startIndex++];
				if (c12 == 'S' || c12 == 's')
				{
					result = XmlToken.MemberOfRoles;
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
			if (c4 != 'E' && c4 != 'e')
			{
				break;
			}
			switch (buffer[startIndex++])
			{
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
						result = XmlToken.TableDataType;
					}
				}
				break;
			}
			case 'V':
			case 'v':
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
				if (c8 != 'I' && c8 != 'i')
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
						result = XmlToken.TableVariable;
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
			if (c != 'D' && c != 'd')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'T' && c2 != 't')
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
					result = XmlToken.UdtDataMember;
				}
			}
			break;
		}
		}
		return result;
	}

	private static XmlToken Find_Len14(string buffer, int startIndex)
	{
		XmlToken result = XmlToken.None;
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
			if (c10 != 'K' && c10 != 'k')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'E' && c11 != 'e')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 == 'Y' || c12 == 'y')
			{
				char c13 = buffer[startIndex++];
				if (c13 == 'S' || c13 == 's')
				{
					result = XmlToken.AsymmetricKeys;
				}
			}
			break;
		}
		case 'B':
		case 'b':
		{
			char c = buffer[startIndex++];
			if (c != 'A' && c != 'a')
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
			if (c6 != 'J' && c6 != 'j')
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
			if (c9 != 'T' && c9 != 't')
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
			if (c12 == 'M' || c12 == 'm')
			{
				char c13 = buffer[startIndex++];
				if (c13 == 'E' || c13 == 'e')
				{
					result = XmlToken.BaseObjectName;
				}
			}
			break;
		}
		case 'C':
		case 'c':
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
				if (c3 != 'L' && c3 != 'l')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'S' && c4 != 's')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'P' && c5 != 'p')
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
				if (c8 != 'O' && c8 != 'o')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'B' && c9 != 'b')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'J' && c10 != 'j')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'E' && c11 != 'e')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 == 'C' || c12 == 'c')
				{
					char c13 = buffer[startIndex++];
					if (c13 == 'T' || c13 == 't')
					{
						result = XmlToken.CellsPerObject;
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
					if (c10 != 'T' && c10 != 't')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 != 'Y' && c11 != 'y')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if (c12 == 'P' || c12 == 'p')
					{
						char c13 = buffer[startIndex++];
						if (c13 == 'E' || c13 == 'e')
						{
							result = XmlToken.CursorDataType;
						}
					}
					break;
				}
				case 'V':
				case 'v':
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
					if (c9 != 'I' && c9 != 'i')
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
					if (c12 == 'L' || c12 == 'l')
					{
						char c13 = buffer[startIndex++];
						if (c13 == 'E' || c13 == 'e')
						{
							result = XmlToken.CursorVariable;
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
			if (c12 == 'O' || c12 == 'o')
			{
				char c13 = buffer[startIndex++];
				if (c13 == 'N' || c13 == 'n')
				{
					result = XmlToken.ForReplication;
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
			if (c8 != 'O' && c8 != 'o')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'P' && c9 != 'p')
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
					result = XmlToken.HasCheckOption;
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
				if (c4 != 'X' && c4 != 'x')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'E' && c5 != 'e')
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
				if (c10 != 'U' && c10 != 'u')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'M' && c11 != 'm')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 == 'N' || c12 == 'n')
				{
					char c13 = buffer[startIndex++];
					if (c13 == 'S' || c13 == 's')
					{
						result = XmlToken.IndexedColumns;
					}
				}
				break;
			}
			case 'S':
			case 's':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'S' && c2 != 's')
				{
					break;
				}
				char c3 = buffer[startIndex++];
				if (c3 != 'Y' && c3 != 'y')
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
				if (c8 != 'O' && c8 != 'o')
				{
					break;
				}
				char c9 = buffer[startIndex++];
				if (c9 != 'B' && c9 != 'b')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'J' && c10 != 'j')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'E' && c11 != 'e')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 == 'C' || c12 == 'c')
				{
					char c13 = buffer[startIndex++];
					if (c13 == 'T' || c13 == 't')
					{
						result = XmlToken.IsSystemObject;
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
			if (c3 != 'M' && c3 != 'm')
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
			if (c10 != 'T' && c10 != 't')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'Y' && c11 != 'y')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 == 'P' || c12 == 'p')
			{
				char c13 = buffer[startIndex++];
				if (c13 == 'E' || c13 == 'e')
				{
					result = XmlToken.PermissionType;
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
				if (c4 != 'A' && c4 != 'a')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'R' && c5 != 'r')
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
					if (c10 != 'T' && c10 != 't')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 != 'Y' && c11 != 'y')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if (c12 == 'P' || c12 == 'p')
					{
						char c13 = buffer[startIndex++];
						if (c13 == 'E' || c13 == 'e')
						{
							result = XmlToken.ScalarDataType;
						}
					}
					break;
				}
				case 'V':
				case 'v':
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
					if (c9 != 'I' && c9 != 'i')
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
					if (c12 == 'L' || c12 == 'l')
					{
						char c13 = buffer[startIndex++];
						if (c13 == 'E' || c13 == 'e')
						{
							result = XmlToken.ScalarVariable;
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
				if (c5 != 'M' && c5 != 'm')
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
				if (c11 != 'Y' && c11 != 'y')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 == 'P' || c12 == 'p')
				{
					char c13 = buffer[startIndex++];
					if (c13 == 'E' || c13 == 'e')
					{
						result = XmlToken.SystemDataType;
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
			if (c4 != 'E' && c4 != 'e')
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
			if (c8 != 'A' && c8 != 'a')
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
					result = XmlToken.TableParameter;
				}
			}
			break;
		}
		}
		return result;
	}

	private static XmlToken Find_Len15(string buffer, int startIndex)
	{
		XmlToken result = XmlToken.None;
		switch (buffer[startIndex++])
		{
		case 'A':
		case 'a':
			switch (buffer[startIndex++])
			{
			case 'P':
			case 'p':
			{
				char c2 = buffer[startIndex++];
				if (c2 != 'P' && c2 != 'p')
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
				if (c10 != 'N' && c10 != 'n')
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
				if (c13 == 'L' || c13 == 'l')
				{
					char c14 = buffer[startIndex++];
					if (c14 == 'E' || c14 == 'e')
					{
						result = XmlToken.ApplicationRole;
					}
				}
				break;
			}
			case 'S':
			case 's':
			{
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
				if (c4 != 'C' && c4 != 'c')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'I' && c5 != 'i')
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
				if (c12 != 'D' && c12 != 'd')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 == 'E' || c13 == 'e')
				{
					char c14 = buffer[startIndex++];
					if (c14 == 'X' || c14 == 'x')
					{
						result = XmlToken.AssociatedIndex;
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
			if (c != 'O' && c != 'o')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'U' && c2 != 'u')
			{
				break;
			}
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
			if (c7 != 'G' && c7 != 'g')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'B' && c8 != 'b')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'O' && c9 != 'o')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'X' && c10 != 'x')
			{
				break;
			}
			switch (buffer[startIndex++])
			{
			case 'X':
			case 'x':
			{
				char c12 = buffer[startIndex++];
				if (c12 != 'M' && c12 != 'm')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case 'A':
				case 'a':
				{
					char c14 = buffer[startIndex++];
					if (c14 == 'X' || c14 == 'x')
					{
						result = XmlToken.BoundingBoxXMax;
					}
					break;
				}
				case 'I':
				case 'i':
				{
					char c14 = buffer[startIndex++];
					if (c14 == 'N' || c14 == 'n')
					{
						result = XmlToken.BoundingBoxXMin;
					}
					break;
				}
				}
				break;
			}
			case 'Y':
			case 'y':
			{
				char c12 = buffer[startIndex++];
				if (c12 != 'M' && c12 != 'm')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case 'A':
				case 'a':
				{
					char c14 = buffer[startIndex++];
					if (c14 == 'X' || c14 == 'x')
					{
						result = XmlToken.BoundingBoxYMax;
					}
					break;
				}
				case 'I':
				case 'i':
				{
					char c14 = buffer[startIndex++];
					if (c14 == 'N' || c14 == 'n')
					{
						result = XmlToken.BoundingBoxYMin;
					}
					break;
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
				if (c10 != 'R' && c10 != 'r')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'A' && c11 != 'a')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'I' && c12 != 'i')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 == 'N' || c13 == 'n')
				{
					char c14 = buffer[startIndex++];
					if (c14 == 'T' || c14 == 't')
					{
						result = XmlToken.CheckConstraint;
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
				if (c12 != 'T' && c12 != 't')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 == 'E' || c13 == 'e')
				{
					char c14 = buffer[startIndex++];
					if (c14 == 'R' || c14 == 'r')
					{
						result = XmlToken.CursorParameter;
					}
				}
				break;
			}
			}
			break;
		case 'D':
		case 'd':
		{
			char c = buffer[startIndex++];
			if (c != 'E' && c != 'e')
			{
				break;
			}
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
			if (c6 != 'T' && c6 != 't')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'D' && c7 != 'd')
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
			if (c12 != 'A' && c12 != 'a')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if (c13 == 'S' || c13 == 's')
			{
				char c14 = buffer[startIndex++];
				if (c14 == 'E' || c14 == 'e')
				{
					result = XmlToken.DefaultDatabase;
				}
			}
			break;
		}
		case 'I':
		case 'i':
		{
			char c = buffer[startIndex++];
			if (c != 'S' && c != 's')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'B' && c2 != 'b')
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
			if (c7 != 'Y' && c7 != 'y')
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
			if (c10 != 'D' && c10 != 'd')
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
			if (c13 == 'E' || c13 == 'e')
			{
				char c14 = buffer[startIndex++];
				if (c14 == 'D' || c14 == 'd')
				{
					result = XmlToken.IsBinaryOrdered;
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
				if (c9 != 'S' && c9 != 's')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'C' && c10 != 'c')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'H' && c11 != 'h')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'E' && c12 != 'e')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 == 'M' || c13 == 'm')
				{
					char c14 = buffer[startIndex++];
					if (c14 == 'E' || c14 == 'e')
					{
						result = XmlToken.PartitionScheme;
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
				if (c3 != 'M' && c3 != 'm')
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
				if (c10 != 'S' && c10 != 's')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'T' && c11 != 't')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'A' && c12 != 'a')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 == 'T' || c13 == 't')
				{
					char c14 = buffer[startIndex++];
					if (c14 == 'E' || c14 == 'e')
					{
						result = XmlToken.PermissionState;
					}
				}
				break;
			}
			}
			break;
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
				if (c10 != 'T' && c10 != 't')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'A' && c11 != 'a')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'B' && c12 != 'b')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 == 'L' || c13 == 'l')
				{
					char c14 = buffer[startIndex++];
					if (c14 == 'E' || c14 == 'e')
					{
						result = XmlToken.ReferencedTable;
					}
				}
				break;
			}
			case 'L':
			case 'l':
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
				if (c7 != 'N' && c7 != 'n')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'A' && c8 != 'a')
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
				if (c11 != 'N' && c11 != 'n')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'D' && c12 != 'd')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 == 'E' || c13 == 'e')
				{
					char c14 = buffer[startIndex++];
					if (c14 == 'X' || c14 == 'x')
					{
						result = XmlToken.RelationalIndex;
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
				if (c12 != 'T' && c12 != 't')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 == 'E' || c13 == 'e')
				{
					char c14 = buffer[startIndex++];
					if (c14 == 'R' || c14 == 'r')
					{
						result = XmlToken.ScalarParameter;
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
				if (c6 != 'D' && c6 != 'd')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'D' && c7 != 'd')
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
						result = XmlToken.ServerDdlEvents;
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
				if (c5 != 'D' && c5 != 'd')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'P' && c6 != 'p')
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
				if (c9 != 'C' && c9 != 'c')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'E' && c10 != 'e')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'D' && c11 != 'd')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'U' && c12 != 'u')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 == 'R' || c13 == 'r')
				{
					char c14 = buffer[startIndex++];
					if (c14 == 'E' || c14 == 'e')
					{
						result = XmlToken.StoredProcedure;
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
					result = XmlToken.UserDefinedType;
				}
			}
			break;
		}
		}
		return result;
	}

	private static XmlToken Find_Len16(string buffer, int startIndex)
	{
		XmlToken result = XmlToken.None;
		switch (buffer[startIndex++])
		{
		case 'A':
		case 'a':
		{
			char c = buffer[startIndex++];
			if (c != 'P' && c != 'p')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'P' && c2 != 'p')
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
			if (c10 != 'N' && c10 != 'n')
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
			if (c13 != 'L' && c13 != 'l')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 == 'E' || c14 == 'e')
			{
				char c15 = buffer[startIndex++];
				if (c15 == 'S' || c15 == 's')
				{
					result = XmlToken.ApplicationRoles;
				}
			}
			break;
		}
		case 'D':
		case 'd':
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
			if (c3 != 'A' && c3 != 'a')
			{
				break;
			}
			char c4 = buffer[startIndex++];
			if (c4 != 'L' && c4 != 'l')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'L' && c5 != 'l')
			{
				break;
			}
			char c6 = buffer[startIndex++];
			if (c6 != 'O' && c6 != 'o')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'W' && c7 != 'w')
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
			if (c10 != 'W' && c10 != 'w')
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
			if (c14 == 'K' || c14 == 'k')
			{
				char c15 = buffer[startIndex++];
				if (c15 == 'S' || c15 == 's')
				{
					result = XmlToken.DisallowRowLocks;
				}
			}
			break;
		}
		case 'E':
		case 'e':
		{
			char c = buffer[startIndex++];
			if (c != 'X' && c != 'x')
			{
				break;
			}
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
			if (c8 != 'N' && c8 != 'n')
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
			if (c13 != 'E' && c13 != 'e')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 == 'X' || c14 == 'x')
			{
				char c15 = buffer[startIndex++];
				if (c15 == 'T' || c15 == 't')
				{
					result = XmlToken.ExecutionContext;
				}
			}
			break;
		}
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
				if (c11 != 'I' && c11 != 'i')
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
				if (c14 == 'O' || c14 == 'o')
				{
					char c15 = buffer[startIndex++];
					if (c15 == 'N' || c15 == 'n')
					{
						result = XmlToken.FilterDefinition;
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
				if (c3 != 'E' && c3 != 'e')
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
				if (c9 != 'Y' && c9 != 'y')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'C' && c10 != 'c')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'O' && c11 != 'o')
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
				if (c14 == 'M' || c14 == 'm')
				{
					char c15 = buffer[startIndex++];
					if (c15 == 'N' || c15 == 'n')
					{
						result = XmlToken.ForeignKeyColumn;
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
			if (c10 != 'C' && c10 != 'c')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'I' && c11 != 'i')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 != 'S' && c12 != 's')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if (c13 != 'I' && c13 != 'i')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 == 'O' || c14 == 'o')
			{
				char c15 = buffer[startIndex++];
				if (c15 == 'N' || c15 == 'n')
				{
					result = XmlToken.NumericPrecision;
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
			if (c2 != 'F' && c2 != 'f')
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
			if (c10 != 'C' && c10 != 'c')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'O' && c11 != 'o')
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
			if (c14 == 'M' || c14 == 'm')
			{
				char c15 = buffer[startIndex++];
				if (c15 == 'N' || c15 == 'n')
				{
					result = XmlToken.ReferencedColumn;
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
				if (c6 != 'D' && c6 != 'd')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'D' && c7 != 'd')
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
				if (c12 != 'G' && c12 != 'g')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 != 'G' && c13 != 'g')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if (c14 == 'E' || c14 == 'e')
				{
					char c15 = buffer[startIndex++];
					if (c15 == 'R' || c15 == 'r')
					{
						result = XmlToken.ServerDdlTrigger;
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
				if (c5 != 'D' && c5 != 'd')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'P' && c6 != 'p')
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
				if (c9 != 'C' && c9 != 'c')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'E' && c10 != 'e')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'D' && c11 != 'd')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'U' && c12 != 'u')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 != 'R' && c13 != 'r')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if (c14 == 'E' || c14 == 'e')
				{
					char c15 = buffer[startIndex++];
					if (c15 == 'S' || c15 == 's')
					{
						result = XmlToken.StoredProcedures;
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
			if (c12 != 'A' && c12 != 'a')
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
				if (c15 == 'T' || c15 == 't')
				{
					result = XmlToken.UniqueConstraint;
				}
			}
			break;
		}
		}
		return result;
	}

	private static XmlToken Find_Len17(string buffer, int startIndex)
	{
		XmlToken result = XmlToken.None;
		switch (buffer[startIndex++])
		{
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
				if (c7 != 'E' && c7 != 'e')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case 'D':
				case 'd':
				{
					char c9 = buffer[startIndex++];
					if (c9 != 'D' && c9 != 'd')
					{
						break;
					}
					char c10 = buffer[startIndex++];
					if (c10 != 'L' && c10 != 'l')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 != 'E' && c11 != 'e')
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
					if (c14 != 'N' && c14 != 'n')
					{
						break;
					}
					char c15 = buffer[startIndex++];
					if (c15 == 'T' || c15 == 't')
					{
						char c16 = buffer[startIndex++];
						if (c16 == 'S' || c16 == 's')
						{
							result = XmlToken.DatabaseDdlEvents;
						}
					}
					break;
				}
				case 'P':
				case 'p':
				{
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
					if (c11 != 'N' && c11 != 'n')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if (c12 != 'C' && c12 != 'c')
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
					if (c15 == 'A' || c15 == 'a')
					{
						char c16 = buffer[startIndex++];
						if (c16 == 'L' || c16 == 'l')
						{
							result = XmlToken.DatabasePrincipal;
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
				if (c6 != 'T' && c6 != 't')
				{
					break;
				}
				switch (buffer[startIndex++])
				{
				case 'C':
				case 'c':
				{
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
					if (c10 != 'S' && c10 != 's')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 != 'T' && c11 != 't')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if (c12 != 'R' && c12 != 'r')
					{
						break;
					}
					char c13 = buffer[startIndex++];
					if (c13 != 'A' && c13 != 'a')
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
						if (c16 == 'T' || c16 == 't')
						{
							result = XmlToken.DefaultConstraint;
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
					if (c11 != 'M' && c11 != 'm')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if (c12 != 'A' && c12 != 'a')
					{
						break;
					}
					char c13 = buffer[startIndex++];
					if (c13 != 'N' && c13 != 'n')
					{
						break;
					}
					char c14 = buffer[startIndex++];
					if (c14 != 'A' && c14 != 'a')
					{
						break;
					}
					char c15 = buffer[startIndex++];
					if (c15 == 'M' || c15 == 'm')
					{
						char c16 = buffer[startIndex++];
						if (c16 == 'E' || c16 == 'e')
						{
							result = XmlToken.DefaultSchemaName;
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
				if (c3 != 'A' && c3 != 'a')
				{
					break;
				}
				char c4 = buffer[startIndex++];
				if (c4 != 'L' && c4 != 'l')
				{
					break;
				}
				char c5 = buffer[startIndex++];
				if (c5 != 'L' && c5 != 'l')
				{
					break;
				}
				char c6 = buffer[startIndex++];
				if (c6 != 'O' && c6 != 'o')
				{
					break;
				}
				char c7 = buffer[startIndex++];
				if (c7 != 'W' && c7 != 'w')
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
				if (c10 != 'G' && c10 != 'g')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'E' && c11 != 'e')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'L' && c12 != 'l')
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
				if (c15 == 'K' || c15 == 'k')
				{
					char c16 = buffer[startIndex++];
					if (c16 == 'S' || c16 == 's')
					{
						result = XmlToken.DisallowPageLocks;
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
			if (c3 != 'F' && c3 != 'f')
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
					result = XmlToken.NotForReplication;
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
			if (c2 != 'F' && c2 != 'f')
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
			if (c10 != 'G' && c10 != 'g')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'C' && c11 != 'c')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 != 'O' && c12 != 'o')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if (c13 != 'L' && c13 != 'l')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 != 'U' && c14 != 'u')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if (c15 == 'M' || c15 == 'm')
			{
				char c16 = buffer[startIndex++];
				if (c16 == 'N' || c16 == 'n')
				{
					result = XmlToken.ReferencingColumn;
				}
			}
			break;
		}
		case 'S':
		case 's':
		{
			char c = buffer[startIndex++];
			if (c != 'Y' && c != 'y')
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
			if (c6 != 'C' && c6 != 'c')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'L' && c7 != 'l')
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
			if (c14 != 'Y' && c14 != 'y')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if (c15 == 'P' || c15 == 'p')
			{
				char c16 = buffer[startIndex++];
				if (c16 == 'E' || c16 == 'e')
				{
					result = XmlToken.SystemClrDataType;
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
			if (c4 != 'E' && c4 != 'e')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'V' && c5 != 'v')
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
			if (c8 != 'I' && c8 != 'i')
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
			if (c13 != 'N' && c13 != 'n')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 != 'A' && c14 != 'a')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if (c15 == 'M' || c15 == 'm')
			{
				char c16 = buffer[startIndex++];
				if (c16 == 'E' || c16 == 'e')
				{
					result = XmlToken.TableVariableName;
				}
			}
			break;
		}
		}
		return result;
	}

	private static XmlToken Find_Len18(string buffer, int startIndex)
	{
		XmlToken result = XmlToken.None;
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
			if (c4 != 'S' && c4 != 's')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'Y' && c5 != 'y')
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
			if (c9 != 'M' && c9 != 'm')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'D' && c10 != 'd')
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
			if (c15 != 'Y' && c15 != 'y')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if (c16 == 'P' || c16 == 'p')
			{
				char c17 = buffer[startIndex++];
				if (c17 == 'E' || c17 == 'e')
				{
					result = XmlToken.BaseSystemDataType;
				}
			}
			break;
		}
		case 'C':
		case 'c':
		{
			char c = buffer[startIndex++];
			if (c != 'O' && c != 'o')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'M' && c2 != 'm')
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
				if (c7 != 'B' && c7 != 'b')
				{
					break;
				}
				char c8 = buffer[startIndex++];
				if (c8 != 'I' && c8 != 'i')
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
				if (c13 != 'L' && c13 != 'l')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if (c14 != 'E' && c14 != 'e')
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
					if (c17 == 'L' || c17 == 'l')
					{
						result = XmlToken.CompatibilityLevel;
					}
				}
				break;
			}
			case 'U':
			case 'u':
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
				if (c7 != 'D' && c7 != 'd')
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
				if (c11 != 'U' && c11 != 'u')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'M' && c12 != 'm')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 != 'N' && c13 != 'n')
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
				if (c16 == 'F' || c16 == 'f')
				{
					char c17 = buffer[startIndex++];
					if (c17 == 'O' || c17 == 'o')
					{
						result = XmlToken.ComputedColumnInfo;
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
			if (c7 != 'E' && c7 != 'e')
			{
				break;
			}
			switch (buffer[startIndex++])
			{
			case 'D':
			case 'd':
			{
				char c9 = buffer[startIndex++];
				if (c9 != 'D' && c9 != 'd')
				{
					break;
				}
				char c10 = buffer[startIndex++];
				if (c10 != 'L' && c10 != 'l')
				{
					break;
				}
				char c11 = buffer[startIndex++];
				if (c11 != 'T' && c11 != 't')
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
				if (c14 != 'G' && c14 != 'g')
				{
					break;
				}
				char c15 = buffer[startIndex++];
				if (c15 != 'G' && c15 != 'g')
				{
					break;
				}
				char c16 = buffer[startIndex++];
				if (c16 == 'E' || c16 == 'e')
				{
					char c17 = buffer[startIndex++];
					if (c17 == 'R' || c17 == 'r')
					{
						result = XmlToken.DatabaseDdlTrigger;
					}
				}
				break;
			}
			case 'P':
			case 'p':
			{
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
				if (c11 != 'M' && c11 != 'm')
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
				if (c14 != 'S' && c14 != 's')
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
						result = XmlToken.DatabasePermission;
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
			if (c != 'D' && c != 'd')
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
			if (c6 != 'T' && c6 != 't')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'Y' && c7 != 'y')
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
			if (c11 != 'U' && c11 != 'u')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 != 'M' && c12 != 'm')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if (c13 != 'N' && c13 != 'n')
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
			if (c16 == 'F' || c16 == 'f')
			{
				char c17 = buffer[startIndex++];
				if (c17 == 'O' || c17 == 'o')
				{
					result = XmlToken.IdentityColumnInfo;
				}
			}
			break;
		}
		case 'U':
		case 'u':
		{
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
			if (c3 != 'R' && c3 != 'r')
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
			if (c11 != 'C' && c11 != 'c')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 != 'L' && c12 != 'l')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if (c13 != 'R' && c13 != 'r')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 != 'T' && c14 != 't')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if (c15 != 'Y' && c15 != 'y')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if (c16 == 'P' || c16 == 'p')
			{
				char c17 = buffer[startIndex++];
				if (c17 == 'E' || c17 == 'e')
				{
					result = XmlToken.UserDefinedClrType;
				}
			}
			break;
		}
		}
		return result;
	}

	private static XmlToken Find_Len19(string buffer, int startIndex)
	{
		XmlToken result = XmlToken.None;
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
			if (c2 != 'M' && c2 != 'm')
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
			if (c9 != 'R' && c9 != 'r')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'G' && c10 != 'g')
			{
				break;
			}
			char c18 = buffer[startIndex++];
			if (c18 != 'E' && c18 != 'e')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'O' && c11 != 'o')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 != 'B' && c12 != 'b')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if (c13 != 'J' && c13 != 'j')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 != 'E' && c14 != 'e')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if (c15 != 'C' && c15 != 'c')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if (c16 == 'T' || c16 == 't')
			{
				char c17 = buffer[startIndex++];
				if (c17 == 'S' || c17 == 's')
				{
					result = XmlToken.CompactLargeObjects;
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
			if (c9 != 'M' && c9 != 'm')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'F' && c10 != 'f')
			{
				break;
			}
			char c18 = buffer[startIndex++];
			if (c18 != 'I' && c18 != 'i')
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
			if (c16 == 'U' || c16 == 'u')
			{
				char c17 = buffer[startIndex++];
				if (c17 == 'P' || c17 == 'p')
				{
					result = XmlToken.FileStreamFileGroup;
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
			if (c5 != 'E' && c5 != 'e')
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
			if (c10 != 'I' && c10 != 'i')
			{
				break;
			}
			char c18 = buffer[startIndex++];
			if (c18 != 'C' && c18 != 'c')
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
			if (c13 != 'E' && c13 != 'e')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 != 'K' && c14 != 'k')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if (c15 != 'E' && c15 != 'e')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if (c16 == 'Y' || c16 == 'y')
			{
				char c17 = buffer[startIndex++];
				if (c17 == 'S' || c17 == 's')
				{
					result = XmlToken.IgnoreDuplicateKeys;
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
			if (c2 != 'T' && c2 != 't')
			{
				break;
			}
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
			if (c9 != 'E' && c9 != 'e')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'W' && c10 != 'w')
			{
				break;
			}
			char c18 = buffer[startIndex++];
			if (c18 != 'M' && c18 != 'm')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'E' && c11 != 'e')
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
			if (c14 != 'D' && c14 != 'd')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if (c15 != 'A' && c15 != 'a')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if (c16 == 'T' || c16 == 't')
			{
				char c17 = buffer[startIndex++];
				if (c17 == 'A' || c17 == 'a')
				{
					result = XmlToken.ReturnsViewMetadata;
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
			if (c4 != 'E' && c4 != 'e')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'V' && c5 != 'v')
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
			if (c8 != 'U' && c8 != 'u')
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
			char c18 = buffer[startIndex++];
			if (c18 != 'F' && c18 != 'f')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'U' && c11 != 'u')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 != 'N' && c12 != 'n')
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
					result = XmlToken.TableValuedFunction;
				}
			}
			break;
		}
		case 'U':
		case 'u':
		{
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
			if (c3 != 'R' && c3 != 'r')
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
			if (c9 != 'E' && c9 != 'e')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'D' && c10 != 'd')
			{
				break;
			}
			switch (buffer[startIndex++])
			{
			case 'C':
			case 'c':
			{
				char c11 = buffer[startIndex++];
				if (c11 != 'L' && c11 != 'l')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'R' && c12 != 'r')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 != 'T' && c13 != 't')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if (c14 != 'Y' && c14 != 'y')
				{
					break;
				}
				char c15 = buffer[startIndex++];
				if (c15 != 'P' && c15 != 'p')
				{
					break;
				}
				char c16 = buffer[startIndex++];
				if (c16 == 'E' || c16 == 'e')
				{
					char c17 = buffer[startIndex++];
					if (c17 == 'S' || c17 == 's')
					{
						result = XmlToken.UserDefinedClrTypes;
					}
				}
				break;
			}
			case 'D':
			case 'd':
			{
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
				if (c15 != 'Y' && c15 != 'y')
				{
					break;
				}
				char c16 = buffer[startIndex++];
				if (c16 == 'P' || c16 == 'p')
				{
					char c17 = buffer[startIndex++];
					if (c17 == 'E' || c17 == 'e')
					{
						result = XmlToken.UserDefinedDataType;
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

	private static XmlToken Find_Len20(string buffer, int startIndex)
	{
		XmlToken result = XmlToken.None;
		switch (buffer[startIndex++])
		{
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
			if (c3 != 'E' && c3 != 'e')
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
			if (c9 != 'Y' && c9 != 'y')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'C' && c10 != 'c')
			{
				break;
			}
			char c19 = buffer[startIndex++];
			if (c19 != 'O' && c19 != 'o')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'N' && c11 != 'n')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 != 'S' && c12 != 's')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if (c13 != 'T' && c13 != 't')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 != 'R' && c14 != 'r')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if (c15 != 'A' && c15 != 'a')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if (c16 != 'I' && c16 != 'i')
			{
				break;
			}
			char c17 = buffer[startIndex++];
			if (c17 == 'N' || c17 == 'n')
			{
				char c18 = buffer[startIndex++];
				if (c18 == 'T' || c18 == 't')
				{
					result = XmlToken.ForeignKeyConstraint;
				}
			}
			break;
		}
		case 'I':
		case 'i':
		{
			char c = buffer[startIndex++];
			if (c != 'S' && c != 's')
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
			if (c4 != 'O' && c4 != 'o')
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
			if (c7 != 'D' && c7 != 'd')
			{
				break;
			}
			char c8 = buffer[startIndex++];
			if (c8 != 'I' && c8 != 'i')
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
			char c19 = buffer[startIndex++];
			if (c19 != 'N' && c19 != 'n')
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
			if (c15 != 'E' && c15 != 'e')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if (c16 != 'R' && c16 != 'r')
			{
				break;
			}
			char c17 = buffer[startIndex++];
			if (c17 == 'O' || c17 == 'o')
			{
				char c18 = buffer[startIndex++];
				if (c18 == 'N' || c18 == 'n')
				{
					result = XmlToken.IsQuotedIdentifierOn;
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
			if (c6 != 'Y' && c6 != 'y')
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
			if (c9 != 'Y' && c9 != 'y')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'C' && c10 != 'c')
			{
				break;
			}
			char c19 = buffer[startIndex++];
			if (c19 != 'O' && c19 != 'o')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'N' && c11 != 'n')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 != 'S' && c12 != 's')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if (c13 != 'T' && c13 != 't')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 != 'R' && c14 != 'r')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if (c15 != 'A' && c15 != 'a')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if (c16 != 'I' && c16 != 'i')
			{
				break;
			}
			char c17 = buffer[startIndex++];
			if (c17 == 'N' || c17 == 'n')
			{
				char c18 = buffer[startIndex++];
				if (c18 == 'T' || c18 == 't')
				{
					result = XmlToken.PrimaryKeyConstraint;
				}
			}
			break;
		}
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
			if (c9 != 'U' && c9 != 'u')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'E' && c10 != 'e')
			{
				break;
			}
			char c19 = buffer[startIndex++];
			if (c19 != 'D' && c19 != 'd')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'F' && c11 != 'f')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 != 'U' && c12 != 'u')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if (c13 != 'N' && c13 != 'n')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 != 'C' && c14 != 'c')
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
					result = XmlToken.ScalarValuedFunction;
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
			if (c4 != 'E' && c4 != 'e')
			{
				break;
			}
			char c5 = buffer[startIndex++];
			if (c5 != 'V' && c5 != 'v')
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
			if (c8 != 'U' && c8 != 'u')
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
			char c19 = buffer[startIndex++];
			if (c19 != 'F' && c19 != 'f')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'U' && c11 != 'u')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 != 'N' && c12 != 'n')
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
					result = XmlToken.TableValuedFunctions;
				}
			}
			break;
		}
		case 'U':
		case 'u':
		{
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
			if (c3 != 'R' && c3 != 'r')
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
			if (c9 != 'E' && c9 != 'e')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'D' && c10 != 'd')
			{
				break;
			}
			switch (buffer[startIndex++])
			{
			case 'A':
			case 'a':
			{
				char c11 = buffer[startIndex++];
				if (c11 != 'G' && c11 != 'g')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'G' && c12 != 'g')
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
				if (c15 != 'G' && c15 != 'g')
				{
					break;
				}
				char c16 = buffer[startIndex++];
				if (c16 != 'A' && c16 != 'a')
				{
					break;
				}
				char c17 = buffer[startIndex++];
				if (c17 == 'T' || c17 == 't')
				{
					char c18 = buffer[startIndex++];
					if (c18 == 'E' || c18 == 'e')
					{
						result = XmlToken.UserDefinedAggregate;
					}
				}
				break;
			}
			case 'D':
			case 'd':
			{
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
				if (c15 != 'Y' && c15 != 'y')
				{
					break;
				}
				char c16 = buffer[startIndex++];
				if (c16 != 'P' && c16 != 'p')
				{
					break;
				}
				char c17 = buffer[startIndex++];
				if (c17 == 'E' || c17 == 'e')
				{
					char c18 = buffer[startIndex++];
					if (c18 == 'S' || c18 == 's')
					{
						result = XmlToken.UserDefinedDataTypes;
					}
				}
				break;
			}
			case 'T':
			case 't':
			{
				char c11 = buffer[startIndex++];
				if (c11 != 'A' && c11 != 'a')
				{
					break;
				}
				char c12 = buffer[startIndex++];
				if (c12 != 'B' && c12 != 'b')
				{
					break;
				}
				char c13 = buffer[startIndex++];
				if (c13 != 'L' && c13 != 'l')
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
				if (c16 != 'Y' && c16 != 'y')
				{
					break;
				}
				char c17 = buffer[startIndex++];
				if (c17 == 'P' || c17 == 'p')
				{
					char c18 = buffer[startIndex++];
					if (c18 == 'E' || c18 == 'e')
					{
						result = XmlToken.UserDefinedTableType;
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

	private static XmlToken Find_Len21(string buffer, int startIndex)
	{
		XmlToken result = XmlToken.None;
		switch (buffer[startIndex++])
		{
		case 'D':
		case 'd':
		{
			char c20 = buffer[startIndex++];
			if (c20 != 'E' && c20 != 'e')
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
			if (c4 != 'E' && c4 != 'e')
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
			if (c9 != 'V' && c9 != 'v')
			{
				break;
			}
			char c19 = buffer[startIndex++];
			if (c19 != 'A' && c19 != 'a')
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
			if (c13 != 'N' && c13 != 'n')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 != 'O' && c14 != 'o')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if (c15 != 'R' && c15 != 'r')
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
				if (c18 == 'R' || c18 == 'r')
				{
					result = XmlToken.DeleteActivationOrder;
				}
			}
			break;
		}
		case 'I':
		case 'i':
		{
			char c20 = buffer[startIndex++];
			if (c20 != 'N' && c20 != 'n')
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
			if (c3 != 'R' && c3 != 'r')
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
			if (c9 != 'V' && c9 != 'v')
			{
				break;
			}
			char c19 = buffer[startIndex++];
			if (c19 != 'A' && c19 != 'a')
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
			if (c13 != 'N' && c13 != 'n')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 != 'O' && c14 != 'o')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if (c15 != 'R' && c15 != 'r')
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
				if (c18 == 'R' || c18 == 'r')
				{
					result = XmlToken.InsertActivationOrder;
				}
			}
			break;
		}
		case 'S':
		case 's':
		{
			char c20 = buffer[startIndex++];
			if (c20 != 'C' && c20 != 'c')
			{
				break;
			}
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
			if (c5 != 'V' && c5 != 'v')
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
			if (c8 != 'U' && c8 != 'u')
			{
				break;
			}
			char c9 = buffer[startIndex++];
			if (c9 != 'E' && c9 != 'e')
			{
				break;
			}
			char c19 = buffer[startIndex++];
			if (c19 != 'D' && c19 != 'd')
			{
				break;
			}
			char c10 = buffer[startIndex++];
			if (c10 != 'F' && c10 != 'f')
			{
				break;
			}
			char c11 = buffer[startIndex++];
			if (c11 != 'U' && c11 != 'u')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 != 'N' && c12 != 'n')
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
					result = XmlToken.ScalarValuedFunctions;
				}
			}
			break;
		}
		case 'U':
		case 'u':
			switch (buffer[startIndex++])
			{
			case 'P':
			case 'p':
			{
				char c = buffer[startIndex++];
				if (c != 'D' && c != 'd')
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
				if (c4 != 'E' && c4 != 'e')
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
				if (c9 != 'V' && c9 != 'v')
				{
					break;
				}
				char c19 = buffer[startIndex++];
				if (c19 != 'A' && c19 != 'a')
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
				if (c13 != 'N' && c13 != 'n')
				{
					break;
				}
				char c14 = buffer[startIndex++];
				if (c14 != 'O' && c14 != 'o')
				{
					break;
				}
				char c15 = buffer[startIndex++];
				if (c15 != 'R' && c15 != 'r')
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
					if (c18 == 'R' || c18 == 'r')
					{
						result = XmlToken.UpdateActivationOrder;
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
				if (c7 != 'N' && c7 != 'n')
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
				switch (buffer[startIndex++])
				{
				case 'A':
				case 'a':
				{
					char c10 = buffer[startIndex++];
					if (c10 != 'G' && c10 != 'g')
					{
						break;
					}
					char c11 = buffer[startIndex++];
					if (c11 != 'G' && c11 != 'g')
					{
						break;
					}
					char c12 = buffer[startIndex++];
					if (c12 != 'R' && c12 != 'r')
					{
						break;
					}
					char c13 = buffer[startIndex++];
					if (c13 != 'E' && c13 != 'e')
					{
						break;
					}
					char c14 = buffer[startIndex++];
					if (c14 != 'G' && c14 != 'g')
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
					if (c17 == 'E' || c17 == 'e')
					{
						char c18 = buffer[startIndex++];
						if (c18 == 'S' || c18 == 's')
						{
							result = XmlToken.UserDefinedAggregates;
						}
					}
					break;
				}
				case 'T':
				case 't':
				{
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
					if (c12 != 'L' && c12 != 'l')
					{
						break;
					}
					char c13 = buffer[startIndex++];
					if (c13 != 'E' && c13 != 'e')
					{
						break;
					}
					char c14 = buffer[startIndex++];
					if (c14 != 'T' && c14 != 't')
					{
						break;
					}
					char c15 = buffer[startIndex++];
					if (c15 != 'Y' && c15 != 'y')
					{
						break;
					}
					char c16 = buffer[startIndex++];
					if (c16 != 'P' && c16 != 'p')
					{
						break;
					}
					char c17 = buffer[startIndex++];
					if (c17 == 'E' || c17 == 'e')
					{
						char c18 = buffer[startIndex++];
						if (c18 == 'S' || c18 == 's')
						{
							result = XmlToken.UserDefinedTableTypes;
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

	private static XmlToken Find_Len22(string buffer, int startIndex)
	{
		XmlToken result = XmlToken.None;
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
			if (c6 != 'U' && c6 != 'u')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'M' && c7 != 'm')
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
			if (c10 != 'P' && c10 != 'p')
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
			if (c13 != 'I' && c13 != 'i')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 != 'F' && c14 != 'f')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if (c15 != 'I' && c15 != 'i')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if (c16 != 'C' && c16 != 'c')
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
					result = XmlToken.HasColumnSpecification;
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
			if (c2 != 'T' && c2 != 't')
			{
				break;
			}
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
			if (c9 != 'L' && c9 != 'l')
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
			if (c12 != 'N' && c12 != 'n')
			{
				break;
			}
			char c13 = buffer[startIndex++];
			if (c13 != 'N' && c13 != 'n')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 != 'U' && c14 != 'u')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if (c15 != 'L' && c15 != 'l')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if (c16 != 'L' && c16 != 'l')
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
			if (c19 != 'P' && c19 != 'p')
			{
				break;
			}
			char c20 = buffer[startIndex++];
			if (c20 == 'U' || c20 == 'u')
			{
				char c21 = buffer[startIndex++];
				if (c21 == 'T' || c21 == 't')
				{
					result = XmlToken.ReturnsNullOnNullInput;
				}
			}
			break;
		}
		}
		return result;
	}

	private static XmlToken Find_Len23(string buffer, int startIndex)
	{
		XmlToken result = XmlToken.None;
		char c = buffer[startIndex++];
		if (c == 'E' || c == 'e')
		{
			char c2 = buffer[startIndex++];
			if (c2 == 'X' || c2 == 'x')
			{
				char c3 = buffer[startIndex++];
				if (c3 == 'T' || c3 == 't')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'E' || c4 == 'e')
					{
						char c5 = buffer[startIndex++];
						if (c5 == 'N' || c5 == 'n')
						{
							char c6 = buffer[startIndex++];
							if (c6 == 'D' || c6 == 'd')
							{
								char c7 = buffer[startIndex++];
								if (c7 == 'E' || c7 == 'e')
								{
									char c8 = buffer[startIndex++];
									if (c8 == 'D' || c8 == 'd')
									{
										char c9 = buffer[startIndex++];
										if (c9 == 'S' || c9 == 's')
										{
											char c10 = buffer[startIndex++];
											if (c10 == 'T' || c10 == 't')
											{
												char c11 = buffer[startIndex++];
												if (c11 == 'O' || c11 == 'o')
												{
													char c12 = buffer[startIndex++];
													if (c12 == 'R' || c12 == 'r')
													{
														char c13 = buffer[startIndex++];
														if (c13 == 'E' || c13 == 'e')
														{
															char c14 = buffer[startIndex++];
															if (c14 == 'D' || c14 == 'd')
															{
																char c15 = buffer[startIndex++];
																if (c15 == 'P' || c15 == 'p')
																{
																	char c16 = buffer[startIndex++];
																	if (c16 == 'R' || c16 == 'r')
																	{
																		char c17 = buffer[startIndex++];
																		if (c17 == 'O' || c17 == 'o')
																		{
																			char c18 = buffer[startIndex++];
																			if (c18 == 'C' || c18 == 'c')
																			{
																				char c19 = buffer[startIndex++];
																				if (c19 == 'E' || c19 == 'e')
																				{
																					char c20 = buffer[startIndex++];
																					if (c20 == 'D' || c20 == 'd')
																					{
																						char c21 = buffer[startIndex++];
																						if (c21 == 'U' || c21 == 'u')
																						{
																							char c22 = buffer[startIndex++];
																							if (c22 == 'R' || c22 == 'r')
																							{
																								char c23 = buffer[startIndex++];
																								if (c23 == 'E' || c23 == 'e')
																								{
																									result = XmlToken.ExtendedStoredProcedure;
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

	private static XmlToken Find_Len24(string buffer, int startIndex)
	{
		XmlToken result = XmlToken.None;
		switch (buffer[startIndex++])
		{
		case 'E':
		case 'e':
		{
			char c = buffer[startIndex++];
			if (c != 'X' && c != 'x')
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
			if (c6 != 'E' && c6 != 'e')
			{
				break;
			}
			char c7 = buffer[startIndex++];
			if (c7 != 'D' && c7 != 'd')
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
			if (c10 != 'O' && c10 != 'o')
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
			if (c13 != 'D' && c13 != 'd')
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
			if (c21 != 'R' && c21 != 'r')
			{
				break;
			}
			char c22 = buffer[startIndex++];
			if (c22 == 'E' || c22 == 'e')
			{
				char c23 = buffer[startIndex++];
				if (c23 == 'S' || c23 == 's')
				{
					result = XmlToken.ExtendedStoredProcedures;
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
			if (c2 != 'A' && c2 != 'a')
			{
				break;
			}
			char c3 = buffer[startIndex++];
			if (c3 != 'U' && c3 != 'u')
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
			if (c15 != 'M' && c15 != 'm')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if (c16 != 'P' && c16 != 'p')
			{
				break;
			}
			char c17 = buffer[startIndex++];
			if (c17 != 'U' && c17 != 'u')
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
					result = XmlToken.NoAutomaticRecomputation;
				}
			}
			break;
		}
		}
		return result;
	}

	private static XmlToken Find_Len25(string buffer, int startIndex)
	{
		XmlToken result = XmlToken.None;
		switch (buffer[startIndex++])
		{
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
			if (c11 != 'A' && c11 != 'a')
			{
				break;
			}
			char c12 = buffer[startIndex++];
			if (c12 != 'R' && c12 != 'r')
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
			if (c17 != 'O' && c17 != 'o')
			{
				break;
			}
			char c18 = buffer[startIndex++];
			if (c18 != 'N' && c18 != 'n')
			{
				break;
			}
			char c19 = buffer[startIndex++];
			if (c19 != 'S' && c19 != 's')
			{
				break;
			}
			char c20 = buffer[startIndex++];
			if (c20 != 'C' && c20 != 'c')
			{
				break;
			}
			char c21 = buffer[startIndex++];
			if (c21 != 'H' && c21 != 'h')
			{
				break;
			}
			char c22 = buffer[startIndex++];
			if (c22 != 'E' && c22 != 'e')
			{
				break;
			}
			char c23 = buffer[startIndex++];
			if (c23 == 'M' || c23 == 'm')
			{
				char c24 = buffer[startIndex++];
				if (c24 == 'E' || c24 == 'e')
				{
					result = XmlToken.FileStreamPartitionScheme;
				}
			}
			break;
		}
		case 'I':
		case 'i':
		{
			char c = buffer[startIndex++];
			if (c != 'S' && c != 's')
			{
				break;
			}
			char c2 = buffer[startIndex++];
			if (c2 != 'G' && c2 != 'g')
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
			if (c10 != 'D' && c10 != 'd')
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
			if (c13 != 'W' && c13 != 'w')
			{
				break;
			}
			char c14 = buffer[startIndex++];
			if (c14 != 'A' && c14 != 'a')
			{
				break;
			}
			char c15 = buffer[startIndex++];
			if (c15 != 'Y' && c15 != 'y')
			{
				break;
			}
			char c16 = buffer[startIndex++];
			if (c16 != 'S' && c16 != 's')
			{
				break;
			}
			char c17 = buffer[startIndex++];
			if (c17 != 'A' && c17 != 'a')
			{
				break;
			}
			char c18 = buffer[startIndex++];
			if (c18 != 'S' && c18 != 's')
			{
				break;
			}
			char c19 = buffer[startIndex++];
			if (c19 != 'R' && c19 != 'r')
			{
				break;
			}
			char c20 = buffer[startIndex++];
			if (c20 != 'O' && c20 != 'o')
			{
				break;
			}
			char c21 = buffer[startIndex++];
			if (c21 != 'W' && c21 != 'w')
			{
				break;
			}
			char c22 = buffer[startIndex++];
			if (c22 != 'E' && c22 != 'e')
			{
				break;
			}
			char c23 = buffer[startIndex++];
			if (c23 == 'N' || c23 == 'n')
			{
				char c24 = buffer[startIndex++];
				if (c24 == 'D' || c24 == 'd')
				{
					result = XmlToken.IsGeneratedAlwaysAsRowEnd;
				}
			}
			break;
		}
		}
		return result;
	}

	private static XmlToken Find_Len27(string buffer, int startIndex)
	{
		XmlToken result = XmlToken.None;
		char c = buffer[startIndex++];
		if (c == 'I' || c == 'i')
		{
			char c2 = buffer[startIndex++];
			if (c2 == 'S' || c2 == 's')
			{
				char c3 = buffer[startIndex++];
				if (c3 == 'G' || c3 == 'g')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'E' || c4 == 'e')
					{
						char c5 = buffer[startIndex++];
						if (c5 == 'N' || c5 == 'n')
						{
							char c6 = buffer[startIndex++];
							if (c6 == 'E' || c6 == 'e')
							{
								char c7 = buffer[startIndex++];
								if (c7 == 'R' || c7 == 'r')
								{
									char c8 = buffer[startIndex++];
									if (c8 == 'A' || c8 == 'a')
									{
										char c9 = buffer[startIndex++];
										if (c9 == 'T' || c9 == 't')
										{
											char c10 = buffer[startIndex++];
											if (c10 == 'E' || c10 == 'e')
											{
												char c11 = buffer[startIndex++];
												if (c11 == 'D' || c11 == 'd')
												{
													char c12 = buffer[startIndex++];
													if (c12 == 'A' || c12 == 'a')
													{
														char c13 = buffer[startIndex++];
														if (c13 == 'L' || c13 == 'l')
														{
															char c14 = buffer[startIndex++];
															if (c14 == 'W' || c14 == 'w')
															{
																char c15 = buffer[startIndex++];
																if (c15 == 'A' || c15 == 'a')
																{
																	char c16 = buffer[startIndex++];
																	if (c16 == 'Y' || c16 == 'y')
																	{
																		char c17 = buffer[startIndex++];
																		if (c17 == 'S' || c17 == 's')
																		{
																			char c18 = buffer[startIndex++];
																			if (c18 == 'A' || c18 == 'a')
																			{
																				char c19 = buffer[startIndex++];
																				if (c19 == 'S' || c19 == 's')
																				{
																					char c20 = buffer[startIndex++];
																					if (c20 == 'R' || c20 == 'r')
																					{
																						char c21 = buffer[startIndex++];
																						if (c21 == 'O' || c21 == 'o')
																						{
																							char c22 = buffer[startIndex++];
																							if (c22 == 'W' || c22 == 'w')
																							{
																								char c23 = buffer[startIndex++];
																								if (c23 == 'S' || c23 == 's')
																								{
																									char c24 = buffer[startIndex++];
																									if (c24 == 'T' || c24 == 't')
																									{
																										char c25 = buffer[startIndex++];
																										if (c25 == 'A' || c25 == 'a')
																										{
																											char c26 = buffer[startIndex++];
																											if (c26 == 'R' || c26 == 'r')
																											{
																												char c27 = buffer[startIndex++];
																												if (c27 == 'T' || c27 == 't')
																												{
																													result = XmlToken.IsGeneratedAlwaysAsRowStart;
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

	private static XmlToken Find_Len35(string buffer, int startIndex)
	{
		XmlToken result = XmlToken.None;
		char c = buffer[startIndex++];
		if (c == 'I' || c == 'i')
		{
			char c2 = buffer[startIndex++];
			if (c2 == 'S' || c2 == 's')
			{
				char c3 = buffer[startIndex++];
				if (c3 == 'G' || c3 == 'g')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'E' || c4 == 'e')
					{
						char c5 = buffer[startIndex++];
						if (c5 == 'N' || c5 == 'n')
						{
							char c6 = buffer[startIndex++];
							if (c6 == 'E' || c6 == 'e')
							{
								char c7 = buffer[startIndex++];
								if (c7 == 'R' || c7 == 'r')
								{
									char c8 = buffer[startIndex++];
									if (c8 == 'A' || c8 == 'a')
									{
										char c9 = buffer[startIndex++];
										if (c9 == 'T' || c9 == 't')
										{
											char c10 = buffer[startIndex++];
											if (c10 == 'E' || c10 == 'e')
											{
												char c11 = buffer[startIndex++];
												if (c11 == 'D' || c11 == 'd')
												{
													char c12 = buffer[startIndex++];
													if (c12 == 'A' || c12 == 'a')
													{
														char c13 = buffer[startIndex++];
														if (c13 == 'L' || c13 == 'l')
														{
															char c14 = buffer[startIndex++];
															if (c14 == 'W' || c14 == 'w')
															{
																char c15 = buffer[startIndex++];
																if (c15 == 'A' || c15 == 'a')
																{
																	char c16 = buffer[startIndex++];
																	if (c16 == 'Y' || c16 == 'y')
																	{
																		char c17 = buffer[startIndex++];
																		if (c17 == 'S' || c17 == 's')
																		{
																			char c18 = buffer[startIndex++];
																			if (c18 == 'A' || c18 == 'a')
																			{
																				char c19 = buffer[startIndex++];
																				if (c19 == 'S' || c19 == 's')
																				{
																					char c20 = buffer[startIndex++];
																					if (c20 == 'T' || c20 == 't')
																					{
																						char c21 = buffer[startIndex++];
																						if (c21 == 'R' || c21 == 'r')
																						{
																							char c22 = buffer[startIndex++];
																							if (c22 == 'A' || c22 == 'a')
																							{
																								char c23 = buffer[startIndex++];
																								if (c23 == 'N' || c23 == 'n')
																								{
																									char c24 = buffer[startIndex++];
																									if (c24 == 'S' || c24 == 's')
																									{
																										char c25 = buffer[startIndex++];
																										if (c25 == 'A' || c25 == 'a')
																										{
																											char c26 = buffer[startIndex++];
																											if (c26 == 'C' || c26 == 'c')
																											{
																												char c27 = buffer[startIndex++];
																												if (c27 == 'T' || c27 == 't')
																												{
																													char c28 = buffer[startIndex++];
																													if (c28 == 'I' || c28 == 'i')
																													{
																														char c29 = buffer[startIndex++];
																														if (c29 == 'O' || c29 == 'o')
																														{
																															char c30 = buffer[startIndex++];
																															if (c30 == 'N' || c30 == 'n')
																															{
																																char c31 = buffer[startIndex++];
																																if (c31 == 'I' || c31 == 'i')
																																{
																																	char c32 = buffer[startIndex++];
																																	if (c32 == 'D' || c32 == 'd')
																																	{
																																		char c33 = buffer[startIndex++];
																																		if (c33 == 'E' || c33 == 'e')
																																		{
																																			char c34 = buffer[startIndex++];
																																			if (c34 == 'N' || c34 == 'n')
																																			{
																																				char c35 = buffer[startIndex++];
																																				if (c35 == 'D' || c35 == 'd')
																																				{
																																					result = XmlToken.IsGeneratedAlwaysAsTransactionIdEnd;
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
			}
		}
		return result;
	}

	private static XmlToken Find_Len36(string buffer, int startIndex)
	{
		XmlToken result = XmlToken.None;
		char c = buffer[startIndex++];
		if (c == 'I' || c == 'i')
		{
			char c2 = buffer[startIndex++];
			if (c2 == 'S' || c2 == 's')
			{
				char c3 = buffer[startIndex++];
				if (c3 == 'G' || c3 == 'g')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'E' || c4 == 'e')
					{
						char c5 = buffer[startIndex++];
						if (c5 == 'N' || c5 == 'n')
						{
							char c6 = buffer[startIndex++];
							if (c6 == 'E' || c6 == 'e')
							{
								char c7 = buffer[startIndex++];
								if (c7 == 'R' || c7 == 'r')
								{
									char c8 = buffer[startIndex++];
									if (c8 == 'A' || c8 == 'a')
									{
										char c9 = buffer[startIndex++];
										if (c9 == 'T' || c9 == 't')
										{
											char c10 = buffer[startIndex++];
											if (c10 == 'E' || c10 == 'e')
											{
												char c11 = buffer[startIndex++];
												if (c11 == 'D' || c11 == 'd')
												{
													char c12 = buffer[startIndex++];
													if (c12 == 'A' || c12 == 'a')
													{
														char c13 = buffer[startIndex++];
														if (c13 == 'L' || c13 == 'l')
														{
															char c14 = buffer[startIndex++];
															if (c14 == 'W' || c14 == 'w')
															{
																char c15 = buffer[startIndex++];
																if (c15 == 'A' || c15 == 'a')
																{
																	char c16 = buffer[startIndex++];
																	if (c16 == 'Y' || c16 == 'y')
																	{
																		char c17 = buffer[startIndex++];
																		if (c17 == 'S' || c17 == 's')
																		{
																			char c18 = buffer[startIndex++];
																			if (c18 == 'A' || c18 == 'a')
																			{
																				char c19 = buffer[startIndex++];
																				if (c19 == 'S' || c19 == 's')
																				{
																					char c20 = buffer[startIndex++];
																					if (c20 == 'S' || c20 == 's')
																					{
																						char c21 = buffer[startIndex++];
																						if (c21 == 'E' || c21 == 'e')
																						{
																							char c22 = buffer[startIndex++];
																							if (c22 == 'Q' || c22 == 'q')
																							{
																								char c23 = buffer[startIndex++];
																								if (c23 == 'U' || c23 == 'u')
																								{
																									char c24 = buffer[startIndex++];
																									if (c24 == 'E' || c24 == 'e')
																									{
																										char c25 = buffer[startIndex++];
																										if (c25 == 'N' || c25 == 'n')
																										{
																											char c26 = buffer[startIndex++];
																											if (c26 == 'C' || c26 == 'c')
																											{
																												char c27 = buffer[startIndex++];
																												if (c27 == 'E' || c27 == 'e')
																												{
																													char c28 = buffer[startIndex++];
																													if (c28 == 'N' || c28 == 'n')
																													{
																														char c29 = buffer[startIndex++];
																														if (c29 == 'U' || c29 == 'u')
																														{
																															char c30 = buffer[startIndex++];
																															if (c30 == 'M' || c30 == 'm')
																															{
																																char c31 = buffer[startIndex++];
																																if (c31 == 'B' || c31 == 'b')
																																{
																																	char c32 = buffer[startIndex++];
																																	if (c32 == 'E' || c32 == 'e')
																																	{
																																		char c33 = buffer[startIndex++];
																																		if (c33 == 'R' || c33 == 'r')
																																		{
																																			char c34 = buffer[startIndex++];
																																			if (c34 == 'E' || c34 == 'e')
																																			{
																																				char c35 = buffer[startIndex++];
																																				if (c35 == 'N' || c35 == 'n')
																																				{
																																					char c36 = buffer[startIndex++];
																																					if (c36 == 'D' || c36 == 'd')
																																					{
																																						result = XmlToken.IsGeneratedAlwaysAsSequenceNumberEnd;
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
				}
			}
		}
		return result;
	}

	private static XmlToken Find_Len37(string buffer, int startIndex)
	{
		XmlToken result = XmlToken.None;
		char c = buffer[startIndex++];
		if (c == 'I' || c == 'i')
		{
			char c2 = buffer[startIndex++];
			if (c2 == 'S' || c2 == 's')
			{
				char c3 = buffer[startIndex++];
				if (c3 == 'G' || c3 == 'g')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'E' || c4 == 'e')
					{
						char c5 = buffer[startIndex++];
						if (c5 == 'N' || c5 == 'n')
						{
							char c6 = buffer[startIndex++];
							if (c6 == 'E' || c6 == 'e')
							{
								char c7 = buffer[startIndex++];
								if (c7 == 'R' || c7 == 'r')
								{
									char c8 = buffer[startIndex++];
									if (c8 == 'A' || c8 == 'a')
									{
										char c9 = buffer[startIndex++];
										if (c9 == 'T' || c9 == 't')
										{
											char c10 = buffer[startIndex++];
											if (c10 == 'E' || c10 == 'e')
											{
												char c11 = buffer[startIndex++];
												if (c11 == 'D' || c11 == 'd')
												{
													char c12 = buffer[startIndex++];
													if (c12 == 'A' || c12 == 'a')
													{
														char c13 = buffer[startIndex++];
														if (c13 == 'L' || c13 == 'l')
														{
															char c14 = buffer[startIndex++];
															if (c14 == 'W' || c14 == 'w')
															{
																char c15 = buffer[startIndex++];
																if (c15 == 'A' || c15 == 'a')
																{
																	char c16 = buffer[startIndex++];
																	if (c16 == 'Y' || c16 == 'y')
																	{
																		char c17 = buffer[startIndex++];
																		if (c17 == 'S' || c17 == 's')
																		{
																			char c18 = buffer[startIndex++];
																			if (c18 == 'A' || c18 == 'a')
																			{
																				char c19 = buffer[startIndex++];
																				if (c19 == 'S' || c19 == 's')
																				{
																					char c20 = buffer[startIndex++];
																					if (c20 == 'T' || c20 == 't')
																					{
																						char c21 = buffer[startIndex++];
																						if (c21 == 'R' || c21 == 'r')
																						{
																							char c22 = buffer[startIndex++];
																							if (c22 == 'A' || c22 == 'a')
																							{
																								char c23 = buffer[startIndex++];
																								if (c23 == 'N' || c23 == 'n')
																								{
																									char c24 = buffer[startIndex++];
																									if (c24 == 'S' || c24 == 's')
																									{
																										char c25 = buffer[startIndex++];
																										if (c25 == 'A' || c25 == 'a')
																										{
																											char c26 = buffer[startIndex++];
																											if (c26 == 'C' || c26 == 'c')
																											{
																												char c27 = buffer[startIndex++];
																												if (c27 == 'T' || c27 == 't')
																												{
																													char c28 = buffer[startIndex++];
																													if (c28 == 'I' || c28 == 'i')
																													{
																														char c29 = buffer[startIndex++];
																														if (c29 == 'O' || c29 == 'o')
																														{
																															char c30 = buffer[startIndex++];
																															if (c30 == 'N' || c30 == 'n')
																															{
																																char c31 = buffer[startIndex++];
																																if (c31 == 'I' || c31 == 'i')
																																{
																																	char c32 = buffer[startIndex++];
																																	if (c32 == 'D' || c32 == 'd')
																																	{
																																		char c33 = buffer[startIndex++];
																																		if (c33 == 'S' || c33 == 's')
																																		{
																																			char c34 = buffer[startIndex++];
																																			if (c34 == 'T' || c34 == 't')
																																			{
																																				char c35 = buffer[startIndex++];
																																				if (c35 == 'A' || c35 == 'a')
																																				{
																																					char c36 = buffer[startIndex++];
																																					if (c36 == 'R' || c36 == 'r')
																																					{
																																						char c37 = buffer[startIndex++];
																																						if (c37 == 'T' || c37 == 't')
																																						{
																																							result = XmlToken.IsGeneratedAlwaysAsTransactionIdStart;
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
					}
				}
			}
		}
		return result;
	}

	private static XmlToken Find_Len38(string buffer, int startIndex)
	{
		XmlToken result = XmlToken.None;
		char c = buffer[startIndex++];
		if (c == 'I' || c == 'i')
		{
			char c2 = buffer[startIndex++];
			if (c2 == 'S' || c2 == 's')
			{
				char c3 = buffer[startIndex++];
				if (c3 == 'G' || c3 == 'g')
				{
					char c4 = buffer[startIndex++];
					if (c4 == 'E' || c4 == 'e')
					{
						char c5 = buffer[startIndex++];
						if (c5 == 'N' || c5 == 'n')
						{
							char c6 = buffer[startIndex++];
							if (c6 == 'E' || c6 == 'e')
							{
								char c7 = buffer[startIndex++];
								if (c7 == 'R' || c7 == 'r')
								{
									char c8 = buffer[startIndex++];
									if (c8 == 'A' || c8 == 'a')
									{
										char c9 = buffer[startIndex++];
										if (c9 == 'T' || c9 == 't')
										{
											char c10 = buffer[startIndex++];
											if (c10 == 'E' || c10 == 'e')
											{
												char c11 = buffer[startIndex++];
												if (c11 == 'D' || c11 == 'd')
												{
													char c12 = buffer[startIndex++];
													if (c12 == 'A' || c12 == 'a')
													{
														char c13 = buffer[startIndex++];
														if (c13 == 'L' || c13 == 'l')
														{
															char c14 = buffer[startIndex++];
															if (c14 == 'W' || c14 == 'w')
															{
																char c15 = buffer[startIndex++];
																if (c15 == 'A' || c15 == 'a')
																{
																	char c16 = buffer[startIndex++];
																	if (c16 == 'Y' || c16 == 'y')
																	{
																		char c17 = buffer[startIndex++];
																		if (c17 == 'S' || c17 == 's')
																		{
																			char c18 = buffer[startIndex++];
																			if (c18 == 'A' || c18 == 'a')
																			{
																				char c19 = buffer[startIndex++];
																				if (c19 == 'S' || c19 == 's')
																				{
																					char c20 = buffer[startIndex++];
																					if (c20 == 'S' || c20 == 's')
																					{
																						char c21 = buffer[startIndex++];
																						if (c21 == 'E' || c21 == 'e')
																						{
																							char c22 = buffer[startIndex++];
																							if (c22 == 'Q' || c22 == 'q')
																							{
																								char c23 = buffer[startIndex++];
																								if (c23 == 'U' || c23 == 'u')
																								{
																									char c24 = buffer[startIndex++];
																									if (c24 == 'E' || c24 == 'e')
																									{
																										char c25 = buffer[startIndex++];
																										if (c25 == 'N' || c25 == 'n')
																										{
																											char c26 = buffer[startIndex++];
																											if (c26 == 'C' || c26 == 'c')
																											{
																												char c27 = buffer[startIndex++];
																												if (c27 == 'E' || c27 == 'e')
																												{
																													char c28 = buffer[startIndex++];
																													if (c28 == 'N' || c28 == 'n')
																													{
																														char c29 = buffer[startIndex++];
																														if (c29 == 'U' || c29 == 'u')
																														{
																															char c30 = buffer[startIndex++];
																															if (c30 == 'M' || c30 == 'm')
																															{
																																char c31 = buffer[startIndex++];
																																if (c31 == 'B' || c31 == 'b')
																																{
																																	char c32 = buffer[startIndex++];
																																	if (c32 == 'E' || c32 == 'e')
																																	{
																																		char c33 = buffer[startIndex++];
																																		if (c33 == 'R' || c33 == 'r')
																																		{
																																			char c34 = buffer[startIndex++];
																																			if (c34 == 'S' || c34 == 's')
																																			{
																																				char c35 = buffer[startIndex++];
																																				if (c35 == 'T' || c35 == 't')
																																				{
																																					char c36 = buffer[startIndex++];
																																					if (c36 == 'A' || c36 == 'a')
																																					{
																																						char c37 = buffer[startIndex++];
																																						if (c37 == 'R' || c37 == 'r')
																																						{
																																							char c38 = buffer[startIndex++];
																																							if (c38 == 'T' || c38 == 't')
																																							{
																																								result = XmlToken.IsGeneratedAlwaysAsSequenceNumberStart;
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
						}
					}
				}
			}
		}
		return result;
	}
}
