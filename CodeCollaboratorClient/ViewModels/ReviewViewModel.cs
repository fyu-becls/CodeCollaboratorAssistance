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
                    ReviewSummary = _globalParameters.Server.ReviewService.GetReviewSummary(int.Parse(ReviewId), true);
                    IsLoaded = ReviewSummary != null;
                    ReviewInfo = _globalParameters.Server.ReviewService.GetReviewInfo(int.Parse(ReviewId));
                    ReviewTemplates = _globalParameters.Server.ReviewService.GetAvailableTemplates();
                    SelectedReviewTemplate = ReviewTemplates.FirstOrDefault((r) => r.name == ReviewSummary.generalInfo.reviewTemplate.name);

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
                    ReviewSummary = _globalParameters.Server.ReviewService.GetReviewSummary(int.Parse(reviewId), true);
                    IsLoaded = ReviewSummary != null;
                    ReviewInfo = _globalParameters.Server.ReviewService.GetReviewInfo(int.Parse(reviewId));
                    ReviewTemplates = _globalParameters.Server.ReviewService.GetAvailableTemplates();
                    SelectedReviewTemplate =
                        ReviewTemplates.FirstOrDefault((r) => r.name == ReviewSummary.generalInfo.reviewTemplate.name);
                    //RaisePropertyChanged(nameof(ReviewSummary));
                    //RaisePropertyChanged(nameof(ReviewInfo));
                    //RaisePropertyChanged(nameof(ReviewTemplates));
                    //RaisePropertyChanged(nameof(SelectedReviewTemplate));
                    //RaisePropertyChanged(nameof(Title));
                    if (IsLoaded)
                    {
                        GlobalParameters.ReviewId = reviewId;
                    }

                    Messenger.Default.Send<RefreshReviewPageMessage>(new RefreshReviewPageMessage());
                }
                catch (Exception e)
                {
                    IsLoaded = false;
                }
                finally
                {

                    CurrentMainWindow.Instance.IsBusy = false;
                }
            });
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