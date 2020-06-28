// Decompiled with JetBrains decompiler
// Type: SmartBear.Collaborator.Api.ApiUtils
// Assembly: Collaborator.Api, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9c3e7b13c78c6163
// MVID: 82D41E82-1BF6-4105-8B80-342693CB64ED
// Assembly location: C:\Users\fyu\AppData\Local\Microsoft\VisualStudio\16.0_be238a39\Extensions\gzzm4hy2.wuy\Collaborator.Api.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;

namespace CollabAPI
{
  public static class ApiUtils
  {
    private const string _commentSymbol = "#";

    public static bool ToggleAllowUnsafeHeaderParsing(bool enable)
    {
            //Assembly assembly = Assembly.GetAssembly(typeof (SettingsSection));
            //if (assembly != (Assembly) null)
            //{
            //  Type type = assembly.GetType("System.Net.Configuration.SettingsSectionInternal");
            //  if (type != (Type) null)
            //  {
            //    object obj = type.InvokeMember("Section", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.GetProperty, (Binder) null, (object) null, new object[0]);
            //    if (obj != null)
            //    {
            //      FieldInfo field = type.GetField("useUnsafeHeaderParsing", BindingFlags.Instance | BindingFlags.NonPublic);
            //      if (field != (FieldInfo) null)
            //      {
            //        field.SetValue(obj, (object) enable);
            //        return true;
            //      }
            //    }
            //  }
            //}
            //return false;
            return true;
    }

    public static DateTime GetJsonSafeDateTime(DateTime dateTime)
    {
      return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
    }

    public static Group.GroupDescription GetGroupDescription(
      string title,
      IEnumerable<Group.GroupDescription> groups)
    {
      if (groups != null)
      {
        foreach (Group.GroupDescription group in groups)
        {
          if (string.Equals(group.title, title))
            return group;
        }
      }
      return (Group.GroupDescription) null;
    }

    public static string GetGroupGuid(string title, IEnumerable<Group.GroupDescription> groups)
    {
      if (groups != null)
      {
        foreach (Group.GroupDescription group in groups)
        {
          if (string.Equals(group.title, title))
            return group.guid;
        }
      }
      return string.Empty;
    }

    public static string GetGroupGuid(string title, int reviewId, IServer server)
    {
      List<Group.GroupDescription> groupList = server.GroupService.GetGroupList(reviewId);
      return groupList == null ? string.Empty : ApiUtils.GetGroupGuid(title, (IEnumerable<Group.GroupDescription>) groupList);
    }

    public static string GetLocationDisplayString(Review.Locator locator, string filePath)
    {
      Review.LocatorType locatorType = ApiUtils.GetLocatorType(locator.type);
      string typeFriendlyName = ApiUtils.GetLocatorTypeFriendlyName(new Review.LocatorType?(locatorType));
      switch (locatorType)
      {
        case Review.LocatorType.ANNOTATION:
          return typeFriendlyName;
        case Review.LocatorType.OVERALL:
          return string.IsNullOrWhiteSpace(filePath) ? typeFriendlyName : string.Format("{0}, {1}", (object) Path.GetFileName(filePath), (object) typeFriendlyName);
        case Review.LocatorType.DELETED:
          return typeFriendlyName;
        case Review.LocatorType.LABEL:
          return locator.label;
        case Review.LocatorType.LINE:
          return string.IsNullOrWhiteSpace(filePath) ? string.Format("{0} {1}", (object) typeFriendlyName, (object) locator.lineNumber) : string.Format("{0}, {1} {2}", (object) Path.GetFileName(filePath), (object) typeFriendlyName, (object) locator.lineNumber);
        case Review.LocatorType.COORDINATE:
          string str;
          if (locator.pinNumber == 0)
            str = string.Format("Page {0} - Pin [{1}, {2}]", (object) locator.page, (object) locator.x, (object) locator.y);
          else
            str = string.Format("Page {0} - Pin {1} [{2}, {3}]", (object) locator.page, (object) locator.pinNumber, (object) locator.x, (object) locator.y);
          return str;
        case Review.LocatorType.CELL:
          return ApiUtils.GetLocatorTypeFriendlyName(new Review.LocatorType?(locatorType));
        default:
          return string.Empty;
      }
    }

