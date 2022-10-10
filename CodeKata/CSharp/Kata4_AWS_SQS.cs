/*
 * AWS CLI
 * aws --endpoint-url=http://localhost:4566 sqs create-queue --queue-name myQueue
 * aws --endpoint-url=http://localhost:4566 sqs list-queues
 * aws --endpoint-url=http://localhost:4566 sqs delete-queue --queue-url <value>
*/

using Amazon.SQS;
using Amazon.SQS.Model;
using CodeKata.CSharp.Helpers.Kata4;
using System;
using System.Text.Json;
using System.Collections.Generic;
using System.Net;
using System.Threading;

var config = new AmazonSQSConfig();
config.ServiceURL = "http://localhost:4566";
var sqsClient = new AmazonSQSClient(config);
var queueName = "myQueue";
var queue = await sqsClient.GetQueueUrlAsync(queueName);

//Add 10 items to the queue
for (int i = 0; i < 100; i++)
{
    var payload = new JobCreated() { Id = i };

    var request = new SendMessageRequest()
    {
        QueueUrl = queue.QueueUrl,
        MessageBody = JsonSerializer.Serialize(payload)
    };

    await sqsClient.SendMessageAsync(request);
}


var ct = new CancellationToken();

while (!ct.IsCancellationRequested)
{
    var receiveRequest = new ReceiveMessageRequest()
    {
        QueueUrl = queue.QueueUrl,
        MessageAttributeNames = new List<string>() { "All" },
        AttributeNames = new List<string>() { "All" }
    };

    var response = await sqsClient.ReceiveMessageAsync(receiveRequest);
    if (response.HttpStatusCode != HttpStatusCode.OK)
    {
        //handle error
    }

    foreach (var msg in response.Messages)
    {
        Console.WriteLine(msg.Body);
        //Once the message is processed we need to remove it from the queue, so it's not reprocessed.
        await sqsClient.DeleteMessageAsync(queue.QueueUrl, msg.ReceiptHandle);
    }
}


Console.ReadLine();