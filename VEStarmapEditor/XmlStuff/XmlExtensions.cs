namespace VEStarmapEditor.XmlStuff
{

		#region

		using System;
		using System.Globalization;
		using System.Linq;
		using System.Xml.Linq;

		#endregion

		/// <summary>
		///     The xml extensions.
		/// </summary>
		public static class XmlExtensions
		{
			#region Public Methods and Operators

			/// <summary>
			///     The clone.
			/// </summary>
			/// <param name="element">
			///     The element.
			/// </param>
			/// <returns>
			///     The <see cref="XElement" />.
			/// </returns>
			public static XElement Clone(this XElement element)
			{
				return new XElement(
					element.Name,
					element.Attributes(),
					element.Nodes().Select(
						n =>
						{
							XElement e = n as XElement;
							if (e != null)
							{
								return Clone(e);
							}

							return n;
						}));
			}

			/// <summary>
			///     The parse.
			/// </summary>
			/// <param name="el">
			///     The el.
			/// </param>
			/// <param name="func">
			///     The func.
			/// </param>
			/// <typeparam name="T">
			/// </typeparam>
			/// <returns>
			///     The <see cref="T" />.
			/// </returns>
			/// <exception cref="Exception">
			/// </exception>
			public static T Parse<T>(this XElement el, Func<XElement, T> func)
			{
				if (el == null)
				{
					throw new Exception("Not defined elibute");
				}

				return func.Invoke(el);
			}

			/// <summary>
			///     The to bool.
			/// </summary>
			/// <param name="el">
			///     The el.
			/// </param>
			/// <returns>
			///     The <see cref="bool" />.
			/// </returns>
			public static bool ToBool(this XElement el)
			{
				return el.ToByte() == 1;
			}

			/// <summary>
			///     The to bool with default.
			/// </summary>
			/// <param name="el">
			///     The el.
			/// </param>
			/// <param name="defaultValue">
			///     The default value.
			/// </param>
			/// <returns>
			///     The <see cref="bool" />.
			/// </returns>
			public static bool ToBoolWithDefault(this XElement el, bool defaultValue)
			{
				return el.ToByteWithDefault(defaultValue ? (byte)1 : (byte)0) == 1;
			}

			/// <summary>
			///     The to byte.
			/// </summary>
			/// <param name="el">
			///     The el.
			/// </param>
			/// <returns>
			///     The <see cref="byte" />.
			/// </returns>
			public static byte ToByte(this XElement el)
			{
				return byte.Parse(el.Value, CultureInfo.InvariantCulture);
			}

			/// <summary>
			///     The to byte with default.
			/// </summary>
			/// <param name="el">
			///     The el.
			/// </param>
			/// <param name="defaultValue">
			///     The default value.
			/// </param>
			/// <returns>
			///     The <see cref="byte" />.
			/// </returns>
			public static byte ToByteWithDefault(this XElement el, byte defaultValue)
			{
				return el != null ? byte.Parse(el.Value, CultureInfo.InvariantCulture) : defaultValue;
			}

			/// <summary>
			///     The to byte with default.
			/// </summary>
			/// <param name="el">
			///     The el.
			/// </param>
			/// <param name="defaultValue">
			///     The default value.
			/// </param>
			/// <returns>
			///     The <see cref="byte?" />.
			/// </returns>
			public static byte? ToByteWithDefault(this XElement el, byte? defaultValue)
			{
				return el != null ? byte.Parse(el.Value, CultureInfo.InvariantCulture) : defaultValue;
			}

			/// <summary>
			///     The to double.
			/// </summary>
			/// <param name="el">
			///     The el.
			/// </param>
			/// <returns>
			///     The <see cref="double" />.
			/// </returns>
			public static double ToDouble(this XElement el)
			{
				return double.Parse(el.Value, CultureInfo.InvariantCulture);
			}

			public static double ToDoubleWithDefault(this XElement el, double defaultValue)
			{
				return el != null ? double.Parse(el.Value, CultureInfo.InvariantCulture) : defaultValue;
			}

			/// <summary>
			///     The to enum.
			/// </summary>
			/// <param name="el">
			///     The el.
			/// </param>
			/// <typeparam name="T">
			/// </typeparam>
			/// <returns>
			///     The <see cref="T" />.
			/// </returns>
			public static T ToEnum<T>(this XElement el) where T : struct
			{
				return (T)Enum.ToObject(typeof(T), byte.Parse(el.Value));
			}

			/// <summary>
			///     The to enum with default.
			/// </summary>
			/// <param name="el">
			///     The el.
			/// </param>
			/// <param name="defaultValue">
			///     The default value.
			/// </param>
			/// <typeparam name="T">
			/// </typeparam>
			/// <returns>
			///     The <see cref="T" />.
			/// </returns>
			public static T ToEnumWithDefault<T>(this XElement el, T defaultValue) where T : struct
			{
				if (el == null)
				{
					return defaultValue;
				}

				byte value = byte.Parse(el.Value);
				Type type = typeof(T);
				if (Enum.IsDefined(type, value))
				{
					return (T)Enum.ToObject(type, value);
				}

				return defaultValue;
			}

			/// <summary>
			///     The to float.
			/// </summary>
			/// <param name="el">
			///     The el.
			/// </param>
			/// <returns>
			///     The <see cref="float" />.
			/// </returns>
			public static float ToFloat(this XElement el)
			{
				return float.Parse(el.Value, CultureInfo.InvariantCulture);
			}

			/// <summary>
			///     The to float with default.
			/// </summary>
			/// <param name="el">
			///     The el.
			/// </param>
			/// <param name="defaultValue">
			///     The default value.
			/// </param>
			/// <returns>
			///     The <see cref="float" />.
			/// </returns>
			public static float ToFloatWithDefault(this XElement el, float defaultValue)
			{
				return el != null ? float.Parse(el.Value, CultureInfo.InvariantCulture) : defaultValue;
			}

			/// <summary>
			///     The to int 16.
			/// </summary>
			/// <param name="el">
			///     The el.
			/// </param>
			/// <returns>
			///     The <see cref="short" />.
			/// </returns>
			public static short ToInt16(this XElement el)
			{
				return short.Parse(el.Value, CultureInfo.InvariantCulture);
			}

			/// <summary>
			///     The to int 32.
			/// </summary>
			/// <param name="el">
			///     The el.
			/// </param>
			/// <returns>
			///     The <see cref="int" />.
			/// </returns>
			public static int ToInt32(this XElement el)
			{
				return int.Parse(el.Value, CultureInfo.InvariantCulture);
			}

			/// <summary>
			///     The to s byte.
			/// </summary>
			/// <param name="el">
			///     The el.
			/// </param>
			/// <returns>
			///     The <see cref="sbyte" />.
			/// </returns>
			public static sbyte ToSByte(this XElement el)
			{
				return sbyte.Parse(el.Value, CultureInfo.InvariantCulture);
			}

			/// <summary>
			///     The to s byte with default.
			/// </summary>
			/// <param name="el">
			///     The el.
			/// </param>
			/// <param name="defaultValue">
			///     The default value.
			/// </param>
			/// <returns>
			///     The <see cref="sbyte" />.
			/// </returns>
			public static sbyte ToSByteWithDefault(this XElement el, sbyte defaultValue)
			{
				return el != null ? sbyte.Parse(el.Value, CultureInfo.InvariantCulture) : defaultValue;
			}

			/// <summary>
			///     The to string with default.
			/// </summary>
			/// <param name="el">
			///     The el.
			/// </param>
			/// <param name="defaultValue">
			///     The default value.
			/// </param>
			/// <returns>
			///     The <see cref="string" />.
			/// </returns>
			public static string ToStringWithDefault(this XElement el, string defaultValue)
			{
				return el != null ? el.Value : defaultValue;
			}

			public static string ToStringWithDefaultIfEmpty(this XElement el, string defaultValue)
			{
				return el != null && !string.IsNullOrEmpty(el.Value) ? el.Value : defaultValue;
			}

			/// <summary>
			///     The to u int 16.
			/// </summary>
			/// <param name="el">
			///     The el.
			/// </param>
			/// <returns>
			///     The <see cref="ushort" />.
			/// </returns>
			public static ushort ToUInt16(this XElement el)
			{
				return ushort.Parse(el.Value, CultureInfo.InvariantCulture);
			}

			/// <summary>
			///     The to u int 16 with default.
			/// </summary>
			/// <param name="el">
			///     The el.
			/// </param>
			/// <param name="defaultValue">
			///     The default value.
			/// </param>
			/// <returns>
			///     The <see cref="ushort" />.
			/// </returns>
			public static ushort ToUInt16WithDefault(this XElement el, ushort defaultValue)
			{
				return el != null ? ushort.Parse(el.Value, CultureInfo.InvariantCulture) : defaultValue;
			}

			/// <summary>
			///     The to u int 32.
			/// </summary>
			/// <param name="el">
			///     The el.
			/// </param>
			/// <returns>
			///     The <see cref="uint" />.
			/// </returns>
			public static uint ToUInt32(this XElement el)
			{
				return uint.Parse(el.Value, CultureInfo.InvariantCulture);
			}

			/// <summary>
			///     The to u int 32 with default.
			/// </summary>
			/// <param name="el">
			///     The el.
			/// </param>
			/// <param name="defaultValue">
			///     The default value.
			/// </param>
			/// <returns>
			///     The <see cref="uint" />.
			/// </returns>
			public static uint ToUInt32WithDefault(this XElement el, uint defaultValue)
			{
				return el != null ? uint.Parse(el.Value, CultureInfo.InvariantCulture) : defaultValue;
			}

			public static XmlColor ToXmlColor(this XElement el)
			{
				return XmlColor.Parse(el.Value);
			}

			public static XmlColor ToXmlColorWithDefault(this XElement el, XmlColor defaultValue)
			{
				return el != null ? XmlColor.Parse(el.Value) : defaultValue;
			}


			/// <summary>
			///     The to xml vector 2.
			/// </summary>
			/// <param name="el">
			///     The el.
			/// </param>
			/// <returns>
			///     The <see cref="XmlVector2" />.
			/// </returns>
			public static XmlVector2 ToXmlVector2(this XElement el)
			{
				return XmlVector2.Parse(el.Value);
			}

			/// <summary>
			///     The to xml vector 2 with default.
			/// </summary>
			/// <param name="el">
			///     The el.
			/// </param>
			/// <param name="defaultValue">
			///     The default value.
			/// </param>
			/// <returns>
			///     The <see cref="XmlVector2" />.
			/// </returns>
			public static XmlVector2 ToXmlVector2WithDefault(this XElement el, XmlVector2 defaultValue)
			{
				return el != null ? XmlVector2.Parse(el.Value) : defaultValue;
			}

			/// <summary>
			///     The to xml vector 3.
			/// </summary>
			/// <param name="el">
			///     The el.
			/// </param>
			/// <returns>
			///     The <see cref="XmlVector3" />.
			/// </returns>
			public static XmlVector3 ToXmlVector3(this XElement el)
			{
				return XmlVector3.Parse(el.Value);
			}

			/// <summary>
			///     The to xml vector 3 with default.
			/// </summary>
			/// <param name="el">
			///     The el.
			/// </param>
			/// <param name="defaultValue">
			///     The default value.
			/// </param>
			/// <returns>
			///     The <see cref="XmlVector3" />.
			/// </returns>
			public static XmlVector3 ToXmlVector3WithDefault(this XElement el, XmlVector3 defaultValue)
			{
				return el != null ? XmlVector3.Parse(el.Value) : defaultValue;
			}

			#endregion
		}
	
}