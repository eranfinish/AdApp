

export class User {
  id: number;
  name?: string;
  email: string;
  password: string;
  userName: string;
  isAdmin: boolean;
  isRegistering: boolean;
  address: string;
  phone: string;
  //isLogedIn: boolean;
  //mobile?: string;
  //token: string;
 //lastEntrance: Date



  constructor(
    id: number,
    email: string,
    password: string,
    userName: string,
    name:string,
    isAdmin: boolean,
    isRegistering: boolean,
    address: string,
    phone: string,
   //sLogedIn: boolean,
    //name?: string,
   // mobile?: string,
   // token: string = '',
    //lastEntrance: Date = new Date
  ) {
    this.id = id;
    this.email = email;
    this.password = password;
    this.userName = userName;
    this.name = name;
    this.isAdmin = isAdmin;
    this.isRegistering = isRegistering;
    this.address = address;
    this.phone = phone;
   // this.isLogedIn = isLogedIn;
    //this.name = name;
   // this.mobile = mobile;
   // this.token = token;
   // this.lastEntrance = lastEntrance;
  }

}
