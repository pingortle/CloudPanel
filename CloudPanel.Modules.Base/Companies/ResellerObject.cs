using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudPanel.Modules.Base.Companies
{
    public class ResellerObject
    {
        #region Variables

        private int _companyid;

        private string _companyname;
        private string _companycode;
        private string _street;
        private string _city;
        private string _state;
        private string _zipcode;
        private string _country;
        private string _telephone;
        private string _website;
        private string _description;
        private string _adminname;
        private string _adminemail;
        private string _distinguishedname;

        private DateTime _created;

        #endregion

        #region Getters & Setters

        /// <summary>
        /// The unique SQL ID of the company in the database
        /// </summary>
        public int CompanyID
        {
            get { return _companyid; }
            set { _companyid = value; }
        }

        /// <summary>
        /// The company's name
        /// </summary>
        public string CompanyName
        {
            get { return _companyname; }
            set { _companyname = value; }
        }

        /// <summary>
        /// The unique ID of the company (company code)
        /// </summary>
        public string CompanyCode
        {
            get { return _companycode; }
            set { _companycode = value; }
        }

        /// <summary>
        /// Street of the company
        /// </summary>
        public string Street
        {
            get { return _street; }
            set { _street = value; }
        }

        /// <summary>
        /// City of the company
        /// </summary>
        public string City
        {
            get { return _city; }
            set { _city = value; }
        }

        /// <summary>
        /// State of the company
        /// </summary>
        public string State
        {
            get { return _state; }
            set { _state = value; }
        }

        /// <summary>
        /// Zip code of the company (not integer because other countries have non-integer postal codes)
        /// </summary>
        public string ZipCode
        {
            get { return _zipcode; }
            set { _zipcode = value; }
        }

        /// <summary>
        /// Country of the company
        /// </summary>
        public string Country
        {
            get { return _country; }
            set { _country = value; }
        }

        /// <summary>
        /// Telephone for the company
        /// </summary>
        public string Telephone
        {
            get { return _telephone; }
            set { _telephone = value; }
        }

        /// <summary>
        /// Website for the company
        /// </summary>
        public string Website
        {
            get { return _website; }
            set { _website = value; }
        }

        /// <summary>
        /// Any description or notes added to the company
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        /// A person's name for contact for the company
        /// </summary>
        public string AdminName
        {
            get { return _adminname; }
            set { _adminname = value; }
        }

        /// <summary>
        /// A person's email for contact for the company
        /// </summary>
        public string AdminEmail
        {
            get { return _adminemail; }
            set { _adminemail = value; }
        }

        /// <summary>
        /// DistinguishedName of the company from Active Directory
        /// </summary>
        public string DistinguishedName
        {
            get { return _distinguishedname; }
            set { _distinguishedname = value; }
        }

        /// <summary>
        /// When the company was created in CloudPanel
        /// </summary>
        public DateTime Created
        {
            get { return _created; }
            set { _created = value; }
        }

        #endregion

        #region Other

        /// <summary>
        /// Returned a formatted address
        /// </summary>
        public string FullAddressFormatted
        {
            get
            {
                string returnAddress = string.Empty;

                return string.Format("{0}<br />{1}, {2}  {3}", Street, City, State, ZipCode);
            }
        }

        #endregion
    }
}
