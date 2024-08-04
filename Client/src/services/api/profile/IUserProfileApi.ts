interface IUserProfileApi {
    GetUserProfileAsync:(username:string) => Promise<{code: number, profile?: UserProfile}>
    GetCurrentUserProfileAsync:() => Promise<{code: number, profile?: UserProfile}>
    GetCurrentUserNameAsync:() => Promise<{code: number, username?: string}>
    UpdateProfile:(id:string, profile:UserProfile) => Promise<RequesResult>
}