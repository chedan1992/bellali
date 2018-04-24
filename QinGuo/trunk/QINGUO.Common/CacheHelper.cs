using System;
using System.Web;
using System.Web.Caching;

namespace QINGUO.Common
{
	/// <summary>
	/// 缓存操作类
    /// Copyright (C) Maticsoft
	/// </summary>
    public class CacheHelper
	{
        private static readonly Cache Cache = HttpRuntime.Cache;

        /// <summary>
        /// 新增永久缓存
        /// </summary>
        public static void AddCache(string key, object value)
        {
            Remove(key);
            Cache.Insert(key, value);
        }

        public static void Remove(string key)
        {
            Cache.Remove(key);
        }
		/// <summary>
		/// 获取当前应用程序指定CacheKey的Cache值
		/// </summary>
        /// <param name="cacheKey"></param>
		/// <returns></returns>
		public static object GetCache(string cacheKey)
		{
			System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            return objCache[cacheKey];
		}

		/// <summary>
		/// 设置当前应用程序指定CacheKey的Cache值
		/// </summary>
        /// <param name="cacheKey"></param>
		/// <param name="objObject"></param>
        public static void SetCache(string cacheKey, object objObject)
		{
			System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(cacheKey, objObject);
		}

		/// <summary>
        /// 设置当前应用程序指定CacheKey的Cache值
		/// </summary>
		/// <param name="cacheKey"></param>
		/// <param name="objObject"></param>
		/// <param name="absoluteExpiration"></param>
		/// <param name="slidingExpiration"></param>
        public static void SetCache(string cacheKey, object objObject, DateTime absoluteExpiration, TimeSpan slidingExpiration)
		{
			System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(cacheKey, objObject, null, absoluteExpiration, slidingExpiration);
		}

        public static object Get(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }
            return Cache.Get(key);
        }

	}
}
