# Overview
Traffic Light Simulator implementation with C# based on .Net 6, SignalR and Angular

- **TrafficLightSimulator.Server**
The poject contains the code for the server

- **TrafficLightSimulator.Client**
The project is an Angular Client

- **TrafficLightSimulator.ConsoleApp**
The project is a .Net Client

- **TrafficLightSimulator.Tests**
The project contains the tests for the server

# Getting Started
1. Build the solution
2. Start TrafficLightSimulator.Server
3. Run the Angular client.
        Open command prompt and navigate to ~\TrafficLightSimulator\TrafficLightSimulator.Client\ClientApp
        Run command npm i
        Run command npm start
        Angular client should be started on https://localhost:44431 (Cors is configured on server side please ensure ports match)
4. Run the .Net client
5. Run the tests


# Extra
Northbound right turn functionality has been implemented. To enable the feature, please set "IsNorthboundRightTurnEnabled" flag to true in appsettings.json in TrafficLightSimulator.Server project


# Requirements & Functionalities

In this test we would like you to implement a traffic light system. We are required to have 4 sets of lights, as follows.

    Lights 1: Traffic is travelling south
    Lights 2: Traffic is travelling west
    Lights 3: Traffic is travelling north
    Lights 4: Traffic is travelling east

- The lights in which traffic is travelling on the same axis can be green at the same time. 

- During normal hours all lights stay green for 20 seconds, but during peak times north and south lights are green for 40 seconds while west and east are green for 10 seconds. 

- Peak hours are 0800 to 1000 and 1700 to 1900. 

- Yellow lights are shown for 5 seconds before red lights are shown. Red lights stay on until the cross-traffic is red for at least 4 seconds, once a red light goes off then the green is shown for the required time(eg the sequence is reset).

- Bonus: At this intersection north bound traffic has a green right-turn signal, which stops the south bound traffic and allows north bound traffic to turn right. This is green at the end of north/south green light and stays green for 10 seconds. During this time north bound is green, north right-turn is green and all other lights are red.


Implementation/Outcomes:

1. Implement a front-end and backend (you can use ????dotnet new???? templates of your choice)

2. The backend will contain the logic and state of the running traffic lights. The front-end will be a visual representation of the traffic lights, with the data served from the backend.

3. There????s no need to have a perfect design on the front end, something simple and functional is fine (unless this is an area of strength you would like to show off). Noting* we will review the client side code.

4. There????s no need to implement entity framework (or similar) to store the data in a database, a in-memory store is fine

5. Code needs to follow architecture & best practices for enterprise grade systems

Note: Code will be evaluated not just for function, but on the quality of the code. Suggested submit within 24 hours.
