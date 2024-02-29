async function getCurrentProfile(userProfileApi:IUserProfileApi) : Promise<UserProfile> {
    const result = await userProfileApi.GetCurrentUserProfileAsync()
    if(result.code === 200){
        if(result.profile) {
            return result.profile
        } else {
            throw new Error("Error : profile is null")
        }
    } else if(result.code === 401) {
        throw new Error("Error: unauthorized")
    } else {
        throw new Error(`Error : ${result.code}`)
    }
}

async function getProfile(username:string, userProfileApi:IUserProfileApi) : Promise<UserProfile> {
    const result = await userProfileApi.GetUserProfileAsync(username)
    if(result.code === 200) {
        if(result.profile) {
            return result.profile
        } else {
            throw new Error("Profile is null")
        }
    } else {
        throw new Error(`Error: ${result.code}`)
    }
}

export { getCurrentProfile, getProfile }