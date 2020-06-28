using System;
using System.Collections.Generic;
using System.Text;

namespace CollabAPI
{
    public class Review
    {
        public class Assignment
        {
            public string poolGuid { get; set; }

            public Review.Role role { get; set; }

            public string user { get; set; }
        }

        public class AssignmentsRequest : Review.HasReviewId
        {
            public List<Review.Assignment> assignments { get; set; }
        }

        public class AddFilesRequest : Review.HasReviewId
        {
            public List<Review.ChangelistDescriptor> changelists { get; set; }

            public string zipName { get; set; }

            public bool postCommit { get; set; }

            public bool skipIfExists { get; set; }
        }

        public class AssignReviewPoolRequest : Review.HasReviewId
        {
            public Review.Assignment assignment { get; set; }
        }

        public class AutomaticLink
        {
            public bool caseSensitive { get; set; }

            public string regex { get; set; }

            public string title { get; set; }

            public string urlFormat { get; set; }
        }

        public class BaseChangelist : Review.IHasChangelistId, Review.IHasCommitInfo
        {
            public int changelistId { get; set; }

            public Review.CommitInfo commitInfo { get; set; }

            public Review.ScmToken scmToken { get; set; }
        }

        public class BaseCommentRequest
        {
            public string comment { get; set; }
        }

        public class BaseDefectRequest : Review.BaseCommentRequest
        {
            public List<Review.CustomField> customFields { get; set; }
        }

        public class BaseReview : Review.HasTemplateId
        {
            public Review.AccessPolicy? accessPolicy { get; set; }

            public List<Review.CustomField> customFields { get; set; }

            public DateTime? deadline { get; set; }

            public string groupGuid { get; set; }

            public List<Review.CustomField> internalCustomFields { get; set; }

            public List<Review.CustomField> participantCustomFields { get; set; }

            public List<Review.CustomField> checklistItemCustomFields { get; set; }

            public string templateName { get; set; }

            public string title { get; set; }
        }

        public class BaseVersionDescriptor : Review.HasCommitInfo
        {
            public Review.Action action { get; set; }

            public int changelistCommitId { get; set; }

            public string localPath { get; set; }

            public string md5 { get; set; }

            public string scmPath { get; set; }

            public string scmVersionName { get; set; }

            public Review.Source source { get; set; }

            public int versionId { get; set; }
        }

        public class ChangelistDescriptor : Review.BaseChangelist
        {
            public List<string> scmConnectionParameters { get; set; }

            public Review.BaseVersionDescriptor versionCreated { get; set; }

            public List<Review.VersionDescriptor> versions { get; set; }

            public string zipName { get; set; }
        }

        public class CommitInfo
        {
            public string author { get; set; }

            public string comment { get; set; }

            public DateTime date { get; set; }

            public string hostGuid { get; set; }

            public string scmId { get; set; }

            public bool local { get; set; }
        }

        public class ConversationSummary
        {
            public bool containsUserComment { get; set; }

            public int firstEntryId { get; set; }

            public Review.Locator locator { get; set; }

            public Review.Locator originLocator { get; set; }

            public int originVersionId { get; set; }

            public List<Review.ParticipantState> participantStates { get; set; }

            public string state { get; set; }

            public bool unread { get; set; }
        }

        public class CreateReviewRequest : Review.BaseReview
        {
            public string creator { get; set; }
        }

        public class CopyReviewRequest : Review.HasReviewId
        {
            public string title { get; set; }

            public bool copyParticipants { get; set; }

            public bool copyCustomFields { get; set; }

            public bool copyParticipantCustomFields { get; set; }

            public bool copyRemoteSystemLinks { get; set; }

            public bool copySubcriptions { get; set; }

            public bool copyMaterials { get; set; }
        }

        public class CustomField
        {
            public string name { get; set; }

            public List<string> value { get; set; }

            public string user { get; set; }

            public int reviewChecklistItemId { get; set; }
        }

        public class DefectLogEntry
        {
            public Review.DefectSummary defectSummary { get; set; }

            public string filePath { get; set; }

            public int fileVersionId { get; set; }

