interface IAuthorizationService {
    Login(login:string, password:string): Promise<RequesResult>
    Logout(): Promise<RequesResult>
}