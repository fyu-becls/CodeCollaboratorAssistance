using System;
using System.Collections.Generic;
using System.Text;

namespace CollabAPI
{
    public interface IUserService : IBaseService
    {
        void DismissCommitTodo(int reviewId);

        List<User.ActionItem> GetActionItems();

        User.SuggestedReviewsResponse GetSuggestedReviews(
          User.SuggestionType suggestionType = User.SuggestionType.UPLOAD,
          string changelistId = "",
          int maxResults = 100);

        User.UserInfo GetUserInfo();

        List<User.UserInfo> GetUserList(bool forceUpdate = false);
    }
}