            public Review.Locator locator { get; set; }

            public int originalFileVersionId { get; set; }

            public Review.Locator originLocator { get; set; }
        }

        public class DeleteDefectRequest : Review.HasDefectId
        {
        }

        public class DeleteReviewRequest : Review.HasReviewId
        {
        }

        public class RejectReviewRequest : Review.HasReviewId
        {
            public string reason { get; set; }
        }

        public class ConversationCommentRequest : Review.BaseCommentRequest, Review.IConversationLocation, Review.ILocation
        {
            public int conversationId { get; set; }
        }

        public class ConversationDefectRequest : Review.BaseDefectRequest, Review.IConversationLocation, Review.ILocation
        {
            public int conversationId { get; set; }
        }

        public class CommentSummary : Review.CommentInfo
        {
            public User.Contributor creator { get; set; }

            public int creatorId { get; set; }

            public bool markedRead { get; set; }

            public string publishState { get; set; }

            public string whyNotAllowedToRedact { get; set; }
        }

        public class CommentInfo : Review.IHasCommentId, Review.IHasReviewId
        {
            public int reviewId { get; set; }

            public int commentId { get; set; }

            public int conversationId { get; set; }

            public string creationDate { get; set; }

            public SortedDictionary<string, string> creatorInfo { get; set; }

            public string location { get; set; }

            public string text { get; set; }

            public string type { get; set; }
        }

        public class Conversation : Review.ConversationLocation
        {
            public List<Review.CommentSummary> comments { get; set; }

            public List<Review.DefectSummary> defects { get; set; }

            public List<Review.LocatorByVersion> locators { get; set; }

            public Review.Locator originLocator { get; set; }

            public string originVersionContentMD5 { get; set; }

            public int originVersionId { get; set; }

            public string userThreadState { get; set; }

            public string whyNotAllowedToAccept { get; set; }
        }

        public class ConversationLocation : Review.Location
        {
            public int conversationId { get; set; }
        }

        public class ConversationProvider
        {
            public bool allowsNewConversations { get; set; }

            public List<User.Contributor> contributors { get; set; }

            public string name { get; set; }

            public Review.Conversation overallConversation { get; set; }

            public int refreshInterval { get; set; }

            public int reviewId { get; set; }

            public string whyNotAllowedToCreateComment { get; set; }

            public string whyNotAllowedToCreateDefect { get; set; }

            public string whyNotAllowedToMarkRead { get; set; }

            public string getWhyNotAllowedToUploadContent { get; set; }
        }

        public class DefectSummary : Review.DefectInfo
        {
            public User.Contributor creator { get; set; }

            public int creatorId { get; set; }

            public List<SystemAdmin.CustomFieldSettingsTarget> customFieldValue { get; set; }

            public string whyNotAllowedToDelete { get; set; }

            public string whyNotAllowedToModify { get; set; }
        }

        public class DefectInfo : Review.IHasDefectId, Review.IHasReviewId
        {
            public int defectId { get; set; }

            public int reviewId { get; set; }

            public string creationDate { get; set; }

            public SortedDictionary<string, string> creatorInfo { get; set; }

            public string externalName { get; set; }

            public string location { get; set; }

            public string name { get; set; }

            public string state { get; set; }

            public string text { get; set; }

            public SortedDictionary<string, string> userDefinedFields { get; set; }
        }

        public class EditDefectRequest : Review.BaseDefectRequest, Review.IHasDefectId
        {
            public int defectId { get; set; }
        }

        public class EditReviewRequest : Review.BaseReview, Review.IHasReviewId
        {
            public int reviewId { get; set; }
        }

        public class HasChangelistId : Review.IHasChangelistId
        {
            public int changelistId { get; set; }
        }

        public class HasCommitInfo : Review.IHasCommitInfo
        {
            public Review.CommitInfo commitInfo { get; set; }
        }

        public class HasReviewId : Review.IHasReviewId
        {
            public int reviewId { get; set; }
        }

        public class HasTemplateId : Review.IHasTemplateId
        {
            public int? templateId { get; set; }
        }

