using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudPanel.Modules.Base.Exchange
{
    public class RecipientFilters
    {
        private const string RoomFilter = "((Alias -ne $null) -and (CustomAttribute1 -eq '{0}') -and (((RecipientDisplayType -eq 'ConferenceRoomMailbox') -or (RecipientDisplayType -eq 'SyncedConferenceRoomMailbox'))))";
        private const string UsersFilter = "((Alias -ne $null) -and (CustomAttribute1 -eq '{0}') -and (((((ObjectCategory -like 'person') -and (ObjectClass -eq 'user') -and (-not(Database -ne $null)) -and (-not(ServerLegacyDN -ne $null)))) -or (((ObjectCategory -like 'person') -and (ObjectClass -eq 'user') -and (((Database -ne $null) -or (ServerLegacyDN -ne $null))))))))";
        private const string ContactsFilter = "((Alias -ne $null) -and (CustomAttribute1 -eq '{0}') -and (((ObjectCategory -like 'person') -and (ObjectClass -eq 'contact'))))";
        private const string GroupsFilter = "((Alias -ne $null) -and (CustomAttribute1 -eq '{0}') -and (ObjectCategory -like 'group'))";
        private const string GALFilter = "(Alias -ne $null) -and (CustomAttribute1 -eq '{0}')";

        private const string RoomName = "{0} - All Rooms";
        private const string UsersName = "{0} - All Users";
        private const string ContactsName = "{0} - All Contacts";
        private const string GroupsName = "{0} - All Groups";
        private const string GALName = "{0} GAL";
        private const string OALName = "{0} OAL";
        private const string ABPName = "{0} ABP";

        #region Get Filters
        public static string GetRoomFilter(string companyCode) { return string.Format(RoomFilter, companyCode); }

        public static string GetUsersFilter(string companyCode) { return string.Format(UsersFilter, companyCode); }

        public static string GetContactsFilter(string companyCode) { return string.Format(ContactsFilter, companyCode); }

        public static string GetGroupsFilter(string companyCode) { return string.Format(GroupsFilter, companyCode); }

        public static string GetGALFilter(string companyCode) { return string.Format(GALFilter, companyCode); }
        #endregion

        #region Get Names

        public static string GetRoomName(string companyCode) { return string.Format(RoomName, companyCode); }

        public static string GetUsersName(string companyCode) { return string.Format(UsersName, companyCode); }

        public static string GetContactsName(string companyCode) { return string.Format(ContactsName, companyCode); }

        public static string GetGroupsName(string companyCode) { return string.Format(GroupsName, companyCode); }

        public static string GetGALName(string companyCode) { return string.Format(GALName, companyCode); }

        public static string GetOALName(string companyCode) { return string.Format(OALName, companyCode); }

        public static string GetABPName(string companyCode) { return string.Format(ABPName, companyCode); }

        #endregion

        #region Get String array

        public static string[] GetABPAddressLists(string companyCode)
        {
            return new string[] { GetUsersName(companyCode), GetContactsName(companyCode), GetGroupsName(companyCode) };
        }

        #endregion
    }
}
