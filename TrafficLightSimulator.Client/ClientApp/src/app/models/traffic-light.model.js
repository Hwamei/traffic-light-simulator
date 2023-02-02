"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Status = exports.Direction = void 0;
var Direction;
(function (Direction) {
    Direction[Direction["Northbound"] = 0] = "Northbound";
    Direction[Direction["Southbound"] = 1] = "Southbound";
    Direction[Direction["Eastbound"] = 2] = "Eastbound";
    Direction[Direction["Westbound"] = 3] = "Westbound";
})(Direction = exports.Direction || (exports.Direction = {}));
var Status;
(function (Status) {
    Status[Status["Red"] = 0] = "Red";
    Status[Status["Yellow"] = 1] = "Yellow";
    Status[Status["Green"] = 2] = "Green";
    Status[Status["RightTurn"] = 3] = "RightTurn";
})(Status = exports.Status || (exports.Status = {}));
//# sourceMappingURL=traffic-light.model.js.map