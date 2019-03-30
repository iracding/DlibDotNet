﻿using System;
using System.Runtime.InteropServices;

// ReSharper disable once CheckNamespace
namespace DlibDotNet
{

    internal sealed partial class NativeMethods
    {

        [DllImport(NativeMethods.NativeLibrary, CallingConvention = NativeMethods.CallingConvention)]
        public static extern ErrorType save_jpeg(Array2DType type, IntPtr array, byte[] path, int quality);

        [DllImport(NativeMethods.NativeLibrary, CallingConvention = NativeMethods.CallingConvention)]
        public static extern ErrorType save_jpeg_matrix(MatrixElementType type, IntPtr matrix, int templateRows, int templateColumn, byte[] path, int quality);

    }

}