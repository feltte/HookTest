using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HookTest
{
	public partial class Form1 : Form
	{
		#region Win32API Methods
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		private static extern IntPtr GetForegroundWindow();

		[System.Runtime.InteropServices.DllImport("user32.dll")]
		private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

		[System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
		private static extern IntPtr WindowFromPoint(Point point);
		/*
				[System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
				private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

				[System.Runtime.InteropServices.DllImport("kernel32.dll")]
				private static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, uint dwProcessId);

				[System.Runtime.InteropServices.DllImport("kernel32.dll")]
				private static extern bool CloseHandle(IntPtr handle);
		*/
		#endregion

		private readonly int windowTextBufferLength = 256;
		private StringBuilder windowTextBuffer;
		private readonly int logMessageBufferLength = 256;
		private StringBuilder logMessageBuffer;

		public Form1()
		{
			InitializeComponent();
			windowTextBuffer = new StringBuilder(windowTextBufferLength);
			logMessageBuffer = new StringBuilder(logMessageBufferLength);
		}

		private StreamWriter logStream;
		private bool OpenLogFile()
		{
			try
			{
				logStream = new StreamWriter($"{DateTime.Now.ToString("yyyyMMdd-HHmm")}.txt", true, Encoding.GetEncoding("SHIFT_JIS"));
			}
			catch (Exception e)
			{
				return false;
			}
			return true;
		}

		private void FlushLogFile() => logStream?.Flush();

		private void CloseLogFile()
		{
			logStream?.Close();
			logStream = null;
		}

		private (long, string) GetForgroundWindowHundleAndText()
		{
			long windowHundleInt64 = 0;
			windowTextBuffer.Length = 0;
			var hwnd = GetForegroundWindow();
			if (hwnd != IntPtr.Zero)
			{
				GetWindowText(hwnd, windowTextBuffer, windowTextBuffer.Capacity);
				windowHundleInt64 = hwnd.ToInt64();
			}
			return (windowHundleInt64, windowTextBuffer.ToString());
		}

		readonly MouseHook.Stroke strokeMask = MouseHook.Stroke.LEFT_DOWN | MouseHook.Stroke.LEFT_UP
											| MouseHook.Stroke.RIGHT_DOWN | MouseHook.Stroke.RIGHT_UP;

		void hookMouseTest(ref MouseHook.StateMouse s)
		{
			button1.Text = s.X + ", " + s.Y;
			if ((s.Stroke & strokeMask) > 0)
			{
				logMessageBuffer.Length = 0;
				(var hundle, var windowText) = GetForgroundWindowHundleAndText();
				logMessageBuffer.Append($"{DateTime.Now:HH:mm:ss}, 0x{hundle:X8}, {windowText}, Mouse.{s.Stroke}");

				logStream.WriteLine(logMessageBuffer);
				textBox1.Text = logMessageBuffer + "\r\n" + textBox1.Text;
			}
		}

		void hookKeyboardTest(ref KeyboardHook.StateKeyboard s)
		{
			button2.Text = s.Key.ToString();
			logMessageBuffer.Length = 0;
			(var hundle, var windowText) = GetForgroundWindowHundleAndText();
			logMessageBuffer.Append($"{DateTime.Now:HH:mm:ss}, 0x{hundle:X8}, {windowText}, Keyboard");

			logStream.WriteLine(logMessageBuffer);
			textBox1.Text = logMessageBuffer + "\r\n" + textBox1.Text;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (logStream == null)
			{
				if (!OpenLogFile())
				{
					textBox1.Text = "failed to open log file." + textBox1.Text;
				}
			}
			if (MouseHook.IsHooking)
			{
				MouseHook.Stop();
				button1.Text = "Hook Mouse";
				FlushLogFile();
				return;
			}

			MouseHook.AddEvent(hookMouseTest);
			MouseHook.Start();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			if (logStream == null)
			{
				if (!OpenLogFile())
				{
					textBox1.Text = "failed to open log file.\r\n" + textBox1.Text;
				}
			}
			if (KeyboardHook.IsHooking)
			{
				KeyboardHook.Stop();
				button2.Text = "Hook Keyboard";
				FlushLogFile();
				return;
			}

			KeyboardHook.AddEvent(hookKeyboardTest);
			KeyboardHook.Start();
		}

		private void Form1_FormClosed(object sender, FormClosedEventArgs e)
		{
			CloseLogFile();
		}
	}
}