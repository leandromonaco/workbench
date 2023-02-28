//docker run --name redisserver -p 6379:6379 -v redisserverdata:/var/opt/redis -d redis:latest --requirepass test
//https://stackexchange.github.io/StackExchange.Redis/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Security;
using System.Security.Authentication;
using System.Threading.Tasks;
using StackExchange.Redis;



#region   Section 1 - Introduction to StackExchange.Redis

#region 1.2 Getting Started

#region Connect to Redis with Connection String
var connectionString = "localhost:6379,password=test";
var connectionMultiplexer1 = await ConnectionMultiplexer.ConnectAsync(connectionString);
var database1 = connectionMultiplexer1.GetDatabase();
var res = database1.Ping();
Console.WriteLine($"The ping took: {res.TotalMilliseconds} ms");
#endregion

#region Connect to Redis with ConfigurationOptions
var options = new ConfigurationOptions
{
    EndPoints = new EndPointCollection { "localhost:6379" },
    Password = "test",
};

var connectionMultiplexer2 = await ConnectionMultiplexer.ConnectAsync(options);
var database2 = connectionMultiplexer2.GetDatabase();
var res2 = database2.Ping();
Console.WriteLine($"The ping took: {res2.TotalMilliseconds} ms");
#endregion

#endregion

#region 1.3 The Interfaces of StackExchange.Redis
#region IConnectionMultiplexer

/* The IConnectionMultiplexer is responsible for maintaining all of the connections to Redis. As I described in the previous section. It routes all the commands to Redis through a single connection for interactive commands, and a separate connection for subscription, which we'll discuss more in depth later.

The IConnectionMultiplexer is responsible for exposing a simple interface to get other critical interfaces of the library. Including the IDatabase, ISubscriber, and IServer. */

#endregion

#region IDatabase
/*
  The IDatabase can be thought of as the primary interactive interface to Redis. It provides a single interface for your entire Redis Instance, and is the preferred interface when you are executing single commands that manipulate your application's data to Redis.

The IDatabase, unlike the IServer, abstracts the particulars of your Redis deployments away. Consequentially, if you are running in a cluster and are preforming a write, the IDatabase does not require you to know which server in particular you need to write to. Also, if you have many replicas per master shard in your Cluster or Sentinel Redis deployments, the IDatabase will leverage the ConnectionMultiplexer to automatically distribute your reads across your deployment. 
 */

#endregion

#region IServer
/*
    The IServer is an abstraction to a single instance of a Redis Server. You can grab an instance of an IServer by using the IConnectionMultiplexer.GetServer command, passing in the exact endpoint information you want to retrieve.

    IServer has a fundamentally different role than IDatabase as you're going to use it to handle the server level commands. That means that in general, data modeling commands are not appropriate to be used on a server. Rather operations like checking the server's info (the basic info of Redis), it's configuration, updating it's configuration, checking memory statistics, and the like are IServer operations. Even scanning for the keys of a Redis server should be done at the server level. 
 */

var server = connectionMultiplexer1.GetServer("localhost", 6379);
var pingResult = server.Execute("PING");

#endregion

#region ISubscriber

/* The ISubscriber is the interface responsible for maintaining subscriptions to Redis in the pub/sub interface. Unlike the other interfaces we've looked at thus far, the subscriber does not leverage the interactive connection.

The Multiplexer explicitly opens a separate connection for subscriptions because when you subscribe to any channel on a client in Redis, the client connection converts to subscription mode. This limits the connection to only use commands that implement subscriber functionality.

True to it's name however, the Multiplexer continues to maintain a single connection per server, and all subscriptions are handled on that single connection. */

var subscriber = connectionMultiplexer1.GetSubscriber();


#endregion

#region ITransaction
//https://redis.io/docs/manual/transactions/
/*The ITransaction interface is fundamentally an async interface. It exposes a very similar command set to the IDatabase, 
 * but it will only expose async versions of each command. That is because each command in ITransaction is async, 
 * as they will not be competed until after Execute is called. Only after the Execute is called can the underpinning tasks for 
 * the Transaction be awaited. */

