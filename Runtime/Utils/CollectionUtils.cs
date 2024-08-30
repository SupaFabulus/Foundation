using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SupaFabulus.Dev.Foundation.Utils
{
    public static class CollectionUtils
    {
        public static T[,] ArrayToGrid<T>
        (
            T[] flatData,
            int width,
            int height
        )
        {
            int x;
            int y;
            T[,] result;

            if (width * height != flatData.Length)
            {
                Debug.LogError("Dimensions/Dataset Size Mismatch!");
                return default;
            }

            result = new T[width, height];

            for (y = 0; y < height; y++)
            {
                for (x = 0; x < width; x++)
                {
                    result[y, x] = flatData[(y * width) + x];
                }
            }

            return result;
        }
        
        public static T[] GridToArray<T>
        (
            T[,] gridData,
            int width,
            int height
        )
        {
            int x;
            int y;
            T[] result;

            
            if (width * height != gridData.Length)
            {
                Debug.LogError("Dimensions/Dataset Size Mismatch!");
                Debug.Log($"w:{width}, h:{height}, data:{gridData.Length}");
                return default;
            }

            result = new T[width * height];

            for (y = 0; y < height; y++)
            {
                for (x = 0; x < width; x++)
                {
                    result[(y * width) + x] = gridData[y, x];
                }
            }

            return result;
        }
    }
}