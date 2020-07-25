using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace HookTest
{
	public partial class Form1 : Form
	{
		private readonly int WINDOW_TEXT_BUFFRE_LENGTH = 256;
		/// <summary>
		/// Window Text(アプリタイトル)取得用バッファ
		/// </summary>
		private StringBuilder windowTextBuffer;

		private StringBuilder logMessageBuffer;

		Dictionary<IntPtr, string> windowHandleExeFilePathDic;

		public Form1()
		{
			InitializeComponent();
			windowTextBuffer = new StringBuilder(WINDOW_TEXT_BUFFRE_LENGTH);
			logMessageBuffer = new StringBuilder();
			windowHandleExeFilePathDic = new Dictionary<IntPtr, string>();
			this.Text = Properties.Resources.ApplicationTittle;
		}

		private StreamWriter logStream;
		private string OpenLogFile()
		{
			var logFileName = $"{DateTime.Now:yyyyMMdd-HHmm}.txt";
			try
			{
				logStream = new StreamWriter(logFileName, true, Encoding.GetEncoding("SHIFT_JIS"));
			}
			catch (Exception)
			{
				return null;
			}
			return logFileName;
		}

		readonly int FlushLogCount = 500;
		int logCounter;
		private void FlushLogFile()
		{
			logCounter = 0;
			logStream?.Flush();
		}

		private void CloseLogFile()
		{
			logStream?.Close();
			logStream = null;
		}

		private (IntPtr, string) GetForgroundWindowHundleAndText()
		{
			windowTextBuffer.Length = 0;
			var hwnd = Win32Api.GetForegroundWindow();
			if (hwnd != IntPtr.Zero)
			{
				Win32Api.GetWindowText(hwnd, windowTextBuffer, windowTextBuffer.Capacity);
			}
			return (hwnd, windowTextBuffer.ToString());
		}

		private (IntPtr, string) GetWindowHundleAndTextFromPoint(Point point)
		{
			windowTextBuffer.Length = 0;
			var hwnd = Win32Api.WindowFromPoint(point);
			if (hwnd != IntPtr.Zero)
			{
				Win32Api.GetWindowText(hwnd, windowTextBuffer, windowTextBuffer.Capacity);
			}
			return (hwnd, windowTextBuffer.ToString());
		}

		readonly MouseHook.Stroke strokeMask = MouseHook.Stroke.LEFT_DOWN | MouseHook.Stroke.LEFT_UP
											| MouseHook.Stroke.RIGHT_DOWN | MouseHook.Stroke.RIGHT_UP;

		private int previousX, previousY;

		class ProcessInfo
		{
			public uint ProcessId = 0;
			public IntPtr ProcessHandle = IntPtr.Zero;
			public int NumberOfModules = 0;
			public int Win32ErrorCode = 0;
			public string ProcessFilePath=null;
		}

		private ProcessInfo GetProcessInfoFromWindowHandle(IntPtr windowHandle)
		{
			var processInfo = new ProcessInfo();

			Debug.WriteLine("");
			Win32Api.GetWindowThreadProcessId(windowHandle, out processInfo.ProcessId);
			if (processInfo.ProcessId == 0)	return processInfo;
			Debug.WriteLine($"Process ID: {processInfo.ProcessId}");

			processInfo.ProcessHandle = Win32Api.OpenProcess(Win32Api.PROCESS_QUERY_INFORMATION | Win32Api.PROCESS_VM_READ, false, processInfo.ProcessId);
			if (processInfo.ProcessHandle == IntPtr.Zero) return processInfo;
			Debug.WriteLine($"Process Handle: {processInfo.ProcessHandle}");

			const uint LIST_MODULES_ALL = 0x03;
			IntPtr[] moduleHandles = new IntPtr[0];
			int bytesNeeded = 0;
			if (!Win32Api.EnumProcessModulesEx(processInfo.ProcessHandle, moduleHandles, 0, out bytesNeeded, LIST_MODULES_ALL))
			{
				Debug.WriteLine($"EnumProcessModulesEx モジュールサイズ取得失敗");
				Win32Api.CloseHandle(processInfo.ProcessHandle);
				processInfo.Win32ErrorCode = Marshal.GetLastWin32Error();
				return processInfo;
			}
			Debug.WriteLine($"Module Size: {bytesNeeded}");

			processInfo.NumberOfModules = bytesNeeded / IntPtr.Size;
			moduleHandles = new IntPtr[processInfo.NumberOfModules];
			if (!Win32Api.EnumProcessModulesEx(processInfo.ProcessHandle, moduleHandles, (uint)bytesNeeded, out bytesNeeded, LIST_MODULES_ALL))
			{
				Debug.WriteLine($"EnumProcessModulesEx モジュール取得失敗");
				Win32Api.CloseHandle(processInfo.ProcessHandle);
				processInfo.Win32ErrorCode = Marshal.GetLastWin32Error();
				return processInfo;
			}

			StringBuilder moduleFilePath = new StringBuilder(1024);
			Win32Api.GetModuleFileNameEx(processInfo.ProcessHandle, moduleHandles[0], moduleFilePath, (uint)(moduleFilePath.Capacity));
			processInfo.ProcessFilePath = moduleFilePath.ToString();
			Debug.WriteLine($"モジュールファイルパス:{processInfo.ProcessFilePath}");

			Win32Api.CloseHandle(processInfo.ProcessHandle);
			return processInfo;
		}

		void AppendToLogFileAndTextBox(string logMessage)
		{
			logMessageBuffer.Insert(0, logMessage + "\r\n");
			textBoxLog.Text = logMessageBuffer.ToString();
			logStream.WriteLine(logMessage);
			if (++logCounter > FlushLogCount) FlushLogFile();

			textBoxLogSize.Text = logMessageBuffer.Length.ToString();
			textBoxHandleCount.Text = windowHandleExeFilePathDic.Count.ToString();
		}

		void hookMouseTest(ref MouseHook.StateMouse s)
		{
			textBoxMouse.Text = s.X + ", " + s.Y;

			if ((s.Stroke & strokeMask) == 0)	return;			// Down/Up以外は無視
			if (s.X == previousX && s.Y == previousY) return;	// 前回と同じ場所でのクリックは無視

			(var handle, var windowText) = GetForgroundWindowHundleAndText();

			if (handle == this.Handle) return;

			if(!windowHandleExeFilePathDic.ContainsKey(handle))
			{
				var processInfo = GetProcessInfoFromWindowHandle(handle);
				windowHandleExeFilePathDic.Add(handle, Path.GetFileName(processInfo.ProcessFilePath));
			}

			AppendToLogFileAndTextBox($"{DateTime.Now:HH:mm:ss}\tM\t{windowText}\t{windowHandleExeFilePathDic[handle]}");

			previousX = s.X;
			previousY = s.Y;
		}

		IntPtr previousHwnd = IntPtr.Zero;
		long previousTicks = 0;
		void hookKeyboardTest(ref KeyboardHook.StateKeyboard s)
		{
			textBoxKeyboard.Text = s.Key.ToString();
			(var handle, var windowText) = GetForgroundWindowHundleAndText();

			if (handle == previousHwnd && (DateTime.Now.Ticks - previousTicks)<10000000) return;    // 同じウインドウ上で1秒以内のキー入力は無視する
			if (handle == this.Handle) return;

			if (!windowHandleExeFilePathDic.ContainsKey(handle))
			{
				var processInfo = GetProcessInfoFromWindowHandle(handle);
				windowHandleExeFilePathDic.Add(handle, Path.GetFileName(processInfo.ProcessFilePath));
			}

			AppendToLogFileAndTextBox($"{DateTime.Now:HH:mm:ss}\tK\t{windowText}\t{windowHandleExeFilePathDic[handle]}");

			previousHwnd = handle;
			previousTicks = DateTime.Now.Ticks;
		}

		private void buttonLog_Click(object sender, EventArgs e)
		{
			string logFileName = null;
			if (logStream == null)
			{
				logFileName = OpenLogFile();
				if (logFileName==null)
				{
					textBoxLog.Text = "ログファイルが作成できません.\r\n" + textBoxLog.Text;
					return;
				}
			}

			if (MouseHook.IsHooking)
			{
				this.Text = Properties.Resources.ApplicationTittle;
				textBoxLog.Text = "記録を停止しました\r\n" + textBoxLog.Text;
				MouseHook.Stop();
				KeyboardHook.Stop();
				buttonLog.Text = "記録開始";
				FlushLogFile();
				return;
			}

			textBoxLog.Text = $"記録を開始します {Path.GetFileName(logFileName)}\r\n" + textBoxLog.Text;
			buttonLog.Text = "記録停止";
			this.Text = Properties.Resources.ApplicationTittle + " - 記録中";

			MouseHook.AddEvent(hookMouseTest);
			MouseHook.Start();
			KeyboardHook.AddEvent(hookKeyboardTest);
			KeyboardHook.Start();
		}

		private void Form1_FormClosed(object sender, FormClosedEventArgs e)
		{
			CloseLogFile();
		}
	}
}