var transaction = database1.CreateTransaction();
//await transaction.StringSetAsync("Key1", "Value1");
//await transaction.StringSetAsync("Key2", "Value2");
//await transaction.ExecuteAsync();
#endregion

#endregion

#region 1.4 Advanced Connections

#region Connect to Redis with TLS
//

/*
  https://learn.microsoft.com/en-us/dotnet/api/system.net.security.sslclientauthenticationoptions?view=net-7.0

  In .NET Core (3.1, 5, 6, 7+) SslClientAuthenticationOptions was added as an optional way of configuring an SslStream. This provides a great deal of extra flexibility when it comes to configuring TLS settings Redis.

  Using the SslClientAuthenticationOptions delegate in ConfigurationOptions you can configure:

    Allowed SSL/TLS protocols
    TLS/SSL Cipher Suites allowed in the cipher negotiation
    Certificate selection delegate
    Certificate validation delegate

 */

var optionsssl = new ConfigurationOptions
{
    EndPoints = new EndPointCollection { "sslredisInstance:6379" },
    SslClientAuthenticationOptions = new Func<string, SslClientAuthenticationOptions>(
    hostName => new SslClientAuthenticationOptions
    {
        EnabledSslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13,

    })
};
#endregion

#region Connect to Redis Cluster
/*
  Connecting to an OSS Cluster instance is the same as connecting to a standalone Redis Instance. 
  The only exception is that when you are listing endpoints, you'll want to use more than one. 
  This is so that if the endpoint you are trying to reach has failed over, the Multiplexer still has a chance
  to connect to the other master instances. 
 */

var optionscluster = new ConfigurationOptions
{
    // add and update parameters as needed
    EndPoints = new EndPointCollection { "redis-1:6379", "redis-2:6379", "redis-3:6379" }
};

#endregion

#region Connect to Redis Sentinel

/*
  https://redis.io/docs/management/sentinel/
  The key difference is that rather than connecting to the master server, you connect to one of the 'sentinels' 
  the instances of Redis responsible for monitoring your master and replicas, detecting fail-overs, and promoting new masters. 
  Additionally, you must specify the ServiceName, which corresponds to the master name that you tell the sentinels to monitor when configuring them. 
 */

var optionsSentinel = new ConfigurationOptions
{
    EndPoints = new EndPointCollection { "sentinel-1:26379" },
    ServiceName = "sentinel"
};

#endregion

#endregion

#region 1.5 Pipelining

/*Pipelining

    Pipelining is a critically important concept for maximizing throughput to Redis. When you need to execute multiple commands against Redis, and the intermediate results can be temporarily ignored, pipelining can drastically reduce the number of round trips required to Redis, which can drastically increase performance, as many operations are hundreds of times faster than the Round Trip Time (RTT).

    With StackExchange.Redis, there are two ways to pipeline commands, either implicitly with the Async API, and explicitly with the IBatch API.

Implicit Pipelining with Async API

    If you use the async version of a command, the command will be automatically pipelined to Redis. If you use await to await the result of a task dispatched by one of these async commands, it will wait until the command is complete before returning control. However, if you group a set of tasks dispatched by the async methods together and await them all in one shot, the ConnectionMultiplexer will automatically find an efficient way to pipeline the commands to Redis, so that you can cut down the number of round trips significantly.

Explicit Pipelining with IBatch

    You can also be much more explicit about pipelining commands. The IBatch API only provides the async interface for commands. You can set up however many commands you want to pipeline with those async methods, preserving the tasks as they will provide you the results.

    When you have all the commands that you want pipelined dispatched, you can call Execute to run them all. This will pipeline all of your commands in one contiguous block to Redis. Using this method, no other commands will be interleaved with your batched commands from the client. However, if there are other clients sending commands to Redis, it's possible that their commands will be interleaved with the batched commands. */

#region Un-Pipelined

var stopwatch = Stopwatch.StartNew();
// un-pipelined commands incur the added cost of an extra round trip
for (var i = 0; i < 1000; i++)
{
    await database1.PingAsync();
}

Console.WriteLine($"1000 un-pipelined commands took: {stopwatch.ElapsedMilliseconds} ms to execute");


#endregion

#region Implicitly Pipelined