        public class HasDefectId : Review.IHasDefectId
        {
            public int defectId { get; set; }
        }

        public class HasCommentId : Review.IHasCommentId
        {
            public int commentId { get; set; }
        }

        public class HasVersionId : Review.IHasVersionId
        {
            public int versionId { get; set; }
        }

        public class LineCommentRequest : Review.BaseCommentRequest, Review.ILineLocation, Review.IVersionLocation, Review.IReviewLocation, Review.IHasReviewId, Review.ILocation
        {
            public int lineNumber { get; set; }

            public string path { get; set; }

            public int versionId { get; set; }

            public int reviewId { get; set; }
        }

        public class LineDefectRequest : Review.BaseDefectRequest, Review.ILineLocation, Review.IVersionLocation, Review.IReviewLocation, Review.IHasReviewId, Review.ILocation
        {
            public int lineNumber { get; set; }

            public string path { get; set; }

            public int versionId { get; set; }

            public int reviewId { get; set; }
        }

        public class LineLocation : Review.VersionLocation
        {
            public int lineNumber { get; set; }
        }

        public class Location : Review.ILocation
        {
        }

        public class LocatorByVersion
        {
            public Review.Locator locator { get; set; }

            public int versionId { get; set; }
        }

        public class Locator
        {
            public string label { get; set; }

            public int lineNumber { get; set; }

            public int page { get; set; }

            public int pinNumber { get; set; }

            public string type { get; set; }

            public int x { get; set; }

            public int y { get; set; }
        }

        public class MarkDefectRequest : Review.HasDefectId
        {
            public Review.DefectMarkType defectMarkType { get; set; }

            public string externalName { get; set; }
        }

        public class MoveReviewToAnnotatePhaseRequest : Review.HasReviewId
        {
        }

        public class ParticipantResponse
        {
            public Group.GroupListResponse groupListResponse { get; set; }

            public List<User.UserInfo> userInfos { get; set; }
        }

        public class ParticipantState
        {
            public int reviewParticipantId { get; set; }

            public string state { get; set; }
        }

        public class PokeParticipantsRequest : Review.HasReviewId
        {
            public List<string> participants;
            public List<string> poolParticipants;
        }

        public class ElectronicSignatureRequest : Review.HasReviewId, Review.ILoginTicket
        {
            public string login { get; set; }

            public string password { get; set; }
        }

        public class PreviousVersionDescriptor : Review.BaseVersionDescriptor
        {
        }

        public class RemoteSystemLinksRequest : Review.HasReviewId
        {
            public int id { get; set; }

            public int remoteSystemId { get; set; }

            public string @ref { get; set; }
        }

        public class ReviewRemoteSystem
        {
            public int id { get; set; }

            public string title { get; set; }

            public Review.ReviewRemoteSystemType type { get; set; }

            public ReviewRemoteSystem()
            {
            }

            public ReviewRemoteSystem(SystemAdmin.RemoteSystem remoteSystem)
            {
                this.id = remoteSystem.id;
                this.title = remoteSystem.title;
                this.type = remoteSystem.type;
            }

            public override string ToString()
            {
                return this.title;
            }
        }

        public class ReviewRemoteSystemItem
        {
            public int id { get; set; }

            public int reviewId { get; set; }

            public string title { get; set; }

            public string status { get; set; }

            public string url { get; set; }

            public Review.ReviewRemoteSystem remoteSystem { get; set; }
        }

        public class ReviewChecklistItem
        {
            public string dateModified { get; set; }

            public int id { get; set; }

            public int reviewId { get; set; }

            public string title { get; set; }

            public User.UserInfo userInfo { get; set; }

            public bool @checked { get; set; }

            public List<SystemAdmin.CustomFieldSettingsTarget> customFieldValue { get; set; }
        }

        public class ReviewChecklistItemsResponse
        {
            public List<Review.ReviewChecklistItem> reviewChecklistItems { get; set; }
        }

        public class ReviewCommentRequest : Review.BaseCommentRequest, Review.IReviewLocation, Review.IHasReviewId, Review.ILocation
        {
            public int reviewId { get; set; }
        }

