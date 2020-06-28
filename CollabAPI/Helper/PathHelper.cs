// Decompiled with JetBrains decompiler
// Type: SmartBear.Collaborator.Api.Utils.PathHelper
// Assembly: Collaborator.Api, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9c3e7b13c78c6163
// MVID: 82D41E82-1BF6-4105-8B80-342693CB64ED
// Assembly location: C:\Users\fyu\AppData\Local\Microsoft\VisualStudio\16.0_be238a39\Extensions\gzzm4hy2.wuy\Collaborator.Api.dll

using System;
using System.IO;

namespace CollabAPI
{
  public class PathHelper
  {
    private const string SLASH = "/";
    private const string BACK_SLASH = "\\";
    private const string DOUBLE_BACK_SLASH = "\\\\";

    public static string NormalizePath(string path, bool toLower = true)
    {
      if (string.IsNullOrWhiteSpace(path))
        return string.Empty;
      string str1 = path;
      if (toLower)
        str1 = path.ToLower();
      string str2 = str1.Replace("/", "\\");
      string str3;
      do
      {
        str3 = str2;
        str2 = str3.Replace("\\\\", "\\");
      }
      while (str3.Length != str2.Length);
      return str2;
    }

    public static string AddTrailingPathDelimiter(string path)
    {
      if (string.IsNullOrWhiteSpace(path))
        return string.Empty;
      string str = PathHelper.NormalizePath(path, false);
      if (!str.EndsWith("\\"))
        str += "\\";
      return str;
    }

    public static string RemoveTrailingPathDelimiter(string path)
    {
      if (string.IsNullOrWhiteSpace(path))
        return string.Empty;
      return path.Trim(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
    }

    public static string NormalizeUrl(string url, bool toLower = true)
    {
      return string.IsNullOrWhiteSpace(url) ? string.Empty : (toLower ? url.ToLower() : url).Replace("http:\\", "http://").Replace("https:\\", "https://").Replace("\\", "/");
    }

    public static string RemovePathDelimiters(string path)
    {
      if (string.IsNullOrWhiteSpace(path))
        return string.Empty;
      return path.Trim(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
    }

    private static bool CreateNewFile(string fileLocation)
    {
      try
      {
        string directoryName = Path.GetDirectoryName(fileLocation);
        if (!Directory.Exists(directoryName))
          Directory.CreateDirectory(directoryName);
        new FileStream(fileLocation, FileMode.CreateNew).Close();
      }
      catch (Exception ex)
      {
        return false;
      }
      return true;
    }

    private static bool FillFile(string fileLocation, string fileContent)
    {
      if (!File.Exists(fileLocation))
        return false;
      try
      {
        StreamWriter streamWriter = new StreamWriter(fileLocation);
        streamWriter.Write(fileContent);
        streamWriter.Close();
      }
      catch (Exception ex)
      {
        return false;
      }
      return true;
    }

    public static bool InitializeFile(string fileLocation, string fileContent)
    {
      try
      {
        PathHelper.CreateNewFile(fileLocation);
        PathHelper.FillFile(fileLocation, fileContent);
      }
      catch (Exception ex)
      {
        return false;
      }
      return true;
    }
  }
}
