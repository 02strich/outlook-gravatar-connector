using System;
using System.Diagnostics;
using OutlookSocialProvider;

namespace GravatarOSC
{
    public class OSCProvider : ISocialProvider
    {
        public const string NetworkName = "Gravatar";
        public const string NetworkUrl = "http://www.gravatar.com/";

        private const string ProviderVersion = "1.1";
        private const string ProviderGuidString = "{C8A61225-F18B-4AB9-ADCB-3ADDDA709AA7}";

        public string GetCapabilities() {
            
            var capabilities = new OSCSchema.capabilities {
                // OSC 1.0 capabilities
                getFriends = true,
                cacheFriends = false,
                followPerson = false,
                doNotFollowPerson = false,
                getActivities = true,
                cacheActivities = false,

                // OSC 1.1 capabilities
                dynamicActivitiesLookupEx = false,
                dynamicActivitiesLookupExSpecified = true,
                hideHyperlinks = false,
                hideHyperlinksSpecified = true,
                supportsAutoConfigure = false,
                supportsAutoConfigureSpecified = true,
                dynamicContactsLookup = true,
                dynamicContactsLookupSpecified = true,
                useLogonCached = true,
                useLogonCachedSpecified = true,
                hideRememberMyPassword = false,
                hideRememberMyPasswordSpecified = true,
                hashFunction = "MD5"
            };

            var result = HelperMethods.SerializeObjectToString(capabilities);
            Debug.WriteLine(result);
            return result;
        }

        /// <summary>
        /// Executed because we signal "supportsAutoConfigure"
        /// </summary>
        public ISocialSession GetAutoConfiguredSession() {
            Debug.WriteLine("GetAutoConfiguredSession called");

            return new OSCSession(this, true);
        }

        /// <summary>
        /// Should not be called by Outlook, but still returns something
        /// </summary>
        public ISocialSession GetSession() {
            Debug.WriteLine("GetSession called");

            return new OSCSession(this);
        }

        /// <summary>
        /// Not supported in OSC version 1.0 and version 1.1
        /// </summary>
        public void GetStatusSettings(out string statusDefault, out int maxStatusLength) {
            Debug.WriteLine("GetStatusSetting called");

            throw new NotImplementedException();
        }

        /// <summary>
        /// Initialize Plugin
        /// </summary>
        public void Load(string socialProviderInterfaceVersion, string languageTag) {
            Debug.WriteLine("Load called with socialProviderInterfaceVersion=" + socialProviderInterfaceVersion + " and languageTag=" + languageTag);
        }

        public byte[] SocialNetworkIcon {
            get { return HelperMethods.GetProviderJpeg(); }
        }

        public string SocialNetworkName {
            get { return NetworkName; }
        }

        public Guid SocialNetworkGuid {
            get { return new Guid(ProviderGuidString); }
        }

        public string[] DefaultSiteUrls {
            get { return new [] { NetworkUrl }; }
        }

        public string Version { 
            get { return ProviderVersion; }
        }
    }
}
