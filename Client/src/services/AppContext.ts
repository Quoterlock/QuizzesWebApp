import { createContext } from "react"
import { MockQuizApi } from "./quiz/MockQuizApi"
import { AuthorizationService } from "./authorization/AuthorizationService"
import UserProfileService from "./profile/UserProfileService"
//import { QuizApi } from "./quiz/quiz-api"

interface ContextType{
    api : IQuizApi
    authorizationService : IAuthorizationService
    userProfileService: IUserProfileService
}

export const AppContext = createContext<ContextType>({
    api: new MockQuizApi,
    authorizationService : new AuthorizationService,
    userProfileService : new UserProfileService
})