# appservice-loadbalancing

This repository demonstrates how load balancing of an API application can be achieved.

## source code

The repo contains two projects, both written in .NET 6:
- An API application
- A console application

The API application contains a singular endpoint which returns the instance id of the API that is currently responding to the request.

The console application has the ability to call the API endpoint and display the response on the screen. That is, it displays the instance id of the API that handled the request.

## infrastructure

The API is hosted in an Azure App Service with the following parameters:
- An app service plan using the Basic B1 pricing tier
- An instance count manually set to a constant value of 2
- ARR affinity turned off

## methodology

Call the API endpoint periodically every X seconds and output the results on the screen. If load balancing is successful, then we should see responses from each of the two instances, with a roughly 50/50 split between them.

In the specific experiment I did, I called the API every 2 seconds for 5 minutes

## results

After running the experiment, I got the following result:
- 75 times out of 150, the response came from the API instance of 70581e43e5ae277a3814559676c76df45bdd64daf60e286ba7d27fe1d72cd9d6
- 75 times out of 150, the response came from the API instance of 4110ecf8b0ce92a2da661ff130829493f79dd9a55b8bb9c740d6da950151fe99

See `/assets/results.png` for a screenshot of the console output.

## conclusion

Load balancing can be achieved using Azure App Service, which has the ability to use more than one instance of an API. Any functionality related to load balancing, which may include tracking a user's session, does not need to be handled in the API code itself.