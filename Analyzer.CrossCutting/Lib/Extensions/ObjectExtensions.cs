using System;
using System.Linq;
using System.Reflection;

namespace Analyzer.CrossCutting.Lib.Extensions
{
    public static class ObjectExtensions
    {
        public static T CopyFrom<T>(this T o, object from, string[] propriedades = null)
        {
            if (o == null)
                o = (T)Activator.CreateInstance(typeof(T));

            if (from == null)
                return o;

            var fromType = from.GetType();
            PropertyInfo[] props;

            if (propriedades == null)
            {
                props = o.GetType().GetProperties();
            }
            else
            {
                var toType = o.GetType();
                int tamanho = propriedades.Count();
                props = new PropertyInfo[tamanho];
                for (int i = 0; i < tamanho; i++)
                {
                    props[i] = toType.GetProperty(propriedades[i]);
                }
            }

            foreach (var prop in props)
            {
                if (prop != null)
                {
                    var fromProp = fromType.GetProperty(prop.Name);
                    if (fromProp != null)
                    {
                        try
                        {
                            prop.SetValue(o, fromProp.GetValue(from));
                        }
                        catch { }
                    }
                }
            }
            return o;
        }

        public static T CopyTo<T>(this object o, T to, string[] propriedades = null)
        {
            if (to == null)
                to = (T)Activator.CreateInstance(typeof(T));
            return to.CopyFrom(o, propriedades);
        }

        public static T GetAsType<T>(this object o)
        {
            return (T)o;
        }

        public static bool PropsIsNullOrEmpty(this object o, string[] propriedades = null)
        {
            bool retorno = true;
            foreach (var propriedade in propriedades)
            {
                if (!o.PropIsNullOrEmpty(propriedade))
                {
                    retorno = false;
                }
            }
            return retorno;
        }

        public static bool PropIsNullOrEmpty(this object o, string propriedade)
        {
            var valor = o.GetPropByNameAsString(propriedade);
            return (string.IsNullOrEmpty(valor) || (valor == "0"));
        }

        public static object GetPropByName(this object o, string propriedade)
        {
            var tipo = o.GetType();
            var prop = tipo.GetProperty(propriedade);
            return prop.GetValue(o);
        }

        public static void SetPropByName(this object o, string propriedade, object value)
        {
            var tipo = o.GetType();
            var prop = tipo.GetProperty(propriedade);
            prop.SetValue(o, value);
        }

        public static T GetPropByName<T>(this object o, string propriedade)
        {
            return (T)o.GetPropByName(propriedade);
        }

        public static string GetPropByNameAsString(this object o, string propriedade)
        {
            var valor = o.GetPropByName(propriedade);
            if (valor == null)
            {
                return null;
            }
            else
            {
                return valor.ToString();
            }
        }

        public static int GetPropByNameAsInt(this object o, string propriedade)
        {
            return Convert.ToInt32(o.GetPropByNameAsString(propriedade));
        }

        public static T GetFirstOrDefaultAttribute<T>(this object o) where T : Attribute
        {
            var fi = o.GetType().GetField(o.ToString());
            try
            {
                var attributes = fi.GetCustomAttributes(typeof(T), false);
                if (attributes != null && attributes.Length > 0)
                    return (T)attributes[0];
            }
            catch
            {
            }
            return default(T);
        }

        public static void CopyPropertiesTo<T, TU>(this T source, TU dest)
        {
            var sourceProps = typeof(T).GetProperties().Where(x => x.CanRead).ToList();
            var destProps = typeof(TU).GetProperties()
                    .Where(x => x.CanWrite)
                    .ToList();

            foreach (var sourceProp in sourceProps)
            {
                if (destProps.Any(x => x.Name == sourceProp.Name))
                {
                    var p = destProps.First(x => x.Name == sourceProp.Name);
                    if (p.CanWrite)
                    { // check if the property can be set or no.
                        p.SetValue(dest, sourceProp.GetValue(source, null), null);
                    }
                }

            }

        }
    }
}
