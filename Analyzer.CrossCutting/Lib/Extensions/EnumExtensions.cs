using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Analyzer.CrossCutting.Lib.Extensions
{
    public static class EnumExtensions
    {
        public static string Description(this IConvertible value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            try
            {
                DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

                if (attributes != null &&
                    attributes.Length > 0)
                    return attributes[0].Description;
                else
                    return value.ToString();
            }
            catch (Exception)
            {
                //LogUtil.Error(ex);
                return value.ToString();
            }
        }

        public static string Name(this Enum value)
        {
            return value.ToString();
        }

        public static int ValueAsInt(this Enum value)
        {
            return Convert.ToInt32(value);
        }

        public static char ValueAsChar(this Enum value)
        {
            return Convert.ToChar(value);
        }

        public static T FromChar<T>(string caracter) where T : struct, IConvertible
        {
            return EnumExtensions.FromChar<T>(caracter[0]);
        }

        public static T FromChar<T>(char caracter) where T : struct, IConvertible
        {
            try
            {
                if (Enum.IsDefined(typeof(T), Convert.ToInt32(caracter)))
                {
                    return (T)Enum.ToObject(typeof(T), Convert.ToInt32(caracter));
                }
                else
                {
                    return (T)(Enum.GetValues(typeof(T)).GetValue(0));
                }
            }
            catch (Exception)
            {
                //LogUtil.Error(ex);
                return (T)(Enum.GetValues(typeof(T)).GetValue(0));
            }


        }

        public static T FromInt<T>(int numero) where T : struct, IConvertible
        {
            try
            {
                if (Enum.IsDefined(typeof(T), numero))
                {
                    return (T)Enum.ToObject(typeof(T), numero);
                }
                else
                {
                    return (T)(Enum.GetValues(typeof(T)).GetValue(0));
                }
            }
            catch (Exception)
            {
                //LogUtil.Error(ex);
                return (T)(Enum.GetValues(typeof(T)).GetValue(0));
            }
        }

        public static T FromString<T>(string nome) where T : struct, IConvertible
        {
            try
            {
                return (T)Enum.Parse(typeof(T), nome.Replace(" ", ""), true);
            }
            catch (Exception)
            {
                //LogUtil.Error(ex);
                return (T)(Enum.GetValues(typeof(T)).GetValue(0));
            }

        }

        public static T FromDescription<T>(string nome) where T : struct, IConvertible
        {
            try
            {
                return EnumExtensions.GetItens<T>().FirstOrDefault(x => x.Description() == nome);
            }
            catch (Exception)
            {
                //LogUtil.Error(ex);
                return (T)(Enum.GetValues(typeof(T)).GetValue(0));
            }

        }

        public static List<T> GetItens<T>() where T : struct, IConvertible
        {
            return Enum.GetNames(typeof(T)).ToArray().Select(e => EnumExtensions.FromString<T>(e)).ToList();

        }

        public static string ValueAsString(this Enum value)
        {
            return value.ValueAsChar().ToString();
        }

        public static TEnum ToEnum<TEnum>(this string value)
        {
            return (TEnum)Enum.Parse(typeof(TEnum), value, true);
        }

        public static TEnum ToEnum<TEnum>(this string value, TEnum defaultValue) where TEnum : struct
        {
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            TEnum result;
            return Enum.TryParse<TEnum>(value, true, out result) ? result : defaultValue;
        }

        public static string ToDescription(this Enum en)
        {
            Type type = en.GetType();

            MemberInfo[] memInfo = type.GetMember(en.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(
                                              typeof(DescriptionAttribute),

                                              false);

                if (attrs != null && attrs.Length > 0)

                    return ((DescriptionAttribute)attrs[0]).Description;
            }

            return en.ToString();
        }

    }
}