        public class ReviewDefectRequest : Review.BaseDefectRequest, Review.IReviewLocation, Review.IHasReviewId, Review.ILocation
        {
            public int reviewId { get; set; }
        }

        public class ReviewLocation : Review.HasReviewId, Review.ILocation
        {
        }

        public class ReviewPhaseRequest : Review.HasReviewId
        {
            public Review.ReviewActivityType until { get; set; }
        }

        public class ReviewTemplate
        {
            public List<Review.AutomaticLink> automaticLinks { get; set; }

            public int id { get; set; }

            public string name { get; set; }

            public List<SystemAdmin.CustomFieldSettings> reviewCustomFields { get; set; }

            public List<SystemAdmin.CustomFieldSettings> participantCustomFields { get; set; }

            public List<SystemAdmin.CustomFieldSettings> checklistCustomFields { get; set; }

            public List<SystemAdmin.CustomFieldSettings> defectCustomFields { get; set; }

            public List<SystemAdmin.RoleSettings> roles { get; set; }

            public string description { get; set; }

            public bool active { get; set; }

            public bool remoteSystemLinksEnabled { get; set; }

            public bool usedByDefault { get; set; }

            public override string ToString()
            {
                return this.active ? this.name : string.Format("Disabled template", (object)this.name);
            }

            public override bool Equals(object obj)
            {
                return obj != null && !(this.GetType() != obj.GetType()) && this.id == ((Review.ReviewTemplate)obj).id;
            }

            public override int GetHashCode()
            {
                return this.id;
            }
        }

        public class ReviewTemplateRequest : Review.IHasReviewId, User.IClientBuildRequest
        {
            public int reviewId { get; set; }

            public int clientBuild { get; set; }
        }

        public class ReviewTemplateResponse
        {
            public List<Review.ReviewTemplate> reviewTemplates { get; set; }
        }

        public class ReviewGeneralInfo
        {
            public string completionDate { get; set; }

            public string creationDate { get; set; }

            public List<SystemAdmin.CustomFieldSettingsTarget> customFieldValue { get; set; }

            public Group.GroupDescription group { get; set; }

            public string rejectReason { get; set; }

            public string reviewAccess { get; set; }

            public Review.ReviewTemplate reviewTemplate { get; set; }

            public string title { get; set; }

            public string whyNotAllowedToAddReviewPoolParticipant { get; set; }

            public string whyNotAllowedToCancel { get; set; }

            public string whyNotAllowedToModify { get; set; }

            public string whyNotAllowedToModifyParticipants { get; set; }

            public string whyNotAllowedToReject { get; set; }

            public string whyNotAllowedToArchive { get; set; }

            public bool isAllowedArchiveReviewInAnyPhase { get; set; }
        }

        public class ReviewParticipantsResponse
        {
            public List<Review.Assignment> assignments { get; set; }
        }

        public class ReviewInfo : Review.BaseReview, Review.IHasReviewId
        {
            public int reviewId { get; set; }

            public DateTime creationDate { get; set; }

            public string displayText { get; set; }

            public DateTime lastActivity { get; set; }

            public Review.ReviewPhase reviewPhase { get; set; }
        }

        public class ReviewFindByIdRequest : Review.HasReviewId
        {
        }

        public class ReviewFile : Review.ConversationProvider
        {
            public List<Review.AutomaticLink> automaticLinks { get; set; }

            public string changeType { get; set; }

            public List<Review.Conversation> conversations { get; set; }

            public int defaultPrevVersionId { get; set; }

            public int defaultVersionId { get; set; }

            public List<SystemAdmin.CustomFieldSettings> defectCustomFields { get; set; }

            public List<Review.ReviewVersion> reviewPathVersionIds { get; set; }

            public Review.Scm scm { get; set; }

            public List<Review.Version> versions { get; set; }
        }

        public class ReviewVersion
        {
            public Review.ReviewVersion baseVersion { get; set; }

            public int id { get; set; }
        }

        public class ReviewFileRequest : Review.ReviewSummaryRequest
        {
            public int versionId { get; set; }
        }

