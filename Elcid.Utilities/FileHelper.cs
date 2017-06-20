using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Elcid.Utilities
{
    /// <summary>
    /// 文件操作辅助类
    /// </summary>
    public class FileHelper
    {
        static FileHelper()
        {
            Encoding = Encoding.UTF8;
        }

        /// <summary>  
        /// 编码方式  
        /// </summary>  
        public static Encoding Encoding { get; set; }

        #region 获取文件后缀名
        /// <summary>
        /// 取后缀名
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>.gif|.html格式</returns>
        public static string GetPostfixStr(string fileName)
        {
            int position = fileName.LastIndexOf(".", StringComparison.Ordinal);
            int length = fileName.Length;
            return fileName.Substring(position, length - position);
        }
        #endregion

        #region 获取文件大小,按适当单位转换
        /// <summary>  
        /// 获取文件大小，按适当单位转换  
        /// </summary>
        /// <param name="filePath">文件路径及文件名</param>  
        /// <returns>文件大小0KB</returns>  
        public static string GetFileSize(string filePath)
        {
            string result = "0KB";
            if (File.Exists(filePath))
            {
                long size = new FileInfo(filePath).Length;
                result = ConvertFileLengthUnit(size);
            }
            return result;
        }
        #endregion

        #region 写入文件
        /// <summary>  
        /// 写入文件(覆盖)
        /// </summary>
        /// <param name="filePath">文件名</param>  
        /// <param name="content">文件内容</param>  
        public static void WriteFileByCovered(string filePath, string content)
        {
            FileStream fs = new FileStream(filePath, FileMode.Create);
            //获得字节数组  
            byte[] data = Encoding.GetBytes(content);
            //开始写入  
            fs.Write(data, 0, data.Length);
            //清空缓冲区、关闭流  
            fs.Flush();
            fs.Close();
        }

        /// <summary>
        /// 写入文件(追加)
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="content">内容</param>
        public static void WriteFileByAdditional(string filePath, string content)
        {
            StreamWriter sw = File.AppendText(filePath);
            sw.Write(content);
            sw.Flush();
            sw.Close();
            sw.Dispose();
        }
        #endregion

        #region 读取文件
        /// <summary>  
        /// 读取文件  
        /// </summary>  
        /// <param name="filePath">文件路径</param>  
        /// <returns>文件内容</returns>  
        public static string ReadFile(string filePath)
        {
            return ReadFile(filePath, Encoding);
        }

        /// <summary>  
        /// 读取文件  
        /// </summary>  
        /// <param name="filePath">文件路径</param>  
        /// <param name="encoding">字符编码</param>  
        /// <returns></returns>  
        public static string ReadFile(string filePath, Encoding encoding)
        {
            using (var sr = new StreamReader(filePath, encoding))
            {
                return sr.ReadToEnd();
            }
        }

        /// <summary>  
        /// 读取文件
        /// </summary>  
        /// <param name="filePath"></param>  
        /// <returns>文件内容行列表</returns>  
        public static List<string> ReadFileLines(string filePath)
        {
            var str = new List<string>();
            using (var sr = new StreamReader(filePath, Encoding))
            {
                string input;
                while ((input = sr.ReadLine()) != null)
                {
                    str.Add(input);
                }
            }
            return str;
        }
        #endregion

        #region 拷贝文件
        /// <summary>
        /// 拷贝文件
        /// </summary>
        /// <param name="srcFile">原始文件</param>
        /// <param name="desFile">新文件路径</param>
        public static void CopyFile(string srcFile, string desFile)
        {
            File.Copy(srcFile, desFile, true);
        }
        #endregion

        #region 移动文件
        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="srcFileName">原始文件路径及文件名</param>
        /// <param name="desFileName">新文件路径及文件名</param>
        public static void MoveFile(string srcFileName, string desFileName)
        {
            File.Move(srcFileName, desFileName);
        }
        #endregion

        #region 删除文件
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filePath">路径</param>
        public static void DeleteFile(string filePath)
        {
            File.Delete(filePath);
        }
        #endregion

        #region 判断文件夹是否存在
        /// <summary>
        /// 判断文件夹是否存在
        /// </summary>
        /// <param name="path">C:\\1</param>
        public static bool IsExistFolder(string path)
        {
            return Directory.Exists(path);
        }
        #endregion

        #region 创建文件夹
        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="path">C:\\1</param>
        public static void CreateFolder(string path)
        {
            Directory.CreateDirectory(path);
        }

        /// <summary>
        /// 在当前文件夹下创建文件夹
        /// </summary>
        /// <param name="rootFolderPath">当前目录路径 如C:\\1</param>
        /// <param name="newFolder">新目录 如2\\3</param>
        public static void CreateFolder(string rootFolderPath, string newFolder)
        {
            Directory.SetCurrentDirectory(rootFolderPath);
            Directory.CreateDirectory(newFolder);
        }
        #endregion

        #region 复制文件夹
        /// <summary>  
        /// 复制文件夹（及文件夹下所有子文件夹和文件）  
        /// </summary>  
        /// <param name="srcPath">待复制的文件夹路径C:\\1</param>  
        /// <param name="desPath">目标路径C:\\2</param>  
        public static void CopyFolder(string srcPath, string desPath)
        {
            DirectoryInfo info = new DirectoryInfo(srcPath);
            CreateFolder(desPath);
            foreach (FileSystemInfo fsi in info.GetFileSystemInfos())
            {
                string desName = Path.Combine(desPath, fsi.Name);

                if (fsi is FileInfo) //如果是文件，复制文件  
                {
                    File.Copy(fsi.FullName, desName);
                }
                else //如果是文件夹，新建文件夹，递归  
                {
                    CreateFolder(desName);
                    CopyFolder(fsi.FullName, desName);
                }
            }
        }
        #endregion

        #region 删除文件夹
        /// <summary>  
        /// 删除文件夹（及文件夹下所有子文件夹和文件）  
        /// </summary>  
        /// <param name="dirPath">文件夹路径</param>  
        public static void DeleteFolder(string dirPath)
        {
            ClearFolder(dirPath);
            Directory.Delete(dirPath); //删除空文件夹  
        }
        #endregion

        #region 清空文件夹
        /// <summary>  
        /// 清空文件夹（及文件夹下所有子文件夹和文件）  
        /// </summary>  
        /// <param name="dirPath"></param>  
        public static void ClearFolder(string dirPath)
        {
            foreach (string dir in Directory.GetFileSystemEntries(dirPath))
            {
                if (File.Exists(dir))
                {
                    var fi = new FileInfo(dir);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly", StringComparison.Ordinal) != -1)
                        fi.Attributes = FileAttributes.Normal;
                    File.Delete(dir); //删除文件     
                }
                else
                {
                    DeleteFolder(dir); //删除文件夹  
                }
            }
        }
        #endregion

        #region 获取文件夹大小
        /// <summary>
        /// 获取文件夹大小
        /// </summary>
        /// <param name="dirPath">文件夹路径</param>
        /// <returns>文件大小0KB</returns>
        public static string GetFolderSize(string dirPath)
        {
            string result = "0KB";
            if (!Directory.Exists(dirPath))
            {
                return result;
            }
            long size = GetDirLength(dirPath);
            return ConvertFileLengthUnit(size);
        }
        #endregion

        #region 私有公共方法
        private static long GetDirLength(string dirPath)
        {
            long size = 0;
            DirectoryInfo rootDir = new DirectoryInfo(dirPath);
            foreach (FileInfo file in rootDir.GetFiles())
            {
                size += file.Length;
            }
            DirectoryInfo[] dirs = rootDir.GetDirectories();
            if (dirs.Length > 0)
            {
                foreach (DirectoryInfo dir in dirs)
                {
                    size += GetDirLength(dir.FullName);
                }
            }
            return size;
        }

        private static string ConvertFileLengthUnit(long size)
        {
            string result = "0KB";
            int length = size.ToString().Length;
            if (length < 4)
                result = size + "byte";
            else if (length < 7)
                result = Math.Round(Convert.ToDouble(size / 1024d), 2) + "KB";
            else if (length < 10)
                result = Math.Round(Convert.ToDouble(size / 1024d / 1024), 2) + "MB";
            else if (length < 13)
                result = Math.Round(Convert.ToDouble(size / 1024d / 1024 / 1024), 2) + "GB";
            else
                result = Math.Round(Convert.ToDouble(size / 1024d / 1024 / 1024 / 1024), 2) + "TB";
            return result;
        }
        #endregion
    }

    /// <summary>
    /// 简单文件类
    /// </summary>
    public class FileStruct
    {
        /// <summary>  
        /// 是否为目录  
        /// </summary>  
        public bool IsDirectory { get; set; }
        /// <summary>  
        /// 创建时间  
        /// </summary>  
        public DateTime CreateTime { get; set; }
        /// <summary>  
        /// 文件或目录名称  
        /// </summary>  
        public string Name { get; set; }
        /// <summary>  
        /// 路径  
        /// </summary>  
        public string Path { get; set; }
    }
}
