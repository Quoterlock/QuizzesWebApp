interface IAuthorizationService {
    Login(email:string, password:string): Promise<RequesResult>
    Register(username:string, email:string, password:string): Promise<RequesResult>
    Logout(): Promise<RequesResult>
}