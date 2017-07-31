using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeacherClient.Core;

namespace TeacherClient
{
    public class CacheHelper
    {
        static readonly string _defaultpath = AppDomain.CurrentDomain.BaseDirectory + "Cache";
        /// <summary>
        /// 缓存地址
        /// </summary>
        public static string CacheFilePath
        {
            get
            {
                var path = ConfigManagerHelper.GetConfigByName(Config.CacheFilePath);
                if (string.IsNullOrWhiteSpace(CacheHelper.CacheFilePath))
                {
                    path = CacheFilePath = _defaultpath;
                }
                return path;
            }
            set
            {
                ConfigManagerHelper.SetConfigByName(Config.CacheFilePath, value);
            }
        }
        /// <summary>
        /// 获取换成大小
        /// </summary>
        /// <returns></returns>
        public static double GetCacheCount()
        {
            return GetDirectoryLength(CacheFilePath) / 1024d / 1024d;
        }
        /// <summary>
        /// 清除缓存文件夹
        /// </summary>
        public static void ClearCache()
        {
            try
            {
                ClearCache(CacheFilePath);
            }
            catch (Exception ex)
            {
                Log.Error("清除缓存失败", ex);
            }
        }

        static void ClearCache(string path)
        {
            if (Directory.Exists(path))
            {
                foreach (var dir in Directory.GetDirectories(path))
                {
                    ClearCache(dir);
                }
                foreach (var file in Directory.GetFiles(path))
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch (Exception ex)
                    {
                        Log.Info("清除缓存文件失败", ex);
                    }
                }
            }
        }

        /// <summary>
        /// 获取文件夹大小
        /// </summary>
        /// <param name="dirPath"></param>
        /// <returns></returns>
        static long GetDirectoryLength(string dirPath)
        {
            //判断给定的路径是否存在,如果不存在则退出
            if (!Directory.Exists(dirPath))
                return 0;
            long len = 0;

            //定义一个DirectoryInfo对象
            DirectoryInfo di = new DirectoryInfo(dirPath);

            //通过GetFiles方法,获取di目录中的所有文件的大小
            foreach (FileInfo fi in di.GetFiles())
            {
                len += fi.Length;
            }

            //获取di中所有的文件夹,并存到一个新的对象数组中,以进行递归
            DirectoryInfo[] dis = di.GetDirectories();
            if (dis.Length > 0)
            {
                for (int i = 0; i < dis.Length; i++)
                {
                    len += GetDirectoryLength(dis[i].FullName);
                }
            }
            return len;
        }
    }
}