        public class ReviewMovingOn
        {
            public string authorRoleName;
            public string explanaition;
            public Review.ReviewPhase phase;
            public Review.ReviewSummary reviewSummary;
            public string signatureStatus;
            public string whyNotAllowedToAnnotate;
            public SortedDictionary<string, string> whyNotAllowedToFinish;
            public SortedDictionary<string, string> whyNotAllowedToWait;
            public bool authorRole;
            public bool needsSignatures;
        }

        public class ReviewParticipant
        {
            public string assignmentState { get; set; }

            public List<SystemAdmin.CustomFieldSettingsTarget> customFieldValue { get; set; }

            public string displayName { get; set; }

            public Group.GroupDescription poolGroup { get; set; }

            public SystemAdmin.RoleSettings role { get; set; }

            public string signatureStatus { get; set; }

            public User.Contributor user { get; set; }

            public string whyNotAllowedToAccess { get; set; }

            public string whyNotAllowedToChangeRole { get; set; }

            public string whyNotAllowedToPoke { get; set; }

            public string whyNotAllowedToRemove { get; set; }

            public string whyNotAllowedToTake { get; set; }
        }

        public class ReviewSummaryRequest : Review.IHasReviewId, User.IClientBuildRequest, User.IClientGuidRequest
        {
            public int reviewId { get; set; }

            public int clientBuild { get; set; }

            public string clientGuid { get; set; }

            public bool active { get; set; }

            public SortedDictionary<string, string> consolidationMethods { get; set; }

            public string updateToken { get; set; }
        }

        public class ReviewSummary : Review.ConversationProvider
        {
            public List<Review.DefectLogEntry> defectLogEntrys { get; set; }

            public Review.ReviewGeneralInfo generalInfo { get; set; }

            public Review.ReviewMovingOn reviewMovingOn { get; set; }

            public List<Review.ReviewParticipant> reviewParticipants { get; set; }

            public List<Review.ScmMaterials> scmMaterials { get; set; }

            public bool commitTodo { get; set; }

            public List<Review.ReviewChecklistItem> reviewChecklistItems { get; set; }

            public List<Review.ReviewRemoteSystemItem> remoteSystemItems { get; set; }
        }

        public class ReviewSummaryChangelist
        {
            public List<string> authors { get; set; }

            public int changeListId { get; set; }

            public string date { get; set; }

            public string description { get; set; }

            public string name { get; set; }

            public List<Review.ReviewSummaryFile> reviewSummaryFiles { get; set; }

            public string scmId { get; set; }
        }

        public class ReviewSummaryFile
        {
            public string annotation { get; set; }

            public string changeType { get; set; }

            public List<Review.ConversationSummary> conversationSummarys { get; set; }

            public string fileSource { get; set; }

            public int latestVersionId { get; set; }

            public int linesAdded { get; set; }

            public int linesDeleted { get; set; }

            public int linesModified { get; set; }

            public string localFilePath { get; set; }

            public int numReworks { get; set; }

            public string path { get; set; }

            public string statusText { get; set; }
        }

        public class Scm
        {
            public string displayName { get; set; }

            public string token { get; set; }
        }

        public class ScmMaterials : Review.Scm
        {
            public Review.ReviewSummaryChangelist consolidatedChangelist { get; set; }

            public List<int> consolidatedChangelistIds { get; set; }

            public string defaultConsolidationMethod { get; set; }

            public string id { get; set; }

            public List<Review.ReviewSummaryChangelist> reviewSummaryChangelists { get; set; }
        }

        public class Version : Review.BaseVersionDescriptor
        {
            public string author { get; set; }

            public string changeType { get; set; }

            public string comment { get; set; }

            public string date { get; set; }

            public string fileType { get; set; }

            public int id { get; set; }

            public string name { get; set; }

            public int numPages { get; set; }

            public string path { get; set; }

            public string title { get; set; }

            public string versionType { get; set; }
        }

        public class VersionCommentRequest : Review.BaseCommentRequest, Review.IVersionLocation, Review.IReviewLocation, Review.IHasReviewId, Review.ILocation
        {
            public string path { get; set; }

