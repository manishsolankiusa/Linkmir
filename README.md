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
