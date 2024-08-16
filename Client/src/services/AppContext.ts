import { createContext } from "react"
import { AuthorizationApi } from "./api/authorization/AuthorizationApi"
import UserProfileApi from "./api/profile/UserProfileApi"
import { QuizApi } from "./api/quiz/QuizApi"
import { RatesApi } from "./rates/RatesApi"

interface ContextType{
    api : IQuizApi
    authorizationApi : IAuthorizationApi
    userProfileApi: IUserProfileApi
    ratesApi : IRatesApi
}

const apiRoute = "http://192.168.0.103:5000/api"

export const AppContext = createContext<ContextType>({
    api: new QuizApi(apiRoute),
    authorizationApi : new AuthorizationApi(apiRoute),
    userProfileApi : new UserProfileApi(apiRoute),
    ratesApi : new RatesApi(apiRoute)
})