    public static bool IsReviewPhaseTerminal(Review.ReviewPhase reviewPhase)
    {
      return reviewPhase == Review.ReviewPhase.CANCELLED || reviewPhase == Review.ReviewPhase.COMPLETED || reviewPhase == Review.ReviewPhase.REJECTED;
    }

    public static string GetReviewUrl(int reviewId, IServer server)
    {
      ISessionInfo sessionInfo = server.SessionInfo;
      if (sessionInfo == null)
        return string.Empty;
      return string.Format("{0}/ui#review:id={1}", (object) sessionInfo.ServerURL.TrimEnd('/'), (object) reviewId);
    }

    public static string GetLegacyReviewUrl(int reviewId, IServer server)
    {
      ISessionInfo sessionInfo = server.SessionInfo;
      if (sessionInfo == null)
        return string.Empty;
      return string.Format("{0}/go?page=ReviewDisplay&reviewid={1}", (object) sessionInfo.ServerURL.TrimEnd('/'), (object) reviewId);
    }

    public static string GetCommentTypeJSonName(Client.CommentType? item)
    {
      Client.CommentType? nullable = item;
      if (nullable.HasValue)
      {
        switch (nullable.GetValueOrDefault())
        {
          case Client.CommentType.USER_COMMENT:
            return "USER";
          case Client.CommentType.FILE_ANNOTATION:
            return "NOTE";
          case Client.CommentType.ACCEPT:
            return "ACCEPT";
          case Client.CommentType.MARK_READ:
            return "READ";
          case Client.CommentType.FILE_UPLOAD:
            return "FILE_UPLOAD";
          case Client.CommentType.FILES_REMOVED:
            return "FILE_REMOVE";
          case Client.CommentType.DEFECT_NEW:
            return "DEFECT_NEW";
          case Client.CommentType.DEFECT_CHANGE:
            return "DEFECT_CHANGE";
          case Client.CommentType.DEFECT_DELETE:
            return "DEFECT_DELETE";
          case Client.CommentType.DEFECT_MARK_FIXED:
            return "DEFECT_FIXED";
          case Client.CommentType.DEFECT_MARK_OPEN:
            return "DEFECT_OPEN";
          case Client.CommentType.DEFECT_MARK_EXTERNAL:
            return "DEFECT_EXTERNAL";
          case Client.CommentType.DEFECT_MARK_NOT_EXTERNAL:
            return "DEFECT_NOTEXTERNAL";
          case Client.CommentType.CHECKLIST_ITEM_CHECKED:
            return "CHECKLIST_CHECKED";
          case Client.CommentType.CHECKLIST_ITEM_UNCHECKED:
            return "CHECKLIST_UNCHECKED";
          case Client.CommentType.SIGNED:
            return "ESIG_SIGNED";
          case Client.CommentType.DECLINED_TO_SIGN:
            return "ESIG_DECLINED";
          case Client.CommentType.COMMENT_MOVED:
            return "MOVED";
        }
      }
      return string.Empty;
    }

    public static Client.CommentType GetCommentType(string name)
    {
      foreach (Client.CommentType commentType in Enum.GetValues(typeof (Client.CommentType)))
      {
        string commentTypeJsonName = ApiUtils.GetCommentTypeJSonName(new Client.CommentType?(commentType));
        if (string.Equals(name, commentTypeJsonName))
          return commentType;
        string b = commentType.ToString("F");
        if (string.Equals(name, b))
          return commentType;
      }
      throw new InvalidEnumArgumentException(string.Format("Unknown CommentType name - {0}", (object) name));
    }