// If we run out async tasks to StackExchange.Redis concurrently, the library
// will automatically manage pipelining of these commands to Redis, making
// them significantly more performant as we remove most of the round trips to Redis.
var pingTasks = new List<Task<TimeSpan>>();

// restart stopwatch
stopwatch.Restart();

for (var i = 0; i < 1000; i++)
{
    pingTasks.Add(database1.PingAsync());
}

await Task.WhenAll(pingTasks);

Console.WriteLine($"1000 automatically pipelined tasks took: {stopwatch.ElapsedMilliseconds}ms to execute, first result: {pingTasks[0].Result}");

#endregion

#region Explicit Pipelining with IBatch

/*
    Finally, let's explicitly pipeline all of our pings using an IBatch. An IBatch will guarantee that the client sends the entire 
    batch to Redis in one shot, with no other commands interleaved in the pipeline. This is slightly different behavior than our 
    implicit pipelining as in the case of implicit pipelining, commands may be interleaved with any other commands the client was 
    executing at the time.

    To explicitly pipeline these commands we'll follow a similar pattern, in this case, however, we will use the 
    IDatabase.CreateBatch() method to create the batch, and use the batch's async methods to 'dispatch' the the tasks. 
    It's important to note here that unlike in our implicit case, the tasks will not be truly dispatched until after the 
    IBatch.Execute() method is called, if you try awaiting any of the tasks before then, you can accidentally deadlock your command. After calling Execute, you can then await all of the tasks. 
 
 */

// clear our ping tasks list.
pingTasks.Clear();

// Batches allow you to more intentionally group together the commands that you want to send to Redis.
// If you employee a batch, all commands in the batch will be sent to Redis in one contiguous block, with no
// other commands from the client interleaved. Of course, if there are other clients to Redis, commands from those
// other clients may be interleaved with your batched commands.
var batch = database1.CreateBatch();

// restart stopwatch
stopwatch.Restart();

for (var i = 0; i < 1000; i++)
{
    pingTasks.Add(batch.PingAsync());
}

batch.Execute();
await Task.WhenAll(pingTasks);
Console.WriteLine($"1000 batched commands took: {stopwatch.ElapsedMilliseconds}ms to execute, first result: {pingTasks[0].Result}");

#endregion

#endregion

#endregion



#region Section 2 - Using Redis Data Structures with .NET

#region 2.1 - Redis Strings
/*
  Redis Strings are the simplest of the Redis Data Structures. They map a single Redis Key to a single Redis Value. 
  In spite of their simplicity they have a variety of capabilities that make them useful across a number of use cases. 
*/

//Text
var instructorNameKey = new RedisKey("instructors:1:name");
database1.StringSet(instructorNameKey, "Name");
database1.StringAppend(instructorNameKey, " Appended");
var instructor1Name = database1.StringGet(instructorNameKey);
Console.WriteLine($"Instructor 1's name is: {instructor1Name}");

//Numeric
var tempKey = "temperature";
database1.StringSet(tempKey, 42);
database1.StringIncrement(tempKey, 5);
database1.StringIncrement(tempKey, .5);
var temperature = database1.StringGet(tempKey);
Console.WriteLine($"New temperature: {temperature}");

/*
 Expiration

 You can set an expiration on a string when you set it by using the expiry option. The expiry is a TimeSpan, 
 there are a variety of ways you can create them. In this instance we'll just create a super short lived key by using the 
 FromSeconds method to initialize a key that will live for 1 second. 
 */

database1.StringSet("temporaryKey", "hello world", expiry: TimeSpan.FromSeconds(1));

/*
 Conditional Set

    You can also make your StringSet conditional using the when option. The when option uses the When enum, and has three possible values.

    Always: This is the default, always set the key.
    Exists: Set only if the key exists
    NotExists: Set only if the key does not exist.

 */

var conditionalKey = "ConditionalKey";
var conditionalKeyText = "this has been set";
// You can also specify a condition for when you want to set a key
// For example, if you only want to set a key when it does not exist
// you can by specifying the NotExists condition
var wasSet = database1.StringSet(conditionalKey, conditionalKeyText, when: When.NotExists);
Console.WriteLine($"Key set: {wasSet}");

