using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace CollabAPI
{
    public static class ReviewService
    {
        public class acceptConversation : Review.ConversationLocation
        {
        }

        public class acceptOverallConversation : Review.VersionLocation
        {
        }

        public class acceptReviewConversation : Review.ReviewLocation
        {
        }

        public class acceptLine : Review.LineLocation
        {
        }

        public class addFiles : Review.AddFilesRequest
        {
        }

        public class assignReviewPool : Review.AssignReviewPoolRequest
        {
        }

        public class createCommentAtConversation : Review.ConversationCommentRequest
        {
        }

        public class createDefectAtConversation : Review.ConversationDefectRequest
        {
        }

        public class createLineComment : Review.LineCommentRequest
        {
        }

        public class createLineDefect : Review.LineDefectRequest
        {
        }

        public class createReview : Review.CreateReviewRequest
        {
        }

        public class copy : Review.CopyReviewRequest
        {
        }

        public class createReviewComment : Review.ReviewCommentRequest
        {
        }

        public class createOverallComment : Review.VersionCommentRequest
        {
        }

        public class createOverallDefect : Review.VersionDefectRequest
        {
        }

        public class createReviewDefect : Review.ReviewDefectRequest
        {
        }

        public class deleteDefect : Review.DeleteDefectRequest
        {
        }

        public class cancel : Review.HasReviewId
        {
        }

        public class reject : Review.RejectReviewRequest
        {
        }

        public class reopen : Review.HasReviewId
        {
        }

        public class deleteReview : Review.DeleteReviewRequest
        {
        }

        public class editDefect : Review.EditDefectRequest
        {
        }

        public class editReview : Review.EditReviewRequest
        {
        }

        public class editReviewResponse : Review.HasReviewId
        {
        }

        public class findReviewById : Review.ReviewFindByIdRequest
        {
        }

        public class findReviewsByCustomFieldValue : Review.ReviewsFindByCustomFieldValueRequest
        {

        }

        public class findReviews : Review.ReviewsFindByTextRequest
        {

        }

        public class finishReviewPhase : Review.ReviewPhaseRequest
        {
        }

        public class getReviewChecklistItems : Review.HasReviewId
        {
        }

        public class setChecklistItemStatus : Review.ReviewChecklistItem
        {
        }

        public class getReviewSummary : Review.ReviewSummaryRequest
        {
        }

        public class getReviewFile : Review.ReviewFileRequest
        {
        }

        public class getReviewTemplate : Review.ReviewTemplateRequest
        {
        }

        public class getReviewTemplates : User.ClientBuildRequest
        {
        }

        public class markDefect : Review.MarkDefectRequest
        {
        }

        public class markConversationRead : Review.ConversationLocation
        {
        }

        public class markLineRead : Review.LineLocation
        {
        }

        public class markOverallConversationRead : Review.VersionLocation
        {
        }

        public class markReviewConversationRead : Review.ReviewLocation
        {
        }

        public class updateAssignments : Review.AssignmentsRequest
        {
        }

        public class removeAssignments : Review.AssignmentsRequest
        {
        }

        public class setAssignments : Review.AssignmentsRequest
        {
        }

        public class getPossibleParticipants : Group.HasGuid
        {
        }

        public class moveReviewToAnnotatePhase : Review.MoveReviewToAnnotatePhaseRequest
        {
        }

        public class waitOnPhase : Review.ReviewPhaseRequest
        {
        }

        public class pokeParticipants : Review.PokeParticipantsRequest
        {
        }

        public class setFileAnnotation : Review.VersionCommentRequest
        {
        }

        public class addRemoteSystemLink : Review.RemoteSystemLinksRequest
        {
        }

        public class removeRemoteSystemLink : Review.RemoteSystemLinksRequest
        {
        }

        public class refreshRemoteSystemLink : Review.RemoteSystemLinksRequest
        {
        }
    }
}
