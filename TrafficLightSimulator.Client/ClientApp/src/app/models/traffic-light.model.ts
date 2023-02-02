//export interface TrafficLight {
//  direction: Direction;
//  //trafficLightStatusDict: { [key: number]: boolean };
//  //trafficLightStatusDict: Map<number, boolean>;
//  trafficLightStatusDict: any
//  //statuses: Map<Status, boolean>;
//}

import { Direction } from './direction.enum';

export interface TrafficLight {
  Direction: Direction;
  TrafficLightStatusDict: any
}

//export enum Direction {
//  Northbound,
//  Southbound,
//  Eastbound,
//  Westbound
//}

//export enum Status {
//  Red,
//  Yellow,
//  Green,
//  RightTurn
//}
