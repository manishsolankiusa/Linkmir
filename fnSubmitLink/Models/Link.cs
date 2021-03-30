using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Text.RegularExpressions;

/*
 * Author   : Manish Solanki
 * Date     : March, 2021
 * All rights reserved.
 */

namespace Linkmir.AzFunctions.Models
{
    public class Link
    {
        private const string charSet = "0123456789abcdefghijklmnopqrstuvwxyz";
        private static readonly int numberBase = charSet.Length;
        private static IDictionary<char, int> charIndex;

        private static void Initialize()
        {
            charIndex = charSet
                .Select((c, i) => new { Index = i, Char = c })
                .ToDictionary(c => c.Char, c => c.Index);
        }

        public long ID { get; set; }

        public string LinkShort { get; internal set; }

        public string LinkOriginal { get; set; }

        public string LinkValidated { get; internal set; }

        public string LinkDomain { get; internal set; }

        public string LinkSubdomain { get; internal set; }

        public string HashValue { get; set; }

        public long AccessCount { get; set; }

        public long SubmitCount { get; set; }

        public string SubmitIP { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Status { get; set; } = "Not Validated";

        public void GenerateShortLink()
        {
            if (ID < numberBase)
            {
                LinkShort = charSet[(int) ID].ToString();
                return;
            }

            StringBuilder strBuilder = new StringBuilder();
            long i = ID;

            while (i > 0)
            {
                strBuilder.Insert(0, charSet[(int)(i % numberBase)]);
                i /= numberBase;
            }

            LinkShort = strBuilder.ToString();
        }

        public bool ValidateOriginal(ref string message)
        {
            try
            {
                if (LinkOriginal.IndexOfAny(new char[] { ' ', '\t', '*', '"', '\n', '\r', '\f', '\b', '\\', '\'' }) > 0)
                {
                    message = "URL must not contain space.";
                    Status = "Invalid";
                    return false;
                }
                Uri uri = new Uri(LinkOriginal);
                if (!(uri.Scheme == "http" || uri.Scheme == "https"))
                {
                    message = "URL must start with HTTP or HTTPS.";
                    Status = "Invalid";
                    return false;
                }

                string[] hostParts = uri.Host.Split(".", StringSplitOptions.RemoveEmptyEntries);
                LinkSubdomain = String.Join(".", hostParts, 0, hostParts.Length - 2);
                if (LinkSubdomain == "www") LinkSubdomain = "";
                LinkDomain = hostParts[hostParts.Length - 2] + "." + hostParts[hostParts.Length - 1];

                LinkValidated = uri.AbsoluteUri;

                Status = "Valid";
                return true;
            }
            catch(Exception e)
            {
                Status = "Invalid";
                return false;
            }
        }

        public bool ValidateShort(ref string message)
        {
            try
            {
                if (LinkShort.IndexOf(" ") > 0)
                {
                    message = "Link must not contain spaces.";
                    Status = "Invalid";
                    return false;
                }
                if (LinkShort.Length > 10 || !(new Regex("^[a-z0-9]").IsMatch(LinkShort)))
                {
                    message = "Invalid link provided.";
                    Status = "Invalid";
                    return false;
                }

                Status = "Valid";
                return true;
            }
            catch (Exception e)
            {
                Status = "Invalid";
                return false;
            }
        }
    }
}