// Of course, after the key has been set, if you try to set the key again
// it will not work, and you will get false back from StringSet
wasSet = database1.StringSet(conditionalKey, "this text doesn't matter since it won't be set", when: When.NotExists);
Console.WriteLine($"Key set: {wasSet}");

// You can also use When.Exists, to set the key only if the key already exists
wasSet = database1.StringSet(conditionalKey, "we reset the key!");
Console.WriteLine($"Key set: {wasSet}");


#endregion

#region 2.2 - Redis Lists

/*
 Redis Lists are doubly linked lists that allow you to push and pop from the front and tail. 
 This allows you to build what are in essence queues and stacks of Redis Strings. 
 The StackExchange.Redis library provides an intuitive interface for working with Lists in the IDatabase that 
 we will go over in this section. 
*/

var fruitKey = "fruits";
var vegetableKey = "vegetables";
database1.KeyDelete(new RedisKey[] { fruitKey, vegetableKey });

database1.ListLeftPush(fruitKey, new RedisValue[] { "Banana", "Mango", "Apple", "Pepper", "Kiwi", "Grape" });
Console.WriteLine($"The first fruit in the list is: {database1.ListGetByIndex(fruitKey, 0)}");

database1.ListRightPush(vegetableKey, new RedisValue[] { "Potato", "Carrot", "Asparagus", "Beet", "Garlic", "Tomato" });
Console.WriteLine($"The first vegetable in the list is: {database1.ListGetByIndex(vegetableKey, 0)}");

/*
 Enumerate a List

  To enumerate a list, you can use the ListRange method. If you pass in a start index, the range will start from there, 
  and if you pass in a stop index, it will stop there, if you pass in neither a start nor a stop, it will pull back the whole list. 
*/

Console.WriteLine($"Fruit indexes 0 to -1: {string.Join(", ", database1.ListRange(fruitKey))}");
Console.WriteLine($"Vegetables index 0 to -2: {string.Join(", ", database1.ListRange(vegetableKey, 0, -2))}");

/*
    Move elements Between Lists

    You can also transfer elements between lists using the ListMove method This accepts two list keys, 
    as well as the source side and destination side. These are the sides of the list you will pop from and push to 
    respectively. Let's transfer "Tomato" from our vegetable list to our fruit list, since it's in fact a fruit and not a 
    vegetable. 
 */

Console.WriteLine($"Fruit before the move: {string.Join(", ", database1.ListRange(fruitKey))}");
Console.WriteLine($"Vegetables before the move: {string.Join(", ", database1.ListRange(vegetableKey))}");
database1.ListMove(vegetableKey, fruitKey, ListSide.Right, ListSide.Right);
Console.WriteLine($"Fruit after the move: {string.Join(", ", database1.ListRange(fruitKey))}");
Console.WriteLine($"Vegetables after the move: {string.Join(", ", database1.ListRange(vegetableKey))}");

/*
 List as a Queue

 You can user a Redis List as a defacto FIFO Queue by pushing and popping from different sides. 
 Conventionally you'd push left, pop right: 
 */

Console.WriteLine($"Vegetables FIFO List: {string.Join(", ", database1.ListRange(vegetableKey))}");
database1.ListLeftPush(vegetableKey, "Item 1");
Console.WriteLine($"Queuing Item 1: {string.Join(", ", database1.ListRange(vegetableKey))}");
Console.WriteLine($"Dequeued: {database1.ListRightPop(vegetableKey)}");
Console.WriteLine($"Dequeued: {database1.ListRightPop(vegetableKey)}");
Console.WriteLine($"Dequeued: {database1.ListRightPop(vegetableKey)}");
Console.WriteLine($"Dequeued: {database1.ListRightPop(vegetableKey)}");
Console.WriteLine($"Dequeued: {database1.ListRightPop(vegetableKey)}");
Console.WriteLine($"Dequeued: {database1.ListRightPop(vegetableKey)}");
Console.WriteLine($"Dequeued: {database1.ListRightPop(vegetableKey)}");
Console.WriteLine($"Dequeued: {database1.ListRightPop(vegetableKey)}");

/*
 List as a Stack

You can also get your lists to act as LIFO stacks, by pushing and popping from the same, by convention you'd typically use the left side.

 */


