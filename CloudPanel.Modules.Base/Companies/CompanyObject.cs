using CloudPanel.Modules.Base.Plans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudPanel.Modules.Base.Companies
{
    public class CompanyObject
    {
        #region Variables 

        private int _companyid;
        private int _orgplanid;

        private bool _isexchangeenabled;
        private bool _islyncenabled;
        private bool _iscitrixenabled;
        private bool _isexchangepermissionsfixed;
        private bool _usecompanynameinsteadofcompanycode;

        private string _resellercode;
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

        private string[] _domains;

        private DateTime _created;

        private CompanyPlanObject _companyplanobject;

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
        /// The ID of the company plan assigned to the company
        /// </summary>
        public int CompanyPlanID
        {
            get { return _orgplanid; }
            set { _orgplanid = value; }
        }

        /// <summary>
        /// If Exchange is enabled or not for the company
        /// </summary>
        public bool IsExchangeEnabled
        {
            get { return _isexchangeenabled; }
            set { _isexchangeenabled = value; }
        }

        /// <summary>
        /// If Lync is enabled or not for the company
        /// </summary>
        public bool IsLyncEnabled
        {
            get { return _islyncenabled; }
            set { _islyncenabled = value; }
        }

        /// <summary>
        /// If Citrix is enabled or not for the company
        /// </summary>
        public bool IsCitrixEnabled
        {
            get { return _iscitrixenabled; }
            set { _iscitrixenabled = value; }
        }

        /// <summary>
        /// If the Exchange permissions have been fixed (only used if they used CloudPanel 3.0.600 or before)
        /// </summary>
        public bool IsExchangePermissionsFixed
        {
            get { return _isexchangepermissionsfixed; }
            set { _isexchangepermissionsfixed = value; }
        }

        /// <summary>
        /// If we are using the company name instead of a auto generated one
        /// </summary>
        public bool UseCompanyNameInsteadofCompanyCode
        {
            get { return _usecompanynameinsteadofcompanycode; }
            set { _usecompanynameinsteadofcompanycode = value; }
        }

        /// <summary>
        /// The reseller code that the company belongs to
        /// </summary>
        public string ResellerCode
        {
            get { return _resellercode; }
            set { _resellercode = value; }
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
        /// Domains that the company has
        /// </summary>
        public string[] Domains
        {
            get { return _domains;  }
            set { _domains = value; }
        }

        /// <summary>
        /// When the company was created in CloudPanel
        /// </summary>
        public DateTime Created
        {
            get { return _created; }
            set { _created = value; }
        }

        public CompanyPlanObject CompanyPlanObject
        {
            get { return _companyplanobject; }
            set { _companyplanobject = value; }
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

        /// <summary>
        /// Generates a unique company code based on the company's company name
        /// </summary>
        /// <param name="companyName"></param>
        /// <returns></returns>
        public static string GenerateCompanyCode(string companyName, bool useCompanyName)
        {
            char[] stripped = null;
            if (useCompanyName)
                stripped = companyName.Where(c => char.IsLetter(c) || char.IsWhiteSpace(c) || char.IsNumber(c)).ToArray();
            else
                stripped = companyName.Where(c => char.IsLetter(c)).ToArray();

            return new string(stripped); // This should contain only letters, numbers, and whitespaces if not using company code generator 
        }

        #endregion
    }
}
