﻿using System;
using System.Runtime.InteropServices;
using DlibDotNet.Util;

// ReSharper disable once CheckNamespace
namespace DlibDotNet
{

    public sealed class DPoint : VectorBase<double>
    {

        #region Constructors

        public DPoint(double x, double y)
        {
            this.NativePtr = Native.dpoint_new1(x, y);
        }

        public DPoint()
        {
            this.NativePtr = Native.dpoint_new();
        }

        internal DPoint(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
                throw new ArgumentException("Can not pass IntPtr.Zero", nameof(ptr));

            this.NativePtr = ptr;
        }

        #endregion

        #region Properties

        public override double Length
        {
            get
            {
                this.ThrowIfDisposed();
                return Native.dpoint_length(this.NativePtr);
            }
        }

        public override double LengthSquared
        {
            get
            {
                this.ThrowIfDisposed();
                return Native.dpoint_length_squared(this.NativePtr);
            }
        }

        public override double X
        {
            get
            {
                this.ThrowIfDisposed();
                return Native.dpoint_x(this.NativePtr);
            }
        }

        public override double Y
        {
            get
            {
                this.ThrowIfDisposed();
                return Native.dpoint_y(this.NativePtr);
            }
        }

        public override double Z
        {
            get
            {
                this.ThrowIfDisposed();
                return 0;
            }
        }

        #endregion

        #region Methods

        public DPoint Rotate(DPoint point, double angle)
        {
            this.ThrowIfDisposed();

            if (point == null)
                throw new ArgumentNullException(nameof(point));

            point.ThrowIfDisposed();

            var ret = Native.rotate_dpoint(this.NativePtr, point.NativePtr, MathHelper.ConvertToRadian(angle));
            return new DPoint(ret);
        }

        public static DPoint Rotate(DPoint center, DPoint point, double angle)
        {
            return center.Rotate(point, angle);
        }

        #region Overrides

        public static DPoint operator +(DPoint point, DPoint rhs)
        {
            if (point == null)
                throw new ArgumentNullException(nameof(point));
            if (rhs == null)
                throw new ArgumentNullException(nameof(rhs));

            point.ThrowIfDisposed();
            rhs.ThrowIfDisposed();

            var ptr = Native.dpoint_operator_add(point.NativePtr, rhs.NativePtr);
            return new DPoint(ptr);
        }

        public static DPoint operator -(DPoint point, DPoint rhs)
        {
            if (point == null)
                throw new ArgumentNullException(nameof(point));
            if (rhs == null)
                throw new ArgumentNullException(nameof(rhs));

            point.ThrowIfDisposed();
            rhs.ThrowIfDisposed();

            var ptr = Native.dpoint_operator_sub(point.NativePtr, rhs.NativePtr);
            return new DPoint(ptr);
        }

        public static DPoint operator *(DPoint point, double rhs)
        {
            if (point == null)
                throw new ArgumentNullException(nameof(point));

            point.ThrowIfDisposed();

            var ptr = Native.dpoint_operator_mul(point.NativePtr, rhs);
            return new DPoint(ptr);
        }

        public static DPoint operator /(DPoint point, double rhs)
        {
            if (point == null)
                throw new ArgumentNullException(nameof(point));

            point.ThrowIfDisposed();

            if (Math.Abs(rhs) < double.Epsilon)
                throw new DivideByZeroException();

            var ptr = Native.dpoint_operator_div(point.NativePtr, rhs);
            return new DPoint(ptr);
        }

        public static bool operator ==(DPoint point, DPoint rhs)
        {
            if (ReferenceEquals(point, rhs))
                return true;
            if (ReferenceEquals(point, null) || ReferenceEquals(rhs, null))
                return false;

            point.ThrowIfDisposed();
            rhs.ThrowIfDisposed();

            return Native.dpoint_operator_equal(point.NativePtr, rhs.NativePtr);
        }

        public static bool operator !=(DPoint point, DPoint rhs)
        {
            if (ReferenceEquals(point, rhs))
                return false;
            if (ReferenceEquals(point, null) || ReferenceEquals(rhs, null))
                return true;

            point.ThrowIfDisposed();
            rhs.ThrowIfDisposed();

            return !Native.dpoint_operator_equal(point.NativePtr, rhs.NativePtr);
        }

        protected override void DisposeUnmanaged()
        {
            base.DisposeUnmanaged();
            Native.dpoint_delete(this.NativePtr);
        }

        #endregion

        #endregion

        internal sealed class Native
        {

            [DllImport(NativeMethods.NativeLibrary, CallingConvention = NativeMethods.CallingConvention)]
            public static extern IntPtr dpoint_new();

            [DllImport(NativeMethods.NativeLibrary, CallingConvention = NativeMethods.CallingConvention)]
            public static extern IntPtr dpoint_new1(double x, double y);

            [DllImport(NativeMethods.NativeLibrary, CallingConvention = NativeMethods.CallingConvention)]
            public static extern void dpoint_delete(IntPtr point);

            [DllImport(NativeMethods.NativeLibrary, CallingConvention = NativeMethods.CallingConvention)]
            public static extern double dpoint_length(IntPtr dpoint);

            [DllImport(NativeMethods.NativeLibrary, CallingConvention = NativeMethods.CallingConvention)]
            public static extern double dpoint_length_squared(IntPtr dpoint);

            [DllImport(NativeMethods.NativeLibrary, CallingConvention = NativeMethods.CallingConvention)]
            public static extern double dpoint_x(IntPtr dpoint);

            [DllImport(NativeMethods.NativeLibrary, CallingConvention = NativeMethods.CallingConvention)]
            public static extern double dpoint_y(IntPtr dpoint);

            [DllImport(NativeMethods.NativeLibrary, CallingConvention = NativeMethods.CallingConvention)]
            public static extern IntPtr dpoint_operator_add(IntPtr point, IntPtr rhs);

            [DllImport(NativeMethods.NativeLibrary, CallingConvention = NativeMethods.CallingConvention)]
            public static extern IntPtr dpoint_operator_sub(IntPtr point, IntPtr rhs);

            [DllImport(NativeMethods.NativeLibrary, CallingConvention = NativeMethods.CallingConvention)]
            public static extern IntPtr dpoint_operator_mul(IntPtr point, double rhs);

            [DllImport(NativeMethods.NativeLibrary, CallingConvention = NativeMethods.CallingConvention)]
            public static extern IntPtr dpoint_operator_div(IntPtr point, double rhs);

            [DllImport(NativeMethods.NativeLibrary, CallingConvention = NativeMethods.CallingConvention)]
            [return: MarshalAs(UnmanagedType.U1)]
            public static extern bool dpoint_operator_equal(IntPtr point, IntPtr rhs);

            [DllImport(NativeMethods.NativeLibrary, CallingConvention = NativeMethods.CallingConvention)]
            public static extern IntPtr rotate_dpoint(IntPtr center, IntPtr p, double rhs);

        }

    }

}
