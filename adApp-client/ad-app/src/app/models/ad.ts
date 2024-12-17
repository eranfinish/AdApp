export interface Ad {
  id: number;
  title: string;
  description: string;
  category: string;
  createdAt: string;
  ownerId: number;
  imageBase64?: string;
  name:string;
  address: string;
  phone: string;
  price: number;
  currency:number;
}
