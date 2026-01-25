#if UNITY_STANDALONE_WIN || UNITY_EDITOR
using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.IO;

namespace ConsoleTestWindows
{
    /// <summary>
    /// 控制台窗口管理类
    /// 负责在Unity Windows独立版或编辑器中创建、配置和关闭系统控制台窗口
    /// 提供标准输入输出重定向及编码设置功能
    /// </summary>
	public class ConsoleWindow
	{
		TextWriter oldOutput;

		public void Initialize()
		{
            /// <summary>
            /// 连接到我们现有的控制台（0x0ffffffff表示当前进程），如果连接失败，就创建一个新的控制台。
            /// </summary>

			//if ( !AttachConsole( 0x0ffffffff ) )
			{
				AllocConsole();
			}

			oldOutput = Console.Out;

            try
			{
				IntPtr stdHandle = GetStdHandle( STD_OUTPUT_HANDLE );
				Microsoft.Win32.SafeHandles.SafeFileHandle safeFileHandle = new Microsoft.Win32.SafeHandles.SafeFileHandle( stdHandle, true );
                FileStream fileStream = new FileStream(safeFileHandle, FileAccess.Write);
				System.Text.Encoding encoding = System.Text.Encoding.UTF8;
				StreamWriter standardOutput = new StreamWriter( fileStream, encoding );
				standardOutput.AutoFlush = true;
				Console.SetOut( standardOutput );

            }
			catch ( System.Exception e )
			{
				Debug.Log( "Couldn't redirect output: " + e.Message );
			}

            Console.WriteLine("Current ConsoleCP: {0}",  GetConsoleCP());
            SetConsoleCP(65001); // 65001 = UTF8编码
            Console.WriteLine("Current ConsoleCP: {0}",  GetConsoleCP());
            Console.WriteLine("Current Output ConsoleCP: {0}", GetConsoleOutputCP());
            SetConsoleOutputCP(65001);
            Console.WriteLine("Current Output ConsoleCP: {0}", GetConsoleOutputCP());

            Console.WriteLine("当前输入编码方案: {0}",
                                        Console.InputEncoding);
            Console.WriteLine("当前输出编码方案: {0}",
                                        Console.OutputEncoding);
        }

		public void Shutdown()
		{
			Console.SetOut( oldOutput );
			FreeConsole();
		}

		public void SetTitle( string strName )
		{
			SetConsoleTitle( strName );
		}

		private const int STD_OUTPUT_HANDLE = -11;

		[DllImport( "kernel32.dll", SetLastError = true )]
		static extern bool AttachConsole( uint dwProcessId );

		[DllImport( "kernel32.dll", SetLastError = true )]
		static extern bool AllocConsole();

		[DllImport( "kernel32.dll", SetLastError = true )]
		static extern bool FreeConsole();


        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetConsoleCP(uint pageCode);
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern uint GetConsoleCP();
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetConsoleOutputCP(uint pageCode);
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern uint GetConsoleOutputCP();
        
        [DllImport( "kernel32.dll", EntryPoint = "GetStdHandle", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall )]
		private static extern IntPtr GetStdHandle( int nStdHandle );

		[DllImport( "kernel32.dll" )]
		static extern bool SetConsoleTitle( string lpConsoleTitle );
	}
}
#endif