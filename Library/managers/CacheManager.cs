using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using Genso.Framework;
using Microsoft.Extensions.Caching.Memory;
//using Xfrogcn.BinaryFormatter;

namespace Genso.Astrology.Library
{
    /// <summary>
    /// Manages the caches used to speed up astrological functions
    /// Caches presist accross app startup (save to disk)
    /// Note : Caches from previous file are combined with new caches & saved back together as file (cache accumulates)
    /// </summary>
    public static class CacheManager
    {

        private static object sync = new Object();

        //keep track of cache files created
        private static object cacheCounter = new { count = 0 };

        //the limit on how many cache items are saved in each file on disk
        //cache is saved in pieces to optimeze load time with threading
        private const int CacheFileLimit = 5000;

        //list of caches of methods
        private static ConcurrentDictionary<string, ConcurrentDictionary<CacheKey, object>> _cacheList = new();

        //Number of times cache was used
        public static int CacheUseCount = 0;
        public static int CacheNotUseCount = 0;


        //PUBLIC METHODS

        /// <summary>
        /// Takes current cache in memory & saves them to disk
        /// Note :
        /// -Cache is split into pieces & saved to speed up load time (using parallel)
        /// -Caches from previous file are combined with new caches & saved together (cache accumulates)
        /// </summary>
        public static void SaveCacheToDisk()
        {
            //keep track of cache files created per method, used to number cache files
            var cacheFileCount = 0;

            //save each method cache as a seperate cache file on disk
            foreach (var methodCache in _cacheList)
            {
                var methodName = methodCache.Key;
                var methodCacheData = methodCache.Value;

                //keep track of the number of cache items saved (to meet threshold)
                var itemsSaved = 0;

                //temp list to gather cache items before being saved in a file
                var tempCacheList = new ConcurrentDictionary<CacheKey, object>();

                //get the value for each key and add it to list
                foreach (var cache in methodCacheData)
                {
                    //once cache file limit (threshold) has been hit, save list to disk & reset counter
                    if (itemsSaved >= CacheFileLimit) { flushTempList(); }

                    //place cache in list
                    tempCacheList.TryAdd(cache.Key, cache.Value);

                    //increment saved counter
                    itemsSaved++;
                }

                //if some cache items have not been saved (below threshold), save them now
                if (tempCacheList.Count > 0) { flushTempList(); }

                //reset counter for this method's cache
                cacheFileCount = 0;

                //used when cache needs to be saved to disk
                void flushTempList()
                {
                    //increment count for each time saved
                    cacheFileCount++;
                    //flush the current list to disk in a new cache file
                    saveCacheInNewFile(methodName, cacheFileCount, tempCacheList);
                    //clear the list
                    tempCacheList.Clear();
                    //clear the threshold counter
                    itemsSaved = 0;
                }
            }


        }

        /// <summary>
        /// Takes cache file & loads them into memory for use (if exist)
        /// </summary>
        public static void LoadCacheFromDisk0()
        {
            //get all existing cache file names
            var foundFiles = Directory.GetFiles(Syntax.CacheFilePath, "cache*", SearchOption.TopDirectoryOnly);

            //load each cache file to memory
            Parallel.ForEach(foundFiles, file =>
            {
                //get the cache file from disk
                using var stream = File.OpenRead(file);
                var formatter = new BinaryFormatter();

                //parse the cache
                var cacheData = formatter.Deserialize(stream) as ConcurrentDictionary<CacheKey, object>;

                //get name of the method the cache belongs to
                var rawName = file.Split('_');
                var methodName = rawName[1];

                //load cache into memory
                _cacheList.TryAdd(methodName, cacheData);

            });
        }

        public static void LoadCacheFromDisk()
        {
            //starts the thread 
            Thread thread = new(_loadCacheFromDisk);
            thread.Start();


        }

        private static void _loadCacheFromDisk()
        {
            //get all existing cache file names
            var foundFiles = Directory.GetFiles(Syntax.CacheFilePath, "cache*", SearchOption.TopDirectoryOnly);

            //load each cache file to memory
            Parallel.ForEach(foundFiles, file =>
            {
                //get the cache file from disk
                using var stream = File.OpenRead(file);
                var formatter = new BinaryFormatter();

                //get name of the method the cache belongs to
                var rawName = file.Split('_');
                var methodName = rawName[1];

                ConcurrentDictionary<CacheKey, object> cacheData;

                //parse the cache
                try
                {
                    cacheData = formatter.Deserialize(stream) as ConcurrentDictionary<CacheKey, object>;
                }
                //if fail just skip this cache file
                catch (Exception)
                {
                    LogManager.Error($"Loading cache failed : {rawName}");
                    return;
                }


                //try load whole cache into memory
                var result = _cacheList.TryAdd(methodName, cacheData);

                //if loading whole cache failed, 
                if (!result)
                {
                    //try load one cache item at a time
                    var methodCache = getMethodCache(methodName);
                    Parallel.ForEach(cacheData, cache => methodCache.TryAdd(cache.Key, cache.Value));
                }

                LogManager.Debug("Cache Loaded: " + methodName);

            });

            LogManager.Debug("All Cache Loaded");

        }