            public int versionId { get; set; }

            public int reviewId { get; set; }
        }

        public class VersionDefectRequest : Review.BaseDefectRequest, Review.IVersionLocation, Review.IReviewLocation, Review.IHasReviewId, Review.ILocation
        {
            public string path { get; set; }

            public int versionId { get; set; }

            public int reviewId { get; set; }
        }

        public class VersionDescriptor : Review.BaseVersionDescriptor
        {
            public Review.PreviousVersionDescriptor baseVersion { get; set; }
        }

        public class VersionLocation : Review.ReviewLocation
        {
            public string path { get; set; }

            public int versionId { get; set; }
        }

        public class ElectronicSignaturePromptsResponse
        {
            public string declinePrompt { get; set; }

            public string signPrompt { get; set; }
        }

        public interface IConversationLocation : Review.ILocation
        {
            int conversationId { get; set; }
        }

        public interface IHasChangelistId
        {
            int changelistId { get; set; }
        }

        public interface IHasCommitInfo
        {
            Review.CommitInfo commitInfo { get; set; }
        }

        public interface IHasReviewId
        {
            int reviewId { get; set; }
        }

        public interface IHasVersionId
        {
            int versionId { get; set; }
        }

        public interface ILoginTicket
        {
            string login { get; set; }

            string password { get; set; }
        }

        public interface IHasTemplateId
        {
            int? templateId { get; set; }
        }

        public interface IHasDefectId
        {
            int defectId { get; set; }
        }

        public interface IHasCommentId
        {
            int commentId { get; set; }
        }

        public interface ILineLocation : Review.IVersionLocation, Review.IReviewLocation, Review.IHasReviewId, Review.ILocation
        {
            int lineNumber { get; set; }
        }

        public interface ILocation
        {
        }

        public interface IVersionLocation : Review.IReviewLocation, Review.IHasReviewId, Review.ILocation
        {
            string path { get; set; }

            int versionId { get; set; }
        }

        public interface IReviewLocation : Review.IHasReviewId, Review.ILocation
        {
        }

        public enum AccessPolicy
        {
            ANYONE,
            GROUP,
            PARTICIPANTS,
            GROUP_AND_PARTICIPANTS,
            GROUP_OR_PARTICIPANTS,
        }

        public enum Action
        {
            UNKNOWN,
            ADDED,
            MODIFIED,
            DELETED,
            REVERTED,
            ADDEDDIRECTORY,
            INCLUDED,
            EXCLUDED,
            INCLUDED_DIR_ONLY,
            INCLUDED_DIR,
            EXCLUDED_DIR,
            UPLOADED,
            BRANCHED,
            INTEGRATED,
        }

        public enum DefectMarkType
        {
            FIXED,
            OPEN,
            EXTERNAL,
            NOT_EXTERNAL,
        }

        public enum ReviewActivityType
        {
            ANY,
            AUTHOR,
            FILE,
            POKE,
        }

        public enum LocatorType
        {
            ANNOTATION,
            OVERALL,
            DELETED,
            LABEL,
            LINE,
            COORDINATE,
            CELL,
        }

        public enum ReviewPhase
        {
            PLANNING,
            INSPECTING,
            REWORK,
            COMPLETED,
            CANCELLED,
            REJECTED,
            ANNOTATING,
        }

        public enum ReviewRemoteSystemType
        {
            WORKITEM,
            PR,
            BUILD,
            NONE,
        }

        public enum Role
        {
            AUTHOR,
            REVIEWER,
            OBSERVER,
            MODERATOR,
            READER,
            TESTER,
        }

        public enum ScmToken
        {
            NONE,
            SUBVERSION,
            CVS,
            PERFORCE,
            CLEARCASE,
            TFS,
            VSS,
            CMVC,
            ACCUREV,
            STARTEAM,
            MERCURIAL,
            GIT,
            SYNERGY,
            RTC,
            SURROUND,
            MKS,
            VAULT,
        }

        public enum Source
        {
            UNKNOWN,
            LOCAL,
            CHECKEDIN,
            URL,
            PRINTED,
        }
    }
}