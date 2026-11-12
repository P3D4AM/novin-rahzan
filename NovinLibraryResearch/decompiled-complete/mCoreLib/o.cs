using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using mCore;

internal class o
{
	private int m_a;

	private string m_b;

	private f m_c;

	private Collection d;

	internal o(ref SMS A_0)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		this.m_b = "";
		d = new Collection();
		this.m_c = A_0.gm;
	}

	[SpecialName]
	internal int b()
	{
		return this.m_a;
	}

	[SpecialName]
	internal void b(int A_0)
	{
		this.m_a = A_0;
	}

	[SpecialName]
	internal string a()
	{
		return this.m_b;
	}

	[SpecialName]
	internal void a(string A_0)
	{
		string text = this.m_b;
		Monitor.TryEnter(d, this.m_c.a4);
		try
		{
			if (A_0.EndsWith("\\"))
			{
				this.m_b = A_0;
			}
			else
			{
				this.m_b = A_0 + "\\";
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			this.m_b = text;
			this.m_c.a(9007L, "Error setting log folder path " + ex2.Message, "GENERAL");
			ProjectData.ClearProjectError();
		}
		finally
		{
			Monitor.Exit(d);
		}
	}

	internal long c()
	{
		Monitor.TryEnter(d, this.m_c.a4);
		long result = default(long);
		try
		{
			result = checked(File.ReadAllLines(this.m_b + "mCoreLib.log").GetUpperBound(0) + 1);
			return result;
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			this.m_c.a(9004L, "Error getting log file size. " + ex2.Message, "GENERAL");
			ProjectData.ClearProjectError();
		}
		finally
		{
			Monitor.Exit(d);
		}
		return result;
	}

	internal void a(long A_0, string A_1)
	{
		string text = "";
		Monitor.TryEnter(d, this.m_c.a4);
		try
		{
			if (this.m_a >= 1 && A_0 != 0 && A_0 != 9005)
			{
				A_1 = A_1.Replace('\r'.ToString(), "<cr>").Replace('\n'.ToString(), "<lf>");
				text = DateTime.Now.ToString() + ": ERROR [" + A_0 + "] " + A_1 + "\r\n";
				File.AppendAllText(this.m_b + "mCoreLib.log", text);
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			this.m_a = 0;
			this.m_c.a(9005L, "Error opening log file. " + ex2.Message, "GENERAL");
			ProjectData.ClearProjectError();
		}
		finally
		{
			Monitor.Exit(d);
		}
	}

	internal void b(string A_0)
	{
		string text = "";
		Monitor.TryEnter(d, this.m_c.a4);
		try
		{
			if (this.m_a >= 2 && A_0.Trim().Length != 0)
			{
				A_0 = A_0.Replace('\r'.ToString(), "<cr>").Replace('\n'.ToString(), "<lf>");
				text = DateTime.Now.ToString() + ": " + A_0 + "\r\n";
				File.AppendAllText(this.m_b + "mCoreLib.log", text);
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			this.m_a = 0;
			this.m_c.a(9005L, "Error opening log file. " + ex2.Message, "GENERAL");
			ProjectData.ClearProjectError();
		}
		finally
		{
			Monitor.Exit(d);
		}
	}

	internal bool a(int A_0)
	{
		string text = "";
		string contents = "";
		Monitor.TryEnter(d, this.m_c.a4);
		bool result = false;
		text = this.m_b + "mCoreLib.log";
		checked
		{
			try
			{
				if (File.Exists(text))
				{
					if (A_0 == 0)
					{
						File.Delete(text);
						File.AppendAllText(text, "");
					}
					else
					{
						string[] array = File.ReadAllLines(text);
						if (array.GetUpperBound(0) >= 0)
						{
							File.Delete(text);
							int upperBound = array.GetUpperBound(0);
							for (int num = 0; num <= upperBound; num++)
							{
								if (num >= array.GetUpperBound(0) - A_0)
								{
									contents = array[num] + "\r\n";
								}
							}
							File.AppendAllText(text, contents);
						}
					}
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				this.m_c.a(9006L, "Error clearing log file. " + ex2.Message, "GENERAL");
				ProjectData.ClearProjectError();
			}
			finally
			{
				Monitor.Exit(d);
			}
			return result;
		}
	}
}
