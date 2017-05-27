using System;
using System.Collections.Generic;
using System.IO;
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
        private FtpWebRequest request = null;
        /// <summary>  
        /// FTP服务器地址  
        /// </summary>  
        public string FtpUri { get; private set; }
        /// <summary>  
        /// FTP服务器IP  
        /// </summary>  
        public string FtpServerIp { get; private set; }
        /// <summary>  
        /// FTP服务器默认目录  
        /// </summary>  
        public string FtpRemotePath { get; private set; }
        /// <summary>  
        /// FTP服务器登录用户名  
        /// </summary>  
        public string FtpUserId { get; private set; }
        /// <summary>  
        /// FTP服务器登录密码  
        /// </summary>  
        public string FtpPassword { get; private set; }

        /// <summary>    
        /// 初始化  
        /// </summary>    
        /// <param name="ftpServerIp">FTP连接地址</param>    
        /// <param name="remotePath">指定FTP连接成功后的当前目录, 如果不指定即默认为根目录</param>    
        /// <param name="userId">用户名</param>
        /// <param name="password">密码</param>
        public FtpHelper(string ftpServerIp, string remotePath, string userId, string password)
        {
            FtpServerIp = ftpServerIp;
            FtpRemotePath = remotePath ?? "";
            FtpUserId = userId;
            FtpPassword = password;
            FtpUri = "ftp://" + FtpServerIp + "/" + FtpRemotePath + "/";
        }
        ~FtpHelper()
        {
            if (request != null)
            {
                request.Abort();
                request = null;
            }
        }

        #region 上传文件
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="filePath">本地文件路径及文件名</param>
        public void Upload(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            request = OpenRequest(new Uri(FtpUri + fileInfo.Name), WebRequestMethods.Ftp.UploadFile);
            request.ContentLength = fileInfo.Length;
            int buffLength = 2048;
            byte[] buff = new byte[buffLength];
            using (FileStream fs = fileInfo.OpenRead())
            {

                Stream stream = request.GetRequestStream();
                var contentLen = fs.Read(buff, 0, buffLength);
                while (contentLen != 0)
                {
                    stream.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }
                stream.Close();
                stream.Dispose();
            }
        }
        #endregion

        #region 下载文件
        /// <summary>  
        /// 下载文件
        /// </summary>
        /// <param name="saveFilePath">下载后的保存路径</param>  
        /// <param name="remoteFileName">要下载的文件路径及文件名</param>  
        public void Download(string saveFilePath, string remoteFileName)
        {
            string fileName = remoteFileName;
            if (remoteFileName.Contains("/"))
            {
                int index = remoteFileName.LastIndexOf("/", StringComparison.Ordinal);
                fileName = remoteFileName.Substring(index+1, remoteFileName.Length-1-index);
            }
            FileHelper.CreateFolder(saveFilePath);
            using (FileStream outputStream = new FileStream(saveFilePath + "\\" + fileName, FileMode.Create))
            {
                request = OpenRequest(new Uri(FtpUri + remoteFileName), WebRequestMethods.Ftp.DownloadFile);
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                using (Stream ftpStream = response.GetResponseStream())
                {
                    int bufferSize = 2048;
                    byte[] buffer = new byte[bufferSize];
                    int readCount = ftpStream.Read(buffer, 0, bufferSize);
                    while (readCount > 0)
                    {
                        outputStream.Write(buffer, 0, readCount);
                        readCount = ftpStream.Read(buffer, 0, bufferSize);
                    }
                }
                response.Close();
            }
        }
        #endregion

        #region 删除文件
        /// <summary>    
        /// 删除文件    
        /// </summary>    
        /// <param name="remoteFilePath">要删除的文件路径及文件名</param>  
        public FtpWebResponse DeleteFile(string remoteFilePath)
        {
            request = OpenRequest(new Uri(FtpUri + remoteFilePath), WebRequestMethods.Ftp.DeleteFile);
            return (FtpWebResponse)request.GetResponse();
        }
        #endregion

        #region 创建目录
        /// <summary>  
        /// 创建目录  
        /// </summary>  
        /// <param name="remoteDirectoryName">目录名</param>  
        public FtpWebResponse CreateDirectory(string remoteDirectoryName)
        {
            request = OpenRequest(new Uri(FtpUri + remoteDirectoryName ?? ""), WebRequestMethods.Ftp.MakeDirectory);
            return (FtpWebResponse)request.GetResponse();
        }
        #endregion

        #region 检测是否存在目录

        #endregion

        #region 切换当前目录
        /// <summary>
        /// 切换当前目录
        /// </summary>
        /// <param name="directoryName">路径</param>
        /// <param name="isRoot">true:绝对路径 false:相对路径</param>
        public void GotoDirectory(string directoryName, bool isRoot)
        {
            if (isRoot)
            {
                FtpRemotePath = directoryName;
            }
            else
            {
                if (!FtpRemotePath.EndsWith("/"))
                {
                    FtpRemotePath += "/";
                }
                FtpRemotePath += directoryName + "/";
            }
            if (!FtpRemotePath.EndsWith("/"))
            {
                FtpRemotePath += "/";
            }
            FtpUri = "ftp://" + FtpServerIp + "/" + FtpRemotePath;
        }
        #endregion

        #region 判断当前目录或文件是否存在
        /// <summary>
        /// 指定文件夹是否存在
        /// </summary>
        /// <param name="remoteDesDirName">远程文件目录</param>
        /// <param name="dirName">文件夹名</param>
        /// <returns></returns>
        public bool IsDirectoryExist(string remoteDesDirName,string dirName)
        {
            bool isExist = false;
            var listDir = GetDirList(remoteDesDirName);
            foreach (FileStruct dir in listDir)
            {
                if (dir.Name == dirName)
                {
                    isExist = true;
                }
            }
            return isExist;
        }
        /// <summary>         
        /// 判断当前目录下指定的子文件是否存在        
        /// </summary>         
        /// <param name="remoteDesDirName">远程文件目录</param>  
        /// <param name="fileName">文件名</param>          
        public bool IsFileExist(string remoteDesDirName, string fileName)
        {
            bool isExist = false;
            var listFile = GetFileList(remoteDesDirName);
            foreach (FileStruct file in listFile)
            {
                if (file.Name == fileName)
                {
                    isExist = true;
                }
            }
            return isExist;
        }
        #endregion

        #region 删除目录(包括下面所有子目录和子文件)
        /// <summary>  
        /// 删除目录(包括下面所有子目录和子文件)  
        /// </summary>  
        /// <param name="remoteDirectoryName">要删除的带路径目录名：如web/test</param>  
        public FtpWebResponse RemoveDirectory(string remoteDirectoryName)
        {
            var all = GetFileAndDirList(remoteDirectoryName);
            foreach (var m in all)
            {
                if (m.IsDirectory)
                {
                    RemoveDirectory(m.Path);
                }
                else
                {
                    DeleteFile(m.Path);
                }
            }
            request = OpenRequest(new Uri(FtpUri + remoteDirectoryName ?? ""), WebRequestMethods.Ftp.RemoveDirectory);
            return (FtpWebResponse)request.GetResponse();
        }
        #endregion

        #region 更改目录或文件名
        /// <summary>  
        /// 更改目录或文件名
        /// </summary>  
        /// <param name="currentName">当前名称</param>  
        /// <param name="newName">修改后新名称</param>  
        public FtpWebResponse ReName(string currentName, string newName)
        {
            request = OpenRequest(new Uri(FtpUri + currentName), WebRequestMethods.Ftp.Rename);
            request.RenameTo = newName;
            return (FtpWebResponse)request.GetResponse();
        }
        #endregion

        #region 获取FTP文件列表
        /// <summary>
        /// 获取当前目录的文件和一级子目录信息
        /// </summary>
        /// <param name="remoteDirectoryName">目录名</param>
        /// <returns></returns>
        public List<FileStruct> GetFileAndDirList(string remoteDirectoryName)
        {
            List<FileStruct> fileList = new List<FileStruct>();
            request = OpenRequest(new Uri(FtpUri + remoteDirectoryName ?? ""), WebRequestMethods.Ftp.ListDirectoryDetails);
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            using (var stream = response.GetResponseStream())
            {
                using (var sr = new StreamReader(stream))
                {
                    string line = null;
                    while ((line = sr.ReadLine()) != null)
                    {
                        //line的格式如下：  
                        //08-18-13  11:05PM       <DIR>          aspnet_client  
                        //09-22-13  11:39PM                 2946 Default.aspx  
                        DateTime dtDate = DateTime.ParseExact(line.Substring(0, 8), "MM-dd-yy", null);
                        DateTime dtDateTime = DateTime.Parse(dtDate.ToString("yyyy-MM-dd") + line.Substring(8, 9));
                        string[] arrs = line.Split(' ');
                        var model = new FileStruct()
                        {
                            IsDirectory = line.IndexOf("<DIR>", StringComparison.Ordinal) > 0,
                            CreateTime = dtDateTime,
                            Name = arrs[arrs.Length - 1],
                            Path = FtpRemotePath + "/" + remoteDirectoryName + "/" + arrs[arrs.Length - 1]
                        };
                        fileList.Add(model);
                    }
                }
            }
            response.Close();
            return fileList;
        }

        /// <summary>
        /// 获取文件列表(不包括文件夹)
        /// </summary>
        /// <param name="remoteDirectoryName"></param>
        /// <returns></returns>
        public List<FileStruct> GetFileList(string remoteDirectoryName)
        {
            List<FileStruct> result = new List<FileStruct>();
            List<FileStruct> all = GetFileAndDirList(remoteDirectoryName);
            foreach (FileStruct item in all)
            {
                if (!item.IsDirectory)
                {
                    result.Add(item);
                }
            }
            return result;
        }


        /// <summary>         
        /// 列出当前目录的所有一级子目录
        /// </summary>         
        public List<FileStruct> GetDirList(string remoteDirectoryName)
        {
            List<FileStruct> all = GetFileAndDirList(remoteDirectoryName);
            List<FileStruct> dirs = new List<FileStruct>();
            foreach (FileStruct item in all)
            {
                if (item.IsDirectory)
                {
                    dirs.Add(item);
                }
            }
            return dirs;
        }
        #endregion

        #region 私有方法
        /// <summary>         
        /// 建立FTP链接,返回请求对象         
        /// </summary>        
        /// <param name="uri">FTP地址</param>         
        /// <param name="ftpMethod">操作命令</param>         
        private FtpWebRequest OpenRequest(Uri uri, string ftpMethod)
        {
            request = (FtpWebRequest)WebRequest.Create(uri);
            request.Method = ftpMethod;
            request.UseBinary = true;
            request.KeepAlive = false;
            request.Credentials = new NetworkCredential(FtpUserId, FtpPassword);
            return request;
        }

        #endregion  
    }
}
