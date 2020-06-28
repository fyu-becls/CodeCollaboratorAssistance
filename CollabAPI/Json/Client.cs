// Decompiled with JetBrains decompiler
// Type: SmartBear.Collaborator.Api.Json.Client
// Assembly: Collaborator.Api, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9c3e7b13c78c6163
// MVID: 82D41E82-1BF6-4105-8B80-342693CB64ED
// Assembly location: C:\Users\fyu\AppData\Local\Microsoft\VisualStudio\16.0_be238a39\Extensions\gzzm4hy2.wuy\Collaborator.Api.dll

namespace CollabAPI
{
  public class Client
  {
    public enum ActionItemType
    {
      REVIEW_COMPLETE,
      REVIEW_CANCELED,
      REVIEW_REJECTED,
      REVIEW_WAITING_FOR_COMMENT,
      REVIEW_WAITING_FOR_REWORK,
      REVIEW_WAITING_FOR_REVIEWERS,
      REVIEW_NEEDS_COMMIT,
      REVIEW_IN_PLANNING,
      REVIEW_ANNOTATING_REQUEST,
      REVIEW_ANNOTATING_RESPOND,
      REVIEW_POOL_REQUEST,
      REVIEW_POOL_RESPOND,
      REVIEW_IN_REWORK,
      REVIEW_IN_PROGRESS_RESPOND,
      REVIEW_IN_PROGRESS_ATTACK,
      USER_NOT_CONFIGURED,
      REVIEW_NEEDS_SIGNATURE,
      REVIEW_WAITING_SIGNATURES,
    }

    public enum AssignmentState
    {
      NEW,
      ACTIVE,
      WAITING_FOR_ANY,
      WAITING_FOR_AUTHOR,
      WAITING_FOR_FILES,
      WAITING_FOR_POKE,
      FINISHED_UNTIL_ANY,
      FINISHED_UNTIL_AUTHOR,
      FINISHED_UNTIL_FILES,
      FINISHED_UNTIL_POKE,
      REWORKED_UNTIL_ANY,
      REWORKED_UNTIL_AUTHOR,
      REWORKED_UNTIL_FILES,
      REWORKED_UNTIL_POKE,
      COMPLETED,
    }

    public enum ChangeType
    {
      UNKNOWN,
      MODIFIED,
      ADDED,
      DELETED,
      BRANCHED,
      INTEGRATED,
      UPLOADED,
      REVERTED,
      ADDEDDIRECTORY,
      INCLUDED,
      INCLUDED_DIR,
      INCLUDED_DIR_ONLY,
      EXCLUDED,
      EXCLUDED_DIR,
    }

    public enum CommentType
    {
      USER_COMMENT,
      FILE_ANNOTATION,
      ACCEPT,
      MARK_READ,
      FILE_UPLOAD,
      FILES_REMOVED,
      DEFECT_NEW,
      DEFECT_CHANGE,
      DEFECT_DELETE,
      DEFECT_MARK_FIXED,
      DEFECT_MARK_OPEN,
      DEFECT_MARK_EXTERNAL,
      DEFECT_MARK_NOT_EXTERNAL,
      CHECKLIST_ITEM_CHECKED,
      CHECKLIST_ITEM_UNCHECKED,
      SIGNED,
      DECLINED_TO_SIGN,
      COMMENT_MOVED,
    }

    public enum DefectState
    {
      OPEN,
      FIXED,
      EXTERNAL,
    }

    public enum PublishState
    {
      SENT,
      REDACTED,
    }

    public enum ScmConsolidationMethod
    {
      CONSOLIDATE_NONE,
      CONSOLIDATE_ALL,
      CONSOLIDATE_NONATOMIC,
    }

    public enum UserThreadState
    {
      NO_COMMENT,
      ACCEPTED,
      CHATTING,
      DEFECT_FIXED,
      DEFECT_EXTERNALIZED,
      DEFECT_OPEN,
    }

    public enum ArchiveOption
    {
      ONLY_REPORT,
      ALL,
      LATEST_VERSIONS,
    }
  }
}
