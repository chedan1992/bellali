using System;
using System.IO;
using System.Text;

namespace QINGUO.Common
{
    /// <summary>
    /// 文件操作类  by zhoupeng
    /// </summary>
    public class FileHelper
    {
        private Encoding defaultEncoding = Encoding.UTF8;
        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="filePath">文件夹的物理路径</param>
        /// <returns></returns>
        public bool CreateDirectory(string filePath)
        {
            try
            {
                if (!Directory.Exists(filePath))
                {
                    DirectoryInfo di = Directory.CreateDirectory(filePath);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="filePath">文件的物理路径</param>
        /// <param name="fileContent">文件的内容</param>
        public bool CreateFile(string filePath, string fileContent)
        {
            try
            {
                if (CreateDirectory(Path.GetDirectoryName(filePath)))
                {
                    Encoding code = defaultEncoding;
                    StreamWriter mySream = new StreamWriter(filePath, false, code);
                    mySream.WriteLine(fileContent);
                    mySream.Flush();
                    mySream.Close();
                    mySream = null;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取文件内容
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public string ReadFileContent(string filePath, Encoding encoding)
        {
            try
            {
                string fileContent = "";
                using (StreamReader sr = new StreamReader(filePath, encoding))
                {
                    fileContent = sr.ReadToEnd();
                }
                return fileContent;
            }
            catch
            {
                return "读取文件时产生不可预知的错误。";
            }
        }

        /// <summary>
        /// 获取文件内容
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public string ReadFileContent(string filePath)
        {
            return ReadFileContent(filePath, defaultEncoding);
        }

        /// <summary>
        /// 写文件操作
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileContent"></param>
        /// <param name="isAppend"></param>
        /// <returns></returns>
        public bool WriteFileContent(string filePath, string fileContent, bool isAppend)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    CreateFile(filePath, "");
                }
                StreamWriter Fso = new StreamWriter(filePath);
                Fso.WriteLine(fileContent);
                Fso.Close();
                Fso.Dispose();
                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public bool DeleteFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 删除文件组
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>是否全部删除，全部删除则为True,否则为False</returns>
        public bool DeleteFile(string[] filePath)
        {
            bool isAllDelete = true;
            for (int i = 0; i < filePath.Length; i++)
            {
                if (!DeleteFile(filePath[i]))
                {
                    isAllDelete = false;
                }
            }
            return isAllDelete;
        }

        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        public bool DeleteDirectory(string directoryPath)
        {
            try
            {
                if (Directory.Exists(directoryPath))
                {
                    Directory.Delete(directoryPath);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 读取HTML文件内容
        /// </summary>
        /// <param name="Path">物理路径</param>
        /// <returns></returns>
        public static string ReadHtml(string Path)
        {
            string result = string.Empty;
            if (File.Exists(Path))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(Path, Encoding.GetEncoding("UTF-8")))
                    {
                        result = sr.ReadToEnd();
                    }
                }
                catch
                { }
            }
            else
            {
                result = "模板不存在!";
            }
            return result;
        }
        /// <summary>
        /// 重命名文件
        /// </summary>
        /// <param name="filePath">文件所在的目录,不要最后那个斜杠例如E:\\Dir\\GG</param>
        /// <param name="oldName">原名称</param>
        /// <param name="newName">修改的名称</param>
        /// <param name="fileType">文件类型 0为文件夹 1是文件</param>
        /// <returns></returns>
        public bool ReNameFile(string filePath, string oldName, string newName, int fileType)
        {
            try
            {
                if (fileType.Equals(0))
                {
                    if (Directory.Exists(filePath + "\\" + oldName))
                    {
                        Directory.Move(filePath + "\\" + oldName, filePath + "\\" + newName.Replace(".", ""));
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (File.Exists(filePath + "\\" + oldName))
                    {
                        File.Move(filePath + "\\" + oldName, filePath + "\\" + newName);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        //对文件生成树
        public StringBuilder fileTree = new StringBuilder();
        int directoryIndex = 0;
        public string rootUrl = "";
        int listIndex = 0;
        public void listFiles(string dir, int level)
        {
            try
            {
                string[] dirs = Directory.GetDirectories(dir);
                foreach (string d in dirs)
                {
                    directoryIndex++;
                    if (d.LastIndexOf("\\") == -1)
                    {
                        fileTree.AppendLine("d.add(" + directoryIndex + "," + level + ",'" + d.Substring(d.LastIndexOf("/") + 1) + "');");
                    }
                    else
                    {
                        fileTree.AppendLine("d.add(" + directoryIndex + "," + level + ",'" + d.Substring(d.LastIndexOf("\\") + 1) + "');");
                    }
                    if (Directory.Exists(d))
                    {
                        listFiles(d, directoryIndex);
                    }

                }

                string[] files = Directory.GetFiles(dir, "*.*htm*");

                foreach (string f in files)
                {

                    directoryIndex++;
                    if (f.LastIndexOf("\\") == -1)
                    {
                        fileTree.AppendLine("filelist[" + listIndex + "]='" + ((f.Replace(rootUrl + "\\", "")).Replace("\\", "/")) + "';");
                        fileTree.AppendLine("d.add(" + directoryIndex + "," + level + ",''" + f.Substring(f.LastIndexOf("/") + 1) + "','javascript: GetFileUrl(filelist[" + listIndex + "]);');");
                    }
                    else
                    {
                        fileTree.AppendLine("filelist[" + listIndex + "]='" + ((f.Replace(rootUrl + "\\", "")).Replace("\\", "/")) + "';");
                        fileTree.AppendLine("d.add(" + directoryIndex + "," + level + ",'" + f.Substring(f.LastIndexOf("\\") + 1) + "','javascript: GetFileUrl(filelist[" + listIndex + "]);');");
                    }
                    listIndex++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 只取文件夹名
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="level"></param>
        /// kevin改
        public void listFileName(string dir, int level)
        {
            try
            {
                string[] dirs = Directory.GetDirectories(dir);
                foreach (string d in dirs)
                {
                    directoryIndex++;
                    if (d.LastIndexOf("\\") == -1)
                    {
                        fileTree.AppendLine("d.add(" + directoryIndex + "," + level + ",'" + d.Substring(d.LastIndexOf("/") + 1) + "','javascript: GetFileUrl(filelist[" + listIndex + "]);');");
                    }
                    else
                    {
                        fileTree.AppendLine("filelist[" + listIndex + "]='" + ((d.Replace(rootUrl + "\\", "")).Replace("\\", "/")) + "';");
                        fileTree.AppendLine("d.add(" + directoryIndex + "," + level + ",'" + d.Substring(d.LastIndexOf("\\") + 1) + "','javascript: GetFileUrl(filelist[" + listIndex + "]);');");
                    }
                    listIndex++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 计算文件大小函数(保留两位小数),Size为字节大小
        /// </summary>
        /// <param name="Size">初始文件大小</param>
        /// <returns></returns>
        public string CountSize(long Size)
        {
            string m_strSize = "";
            long FactSize = 0;
            FactSize = Size;
            if (FactSize < 1024.00)
                m_strSize = FactSize.ToString("F2") + " Byte";
            else if (FactSize >= 1024.00 && FactSize < 1048576)
                m_strSize = (FactSize / 1024.00).ToString("F2") + " KB";
            else if (FactSize >= 1048576 && FactSize < 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00).ToString("F2") + " MB";
            else if (FactSize >= 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00 / 1024.00).ToString("F2") + " GB";
            return m_strSize;
        }

        /// <summary>
        /// 获得文件的类型
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
  
        public static String getFileType(String fileName)
        {
            String type = "";
            if (fileName == null || fileName == "")
                return type;
            int position = fileName.LastIndexOf(".");
            if (position != -1)
            {
                type = fileName.Substring(position + 1, fileName.Length);
            }
            return type;
        }
    }
}
