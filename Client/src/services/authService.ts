import { IsEmailValid, IsPasswordValid } from "../utils/UserCredentialsCheck";


async function loginUser(email:string, password:string, authorizationApi:IAuthorizationApi, userProfileApi:IUserProfileApi) {    
    if(!IsEmailValid(email)) {
        throw new Error("Invalid email address!")
    }
    
    const result = await authorizationApi.Login(email, password)
    console.log(result);
    if(result.code === 200) {
        const profileResult = await userProfileApi.GetCurrentUserProfileAsync()
        if(profileResult.profile != null){
            localStorage.setItem("current-username", profileResult.profile.owner.username)
            localStorage.setItem("current-user-id", profileResult.profile.owner.id)
            return { success:true }
        }
        return { success:false, message: "Can't find user profile"}
    } else {
        return { success:false, message: result.message }
    }
}

async function registerUser(username:string, email:string, password:string, authorizationApi:IAuthorizationApi) {    
    if(!IsEmailValid(email)){
        throw new Error("Invalid email address!")
    }
    
    if(!IsPasswordValid(password)){
        throw new Error("Password must contain:\n- At least one lowercase letter.\n- At least one uppercase letter.\n- At least one digit.\n- At least one non-alphanumeric character.\n- Minimum length of 8 characters.\n")
    }

    const result = await authorizationApi.Register(username, email, password)
    if(result.code === 200) {
        return { success: true }
    } else {
        return { success: false, message: result.message }
    }
}

function logoutUser() {
    localStorage.removeItem("jwt-token")
    localStorage.removeItem("current-username")
    localStorage.removeItem("current-user-id")
}

export {loginUser, registerUser, logoutUser}