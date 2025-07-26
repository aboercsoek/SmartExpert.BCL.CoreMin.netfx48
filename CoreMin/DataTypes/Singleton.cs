//--------------------------------------------------------------------------
// File:    Singleton.cs
// Content:	Implementation of class Singleton
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Linq;
using SmartExpert;
using SmartExpert.Linq;


#endregion

namespace SmartExpert
{
	///<summary>Generic Singleton implementation</summary>
	/// <typeparam name="T">Type that should be turned into a Singleton</typeparam>
	public static class Singleton<T> where T : new()
	{
		#region static Ctor to get lazy intanciation

		static Singleton()
		{
		}

		#endregion

		/// <summary>
		/// Generic Singleton access point
		/// </summary>
		public static readonly T Instance = new T();

		///// <summary>
		///// Generic Singleton access point
		///// </summary>
		//public static T Instance
		//{
		//    get
		//    {
		//        return Nested.instance;
		//    }
		//}

		///// <summary>
		///// The private nested singleton frame for type T
		///// </summary>
		//private static class Nested
		//{
		//    public static readonly T instance = new T();
		//}
	}
}
