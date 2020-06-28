using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
#pragma warning disable 0649 // Expected warnings in JSON classes
namespace CollabAPI
{
    public class User
    {
        //public class getActionItems
        //{
        //    // no arguments needed, runs in context of current user
        //}

        //public class getActionItemsResponse
        //{
        //    public List<ActionItem> actionItems;
        //}

        //public class ActionItem
        //{
        //    public string text;
        //    public string nextActionText;
        //}

        ///* not currently used
        //public class findUserByGuid
        //{
        //    public string guid;
        //}

        //public class findUserByLogin
        //{
        //    public string login;
        //}
        //*/

        //public enum SuggestionType
        //{
        //    UPLOAD,
        //    COMMIT
        //}

        //public class getSuggestedReviews
        //{
        //    public string changelistId;
        //    public SuggestionType suggestionType;
        //    public int maxResults;
        //}

        //public class SuggestedReviewsResponse
        //{
        //    public List<SuggestedReview> suggestedReviews;
        //}

        //public class SuggestedReview
        //{
        //    // This needs getters and setters because we use reviewId with lync
        //    public int reviewId { get; set; }
        //    public bool containsChangelist;
        //    public DateTime lastModified;

        //    // This needs getters and setters because we use displayText with lync
        //    public string displayText { get; set; }
        //}


        public class ActionItem
        {
            public string nextActionText { get; set; }

            public string relativeUrl { get; set; }

            public bool requiresUserAction { get; set; }

            public int reviewId { get; set; }

            public bool reviewNeedsCommit { get; set; }

            public string reviewText { get; set; }

            public string roleText { get; set; }

            public string text { get; set; }
        }

        public class Contributor : User.UserInfo
        {
            public string initials { get; set; }
        }

        public class ClientBuildRequest : User.IClientBuildRequest
        {
            public int clientBuild { get; set; }
        }

        public class ClientGuidRequest : User.IClientGuidRequest
        {
            public string clientGuid { get; set; }
        }

        public class DismissCommitTodoRequest : Review.HasReviewId
        {
        }

        public class GetActionItemsResponse
        {
            public List<User.ActionItem> actionItems { get; set; }
        }

        public class SuggestedReview
        {
            public int reviewId { get; set; }

            public bool containsChangelist { get; set; }

            public DateTime lastModified { get; set; }

            public string displayText { get; set; }
        }

        public class SuggestedReviewsRequest
        {
            public string changelistId { get; set; }

            public int maxResults { get; set; }

            public User.SuggestionType suggestionType { get; set; }
        }

        public class SuggestedReviewsResponse
        {
            public List<User.SuggestedReview> suggestedReviews { get; set; }
        }

        public class UserInfo
        {
            public bool admin { get; set; }

            public string email { get; set; }

            public bool enabled { get; set; }

            public string fullName { get; set; }

            public string guid { get; set; }

            public int id { get; set; }

            public string login { get; set; }

            public string phone { get; set; }
        }

        public class UserListResponse
        {
            public List<User.UserInfo> userList { get; set; }
        }

        public interface IClientBuildRequest
        {
            int clientBuild { get; set; }
        }

        public interface IClientGuidRequest
        {
            string clientGuid { get; set; }
        }

        public enum SuggestionType
        {
            UPLOAD,
            COMMIT,
        }
    }
}
#pragma warning restore 0649 // Expected warnings in JSON classes