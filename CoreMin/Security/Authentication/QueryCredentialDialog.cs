//--------------------------------------------------------------------------
// File:    QueryCredentialDialog.cs
// Content:	Implementation of class QueryCredentialDialog
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Windows.Forms;
using SmartExpert.Error;
using SmartExpert.Interop;

#endregion

namespace SmartExpert.Security.Authentication
{
	/// <summary>
	/// Implements a common dialog that asks the user to provide a username and password
	/// combination, by calling the CredUIPromptForCredentials() Win32 function.
	/// </summary>
	public class QueryCredentialDialog : CommonDialog
	{
		#region Private Fields
		private string m_Username = "";
		private SecureString m_Password;
		private string m_TargetName = "";

		private Bitmap m_Image;
		private string m_CaptionText = string.Empty;
		private string m_MessageText;
		private bool m_SavePassword;

		private QueryCredentialOptions m_Options = QueryCredentialOptions.GenericCredentials |
			QueryCredentialOptions.ShowSaveCheckBox |
			QueryCredentialOptions.AlwaysShowUI |
			QueryCredentialOptions.ExpectConfirmation;
		#endregion

		#region Public Methods

		/// <summary>
		/// Marks the credentials stored with the current targetname either as valid or as invalid.
		/// </summary>
		/// <param name="valid">True, if the entered credentials are valid, false otherwise.</param>
		public void ConfirmCredentials(bool valid)
		{
			CredUi.CredUIConfirmCredentials(TargetName, valid);
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// The targetname for the credential.
		/// </summary>
		public string TargetName
		{
			get { return m_TargetName; }
			set { m_TargetName = value; }
		}

		/// <summary>
		/// The <see cref="QueryCredentialOptions"/> used when displaying the dialog.
		/// </summary>
		public QueryCredentialOptions Options
		{
			get { return m_Options; }
			set { m_Options = value; }
		}

		/// <summary>
		/// Gets or sets the username. If set before the <see cref="CommonDialog.ShowDialog()"/> method is called,
		/// the username field of the dialog will be populated with this value. If the
		/// <see cref="CommonDialog.ShowDialog()"/> returns <see cref="DialogResult.OK"/>, this property
		/// contains the username specified by the user.
		/// </summary>
		public string Username
		{
			get { return m_Username; }
			set { m_Username = value; }
		}

		/// <summary>
		/// A <see cref="SecureString"/> containing the password.
		/// </summary>
		public SecureString Password
		{
			get { return m_Password; }
			set { m_Password = value; }
		}

		/// <summary>
		///  A string containing the title for the dialog box
		/// </summary>
		public string CaptionText
		{
			get { return m_CaptionText; }
			set { m_CaptionText = value; }
		}

		/// <summary>
		/// Bitmap to display in the dialog box. If this member is null, a default 
		/// bitmap is used. The bitmap size is limited to 320x60 pixels. 
		/// </summary>
		public Bitmap Image
		{
			get { return m_Image; }
			set { m_Image = value; }
		}

		/// <summary>
		/// A string containing a brief message to display in the dialog box.
		/// </summary>
		public string MessageText
		{
			get { return m_MessageText; }
			set { m_MessageText = value; }
		}

		/// <summary>
		/// If true, the "Save Password" checkbox is checked, false otherwise.
		/// </summary>
		public bool SavePassword
		{
			get { return m_SavePassword; }
			set { m_SavePassword = value; }
		}

		/// <summary>
		/// Does nothing.
		/// </summary>
		public override void Reset()
		{
		}

		#endregion

		#region Protected Overriden Methods

		/// <summary>
		/// Displays a dialog that requests a username/password combination from the user.
		/// </summary>
		/// <param name="hwndOwner"></param>
		/// <returns>True, if a username/password combination was provided. False, otherwise</returns>
		protected override bool RunDialog(IntPtr hwndOwner)
		{
			CredUiInfo info = new CredUiInfo();
			info.hwndParent = hwndOwner;
			info.pszCaptionText = CaptionText;
			info.pszMessageText = MessageText;
			if (Image != null) info.hbmBanner = Image.GetHbitmap();
			info.cbSize = Marshal.SizeOf(info);
			StringBuilder username = new StringBuilder(256 + 1);
			username.Append(Username);

			char[] password = new char[256 + 1];
			bool save = SavePassword;
			unsafe
			{
				fixed (char* buffer = password)
				{
					try
					{
						Array.Clear(password, 0, password.Length);

						QueryCredentialError result = CredUi.CredUIPromptForCredentials(ref info, TargetName, IntPtr.Zero, 0, username, username.Capacity, buffer, 256 * sizeof(char), ref save, Options);
						if (result == QueryCredentialError.Cancelled) return false;

						if (result != QueryCredentialError.None)
							throw new QueryCredentialDialogException(result, "An exception querying the user credentials.");

						m_SavePassword = save;

						int length = 0;
						while (length < password.Length && password[length] != 0) length++;

						m_Username = username.ToString();
						m_Password = new SecureString(buffer, length);
					}
					finally
					{
						Array.Clear(password, 0, password.Length);
					}

					return true;
				}
			}
		}

		#endregion
	}
}