Console.WriteLine("Pushing Grapefruit");
database1.ListLeftPush(fruitKey, "Grapefruit");
Console.WriteLine($"Popping Fruit: {string.Join(",", database1.ListLeftPop(fruitKey, 2))}");

/*
 Searching Lists

 Redis Lists also allow you to find the index of a particular item using the ListPosition method.
 */

Console.WriteLine($"Position of Mango: {database1.ListPosition(fruitKey, "Mango")}");

/*
 List Size
 And finally, you use the ListLength method to determine the size of a given list.
*/

Console.WriteLine($"There are {database1.ListLength(fruitKey)} fruits in our Fruit List");

#endregion



#region  2.3 - Redis Sets

//Sets do not allow duplication, but they allow very rapid O(1) membership checks

var allPlayers = "players";
var playersTeamA = "players:teamA";
var playersTeamB = "players:teamB";

database1.KeyDelete(new RedisKey[] { allPlayers, playersTeamA, playersTeamB });

database1.SetAdd(playersTeamA, new RedisValue[] { "PlayerTeamA:1", "PlayerTeamA:2", "PlayerTeamA:2", "PlayerTeamA:2" });
database1.SetAdd(playersTeamA, new RedisValue[] { "PlayerTeamA:3" });
database1.SetAdd(playersTeamA, new RedisValue[] { "PlayerTeamA:4" });
database1.SetRemove(playersTeamA, new RedisValue[] { "PlayerTeamA:3" });
database1.SetAdd(playersTeamB, new RedisValue[] { "PlayerTeamB:1", "PlayerTeamB:2" });


database1.SetCombineAndStore(SetOperation.Union, allPlayers, new RedisKey[] { playersTeamA, playersTeamB });

var playerTeamA1 = database1.SetContains(playersTeamA, "PlayerTeamA:1");
Console.WriteLine($"Player Team A: {playerTeamA1}");

playerTeamA1 = database1.SetContains(playersTeamB, "PlayerTeamA:1");
Console.WriteLine($"Player Team A: {playerTeamA1}");

/*Enumerate Entire Set

If you want to guarantee that you are enumerating the entire set in one round trip, you can do so by using the SetMembers method. 
This will use the SMEMBERS command in Redis. If your set is relatively compact (under 1000 members), this is a perfectly valid way 
to pull back all of your set members. */

Console.WriteLine($"All Users In one shot: {string.Join(", ", database1.SetMembers(allPlayers))}");

/*
 Enumerate Set in Chunks

The alternate way to enumerate a set is to enumerate it with SetScan, which will create a Set Enumerator, and use the SSCAN 
command to scan over the entire set until the set is exhausted. 
 
 */

Console.WriteLine($"All Users with scan  : {string.Join(", ", database1.SetScan(allPlayers))}");

//Move Elements Between Sets

Console.WriteLine("Transferring PlayerTeamA:1 to Team B");
var moved = database1.SetMove(playersTeamA, playersTeamB, "PlayerTeamA:1");
Console.WriteLine($"Move Successful: {moved}");

Console.WriteLine("Transferring PlayerTeamA:3 to Team B");
moved = database1.SetMove(playersTeamA, playersTeamB, "PlayerTeamA:3");
Console.WriteLine($"Move Successful: {moved}");

#endregion

#region  2.4 - Redis Sorted Sets

/*
 Sorted Sets

 Sorted Sets are similar conceptually to sets, however unlike regular sets, sorted sets store and retrieve members in order
 using the member's score
 */

var userAgeSet = "users:age";
database1.SortedSetAdd(userAgeSet,
       new SortedSetEntry[]
       {
            new("Maria", 20),
            new("Leandro", 23),
            new("Pepe", 18),
            new("Ana", 35),
            new("Julia", 55),
            new("Nick", 62)
       });

//To check a score of a member, simply use SortedSetScore passing in the key name and the user id. 
var user3Score = database1.SortedSetScore(userAgeSet, "User:3");
Console.WriteLine($"User:3 Score: {user3Score}");

/*To check a user's rank, you can use the SortedSetRank method. With this method you can pass in the key, the User Id, 
 * and a direction that you want the sorting to be done in. In this case, when we say "Rank" we mean we want the high score 
 * coming up first. Hence we'll use the Descending direction. 
*/