    public static bool IsCommentLine(string line)
    {
      return !string.IsNullOrEmpty(line) && line.Trim().StartsWith("#");
    }

    public static string GetDefectStateFriendlyName(Client.DefectState? item)
    {
      Client.DefectState? nullable = item;
      if (nullable.HasValue)
      {
        switch (nullable.GetValueOrDefault())
        {
          case Client.DefectState.OPEN:
            return "Open";
          case Client.DefectState.FIXED:
            return "Fixed";
          case Client.DefectState.EXTERNAL:
            return "External";
        }
      }
      return string.Empty;
    }

    public static Client.DefectState GetDefectState(string name)
    {
      foreach (Client.DefectState defectState in Enum.GetValues(typeof (Client.DefectState)))
      {
        string stateFriendlyName = ApiUtils.GetDefectStateFriendlyName(new Client.DefectState?(defectState));
        if (string.Equals(name, stateFriendlyName))
          return defectState;
        string b = defectState.ToString("F");
        if (string.Equals(name, b))
          return defectState;
      }
      throw new InvalidEnumArgumentException(string.Format("Unknown DefectState name - {0}", (object) name));
    }

    public static Review.Role GetRoleBySystemName(string systemName)
    {
      foreach (Review.Role role in Enum.GetValues(typeof (Review.Role)))
      {
        string b = role.ToString("F");
        if (string.Equals(systemName, b, StringComparison.OrdinalIgnoreCase))
          return role;
      }
      throw new InvalidEnumArgumentException(string.Format("Unknown Role system name - {0}", (object) systemName));
    }

    public static Review.Role GetRole(string roleName, IServer server)
    {
      List<SystemAdmin.RoleSettings> availableRoles = server.ReviewService.GetAvailableRoles();
      SystemAdmin.RoleSettings roleSettings = availableRoles.Find((Predicate<SystemAdmin.RoleSettings>) (rs => string.Equals(rs.name, roleName))) ?? availableRoles.Find((Predicate<SystemAdmin.RoleSettings>) (rs => string.Equals(rs.systemName, roleName)));
      if (roleSettings != null)
        return ApiUtils.GetRoleBySystemName(roleSettings.systemName);
      throw new InvalidEnumArgumentException(string.Format("Unknown Role name - {0}", (object) roleName));
    }

    public static List<string> GetAccessPolicyDisplayList()
    {
      List<string> stringList = new List<string>();
      foreach (Review.AccessPolicy accessPolicy in Enum.GetValues(typeof (Review.AccessPolicy)))
        stringList.Add(ApiUtils.GetAccessPolicyFriendlyName(new Review.AccessPolicy?(accessPolicy)));
      return stringList;
    }

    public static List<string> GetAccessPolicyDisplayList(string allowedAccessValues)
    {
      List<string> stringList = new List<string>();
      if (string.IsNullOrEmpty(allowedAccessValues))
        return stringList;
      string str1 = allowedAccessValues;
      char[] separator = new char[1]{ ',' };
      foreach (string str2 in str1.Split(separator, StringSplitOptions.RemoveEmptyEntries))
      {
        Review.AccessPolicy? nullable = ApiUtils.GetAccessPolicy(str2);
        if (!nullable.HasValue)
        {
          try
          {
            nullable = new Review.AccessPolicy?(ApiUtils.GetAccessPolicyBySystemName(str2));
          }
          catch (InvalidEnumArgumentException ex)
          {
            continue;
          }
        }
        stringList.Add(ApiUtils.GetAccessPolicyFriendlyName(nullable));
      }
      return stringList.Count > 0 ? stringList : ApiUtils.GetAccessPolicyDisplayList();
    }

