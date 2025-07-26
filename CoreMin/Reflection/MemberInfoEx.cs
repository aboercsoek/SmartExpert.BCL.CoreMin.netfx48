//--------------------------------------------------------------------------
// File:    MemberInfoEx.cs
// Content:	Implementation of class MemberInfoEx
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2011 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using SmartExpert.Error;

#endregion

namespace SmartExpert.Reflection
{
	///<summary>Container class for <see cref="MemberInfo"/> extension methods.</summary>
	public static class MemberInfoEx
	{
		/// <summary>
		/// Determines whether the specified member is a data member of a data contract.
		/// </summary>
		/// <param name="member">The member.</param>
		/// <returns>
		///   <see langword="true"/> if the specified member is a data member of a data contract; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool IsDataMember(this MemberInfo member)
		{
			ArgChecker.ShouldNotBeNull(member, "member");

			if (member.HasAttribute<IgnoreDataMemberAttribute>())
			{
				return false;
			}
			//if (member.DeclaringType.HasDataContractAttribute() == false)
			//{
			//    return false;
			//}
			
			return true;
		}

		/// <summary>
		/// Gets the name of the data member.
		/// <note>This method tries to get the data member name from the <see cref="DataMemberAttribute"/>, 
		/// if one is defined on the <paramref name="member"/>; otherwise the name of the <paramref name="member"/> is returned.</note>
		/// </summary>
		/// <param name="member">The member.</param>
		/// <returns>The name of the data member.</returns>
		/// <exception cref="ArgNullException">Is thrown if <paramref name="member"/> is <see langword="null"/>.</exception>
		public static string GetDataMemberName(this MemberInfo member)
		{
			ArgChecker.ShouldNotBeNull(member, "member");

			if (member.IsDataMember() == false)
				return string.Empty;

			DataMemberAttribute dataMember;
			
			if (member.TryGetAttribute(out dataMember) && !String.IsNullOrEmpty(dataMember.Name)) 
				return dataMember.Name;

			return member.Name;
		}

		/// <summary>
		/// Gets the name of the enum member.
		/// <note>This method tries to get the enum member name from the <see cref="EnumMemberAttribute"/>, 
		/// if one is defined on the <paramref name="member"/>; otherwise the name of the <paramref name="member"/> is returned.</note>
		/// </summary>
		/// <param name="member">The member.</param>
		/// <returns>The name of the enum member.</returns>
		/// <exception cref="ArgNullException">Is thrown if <paramref name="member"/> is <see langword="null"/>.</exception>
		public static string GetEnumMemberName(this MemberInfo member)
		{
			ArgChecker.ShouldNotBeNull(member, "member");

			EnumMemberAttribute enumMember;

			if (member.TryGetAttribute(out enumMember) && !String.IsNullOrEmpty(enumMember.Value)) 
				return enumMember.Value;

			return member.Name;
		}

		/// <summary>
		/// Finds a specific member of a type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="predicate">The generic search filter.</param>
		/// <returns>The MemberInfo of the member; or <see langword="null"/> if the member was not found.</returns>
		public static MemberInfo FindMember(this Type type, Predicate<MemberInfo> predicate)
		{
			return type.GetMembers().FirstOrDefault(m => predicate(m));
		}

		/// <summary>
		/// Finds a specific member of a type.
		/// </summary>
		/// <typeparam name="T">The type member info.</typeparam>
		/// <param name="type">The type.</param>
		/// <param name="predicate">The generic search filter.</param>
		/// <returns>The specific member info type; or <see langword="null"/> if not found.</returns>
		public static T FindMember<T>(this Type type, Predicate<T> predicate) where T : MemberInfo
		{
			return (T)type.GetMembers().FirstOrDefault(m => m is T && predicate((T)m));
		}
	}
}
