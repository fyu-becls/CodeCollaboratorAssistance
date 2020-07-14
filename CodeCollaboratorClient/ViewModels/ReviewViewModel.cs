using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CodeCollaboratorClient.Messages;
using CollabAPI;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace CodeCollaboratorClient.ViewModels
{
    public class ReviewViewModel : ObservableObject
    {
        private GlobalParameters _globalParameters;
        public ReviewViewModel(GlobalParameters globalParameters)
        {
            _globalParameters = globalParameters;
            SearchedReviewId = _globalParameters.ReviewId;

            if (!string.IsNullOrWhiteSpace(ReviewId) && _globalParameters.Server != null)
            {
                try
                {
                    Reload(int.Parse(ReviewId));
                }
                catch (Exception e)
                {
                    IsLoaded = false;
                }
            }
        }

        public string SearchedReviewId { get; set; }

        private bool _isLoaded;
        public bool IsLoaded
        {
            get => _isLoaded;
            set => Set(ref _isLoaded, value);
        }

        public GlobalParameters GlobalParameters => _globalParameters;

        public List<Review.ReviewTemplate> ReviewTemplates { get; set; }

        public Review.ReviewTemplate SelectedReviewTemplate { get; set; }

        public List<string> RestrictAccessList { get; set; }

        public string SelectedRestrictAccess { get; set; }

        public ICommand ChangeHeadlineCommand
        {
            get => new RelayCommand(() =>
            {

            });
        }

        public ICommand SearchCommand
        {
            get => new RelayCommand<string>(async (reviewId) =>
            {
                try
                {
                    CurrentMainWindow.Instance.IsBusy = true;
                    await Task.Delay(200);
                    Reload(int.Parse(reviewId));
                    SearchedReviewId = reviewId;
                }
                catch (Exception e)
                {
                    IsLoaded = false;
                }
                finally
                {
                    Messenger.Default.Send<RefreshReviewPageMessage>(new RefreshReviewPageMessage());
                    CurrentMainWindow.Instance.IsBusy = false;
                }
            });
        }

        private void Reload(int reviewId)
        {
            try
            {
                ReviewSummary = _globalParameters.Server.ReviewService.GetReviewSummary(reviewId, true);
                IsLoaded = ReviewSummary != null;
                ReviewInfo = _globalParameters.Server.ReviewService.GetReviewInfo(reviewId);
                ReviewTemplates = _globalParameters.Server.ReviewService.GetAvailableTemplates();
                SelectedReviewTemplate = ReviewTemplates.FirstOrDefault((r) => r.name == ReviewSummary.generalInfo.reviewTemplate.name);
                RestrictAccessList = ApiUtils.GetAccessPolicyDisplayList("Anyone,Participants");
                SelectedRestrictAccess = ApiUtils.GetAccessPolicyFriendlyName(ReviewInfo.accessPolicy);

                foreach (var field in SelectedReviewTemplate.reviewCustomFields)
                {
                    field.selectedItem = ReviewSummary.generalInfo.customFieldValue.FirstOrDefault(v => v.customFieldTitle == field.title)?.customFieldValue.FirstOrDefault();
                }

                if (IsLoaded)
                {
                    GlobalParameters.ReviewId = reviewId.ToString();
                }

            }
            catch (Exception e)
            {
                IsLoaded = false;
            }
        }

        public string ReviewId
        {
            get => _globalParameters.ReviewId;
        }

        public string Title
        {
            get { return $"Review #{ReviewId}: {ReviewSummary.generalInfo.title}"; }
        }

        public Review.ReviewSummary ReviewSummary { get; set; }

        public Review.ReviewInfo ReviewInfo { get; set; }
    }
}