    public static string GetAccessPolicyFriendlyName(Review.AccessPolicy? item)
    {
      Review.AccessPolicy? nullable = item;
      if (nullable.HasValue)
      {
        switch (nullable.GetValueOrDefault())
        {
          case Review.AccessPolicy.ANYONE:
            return "Anyone";
          case Review.AccessPolicy.GROUP:
            return "Group Based";
          case Review.AccessPolicy.PARTICIPANTS:
            return "Participants";
          case Review.AccessPolicy.GROUP_AND_PARTICIPANTS:
            return "Participants and Group Based";
          case Review.AccessPolicy.GROUP_OR_PARTICIPANTS:
            return "Participants or Group Based";
        }
      }
      return string.Empty;
    }

    public static Review.AccessPolicy? GetAccessPolicy(string friendlyName)
    {
      foreach (Review.AccessPolicy accessPolicy in Enum.GetValues(typeof (Review.AccessPolicy)))
      {
        if (string.Equals(ApiUtils.GetAccessPolicyFriendlyName(new Review.AccessPolicy?(accessPolicy)), friendlyName, StringComparison.OrdinalIgnoreCase))
          return new Review.AccessPolicy?(accessPolicy);
      }
      return new Review.AccessPolicy?();
    }

    public static Review.AccessPolicy GetAccessPolicyBySystemName(string systemName)
    {
      foreach (Review.AccessPolicy accessPolicy in Enum.GetValues(typeof (Review.AccessPolicy)))
      {
        string b = accessPolicy.ToString("F");
        if (string.Equals(systemName, b, StringComparison.OrdinalIgnoreCase))
          return accessPolicy;
      }
      throw new InvalidEnumArgumentException(string.Format("Unknown AccessPolicy system name - {0}", (object) systemName));
    }

    public static ApiUtils.AssignmentShortState? GetAssignmentShortState(
      Client.AssignmentState? assignmentState)
    {
      Client.AssignmentState? nullable = assignmentState;
      if (nullable.HasValue)
      {
        switch (nullable.GetValueOrDefault())
        {
          case Client.AssignmentState.NEW:
            return new ApiUtils.AssignmentShortState?(ApiUtils.AssignmentShortState.NEW);
          case Client.AssignmentState.ACTIVE:
            return new ApiUtils.AssignmentShortState?(ApiUtils.AssignmentShortState.ACTIVE);
          case Client.AssignmentState.WAITING_FOR_ANY:
          case Client.AssignmentState.WAITING_FOR_AUTHOR:
          case Client.AssignmentState.WAITING_FOR_FILES:
          case Client.AssignmentState.WAITING_FOR_POKE:
            return new ApiUtils.AssignmentShortState?(ApiUtils.AssignmentShortState.WAITING);
          case Client.AssignmentState.FINISHED_UNTIL_ANY:
          case Client.AssignmentState.FINISHED_UNTIL_AUTHOR:
          case Client.AssignmentState.FINISHED_UNTIL_FILES:
          case Client.AssignmentState.FINISHED_UNTIL_POKE:
            return new ApiUtils.AssignmentShortState?(ApiUtils.AssignmentShortState.APPROVED);
        }
      }
      return new ApiUtils.AssignmentShortState?();
    }

    public static Review.ReviewActivityType? GetReviewActivityType(
      Client.AssignmentState? assignmentState)
    {
      Client.AssignmentState? nullable = assignmentState;
      if (nullable.HasValue)
      {
        switch (nullable.GetValueOrDefault())
        {
          case Client.AssignmentState.WAITING_FOR_ANY:
          case Client.AssignmentState.FINISHED_UNTIL_ANY:
            return new Review.ReviewActivityType?(Review.ReviewActivityType.ANY);
          case Client.AssignmentState.WAITING_FOR_AUTHOR:
          case Client.AssignmentState.FINISHED_UNTIL_AUTHOR:
            return new Review.ReviewActivityType?(Review.ReviewActivityType.AUTHOR);
          case Client.AssignmentState.WAITING_FOR_FILES:
          case Client.AssignmentState.FINISHED_UNTIL_FILES:
            return new Review.ReviewActivityType?(Review.ReviewActivityType.FILE);
          case Client.AssignmentState.WAITING_FOR_POKE:
          case Client.AssignmentState.FINISHED_UNTIL_POKE:
            return new Review.ReviewActivityType?(Review.ReviewActivityType.POKE);
        }
      }
      return new Review.ReviewActivityType?();
    }

