# appservice-loadbalancing

This repository contains code of a sample API system that is successfully load balanced without the need for the API to contain any load balancing related code such as tracking user sessions.

## source code

The repo contains two projects:
- An API application
- A console application

The API application contains a single GET endpoint which returns the instance id of the API that is currently responding to the request.

The console application has the ability to call the API endpoint and display the response on the screen. That is, it displays the instance id of the API that handled the request.

## infrastructure

The API is hosted in an Azure App Service with the following parameters:
- An app service plan using the Basic B1 pricing tier
- An instance count manually set to a constant value of 2
- ARR affinity turned off

## methodology

Call the API endpoint periodically every X seconds and output the results on the screen. This is done via the console app. If load balancing is successful, then we should see responses from each of the two instances, ideally with a roughly 50/50 split between them.

In the specific experiment I did, I called the API every 2 seconds for 5 minutes for a total of 150 API calls.

## results

I got the following result:
- 75 times (50%), the response came from one instance with id `411...e99`
- 75 times (50%), the response came from another instance with id `705...9d6`

See `/assets/results.png` for a screenshot of the console output. The two instance ids correspond to the two instances of the API.

This result shows that the API has been successfully load balanced even if there is no load balancing related code in it.

## redundancy test

I conducted another experiment to see how the application would behave if the instance count was modified while the test was running. This serves as a simulation of an instance unintentionally becoming unavailable. I used the same parameters as the previous experiment, calling the API every 2 seconds for 5 minutes.

Here are the specific steps I performed:
1. Start the console app and let it run for a few seconds
2. Manually set the instance count down to 1, and observe the output of the console app for a few seconds
3. Manually set the instance count back up to 2, and observe the output of the console app for a few seconds
4. Wait for the app to finish

The results are as follows:
- For the first few seconds, the responses come from two instances `411...e99` and `7e8...262` with an approximately 50/50 split between them
- After step 2 was performed and the instance count is down to 1, all the responses exclusively come from instance `411...e99`
- After step 3 was performed and the instance count is back up to 2, the responses now come back from both instance `411...e99` and a new instance `b80...757`, with a 50/50 split between them

See `/assets/results_2.png` for a screenshot of the console output.

## conclusion

Load balancing of an API application can be achieved even if there is no load balancing related code in the API. This repository demonstrates how it can be done one way, which is via Azure App Service.