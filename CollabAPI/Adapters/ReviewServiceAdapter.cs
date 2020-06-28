// Decompiled with JetBrains decompiler
// Type: SmartBear.Collaborator.Api.Adapters.ReviewServiceAdapter
// Assembly: Collaborator.Api, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9c3e7b13c78c6163
// MVID: 82D41E82-1BF6-4105-8B80-342693CB64ED
// Assembly location: C:\Users\fyu\AppData\Local\Microsoft\VisualStudio\16.0_be238a39\Extensions\gzzm4hy2.wuy\Collaborator.Api.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace CollabAPI
{
  public class ReviewServiceAdapter : BaseServiceAdapter, IReviewService, IBaseService
  {
    private SortedDictionary<int, ReviewServiceAdapter.DataByReviewId> _dataByReviewId = new SortedDictionary<int, ReviewServiceAdapter.DataByReviewId>();
    private int _lastRolesUpdateTime = int.MinValue;
    private const uint _updateIntervalInMsecRoles = 120000;
    private List<SystemAdmin.RoleSettings> _roles;

    public ReviewServiceAdapter(IServer server)
      : base(server)
    {
    }

    private void InvalidateReviewChecklistItems(int reviewId)
    {
      this.InvalidateReviewSummary(reviewId);
    }

    private void InvalidateReviewSummary(int reviewId)
    {
      if (!this._dataByReviewId.ContainsKey(reviewId))
        return;
      ReviewServiceAdapter.DataByReviewId dataByReviewId = this._dataByReviewId[reviewId];
      dataByReviewId.ReviewSummary = (Review.ReviewSummary) null;
      dataByReviewId.LastUpdateTime_ReviewSummary = int.MinValue;
    }

    public override void InvalidateCachedData()
    {
      lock (this.UpdateDataLock)
      {
        foreach (ReviewServiceAdapter.DataByReviewId dataByReviewId in this._dataByReviewId.Values)
          dataByReviewId.Invalidate();
        if (this.Server.Connected)
          return;
        this._lastRolesUpdateTime = int.MinValue;
        this._roles = (List<SystemAdmin.RoleSettings>) null;
      }
    }

    public void AcceptConversation(int conversationID)
    {
      ReviewService.acceptConversation acceptConversation = new ReviewService.acceptConversation();
      acceptConversation.conversationId = conversationID;
      this.SendRequest((object) acceptConversation, true);
    }

    public void AcceptLine(int reviewId, string path, int versionId, int lineNumber)
    {
      ReviewService.acceptLine acceptLine = new ReviewService.acceptLine();
      acceptLine.reviewId = reviewId;
      acceptLine.path = path;
      acceptLine.versionId = versionId;
      acceptLine.lineNumber = lineNumber;
      this.SendRequest((object) acceptLine, true);
    }

    public void AcceptOverallConversation(int reviewId, string path, int versionId)
    {
      ReviewService.acceptOverallConversation overallConversation = new ReviewService.acceptOverallConversation();
      overallConversation.reviewId = reviewId;
      overallConversation.path = path;
      overallConversation.versionId = versionId;
      this.SendRequest((object) overallConversation, true);
    }

    public void AcceptReviewConversation(int reviewId)
    {
      ReviewService.acceptReviewConversation reviewConversation = new ReviewService.acceptReviewConversation();
      reviewConversation.reviewId = reviewId;
      this.SendRequest((object) reviewConversation, true);
    }

    public void AssignReviewPool(int reviewId, Review.Assignment assignment)
    {
      ReviewService.assignReviewPool assignReviewPool = new ReviewService.assignReviewPool();
      assignReviewPool.reviewId = reviewId;
      assignReviewPool.assignment = assignment;
      this.SendRequest((object) assignReviewPool, true);
    }

    public int CreateLineComment(
      int reviewId,
      int versionId,
      string path,
      int lineNumber,
      string comment)
    {
      ReviewService.createLineComment createLineComment = new ReviewService.createLineComment();
      createLineComment.reviewId = reviewId;
      createLineComment.versionId = versionId;
      createLineComment.path = path;
      createLineComment.lineNumber = lineNumber;
      createLineComment.comment = comment;
      JsonResult jsonResult = this.SendRequest((object) createLineComment, true);
      return jsonResult != null ? jsonResult.GetResponse<Review.HasDefectId>().defectId : 0;
    }

    public int CreateOverallComment(int reviewId, int versionId, string path, string comment)
    {
      ReviewService.createOverallComment createOverallComment = new ReviewService.createOverallComment();
      createOverallComment.reviewId = reviewId;
      createOverallComment.versionId = versionId;
      createOverallComment.path = path;
      createOverallComment.comment = comment;
      JsonResult jsonResult = this.SendRequest((object) createOverallComment, true);
      return jsonResult != null ? jsonResult.GetResponse<Review.HasDefectId>().defectId : 0;
    }

    public int CreateReviewComment(int reviewId, string comment)
    {
      ReviewService.createReviewComment createReviewComment = new ReviewService.createReviewComment();
      createReviewComment.reviewId = reviewId;
      createReviewComment.comment = comment;
      JsonResult jsonResult = this.SendRequest((object) createReviewComment, true);
      return jsonResult != null ? jsonResult.GetResponse<Review.HasCommentId>().commentId : 0;
    }

    public int СreateCommentAtConversation(int conversationId, string comment)
    {
      ReviewService.createCommentAtConversation commentAtConversation = new ReviewService.createCommentAtConversation();
      commentAtConversation.conversationId = conversationId;
      commentAtConversation.comment = comment;
      JsonResult jsonResult = this.SendRequest((object) commentAtConversation, true);
      return jsonResult != null ? jsonResult.GetResponse<Review.HasCommentId>().commentId : 0;
    }

    public int CreateDefectAtConversation(
      int conversationId,
      string comment,
      List<Review.CustomField> customFields)
    {
      ReviewService.createDefectAtConversation defectAtConversation = new ReviewService.createDefectAtConversation();
      defectAtConversation.conversationId = conversationId;
      defectAtConversation.comment = comment;
      defectAtConversation.customFields = customFields;
      JsonResult jsonResult = this.SendRequest((object) defectAtConversation, true);
      return jsonResult != null ? jsonResult.GetResponse<Review.HasDefectId>().defectId : 0;
    }

    public int CreateLineDefect(
      int reviewId,
      int versionId,
      string path,
      int lineNumber,
      string defect,
      List<Review.CustomField> customFields)
    {
      ReviewService.createLineDefect createLineDefect = new ReviewService.createLineDefect();
      createLineDefect.reviewId = reviewId;
      createLineDefect.versionId = versionId;
      createLineDefect.path = path;
      createLineDefect.lineNumber = lineNumber;
      createLineDefect.comment = defect;
      createLineDefect.customFields = customFields;
      JsonResult jsonResult = this.SendRequest((object) createLineDefect, true);
      return jsonResult != null ? jsonResult.GetResponse<Review.HasDefectId>().defectId : 0;
    }

    public int CreateOverallDefect(
      int reviewId,
      int versionId,
      string path,
      string comment,
      List<Review.CustomField> customFields)
    {
      ReviewService.createOverallDefect createOverallDefect = new ReviewService.createOverallDefect();
      createOverallDefect.reviewId = reviewId;
      createOverallDefect.versionId = versionId;
      createOverallDefect.path = path;
      createOverallDefect.comment = comment;
      createOverallDefect.customFields = customFields;
      JsonResult jsonResult = this.SendRequest((object) createOverallDefect, true);
      return jsonResult != null ? jsonResult.GetResponse<Review.HasDefectId>().defectId : 0;
    }

    public int CreateReviewDefect(
      int reviewId,
      string comment,
      List<Review.CustomField> customFields)
    {
      ReviewService.createReviewDefect createReviewDefect = new ReviewService.createReviewDefect();
      createReviewDefect.reviewId = reviewId;
      createReviewDefect.comment = comment;
      createReviewDefect.customFields = customFields;
      JsonResult jsonResult = this.SendRequest((object) createReviewDefect, true);
      return jsonResult != null ? jsonResult.GetResponse<Review.HasDefectId>().defectId : 0;
    }

    public void DeleteDefect(int defectId)
    {
      ReviewService.deleteDefect deleteDefect = new ReviewService.deleteDefect();
      deleteDefect.defectId = defectId;
      this.SendRequest((object) deleteDefect, true);
    }

    public void EditDefect(int defectId, string comment, List<Review.CustomField> customFields)
    {
      ReviewService.editDefect editDefect = new ReviewService.editDefect();
      editDefect.defectId = defectId;
      editDefect.comment = comment;
      editDefect.customFields = customFields;
      this.SendRequest((object) editDefect, true);
    }

    public int СreateReview(string title, int templateId)
    {
      ReviewService.createReview createReview = new ReviewService.createReview();
      createReview.creator = this.SessionInfo.UserName;
      createReview.title = title;
      createReview.templateId = new int?(templateId);
      JsonResult response = this.SendRequest((object) createReview, true);
      return response != null ? this.GetResponseReviewId(response) : 0;
    }

    private int GetResponseReviewId(JsonResult response)
    {
      this.Server.UserService.InvalidateCachedData();
      return response.GetResponse<Review.HasReviewId>().reviewId;
    }

    public int CopyReview(
      int reviewId,
      string title,
      bool copyParticipants,
      bool copyCustomFields,
      bool copyParticipantCustomFields,
      bool copyMaterials,
      bool copyRemoteSystemLinks)
    {
      ReviewService.copy copy = new ReviewService.copy();
      copy.reviewId = reviewId;
      copy.title = title;
      copy.copyParticipants = copyParticipants;
      copy.copyCustomFields = copyCustomFields;
      copy.copyParticipantCustomFields = copyParticipantCustomFields;
      copy.copyMaterials = copyMaterials;
      copy.copyRemoteSystemLinks = copyRemoteSystemLinks;
      copy.copySubcriptions = false;
      JsonResult response = this.SendRequest((object) copy, true);
      return response != null ? this.GetResponseReviewId(response) : 0;
    }

    public void DeleteReview(int reviewId)
    {
      lock (this.UpdateDataLock)
      {
        ReviewService.deleteReview deleteReview = new ReviewService.deleteReview();
        deleteReview.reviewId = reviewId;
        this.SendRequest((object) deleteReview, true);
        this._dataByReviewId.Remove(reviewId);
        this.Server.UserService.InvalidateCachedData();
      }
    }

    public void CancelReview(int reviewId)
    {
      ReviewService.cancel cancel = new ReviewService.cancel();
      cancel.reviewId = reviewId;
      this.SendRequest((object) cancel, true);
      this.Server.UserService.InvalidateCachedData();
    }

    public void RejectReview(int reviewId, string reason)
    {
      ReviewService.reject reject = new ReviewService.reject();
      reject.reviewId = reviewId;
      reject.reason = reason;
      this.SendRequest((object) reject, true);
      this.Server.UserService.InvalidateCachedData();
    }

    public void ReopenReview(int reviewId)
    {
      ReviewService.reopen reopen = new ReviewService.reopen();
      reopen.reviewId = reviewId;
      this.SendRequest((object) reopen, true);
      this.Server.UserService.InvalidateCachedData();
    }

    public bool EditReview(int reviewId, Review.EditReviewRequest editRequest)
    {
      try
      {
        editRequest.reviewId = reviewId;
        JsonResult jsonResult = this.SendRequest((object) editRequest, true);
        return jsonResult != null && jsonResult.GetResponse<ReviewService.editReviewResponse>().reviewId == reviewId;
      }
      finally
      {
        this.InvalidateCachedData();
      }
    }

    public Review.ReviewSummary GetReviewSummary(int reviewId, bool active)
    {
      lock (this.UpdateDataLock)
      {
        ReviewServiceAdapter.DataByReviewId dataByReviewId;
        if (this._dataByReviewId.ContainsKey(reviewId))
        {
          dataByReviewId = this._dataByReviewId[reviewId];
          if (dataByReviewId.ReviewSummary != null && !this.NeedToUpdateData(dataByReviewId.LastUpdateTime_ReviewSummary))
            return dataByReviewId.ReviewSummary;
        }
        else
        {
          dataByReviewId = new ReviewServiceAdapter.DataByReviewId();
          this._dataByReviewId.Add(reviewId, dataByReviewId);
        }
        ReviewService.getReviewSummary getReviewSummary = new ReviewService.getReviewSummary();
        getReviewSummary.clientBuild = this.SessionInfo.ClientBuild;
        getReviewSummary.clientGuid = this.SessionInfo.ClientGuid;
        getReviewSummary.reviewId = reviewId;
        getReviewSummary.active = active;
        JsonResult jsonResult = this.SendRequest((object) getReviewSummary, true);
        Review.ReviewSummary reviewSummary = (Review.ReviewSummary) null;
        if (jsonResult != null)
          reviewSummary = jsonResult.GetResponse<Review.ReviewSummary>();
        if (reviewSummary != null)
        {
          dataByReviewId.ReviewSummary = reviewSummary;
          dataByReviewId.LastUpdateTime_ReviewSummary = this.GetUpdateTime();
        }
        return reviewSummary;
      }
    }

    public void FinishReviewPhase(int reviewId, Review.ReviewActivityType activity)
    {
      ReviewService.finishReviewPhase finishReviewPhase = new ReviewService.finishReviewPhase();
      finishReviewPhase.reviewId = reviewId;
      finishReviewPhase.until = activity;
      this.SendRequest((object) finishReviewPhase, true);
    }

    public Review.ReviewInfo GetReviewInfo(int reviewId)
    {
      lock (this.UpdateDataLock)
      {
        ReviewServiceAdapter.DataByReviewId dataByReviewId;
        if (this._dataByReviewId.ContainsKey(reviewId))
        {
          dataByReviewId = this._dataByReviewId[reviewId];
          if (dataByReviewId.ReviewInfo != null && !this.NeedToUpdateData(dataByReviewId.LastUpdateTime_ReviewInfo))
            return dataByReviewId.ReviewInfo;
        }
        else
        {
          dataByReviewId = new ReviewServiceAdapter.DataByReviewId();
          this._dataByReviewId.Add(reviewId, dataByReviewId);
        }
        ReviewService.findReviewById findReviewById = new ReviewService.findReviewById();
        findReviewById.reviewId = reviewId;
        JsonResult jsonResult = this.SendRequest((object) findReviewById, true);
        Review.ReviewInfo reviewInfo = (Review.ReviewInfo) null;
        if (jsonResult != null)
          reviewInfo = jsonResult.GetResponse<Review.ReviewInfo>();
        if (reviewInfo != null)
        {
          dataByReviewId.ReviewInfo = reviewInfo;
          dataByReviewId.LastUpdateTime_ReviewInfo = this.GetUpdateTime();
        }
        return reviewInfo;
      }
    }

    public Review.ReviewFile GetReviewFile(int reviewId, int fileVersionId)
    {
      ReviewService.getReviewFile getReviewFile = new ReviewService.getReviewFile();
      getReviewFile.reviewId = reviewId;
      getReviewFile.versionId = fileVersionId;
      getReviewFile.clientBuild = this.SessionInfo.ClientBuild;
      getReviewFile.clientGuid = this.SessionInfo.ClientGuid;
      getReviewFile.active = true;
      JsonResult jsonResult = this.SendRequest((object) getReviewFile, true);
      Review.ReviewFile reviewFile = (Review.ReviewFile) null;
      if (jsonResult != null)
        reviewFile = jsonResult.GetResponse<Review.ReviewFile>();
      return reviewFile;
    }

    public Review.ReviewTemplate GetReviewTemplate(int reviewId)
    {
      ReviewService.getReviewTemplate getReviewTemplate = new ReviewService.getReviewTemplate();
      getReviewTemplate.clientBuild = this.SessionInfo.ClientBuild;
      getReviewTemplate.reviewId = reviewId;
      JsonResult jsonResult = this.SendRequest((object) getReviewTemplate, true);
      Review.ReviewTemplate reviewTemplate = (Review.ReviewTemplate) null;
      if (jsonResult != null)
        reviewTemplate = jsonResult.GetResponse<Review.ReviewTemplate>();
      return reviewTemplate;
    }

    public List<string> GetAvailableTemplateNames()
    {
      List<Review.ReviewTemplate> availableTemplates = this.GetAvailableTemplates();
      List<string> source = new List<string>();
      foreach (Review.ReviewTemplate reviewTemplate in availableTemplates)
      {
        if (reviewTemplate.active)
          source.Add(reviewTemplate.name);
      }
      if (source.Count == 0)
      {
        foreach (Review.ReviewTemplate reviewTemplate in availableTemplates)
          source.Add(reviewTemplate.name);
      }
      return source.OrderBy<string, string>((Func<string, string>) (s => s)).ToList<string>();
    }

    public List<Review.ReviewTemplate> GetAvailableTemplates()
    {
      ReviewService.getReviewTemplates getReviewTemplates = new ReviewService.getReviewTemplates();
      getReviewTemplates.clientBuild = this.SessionInfo.ClientBuild;
      JsonResult jsonResult = this.SendRequest((object) getReviewTemplates, true);
      List<Review.ReviewTemplate> reviewTemplateList = (List<Review.ReviewTemplate>) null;
      if (jsonResult != null)
      {
        Review.ReviewTemplateResponse response = jsonResult.GetResponse<Review.ReviewTemplateResponse>();
        if (response != null)
          reviewTemplateList = response.reviewTemplates;
      }
      if (reviewTemplateList == null)
        reviewTemplateList = new List<Review.ReviewTemplate>();
      return reviewTemplateList;
    }

    public Review.ReviewTemplate GetDefaultTemplate(List<Review.ReviewTemplate> templates)
    {
      if (templates == null)
        return (Review.ReviewTemplate) null;
      foreach (Review.ReviewTemplate template in templates)
      {
        if (template.usedByDefault)
          return template;
      }
      int minTemplateId = templates.Min<Review.ReviewTemplate>((Func<Review.ReviewTemplate, int>) (t => t.id));
      return templates.Find((Predicate<Review.ReviewTemplate>) (t => t.id == minTemplateId));
    }

    public List<SystemAdmin.RoleSettings> GetAvailableRoles()
    {
      lock (this.UpdateDataLock)
      {
        if (this._roles == null || this.NeedToUpdateData(this._lastRolesUpdateTime, 120000U))
        {
          this._roles = new List<SystemAdmin.RoleSettings>();
          List<Review.ReviewTemplate> availableTemplates = this.GetAvailableTemplates();
          if (availableTemplates != null)
          {
            foreach (Review.ReviewTemplate reviewTemplate in availableTemplates)
            {
              foreach (SystemAdmin.RoleSettings role in reviewTemplate.roles)
              {
                SystemAdmin.RoleSettings templateRole = role;
                if (this._roles.Find((Predicate<SystemAdmin.RoleSettings>) (rs => rs.id == templateRole.id)) == null)
                  this._roles.Add(templateRole);
              }
            }
            this._lastRolesUpdateTime = this.GetUpdateTime();
          }
        }
        return this._roles;
      }
    }

    public List<Review.ReviewChecklistItem> GetReviewChecklistItems(int reviewId)
    {
      lock (this.UpdateDataLock)
        return this.GetReviewSummary(reviewId, true).reviewChecklistItems;
    }

    public void SetChecklistItemStatus(int reviewId, Review.ReviewChecklistItem checkListItem)
    {
      ReviewService.setChecklistItemStatus checklistItemStatus = new ReviewService.setChecklistItemStatus();
      checklistItemStatus.reviewId = reviewId;
      checklistItemStatus.id = checkListItem.id;
      checklistItemStatus.title = checkListItem.title;
      checklistItemStatus.@checked = checkListItem.@checked;
      checklistItemStatus.userInfo = checkListItem.userInfo;
      JsonResult jsonResult = this.SendRequest((object) checklistItemStatus, true);
      if (jsonResult == null && jsonResult.IsError())
        return;
      this.InvalidateReviewChecklistItems(reviewId);
    }

    public void MarkDefect(int defectId, Review.DefectMarkType defectMarkType, string externalName)
    {
      ReviewService.markDefect markDefect = new ReviewService.markDefect();
      markDefect.defectId = defectId;
      markDefect.defectMarkType = defectMarkType;
      markDefect.externalName = externalName;
      this.SendRequest((object) markDefect, true);
    }

    public void MarkConversationRead(int conversationID)
    {
      ReviewService.markConversationRead conversationRead = new ReviewService.markConversationRead();
      conversationRead.conversationId = conversationID;
      this.SendRequest((object) conversationRead, true);
    }

    public void MarkLineRead(int reviewId, string path, int versionId, int lineNumber)
    {
      ReviewService.markLineRead markLineRead = new ReviewService.markLineRead();
      markLineRead.reviewId = reviewId;
      markLineRead.path = path;
      markLineRead.versionId = versionId;
      markLineRead.lineNumber = lineNumber;
      this.SendRequest((object) markLineRead, true);
    }

    public void MarkOverallConversationRead(int reviewId, string path, int versionId)
    {
      ReviewService.markOverallConversationRead conversationRead = new ReviewService.markOverallConversationRead();
      conversationRead.reviewId = reviewId;
      conversationRead.path = path;
      conversationRead.versionId = versionId;
      this.SendRequest((object) conversationRead, true);
    }

    public void MarkReviewConversationRead(int reviewId)
    {
      ReviewService.markReviewConversationRead conversationRead = new ReviewService.markReviewConversationRead();
      conversationRead.reviewId = reviewId;
      this.SendRequest((object) conversationRead, true);
    }

    public void MoveReviewToAnnotatePhase(int reviewId)
    {
      ReviewService.moveReviewToAnnotatePhase reviewToAnnotatePhase = new ReviewService.moveReviewToAnnotatePhase();
      reviewToAnnotatePhase.reviewId = reviewId;
      this.SendRequest((object) reviewToAnnotatePhase, true);
    }

    public void UpdateAssignments(int reviewId, List<Review.Assignment> assignments)
    {
      ReviewService.updateAssignments updateAssignments = new ReviewService.updateAssignments();
      updateAssignments.reviewId = reviewId;
      updateAssignments.assignments = assignments;
      this.SendRequest((object) updateAssignments, true);
    }

    public void RemoveAssignments(int reviewId, List<Review.Assignment> assignments)
    {
      ReviewService.removeAssignments removeAssignments = new ReviewService.removeAssignments();
      removeAssignments.reviewId = reviewId;
      removeAssignments.assignments = assignments;
      this.SendRequest((object) removeAssignments, true);
    }

    public void SetAssignments(int reviewId, List<Review.Assignment> assignments)
    {
      ReviewService.setAssignments setAssignments = new ReviewService.setAssignments();
      setAssignments.reviewId = reviewId;
      setAssignments.assignments = assignments;
      this.SendRequest((object) setAssignments, true);
    }

    public Review.ParticipantResponse GetPossibleParticipants(string groupId)
    {
      ReviewService.getPossibleParticipants possibleParticipants = new ReviewService.getPossibleParticipants();
      possibleParticipants.guid = groupId;
      return this.SendRequest((object) possibleParticipants, true)?.GetResponse<Review.ParticipantResponse>();
    }

    public void WaitOnPhase(int reviewId, Review.ReviewActivityType activity)
    {
      ReviewService.waitOnPhase waitOnPhase = new ReviewService.waitOnPhase();
      waitOnPhase.reviewId = reviewId;
      waitOnPhase.until = activity;
      this.SendRequest((object) waitOnPhase, true);
    }

    public void PokeParticipants(
      int reviewId,
      List<string> participants,
      List<string> poolParticipants)
    {
      ReviewService.pokeParticipants pokeParticipants = new ReviewService.pokeParticipants();
      pokeParticipants.reviewId = reviewId;
      pokeParticipants.participants = participants;
      pokeParticipants.poolParticipants = poolParticipants;
      this.SendRequest((object) pokeParticipants, true);
    }

    public void SetFileAnnotation(int reviewId, int versionId, string path, string comment)
    {
      ReviewService.setFileAnnotation setFileAnnotation = new ReviewService.setFileAnnotation();
      setFileAnnotation.reviewId = reviewId;
      setFileAnnotation.versionId = versionId;
      setFileAnnotation.path = path;
      setFileAnnotation.comment = comment;
      this.SendRequest((object) setFileAnnotation, true);
    }

    public void ElectronicSignatureSignReview(int reviewId, string login, string password)
    {
      ElectronicSignatureService.electronicSignatureSignReview signatureSignReview = new ElectronicSignatureService.electronicSignatureSignReview();
      signatureSignReview.reviewId = reviewId;
      signatureSignReview.login = login;
      signatureSignReview.password = password;
      this.SendRequest((object) signatureSignReview, true);
    }

    public void ElectronicSignatureDeclineReview(int reviewId, string login, string password)
    {
      ElectronicSignatureService.electronicSignatureDeclineReview signatureDeclineReview = new ElectronicSignatureService.electronicSignatureDeclineReview();
      signatureDeclineReview.reviewId = reviewId;
      signatureDeclineReview.login = login;
      signatureDeclineReview.password = password;
      this.SendRequest((object) signatureDeclineReview, true);
    }

    public Review.ElectronicSignaturePromptsResponse GetElectronicSignatureButtonPrompts()
    {
      return this.SendRequest((object) new ElectronicSignatureService.getElectronicSignaturePrompts(), true)?.GetResponse<Review.ElectronicSignaturePromptsResponse>();
    }

    public List<Review.ReviewRemoteSystemItem> GetRemoteSystemItems(int reviewId)
    {
      lock (this.UpdateDataLock)
      {
        List<Review.ReviewRemoteSystemItem> remoteSystemItemList = new List<Review.ReviewRemoteSystemItem>();
        Review.ReviewSummary reviewSummary = this.GetReviewSummary(reviewId, true);
        if (reviewSummary != null && reviewSummary.remoteSystemItems != null && reviewSummary.remoteSystemItems.Count > 0)
          remoteSystemItemList.AddRange((IEnumerable<Review.ReviewRemoteSystemItem>) reviewSummary.remoteSystemItems);
        return remoteSystemItemList;
      }
    }

    public void AddRemoteSystemLink(int reviewId, int remoteSystemId, string reference)
    {
      ReviewService.addRemoteSystemLink remoteSystemLink = new ReviewService.addRemoteSystemLink();
      remoteSystemLink.reviewId = reviewId;
      remoteSystemLink.remoteSystemId = remoteSystemId;
      remoteSystemLink.@ref = reference;
      this.SendRequest((object) remoteSystemLink, true);
    }

    public void RemoveRemoteSystemLink(int linkId)
    {
      ReviewService.removeRemoteSystemLink remoteSystemLink = new ReviewService.removeRemoteSystemLink();
      remoteSystemLink.id = linkId;
      this.SendRequest((object) remoteSystemLink, true);
    }

    public void RefreshRemoteSystemLink(int linkId)
    {
      ReviewService.refreshRemoteSystemLink remoteSystemLink = new ReviewService.refreshRemoteSystemLink();
      remoteSystemLink.id = linkId;
      this.SendRequest((object) remoteSystemLink, true);
    }

    private class DataByReviewId
    {
      public DataByReviewId()
      {
        this.Invalidate();
      }

      public int LastUpdateTime_ReviewSummary { get; set; }

      public Review.ReviewSummary ReviewSummary { get; set; }

      public int LastUpdateTime_ReviewInfo { get; set; }

      public Review.ReviewInfo ReviewInfo { get; set; }

      public int LastUpdateTime_ReviewChecklistItems { get; set; }

      public List<Review.ReviewChecklistItem> ReviewChecklistItems { get; set; }

      public void Invalidate()
      {
        this.ReviewChecklistItems = (List<Review.ReviewChecklistItem>) null;
        this.ReviewInfo = (Review.ReviewInfo) null;
        this.ReviewSummary = (Review.ReviewSummary) null;
        this.LastUpdateTime_ReviewChecklistItems = int.MinValue;
        this.LastUpdateTime_ReviewInfo = int.MinValue;
        this.LastUpdateTime_ReviewSummary = int.MinValue;
      }
    }
  }
}
