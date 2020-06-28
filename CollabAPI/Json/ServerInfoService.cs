using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
#pragma warning disable 0649 // Expected warnings in JSON classes
namespace CollabAPI
{
    public class ServerInfoService
    {
        public const string LicenseCompanyKey = "license-company-key";
        public const string LicenseNodeId = "license-node-id";
        public const string LicenseFixedSeats = "license-fixed-seats";
        public const string LicenseFloatingSeats = "license-floating-seats";
        public const string LicenseExpirationDate = "license-expiration-date";
        public const string ReviewDeadlineDefault = "review-deadline-default";
        public const string ReviewAccessAllowed = "review-access-allowed";
        public const string AllowSysadminReviews = "allow-sysadmin-reviews";

        public class getVersion
        {
        }

        public class getVersionResponse
        {
            public string version;
        }

        public class getServerBuild
        {
        }

        public class getServerBuildResponse
        {
            public int serverBuild;
        }

        public class getMinimumJavaClientBuild
        {
        }

        public class getMinimumJavaClientBuildResponse
        {
            public int minimumJavaClientBuild;
        }

        public class getSystemGlobalValues : ServerInfoService.SystemGlobalValuesRequest
        {
        }

        public class SystemGlobalValuesRequest
        {
            public List<string> systemGlobalValuesNames;
        }

        public class SystemGlobalValuesResponse
        {
            public SortedDictionary<string, string> systemGlobalValues;
        }

        public class getRemoteSystemIntegrations : ServerInfoService.RemoteSystemIntegrationsRequest
        {
        }

        public class RemoteSystemIntegrationsRequest
        {
            public string token { get; set; }

            public string ids { get; set; }

            public bool enabledOnly { get; set; }
        }

        public class RemoteSystemIntegrationsResponse
        {
            public List<SystemAdmin.RemoteSystem> remoteSystemIntegrations { get; set; }
        }
    }
}

#pragma warning restore 0649 // Expected warnings in JSON classes