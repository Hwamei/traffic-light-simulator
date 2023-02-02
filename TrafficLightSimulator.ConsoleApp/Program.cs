using Microsoft.AspNetCore.SignalR.Client;
using System.Text.Json;
using TrafficLightSimulator.Server.Models;
using TrafficLightSimulator.Server.Enums;

Console.WriteLine("Traffic Light Simulator");

var connection = new HubConnectionBuilder()
    .WithUrl("https://localhost:7070/hub")
    .Build();

await connection.StartAsync();
Console.WriteLine(connection.ConnectionId);
Console.WriteLine(connection.State);
Console.WriteLine();

connection.On<string>("Changed", data => {
    var message = JsonSerializer.Deserialize<TrafficLightMessage>(data);
    if (message == null)
    {
        Console.WriteLine("An error occurred");
    }
    var northbound = message.TrafficLights.Single(tl => tl.Direction == DirectionEnum.Northbound);
    var northboundRedLight = northbound.TrafficLightStatusDict[StatusEnum.Red];
    var northboundYellowLight = northbound.TrafficLightStatusDict[StatusEnum.Yellow];
    var northboundGreenLight = northbound.TrafficLightStatusDict[StatusEnum.Green];
    var northboundTurnLight = northbound.TrafficLightStatusDict[StatusEnum.RightTurn];

    var southbound = message.TrafficLights.Single(tl => tl.Direction == DirectionEnum.Southbound);
    var southboundRedLight = southbound.TrafficLightStatusDict[StatusEnum.Red];
    var southboundYellowLight = southbound.TrafficLightStatusDict[StatusEnum.Yellow];
    var southboundGreenLight = southbound.TrafficLightStatusDict[StatusEnum.Green];
    var southboundTurnLight = southbound.TrafficLightStatusDict[StatusEnum.RightTurn];

    var eastbound = message.TrafficLights.Single(tl => tl.Direction == DirectionEnum.Eastbound);
    var eastboundRedLight = eastbound.TrafficLightStatusDict[StatusEnum.Red];
    var eastboundYellowLight = eastbound.TrafficLightStatusDict[StatusEnum.Yellow];
    var eastboundGreenLight = eastbound.TrafficLightStatusDict[StatusEnum.Green];
    var eastboundTurnLight = eastbound.TrafficLightStatusDict[StatusEnum.RightTurn];

    var westbound = message.TrafficLights.Single(tl => tl.Direction == DirectionEnum.Westbound);
    var westboundRedLight = westbound.TrafficLightStatusDict[StatusEnum.Red];
    var westboundYellowLight = westbound.TrafficLightStatusDict[StatusEnum.Yellow];
    var westboundGreenLight = westbound.TrafficLightStatusDict[StatusEnum.Green];
    var westboundTurnLight = westbound.TrafficLightStatusDict[StatusEnum.RightTurn];

    Console.WriteLine(DateTime.Now);
    Console.WriteLine($"North R {GetIndicator(northboundRedLight)} South R {GetIndicator(southboundRedLight)} East R {GetIndicator(eastboundRedLight)} West R {GetIndicator(westboundRedLight)}");
    Console.WriteLine($"North Y {GetIndicator(northboundYellowLight)} South Y {GetIndicator(southboundYellowLight)} East Y {GetIndicator(eastboundYellowLight)} West Y {GetIndicator(westboundYellowLight)}");
    Console.WriteLine($"North G {GetIndicator(northboundGreenLight)} South G {GetIndicator(southboundGreenLight)} East G {GetIndicator(eastboundGreenLight)} West G {GetIndicator(westboundGreenLight)}");
    Console.WriteLine($"North T {GetIndicator(northboundTurnLight)} South T {GetIndicator(southboundTurnLight)} East T {GetIndicator(eastboundTurnLight)} West T {GetIndicator(westboundTurnLight)}");
    Console.WriteLine();
});

Console.ReadLine();



static string GetIndicator(bool value) => value ? "-" : " ";