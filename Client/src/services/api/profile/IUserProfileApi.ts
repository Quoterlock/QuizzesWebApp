interface IUserProfileApi {
    GetUserProfileAsync:(username:string) => Promise<{code: number, profile?: UserProfile}>
    GetCurrentUserProfileAsync:() => Promise<{code: number, profile?: UserProfile}>
    GetCurrentUserProfileInfoAsync:() => Promise<{code: number, profile?: UserProfileInfo}>
    GetCurrentUserNameAsync:() => Promise<{code: number, username?: string}>
    UpdateProfile:(profile:UserProfileInfo) => Promise<RequesResult>
    UpdateProfilePhoto(image:File): Promise<RequesResult>
    GetProfileImagePath(username:string): string
}