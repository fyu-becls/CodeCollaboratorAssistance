using Blazored.SessionStorage;
using CollabAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeCollaboratorAssistance.Data
{
    public class CodeReviewService
    {
        private readonly ISessionStorageService _sessionService;
        private readonly ServerService _serverService;
        public CodeReviewService(ServerService serverService, ISessionStorageService sessionService)
        {
            _serverService = serverService;
            _sessionService = sessionService;
        }

        public async Task<List<User.SuggestedReview>> GetSugggestedReviewsAsync()
        {            
            return (await _serverService.GetCollaboratorServer())?.UserService.GetSuggestedReviews().suggestedReviews;
            //var api = new CodeCollabAPI(_serverService.ServerUrl, user, ticket);
            //return api.getSuggestedReviews().GetResponse<User.SuggestedReviewsResponse>().suggestedReviews;
        }


        public async Task<Review.ReviewSummary> GetReviewSummarysAsync(int id)
        {
            var server = await _serverService.GetCollaboratorServer();
            return server?.ReviewService.GetReviewSummary(id, true);

            //var api = new CodeCollabAPI(_serverService.ServerUrl, user, ticket);
            //return api.getReviewSummary(id).GetResponse<Review.ReviewSummary>();
        }
    }
}
