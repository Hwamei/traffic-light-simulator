import { Component, OnInit } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { environment } from '../../environments/environment';
import { Direction } from '../models/direction.enum';
import { TrafficLightMessage } from '../models/traffic-light-message.model';
import { TrafficLight } from '../models/traffic-light.model';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  public title = 'Traffic Light Simulator';
  public message: TrafficLightMessage = {
    TrafficLights: [] = []
  };
  public trafficLights: TrafficLight[] = [];
  public current: Date = new Date();

  public get northbound(): TrafficLight { return this.trafficLights.filter(tl => tl.Direction === Direction.Northbound)[0]; }
  public get southbound(): TrafficLight { return this.trafficLights.filter(tl => tl.Direction === Direction.Southbound)[0]; }
  public get eastbound(): TrafficLight { return this.trafficLights.filter(tl => tl.Direction === Direction.Eastbound)[0]; }
  public get westbound(): TrafficLight { return this.trafficLights.filter(tl => tl.Direction === Direction.Westbound)[0]; }

  public ngOnInit(): void {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.baseUrl}hub`)
      .build();


    connection.start().then(() => {
      console.log('signalR connected');
    }).catch((err: any) => {
      console.error(err.toString());
    });

    connection.on("Changed", (data: string) => {
      this.current = new Date();
      if (data) {
        const message = JSON.parse(data) as TrafficLightMessage;
        if (message) {
          this.trafficLights = message.TrafficLights;
        }
      }
    });
  }
}
