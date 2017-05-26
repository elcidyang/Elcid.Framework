

using System;

namespace Elcid.Common.Extensions
{
    /// <summary>
    /// 一般扩展类
    /// </summary>
    public static class GenericExtension
    {
        /// <summary>
        /// 判断对象是否为空
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this object source)
        {
            return source == null ||
                    source is DBNull ||
                    string.IsNullOrEmpty(source.ToString());
        }


        /// <summary>
        /// 将对象转换为指定的类型。
        /// </summary>
        /// <typeparam name="TSource">对象的类型。</typeparam>
        /// <typeparam name="TTarget">要转换的类型。</typeparam>
        /// <param name="source">源对象。</param>
        /// <param name="defaultValue">转换失败后返回的默认值。</param>
        /// <returns></returns>
        public static TTarget To<TSource, TTarget>(this TSource source, TTarget defaultValue = default(TTarget))
        {
            return (TTarget)ToType(source, typeof(TTarget), defaultValue);
        }

        /// <summary>
        /// 将对象转换为指定的类型。
        /// </summary>
        /// <param name="value">源对象。</param>
        /// <param name="conversionType">要转换的对象类型。</param>
        /// <param name="defaultValue">转换失败后返回的默认值。</param>
        /// <returns></returns>
        public static object ToType(this object value, Type conversionType, object defaultValue = null)
        {
            Guard.ArgumentNull(conversionType, "conversionType");
            if (value.IsNullOrEmpty())
            {
                return conversionType.IsNullableType() ? null : (defaultValue ?? conversionType.GetDefaultValue());
            }
            if (value.GetType() == conversionType)
            {
                return value;
            }

            try
            {
                if (conversionType.IsEnum)
                {
                    return Enum.Parse(conversionType, value.ToString(), true);
                }
                if (conversionType == typeof(bool?) && Convert.ToInt32(value) == -1)
                {
                    return null;
                }

                if (conversionType.IsNullableType())
                {
                    return conversionType.New(new[] { value.ToType(conversionType.GetGenericArguments()[0]) });
                }
                if (conversionType == typeof(bool))
                {
                    if (value is string)
                    {
                        var lower = ((string)value).ToLower();
                        return lower == "true" || lower == "t" || lower == "1" || lower == "yes" || lower == "on";
                    }
                    return Convert.ToInt32(value) == 1;
                }
                if (value is bool)
                {
                    if (conversionType == typeof(string))
                    {
                        return Convert.ToBoolean(value) ? "true" : "false";
                    }
                    return Convert.ToBoolean(value) ? 1 : 0;
                }
                if (conversionType == typeof(Type))
                {
                    return Type.GetType(value.ToString(), false, true);
                }
                if (value is Type && conversionType == typeof(string))
                {
                    return ((Type)value).FullName;
                }
                if (typeof(IConvertible).IsAssignableFrom(conversionType))
                {
                    return Convert.ChangeType(value, conversionType, null);
                }

                return value.CloneTo(conversionType);
            }
            catch (Exception exp)
            {
                return defaultValue;
            }
        }
    }
}