    public static string GetAssignmentShortStateFriendlyName(
      ApiUtils.AssignmentShortState? assignmentState)
    {
      ApiUtils.AssignmentShortState? nullable = assignmentState;
      if (nullable.HasValue)
      {
        switch (nullable.GetValueOrDefault())
        {
          case ApiUtils.AssignmentShortState.NEW:
            return "Newly Assigned";
          case ApiUtils.AssignmentShortState.ACTIVE:
            return "Active";
          case ApiUtils.AssignmentShortState.WAITING:
            return "Waiting";
          case ApiUtils.AssignmentShortState.APPROVED:
            return "Approved";
        }
      }
      return string.Empty;
    }

    public static string GetAssignmentStateFriendlyName(Client.AssignmentState? assignmentState)
    {
      if (!assignmentState.HasValue)
        return string.Empty;
      Client.AssignmentState? nullable = assignmentState;
      if (nullable.HasValue)
      {
        switch (nullable.GetValueOrDefault())
        {
          case Client.AssignmentState.NEW:
            return "Newly Assigned";
          case Client.AssignmentState.ACTIVE:
            return "Active Participant";
          case Client.AssignmentState.WAITING_FOR_ANY:
            return "Waiting for any activity";
          case Client.AssignmentState.WAITING_FOR_AUTHOR:
            return "Waiting for author activity";
          case Client.AssignmentState.WAITING_FOR_FILES:
            return "Waiting for file activity";
          case Client.AssignmentState.WAITING_FOR_POKE:
            return "Waiting to be poked";
          case Client.AssignmentState.FINISHED_UNTIL_ANY:
            return "Finished unless any activity occurs";
          case Client.AssignmentState.FINISHED_UNTIL_AUTHOR:
            return "Finished unless activity by author occurs";
          case Client.AssignmentState.FINISHED_UNTIL_FILES:
            return "Finished unless file activity occurs";
          case Client.AssignmentState.FINISHED_UNTIL_POKE:
            return "Finished unless poked";
        }
      }
      return string.Empty;
    }

    public static Client.AssignmentState GetAssignmentState(string assignmentState)
    {
      foreach (Client.AssignmentState assignmentState1 in Enum.GetValues(typeof (Client.AssignmentState)))
      {
        string stateFriendlyName = ApiUtils.GetAssignmentStateFriendlyName(new Client.AssignmentState?(assignmentState1));
        if (string.Equals(assignmentState, stateFriendlyName))
          return assignmentState1;
        string b = assignmentState1.ToString("F");
        if (string.Equals(assignmentState, b))
          return assignmentState1;
      }
      throw new InvalidEnumArgumentException(string.Format("Unknown AssignmentState name - {0}", (object) assignmentState));
    }

    public static List<string> LocatorTypeDisplayList()
    {
      List<string> stringList = new List<string>();
      foreach (Review.LocatorType locatorType in Enum.GetValues(typeof (Review.LocatorType)))
        stringList.Add(ApiUtils.GetLocatorTypeFriendlyName(new Review.LocatorType?(locatorType)));
      return stringList;
    }

    public static string GetLocatorTypeFriendlyName(Review.LocatorType? item)
    {
      Review.LocatorType? nullable = item;
      if (nullable.HasValue)
      {
        switch (nullable.GetValueOrDefault())
        {
          case Review.LocatorType.ANNOTATION:
            return "Annotation";
          case Review.LocatorType.OVERALL:
            return "Overall";
          case Review.LocatorType.DELETED:
            return "Deleted";
          case Review.LocatorType.LABEL:
            return "Label";
          case Review.LocatorType.LINE:
            return "Line";
          case Review.LocatorType.COORDINATE:
            return "Coordinate";
          case Review.LocatorType.CELL:
            return "Cell";
        }
      }
      return string.Empty;
    }

