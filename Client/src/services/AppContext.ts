import { createContext } from "react"
import { AuthorizationApi } from "./api/authorization/AuthorizationApi"
import UserProfileApi from "./api/profile/UserProfileApi"
import { QuizApi } from "./api/quiz/QuizApi"

interface ContextType{
    api : IQuizApi
    authorizationApi : IAuthorizationApi
    userProfileApi: IUserProfileApi
}

const apiRoute = "https://localhost:44398/api"

export const AppContext = createContext<ContextType>({
    api: new QuizApi(apiRoute),
    authorizationApi : new AuthorizationApi(apiRoute),
    userProfileApi : new UserProfileApi(apiRoute)
})