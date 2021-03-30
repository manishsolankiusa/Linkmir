# Linkmir

/*
 * Author   : Manish Solanki
 * Date     : March, 2021
 * All rights reserved.
 */
 
Current Deployment
------------------
The Linkmir system can be access using the following endpoints:

Submit Link:
https://linkmir.azurewebsites.net/api/SubmitLink?link=https://www.google.com

Parameter : link - provide the complete URL

Returns : JSON with Shortened Link and link status/action

GetLink:
https://linkmir.azurewebsites.net/api/GetLink?=1

Parameter : Shortened link

Returns : JSON with original link (unfurled)

GetStats:
https://linkmir.azurewebsites.net/api/GetStats?query=

Parameter : query - provide search criteria 
			Wildcards: * means any number of characters
					   ? means any one character
		Examples: *.google.com, docs.*, docs.*.com, *.M?cros?ft.com

Returns : JSON with statistics XML
			1. Total links exists in the system with matching criteria
			2. Total number of times these links were accessed
			3. Total number of times these links were submitted

GetLinkStats:
https://linkmir.azurewebsites.net/api/GetStats?link=https://www.linkmir.com/as3rg

Parameter : link - provide shortened link

Returns : JSON with statistics XML
			1. Total links exists in the system with matching criteria (should have always 1 for link)
			2. Total number of times these links were accessed
			3. Total number of times these links were submitted

You can use either use Postman app, other apps or mechanism to access endpoints.


Compile and build
-----------------

Developed using Microsoft Visual Studio 2019
Open the "LinkmirFunctions.sln" in Visual Studio, you can compile, build and publish it.

Dependecies:
The solution requires the following dependencies
* Microsoft.NETCore Framework
* Microsoft.NET.Sdk.Functions (3.0.11 or higher)
* System.Data.SqlClient (4.8.2 or higher)

To run and test locally, you must update the local.sesttings.json with the database connection string:
  "ConnectionStrings": {
    "ConnectionString": "Data Source=localhost;Initial Catalog=LinkmirDB;Integrated Security=True;"
  }

Questions
---------
In general, I would have asked many questions before hand but due to offline nature, I am listing some of the questions below.

- Is it global or local app?
- Get more info on Geographic distribution if global
- How much traffic is expected for the site?
- How many users are expected to access the site?
- How many concurrent users are expected?
- Response time expectation
- Any highs and lows for the traffic?
- Estimates/plans for next 6 months, 1 year and 3/5 years
- Who are the consumers of these services and how/what they utilize for?


- Should http and https URLs treated as one or different assuming remaining part of the URL is identical?
- What's the max URL size to be supported?
- Is there a need to provide delete/remove functionality?
- Is there any expiry of these shortened URLs? (Not implemented in the current codebase)



Code Challenge
On the surface everything is simple; but once you peel back the layers there is infinite room for complexity - some needed, some not.
For this challenge, you will develop the back-end API to a new project named linkmir. Linkmir is a new social media platform with one caveat - you can only post http/https links - no comments, no images, just links.
Your solution must achieve the following:
•	allow an unauthenticated client to POST a string containing a link a client would link to submit
o	verify the link protocol is allowed or respond with the appropriate status code
o	verify the link is new; e.g it has not previously been submitted
	if the link is new a shortened link will be generated, e.g. https://www.google.com/ -> https://www.linkmir.com/{unique_path}
	if the link has been submitted, respond with the previously generated shorten link
•	allow an unauthenticated client to GET a shortened link and unfurl it into its original url in a returned json object; e.g. GET https://www.linkmir.com/{unique_path} -> {"domain": "https://www.google.com"}
•	allow an unauthenticated client to GET the stats on any https://www.linkmir.com/{unique_path}
•	allow an unauthenticated client to GET the stats on any sub-domain, domain or combination thereof;
o	example queries:
	* denotes a wildcard
	sub-domain: “open” & domain: “spotify.com”
	sub-domain: “try” & domain: “*”
o	response stats must include:
	meta counts:
	how many furled matching links exist in the system
	total number of times accessed across all matching
	total number of times submitted across all matching
A few notes:
•	The sub-domain/domain www.linkmir.com is just put in for context and flavor, you do not need to spoof that domain.
•	You can change the URL paths or verbs for any of the requirements; but you must explain why in a project README.md at the root of your codebase
•	Your project must build and the how to build it, including any setup or system dependencies, must be described in your README.md at the root of your codebase
•	Ensure you design, code and test for scale
•	Performance matters! Know how your services perform and how it takes as your usage grows.
•	You must deploy your service; and we must be able to hit the endpoints
•	Add what you believe is an acceptable amount of testing to your project
•	We will ask you to extend your code
•	Please add the following public github accounts to your repo (the repo can be private)
o	awproksel – Andrew Proksel
•	YOU ARE ALLOWED TO ASK QUESTIONS!
o	If you have questions, please create a github issue and assign it to Andrew Proksel (awproksel)
