using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;
using CodeCollaboratorClient.CollabAPIManager;
using CodeCollaboratorClient.Pages;
using CollabAPI;
using CollabCommandAPI;
using CollabCommandAPI.Commands;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace CodeCollaboratorClient.ViewModels
{
    public class HomeViewModel : ObservableObject
    {
        private bool _isServerConnected;
        private readonly APIManager _apiManager;
        private Server _server;
        private readonly ReviewCommands _reviewCommands;
        private readonly ICollabCommandExecutor _commandExecutor;
        private readonly LoginViewModel _loginViewModel;
        private List<User.UserInfo> _userList;

        public LoginViewModel LoginViewModel
        {
            get => _loginViewModel;
        }

        public class ReviewSummary
        {
            public string Id { get; set; }
            public string Title { get; set; }
            public string Stage { get; set; }
        }

        public async Task Initialize()
        {
            CurrentMainWindow.Instance.IsBusy = true;
            await Task.Delay(1000);
            _server = await _apiManager.GetCollaboratorServer("fyu", "Beckman@123");
            IsServerConnected = _server?.Connected ?? false;
            _userList = _server?.UserService.GetUserList();

            CurrentMainWindow.Instance.IsBusy = false;
        }

        public bool IsServerConnected
        {
            get => _isServerConnected;
            set => Set(ref _isServerConnected, value);
        }

        private ObservableCollection<ReviewSummary> _reviewList;
        public ObservableCollection<ReviewSummary> ReviewList
        {
            get => _reviewList;
            set => Set(ref _reviewList, value);
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                Set(ref _isLoading, value); 
                RaisePropertyChanged(nameof(RefreshCommand));
                RefreshCommand.RaiseCanExecuteChanged();
            }
        }

        public HomeViewModel(LoginViewModel loginViewModel, APIManager apiManager, ICollabCommandExecutor commandExecutor, ReviewCommands reviewCommands)
        {
            _loginViewModel = loginViewModel;
            _loginViewModel.PropertyChanged += async (sender, args) =>
            {
                if (args.PropertyName == nameof(LoginViewModel.IsLogin))
                {
                    await RefreshReviews();
                    //if (_loginViewModel.IsLogin)
                    //{
                    //    if (_userList != null)
                    //    {
                    //        IsLoading = true;
                    //        await Task.Delay(500);
                    //        var fullName = _userList.FirstOrDefault(u => u.login == _loginViewModel.UserName || u.fullName == _loginViewModel.UserName).fullName;
                    //        var reviewListResult = await _commandExecutor.ExecuteCommand(_reviewCommands.GetReviewsList(fullName, true), 5000);
                    //        if (reviewListResult.ExitCode == 0)
                    //        {
                    //            var reviewList = reviewListResult.StandardOutput.Split(',').OrderByDescending(r => r).ToList();
                    //            if (reviewList.Count > 0)
                    //            {
                    //                ReviewList = new ObservableCollection<ReviewSummary>();
                    //                var fullReviewListResult = await _commandExecutor.ExecuteCommand(_reviewCommands.GetReviewsList(fullName), 5000);
                    //                var fullReviewList = fullReviewListResult.StandardOutput.Split(Environment.NewLine).ToList();
                    //                foreach (var reviewId in reviewList)
                    //                {
                    //                    var reviewIdIndex = fullReviewList.IndexOf($"    Review ID: {reviewId}");
                    //                    var title = fullReviewList[reviewIdIndex + 1];
                    //                    var stage = fullReviewList[reviewIdIndex + 2];
                    //                    ReviewList.Add(new ReviewSummary { Id = reviewId, Title = title, Stage = stage });
                    //                }
                    //            }
                    //        }
                    //        IsLoading = false;
                    //    }
                    //}
                    //else
                    //{
                    //    ReviewList = new ObservableCollection<ReviewSummary>();
                    //}
                }
            };

            _apiManager = apiManager;
            IsServerConnected = true;
            _commandExecutor = commandExecutor;
            _reviewCommands = reviewCommands;

            SearchCommand = new RelayCommand<string>(SearchReviews, s => true);
            RefreshCommand = new RelayCommand(async () => await RefreshReviews(), ()=> !IsLoading);
            OpenReviewCommand = new RelayCommand<string>((id) =>
            {
                GlobalParameters.ReviewId = id;
                CurrentMainWindow.Instance.NavigateTo(new Uri("Pages/Review.xaml", UriKind.Relative));
            });

            Initialize();
        }

        public ICommand SearchCommand { get; private set; }
        public RelayCommand RefreshCommand { get; private set; }

        public ICommand OpenReviewCommand { get; private set; }

        private async Task RefreshReviews()
        {
            ReviewList = new ObservableCollection<ReviewSummary>();
            if (_loginViewModel.IsLogin)
            {
                if (_userList != null)
                {
                    IsLoading = true;
                    await Task.Delay(500);
                    var fullName = _userList.FirstOrDefault(u => u.login == _loginViewModel.UserName || u.fullName == _loginViewModel.UserName).fullName;
                    var reviewListResult = await _commandExecutor.ExecuteCommand(_reviewCommands.GetReviewsList(fullName, true), 5000);
                    if (reviewListResult.ExitCode == 0)
                    {
                        var reviewList = reviewListResult.StandardOutput.Split(',').OrderByDescending(r => r).ToList();
                        if (reviewList.Count > 0)
                        {
                            var fullReviewListResult = await _commandExecutor.ExecuteCommand(_reviewCommands.GetReviewsList(fullName), 5000);
                            var fullReviewList = fullReviewListResult.StandardOutput.Split(Environment.NewLine).ToList();
                            foreach (var reviewId in reviewList)
                            {
                                var reviewIdIndex = fullReviewList.IndexOf($"    Review ID: {reviewId}");
                                var title = fullReviewList[reviewIdIndex + 1];
                                var stage = fullReviewList[reviewIdIndex + 2];
                                ReviewList.Add(new ReviewSummary { Id = reviewId, Title = title, Stage = stage });
                            }
                        }
                    }
                    IsLoading = false;
                }
            }
        }

        private void SearchReviews(string searchText)
        {
            var result = _commandExecutor.ExecuteCommand(_reviewCommands.GetReviewsList(searchText?.Trim()), 5000);
        }
    }
}