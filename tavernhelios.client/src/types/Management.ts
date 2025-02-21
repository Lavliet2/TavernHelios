export interface Dish {
    id: string;
    name: string;
    description: string;
    dishType: number;
    imageBase64?: string;
}
  
export interface Menu {
    id: string;
    name: string;
    dishes: string[];
}

export interface Schedule {
    id: string;
    Menu: Menu;
    dateTime: string;
}