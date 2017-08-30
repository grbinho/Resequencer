# Resequencer [WIP]
Library for resequencing messages that arrive out of sequence.

Examples in this repository have been build using Azure Web Jobs, but library should be general 
enough to be used in any type of process.

For the sake of simplicity we will implement communication with Azure Queues. Same can be accomplished with Azure Service Bus, MSMQ
or some other messaging platform. 

This library is not concerned with that.

## Configuration

Configuration of connection strings and other sensitive data should be put into AppSettings.config. There should be one 
AppSettings.config for every console application in the solution. That file is not provided with the solution. 
You have to add it yourself.
