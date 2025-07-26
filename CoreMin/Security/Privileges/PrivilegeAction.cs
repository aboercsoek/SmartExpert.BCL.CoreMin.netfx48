//--------------------------------------------------------------------------
// File:    PrivilegeAction.cs
// Content:	Implementation of class PrivilegeAction
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives

using SmartExpert.Interop;

#endregion

namespace SmartExpert.Security.Privileges
{
	internal class PrivilegeAction
	{
		#region Private Fields
		private LUID m_Privilege;
		private PrivilegeState m_State;
		#endregion

		#region Ctor

		public PrivilegeAction(LUID privilege, PrivilegeState newState)
		{
			m_Privilege = privilege;
			m_State = newState;
		}

		#endregion

		#region Public Properties

		public LUID Privilege
		{
			get { return m_Privilege; }
		}


		public PrivilegeState State
		{
			get { return m_State; }
		}

		#endregion

	}
}
