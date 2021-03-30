using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Linkmir.AzFunctions.DBContexts;

/*
 * Author   : Manish Solanki
 * Date     : March, 2021
 * All rights reserved.
 */

namespace Linkmir.AzFunctions.ReportingServices
{
    class SummaryReport
    {
        public string GetByDomain(string criteria)
        {
            criteria = criteria.Replace("*", "%").Replace("?", "_").ToLower();

            string domain;
            string subdomain;
            string[] hostParts = criteria.Split(".", StringSplitOptions.RemoveEmptyEntries);
            if (criteria.Substring(criteria.Length - 1) == "%")
            {
                subdomain = String.Join(".", hostParts, 0, hostParts.Length - 1);
                if (subdomain == "www") subdomain = "";
                domain = hostParts[hostParts.Length - 1];
            }
            else
            {
                subdomain = String.Join(".", hostParts, 0, hostParts.Length - 2);
                if (subdomain == "www") subdomain = "";
                domain = hostParts[hostParts.Length - 2] + "." + hostParts[hostParts.Length - 1];
            }

            ReportContext reportContext = new ReportContext();
            return reportContext.GetSummaryDataByDomain(domain, subdomain);
        }

        public string GetByLink(string link)
        {
            Uri uri = new Uri(link);
            string linkShort = uri.PathAndQuery.Substring(1);

            ReportContext reportContext = new ReportContext();
            return reportContext.GetSummaryDataByLink(linkShort);
        }


        public bool Validate(string criteria, ref string message)
        {
            message = "Invalid input query.";

            try
            {
                if (criteria.Substring(0, 4).ToLower() == "http" || criteria.Substring(0, 4).ToLower() == "https")
                {
                    Uri uri = new Uri(criteria);
                    if (uri.Host != "www.linkmir.com")
                        return false;
                }
                else if (criteria.Length > 100 || criteria.IndexOfAny(new char[] { ' ', '\t', '"', '\n', '\r', '\f', '\b', '\\', '\'' }) > 0)
                {
                    return false;
                }
                message = "";
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
