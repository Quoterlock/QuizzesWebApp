interface IUserProfileService {
    GetUserProfile:(id:string) => Promise<UserProfile|undefined>
    GetCurrentUserProfile: () => Promise<UserProfile|undefined>
    UpdateProfile:(id:string, profile:UserProfile) => Promise<RequesResult>
}