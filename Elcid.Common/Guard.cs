
namespace Elcid.Common
{
    /// <summary>
    /// 提供对程序的异常检查
    /// </summary>
    public static class Guard
    {
        /// <summary>
        /// 检查对象是否为 null。
        /// </summary>
        /// <param name="obj">要检查的对象。</param>
        /// <param name="message">异常的提示信息。</param>
        /// <exception cref="ArgumentException">对象为 null。</exception>
        public static void NullReference(object obj, string message = null)
        {
            if (obj == null)
            {
                if (string.IsNullOrEmpty(message))
                {
                    message = SR.GetString(SRKind.NullReference);
                }

                throw new ArgumentException(message);
            }
        }

        /// <summary>
        /// 检查参数是否为 null。
        /// </summary>
        /// <param name="obj">要检查的参数对象。</param>
        /// <param name="paramName">参数的名称。</param>
        /// <param name="message">异常的提示信息。</param>
        /// <exception cref="ArgumentNullException">参数为 null。</exception>
        public static void ArgumentNull(object obj, string paramName, string message = null)
        {
            if (obj == null)
            {
                if (string.IsNullOrEmpty(message))
                {
                    message = SR.GetString(SRKind.ArgumentNull, paramName);
                }

                throw new ArgumentNullException(message, paramName);
            }
        }
    }
}
