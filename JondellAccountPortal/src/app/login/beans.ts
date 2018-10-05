export interface Credentials {
    UserName: string;
    Password: string;
}

export interface LoginResponse {
    token: string;
    invalidLogin: boolean;
  }