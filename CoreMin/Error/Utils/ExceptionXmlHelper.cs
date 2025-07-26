//--------------------------------------------------------------------------
// File:    ExceptionXmlHelper.cs
// Content:	Implementation of class ExceptionXmlHelper
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives

using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using SmartExpert;
using SmartExpert.Linq;

#endregion

namespace SmartExpert.Error
{
	/// <summary>
	/// Provides Exception XML formatting helper methods.
	/// </summary>
	public static class ExceptionXmlHelper
	{
		#region Exception XML formatting methods

		/// <summary>
		/// Gets the XML representation of an execption.
		/// </summary>
		/// <param name="exception">The exception.</param>
		/// <returns>The XML representation of the execption.</returns>
		public static XDocument GetExceptionXml(this Exception exception)
		{
			ArgChecker.ShouldNotBeNull(exception, "exception");

			XDocument xDocException = new XDocument();

			XElement root = new XElement("exceptionInfo",
				new XElement("fullName", FormatExceptionTypeName(exception)),
				new XElement("message", exception.Message));

			xDocException.Add(root);

			CombinedException combinedException = exception as CombinedException;
			if (combinedException != null)
			{
				foreach (Exception innerException in combinedException.InnerExceptions)
				{
					Exception ex = innerException;
					int deepth = 0;
					while (ex != null)
					{
						AppendExceptionXml(root, ex, deepth);
						ex = ex.InnerException;
						++deepth;
					}
				}
			}
			else
			{
				Exception ex = exception;
				int deepth = 0;
				while (ex != null)
				{
					AppendExceptionXml(root, ex, deepth);
					ex = ex.InnerException;
					++deepth;
				}
			}

			return xDocException;
		}

		#endregion

		#region Private Exception formatting helper methods

		private static string FormatExceptionTypeName(Exception exception)
		{
			if (exception == null)
				return string.Empty;

			string exceptionTypeFullName = exception.GetType().FullName;
			
			if (exceptionTypeFullName == null)
				return string.Empty;

			exceptionTypeFullName = exceptionTypeFullName.Replace("`1[[", "{");
			exceptionTypeFullName = exceptionTypeFullName.Replace("]]", "}");

			int genericTypeStartIndex = exceptionTypeFullName.IndexOf("{");

			if (genericTypeStartIndex < 1)
				return exceptionTypeFullName;

			int fullQualifiedSeperatorIndex = exceptionTypeFullName.IndexOf(",", genericTypeStartIndex);
			int genericTypeEndIndex = exceptionTypeFullName.IndexOf("}", genericTypeStartIndex);

			if (fullQualifiedSeperatorIndex > genericTypeStartIndex)
			{
				exceptionTypeFullName = exceptionTypeFullName.Remove(fullQualifiedSeperatorIndex, genericTypeEndIndex - fullQualifiedSeperatorIndex);
			}
			
			return exceptionTypeFullName;
		}

		private static void AppendExceptionXml(XElement root, Exception exception, int deepth)
		{
			XElement xe = new XElement("exception",
				new XAttribute("deepth", deepth.ToString()),
				new XAttribute("type", FormatExceptionTypeName(exception)),
				new XComment("Exception Target Site"),
				new XElement("targetSite",
					new XAttribute("assembly", exception.TargetSite.DeclaringType.Module.Name),
					new XAttribute("class", exception.TargetSite.DeclaringType.Name),
					new XAttribute("method", exception.TargetSite.ToString())));
			
			xe.Add(new XComment("Exception Properties"));
			XElement xProp = new XElement("properties");
			AppendProperties(xProp, exception);
			xe.Add(xProp);

			xe.Add(new XComment("Exception Data"));
			XElement xData = new XElement("data");
			AppendDictionary(xData, exception.Data);
			xe.Add(xData);

			if (exception.StackTrace != null)
			{
				xe.Add(new XComment("Exception Stack Trace"));
				XElement xStackTrace = new XElement("stackTrace", exception.StackTrace);
				xe.Add(xStackTrace);
			}

			root.Add(xe);
		}

		private static void AppendProperties(XElement xe, Exception exception)
		{
			try
			{
				PropertyInfo[] api = exception.GetType().GetProperties();
				object o;
				foreach (PropertyInfo pi in api)
				{

					try
					{
						if ((pi.Name == "InnerException") || (pi.Name == "StackTrace") || (pi.Name == "Data"))
							continue;

						o = pi.GetValue(exception, null);
						string value = (o != null) ? o.ToString() : "NULL";

						if (value != pi.PropertyType.ToString())
						{
							XElement xProperty = new XElement("property",
								new XAttribute("name", pi.Name),
								new XAttribute("value", value));

							xe.Add(xProperty);
						}
						else
						{
							XElement xProperty = new XElement("property",
															new XAttribute("name", pi.Name),
															new XAttribute("value", value));

							AppendProperties(xProperty, o);
							xe.Add(xProperty);
						}
					}
					catch (Exception ex)
					{
						if (ex.IsFatal())
							throw;
					}
				}
			}
			catch (Exception ex)
			{
				if (ex.IsFatal())
					throw;
			}
		}

		private static void AppendProperties(XElement xe, object instance)
		{
			try
			{
				PropertyInfo[] api = instance.GetType().GetProperties();
				object o;
				foreach (PropertyInfo pi in api)
				{

					try
					{
						o = pi.GetValue(instance, null);
						string value = (o != null) ? o.ToString() : "NULL";

						if (value != pi.PropertyType.ToString())
						{
							XElement xProperty = new XElement("property",
								new XAttribute("name", pi.Name),
								new XAttribute("value", value));

							xe.Add(xProperty);
						}
						else
						{
							XElement xProperty = new XElement("property",
															new XAttribute("name", pi.Name),
															new XAttribute("value", value));

							AppendProperties(xProperty, o);

							xe.Add(xProperty);
						}
					}
					catch (Exception ex)
					{
						if (ex.IsFatal())
							throw;
					}
				}
			}
			catch (Exception ex)
			{
				if (ex.IsFatal())
					throw;
			}
		}

		private static void AppendDictionary(XElement xe, IDictionary dict)
		{
			if (dict == null || dict.Count == 0)
				return;
			try
			{
				object[] keyObjects = new object[dict.Count];
				string[] keys = new string[dict.Count];
				dict.Keys.CopyTo(keyObjects, 0);
				for (int i = 0; i < keyObjects.Length; ++i)
					keys[i] = keyObjects[i].ToString();
				Array.Sort(keys, keyObjects);


				string key;
				object keyObject;
				string value;
				for (int i = 0; i < keyObjects.Length; ++i)
				{
					key = keys[i];
					try
					{
						keyObject = keyObjects[i];
						value = StringHelper.SafeToString(dict[keyObject], "NULL");

						XElement xDataItem = new XElement("dataItem",
							new XAttribute("name", key),
							new XAttribute("value", value));

						xe.Add(xDataItem);
					}
					catch (Exception ex)
					{
						if (ex.IsFatal())
							throw;
					}
				}
			}
			catch (Exception ex)
			{
				if (ex.IsFatal())
					throw;
			}
		}


		#endregion

		

	}
}
