//--------------------------------------------------------------------------
// File:    Examples_CoreMin_Security_LsaAccountManager.cs
// Content:	Implementation of class ExamplesCoreMinSecurityLsaAccountManager
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Security.Principal;
using System.DirectoryServices.AccountManagement;
using System.ComponentModel;
using SmartExpert.CUI;
using SmartExpert.Error;
using SmartExpert.IO;
using SmartExpert.Security;
using SmartExpert.Security.Authentication;
using SmartExpert.Security.Identity;
using SmartExpert.Security.LSA;

#endregion

namespace SmartExpert.Examples
{

	///<summary>ExamplesCoreMinSecurityLsaAccountManager</summary>
	public static class ExamplesCoreMinSecurityLsaAccountManager
	{
		[Description("LsaAccountManager examples")]
		public static void RunAll()
		{
			FileHelper.GetFiles(@".\", "Sample_CoreMin_Security_M_Lsa*.txt").ForEach(FileHelper.DeleteFile);
			
			GetAllLocalGroupsExample();
			ConsoleHelper.NewLine(); ConsoleHelper.HR(); ConsoleHelper.NewLine();
			GetAllLocalUsersExample();
			ConsoleHelper.NewLine(); ConsoleHelper.HR(); ConsoleHelper.NewLine();
			GetUserGroupsExample();
			ConsoleHelper.NewLine(); ConsoleHelper.HR(); ConsoleHelper.NewLine();
			GetGetUserAuthorizationGroupsExample();
			ConsoleHelper.NewLine(); ConsoleHelper.HR(); ConsoleHelper.NewLine();
			GetUserExample();
			ConsoleHelper.NewLine(); ConsoleHelper.HR(); ConsoleHelper.NewLine();
			GetGroupExample();
			ConsoleHelper.NewLine(); ConsoleHelper.HR(); ConsoleHelper.NewLine();
		}

		[Description("Get all local security groups")]
		public static void GetAllLocalGroupsExample()
		{
			try
			{
				ConsoleHelper.OutputFileName = "Sample_CoreMin_Security_M_LsaAccountManager_GetAllLocalGroups.txt";

				#region Sample_CoreMin_Security_M_LsaAccountManager_GetAllLocalGroups

				using (var lsa = new LsaAccountManager())
				{
					IEnumerable<GroupPrincipal> localGroups = lsa.GetAllLocalGroups();
					foreach (var group in localGroups)
					{
						ConsoleHelper.WriteNameValue("ConnectedServer", group.Context.ConnectedServer);
						ConsoleHelper.WriteNameValue("Context Type", group.ContextType);
						ConsoleHelper.WriteNameValue("Name", (group.Name.IsNull()) ? "<null>" : group.Name);
						ConsoleHelper.WriteNameValue("SAM Account Name", group.SamAccountName);
						ConsoleHelper.WriteNameValue("SID", group.Sid);
						ConsoleHelper.HR();
					}
				}

				#endregion

			}
			catch (Exception ex)
			{
				ConsoleHelper.WriteLine(ex);
			}
			finally
			{
				ConsoleHelper.OutputFileName = null;
			}
		}

		[Description("Get all local users")]
		public static void GetAllLocalUsersExample()
		{
			try
			{
				ConsoleHelper.OutputFileName = "Sample_CoreMin_Security_M_LsaAccountManager_GetAllLocalUsers.txt";

				#region Sample_CoreMin_Security_M_LsaAccountManager_GetAllLocalUsers

				using (var lsa = new LsaAccountManager())
				{
					IEnumerable<UserPrincipal> localUsers = lsa.GetAllLocalUsers();
					foreach (var user in localUsers)
					{
						ConsoleHelper.WriteNameValue("ConnectedServer", user.Context.ConnectedServer);
						ConsoleHelper.WriteNameValue("Context Type", user.ContextType);
						ConsoleHelper.WriteNameValue("Name", (user.Name.IsNull()) ? "<null>" : user.Name);
						ConsoleHelper.WriteNameValue("SAM Account Name", user.SamAccountName);
						ConsoleHelper.WriteNameValue("SID", user.Sid);
                        ConsoleHelper.WriteNameValue("Guid", (user.Guid.HasValue) ? user.Guid.Value.ToString() : "<null>");
                        ConsoleHelper.WriteNameValue("User Principal Name", (user.UserPrincipalName.IsNull()) ? "<null>" : user.UserPrincipalName);
                        ConsoleHelper.WriteNameValue("Distinguished Name", (user.DistinguishedName.IsNull()) ? "<null>" : user.DistinguishedName);
						ConsoleHelper.HR();
					}
				}

				#endregion
			}
			catch (Exception ex)
			{
				ConsoleHelper.WriteLine(ex);
			}
			finally
			{
				ConsoleHelper.OutputFileName = null;
			}
		}

		[Description("Get user groups")]
		public static void GetUserGroupsExample()
		{
			try
			{
				ConsoleHelper.OutputFileName = "Sample_CoreMin_Security_M_LsaAccountManager_GetUserGroups.txt";

				#region Sample_CoreMin_Security_M_LsaAccountManager_GetUserGroups

				using (var lsa = new LsaAccountManager())
				{
					ConsoleHelper.WriteLineYellow("Get all local groups where user 'Administrator' is member.");
					IEnumerable<GroupPrincipal> userGroups = lsa.GetUserGroups("anbo42");

					foreach (var group in userGroups)
					{
						ConsoleHelper.WriteNameValue("ConnectedServer", group.Context.ConnectedServer);
						ConsoleHelper.WriteNameValue("Context Type", group.ContextType);
						ConsoleHelper.WriteNameValue("Name", (group.Name.IsNull()) ? "<null>" : group.Name);
						ConsoleHelper.WriteNameValue("SAM Account Name", group.SamAccountName);
						ConsoleHelper.WriteNameValue("SID", group.Sid);
						ConsoleHelper.HR();
					}
				}

				#endregion
			}
			catch (Exception ex)
			{
				ConsoleHelper.WriteLine(ex);
			}
			finally
			{
				ConsoleHelper.OutputFileName = null;
			}
		}

		[Description("Get user authorization groups")]
		public static void GetGetUserAuthorizationGroupsExample()
		{
			try
			{
				ConsoleHelper.OutputFileName = "Sample_CoreMin_Security_M_LsaAccountManager_GetUserAuthorizationGroups.txt";

				#region Sample_CoreMin_Security_M_LsaAccountManager_GetUserAuthorizationGroups

				using (var lsa = new LsaAccountManager())
				{
					ConsoleHelper.WriteLineYellow("Get all authorization groups where user 'Administrator' is member.");
					IEnumerable<GroupPrincipal> userGroups = lsa.GetUserAuthorizationGroups("anbo42");

					foreach (var group in userGroups)
					{
						ConsoleHelper.WriteNameValue("ConnectedServer", group.Context.ConnectedServer);
						ConsoleHelper.WriteNameValue("Context Type", group.ContextType);
						ConsoleHelper.WriteNameValue("Name", (group.Name.IsNull()) ? "<null>" : group.Name);
						ConsoleHelper.WriteNameValue("SAM Account Name", group.SamAccountName);
						ConsoleHelper.WriteNameValue("SID", group.Sid);
						ConsoleHelper.HR();
					}
				}

				#endregion
			}
			catch (Exception ex)
			{
				ConsoleHelper.WriteLine(ex);
			}
			finally
			{
				ConsoleHelper.OutputFileName = null;
			}
		}

		[Description("Get user example")]
		public static void GetUserExample()
		{
			try
			{
				ConsoleHelper.OutputFileName = "Sample_CoreMin_Security_M_LsaAccountManager_GetUser.txt";

				#region Sample_CoreMin_Security_M_LsaAccountManager_GetUser

				using (var lsa = new LsaAccountManager())
				{
					UserPrincipal user = lsa.GetUser("anbo42");
					
					ConsoleHelper.WriteNameValue("ConnectedServer", user.Context.ConnectedServer);
					ConsoleHelper.WriteNameValue("Context Type", user.ContextType);
					ConsoleHelper.WriteNameValue("Name", (user.Name.IsNull()) ? "<null>" : user.Name);
					ConsoleHelper.WriteNameValue("SAM Account Name", user.SamAccountName);
					ConsoleHelper.WriteNameValue("SID", user.Sid);
					ConsoleHelper.HR();
				}

				#endregion
			}
			catch (Exception ex)
			{
				ConsoleHelper.WriteLine(ex);
			}
			finally
			{
				ConsoleHelper.OutputFileName = null;
			}
		}

		[Description("Get group example")]
		public static void GetGroupExample()
		{
			try
			{
				ConsoleHelper.OutputFileName = "Sample_CoreMin_Security_M_LsaAccountManager_GetGroup.txt";

				#region Sample_CoreMin_Security_M_LsaAccountManager_GetGroup

				using (var lsa = new LsaAccountManager())
				{
                    GroupPrincipal group = lsa.GetGroup("Administratoren"); // Users

					ConsoleHelper.WriteNameValue("ConnectedServer", group.Context.ConnectedServer);
					ConsoleHelper.WriteNameValue("Context Type", group.ContextType);
					ConsoleHelper.WriteNameValue("Name", (group.Name.IsNull()) ? "<null>" : group.Name);
					ConsoleHelper.WriteNameValue("SAM Account Name", group.SamAccountName);
					ConsoleHelper.WriteNameValue("SID", group.Sid);
					ConsoleHelper.HR();
				}

				#endregion
			}
			catch (Exception ex)
			{
				ConsoleHelper.WriteLine(ex);
			}
			finally
			{
				ConsoleHelper.OutputFileName = null;
			}
		}
	}
}
