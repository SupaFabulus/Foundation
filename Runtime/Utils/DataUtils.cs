using System;
using System.Runtime.InteropServices;

namespace SupaFabulus.Dev.Foundation.Utils
{
    public static class DataUtils
    {
        public static byte[] ConvertToBytes<TSource>(TSource[] source)
        {
            return ConvertToType<TSource, byte>(source);
        }
        
        
        
        public static char[] ConvertToChars<TSource>(TSource[] source)
        {
            return ConvertToType<TSource, char>(source);
        }

        private static TResult[] ConvertToType<TSource, TResult>(TSource[] source)
        {
            int count = source.Length;
            int srcSize = Marshal.SizeOf(typeof(TSource));
            int totalSize = srcSize * count;
            TResult[] result = new TResult[totalSize];

            int i;
            TSource src;
            byte[] srcBytes;
            
            IntPtr ptr;
            
            for (i = 0; i < count; i++)
            {
                srcBytes = new byte[srcSize];
                src = source[i];
                ptr = IntPtr.Zero;
                
                try
                {
                    ptr = Marshal.AllocHGlobal(srcSize);
                    Marshal.StructureToPtr(src, ptr, true);
                    Marshal.Copy(ptr, srcBytes, 0, srcSize);
                }
                finally
                {
                    Marshal.FreeHGlobal(ptr);
                }
                Array.Copy
                (
                    srcBytes, 
                    0, 
                    result, 
                    i * srcSize, 
                    srcBytes.Length
                );
            }

            return result;
        }
        
        private static TResult[] ConvertToType<TSource, TResult>(TSource source)
        where TSource : struct
        {
            int srcSize = Marshal.SizeOf(typeof(TSource));
            TResult[] result = new TResult[srcSize];
            byte[] srcBytes;
            IntPtr ptr;
            
            srcBytes = new byte[srcSize];
            ptr = IntPtr.Zero;
            
            try
            {
                ptr = Marshal.AllocHGlobal(srcSize);
                Marshal.StructureToPtr(source, ptr, true);
                Marshal.Copy(ptr, srcBytes, 0, srcSize);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
            Array.Copy
            (
                srcBytes, 
                0, 
                result, 
                0, 
                srcBytes.Length
            );

            return result;
        }
        
        public static byte[] StringToBytes(string source)
        {
            if (string.IsNullOrEmpty(source)) return null;

            int i;
            int c = source.Length;
            byte[] result = new byte[c];

            for (i = 0; i < c; i++)
            {
                result[i] = (byte) source[i];
            }

            return result;
        }


        public static TResult ConvertFromBytes<TResult>(byte[] arr)
        {
            TResult result;
            Type t = typeof(TResult);
            int size = Marshal.SizeOf(t);
            IntPtr ptr = IntPtr.Zero;
            
            try
            {
                ptr = Marshal.AllocHGlobal(size);
                Marshal.Copy(arr, 0, ptr, size);
                result = (TResult)Marshal.PtrToStructure(ptr, t);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
            return result;
        }
    }
}