        /// <summary>
        /// Gets cache if available if not, does the heavy computation saves the results to cache
        /// and returns the results to the caller
        /// </summary>
        public static T GetCache<T>(CacheKey key, Func<T> heavyComputation)
        {
            //based on calling method, get the correct cache that holds the data
            var methodCache = getMethodCache(key.Function);

            //if value is in cache return value to caller, end here
            if (methodCache.TryGetValue(key, out var value))
            {
                CacheUseCount++; return (T)value;
            }

            //if no value found in cache 
            //do heavy computation to get the value
            CacheNotUseCount++;
            value = methodCache[key] = heavyComputation();

            //return value to caller
            return (T)value;

        }

        public static IEnumerable GetKeys(this IMemoryCache memoryCache) =>
            ((IDictionary)GetEntriesCollection((MemoryCache)memoryCache)).Keys;

        public static IEnumerable<T> GetKeys<T>(this IMemoryCache memoryCache) =>
            GetKeys(memoryCache).OfType<T>();



        //PRIVATE METHODS


        /// <summary>
        /// Deletes all cache files in disk
        /// </summary>
        private static void deleteCacheFiles()
        {
            //get all existing cache file names
            var foundFiles = Directory.GetFiles(Syntax.CacheFilePath, "cache*", SearchOption.TopDirectoryOnly);

            //delete each file
            foreach (var file in foundFiles)
            {
                File.Delete(file);
            }

        }

        /// <summary>
        /// Creates or overwrites cache file to store the cache data on disk
        /// </summary>
        private static void saveCacheInNewFile(string cacheFileName, int count, object tempCacheList)
        {
            //int count = 1;

            CreateFile:
            //create a new file name based on the count to avoid collision (exp:cache_2.dat)
            var newFileName = $"{Syntax.CacheFileName}_{cacheFileName}_{count}.dat";

            try
            {
                //create/overwrite the cache file
                FileStream stream = File.Create(newFileName);
                var formatter = new BinaryFormatter();

                //save cache from memory to disk
                formatter.Serialize(stream, tempCacheList);
                stream.Close();

            }
            //if accesing file failed, try again with different name (count)
            catch (Exception)
            {
                LogManager.Error("Saving cache file failed!");
                count++;
                goto CreateFile;
            }

        }

        private static int getCacheFileCount()
        {
            //get all existing cache file names
            var foundFiles = Directory.GetFiles(Syntax.CacheFilePath, "cache*", SearchOption.TopDirectoryOnly);

            //return the number of files found
            return foundFiles.Length;
        }

        /// <summary>
        /// Get the cache for the specified method, if none exist make one
        /// </summary>
        private static ConcurrentDictionary<CacheKey, object> getMethodCache(string methodName)
        {

            //if value is in cache return to caller, end here
            if (_cacheList.TryGetValue(methodName, out var value))
            {
                //if value is null, try again, possible miss with multiple threads
                if (value == null)
                {
                    //log the cache miss
                    LogManager.Debug("Cache said to be loaded, but not here!");
                    goto Start;
                }
                return value;
            }

            Start:
            //if no value found in cache, make new cache for the method 
            value = _cacheList[methodName] = new ConcurrentDictionary<CacheKey, object>();

            //return the new cache for the method to caller
            return value;

        }

        // EXTENSION FUNCTIONS TO GET KEYS OUT OF MEMORY CACHE (USED IN ASTRONOMICAL FUNCTION CACHING)

        private static readonly Func<MemoryCache, object> GetEntriesCollection = Delegate.CreateDelegate(
            typeof(Func<MemoryCache, object>),
            typeof(MemoryCache).GetProperty("EntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance).GetGetMethod(true),
            throwOnBindFailure: true) as Func<MemoryCache, object>;


    }
}



//---------------------------ARCHIVE CODE


//-------------------test
//get all keys
//var allKeys = _cache.GetKeys();

//var asList = allKeys.Cast<CacheKey>().ToList();

//var list2 = asList.FindAll(x => x.Function == "GetPlanetSayanaLongitude");

//var list3 = list2.FindAll((x) =>
//{
//    var y = (DateTimeOffset)x.Args[0];

//    return y.ToString() == "12/31/2020 4:00:00 PM +00:00";

//});

//-------------------test

//delete all previous cache files
//deleteCacheFiles();

////keep track of the number of cache items saved (to meet threshold)
//var itemsSaved = 0;

////temp list to gather cache items before being saved in a file
//var tempCacheList = new List<KeyValuePair<CacheKey, object>>();

////get the value for each key and add it to list
//foreach (var keyValue in allCacheData)
//{
//    //once cache file limit (threshold) has been hit, save list to disk & reset counter
//    if (itemsSaved >= CacheFileLimit) { flushTempList(); }

//    //place cache in list
//    tempCacheList.Add(keyValue);

//    //increment saved counter
//    itemsSaved++;
//}

////if some cache items have not been saved (below threshold), save them now
//if (tempCacheList.Any()) { flushTempList(); }


////used when cache needs to be saved to disk
//void flushTempList()
//{
//    //flush the current list to disk in a new cache file
//    saveCacheInNewFile(tempCacheList);
//    //clear the list
//    tempCacheList.Clear();
//    //clear the threshold counter
//    itemsSaved = 0;
//}
