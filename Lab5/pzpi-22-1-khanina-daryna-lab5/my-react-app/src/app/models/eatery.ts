export interface Eatery {
   id: number;
   name: string;
   address: string;
   type: EateryType;
   openingDay: string;
   operatingHours: string;
   maximumCapacity: number;
   temperatureThreshold: number;
}

export enum EateryType {
   Restaurant = 0,
   Cafe = 1,
   Bar = 2,
   FastFood = 3,
   Bakery = 4,
   FoodTruck = 5,
}

export default interface EateryList {
   values: Eatery[];
}
