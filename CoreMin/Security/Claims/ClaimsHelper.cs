//--------------------------------------------------------------------------
// File:    ClaimsHelper.cs
// Content:	Implementation of class ClaimsHelper
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2011 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.Text;

#endregion

namespace SmartExpert.Security.Claims
{
	///<summary><see cref="ClaimSet"/> helper class.</summary>
	public static class ClaimsHelper
	{
		/// <summary>
		/// Converts a ClaimSet into a string.
		/// </summary>
		/// <param name="claimSet">The claim set to convert.</param>
		/// <returns>The string representation of the claim set.</returns>
		public static string ClaimSetToString(ClaimSet claimSet)
		{
			var builder = new StringBuilder();
			builder.AppendLine("ClaimSet [");
			foreach (var claim in claimSet)
			{
				if (claim == null) continue;

				builder.Append("  ");
				builder.AppendLine(claim.ToString());
			}
			
			string str = "] by ";

			ClaimSet issuer = claimSet;
			do
			{
				issuer = issuer.Issuer;
				builder.AppendFormat("{0}{1}", str, (issuer == claimSet) ? "Self" : ((issuer.Count <= 0) ? "Unknown" : issuer[0].ToString()));
				str = " -> ";
			}
			while (issuer.Issuer != issuer);
			return builder.ToString();
		}

		/// <summary>
		/// Disposes the claim set if necessary.
		/// </summary>
		/// <param name="claimSet">The claim set to dispose.</param>
		public static void DisposeClaimSetIfNecessary(ClaimSet claimSet)
		{
			if (claimSet == null) return;

			claimSet.As<WindowsClaimSet>().DisposeIfNecessary();
		}

		/// <summary>
		/// Disposes the claim sets if necessary.
		/// </summary>
		/// <param name="claimSets">The claim sets to dispose.</param>
		public static void DisposeClaimSetsIfNecessary(IEnumerable<ClaimSet> claimSets)
		{
			if (claimSets == null) return;

			claimSets.AsSequence<WindowsClaimSet>().DisposeElementsIfNecessary();
			//foreach (ClaimSet claimSet in claimSets)
			//{
			//    claimSet.As<WindowsClaimSet>().DisposeIfNecessary();
			//}
		}
	}
}
