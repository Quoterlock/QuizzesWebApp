interface IUserProfileApi {
    GetUserProfileAsync:(username:string) => Promise<{code: number, profile?: UserProfile}>
    GetCurrentUserProfileAsync:() => Promise<{code: number, profile?: UserProfile}>
    UpdateProfile:(id:string, profile:UserProfile) => Promise<RequesResult>
}