var user2Rank = database1.SortedSetRank(userAgeSet, "User:2", Order.Descending);
Console.WriteLine($"User:2 Rank: {user2Rank}");

/* 
  Probably the most popular way to query members from a stream is by using the "Range" methods. There are three ways that you can
  run ranges on a sorted set.
*/

//By Rank
var topOldestPeople = database1.SortedSetRangeByRank(userAgeSet, 0, 2, Order.Descending);
Console.WriteLine($"Top 3 Oldest People: {string.Join(", ", topOldestPeople)}");

//By Score
var eighteenToThirty = database1.SortedSetRangeByScoreWithScores(userAgeSet, 18, 30, Exclude.None);
Console.WriteLine($"Users between 18 and 30: {string.Join(", ", eighteenToThirty)}");

//By Lexicographic (REVIEW)
var namesAlphabetized = database1.SortedSetRangeByValue(userAgeSet);
Console.WriteLine($"Names Alphabetized: {string.Join(",", namesAlphabetized)}");

var namesBetweenAandJ = database1.SortedSetRangeByValue(userAgeSet, "A", "K", Exclude.Stop);
Console.WriteLine($"Names between A and J: {string.Join(", ", namesBetweenAandJ)}");

//database1.SortedSetRangeAndStore(userLastAccessSet, mostRecentlyActive, 0, 2, order: Order.Descending);
//var rankOrderMostRecentlyActive = database1.SortedSetCombineWithScores(SetOperation.Intersect, new RedisKey[] { userHighScoreSet, mostRecentlyActive }, new double[] { 1, 0 }).Reverse();
//Console.WriteLine($"Highest Scores Most Recently Active: {string.Join(", ", rankOrderMostRecentlyActive)}");

#endregion

#region  2.5 - Redis Hashes

/*
 Hashes are a simple, but very powerful data structure available within Redis. Hashes are similar to dictionaries in that they 
 allow you to organize a set of key-value pairs at a particular key within Redis. 
 */

var person1 = "person:1";
var person2 = "person:2";
var person3 = "person:3";

database1.KeyDelete(new RedisKey[] { person1, person2, person3 });

database1.HashSet(person1, new HashEntry[]
   {
        new("name","Alice"),
        new("age", 33),
        new("email","alice@example.com")
   });

database1.HashSet(person2, new HashEntry[]
{
        new("name","Bob"),
        new("age", 27),
        new("email","robert@example.com")
});

database1.HashSet(person3, new HashEntry[]
{
        new("name","Charlie"),
        new("age", 50),
        new("email","chuck@example.com")
});

//Increment a Field in a Hash
var newAge = database1.HashIncrement(person3, "age");
Console.WriteLine($"person:3 new age: {newAge}");

//Retrieve a Field from a Hash
var person1Name = database1.HashGet(person1, "name");
Console.WriteLine($"person:1 name: {person1Name}");

//Get All Fields From a Hash
var person2Fields = database1.HashGetAll(person2);
Console.WriteLine($"person:2 fields: {string.Join(", ", person2Fields)}");

var person3Fields = database1.HashScan(person3);
Console.WriteLine($"person:3 fields: {string.Join(", ", person3Fields)}");

#endregion


#region  2.6 - Redis Streams
/*
 Redis Streams are an append-only log like data structure that allows you to enqueue messages from producers to be consumed by 
 consumers in your application

  Stream Limitations in StackExchange.Redis

  Due to the multiplexed nature of StackExchange.Redis, it's important to note at the top that there is no mechanism for using 
  the blocking paradigms available within the stream reading operations.

  This means that the Stream Read operations, StreamRead & StreamReadGroup, will not be able to use the XREAD and XREADGROUP block 
  timer or the special $ id to read only new messages. 
 */

var sensor1 = "sensor:1";
var sensor2 = "sensor:2";

database1.KeyDelete(new RedisKey[] { sensor1, sensor2 });

#endregion

#region  2.7 - Lua Scripting
#endregion

#region  2.8 - Redis Transactions
#endregion

#region  2.9 - Redis Pub/Sub
#endregion



#endregion