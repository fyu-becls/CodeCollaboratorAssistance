using System;
using System.Collections.Generic;
using System.Text;

namespace CollabAPI
{
    public interface IReviewService : IBaseService
    {
        void AcceptConversation(int conversationID);

        void AcceptLine(int reviewId, string path, int versionId, int lineNumber);

        void AcceptOverallConversation(int reviewId, string path, int versionId);

        void AcceptReviewConversation(int reviewId);

        void AddRemoteSystemLink(int reviewId, int remoteSystemId, string reference);

        void AssignReviewPool(int reviewId, Review.Assignment assignment);

        void CancelReview(int reviewId);

        int CopyReview(
          int reviewId,
          string title,
          bool copyParticipants,
          bool copyCustomFields,
          bool copyParticipantCustomFields,
          bool copyMaterials,
          bool copyRemoteSystemLinks);

        int CreateDefectAtConversation(
          int conversationId,
          string comment,
          List<Review.CustomField> customFields);

        int CreateLineComment(
          int reviewId,
          int versionId,
          string path,
          int lineNumber,
          string comment);

        int CreateLineDefect(
          int reviewId,
          int versionId,
          string path,
          int lineNumber,
          string defect,
          List<Review.CustomField> customFields);

        int CreateOverallComment(int reviewId, int versionId, string path, string comment);

        int CreateOverallDefect(
          int reviewId,
          int versionId,
          string path,
          string comment,
          List<Review.CustomField> customFields);

        int CreateReviewComment(int reviewId, string comment);

        int CreateReviewDefect(int reviewId, string comment, List<Review.CustomField> customFields);

        void DeleteDefect(int defectId);

        void DeleteReview(int reviewId);

        void EditDefect(int defectId, string comment, List<Review.CustomField> customFields);

        bool EditReview(int reviewId, Review.EditReviewRequest editRequest);

        void ElectronicSignatureDeclineReview(int reviewId, string login, string password);

        void ElectronicSignatureSignReview(int reviewId, string login, string password);

        void FinishReviewPhase(int reviewId, Review.ReviewActivityType activity);

        List<SystemAdmin.RoleSettings> GetAvailableRoles();

        List<string> GetAvailableTemplateNames();

        List<Review.ReviewTemplate> GetAvailableTemplates();

        Review.ReviewTemplate GetDefaultTemplate(List<Review.ReviewTemplate> templates);

        Review.ElectronicSignaturePromptsResponse GetElectronicSignatureButtonPrompts();

        Review.ParticipantResponse GetPossibleParticipants(string groupId);

        List<Review.ReviewRemoteSystemItem> GetRemoteSystemItems(int reviewId);

        List<Review.ReviewChecklistItem> GetReviewChecklistItems(int reviewId);

        Review.ReviewFile GetReviewFile(int reviewId, int fileVersionId);

        Review.ReviewInfo GetReviewInfo(int reviewId);

        Review.ReviewSummary GetReviewSummary(int reviewId, bool active);

        Review.ReviewTemplate GetReviewTemplate(int reviewId);

        new void InvalidateCachedData();

        void MarkConversationRead(int conversationID);

        void MarkDefect(int defectId, Review.DefectMarkType defectMarkType, string externalName);

        void MarkLineRead(int reviewId, string path, int versionId, int lineNumber);

        void MarkOverallConversationRead(int reviewId, string path, int versionId);

        void MarkReviewConversationRead(int reviewId);

        void MoveReviewToAnnotatePhase(int reviewId);

        void PokeParticipants(int reviewId, List<string> participants, List<string> poolParticipants);

        void RefreshRemoteSystemLink(int linkId);

        void RejectReview(int reviewId, string reason);

        void RemoveAssignments(int reviewId, List<Review.Assignment> assignments);

        void RemoveRemoteSystemLink(int linkId);

        void ReopenReview(int reviewId);

        void SetAssignments(int reviewId, List<Review.Assignment> assignments);

        void SetChecklistItemStatus(int reviewId, Review.ReviewChecklistItem checkListItem);

        void SetFileAnnotation(int reviewId, int versionId, string path, string comment);

        void UpdateAssignments(int reviewId, List<Review.Assignment> assignments);

        void WaitOnPhase(int reviewId, Review.ReviewActivityType activity);

        int СreateCommentAtConversation(int conversationId, string comment);

        int СreateReview(string title, int templateId);
    }
}
