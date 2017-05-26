using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Elcid.Utilities
{
    /// <summary>
    /// FTP上传下载帮助类
    /// </summary>
    public class FtpHelper
    {
        /// <summary>  
        /// FTP请求对象  
        /// </summary>  
        FtpWebRequest request = null;
        /// <summary>  
        /// FTP响应对象  
        /// </summary>  
        FtpWebResponse response = null;
        /// <summary>  
        /// FTP服务器地址  
        /// </summary>  
        public string ftpURI { get; private set; }
        /// <summary>  
        /// FTP服务器IP  
        /// </summary>  
        public string ftpServerIP { get; private set; }
        /// <summary>  
        /// FTP服务器默认目录  
        /// </summary>  
        public string ftpRemotePath { get; private set; }
        /// <summary>  
        /// FTP服务器登录用户名  
        /// </summary>  
        public string ftpUserID { get; private set; }
        /// <summary>  
        /// FTP服务器登录密码  
        /// </summary>  
        public string ftpPassword { get; private set; }

        /// <summary>    
        /// 初始化  
        /// </summary>    
        /// <param name="ftpServerIp">FTP连接地址</param>    
        /// <param name="remotePath">指定FTP连接成功后的当前目录, 如果不指定即默认为根目录</param>    
        /// <param name="userId">用户名</param>
        /// <param name="password">密码</param>
        public FtpHelper(string ftpServerIp, string remotePath, string userId, string password)
        {
            this.ftpServerIP = ftpServerIP;
            this.ftpRemotePath = ftpRemotePath;
            this.ftpUserID = ftpUserID;
            this.ftpPassword = ftpPassword;
            this.ftpURI = "ftp://" + ftpServerIP + "/" + ftpRemotePath + "/";
        }
        ~FtpHelper()
        {
            if (response != null)
            {
                response.Close();
                response = null;
            }
            if (request != null)
            {
                request.Abort();
                request = null;
            }
        }

    }
}
