interface IUserProfileService {
    GetUserProfile:(id:string) => Promise<UserProfile>
    GetCurrentUserProfile: () => Promise<UserProfile>
    UpdateProfile:(id:string, profile:UserProfile) => Promise<RequesResult>
}