using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace CollabAPI
{
    public class SystemAdmin
    {
        public enum CustomFieldType
        {
            STRING,
            STRINGBIG,
            SELECTION,
            TREE_SELECTION,
            MULTI_SELECTION,
        }

        public enum CustomFieldModifyRole
        {
            ANY,
            ASSIGNED,
            AUTHOR,
            MODERATOR,
            AUTHOR_MODERATOR,
            AUTHOR_ASSIGNED,
            MODERATOR_ASSIGNED,
        }

        public class CustomFieldSettings : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
            {
                PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
                if (propertyChanged == null)
                    return;
                propertyChanged((object)this, new PropertyChangedEventArgs(propertyName));
            }

            protected void NotifyPropertyChanged(PropertyChangedEventArgs args)
            {
                PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
                if (propertyChanged == null)
                    return;
                propertyChanged((object)this, args);
            }

            public SystemAdmin.CustomFieldType customFieldType { get; set; }

            public string defaultValue { get; set; }

            public string description { get; set; }

            public List<string> items { get; set; }

            public int maximumLength { get; set; }

            public int minimumLength { get; set; }

            public string title { get; set; }

            public List<Review.ReviewPhase> visiblePhases { get; set; }

            public bool isRequired { get; set; }

            public SystemAdmin.CustomFieldModifyRole customFieldModifyRole { get; set; }
        }

        public class CustomFieldSettingsTarget
        {
            public string customFieldTitle { get; set; }

            public List<string> customFieldValue { get; set; }
        }

        public class RoleSettings
        {
            public string description { get; set; }

            public int id { get; set; }

            public int maximumAllowedInReview { get; set; }

            public int minimumRequiredInReview { get; set; }

            public string name { get; set; }

            public string systemName { get; set; }

            public bool requiredToSign { get; set; }
        }

        public class RemoteSystem
        {
            public int id { get; set; }

            public string token { get; set; }

            public string title { get; set; }

            public string config { get; set; }

            public Review.ReviewRemoteSystemType type { get; set; }
        }
    }
}
