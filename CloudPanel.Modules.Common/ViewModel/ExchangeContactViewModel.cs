using CloudPanel.Modules.Base.Exchange;
using CloudPanel.Modules.Common.Database;
using CloudPanel.Modules.Common.Rollback;
using CloudPanel.Modules.Common.Settings;
using CloudPanel.Modules.Exchange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudPanel.Modules.Common.ViewModel
{
    public class ExchangeContactViewModel : IViewModel
    {
        public void NewContact(string companyCode, MailContactObject mailContact)
        {
            ExchangePowershell powershell = null;
            CPDatabase database = null;

            try
            {
                // Get company distinguished name
                database = new CPDatabase();
                var dn = (from c in database.Companies
                          where !c.IsReseller
                          where c.CompanyCode == companyCode
                          select c.DistinguishedName).First();

                powershell = new ExchangePowershell(StaticSettings.ExchangeURI, StaticSettings.Username, StaticSettings.DecryptedPassword, StaticSettings.ExchangeUseKerberos, StaticSettings.PrimaryDC);
                string distinguishedName = powershell.NewContact(mailContact.DisplayName, mailContact.Email, mailContact.Hidden, companyCode, "OU=Exchange," + dn);

                // Add contact to database
                Contact newContact = new Contact();
                newContact.DisplayName = mailContact.DisplayName;
                newContact.CompanyCode = companyCode;
                newContact.DistinguishedName = distinguishedName;
                newContact.Email = mailContact.Email;
                newContact.Hidden = mailContact.Hidden;
                database.Contacts.Add(newContact);
                database.SaveChanges(); 
            }
            catch (Exception ex)
            {
                ThrowEvent(Base.Enumerations.AlertID.FAILED, ex.Message);
            }
            finally
            {
                if (database != null)
                    database.Dispose();

                if (powershell != null)
                    powershell.Dispose();
            }
        }

        public void UpdateContact(string distinguishedName, MailContactObject mailContact)
        {
            ExchangePowershell powershell = null;
            CPDatabase database = null;

            try
            {
                // Get company distinguished name
                database = new CPDatabase();
                var contact = (from c in database.Contacts
                               where c.DistinguishedName == distinguishedName
                               select c).First();

                powershell = new ExchangePowershell(StaticSettings.ExchangeURI, StaticSettings.Username, StaticSettings.DecryptedPassword, StaticSettings.ExchangeUseKerberos, StaticSettings.PrimaryDC);
                string returnedDN = powershell.UpdateContact(distinguishedName, mailContact.DisplayName, mailContact.Hidden);

                contact.DistinguishedName = returnedDN;
                database.SaveChanges();
            }
            catch (Exception ex)
            {
                ThrowEvent(Base.Enumerations.AlertID.FAILED, ex.Message);
            }
            finally
            {
                if (database != null)
                    database.Dispose();

                if (powershell != null)
                    powershell.Dispose();
            }
        }

        public void DeleteContact(string distinguishedName, string companyCode)
        {
            ExchangePowershell powershell = null;
            CPDatabase database = null;

            try
            {
                // Get company distinguished name
                database = new CPDatabase();
                var contact = (from c in database.Contacts
                               where c.DistinguishedName == distinguishedName
                               select c).First();

                powershell = new ExchangePowershell(StaticSettings.ExchangeURI, StaticSettings.Username, StaticSettings.DecryptedPassword, StaticSettings.ExchangeUseKerberos, StaticSettings.PrimaryDC);
                powershell.DeleteContact(distinguishedName);

                database.Contacts.Remove(contact);
                database.SaveChanges();
            }
            catch (Exception ex)
            {
                ThrowEvent(Base.Enumerations.AlertID.FAILED, ex.Message);
            }
            finally
            {
                if (database != null)
                    database.Dispose();

                if (powershell != null)
                    powershell.Dispose();
            }
        }

        public List<MailContactObject> GetContacts(string companyCode)
        {
            CPDatabase database = null;

            try
            {
                // Get all contacts for company
                database = new CPDatabase();

                var contacts = from c in database.Contacts
                               where c.CompanyCode == companyCode
                               orderby c.DisplayName
                               select new MailContactObject()
                               {
                                   DisplayName = c.DisplayName,
                                   CompanyCode = c.CompanyCode,
                                   DistinguishedName = c.DistinguishedName,
                                   Email = c.Email,
                                   Hidden = c.Hidden
                               };

                if (contacts != null)
                    return contacts.ToList();
                else
                    return null;
            }
            catch (Exception ex)
            {
                ThrowEvent(Base.Enumerations.AlertID.FAILED, ex.Message);
                return null;
            }
            finally
            {
                if (database != null)
                    database.Dispose();
            }
        }

        public MailContactObject GetContact(string distinguishedName)
        {
            CPDatabase database = null;

            try
            {
                // Get all contacts for company
                database = new CPDatabase();

                var contact = (from c in database.Contacts
                               where c.DistinguishedName == distinguishedName
                               orderby c.DisplayName
                               select new MailContactObject()
                               {
                                   DisplayName = c.DisplayName,
                                   CompanyCode = c.CompanyCode,
                                   DistinguishedName = c.DistinguishedName,
                                   Email = c.Email,
                                   Hidden = c.Hidden
                               }).First();

                return contact;
            }
            catch (Exception ex)
            {
                ThrowEvent(Base.Enumerations.AlertID.FAILED, ex.Message);
                return null;
            }
            finally
            {
                if (database != null)
                    database.Dispose();
            }
        }
    }
}
