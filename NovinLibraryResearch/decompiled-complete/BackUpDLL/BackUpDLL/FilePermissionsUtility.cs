using System;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using PacketParser.Entities;

namespace BackUpDLL;

public class FilePermissionsUtility
{
	private WindowsIdentity _currentUser;

	private WindowsPrincipal _currentPrincipal;

	public FilePermissionsUtility()
	{
		_currentUser = WindowsIdentity.GetCurrent();
		_currentPrincipal = new WindowsPrincipal(_currentUser);
	}

	public bool HasAccess(string path, FileSystemRights right)
	{
		try
		{
			FileInfo fileInfo = new FileInfo(path);
			string text = fileInfo.DirectoryName;
			if (string.IsNullOrEmpty(text))
			{
				text = fileInfo.FullName;
			}
			return HasAccess(new FileInfo(text), right);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "string path, FileSystemRights right", "HasAccess", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\FilePermissionsUtility.cs", 30);
		}
		return false;
	}

	public bool HasAccess(DirectoryInfo directory, FileSystemRights right)
	{
		try
		{
			AuthorizationRuleCollection accessRules = directory.GetAccessControl().GetAccessRules(includeExplicit: true, includeInherited: true, typeof(SecurityIdentifier));
			return HasFileOrDirectoryAccess(right, accessRules);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "DirectoryInfo directory, FileSystemRights right", "HasAccess", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\FilePermissionsUtility.cs", 41);
		}
		return false;
	}

	public bool HasAccess(FileInfo file, FileSystemRights right)
	{
		try
		{
			AuthorizationRuleCollection acl = file.GetAccessControl()?.GetAccessRules(includeExplicit: true, includeInherited: true, typeof(SecurityIdentifier));
			return HasFileOrDirectoryAccess(right, acl);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "FileInfo file, FileSystemRights right", "HasAccess", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\FilePermissionsUtility.cs", 51);
		}
		return false;
	}

	private bool HasFileOrDirectoryAccess(FileSystemRights right, AuthorizationRuleCollection acl)
	{
		try
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			for (int i = 0; i < acl.Count; i++)
			{
				FileSystemAccessRule fileSystemAccessRule = (FileSystemAccessRule)acl[i];
				if (!_currentUser.User.Equals(fileSystemAccessRule.IdentityReference) && !_currentPrincipal.IsInRole((SecurityIdentifier)fileSystemAccessRule.IdentityReference))
				{
					continue;
				}
				if (fileSystemAccessRule.AccessControlType.Equals(AccessControlType.Deny))
				{
					if ((fileSystemAccessRule.FileSystemRights & right) == right)
					{
						if (!fileSystemAccessRule.IsInherited)
						{
							return false;
						}
						flag3 = true;
					}
				}
				else if (fileSystemAccessRule.AccessControlType.Equals(AccessControlType.Allow) && (fileSystemAccessRule.FileSystemRights & right) == right)
				{
					if (fileSystemAccessRule.IsInherited)
					{
						flag2 = true;
					}
					else
					{
						flag = true;
					}
				}
			}
			if (flag)
			{
				return true;
			}
			return flag2 && !flag3;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "HasFileOrDirectoryAccess", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\FilePermissionsUtility.cs", 92);
		}
		return false;
	}
}
