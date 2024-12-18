// Decompiled with JetBrains decompiler
// Type: Stub.AlgorithmAES
// Assembly: DesignV3Sv, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7FCAC0DD-9A5C-4DA4-BFB3-EE6355E60533
// Assembly location: C:\Users\hadesker\Desktop\Virus\DesignV\DesignV3Sv.exe

using System;
using System.Security.Cryptography;

#nullable disable
namespace Stub
{
  public class AlgorithmAES
  {
    public static object Decrypt(string input)
    {
      RijndaelManaged rijndaelManaged = new RijndaelManaged();
      MD5CryptoServiceProvider cryptoServiceProvider = new MD5CryptoServiceProvider();
      byte[] destinationArray = new byte[32];
      byte[] hash = cryptoServiceProvider.ComputeHash(Helper.SB(Settings.Mutex));
      Array.Copy((Array) hash, 0, (Array) destinationArray, 0, 16);
      Array.Copy((Array) hash, 0, (Array) destinationArray, 15, 16);
      rijndaelManaged.Key = destinationArray;
      rijndaelManaged.Mode = CipherMode.ECB;
      ICryptoTransform decryptor = rijndaelManaged.CreateDecryptor();
      byte[] inputBuffer = Convert.FromBase64String(input);
      return (object) Helper.BS(decryptor.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length));
    }
  }
}
