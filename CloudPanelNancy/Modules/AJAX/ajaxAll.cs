using CloudPanel.Modules.Base.Plans;
using CloudPanel.Modules.Persistence.EntityFramework.Models;
using Nancy;
using Nancy.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudPanelNancy.Modules.AJAX
{
    public class ajaxAll : NancyModule
    {
        private static List<User> staticUsers;

        public ajaxAll() : base("/AJAX")
        {
            //this.RequiresAuthentication();

            Get["/Plans/Company/Get/{ID}"] = parameters => GetCompanyPlan(parameters.ID);
            Get["/Plans/Mailbox/Get/{ID}"] = parameters => GetMailboxPlan(parameters.ID);
            Get["/Company/Charting/Pie/{CompanyCode}"] = parameters => GetCompanyPieChart(parameters.CompanyCode);
            Get["/Company/{CompanyCode}/Users/GetAll"] = parameters => GetUsers(parameters.CompanyCode);
        }

        public Response GetCompanyPlan(string id)
        {
            CompanyPlanObject test = new CompanyPlanObject();
            test.CompanyPlanName = "BLAH";
            test.MaxUser = 30;
            test.MaxDomains = 5;
            test.MaxExchangeMailboxes = 30;
            test.MaxExchangeContacts = 5;
            test.MaxExchangeDistributionGroups = 5;
            test.MaxExchangeResourceMailboxes = 5;
            test.MaxExchangeMailPublicFolders = 5;

            return Response.AsJson(test, HttpStatusCode.OK);
        }

        public Response GetMailboxPlan(string id)
        {
            MailboxPlanObject test = new MailboxPlanObject();
            test.MailboxPlanName = "Bronze";
            test.MailboxPlanDescription = "Bronze package includes everything but Activesync";
            test.MaxRecipients = 100;
            test.MaxKeepDeletedItemsInDays = 15;
            test.MailboxSizeInMB = 2048;
            test.MaxMailboxSizeInMB = 4096;
            test.MaxSendInKB = 25600;
            test.MaxReceiveInKB = 25600;
            test.EnablePOP3 = true;
            test.EnableIMAP = true;
            test.EnableOWA = true;
            test.EnableMAPI = true;
            test.EnableAS = false;
            test.EnableECP = true;
            test.Cost = "1.95";
            test.Price = "3.95";
            test.AdditionalGBPrice = "1.00";

            return Response.AsJson(test, HttpStatusCode.OK);
        }

        public Response GetCompanyPieChart(string companyCode)
        {
            Dictionary<string, int> data = new Dictionary<string, int>();
            data.Add("Users", 1000);
            data.Add("Mailboxes", 500);
            data.Add("Citrix Users", 100);
            data.Add("Lync Users", 300);
            data.Add("Distribution Groups", 25);
            data.Add("Contacts", 10);

            return Response.AsJson(data, HttpStatusCode.OK);
        }

        public Response GetUsers(string companyCode)
        {
            string[] randomFirstNames = new string[] { "Jacob", "Kaleb", "Robert", "Jerod", "Eric", "Scott", "James", "Charles", "Chris", "Ethen", "Addison", "Tristen", "Leah", "Leonna", "John", "Jane", "Teresa", "Daniel", "Cheryl" };
            string[] randomLastNames = new string[] { "Dixon", "Lape", "Williams", "Dickinson", "Roscoe", "Green", "Hedrick", "Bailey", "Bradford", "Greene", "Mittermeier", "Johnson", "Kilpatrick", "Whitney", "Doe", "Dawn", "Blah", "Tefsgdf", "sdfsd" };
            string[] randomDepartments = new string[] { "Accounting", "", "Information Technology", "Sales", "Help Desk", "Field Techs" };

            if (staticUsers == null)
            {
                Random rand = new Random();

                staticUsers = new List<User>();
                for (int i = 0; i < 10000; i++)
                {
                    string randFirst = randomFirstNames[rand.Next(0, randomFirstNames.Length - 1)];
                    string randLast = randomLastNames[rand.Next(0, randomLastNames.Length - 1)];

                    string newName = randFirst + " " + randLast; // randFirst;// string.Format("{0} {1}", randFirst, randLast);
                    string newUPN = randFirst[0] + randLast + "@compsysar.com"; // string.Format("{0){1}@compsysar.com", randFirst[0], randLast);

                    var total = (from s in staticUsers where s.DisplayName == newName select s).Count();
                    if (total == 0)
                    {
                        User tmp = new User();
                        tmp.UserPrincipalName = newUPN;
                        tmp.Firstname = randFirst;
                        tmp.Lastname = randLast;
                        tmp.DisplayName = newName;
                        tmp.IsEnabled = rand.Next(0, 2) > 0 ? true : false;
                        tmp.IsResellerAdmin = rand.Next(0, 2) > 0 ? true : false;
                        tmp.IsCompanyAdmin = rand.Next(0, 2) > 0 ? true : false;
                        tmp.sAMAccountName = "asdfasdf";
                        tmp.Department = randomDepartments[rand.Next(0, randomDepartments.Length - 1)];

                        staticUsers.Add(tmp);
                    }
                }
            }

            var search = Request.Query.sSearch.HasValue ? ((string)Request.Query.sSearch).ToLower() : "";

            var newUsers = staticUsers;

            // This is if we are searching..
            if (!string.IsNullOrEmpty(search))
                newUsers = newUsers.Where(c => (
                                c.DisplayName.ToLower().Contains(search) ||
                                c.UserPrincipalName.ToLower().Contains(search) ||
                                c.sAMAccountName.ToLower().Contains(search) ||
                                c.Department.ToLower().Contains(search)
                               )).ToList();

            int start = Convert.ToInt32(Request.Query.iDisplayStart.ToString());
            int length = Convert.ToInt32(Request.Query.iDisplayLength.ToString());
            var totalRecords = newUsers.Count;
            var secho = Request.Query.sEcho;
            var sorting = Request.Query.sSortDir_0;

            if (sorting == "asc")
            {
                return Response.AsJson(new { aaData = newUsers.OrderBy(x => x.DisplayName).Skip(start).Take(length), sEcho = secho, iTotalRecords = totalRecords, iTotalDisplayRecords = totalRecords });
            }
            else
            {
                return Response.AsJson(new { aaData = newUsers.OrderByDescending(x => x.DisplayName).Skip(start).Take(length), sEcho = secho.ToString(), iTotalRecords = totalRecords, iTotalDisplayRecords = totalRecords });
            }
        }
    }
}