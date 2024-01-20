interface IUserProfileService {
    GetUserProfileAsync:(username:string) => Promise<UserProfile>
    GetCurrentUserProfileAsync:() => Promise<UserProfile>
    UpdateProfile:(id:string, profile:UserProfile) => Promise<RequesResult>
}