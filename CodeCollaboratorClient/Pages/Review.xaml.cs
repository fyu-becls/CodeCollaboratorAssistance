using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CodeCollaboratorClient.Annotations;

namespace CodeCollaboratorClient.Pages
{
    /// <summary>
    /// Interaction logic for Review.xaml
    /// </summary>
    public partial class Review : Page, INotifyPropertyChanged
    {
        private string _reviewId;
        public Review()
        {
            InitializeComponent();
            ReviewId = GlobalParameters.ReviewId;
            DataContext = this;
        }

        public string ReviewId
        {
            get => _reviewId;
            set
            {
                if (_reviewId != value)
                {
                    _reviewId = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
