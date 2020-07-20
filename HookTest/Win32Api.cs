using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace HookTest
{
	class Win32Api
	{
		#region Win32API Methods
		[DllImport("user32.dll")]
		public static extern IntPtr GetForegroundWindow();

		[DllImport("user32.dll")]
		public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr WindowFromPoint(System.Drawing.Point point);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, uint dwProcessId);

		[DllImport("kernel32.dll")]
		public static extern bool CloseHandle(IntPtr handle);

		[DllImport("psapi.dll", SetLastError = true)]
		public static extern bool EnumProcessModulesEx(IntPtr hProcess, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U4)][In][Out] IntPtr[] lphModule, uint cb, [MarshalAs(UnmanagedType.U4)] out int lpcbNeeded, uint dwFilterFlag);

		[DllImport("psapi.dll")]
		public static extern uint GetModuleFileNameEx(IntPtr hProcess, IntPtr hModule, [Out] StringBuilder lpBaseName, [In][MarshalAs(UnmanagedType.U4)] uint nSize);
		/*
		//		[DllImport("version.dll")]
		//		private static extern bool GetFileVersionInfo(string sFileName, int handle, int size, byte[] infoBuffer);

				[DllImport("version.dll")]
				private static extern int GetFileVersionInfoSize(string sFileName, out int handle);

				[DllImport("version.dll", CharSet = CharSet.Auto, SetLastError = true)]
				//public static extern bool VerQueryValue(byte[] pBlock, string lpSubBlock, out IntPtr lplpBuffer, out int puLen);
				//public static extern bool VerQueryValue(byte[] pBlock, string lpSubBlock, [Out] StringBuilder lplpBuffer, out int puLen);

				[return: MarshalAs(UnmanagedType.Bool)]
				//public static extern bool VerQueryValue([In] IntPtr pBlock, [In] string lpSubBlock, [Out] out IntPtr lplpBuffer, [Out] out int puLen);
				public static extern bool VerQueryValue([In] byte[] pBlock, [In] string lpSubBlock, [Out] out IntPtr lplpBuffer, [Out] out int puLen);
		*/
		#endregion

		public const uint PROCESS_QUERY_INFORMATION = 0x0400U;
		public const uint PROCESS_VM_READ = 0x0010U;
	}
}