    public static Review.LocatorType GetLocatorType(string friendlyName)
    {
      foreach (Review.LocatorType locatorType in Enum.GetValues(typeof (Review.LocatorType)))
      {
        if (string.Equals(ApiUtils.GetLocatorTypeFriendlyName(new Review.LocatorType?(locatorType)), friendlyName, StringComparison.OrdinalIgnoreCase))
          return locatorType;
      }
      throw new Exception("Unknown locator type");
    }

    public static string GetUserThreadStateFriendlyName(Client.UserThreadState item)
    {
      switch (item)
      {
        case Client.UserThreadState.NO_COMMENT:
          return "";
        case Client.UserThreadState.ACCEPTED:
          return "Accepted";
        case Client.UserThreadState.CHATTING:
          return "Chatting";
        case Client.UserThreadState.DEFECT_FIXED:
          return "Fixed";
        case Client.UserThreadState.DEFECT_EXTERNALIZED:
          return "Externalized";
        case Client.UserThreadState.DEFECT_OPEN:
          return "Open";
        default:
          throw new InvalidEnumArgumentException("Unknown UserThreadState entry", (int) item, typeof (Client.UserThreadState));
      }
    }

    public static string GetArchiveOptionValue(Client.ArchiveOption item)
    {
      switch (item)
      {
        case Client.ArchiveOption.ONLY_REPORT:
          return "onlyreport";
        case Client.ArchiveOption.ALL:
          return "all";
        case Client.ArchiveOption.LATEST_VERSIONS:
          return "latestversions";
        default:
          return "onlyreport";
      }
    }

    public static Client.UserThreadState GetUserThreadState(string userThreadState)
    {
      foreach (Client.UserThreadState userThreadState1 in Enum.GetValues(typeof (Client.UserThreadState)))
      {
        string stateFriendlyName = ApiUtils.GetUserThreadStateFriendlyName(userThreadState1);
        if (string.Equals(userThreadState, stateFriendlyName))
          return userThreadState1;
        string b = userThreadState1.ToString("F");
        if (string.Equals(userThreadState, b))
          return userThreadState1;
      }
      throw new InvalidEnumArgumentException(string.Format("Unknown UserThreadState name - {0}", (object) userThreadState));
    }

    public static Client.ChangeType GetChangeType(string changeType)
    {
      foreach (Client.ChangeType changeType1 in Enum.GetValues(typeof (Client.ChangeType)))
      {
        string b = changeType1.ToString("F");
        if (string.Equals(changeType, b))
          return changeType1;
      }
      throw new InvalidEnumArgumentException(string.Format("Unknown ChangeType name - {0}", (object) changeType));
    }

    public static Client.PublishState GetPublishState(string name)
    {
      foreach (Client.PublishState publishState in Enum.GetValues(typeof (Client.PublishState)))
      {
        string b = publishState.ToString("F");
        if (string.Equals(name, b))
          return publishState;
      }
      throw new InvalidEnumArgumentException(string.Format("Unknown PublishState name - {0}", (object) name));
    }

    public static Review.ReviewActivityType GetReviewActivityType(string reviewActivityType)
    {
      foreach (Review.ReviewActivityType reviewActivityType1 in Enum.GetValues(typeof (Review.ReviewActivityType)))
      {
        string b = reviewActivityType1.ToString("F");
        if (string.Equals(reviewActivityType, b))
          return reviewActivityType1;
      }
      throw new InvalidEnumArgumentException(string.Format("Unknown ReviewActivityType name - {0}", (object) reviewActivityType));
    }

    public enum AssignmentShortState
    {
      NEW,
      ACTIVE,
      WAITING,
      APPROVED,
    }
  }
}
